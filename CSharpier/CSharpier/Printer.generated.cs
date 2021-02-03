using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        public Doc Print(SyntaxNode syntaxNode)
        {
            if (syntaxNode is AccessorDeclarationSyntax accessorDeclarationSyntax)
            {
                return this.PrintAccessorDeclarationSyntax(accessorDeclarationSyntax);
            }
            else if (syntaxNode is AliasQualifiedNameSyntax aliasQualifiedNameSyntax)
            {
                return this.PrintAliasQualifiedNameSyntax(aliasQualifiedNameSyntax);
            }
            else if (syntaxNode is AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax)
            {
                return this.PrintAnonymousMethodExpressionSyntax(anonymousMethodExpressionSyntax);
            }
            else if (syntaxNode is AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax)
            {
                return this.PrintAnonymousObjectCreationExpressionSyntax(anonymousObjectCreationExpressionSyntax);
            }
            else if (syntaxNode is AnonymousObjectMemberDeclaratorSyntax anonymousObjectMemberDeclaratorSyntax)
            {
                return this.PrintAnonymousObjectMemberDeclaratorSyntax(anonymousObjectMemberDeclaratorSyntax);
            }
            else if (syntaxNode is ArgumentListSyntax argumentListSyntax)
            {
                return this.PrintArgumentListSyntax(argumentListSyntax);
            }
            else if (syntaxNode is ArgumentSyntax argumentSyntax)
            {
                return this.PrintArgumentSyntax(argumentSyntax);
            }
            else if (syntaxNode is ArrayCreationExpressionSyntax arrayCreationExpressionSyntax)
            {
                return this.PrintArrayCreationExpressionSyntax(arrayCreationExpressionSyntax);
            }
            else if (syntaxNode is ArrayRankSpecifierSyntax arrayRankSpecifierSyntax)
            {
                return this.PrintArrayRankSpecifierSyntax(arrayRankSpecifierSyntax);
            }
            else if (syntaxNode is ArrayTypeSyntax arrayTypeSyntax)
            {
                return this.PrintArrayTypeSyntax(arrayTypeSyntax);
            }
            else if (syntaxNode is ArrowExpressionClauseSyntax arrowExpressionClauseSyntax)
            {
                return this.PrintArrowExpressionClauseSyntax(arrowExpressionClauseSyntax);
            }
            else if (syntaxNode is AssignmentExpressionSyntax assignmentExpressionSyntax)
            {
                return this.PrintAssignmentExpressionSyntax(assignmentExpressionSyntax);
            }
            else if (syntaxNode is AttributeListSyntax attributeListSyntax)
            {
                return this.PrintAttributeListSyntax(attributeListSyntax);
            }
            else if (syntaxNode is AwaitExpressionSyntax awaitExpressionSyntax)
            {
                return this.PrintAwaitExpressionSyntax(awaitExpressionSyntax);
            }
            else if (syntaxNode is BaseExpressionSyntax baseExpressionSyntax)
            {
                return this.PrintBaseExpressionSyntax(baseExpressionSyntax);
            }
            else if (syntaxNode is BaseListSyntax baseListSyntax)
            {
                return this.PrintBaseListSyntax(baseListSyntax);
            }
            else if (syntaxNode is BinaryExpressionSyntax binaryExpressionSyntax)
            {
                return this.PrintBinaryExpressionSyntax(binaryExpressionSyntax);
            }
            else if (syntaxNode is BlockSyntax blockSyntax)
            {
                return this.PrintBlockSyntax(blockSyntax);
            }
            else if (syntaxNode is BracketedArgumentListSyntax bracketedArgumentListSyntax)
            {
                return this.PrintBracketedArgumentListSyntax(bracketedArgumentListSyntax);
            }
            else if (syntaxNode is BreakStatementSyntax breakStatementSyntax)
            {
                return this.PrintBreakStatementSyntax(breakStatementSyntax);
            }
            else if (syntaxNode is CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax)
            {
                return this.PrintCasePatternSwitchLabelSyntax(casePatternSwitchLabelSyntax);
            }
            else if (syntaxNode is CaseSwitchLabelSyntax caseSwitchLabelSyntax)
            {
                return this.PrintCaseSwitchLabelSyntax(caseSwitchLabelSyntax);
            }
            else if (syntaxNode is CastExpressionSyntax castExpressionSyntax)
            {
                return this.PrintCastExpressionSyntax(castExpressionSyntax);
            }
            else if (syntaxNode is CatchClauseSyntax catchClauseSyntax)
            {
                return this.PrintCatchClauseSyntax(catchClauseSyntax);
            }
            else if (syntaxNode is CheckedExpressionSyntax checkedExpressionSyntax)
            {
                return this.PrintCheckedExpressionSyntax(checkedExpressionSyntax);
            }
            else if (syntaxNode is CheckedStatementSyntax checkedStatementSyntax)
            {
                return this.PrintCheckedStatementSyntax(checkedStatementSyntax);
            }
            else if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
            {
                return this.PrintClassDeclarationSyntax(classDeclarationSyntax);
            }
            else if (syntaxNode is ClassOrStructConstraintSyntax classOrStructConstraintSyntax)
            {
                return this.PrintClassOrStructConstraintSyntax(classOrStructConstraintSyntax);
            }
            else if (syntaxNode is CompilationUnitSyntax compilationUnitSyntax)
            {
                return this.PrintCompilationUnitSyntax(compilationUnitSyntax);
            }
            else if (syntaxNode is ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax)
            {
                return this.PrintConditionalAccessExpressionSyntax(conditionalAccessExpressionSyntax);
            }
            else if (syntaxNode is ConditionalExpressionSyntax conditionalExpressionSyntax)
            {
                return this.PrintConditionalExpressionSyntax(conditionalExpressionSyntax);
            }
            else if (syntaxNode is ConstantPatternSyntax constantPatternSyntax)
            {
                return this.PrintConstantPatternSyntax(constantPatternSyntax);
            }
            else if (syntaxNode is ConstructorConstraintSyntax constructorConstraintSyntax)
            {
                return this.PrintConstructorConstraintSyntax(constructorConstraintSyntax);
            }
            else if (syntaxNode is ConstructorDeclarationSyntax constructorDeclarationSyntax)
            {
                return this.PrintConstructorDeclarationSyntax(constructorDeclarationSyntax);
            }
            else if (syntaxNode is ContinueStatementSyntax continueStatementSyntax)
            {
                return this.PrintContinueStatementSyntax(continueStatementSyntax);
            }
            else if (syntaxNode is ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax)
            {
                return this.PrintConversionOperatorDeclarationSyntax(conversionOperatorDeclarationSyntax);
            }
            else if (syntaxNode is DeclarationExpressionSyntax declarationExpressionSyntax)
            {
                return this.PrintDeclarationExpressionSyntax(declarationExpressionSyntax);
            }
            else if (syntaxNode is DeclarationPatternSyntax declarationPatternSyntax)
            {
                return this.PrintDeclarationPatternSyntax(declarationPatternSyntax);
            }
            else if (syntaxNode is DefaultExpressionSyntax defaultExpressionSyntax)
            {
                return this.PrintDefaultExpressionSyntax(defaultExpressionSyntax);
            }
            else if (syntaxNode is DefaultSwitchLabelSyntax defaultSwitchLabelSyntax)
            {
                return this.PrintDefaultSwitchLabelSyntax(defaultSwitchLabelSyntax);
            }
            else if (syntaxNode is DelegateDeclarationSyntax delegateDeclarationSyntax)
            {
                return this.PrintDelegateDeclarationSyntax(delegateDeclarationSyntax);
            }
            else if (syntaxNode is DestructorDeclarationSyntax destructorDeclarationSyntax)
            {
                return this.PrintDestructorDeclarationSyntax(destructorDeclarationSyntax);
            }
            else if (syntaxNode is DiscardPatternSyntax discardPatternSyntax)
            {
                return this.PrintDiscardPatternSyntax(discardPatternSyntax);
            }
            else if (syntaxNode is DoStatementSyntax doStatementSyntax)
            {
                return this.PrintDoStatementSyntax(doStatementSyntax);
            }
            else if (syntaxNode is ElementAccessExpressionSyntax elementAccessExpressionSyntax)
            {
                return this.PrintElementAccessExpressionSyntax(elementAccessExpressionSyntax);
            }
            else if (syntaxNode is ElementBindingExpressionSyntax elementBindingExpressionSyntax)
            {
                return this.PrintElementBindingExpressionSyntax(elementBindingExpressionSyntax);
            }
            else if (syntaxNode is ElseClauseSyntax elseClauseSyntax)
            {
                return this.PrintElseClauseSyntax(elseClauseSyntax);
            }
            else if (syntaxNode is EmptyStatementSyntax emptyStatementSyntax)
            {
                return this.PrintEmptyStatementSyntax(emptyStatementSyntax);
            }
            else if (syntaxNode is EnumDeclarationSyntax enumDeclarationSyntax)
            {
                return this.PrintEnumDeclarationSyntax(enumDeclarationSyntax);
            }
            else if (syntaxNode is EnumMemberDeclarationSyntax enumMemberDeclarationSyntax)
            {
                return this.PrintEnumMemberDeclarationSyntax(enumMemberDeclarationSyntax);
            }
            else if (syntaxNode is EqualsValueClauseSyntax equalsValueClauseSyntax)
            {
                return this.PrintEqualsValueClauseSyntax(equalsValueClauseSyntax);
            }
            else if (syntaxNode is EventDeclarationSyntax eventDeclarationSyntax)
            {
                return this.PrintEventDeclarationSyntax(eventDeclarationSyntax);
            }
            else if (syntaxNode is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
            {
                return this.PrintEventFieldDeclarationSyntax(eventFieldDeclarationSyntax);
            }
            else if (syntaxNode is ExpressionStatementSyntax expressionStatementSyntax)
            {
                return this.PrintExpressionStatementSyntax(expressionStatementSyntax);
            }
            else if (syntaxNode is ExternAliasDirectiveSyntax externAliasDirectiveSyntax)
            {
                return this.PrintExternAliasDirectiveSyntax(externAliasDirectiveSyntax);
            }
            else if (syntaxNode is FieldDeclarationSyntax fieldDeclarationSyntax)
            {
                return this.PrintFieldDeclarationSyntax(fieldDeclarationSyntax);
            }
            else if (syntaxNode is FinallyClauseSyntax finallyClauseSyntax)
            {
                return this.PrintFinallyClauseSyntax(finallyClauseSyntax);
            }
            else if (syntaxNode is FixedStatementSyntax fixedStatementSyntax)
            {
                return this.PrintFixedStatementSyntax(fixedStatementSyntax);
            }
            else if (syntaxNode is ForEachStatementSyntax forEachStatementSyntax)
            {
                return this.PrintForEachStatementSyntax(forEachStatementSyntax);
            }
            else if (syntaxNode is ForStatementSyntax forStatementSyntax)
            {
                return this.PrintForStatementSyntax(forStatementSyntax);
            }
            else if (syntaxNode is FromClauseSyntax fromClauseSyntax)
            {
                return this.PrintFromClauseSyntax(fromClauseSyntax);
            }
            else if (syntaxNode is GenericNameSyntax genericNameSyntax)
            {
                return this.PrintGenericNameSyntax(genericNameSyntax);
            }
            else if (syntaxNode is GotoStatementSyntax gotoStatementSyntax)
            {
                return this.PrintGotoStatementSyntax(gotoStatementSyntax);
            }
            else if (syntaxNode is GroupClauseSyntax groupClauseSyntax)
            {
                return this.PrintGroupClauseSyntax(groupClauseSyntax);
            }
            else if (syntaxNode is IdentifierNameSyntax identifierNameSyntax)
            {
                return this.PrintIdentifierNameSyntax(identifierNameSyntax);
            }
            else if (syntaxNode is IfStatementSyntax ifStatementSyntax)
            {
                return this.PrintIfStatementSyntax(ifStatementSyntax);
            }
            else if (syntaxNode is ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax)
            {
                return this.PrintImplicitArrayCreationExpressionSyntax(implicitArrayCreationExpressionSyntax);
            }
            else if (syntaxNode is ImplicitElementAccessSyntax implicitElementAccessSyntax)
            {
                return this.PrintImplicitElementAccessSyntax(implicitElementAccessSyntax);
            }
            else if (syntaxNode is ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax)
            {
                return this.PrintImplicitStackAllocArrayCreationExpressionSyntax(implicitStackAllocArrayCreationExpressionSyntax);
            }
            else if (syntaxNode is IndexerDeclarationSyntax indexerDeclarationSyntax)
            {
                return this.PrintIndexerDeclarationSyntax(indexerDeclarationSyntax);
            }
            else if (syntaxNode is InitializerExpressionSyntax initializerExpressionSyntax)
            {
                return this.PrintInitializerExpressionSyntax(initializerExpressionSyntax);
            }
            else if (syntaxNode is InterfaceDeclarationSyntax interfaceDeclarationSyntax)
            {
                return this.PrintInterfaceDeclarationSyntax(interfaceDeclarationSyntax);
            }
            else if (syntaxNode is InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax)
            {
                return this.PrintInterpolatedStringExpressionSyntax(interpolatedStringExpressionSyntax);
            }
            else if (syntaxNode is InterpolatedStringTextSyntax interpolatedStringTextSyntax)
            {
                return this.PrintInterpolatedStringTextSyntax(interpolatedStringTextSyntax);
            }
            else if (syntaxNode is InterpolationSyntax interpolationSyntax)
            {
                return this.PrintInterpolationSyntax(interpolationSyntax);
            }
            else if (syntaxNode is InvocationExpressionSyntax invocationExpressionSyntax)
            {
                return this.PrintInvocationExpressionSyntax(invocationExpressionSyntax);
            }
            else if (syntaxNode is IsPatternExpressionSyntax isPatternExpressionSyntax)
            {
                return this.PrintIsPatternExpressionSyntax(isPatternExpressionSyntax);
            }
            else if (syntaxNode is JoinClauseSyntax joinClauseSyntax)
            {
                return this.PrintJoinClauseSyntax(joinClauseSyntax);
            }
            else if (syntaxNode is LabeledStatementSyntax labeledStatementSyntax)
            {
                return this.PrintLabeledStatementSyntax(labeledStatementSyntax);
            }
            else if (syntaxNode is LetClauseSyntax letClauseSyntax)
            {
                return this.PrintLetClauseSyntax(letClauseSyntax);
            }
            else if (syntaxNode is LiteralExpressionSyntax literalExpressionSyntax)
            {
                return this.PrintLiteralExpressionSyntax(literalExpressionSyntax);
            }
            else if (syntaxNode is LocalDeclarationStatementSyntax localDeclarationStatementSyntax)
            {
                return this.PrintLocalDeclarationStatementSyntax(localDeclarationStatementSyntax);
            }
            else if (syntaxNode is LocalFunctionStatementSyntax localFunctionStatementSyntax)
            {
                return this.PrintLocalFunctionStatementSyntax(localFunctionStatementSyntax);
            }
            else if (syntaxNode is LockStatementSyntax lockStatementSyntax)
            {
                return this.PrintLockStatementSyntax(lockStatementSyntax);
            }
            else if (syntaxNode is MakeRefExpressionSyntax makeRefExpressionSyntax)
            {
                return this.PrintMakeRefExpressionSyntax(makeRefExpressionSyntax);
            }
            else if (syntaxNode is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
            {
                return this.PrintMemberAccessExpressionSyntax(memberAccessExpressionSyntax);
            }
            else if (syntaxNode is MemberBindingExpressionSyntax memberBindingExpressionSyntax)
            {
                return this.PrintMemberBindingExpressionSyntax(memberBindingExpressionSyntax);
            }
            else if (syntaxNode is MethodDeclarationSyntax methodDeclarationSyntax)
            {
                return this.PrintMethodDeclarationSyntax(methodDeclarationSyntax);
            }
            else if (syntaxNode is NameColonSyntax nameColonSyntax)
            {
                return this.PrintNameColonSyntax(nameColonSyntax);
            }
            else if (syntaxNode is NameEqualsSyntax nameEqualsSyntax)
            {
                return this.PrintNameEqualsSyntax(nameEqualsSyntax);
            }
            else if (syntaxNode is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
            {
                return this.PrintNamespaceDeclarationSyntax(namespaceDeclarationSyntax);
            }
            else if (syntaxNode is NullableTypeSyntax nullableTypeSyntax)
            {
                return this.PrintNullableTypeSyntax(nullableTypeSyntax);
            }
            else if (syntaxNode is ObjectCreationExpressionSyntax objectCreationExpressionSyntax)
            {
                return this.PrintObjectCreationExpressionSyntax(objectCreationExpressionSyntax);
            }
            else if (syntaxNode is OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax)
            {
                return this.PrintOmittedArraySizeExpressionSyntax(omittedArraySizeExpressionSyntax);
            }
            else if (syntaxNode is OmittedTypeArgumentSyntax omittedTypeArgumentSyntax)
            {
                return this.PrintOmittedTypeArgumentSyntax(omittedTypeArgumentSyntax);
            }
            else if (syntaxNode is OperatorDeclarationSyntax operatorDeclarationSyntax)
            {
                return this.PrintOperatorDeclarationSyntax(operatorDeclarationSyntax);
            }
            else if (syntaxNode is OrderByClauseSyntax orderByClauseSyntax)
            {
                return this.PrintOrderByClauseSyntax(orderByClauseSyntax);
            }
            else if (syntaxNode is ParameterListSyntax parameterListSyntax)
            {
                return this.PrintParameterListSyntax(parameterListSyntax);
            }
            else if (syntaxNode is ParameterSyntax parameterSyntax)
            {
                return this.PrintParameterSyntax(parameterSyntax);
            }
            else if (syntaxNode is ParenthesizedExpressionSyntax parenthesizedExpressionSyntax)
            {
                return this.PrintParenthesizedExpressionSyntax(parenthesizedExpressionSyntax);
            }
            else if (syntaxNode is ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax)
            {
                return this.PrintParenthesizedLambdaExpressionSyntax(parenthesizedLambdaExpressionSyntax);
            }
            else if (syntaxNode is PointerTypeSyntax pointerTypeSyntax)
            {
                return this.PrintPointerTypeSyntax(pointerTypeSyntax);
            }
            else if (syntaxNode is PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax)
            {
                return this.PrintPostfixUnaryExpressionSyntax(postfixUnaryExpressionSyntax);
            }
            else if (syntaxNode is PredefinedTypeSyntax predefinedTypeSyntax)
            {
                return this.PrintPredefinedTypeSyntax(predefinedTypeSyntax);
            }
            else if (syntaxNode is PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax)
            {
                return this.PrintPrefixUnaryExpressionSyntax(prefixUnaryExpressionSyntax);
            }
            else if (syntaxNode is PropertyDeclarationSyntax propertyDeclarationSyntax)
            {
                return this.PrintPropertyDeclarationSyntax(propertyDeclarationSyntax);
            }
            else if (syntaxNode is QualifiedNameSyntax qualifiedNameSyntax)
            {
                return this.PrintQualifiedNameSyntax(qualifiedNameSyntax);
            }
            else if (syntaxNode is QueryBodySyntax queryBodySyntax)
            {
                return this.PrintQueryBodySyntax(queryBodySyntax);
            }
            else if (syntaxNode is QueryContinuationSyntax queryContinuationSyntax)
            {
                return this.PrintQueryContinuationSyntax(queryContinuationSyntax);
            }
            else if (syntaxNode is QueryExpressionSyntax queryExpressionSyntax)
            {
                return this.PrintQueryExpressionSyntax(queryExpressionSyntax);
            }
            else if (syntaxNode is RangeExpressionSyntax rangeExpressionSyntax)
            {
                return this.PrintRangeExpressionSyntax(rangeExpressionSyntax);
            }
            else if (syntaxNode is RecursivePatternSyntax recursivePatternSyntax)
            {
                return this.PrintRecursivePatternSyntax(recursivePatternSyntax);
            }
            else if (syntaxNode is RefExpressionSyntax refExpressionSyntax)
            {
                return this.PrintRefExpressionSyntax(refExpressionSyntax);
            }
            else if (syntaxNode is RefTypeExpressionSyntax refTypeExpressionSyntax)
            {
                return this.PrintRefTypeExpressionSyntax(refTypeExpressionSyntax);
            }
            else if (syntaxNode is RefTypeSyntax refTypeSyntax)
            {
                return this.PrintRefTypeSyntax(refTypeSyntax);
            }
            else if (syntaxNode is RefValueExpressionSyntax refValueExpressionSyntax)
            {
                return this.PrintRefValueExpressionSyntax(refValueExpressionSyntax);
            }
            else if (syntaxNode is ReturnStatementSyntax returnStatementSyntax)
            {
                return this.PrintReturnStatementSyntax(returnStatementSyntax);
            }
            else if (syntaxNode is SelectClauseSyntax selectClauseSyntax)
            {
                return this.PrintSelectClauseSyntax(selectClauseSyntax);
            }
            else if (syntaxNode is SimpleBaseTypeSyntax simpleBaseTypeSyntax)
            {
                return this.PrintSimpleBaseTypeSyntax(simpleBaseTypeSyntax);
            }
            else if (syntaxNode is SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax)
            {
                return this.PrintSimpleLambdaExpressionSyntax(simpleLambdaExpressionSyntax);
            }
            else if (syntaxNode is SingleVariableDesignationSyntax singleVariableDesignationSyntax)
            {
                return this.PrintSingleVariableDesignationSyntax(singleVariableDesignationSyntax);
            }
            else if (syntaxNode is SizeOfExpressionSyntax sizeOfExpressionSyntax)
            {
                return this.PrintSizeOfExpressionSyntax(sizeOfExpressionSyntax);
            }
            else if (syntaxNode is StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax)
            {
                return this.PrintStackAllocArrayCreationExpressionSyntax(stackAllocArrayCreationExpressionSyntax);
            }
            else if (syntaxNode is StructDeclarationSyntax structDeclarationSyntax)
            {
                return this.PrintStructDeclarationSyntax(structDeclarationSyntax);
            }
            else if (syntaxNode is SwitchExpressionSyntax switchExpressionSyntax)
            {
                return this.PrintSwitchExpressionSyntax(switchExpressionSyntax);
            }
            else if (syntaxNode is SwitchSectionSyntax switchSectionSyntax)
            {
                return this.PrintSwitchSectionSyntax(switchSectionSyntax);
            }
            else if (syntaxNode is SwitchStatementSyntax switchStatementSyntax)
            {
                return this.PrintSwitchStatementSyntax(switchStatementSyntax);
            }
            else if (syntaxNode is ThisExpressionSyntax thisExpressionSyntax)
            {
                return this.PrintThisExpressionSyntax(thisExpressionSyntax);
            }
            else if (syntaxNode is ThrowExpressionSyntax throwExpressionSyntax)
            {
                return this.PrintThrowExpressionSyntax(throwExpressionSyntax);
            }
            else if (syntaxNode is ThrowStatementSyntax throwStatementSyntax)
            {
                return this.PrintThrowStatementSyntax(throwStatementSyntax);
            }
            else if (syntaxNode is TryStatementSyntax tryStatementSyntax)
            {
                return this.PrintTryStatementSyntax(tryStatementSyntax);
            }
            else if (syntaxNode is TupleElementSyntax tupleElementSyntax)
            {
                return this.PrintTupleElementSyntax(tupleElementSyntax);
            }
            else if (syntaxNode is TupleExpressionSyntax tupleExpressionSyntax)
            {
                return this.PrintTupleExpressionSyntax(tupleExpressionSyntax);
            }
            else if (syntaxNode is TupleTypeSyntax tupleTypeSyntax)
            {
                return this.PrintTupleTypeSyntax(tupleTypeSyntax);
            }
            else if (syntaxNode is TypeArgumentListSyntax typeArgumentListSyntax)
            {
                return this.PrintTypeArgumentListSyntax(typeArgumentListSyntax);
            }
            else if (syntaxNode is TypeConstraintSyntax typeConstraintSyntax)
            {
                return this.PrintTypeConstraintSyntax(typeConstraintSyntax);
            }
            else if (syntaxNode is TypeOfExpressionSyntax typeOfExpressionSyntax)
            {
                return this.PrintTypeOfExpressionSyntax(typeOfExpressionSyntax);
            }
            else if (syntaxNode is TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax)
            {
                return this.PrintTypeParameterConstraintClauseSyntax(typeParameterConstraintClauseSyntax);
            }
            else if (syntaxNode is TypeParameterListSyntax typeParameterListSyntax)
            {
                return this.PrintTypeParameterListSyntax(typeParameterListSyntax);
            }
            else if (syntaxNode is TypeParameterSyntax typeParameterSyntax)
            {
                return this.PrintTypeParameterSyntax(typeParameterSyntax);
            }
            else if (syntaxNode is UnsafeStatementSyntax unsafeStatementSyntax)
            {
                return this.PrintUnsafeStatementSyntax(unsafeStatementSyntax);
            }
            else if (syntaxNode is UsingDirectiveSyntax usingDirectiveSyntax)
            {
                return this.PrintUsingDirectiveSyntax(usingDirectiveSyntax);
            }
            else if (syntaxNode is UsingStatementSyntax usingStatementSyntax)
            {
                return this.PrintUsingStatementSyntax(usingStatementSyntax);
            }
            else if (syntaxNode is VariableDeclarationSyntax variableDeclarationSyntax)
            {
                return this.PrintVariableDeclarationSyntax(variableDeclarationSyntax);
            }
            else if (syntaxNode is VariableDeclaratorSyntax variableDeclaratorSyntax)
            {
                return this.PrintVariableDeclaratorSyntax(variableDeclaratorSyntax);
            }
            else if (syntaxNode is VarPatternSyntax varPatternSyntax)
            {
                return this.PrintVarPatternSyntax(varPatternSyntax);
            }
            else if (syntaxNode is WhenClauseSyntax whenClauseSyntax)
            {
                return this.PrintWhenClauseSyntax(whenClauseSyntax);
            }
            else if (syntaxNode is WhereClauseSyntax whereClauseSyntax)
            {
                return this.PrintWhereClauseSyntax(whereClauseSyntax);
            }
            else if (syntaxNode is WhileStatementSyntax whileStatementSyntax)
            {
                return this.PrintWhileStatementSyntax(whileStatementSyntax);
            }
            else if (syntaxNode is YieldStatementSyntax yieldStatementSyntax)
            {
                return this.PrintYieldStatementSyntax(yieldStatementSyntax);
            }

            throw new Exception("Can't handle " + syntaxNode.GetType().Name);
        }
    }
}
