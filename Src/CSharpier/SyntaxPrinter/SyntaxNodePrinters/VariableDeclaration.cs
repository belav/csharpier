using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class VariableDeclaration
    {
        // TODO review all of this, all kinds of stuff is borked
        // https://github.com/belav/command-line-api/pull/4
        // https://github.com/belav/aspnetcore/pull/5
        // https://github.com/belav/AspNetWebStack/pull/4
        // https://github.com/belav/AutoMapper/pull/4
        // https://github.com/belav/Core/pull/4
        // https://github.com/belav/efcore/pull/6
        // https://github.com/belav/format/pull/6
        // https://github.com/belav/insite-commerce/pull/4
        // https://github.com/belav/moq4/pull/4
        // https://github.com/belav/Newtonsoft.Json/pull/4
        // https://github.com/belav/roslyn/pull/4
        // https://github.com/belav/runtime/pull/4
        // https://github.com/belav/sdk/pull/1

        /*            
                        var expectedValue = new byte[SqlParameterCollectionExtensions.DefaultValueColumnWidth
                + 100];
                
                        private static readonly string[] NamespaceParts = new[] {
                "RenderTree",
                "Components",
                "AspNetCore",
                "Microsoft",
            };
            
                    public static object? FormatValue<[DynamicallyAccessedMembers(
                DynamicallyAccessedMemberTypes.All)]
            T>(T value, CultureInfo? culture = null)
            
                        public readonly StackObjectPool<Dictionary<
                    object,
                    KeyedItemInfo
                >> KeyedItemInfoDictionaryPool;
                
                        internal StackObjectPool<Dictionary<
                object,
                KeyedItemInfo
            >> KeyedItemInfoDictionaryPool { get; } =
            new StackObjectPool<Dictionary<object, KeyedItemInfo>>(
                maxPreservedItems: 10,
                () => new Dictionary<object, KeyedItemInfo>()
            );
                                    
                                                                var linePosition = diagnostics[
                                    i
                                ].Location.GetLineSpan().StartLinePosition;
                                
                                            RenderFragment<AuthenticationState> customNotAuthorized = state =>
                    builder => builder.AddContent(0, $"Go away, {state.User.Identity.Name}");
                    
        public List<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)> AuthorizeCalls { get; } =
            new List<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)>();
            
                        private static readonly Action<ILogger, string, string, Exception> _displayingNotFound =
                LoggerMessage.Define<string, string>(
                    LogLevel.Debug,
                    new EventId(1, "DisplayingNotFound"),
                    $"Displaying {nameof(NotFound)} because path '{{Path}}' with base URI '{{BaseUri}}' does not match any component route"
                );
                
                            private static readonly Action<
                ILogger,
                Type,
                string,
                string,
                Exception
            > _navigatingToComponent = LoggerMessage.Define<Type, string, string>(
                LogLevel.Debug,
                new EventId(2, "NavigatingToComponent"),
                "Navigating to component {ComponentType} in response to path '{Path}' with base URI '{BaseUri}'"
            );
            
            
                                                            builder3.OpenComponent<SecondCascadingParameterConsumerComponent<
                                                        string,
                                                        int
                                                    >>(0);
                                                    
                                                    
                                                            public static IRemoteAuthenticationBuilder<
            RemoteAuthenticationState,
            RemoteUserAccount
        > AddAccountClaimsPrincipalFactory<[DynamicallyAccessedMembers(
                DynamicallyAccessedMemberTypes.PublicConstructors)]
            TAccountClaimsPrincipalFactory>(
            this IRemoteAuthenticationBuilder<RemoteAuthenticationState, RemoteUserAccount> builder
        )
            where TAccountClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount> =>
            builder.AddAccountClaimsPrincipalFactory<
                RemoteAuthenticationState,
                RemoteUserAccount,
                TAccountClaimsPrincipalFactory
            >();
            
                    private static readonly Action<IApplicationBuilder> ActionNotImplemented =
            new Action<IApplicationBuilder>(
                _ =>
                {
                    throw new NotImplementedException();
                }
            );
         */
        
        public static Doc Print(VariableDeclarationSyntax node)
        {
            if (node.Variables.Count > 1)
            {
                var docs = Doc.Concat(
                    SeparatedSyntaxList.Print(
                        node.Variables,
                        VariableDeclarator.Print,
                        node.Parent is ForStatementSyntax ? Doc.Line : Doc.HardLine
                    )
                );

                return Doc.Concat(Node.Print(node.Type), " ", Doc.Indent(docs));
            }

            var variable = node.Variables[0];
            var initializer = variable.Initializer;

            var formatMode = initializer?.Value
                is AnonymousObjectCreationExpressionSyntax
                    or AnonymousMethodExpressionSyntax
                    or InitializerExpressionSyntax
                    or InvocationExpressionSyntax
                    or ConditionalExpressionSyntax
                    or ObjectCreationExpressionSyntax
                    or SwitchExpressionSyntax
                    or LambdaExpressionSyntax
                ? FormatMode.NoIndent
                : initializer?.Value
                        is BinaryExpressionSyntax
                            or InterpolatedStringExpressionSyntax
                            or IsPatternExpressionSyntax
                            or LiteralExpressionSyntax
                            or QueryExpressionSyntax
                            or StackAllocArrayCreationExpressionSyntax
                        ? FormatMode.IndentWithLine
                        : FormatMode.Indent;

            var groupId = Guid.NewGuid().ToString();

            return Doc.Concat(
                Doc.Group(
                    Node.Print(node.Type),
                    " ",
                    Token.Print(variable.Identifier),
                    variable.ArgumentList != null
                        ? BracketedArgumentList.Print(variable.ArgumentList)
                        : Doc.Null,
                    initializer != null
                        ? Doc.Concat(" ", Token.Print(initializer.EqualsToken))
                        : Doc.Null
                ),
                initializer != null
                    ? Doc.Concat(
                            formatMode is FormatMode.IndentWithLine
                                ? Doc.Null
                                : Doc.GroupWithId(groupId, Doc.Indent(Doc.Line)),
                            initializer.Value
                                is InvocationExpressionSyntax
                                    or ParenthesizedLambdaExpressionSyntax
                                or ObjectCreationExpressionSyntax
                                ? Doc.IndentIfBreak(
                                        Doc.Group(Node.Print(initializer.Value)),
                                        groupId
                                    )
                                : Doc.Group(IndentIfNeeded(initializer.Value, formatMode))
                        )
                    : Doc.Null
            );
        }

        private static Doc IndentIfNeeded(ExpressionSyntax initializerValue, FormatMode formatMode)
        {
            if (formatMode is FormatMode.NoIndent)
            {
                return Node.Print(initializerValue);
            }

            return Doc.Indent(
                formatMode is FormatMode.IndentWithLine ? Doc.Line : Doc.Null,
                Node.Print(initializerValue)
            );
        }

        private enum FormatMode
        {
            NoIndent,
            IndentWithLine,
            Indent
        }
    }
}
