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

            void Traverse(ExpressionSyntax expression)
            {
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
                    Traverse(invocationExpressionSyntax.Expression);
                    printedNodes.Add(
                        new PrintedNode(
                            invocationExpressionSyntax,
                            ArgumentList.Print(invocationExpressionSyntax.ArgumentList)
                        )
                    );
                }
                else if (expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
                {
                    Traverse(memberAccessExpressionSyntax.Expression);
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

            Traverse(node);

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

            var oneLine = groups.SelectMany(o => o).Select(o => o.Doc).ToArray();

            var shouldMergeFirstTwoGroups = ShouldMergeFirstTwoGroups(groups);

            var cutoff = shouldMergeFirstTwoGroups ? 3 : 2;
            if (groups.Count <= cutoff)
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

        // TODO we may need to ditch this, it leads to weird shit, like
        /*
         unless we can figure out some way to make it work..... hmmmm
         ifBreak?
         check length of the 2nd group?
         only if the second group doesn't have a lambda?
        I think the problem is the one line version fits, so it gets used
        
good
        AnIdentifier.DoSomething___________________()
            .DoSomething___________________()
            .DoSomething___________________();
    
        AnIdentifier.DoSomething___________________()
            .DoSomething___________________()
            .DoSomething___________________();
bad
        var someValue = someOtherValue.Where(
            o => someLongCondition__________________________
        ).Where(o => someLongCondition__________________________);
     
        this.Where((o) => someLongCondition__________________________).Where(
            (o) => someLongCondition__________________________
        );
        
         */
        private static bool ShouldMergeFirstTwoGroups(List<List<PrintedNode>> groups)
        {
            return false;

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
                return (
                    firstNode is ThisExpressionSyntax
                    || (
                        firstNode is IdentifierNameSyntax
                    // we might not need any of this
                    // && (
                    //     isFactory(firstNode.name)
                    //     || ( isExpressionStatement && isShort(firstNode.name) )
                    //     || hasComputed)
                    // )
                    )
                );
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

        this.Address1 = addressFields
            .Where(field => field.FieldName == "Address1__________________________")
            .Where(field => yeah)
            .Where(field => yeah);

        roleNames
            .ToList()
            .ForEach(
                (role) =>
                    this.adminContextMock.Setup((ctx) => ctx.IsUserInRole(role)).Returns(false)
            );

        var superLongMethodNameForceLine = someFactoryName__________
            .SuperLongMethodNameForceLine()
            .SomeOtherReallyLongMethodName();

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
        var proxy1 = generator.CreateInterfaceProxyWithTargetInterface<IEventHandler<EventArgs1>>(
            null,
            new[] { lazyInterceptor1 }
        );
        
        // this is just gross, it used to break parameters instead of generic types
        var count = (int)await _invoker.InvokeUnmarshalled<
            string[],
            object?,
            object?,
            Task<object>
        >(GetSatelliteAssemblies, culturesToLoad.ToArray(), null, null);

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
    }
}

 */
