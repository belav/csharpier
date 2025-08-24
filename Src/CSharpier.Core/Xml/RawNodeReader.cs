using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace CSharpier.Core.Xml;

internal
#if !NETSTANDARD2_0
partial
#endif
class RawNodeReader
{
    private readonly string originalXml;
    private readonly string lineEnding;
    private int position;
    private readonly Stack<RawNode> elementStack = new();
    private readonly List<RawNode> rootNodes = [];

#if !NETSTANDARD2_0
    [GeneratedRegex(@"\r\n|\n|\r", RegexOptions.Compiled)]
    private static partial Regex NewlineRegex();
#else
    private static readonly Regex NewlineRegex = new(@"\r\n|\n|\r", RegexOptions.Compiled);
#endif

    private RawNodeReader(string xml, string lineEnding)
    {
        var x = 1;
        this.originalXml = NewlineRegex
#if !NETSTANDARD2_0
            ()
#endif
        .Replace(xml, "\n");
        this.lineEnding = lineEnding;
    }

    public static List<RawNode> ReadAll(string originalXml, string lineEnding)
    {
        var reader = new RawNodeReader(originalXml, lineEnding);
        return reader.ReadAll();
    }

    private List<RawNode> ReadAll()
    {
        while (this.position < this.originalXml.Length)
        {
            this.SkipWhitespace();
            if (this.position >= this.originalXml.Length)
            {
                break;
            }

            if (this.originalXml[this.position] == '<')
            {
                this.ParseTag();
            }
            else
            {
                if (this.rootNodes.Count == 0)
                {
                    throw new XmlException("There do not appear to be any root nodes");
                }
                this.ParseText();
            }
        }

        return this.rootNodes;
    }

    private void SkipWhitespace()
    {
        while (
            this.position < this.originalXml.Length
            && char.IsWhiteSpace(this.originalXml[this.position])
        )
        {
            this.position++;
        }
    }

    private void ParseTag()
    {
        if (this.position + 1 >= this.originalXml.Length)
        {
            return;
        }

        if (this.originalXml[this.position + 1] == '!')
        {
            if (
                this.position + 4 < this.originalXml.Length
                && this.originalXml.Substring(this.position, 4) == "<!--"
            )
            {
                this.ParseComment();
            }
            else if (
                this.position + 9 < this.originalXml.Length
                && this.originalXml.Substring(this.position, 9) == "<![CDATA["
            )
            {
                this.ParseCData();
            }
            else if (
                this.position + 9 < this.originalXml.Length
                && this.originalXml.Substring(this.position, 9) == "<!DOCTYPE"
            )
            {
                this.ParseDocType();
            }
        }
        else if (this.originalXml[this.position + 1] == '?')
        {
            this.ParseProcessingInstruction();
        }
        else if (this.originalXml[this.position + 1] == '/')
        {
            this.ParseEndElement();
        }
        else
        {
            this.ParseStartElement();
        }
    }

    private void ParseComment()
    {
        this.position += 4; // Skip "<!--"

        var content = new StringBuilder();
        while (this.position + 2 < this.originalXml.Length)
        {
            if (this.originalXml.Substring(this.position, 3) == "-->")
            {
                this.position += 3;
                break;
            }

            if (this.originalXml[this.position] == '\n')
            {
                content.Append(this.lineEnding);
            }
            else
            {
                content.Append(this.originalXml[this.position]);
            }
            this.position++;
        }

        var node = new RawNode { NodeType = XmlNodeType.Comment, Value = $"<!--{content}-->" };

        this.AddNode(node);
    }

    private void ParseCData()
    {
        this.position += 9; // Skip "<![CDATA["

        var content = new StringBuilder();
        while (this.position + 2 < this.originalXml.Length)
        {
            if (this.originalXml.Substring(this.position, 3) == "]]>")
            {
                this.position += 3;
                break;
            }

            if (this.originalXml[this.position] == '\n')
            {
                content.Append(this.lineEnding);
            }
            else
            {
                content.Append(this.originalXml[this.position]);
            }
            this.position++;
        }

        var node = new RawNode { NodeType = XmlNodeType.CDATA, Value = $"<![CDATA[{content}]]>" };

        this.AddNode(node);
    }

    private void ParseProcessingInstruction()
    {
        this.position += 2; // Skip "<?"

        var name = this.ReadName();
        this.SkipWhitespace();

        var content = new StringBuilder();
        while (this.position + 1 < this.originalXml.Length)
        {
            if (this.originalXml.Substring(this.position, 2) == "?>")
            {
                this.position += 2;
                break;
            }

            if (this.originalXml[this.position] == '\n')
            {
                content.Append(this.lineEnding);
            }
            else
            {
                content.Append(this.originalXml[this.position]);
            }
            this.position++;
        }

        var node = new RawNode
        {
            Name = name,
            NodeType = XmlNodeType.ProcessingInstruction,
            Value = $"<?{name} {content}?>",
        };

        this.AddNode(node);
    }

    private void ParseEndElement()
    {
        this.position += 2; // Skip "</"

        this.ReadName();
        this.SkipToChar('>');

        if (this.elementStack.Count > 0)
        {
            var element = this.elementStack.Pop();
            for (var x = 0; x < element.Nodes.Count; x++)
            {
                if (x > 0)
                {
                    element.Nodes[x - 1].NextNode = element.Nodes[x];
                    element.Nodes[x].PreviousNode = element.Nodes[x - 1];
                }

                if (x == element.Nodes.Count - 2)
                {
                    element.Nodes[x].NextNode = element.Nodes[x + 1];
                    element.Nodes[x + 1].PreviousNode = element.Nodes[x];
                }
            }
        }
    }

