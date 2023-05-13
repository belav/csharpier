namespace CSharpier.Formatters.CSharp;

using System.Linq.Expressions;

internal abstract class BooleanExpressionParser
{
    public static (Func<Dictionary<string, bool>, bool>, List<string>) Parse(string input)
    {
        var tokens = Tokenize(input);
        var postfix = ShuntingYard(tokens);
        return BuildFunction(postfix);
    }

    private static List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        for (var x = 0; x < input.Length; x++)
        {
            var character = input[x];
            if (char.IsWhiteSpace(character))
            {
                continue;
            }

            if (char.IsLetter(character))
            {
                var start = x;
                while (x < input.Length && char.IsLetter(input[x]))
                {
                    x++;
                }
                tokens.Add(new Token { Type = TokenType.Variable, Value = input[start..x] });
            }
            else if (character == '(')
            {
                tokens.Add(new Token { Type = TokenType.LeftParen });
            }
            else if (character == ')')
            {
                tokens.Add(new Token { Type = TokenType.RightParen });
            }
            else if (character == '&')
            {
                x++;
                tokens.Add(new Token { Type = TokenType.And });
            }
            else if (character == '|')
            {
                x++;
                tokens.Add(new Token { Type = TokenType.Or });
            }
            else if (character == '!')
            {
                tokens.Add(new Token { Type = TokenType.Not });
            }
            else
            {
                throw new ArgumentException("Invalid character in input.");
            }
        }
        return tokens;
    }

    private static Queue<Token> ShuntingYard(List<Token> tokens)
    {
        var output = new Queue<Token>();
        var operators = new Stack<Token>();
        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.Variable:
                    output.Enqueue(token);
                    break;
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Not:
                    while (operators.Count > 0 && operators.Peek().Type != TokenType.LeftParen)
                        output.Enqueue(operators.Pop());
                    operators.Push(token);
                    break;
                case TokenType.LeftParen:
                    operators.Push(token);
                    break;
                case TokenType.RightParen:
                    while (operators.Peek().Type != TokenType.LeftParen)
                        output.Enqueue(operators.Pop());
                    operators.Pop(); // Pop the left parenthesis.
                    break;
            }
        }

        while (operators.Count > 0)
        {
            output.Enqueue(operators.Pop());
        }
        return output;
    }

    private static (Func<Dictionary<string, bool>, bool>, List<string>) BuildFunction(
        Queue<Token> tokens
    )
    {
        var variables = new Dictionary<string, IndexExpression>();
        var paramDict = Expression.Parameter(typeof(Dictionary<string, bool>), "dict");

        var stack = new Stack<Expression>();
        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.Variable:
                    if (!variables.TryGetValue(token.Value, out var param))
                    {
                        param = Expression.Property(
                            paramDict,
                            typeof(Dictionary<string, bool>).GetProperty("Item"),
                            Expression.Constant(token.Value)
                        );
                        variables[token.Value] = param;
                    }
                    stack.Push(param);
                    break;
                case TokenType.And:
                    var rightAnd = stack.Pop();
                    var leftAnd = stack.Pop();
                    stack.Push(Expression.AndAlso(leftAnd, rightAnd));
                    break;
                case TokenType.Or:
                    var rightOr = stack.Pop();
                    var leftOr = stack.Pop();
                    stack.Push(Expression.OrElse(leftOr, rightOr));
                    break;
                case TokenType.Not:
                    var operand = stack.Pop();
                    stack.Push(Expression.Not(operand));
                    break;
            }
        }
        var expression = stack.Pop();
        var lambda = Expression.Lambda<Func<Dictionary<string, bool>, bool>>(expression, paramDict);

        return (lambda.Compile(), variables.Select(o => o.Key).ToList());
    }
}

internal enum TokenType
{
    And,
    Or,
    Not,
    Variable,
    LeftParen,
    RightParen
}

internal class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
}
