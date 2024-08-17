namespace CSharpier.Formatters.CSharp;

using System.Collections.Concurrent;
using System.Linq.Expressions;

internal abstract class BooleanExpressionParser
{
    private static readonly ConcurrentDictionary<string, BooleanExpression> parsedExpressions =
        new();

    public static BooleanExpression Parse(string input)
    {
        if (parsedExpressions.TryGetValue(input, out var booleanExpression))
        {
            return booleanExpression;
        }

        var tokens = Tokenize(input);
        var postfix = ShuntingYard(tokens);
        booleanExpression = BuildFunction(postfix);
        parsedExpressions.TryAdd(input, booleanExpression);

        return booleanExpression;
    }

    private static List<Token> Tokenize(string input)
    {
        bool IsPartOfValue(char character)
        {
            return char.IsLetter(character) || char.IsNumber(character) || character == '_';
        }

        var tokens = new List<Token>();
        for (var x = 0; x < input.Length; x++)
        {
            var character = input[x];
            if (char.IsWhiteSpace(character))
            {
                continue;
            }

            if (IsPartOfValue(character))
            {
                var start = x;
                while (x < input.Length && IsPartOfValue(input[x]))
                {
                    x++;
                }

                var value = input[start..x];

                if (value is "true")
                {
                    tokens.Add(new Token { Type = TokenType.True });
                }
                else if (value is "false")
                {
                    tokens.Add(new Token { Type = TokenType.False });
                }
                else
                {
                    tokens.Add(new Token { Type = TokenType.Variable, Value = value });
                }
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
            else if (character == '=')
            {
                x++;
                tokens.Add(new Token { Type = TokenType.Equals });
            }
            else if (character == '!')
            {
                if (input[x + 1] == '=')
                {
                    x++;
                    tokens.Add(new Token { Type = TokenType.NotEquals });
                }
                else
                {
                    tokens.Add(new Token { Type = TokenType.Not });
                }
            }
            else
            {
                throw new ArgumentException($"Invalid character in input. '{character}'");
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
                case TokenType.False:
                case TokenType.True:
                case TokenType.Variable:
                    output.Enqueue(token);
                    break;
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Equals:
                case TokenType.NotEquals:
                    while (operators.Count > 0 && operators.Peek().Type != TokenType.LeftParen)
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Push(token);
                    break;
                case TokenType.Not:
                case TokenType.LeftParen:
                    operators.Push(token);
                    break;
                case TokenType.RightParen:
                    while (operators.Peek().Type != TokenType.LeftParen)
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Pop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        while (operators.Count > 0)
        {
            output.Enqueue(operators.Pop());
        }
        return output;
    }

    private static BooleanExpression BuildFunction(Queue<Token> tokens)
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
                            typeof(Dictionary<string, bool>).GetProperty("Item")!,
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
                case TokenType.Equals:
                    var rightEquals = stack.Pop();
                    var leftEquals = stack.Pop();
                    stack.Push(Expression.Equal(leftEquals, rightEquals));
                    break;
                case TokenType.NotEquals:
                    var rightNotEquals = stack.Pop();
                    var leftNotEquals = stack.Pop();
                    stack.Push(Expression.NotEqual(leftNotEquals, rightNotEquals));
                    break;
                case TokenType.True:
                    stack.Push(Expression.Constant(true));
                    break;
                case TokenType.False:
                    stack.Push(Expression.Constant(false));
                    break;
            }
        }
        var expression = stack.Pop();
        var lambda = Expression.Lambda<Func<Dictionary<string, bool>, bool>>(expression, paramDict);

        return new BooleanExpression
        {
            Function = lambda.Compile(),
            Parameters = variables.Select(o => o.Key).ToList(),
        };
    }
}

internal enum TokenType
{
    And,
    Or,
    Not,
    Variable,
    LeftParen,
    RightParen,
    Equals,
    True,
    False,
    NotEquals,
}

internal class Token
{
    public TokenType Type { get; init; }
    public string Value { get; init; } = string.Empty;
}