    private void ParseStartElement()
    {
        this.position++; // Skip "<"

        var name = this.ReadName();
        var attributes = this.ParseAttributes();

        var isEmpty = false;
        if (this.position < this.originalXml.Length && this.originalXml[this.position] == '/')
        {
            isEmpty = true;
            this.position++;
        }

        this.SkipToChar('>');

        var node = new RawNode
        {
            Name = name,
            NodeType = XmlNodeType.Element,
            IsEmpty = isEmpty,
            Attributes = attributes.ToArray(),
        };

        this.AddNode(node);

        if (!isEmpty)
        {
            this.elementStack.Push(node);
        }
    }

    private void ParseText()
    {
        var content = new StringBuilder();
        // we skip all whitespace in the main read, so for parsing text go backwards until we find the actual start
        while (this.position >= 0 && this.originalXml[this.position - 1] != '>')
        {
            this.position--;
        }

        while (this.position < this.originalXml.Length && this.originalXml[this.position] != '<')
        {
            if (this.originalXml[this.position] == '\n')
            {
                content.Append(this.lineEnding);
            }
            else
            {
                content.Append(this.originalXml[this.position]);
            }
            this.position++;
        }

        var text = content.ToString();
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        var node = new RawNode { NodeType = XmlNodeType.Text, Value = text };

        this.AddNode(node);
    }

    private List<RawAttribute> ParseAttributes()
    {
        var attributes = new List<RawAttribute>();

        while (this.position < this.originalXml.Length)
        {
            this.SkipWhitespace();
            if (
                this.position >= this.originalXml.Length
                || this.originalXml[this.position] == '>'
                || this.originalXml[this.position] == '/'
            )
            {
                break;
            }

            var attrName = this.ReadName();
            this.SkipWhitespace();

            if (this.position < this.originalXml.Length && this.originalXml[this.position] == '=')
            {
                this.position++;
                this.SkipWhitespace();

                var attrValue = this.ReadQuotedValue();
                attributes.Add(
                    new RawAttribute { Name = attrName, Value = attrValue.Replace("\"", "&quot;") }
                );
            }
        }

        return attributes;
    }

    private string ReadName()
    {
        var name = new StringBuilder();
        while (
            this.position < this.originalXml.Length
            && (
                char.IsLetterOrDigit(this.originalXml[this.position])
                || this.originalXml[this.position] == '_'
                || this.originalXml[this.position] == ':'
                || this.originalXml[this.position] == '-'
                || this.originalXml[this.position] == '.'
            )
        )
        {
            name.Append(this.originalXml[this.position]);
            this.position++;
        }
        return name.ToString();
    }

    private string ReadQuotedValue()
    {
        if (this.position >= this.originalXml.Length)
        {
            return string.Empty;
        }

        var quote = this.originalXml[this.position];
        if (quote is not ('"' or '\''))
        {
            return string.Empty;
        }

        this.position++; // Skip opening quote

        var value = new StringBuilder();
        while (this.position < this.originalXml.Length && this.originalXml[this.position] != quote)
        {
            if (this.originalXml[this.position] == '\n')
            {
                value.Append(this.lineEnding);
            }
            else
            {
                value.Append(this.originalXml[this.position]);
            }
            this.position++;
        }

        if (this.position < this.originalXml.Length)
        {
            this.position++; // Skip closing quote
        }

        return value.ToString();
    }

    private void SkipToChar(char target)
    {
        while (this.position < this.originalXml.Length && this.originalXml[this.position] != target)
        {
            this.position++;
        }

        if (this.position < this.originalXml.Length)
        {
            this.position++; // Skip the target character
        }
    }

    private void AddNode(RawNode node)
    {
        if (this.elementStack.Count > 0)
        {
            var parent = this.elementStack.Peek();
            node.Parent = parent;
            parent.Nodes.Add(node);
        }
        else
        {
            this.rootNodes.Add(node);
        }
    }

    private void ParseDocType()
    {
        this.position += 9; // Skip "<!DOCTYPE"

        var content = new StringBuilder();
        content.Append("<!DOCTYPE");

        var bracketCount = 0;
        var inQuotes = false;
        var quoteChar = '\0';

        while (this.position < this.originalXml.Length)
        {
            var ch = this.originalXml[this.position];

            if (!inQuotes)
            {
                if (ch is '"' or '\'')
                {
                    inQuotes = true;
                    quoteChar = ch;
                }
                else if (ch == '[')
                {
                    bracketCount++;
                }
                else if (ch == ']')
                {
                    bracketCount--;
                }
                else if (ch == '>' && bracketCount == 0)
                {
                    content.Append(ch);
                    this.position++;
                    break;
                }
            }
            else if (ch == quoteChar)
            {
                inQuotes = false;
                quoteChar = '\0';
            }

            if (ch == '\n')
            {
                content.Append(this.lineEnding);
            }
            else
            {
                content.Append(ch);
            }
            this.position++;
        }

        var node = new RawNode { NodeType = XmlNodeType.DocumentType, Value = content.ToString() };

        this.AddNode(node);
    }
}
