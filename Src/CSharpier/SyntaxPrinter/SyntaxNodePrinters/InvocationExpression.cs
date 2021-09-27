using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public record PrintedNode(CSharpSyntaxNode Node, Doc Doc);

    public static class InvocationExpression
    {
        public static Doc Print(InvocationExpressionSyntax node)
        {
            var printedNodes = new List<PrintedNode>();

            FlattenAndPrintNodes(node, printedNodes);

            var groups = GroupPrintedNodes(printedNodes);

            var oneLine = groups.SelectMany(o => o).Select(o => o.Doc).ToArray();

            var shouldMergeFirstTwoGroups = ShouldMergeFirstTwoGroups(groups);

            var cutoff = shouldMergeFirstTwoGroups ? 3 : 2;
            var forceOneLine =
                groups.Count <= cutoff
                && !groups.All(o => o.Last().Node is InvocationExpressionSyntax);

            if (forceOneLine)
            {
                return Doc.Group(oneLine);
            }

            var expanded = Doc.Concat(
                Doc.Concat(groups[0].Select(o => o.Doc).ToArray()),
                shouldMergeFirstTwoGroups
                    ? Doc.Concat(groups[1].Select(o => o.Doc).ToArray())
                    : Doc.Null,
                PrintIndentedGroup(node, groups.Skip(shouldMergeFirstTwoGroups ? 2 : 1).ToList())
            );

            return Doc.ConditionalGroup(Doc.Concat(oneLine), expanded);
        }

        private static void FlattenAndPrintNodes(
            ExpressionSyntax expression,
            List<PrintedNode> printedNodes
        ) {
            /*
              We need to flatten things out because the AST has them this way
              where the first node is this 
              InvocationExpression
              [ this.DoSomething().DoSomething ] [ () ]
              
              SimpleMemberAccessExpression
              [ this.DoSomething() ] [ . ] [ DoSomething ]
              
              InvocationExpression
              [ this.DoSomething ] [ () ]
              
              SimpleMemberAccessExpression
              [ this ] [ . ] [ DoSomething ]
            */
            if (expression is InvocationExpressionSyntax invocationExpressionSyntax)
            {
                FlattenAndPrintNodes(invocationExpressionSyntax.Expression, printedNodes);
                printedNodes.Add(
                    new PrintedNode(
                        invocationExpressionSyntax,
                        ArgumentList.Print(invocationExpressionSyntax.ArgumentList)
                    )
                );
            }
            else if (expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
            {
                FlattenAndPrintNodes(memberAccessExpressionSyntax.Expression, printedNodes);
                printedNodes.Add(
                    new PrintedNode(
                        memberAccessExpressionSyntax,
                        Doc.Concat(
                            Token.Print(memberAccessExpressionSyntax.OperatorToken),
                            Node.Print(memberAccessExpressionSyntax.Name)
                        )
                    )
                );
            }
            else
            {
                printedNodes.Add(new PrintedNode(expression, Node.Print(expression)));
            }
        }

        private static List<List<PrintedNode>> GroupPrintedNodes(List<PrintedNode> printedNodes)
        {
            // Once we have a linear list of printed nodes, we want to create groups out
            // of it.
            //
            //   a().b.c().d().e
            // will be grouped as
            //   [
            //     [Identifier, InvocationExpression],
            //     [MemberAccessExpression, MemberAccessExpression, InvocationExpression],
            //     [MemberAccessExpression, InvocationExpression],
            //     [MemberAccessExpression],
            //   ]

            // so that we can print it as
            //   a()
            //     .b.c()
            //     .d()
            //     .e

            // The first group is the first node followed by
            //   - as many InvocationExpression as possible
            //       < fn()()() >.something()
            //   - as many array accessors as possible
            //       < fn()[0][1][2] >.something()
            //   - then, as many MemberAccessExpression as possible but the last one
            //       < this.items >.something()

            var groups = new List<List<PrintedNode>>();
            var currentGroup = new List<PrintedNode> { printedNodes[0] };
            var index = 1;
            for (; index < printedNodes.Count; index++)
            {
                if (printedNodes[index].Node is InvocationExpressionSyntax)
                {
                    currentGroup.Add(printedNodes[index]);
                }
                else
                {
                    break;
                }
            }

            if (printedNodes[0].Node is not InvocationExpressionSyntax)
            {
                for (; index + 1 < printedNodes.Count; ++index)
                {
                    if (
                        IsMemberish(printedNodes[index].Node)
                        && IsMemberish(printedNodes[index + 1].Node)
                    ) {
                        currentGroup.Add(printedNodes[index]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            groups.Add(currentGroup);
            currentGroup = new List<PrintedNode>();

            var hasSeenInvocationExpression = false;
            for (; index < printedNodes.Count; index++)
            {
                if (hasSeenInvocationExpression && IsMemberish(printedNodes[index].Node))
                {
                    groups.Add(currentGroup);
                    currentGroup = new List<PrintedNode>();
                    hasSeenInvocationExpression = false;
                }

                if (printedNodes[index].Node is InvocationExpressionSyntax)
                {
                    hasSeenInvocationExpression = true;
                }
                currentGroup.Add(printedNodes[index]);
            }

            if (currentGroup.Any())
            {
                groups.Add(currentGroup);
            }

            return groups;
        }

        private static bool IsMemberish(CSharpSyntaxNode node)
        {
            return node is MemberAccessExpressionSyntax;
        }

        private static Doc PrintIndentedGroup(
            InvocationExpressionSyntax node,
            IList<List<PrintedNode>> groups
        ) {
            if (groups.Count == 0)
            {
                return Doc.Null;
            }

            return Doc.IndentIf(
                node.Parent is not ConditionalExpressionSyntax,
                Doc.Group(
                    Doc.HardLine,
                    Doc.Join(
                        Doc.HardLine,
                        groups.Select(o => Doc.Group(o.Select(o => o.Doc).ToArray()))
                    )
                )
            );
        }

        private static bool ShouldMergeFirstTwoGroups(List<List<PrintedNode>> groups)
        {
            if (groups.Count < 2
            // || hasComment(groups[1][0].node)
            )
            {
                return false;
            }

            // wtf is this? it is on the node in prettier
            //var hasComputed = false; // groups[1].Count > 0 && groups[1][0].node.computed;

            if (groups[0].Count == 1)
            {
                var firstNode = groups[0][0].Node;
                return firstNode is ThisExpressionSyntax or PredefinedTypeSyntax
                // or IdentifierNameSyntax
                // I don't think I like this, it screws
                ;
                // we might not need any of this, it was on the identifierNameSyntax
                // && (
                //     isFactory(firstNode.name)
                //     || ( isExpressionStatement && isShort(firstNode.name) )
                //     || hasComputed)
                // )

                ;
            }
            // const lastNode = getLast(groups[0]).node;
            // return (
            //     isMemberExpression(lastNode) &&
            //         lastNode.property.type === "Identifier" &&
            //                                    (isFactory(lastNode.property.name) || hasComputed)
            // );
            return false;
        }
    }
}

// https://github.com/prettier/prettier/issues/5737
// https://github.com/prettier/prettier/issues/8902

// this too, although it got pulled out https://github.com/prettier/prettier/pull/7889
// this looks good https://github.com/prettier/prettier/pull/8063/files
/* test case for above
             XAttribute[] rootAttributes = RootAttributes?.Select(
                item => new XAttribute(item.ItemSpec, item.GetMetadata("Value"))
            ).ToArray();
 */

// some discussions
// https://github.com/prettier/prettier/issues/8003
// https://github.com/prettier/prettier/issues/7884

/*
class ClassName
{
    // this is better, but still looks weird
    public AccessTokenNotAvailableException(
        NavigationManager navigation,
        AccessTokenResult tokenResult,
        IEnumerable<string> scopes
    ) : base(
        message: "Unable to provision an access token for the requested scopes: " + scopes != null
          ? $"'{string.Join(", ", scopes ?? Array.Empty<string>())}'"
          : "(default scopes)"
    ) { }

    void MethodName()
    {

        // this isn't how prettier does it, stretch goal
        this.Address1 = addressFields_________________
            .FirstOrDefault(field => field.FieldName == "Address1");
        // except that conflicts with
        var y = someList.Where(
            o =>
                someLongValue_______________________
                && theseShouldNotIndent_________________
                && theseShouldNotIndent_________________
                    > butThisOneShould_________________________________________
        );

        // this is kinda gross, maybe it should only be short names and this that merge
        var superLongName_______________ = someOtherName____________.CallSomeMethod______________()
            .CallSomeMethod______________();

        // prettier does it this way, but ugly!
        string signedRequest = htmlHelper.ViewContext.HttpContext.Request.Params[
            "signed_request____________"
        ];

        // prettier does it this way, but ugly!
        result[i / ReferenceFrameSize] = RenderTreeFrame.ComponentReferenceCapture(
            0,
            null__________,
            0
        );

        // not sure if anything here should change
        Diagnostic.Create(
            _descriptor,
            symbolForDiagnostic.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax()
                .GetLocation() ?? Location.None,
            symbol.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat)
        );
        
        // see below
        FacebookGroupConnection<TStatus> statuses = await GetFacebookObjectAsync<
            FacebookGroupConnection<TStatus>
        >(client, "me/statuses");

        /* should maybe be
            FacebookGroupConnection<TStatus> statuses = 
                await GetFacebookObjectAsync<FacebookGroupConnection<TStatus>>(
                    client,
                    "me/statuses"
                );
                

        // this one seems fine?
        // but kinda looks better without the merge
        var proxy1 = generator.CreateInterfaceProxyWithTargetInterface<IEventHandler<EventArgs1>>(
            null,
            new[] { lazyInterceptor1 }
        );
        
        // this is just gross, it used to break parameters instead of generic types
        // but it is just barely too long so it shouldn't break on generics
        var count = (int)await _invoker.InvokeUnmarshalled<
            string[],
            object?,
            object?,
            Task<object>
        >(GetSatelliteAssemblies, culturesToLoad.ToArray(), null, null);

        // looks better with the merge
        var ex = Assert.Throws<ArgumentException>(
            () =>
                generator.CreateInterfaceProxyWithTargetInterface<IList<IList<PrivateInterface>>>(
                    new List<IList<PrivateInterface>>(),
                    new IInterceptor[0]
                )
        );

        // these used to not break so weird
        ref var parentFrame = ref diffContext.NewTree[
            newFrame.ComponentReferenceCaptureParentFrameIndexField
        ];

        var newComponentInstance = (CaptureSetParametersComponent)oldTree.GetFrames().Array[
            0
        ].Component;

        parent = (ContainerNode)parent.Children[
            childIndexAtCurrentDepth__________ + siblingIndex__________
        ];

        configuration.EncryptionAlgorithmKeySize = (int)encryptionElement.Attribute(
            "keyLength__________________"
        )!;

        // used to break onto the next line
        var cbTempSubkeys = checked(
            _symmetricAlgorithmSubkeyLengthInBytes + _hmacAlgorithmSubkeyLengthInBytes
        );
        // but then this looks better with checked next to =
        var cbEncryptedData = checked(
            cbCiphertext
            - (
                KEY_MODIFIER_SIZE_IN_BYTES
                + _symmetricAlgorithmBlockSizeInBytes
                + _hmacAlgorithmDigestLengthInBytes
            )
        );

        // used to be 2 lines
        var cacheKey__________ = (
            ModelType: fieldIdentifier.Model.GetType(),
            fieldIdentifier.FieldName
        );

        // not sure what to do with this, nothing??
        storedArguments_______[i] = localVariables[i] = Expression.Parameter(
            parameters[i].ParameterType
        );
        
        
        // this switched and is probably better
        private static readonly MethodInfo _callPropertyGetterOpenGenericMethod =
            typeof(PropertyHelper).GetMethod(
                "CallPropertyGetter",
                BindingFlags.NonPublic | BindingFlags.Static
            );
        private static readonly MethodInfo _callPropertyGetterOpenGenericMethod =
            typeof(PropertyHelper)
                .GetMethod("CallPropertyGetter", BindingFlags.NonPublic | BindingFlags.Static);
        // except this got worse!!
        private static readonly MethodInfo _callPropertyGetterByReferenceOpenGenericMethod =
            typeof(PropertyHelper).GetMethod(
                "CallPropertyGetterByReference",
                BindingFlags.NonPublic | BindingFlags.Static
            );
        private static readonly MethodInfo _callPropertyGetterByReferenceOpenGenericMethod =
            typeof(PropertyHelper)
                .GetMethod(
                    "CallPropertyGetterByReference",
                    BindingFlags.NonPublic | BindingFlags.Static
                );
                

        // this looks weird, but might be correct? prettier has some inconsistent formatting around things like this
                IEnumerable<PropertyInfo> properties = type.GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                )
                    .Where(prop => prop.GetIndexParameters().Length == 0 && prop.GetMethod != null);

            XAttribute[] rootAttributes = RootAttributes?.Select(
                item => new XAttribute(item.ItemSpec, item.GetMetadata("Value"))
            )
                .ToArray();


        // add some test cases for things like this
            return permissionsStatus.Status
                .Where(kvp => kvp.Value == status)
                .Select(kvp => kvp.Key);
                
        // this used to not break before the .ConvertTo
                return modelState.Value
                    .ConvertTo(
                        destinationType,
                        null /asterisk culture asterisk/ 
                    );
                    
        // another better/worse
            _currentChain.Elements
                .Add(new ParameterExpressionFingerprint(node.NodeType, node.Type, parameterIndex));
            _currentChain.Elements
                .Add(
                    new TypeBinaryExpressionFingerprint(node.NodeType, node.Type, node.TypeOperand)
                );
        // another better/worse??
            return SymbolEqualityComparer.Default
                .Equals(symbol.Parameters[0].Type, symbols.IServiceCollection);
            return SymbolEqualityComparer.Default.Equals(
                symbol.Parameters[0].Type,
                symbols.IServiceCollection
            );                

        // probably should merge
        if (
            httpResponse.StatusCode == HttpStatusCode.NotFound
            || httpResponse.ReasonPhrase
                .IndexOf(
                    "The requested URI does not represent any resource on the server.",
                    StringComparison.OrdinalIgnoreCase
                ) == 0
        )
        
        // probably should merge
        var value = string
            .Join(",", Values.Select(o => o.ItemSpec).ToArray())
            .ToLowerInvariant();
        
        if (
            context.Operation is IInvocationOperation invocation
            && invocation.Instance == null
            && invocation.Arguments.Length >= 1
            && SymbolEqualityComparer.Default
                .Equals(
                    invocation.Arguments[0].Parameter?.Type,
                    _context.StartupSymbols.IApplicationBuilder
                )
        )
                    
        if (
            symbol.Name == null
            || !symbol.Name
                .StartsWith(
                    SymbolNames.ConfigureServicesMethodPrefix,
                    StringComparison.OrdinalIgnoreCase
                )
            || !symbol.Name
                .EndsWith(
                    SymbolNames.ConfigureServicesMethodSuffix,
                    StringComparison.OrdinalIgnoreCase
                )
        )
            
        solution = new AdhocWorkspace().CurrentSolution
            .AddProject(
                projectId,
                TestProjectName,
                TestProjectName,
                LanguageNames.CSharp
            );
    }
}

 */
