using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace CSharpier.Core.Xml;

internal
#if !NETSTANDARD2_0
partial
#endif
class CustomXmlReader
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

    // TODO 1679 if this stays, clean it all up
    public CustomXmlReader(string xml, string lineEnding)
    {
        this.originalXml = NewlineRegex
#if !NETSTANDARD2_0
            ()
#endif
        .Replace(xml, "\n");
        this.lineEnding = lineEnding;
        this.position = 0;
    }

    public List<RawNode> ReadAll()
    {
        while (this.position < this.originalXml.Length)
        {
            // TODO 1679 I think this is what is causing most of the issues, trimming off whitespace
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
                this.ParseText();
            }
        }

        return this.rootNodes;
    }

    private void SkipWhitespace()
    {
        // TODO 1679 I think this is causing most of the issues by trimming off whitespace on text nodes
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

        var node = new RawNode
        {
            Name = "",
            NodeType = XmlNodeType.Comment,
            IsEmpty = true,
            Attributes = [],
            Value = $"<!--{content}-->",
        };

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

        var node = new RawNode
        {
            Name = "",
            NodeType = XmlNodeType.CDATA,
            IsEmpty = true,
            Attributes = [],
            Value = $"<![CDATA[{content}]]>",
        };

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
            IsEmpty = true,
            Attributes = [],
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

        var text = content.ToString().Trim();
        if (!string.IsNullOrEmpty(text))
        {
            var node = new RawNode
            {
                Name = "",
                NodeType = XmlNodeType.Text,
                IsEmpty = true,
                Attributes = [],
                Value = text,
            };

            this.AddNode(node);
        }
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
            return "";
        }

        var quote = this.originalXml[this.position];
        if (quote is not ('"' or '\''))
        {
            return "";
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
            if (this.originalXml[this.position] == '\n') { }
            else { }
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

        var node = new RawNode
        {
            Name = "",
            NodeType = XmlNodeType.DocumentType,
            IsEmpty = true,
            Attributes = [],
            Value = content.ToString(),
        };

        this.AddNode(node);
    }
}
