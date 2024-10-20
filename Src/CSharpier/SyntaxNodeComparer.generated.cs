#pragma warning disable CS0168
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    internal partial class SyntaxNodeComparer
    {
        private CompareResult Compare(
            (SyntaxNode originalNode, SyntaxNode originalParent) original,
            (SyntaxNode formattedNode, SyntaxNode formattedParent) formatted
        )
        {
            var (originalNode, originalParent) = original;
            var (formattedNode, formattedParent) = formatted;
            if (originalNode == null && formattedNode == null)
            {
                return Equal;
            }

            if (originalNode == null || formattedNode == null)
            {
                return NotEqual(originalParent, formattedParent);
            }

            var type = originalNode?.GetType();
            if (type != formattedNode?.GetType())
            {
                return NotEqual(originalNode, formattedNode);
            }

            if (originalNode.RawKind != formattedNode.RawKind)
            {
                return NotEqual(originalNode, formattedNode);
            }

            switch (originalNode)
            {
             case AccessorDeclarationSyntax accessorDeclarationSyntax:
                 return this.CompareAccessorDeclarationSyntax(accessorDeclarationSyntax, formattedNode as AccessorDeclarationSyntax);
             case AccessorListSyntax accessorListSyntax:
                 return this.CompareAccessorListSyntax(accessorListSyntax, formattedNode as AccessorListSyntax);
             case AliasQualifiedNameSyntax aliasQualifiedNameSyntax:
                 return this.CompareAliasQualifiedNameSyntax(aliasQualifiedNameSyntax, formattedNode as AliasQualifiedNameSyntax);
             case AllowsConstraintClauseSyntax allowsConstraintClauseSyntax:
                 return this.CompareAllowsConstraintClauseSyntax(allowsConstraintClauseSyntax, formattedNode as AllowsConstraintClauseSyntax);
             case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax:
                 return this.CompareAnonymousMethodExpressionSyntax(anonymousMethodExpressionSyntax, formattedNode as AnonymousMethodExpressionSyntax);
             case AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax:
                 return this.CompareAnonymousObjectCreationExpressionSyntax(anonymousObjectCreationExpressionSyntax, formattedNode as AnonymousObjectCreationExpressionSyntax);
             case AnonymousObjectMemberDeclaratorSyntax anonymousObjectMemberDeclaratorSyntax:
                 return this.CompareAnonymousObjectMemberDeclaratorSyntax(anonymousObjectMemberDeclaratorSyntax, formattedNode as AnonymousObjectMemberDeclaratorSyntax);
             case ArgumentListSyntax argumentListSyntax:
                 return this.CompareArgumentListSyntax(argumentListSyntax, formattedNode as ArgumentListSyntax);
             case ArgumentSyntax argumentSyntax:
                 return this.CompareArgumentSyntax(argumentSyntax, formattedNode as ArgumentSyntax);
             case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax:
                 return this.CompareArrayCreationExpressionSyntax(arrayCreationExpressionSyntax, formattedNode as ArrayCreationExpressionSyntax);
             case ArrayRankSpecifierSyntax arrayRankSpecifierSyntax:
                 return this.CompareArrayRankSpecifierSyntax(arrayRankSpecifierSyntax, formattedNode as ArrayRankSpecifierSyntax);
             case ArrayTypeSyntax arrayTypeSyntax:
                 return this.CompareArrayTypeSyntax(arrayTypeSyntax, formattedNode as ArrayTypeSyntax);
             case ArrowExpressionClauseSyntax arrowExpressionClauseSyntax:
                 return this.CompareArrowExpressionClauseSyntax(arrowExpressionClauseSyntax, formattedNode as ArrowExpressionClauseSyntax);
             case AssignmentExpressionSyntax assignmentExpressionSyntax:
                 return this.CompareAssignmentExpressionSyntax(assignmentExpressionSyntax, formattedNode as AssignmentExpressionSyntax);
             case AttributeArgumentListSyntax attributeArgumentListSyntax:
                 return this.CompareAttributeArgumentListSyntax(attributeArgumentListSyntax, formattedNode as AttributeArgumentListSyntax);
             case AttributeArgumentSyntax attributeArgumentSyntax:
                 return this.CompareAttributeArgumentSyntax(attributeArgumentSyntax, formattedNode as AttributeArgumentSyntax);
             case AttributeListSyntax attributeListSyntax:
                 return this.CompareAttributeListSyntax(attributeListSyntax, formattedNode as AttributeListSyntax);
             case AttributeSyntax attributeSyntax:
                 return this.CompareAttributeSyntax(attributeSyntax, formattedNode as AttributeSyntax);
             case AttributeTargetSpecifierSyntax attributeTargetSpecifierSyntax:
                 return this.CompareAttributeTargetSpecifierSyntax(attributeTargetSpecifierSyntax, formattedNode as AttributeTargetSpecifierSyntax);
             case AwaitExpressionSyntax awaitExpressionSyntax:
                 return this.CompareAwaitExpressionSyntax(awaitExpressionSyntax, formattedNode as AwaitExpressionSyntax);
             case BadDirectiveTriviaSyntax badDirectiveTriviaSyntax:
                 return this.CompareBadDirectiveTriviaSyntax(badDirectiveTriviaSyntax, formattedNode as BadDirectiveTriviaSyntax);
             case BaseExpressionSyntax baseExpressionSyntax:
                 return this.CompareBaseExpressionSyntax(baseExpressionSyntax, formattedNode as BaseExpressionSyntax);
             case BaseListSyntax baseListSyntax:
                 return this.CompareBaseListSyntax(baseListSyntax, formattedNode as BaseListSyntax);
             case BinaryExpressionSyntax binaryExpressionSyntax:
                 return this.CompareBinaryExpressionSyntax(binaryExpressionSyntax, formattedNode as BinaryExpressionSyntax);
             case BinaryPatternSyntax binaryPatternSyntax:
                 return this.CompareBinaryPatternSyntax(binaryPatternSyntax, formattedNode as BinaryPatternSyntax);
             case BlockSyntax blockSyntax:
                 return this.CompareBlockSyntax(blockSyntax, formattedNode as BlockSyntax);
             case BracketedArgumentListSyntax bracketedArgumentListSyntax:
                 return this.CompareBracketedArgumentListSyntax(bracketedArgumentListSyntax, formattedNode as BracketedArgumentListSyntax);
             case BracketedParameterListSyntax bracketedParameterListSyntax:
                 return this.CompareBracketedParameterListSyntax(bracketedParameterListSyntax, formattedNode as BracketedParameterListSyntax);
             case BreakStatementSyntax breakStatementSyntax:
                 return this.CompareBreakStatementSyntax(breakStatementSyntax, formattedNode as BreakStatementSyntax);
             case CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax:
                 return this.CompareCasePatternSwitchLabelSyntax(casePatternSwitchLabelSyntax, formattedNode as CasePatternSwitchLabelSyntax);
             case CaseSwitchLabelSyntax caseSwitchLabelSyntax:
                 return this.CompareCaseSwitchLabelSyntax(caseSwitchLabelSyntax, formattedNode as CaseSwitchLabelSyntax);
             case CastExpressionSyntax castExpressionSyntax:
                 return this.CompareCastExpressionSyntax(castExpressionSyntax, formattedNode as CastExpressionSyntax);
             case CatchClauseSyntax catchClauseSyntax:
                 return this.CompareCatchClauseSyntax(catchClauseSyntax, formattedNode as CatchClauseSyntax);
             case CatchDeclarationSyntax catchDeclarationSyntax:
                 return this.CompareCatchDeclarationSyntax(catchDeclarationSyntax, formattedNode as CatchDeclarationSyntax);
             case CatchFilterClauseSyntax catchFilterClauseSyntax:
                 return this.CompareCatchFilterClauseSyntax(catchFilterClauseSyntax, formattedNode as CatchFilterClauseSyntax);
             case CheckedExpressionSyntax checkedExpressionSyntax:
                 return this.CompareCheckedExpressionSyntax(checkedExpressionSyntax, formattedNode as CheckedExpressionSyntax);
             case CheckedStatementSyntax checkedStatementSyntax:
                 return this.CompareCheckedStatementSyntax(checkedStatementSyntax, formattedNode as CheckedStatementSyntax);
             case ClassDeclarationSyntax classDeclarationSyntax:
                 return this.CompareClassDeclarationSyntax(classDeclarationSyntax, formattedNode as ClassDeclarationSyntax);
             case ClassOrStructConstraintSyntax classOrStructConstraintSyntax:
                 return this.CompareClassOrStructConstraintSyntax(classOrStructConstraintSyntax, formattedNode as ClassOrStructConstraintSyntax);
             case CollectionExpressionSyntax collectionExpressionSyntax:
                 return this.CompareCollectionExpressionSyntax(collectionExpressionSyntax, formattedNode as CollectionExpressionSyntax);
             case CompilationUnitSyntax compilationUnitSyntax:
                 return this.CompareCompilationUnitSyntax(compilationUnitSyntax, formattedNode as CompilationUnitSyntax);
             case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax:
                 return this.CompareConditionalAccessExpressionSyntax(conditionalAccessExpressionSyntax, formattedNode as ConditionalAccessExpressionSyntax);
             case ConditionalExpressionSyntax conditionalExpressionSyntax:
                 return this.CompareConditionalExpressionSyntax(conditionalExpressionSyntax, formattedNode as ConditionalExpressionSyntax);
             case ConstantPatternSyntax constantPatternSyntax:
                 return this.CompareConstantPatternSyntax(constantPatternSyntax, formattedNode as ConstantPatternSyntax);
             case ConstructorConstraintSyntax constructorConstraintSyntax:
                 return this.CompareConstructorConstraintSyntax(constructorConstraintSyntax, formattedNode as ConstructorConstraintSyntax);
             case ConstructorDeclarationSyntax constructorDeclarationSyntax:
                 return this.CompareConstructorDeclarationSyntax(constructorDeclarationSyntax, formattedNode as ConstructorDeclarationSyntax);
             case ConstructorInitializerSyntax constructorInitializerSyntax:
                 return this.CompareConstructorInitializerSyntax(constructorInitializerSyntax, formattedNode as ConstructorInitializerSyntax);
             case ContinueStatementSyntax continueStatementSyntax:
                 return this.CompareContinueStatementSyntax(continueStatementSyntax, formattedNode as ContinueStatementSyntax);
             case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax:
                 return this.CompareConversionOperatorDeclarationSyntax(conversionOperatorDeclarationSyntax, formattedNode as ConversionOperatorDeclarationSyntax);
             case ConversionOperatorMemberCrefSyntax conversionOperatorMemberCrefSyntax:
                 return this.CompareConversionOperatorMemberCrefSyntax(conversionOperatorMemberCrefSyntax, formattedNode as ConversionOperatorMemberCrefSyntax);
             case CrefBracketedParameterListSyntax crefBracketedParameterListSyntax:
                 return this.CompareCrefBracketedParameterListSyntax(crefBracketedParameterListSyntax, formattedNode as CrefBracketedParameterListSyntax);
             case CrefParameterListSyntax crefParameterListSyntax:
                 return this.CompareCrefParameterListSyntax(crefParameterListSyntax, formattedNode as CrefParameterListSyntax);
             case CrefParameterSyntax crefParameterSyntax:
                 return this.CompareCrefParameterSyntax(crefParameterSyntax, formattedNode as CrefParameterSyntax);
             case DeclarationExpressionSyntax declarationExpressionSyntax:
                 return this.CompareDeclarationExpressionSyntax(declarationExpressionSyntax, formattedNode as DeclarationExpressionSyntax);
             case DeclarationPatternSyntax declarationPatternSyntax:
                 return this.CompareDeclarationPatternSyntax(declarationPatternSyntax, formattedNode as DeclarationPatternSyntax);
             case DefaultConstraintSyntax defaultConstraintSyntax:
                 return this.CompareDefaultConstraintSyntax(defaultConstraintSyntax, formattedNode as DefaultConstraintSyntax);
             case DefaultExpressionSyntax defaultExpressionSyntax:
                 return this.CompareDefaultExpressionSyntax(defaultExpressionSyntax, formattedNode as DefaultExpressionSyntax);
             case DefaultSwitchLabelSyntax defaultSwitchLabelSyntax:
                 return this.CompareDefaultSwitchLabelSyntax(defaultSwitchLabelSyntax, formattedNode as DefaultSwitchLabelSyntax);
             case DefineDirectiveTriviaSyntax defineDirectiveTriviaSyntax:
                 return this.CompareDefineDirectiveTriviaSyntax(defineDirectiveTriviaSyntax, formattedNode as DefineDirectiveTriviaSyntax);
             case DelegateDeclarationSyntax delegateDeclarationSyntax:
                 return this.CompareDelegateDeclarationSyntax(delegateDeclarationSyntax, formattedNode as DelegateDeclarationSyntax);
             case DestructorDeclarationSyntax destructorDeclarationSyntax:
                 return this.CompareDestructorDeclarationSyntax(destructorDeclarationSyntax, formattedNode as DestructorDeclarationSyntax);
             case DiscardDesignationSyntax discardDesignationSyntax:
                 return this.CompareDiscardDesignationSyntax(discardDesignationSyntax, formattedNode as DiscardDesignationSyntax);
             case DiscardPatternSyntax discardPatternSyntax:
                 return this.CompareDiscardPatternSyntax(discardPatternSyntax, formattedNode as DiscardPatternSyntax);
             case DocumentationCommentTriviaSyntax documentationCommentTriviaSyntax:
                 return this.CompareDocumentationCommentTriviaSyntax(documentationCommentTriviaSyntax, formattedNode as DocumentationCommentTriviaSyntax);
             case DoStatementSyntax doStatementSyntax:
                 return this.CompareDoStatementSyntax(doStatementSyntax, formattedNode as DoStatementSyntax);
             case ElementAccessExpressionSyntax elementAccessExpressionSyntax:
                 return this.CompareElementAccessExpressionSyntax(elementAccessExpressionSyntax, formattedNode as ElementAccessExpressionSyntax);
             case ElementBindingExpressionSyntax elementBindingExpressionSyntax:
                 return this.CompareElementBindingExpressionSyntax(elementBindingExpressionSyntax, formattedNode as ElementBindingExpressionSyntax);
             case ElifDirectiveTriviaSyntax elifDirectiveTriviaSyntax:
                 return this.CompareElifDirectiveTriviaSyntax(elifDirectiveTriviaSyntax, formattedNode as ElifDirectiveTriviaSyntax);
             case ElseClauseSyntax elseClauseSyntax:
                 return this.CompareElseClauseSyntax(elseClauseSyntax, formattedNode as ElseClauseSyntax);
             case ElseDirectiveTriviaSyntax elseDirectiveTriviaSyntax:
                 return this.CompareElseDirectiveTriviaSyntax(elseDirectiveTriviaSyntax, formattedNode as ElseDirectiveTriviaSyntax);
             case EmptyStatementSyntax emptyStatementSyntax:
                 return this.CompareEmptyStatementSyntax(emptyStatementSyntax, formattedNode as EmptyStatementSyntax);
             case EndIfDirectiveTriviaSyntax endIfDirectiveTriviaSyntax:
                 return this.CompareEndIfDirectiveTriviaSyntax(endIfDirectiveTriviaSyntax, formattedNode as EndIfDirectiveTriviaSyntax);
             case EndRegionDirectiveTriviaSyntax endRegionDirectiveTriviaSyntax:
                 return this.CompareEndRegionDirectiveTriviaSyntax(endRegionDirectiveTriviaSyntax, formattedNode as EndRegionDirectiveTriviaSyntax);
             case EnumDeclarationSyntax enumDeclarationSyntax:
                 return this.CompareEnumDeclarationSyntax(enumDeclarationSyntax, formattedNode as EnumDeclarationSyntax);
             case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax:
                 return this.CompareEnumMemberDeclarationSyntax(enumMemberDeclarationSyntax, formattedNode as EnumMemberDeclarationSyntax);
             case EqualsValueClauseSyntax equalsValueClauseSyntax:
                 return this.CompareEqualsValueClauseSyntax(equalsValueClauseSyntax, formattedNode as EqualsValueClauseSyntax);
             case ErrorDirectiveTriviaSyntax errorDirectiveTriviaSyntax:
                 return this.CompareErrorDirectiveTriviaSyntax(errorDirectiveTriviaSyntax, formattedNode as ErrorDirectiveTriviaSyntax);
             case EventDeclarationSyntax eventDeclarationSyntax:
                 return this.CompareEventDeclarationSyntax(eventDeclarationSyntax, formattedNode as EventDeclarationSyntax);
             case EventFieldDeclarationSyntax eventFieldDeclarationSyntax:
                 return this.CompareEventFieldDeclarationSyntax(eventFieldDeclarationSyntax, formattedNode as EventFieldDeclarationSyntax);
             case ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax:
                 return this.CompareExplicitInterfaceSpecifierSyntax(explicitInterfaceSpecifierSyntax, formattedNode as ExplicitInterfaceSpecifierSyntax);
             case ExpressionColonSyntax expressionColonSyntax:
                 return this.CompareExpressionColonSyntax(expressionColonSyntax, formattedNode as ExpressionColonSyntax);
             case ExpressionElementSyntax expressionElementSyntax:
                 return this.CompareExpressionElementSyntax(expressionElementSyntax, formattedNode as ExpressionElementSyntax);
             case ExpressionStatementSyntax expressionStatementSyntax:
                 return this.CompareExpressionStatementSyntax(expressionStatementSyntax, formattedNode as ExpressionStatementSyntax);
             case ExternAliasDirectiveSyntax externAliasDirectiveSyntax:
                 return this.CompareExternAliasDirectiveSyntax(externAliasDirectiveSyntax, formattedNode as ExternAliasDirectiveSyntax);
             case FieldDeclarationSyntax fieldDeclarationSyntax:
                 return this.CompareFieldDeclarationSyntax(fieldDeclarationSyntax, formattedNode as FieldDeclarationSyntax);
             case FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclarationSyntax:
                 return this.CompareFileScopedNamespaceDeclarationSyntax(fileScopedNamespaceDeclarationSyntax, formattedNode as FileScopedNamespaceDeclarationSyntax);
             case FinallyClauseSyntax finallyClauseSyntax:
                 return this.CompareFinallyClauseSyntax(finallyClauseSyntax, formattedNode as FinallyClauseSyntax);
             case FixedStatementSyntax fixedStatementSyntax:
                 return this.CompareFixedStatementSyntax(fixedStatementSyntax, formattedNode as FixedStatementSyntax);
             case ForEachStatementSyntax forEachStatementSyntax:
                 return this.CompareForEachStatementSyntax(forEachStatementSyntax, formattedNode as ForEachStatementSyntax);
             case ForEachVariableStatementSyntax forEachVariableStatementSyntax:
                 return this.CompareForEachVariableStatementSyntax(forEachVariableStatementSyntax, formattedNode as ForEachVariableStatementSyntax);
             case ForStatementSyntax forStatementSyntax:
                 return this.CompareForStatementSyntax(forStatementSyntax, formattedNode as ForStatementSyntax);
             case FromClauseSyntax fromClauseSyntax:
                 return this.CompareFromClauseSyntax(fromClauseSyntax, formattedNode as FromClauseSyntax);
             case FunctionPointerCallingConventionSyntax functionPointerCallingConventionSyntax:
                 return this.CompareFunctionPointerCallingConventionSyntax(functionPointerCallingConventionSyntax, formattedNode as FunctionPointerCallingConventionSyntax);
             case FunctionPointerParameterListSyntax functionPointerParameterListSyntax:
                 return this.CompareFunctionPointerParameterListSyntax(functionPointerParameterListSyntax, formattedNode as FunctionPointerParameterListSyntax);
             case FunctionPointerParameterSyntax functionPointerParameterSyntax:
                 return this.CompareFunctionPointerParameterSyntax(functionPointerParameterSyntax, formattedNode as FunctionPointerParameterSyntax);
             case FunctionPointerTypeSyntax functionPointerTypeSyntax:
                 return this.CompareFunctionPointerTypeSyntax(functionPointerTypeSyntax, formattedNode as FunctionPointerTypeSyntax);
             case FunctionPointerUnmanagedCallingConventionListSyntax functionPointerUnmanagedCallingConventionListSyntax:
                 return this.CompareFunctionPointerUnmanagedCallingConventionListSyntax(functionPointerUnmanagedCallingConventionListSyntax, formattedNode as FunctionPointerUnmanagedCallingConventionListSyntax);
             case FunctionPointerUnmanagedCallingConventionSyntax functionPointerUnmanagedCallingConventionSyntax:
                 return this.CompareFunctionPointerUnmanagedCallingConventionSyntax(functionPointerUnmanagedCallingConventionSyntax, formattedNode as FunctionPointerUnmanagedCallingConventionSyntax);
             case GenericNameSyntax genericNameSyntax:
                 return this.CompareGenericNameSyntax(genericNameSyntax, formattedNode as GenericNameSyntax);
             case GlobalStatementSyntax globalStatementSyntax:
                 return this.CompareGlobalStatementSyntax(globalStatementSyntax, formattedNode as GlobalStatementSyntax);
             case GotoStatementSyntax gotoStatementSyntax:
                 return this.CompareGotoStatementSyntax(gotoStatementSyntax, formattedNode as GotoStatementSyntax);
             case GroupClauseSyntax groupClauseSyntax:
                 return this.CompareGroupClauseSyntax(groupClauseSyntax, formattedNode as GroupClauseSyntax);
             case IdentifierNameSyntax identifierNameSyntax:
                 return this.CompareIdentifierNameSyntax(identifierNameSyntax, formattedNode as IdentifierNameSyntax);
             case IfDirectiveTriviaSyntax ifDirectiveTriviaSyntax:
                 return this.CompareIfDirectiveTriviaSyntax(ifDirectiveTriviaSyntax, formattedNode as IfDirectiveTriviaSyntax);
             case IfStatementSyntax ifStatementSyntax:
                 return this.CompareIfStatementSyntax(ifStatementSyntax, formattedNode as IfStatementSyntax);
             case ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax:
                 return this.CompareImplicitArrayCreationExpressionSyntax(implicitArrayCreationExpressionSyntax, formattedNode as ImplicitArrayCreationExpressionSyntax);
             case ImplicitElementAccessSyntax implicitElementAccessSyntax:
                 return this.CompareImplicitElementAccessSyntax(implicitElementAccessSyntax, formattedNode as ImplicitElementAccessSyntax);
             case ImplicitObjectCreationExpressionSyntax implicitObjectCreationExpressionSyntax:
                 return this.CompareImplicitObjectCreationExpressionSyntax(implicitObjectCreationExpressionSyntax, formattedNode as ImplicitObjectCreationExpressionSyntax);
             case ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax:
                 return this.CompareImplicitStackAllocArrayCreationExpressionSyntax(implicitStackAllocArrayCreationExpressionSyntax, formattedNode as ImplicitStackAllocArrayCreationExpressionSyntax);
             case IncompleteMemberSyntax incompleteMemberSyntax:
                 return this.CompareIncompleteMemberSyntax(incompleteMemberSyntax, formattedNode as IncompleteMemberSyntax);
             case IndexerDeclarationSyntax indexerDeclarationSyntax:
                 return this.CompareIndexerDeclarationSyntax(indexerDeclarationSyntax, formattedNode as IndexerDeclarationSyntax);
             case IndexerMemberCrefSyntax indexerMemberCrefSyntax:
                 return this.CompareIndexerMemberCrefSyntax(indexerMemberCrefSyntax, formattedNode as IndexerMemberCrefSyntax);
             case InitializerExpressionSyntax initializerExpressionSyntax:
                 return this.CompareInitializerExpressionSyntax(initializerExpressionSyntax, formattedNode as InitializerExpressionSyntax);
             case InterfaceDeclarationSyntax interfaceDeclarationSyntax:
                 return this.CompareInterfaceDeclarationSyntax(interfaceDeclarationSyntax, formattedNode as InterfaceDeclarationSyntax);
             case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax:
                 return this.CompareInterpolatedStringExpressionSyntax(interpolatedStringExpressionSyntax, formattedNode as InterpolatedStringExpressionSyntax);
             case InterpolatedStringTextSyntax interpolatedStringTextSyntax:
                 return this.CompareInterpolatedStringTextSyntax(interpolatedStringTextSyntax, formattedNode as InterpolatedStringTextSyntax);
             case InterpolationAlignmentClauseSyntax interpolationAlignmentClauseSyntax:
                 return this.CompareInterpolationAlignmentClauseSyntax(interpolationAlignmentClauseSyntax, formattedNode as InterpolationAlignmentClauseSyntax);
             case InterpolationFormatClauseSyntax interpolationFormatClauseSyntax:
                 return this.CompareInterpolationFormatClauseSyntax(interpolationFormatClauseSyntax, formattedNode as InterpolationFormatClauseSyntax);
             case InterpolationSyntax interpolationSyntax:
                 return this.CompareInterpolationSyntax(interpolationSyntax, formattedNode as InterpolationSyntax);
             case InvocationExpressionSyntax invocationExpressionSyntax:
                 return this.CompareInvocationExpressionSyntax(invocationExpressionSyntax, formattedNode as InvocationExpressionSyntax);
             case IsPatternExpressionSyntax isPatternExpressionSyntax:
                 return this.CompareIsPatternExpressionSyntax(isPatternExpressionSyntax, formattedNode as IsPatternExpressionSyntax);
             case JoinClauseSyntax joinClauseSyntax:
                 return this.CompareJoinClauseSyntax(joinClauseSyntax, formattedNode as JoinClauseSyntax);
             case JoinIntoClauseSyntax joinIntoClauseSyntax:
                 return this.CompareJoinIntoClauseSyntax(joinIntoClauseSyntax, formattedNode as JoinIntoClauseSyntax);
             case LabeledStatementSyntax labeledStatementSyntax:
                 return this.CompareLabeledStatementSyntax(labeledStatementSyntax, formattedNode as LabeledStatementSyntax);
             case LetClauseSyntax letClauseSyntax:
                 return this.CompareLetClauseSyntax(letClauseSyntax, formattedNode as LetClauseSyntax);
             case LineDirectivePositionSyntax lineDirectivePositionSyntax:
                 return this.CompareLineDirectivePositionSyntax(lineDirectivePositionSyntax, formattedNode as LineDirectivePositionSyntax);
             case LineDirectiveTriviaSyntax lineDirectiveTriviaSyntax:
                 return this.CompareLineDirectiveTriviaSyntax(lineDirectiveTriviaSyntax, formattedNode as LineDirectiveTriviaSyntax);
             case LineSpanDirectiveTriviaSyntax lineSpanDirectiveTriviaSyntax:
                 return this.CompareLineSpanDirectiveTriviaSyntax(lineSpanDirectiveTriviaSyntax, formattedNode as LineSpanDirectiveTriviaSyntax);
             case ListPatternSyntax listPatternSyntax:
                 return this.CompareListPatternSyntax(listPatternSyntax, formattedNode as ListPatternSyntax);
             case LiteralExpressionSyntax literalExpressionSyntax:
                 return this.CompareLiteralExpressionSyntax(literalExpressionSyntax, formattedNode as LiteralExpressionSyntax);
             case LoadDirectiveTriviaSyntax loadDirectiveTriviaSyntax:
                 return this.CompareLoadDirectiveTriviaSyntax(loadDirectiveTriviaSyntax, formattedNode as LoadDirectiveTriviaSyntax);
             case LocalDeclarationStatementSyntax localDeclarationStatementSyntax:
                 return this.CompareLocalDeclarationStatementSyntax(localDeclarationStatementSyntax, formattedNode as LocalDeclarationStatementSyntax);
             case LocalFunctionStatementSyntax localFunctionStatementSyntax:
                 return this.CompareLocalFunctionStatementSyntax(localFunctionStatementSyntax, formattedNode as LocalFunctionStatementSyntax);
             case LockStatementSyntax lockStatementSyntax:
                 return this.CompareLockStatementSyntax(lockStatementSyntax, formattedNode as LockStatementSyntax);
             case MakeRefExpressionSyntax makeRefExpressionSyntax:
                 return this.CompareMakeRefExpressionSyntax(makeRefExpressionSyntax, formattedNode as MakeRefExpressionSyntax);
             case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
                 return this.CompareMemberAccessExpressionSyntax(memberAccessExpressionSyntax, formattedNode as MemberAccessExpressionSyntax);
             case MemberBindingExpressionSyntax memberBindingExpressionSyntax:
                 return this.CompareMemberBindingExpressionSyntax(memberBindingExpressionSyntax, formattedNode as MemberBindingExpressionSyntax);
             case MethodDeclarationSyntax methodDeclarationSyntax:
                 return this.CompareMethodDeclarationSyntax(methodDeclarationSyntax, formattedNode as MethodDeclarationSyntax);
             case NameColonSyntax nameColonSyntax:
                 return this.CompareNameColonSyntax(nameColonSyntax, formattedNode as NameColonSyntax);
             case NameEqualsSyntax nameEqualsSyntax:
                 return this.CompareNameEqualsSyntax(nameEqualsSyntax, formattedNode as NameEqualsSyntax);
             case NameMemberCrefSyntax nameMemberCrefSyntax:
                 return this.CompareNameMemberCrefSyntax(nameMemberCrefSyntax, formattedNode as NameMemberCrefSyntax);
             case NamespaceDeclarationSyntax namespaceDeclarationSyntax:
                 return this.CompareNamespaceDeclarationSyntax(namespaceDeclarationSyntax, formattedNode as NamespaceDeclarationSyntax);
             case NullableDirectiveTriviaSyntax nullableDirectiveTriviaSyntax:
                 return this.CompareNullableDirectiveTriviaSyntax(nullableDirectiveTriviaSyntax, formattedNode as NullableDirectiveTriviaSyntax);
             case NullableTypeSyntax nullableTypeSyntax:
                 return this.CompareNullableTypeSyntax(nullableTypeSyntax, formattedNode as NullableTypeSyntax);
             case ObjectCreationExpressionSyntax objectCreationExpressionSyntax:
                 return this.CompareObjectCreationExpressionSyntax(objectCreationExpressionSyntax, formattedNode as ObjectCreationExpressionSyntax);
             case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax:
                 return this.CompareOmittedArraySizeExpressionSyntax(omittedArraySizeExpressionSyntax, formattedNode as OmittedArraySizeExpressionSyntax);
             case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax:
                 return this.CompareOmittedTypeArgumentSyntax(omittedTypeArgumentSyntax, formattedNode as OmittedTypeArgumentSyntax);
             case OperatorDeclarationSyntax operatorDeclarationSyntax:
                 return this.CompareOperatorDeclarationSyntax(operatorDeclarationSyntax, formattedNode as OperatorDeclarationSyntax);
             case OperatorMemberCrefSyntax operatorMemberCrefSyntax:
                 return this.CompareOperatorMemberCrefSyntax(operatorMemberCrefSyntax, formattedNode as OperatorMemberCrefSyntax);
             case OrderByClauseSyntax orderByClauseSyntax:
                 return this.CompareOrderByClauseSyntax(orderByClauseSyntax, formattedNode as OrderByClauseSyntax);
             case OrderingSyntax orderingSyntax:
                 return this.CompareOrderingSyntax(orderingSyntax, formattedNode as OrderingSyntax);
             case ParameterListSyntax parameterListSyntax:
                 return this.CompareParameterListSyntax(parameterListSyntax, formattedNode as ParameterListSyntax);
             case ParameterSyntax parameterSyntax:
                 return this.CompareParameterSyntax(parameterSyntax, formattedNode as ParameterSyntax);
             case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax:
                 return this.CompareParenthesizedExpressionSyntax(parenthesizedExpressionSyntax, formattedNode as ParenthesizedExpressionSyntax);
             case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax:
                 return this.CompareParenthesizedLambdaExpressionSyntax(parenthesizedLambdaExpressionSyntax, formattedNode as ParenthesizedLambdaExpressionSyntax);
             case ParenthesizedPatternSyntax parenthesizedPatternSyntax:
                 return this.CompareParenthesizedPatternSyntax(parenthesizedPatternSyntax, formattedNode as ParenthesizedPatternSyntax);
             case ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax:
                 return this.CompareParenthesizedVariableDesignationSyntax(parenthesizedVariableDesignationSyntax, formattedNode as ParenthesizedVariableDesignationSyntax);
             case PointerTypeSyntax pointerTypeSyntax:
                 return this.ComparePointerTypeSyntax(pointerTypeSyntax, formattedNode as PointerTypeSyntax);
             case PositionalPatternClauseSyntax positionalPatternClauseSyntax:
                 return this.ComparePositionalPatternClauseSyntax(positionalPatternClauseSyntax, formattedNode as PositionalPatternClauseSyntax);
             case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax:
                 return this.ComparePostfixUnaryExpressionSyntax(postfixUnaryExpressionSyntax, formattedNode as PostfixUnaryExpressionSyntax);
             case PragmaChecksumDirectiveTriviaSyntax pragmaChecksumDirectiveTriviaSyntax:
                 return this.ComparePragmaChecksumDirectiveTriviaSyntax(pragmaChecksumDirectiveTriviaSyntax, formattedNode as PragmaChecksumDirectiveTriviaSyntax);
             case PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTriviaSyntax:
                 return this.ComparePragmaWarningDirectiveTriviaSyntax(pragmaWarningDirectiveTriviaSyntax, formattedNode as PragmaWarningDirectiveTriviaSyntax);
             case PredefinedTypeSyntax predefinedTypeSyntax:
                 return this.ComparePredefinedTypeSyntax(predefinedTypeSyntax, formattedNode as PredefinedTypeSyntax);
             case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax:
                 return this.ComparePrefixUnaryExpressionSyntax(prefixUnaryExpressionSyntax, formattedNode as PrefixUnaryExpressionSyntax);
             case PrimaryConstructorBaseTypeSyntax primaryConstructorBaseTypeSyntax:
                 return this.ComparePrimaryConstructorBaseTypeSyntax(primaryConstructorBaseTypeSyntax, formattedNode as PrimaryConstructorBaseTypeSyntax);
             case PropertyDeclarationSyntax propertyDeclarationSyntax:
                 return this.ComparePropertyDeclarationSyntax(propertyDeclarationSyntax, formattedNode as PropertyDeclarationSyntax);
             case PropertyPatternClauseSyntax propertyPatternClauseSyntax:
                 return this.ComparePropertyPatternClauseSyntax(propertyPatternClauseSyntax, formattedNode as PropertyPatternClauseSyntax);
             case QualifiedCrefSyntax qualifiedCrefSyntax:
                 return this.CompareQualifiedCrefSyntax(qualifiedCrefSyntax, formattedNode as QualifiedCrefSyntax);
             case QualifiedNameSyntax qualifiedNameSyntax:
                 return this.CompareQualifiedNameSyntax(qualifiedNameSyntax, formattedNode as QualifiedNameSyntax);
             case QueryBodySyntax queryBodySyntax:
                 return this.CompareQueryBodySyntax(queryBodySyntax, formattedNode as QueryBodySyntax);
             case QueryContinuationSyntax queryContinuationSyntax:
                 return this.CompareQueryContinuationSyntax(queryContinuationSyntax, formattedNode as QueryContinuationSyntax);
             case QueryExpressionSyntax queryExpressionSyntax:
                 return this.CompareQueryExpressionSyntax(queryExpressionSyntax, formattedNode as QueryExpressionSyntax);
             case RangeExpressionSyntax rangeExpressionSyntax:
                 return this.CompareRangeExpressionSyntax(rangeExpressionSyntax, formattedNode as RangeExpressionSyntax);
             case RecordDeclarationSyntax recordDeclarationSyntax:
                 return this.CompareRecordDeclarationSyntax(recordDeclarationSyntax, formattedNode as RecordDeclarationSyntax);
             case RecursivePatternSyntax recursivePatternSyntax:
                 return this.CompareRecursivePatternSyntax(recursivePatternSyntax, formattedNode as RecursivePatternSyntax);
             case ReferenceDirectiveTriviaSyntax referenceDirectiveTriviaSyntax:
                 return this.CompareReferenceDirectiveTriviaSyntax(referenceDirectiveTriviaSyntax, formattedNode as ReferenceDirectiveTriviaSyntax);
             case RefExpressionSyntax refExpressionSyntax:
                 return this.CompareRefExpressionSyntax(refExpressionSyntax, formattedNode as RefExpressionSyntax);
             case RefStructConstraintSyntax refStructConstraintSyntax:
                 return this.CompareRefStructConstraintSyntax(refStructConstraintSyntax, formattedNode as RefStructConstraintSyntax);
             case RefTypeExpressionSyntax refTypeExpressionSyntax:
                 return this.CompareRefTypeExpressionSyntax(refTypeExpressionSyntax, formattedNode as RefTypeExpressionSyntax);
             case RefTypeSyntax refTypeSyntax:
                 return this.CompareRefTypeSyntax(refTypeSyntax, formattedNode as RefTypeSyntax);
             case RefValueExpressionSyntax refValueExpressionSyntax:
                 return this.CompareRefValueExpressionSyntax(refValueExpressionSyntax, formattedNode as RefValueExpressionSyntax);
             case RegionDirectiveTriviaSyntax regionDirectiveTriviaSyntax:
                 return this.CompareRegionDirectiveTriviaSyntax(regionDirectiveTriviaSyntax, formattedNode as RegionDirectiveTriviaSyntax);
             case RelationalPatternSyntax relationalPatternSyntax:
                 return this.CompareRelationalPatternSyntax(relationalPatternSyntax, formattedNode as RelationalPatternSyntax);
             case ReturnStatementSyntax returnStatementSyntax:
                 return this.CompareReturnStatementSyntax(returnStatementSyntax, formattedNode as ReturnStatementSyntax);
             case ScopedTypeSyntax scopedTypeSyntax:
                 return this.CompareScopedTypeSyntax(scopedTypeSyntax, formattedNode as ScopedTypeSyntax);
             case SelectClauseSyntax selectClauseSyntax:
                 return this.CompareSelectClauseSyntax(selectClauseSyntax, formattedNode as SelectClauseSyntax);
             case ShebangDirectiveTriviaSyntax shebangDirectiveTriviaSyntax:
                 return this.CompareShebangDirectiveTriviaSyntax(shebangDirectiveTriviaSyntax, formattedNode as ShebangDirectiveTriviaSyntax);
             case SimpleBaseTypeSyntax simpleBaseTypeSyntax:
                 return this.CompareSimpleBaseTypeSyntax(simpleBaseTypeSyntax, formattedNode as SimpleBaseTypeSyntax);
             case SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax:
                 return this.CompareSimpleLambdaExpressionSyntax(simpleLambdaExpressionSyntax, formattedNode as SimpleLambdaExpressionSyntax);
             case SingleVariableDesignationSyntax singleVariableDesignationSyntax:
                 return this.CompareSingleVariableDesignationSyntax(singleVariableDesignationSyntax, formattedNode as SingleVariableDesignationSyntax);
             case SizeOfExpressionSyntax sizeOfExpressionSyntax:
                 return this.CompareSizeOfExpressionSyntax(sizeOfExpressionSyntax, formattedNode as SizeOfExpressionSyntax);
             case SkippedTokensTriviaSyntax skippedTokensTriviaSyntax:
                 return this.CompareSkippedTokensTriviaSyntax(skippedTokensTriviaSyntax, formattedNode as SkippedTokensTriviaSyntax);
             case SlicePatternSyntax slicePatternSyntax:
                 return this.CompareSlicePatternSyntax(slicePatternSyntax, formattedNode as SlicePatternSyntax);
             case SpreadElementSyntax spreadElementSyntax:
                 return this.CompareSpreadElementSyntax(spreadElementSyntax, formattedNode as SpreadElementSyntax);
             case StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax:
                 return this.CompareStackAllocArrayCreationExpressionSyntax(stackAllocArrayCreationExpressionSyntax, formattedNode as StackAllocArrayCreationExpressionSyntax);
             case StructDeclarationSyntax structDeclarationSyntax:
                 return this.CompareStructDeclarationSyntax(structDeclarationSyntax, formattedNode as StructDeclarationSyntax);
             case SubpatternSyntax subpatternSyntax:
                 return this.CompareSubpatternSyntax(subpatternSyntax, formattedNode as SubpatternSyntax);
             case SwitchExpressionArmSyntax switchExpressionArmSyntax:
                 return this.CompareSwitchExpressionArmSyntax(switchExpressionArmSyntax, formattedNode as SwitchExpressionArmSyntax);
             case SwitchExpressionSyntax switchExpressionSyntax:
                 return this.CompareSwitchExpressionSyntax(switchExpressionSyntax, formattedNode as SwitchExpressionSyntax);
             case SwitchSectionSyntax switchSectionSyntax:
                 return this.CompareSwitchSectionSyntax(switchSectionSyntax, formattedNode as SwitchSectionSyntax);
             case SwitchStatementSyntax switchStatementSyntax:
                 return this.CompareSwitchStatementSyntax(switchStatementSyntax, formattedNode as SwitchStatementSyntax);
             case ThisExpressionSyntax thisExpressionSyntax:
                 return this.CompareThisExpressionSyntax(thisExpressionSyntax, formattedNode as ThisExpressionSyntax);
             case ThrowExpressionSyntax throwExpressionSyntax:
                 return this.CompareThrowExpressionSyntax(throwExpressionSyntax, formattedNode as ThrowExpressionSyntax);
             case ThrowStatementSyntax throwStatementSyntax:
                 return this.CompareThrowStatementSyntax(throwStatementSyntax, formattedNode as ThrowStatementSyntax);
             case TryStatementSyntax tryStatementSyntax:
                 return this.CompareTryStatementSyntax(tryStatementSyntax, formattedNode as TryStatementSyntax);
             case TupleElementSyntax tupleElementSyntax:
                 return this.CompareTupleElementSyntax(tupleElementSyntax, formattedNode as TupleElementSyntax);
             case TupleExpressionSyntax tupleExpressionSyntax:
                 return this.CompareTupleExpressionSyntax(tupleExpressionSyntax, formattedNode as TupleExpressionSyntax);
             case TupleTypeSyntax tupleTypeSyntax:
                 return this.CompareTupleTypeSyntax(tupleTypeSyntax, formattedNode as TupleTypeSyntax);
             case TypeArgumentListSyntax typeArgumentListSyntax:
                 return this.CompareTypeArgumentListSyntax(typeArgumentListSyntax, formattedNode as TypeArgumentListSyntax);
             case TypeConstraintSyntax typeConstraintSyntax:
                 return this.CompareTypeConstraintSyntax(typeConstraintSyntax, formattedNode as TypeConstraintSyntax);
             case TypeCrefSyntax typeCrefSyntax:
                 return this.CompareTypeCrefSyntax(typeCrefSyntax, formattedNode as TypeCrefSyntax);
             case TypeOfExpressionSyntax typeOfExpressionSyntax:
                 return this.CompareTypeOfExpressionSyntax(typeOfExpressionSyntax, formattedNode as TypeOfExpressionSyntax);
             case TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax:
                 return this.CompareTypeParameterConstraintClauseSyntax(typeParameterConstraintClauseSyntax, formattedNode as TypeParameterConstraintClauseSyntax);
             case TypeParameterListSyntax typeParameterListSyntax:
                 return this.CompareTypeParameterListSyntax(typeParameterListSyntax, formattedNode as TypeParameterListSyntax);
             case TypeParameterSyntax typeParameterSyntax:
                 return this.CompareTypeParameterSyntax(typeParameterSyntax, formattedNode as TypeParameterSyntax);
             case TypePatternSyntax typePatternSyntax:
                 return this.CompareTypePatternSyntax(typePatternSyntax, formattedNode as TypePatternSyntax);
             case UnaryPatternSyntax unaryPatternSyntax:
                 return this.CompareUnaryPatternSyntax(unaryPatternSyntax, formattedNode as UnaryPatternSyntax);
             case UndefDirectiveTriviaSyntax undefDirectiveTriviaSyntax:
                 return this.CompareUndefDirectiveTriviaSyntax(undefDirectiveTriviaSyntax, formattedNode as UndefDirectiveTriviaSyntax);
             case UnsafeStatementSyntax unsafeStatementSyntax:
                 return this.CompareUnsafeStatementSyntax(unsafeStatementSyntax, formattedNode as UnsafeStatementSyntax);
                case UsingDirectiveSyntax usingDirectiveSyntax:
                    if (this.ReorderedUsingsWithDisabledText)
                        return Equal;
                    return this.CompareUsingDirectiveSyntax(usingDirectiveSyntax, formattedNode as UsingDirectiveSyntax);
             case UsingStatementSyntax usingStatementSyntax:
                 return this.CompareUsingStatementSyntax(usingStatementSyntax, formattedNode as UsingStatementSyntax);
             case VariableDeclarationSyntax variableDeclarationSyntax:
                 return this.CompareVariableDeclarationSyntax(variableDeclarationSyntax, formattedNode as VariableDeclarationSyntax);
             case VariableDeclaratorSyntax variableDeclaratorSyntax:
                 return this.CompareVariableDeclaratorSyntax(variableDeclaratorSyntax, formattedNode as VariableDeclaratorSyntax);
             case VarPatternSyntax varPatternSyntax:
                 return this.CompareVarPatternSyntax(varPatternSyntax, formattedNode as VarPatternSyntax);
             case WarningDirectiveTriviaSyntax warningDirectiveTriviaSyntax:
                 return this.CompareWarningDirectiveTriviaSyntax(warningDirectiveTriviaSyntax, formattedNode as WarningDirectiveTriviaSyntax);
             case WhenClauseSyntax whenClauseSyntax:
                 return this.CompareWhenClauseSyntax(whenClauseSyntax, formattedNode as WhenClauseSyntax);
             case WhereClauseSyntax whereClauseSyntax:
                 return this.CompareWhereClauseSyntax(whereClauseSyntax, formattedNode as WhereClauseSyntax);
             case WhileStatementSyntax whileStatementSyntax:
                 return this.CompareWhileStatementSyntax(whileStatementSyntax, formattedNode as WhileStatementSyntax);
             case WithExpressionSyntax withExpressionSyntax:
                 return this.CompareWithExpressionSyntax(withExpressionSyntax, formattedNode as WithExpressionSyntax);
             case XmlCDataSectionSyntax xmlCDataSectionSyntax:
                 return this.CompareXmlCDataSectionSyntax(xmlCDataSectionSyntax, formattedNode as XmlCDataSectionSyntax);
             case XmlCommentSyntax xmlCommentSyntax:
                 return this.CompareXmlCommentSyntax(xmlCommentSyntax, formattedNode as XmlCommentSyntax);
             case XmlCrefAttributeSyntax xmlCrefAttributeSyntax:
                 return this.CompareXmlCrefAttributeSyntax(xmlCrefAttributeSyntax, formattedNode as XmlCrefAttributeSyntax);
             case XmlElementEndTagSyntax xmlElementEndTagSyntax:
                 return this.CompareXmlElementEndTagSyntax(xmlElementEndTagSyntax, formattedNode as XmlElementEndTagSyntax);
             case XmlElementStartTagSyntax xmlElementStartTagSyntax:
                 return this.CompareXmlElementStartTagSyntax(xmlElementStartTagSyntax, formattedNode as XmlElementStartTagSyntax);
             case XmlElementSyntax xmlElementSyntax:
                 return this.CompareXmlElementSyntax(xmlElementSyntax, formattedNode as XmlElementSyntax);
             case XmlEmptyElementSyntax xmlEmptyElementSyntax:
                 return this.CompareXmlEmptyElementSyntax(xmlEmptyElementSyntax, formattedNode as XmlEmptyElementSyntax);
             case XmlNameAttributeSyntax xmlNameAttributeSyntax:
                 return this.CompareXmlNameAttributeSyntax(xmlNameAttributeSyntax, formattedNode as XmlNameAttributeSyntax);
             case XmlNameSyntax xmlNameSyntax:
                 return this.CompareXmlNameSyntax(xmlNameSyntax, formattedNode as XmlNameSyntax);
             case XmlPrefixSyntax xmlPrefixSyntax:
                 return this.CompareXmlPrefixSyntax(xmlPrefixSyntax, formattedNode as XmlPrefixSyntax);
             case XmlProcessingInstructionSyntax xmlProcessingInstructionSyntax:
                 return this.CompareXmlProcessingInstructionSyntax(xmlProcessingInstructionSyntax, formattedNode as XmlProcessingInstructionSyntax);
             case XmlTextAttributeSyntax xmlTextAttributeSyntax:
                 return this.CompareXmlTextAttributeSyntax(xmlTextAttributeSyntax, formattedNode as XmlTextAttributeSyntax);
             case XmlTextSyntax xmlTextSyntax:
                 return this.CompareXmlTextSyntax(xmlTextSyntax, formattedNode as XmlTextSyntax);
             case YieldStatementSyntax yieldStatementSyntax:
                 return this.CompareYieldStatementSyntax(yieldStatementSyntax, formattedNode as YieldStatementSyntax);
                default:
#if DEBUG
                    throw new Exception("Can't handle " + originalNode.GetType().Name);
#else
                    return Equal;
#endif
            }
        }
        
      private CompareResult CompareAccessorDeclarationSyntax(AccessorDeclarationSyntax originalNode, AccessorDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareAccessorListSyntax(AccessorListSyntax originalNode, AccessorListSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Accessors, formattedNode.Accessors, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareAliasQualifiedNameSyntax(AliasQualifiedNameSyntax originalNode, AliasQualifiedNameSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Alias, originalNode));
            formattedStack.Push((formattedNode.Alias, formattedNode));
            result = this.Compare(originalNode.ColonColonToken, formattedNode.ColonColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            return Equal;
        }
      private CompareResult CompareAllowsConstraintClauseSyntax(AllowsConstraintClauseSyntax originalNode, AllowsConstraintClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.AllowsKeyword, formattedNode.AllowsKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Constraints, formattedNode.Constraints, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Constraints.GetSeparators().Take(originalNode.Constraints.Count() - 1).ToList(), formattedNode.Constraints.GetSeparators().Take(formattedNode.Constraints.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareAnonymousMethodExpressionSyntax(AnonymousMethodExpressionSyntax originalNode, AnonymousMethodExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.AsyncKeyword, formattedNode.AsyncKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Block, originalNode));
            formattedStack.Push((formattedNode.Block, formattedNode));
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            result = this.Compare(originalNode.DelegateKeyword, formattedNode.DelegateKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareAnonymousObjectCreationExpressionSyntax(AnonymousObjectCreationExpressionSyntax originalNode, AnonymousObjectCreationExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Initializers, formattedNode.Initializers, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Initializers.GetSeparators().Take(originalNode.Initializers.Count() - 1).ToList(), formattedNode.Initializers.GetSeparators().Take(formattedNode.Initializers.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.NewKeyword, formattedNode.NewKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareAnonymousObjectMemberDeclaratorSyntax(AnonymousObjectMemberDeclaratorSyntax originalNode, AnonymousObjectMemberDeclaratorSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.NameEquals, originalNode));
            formattedStack.Push((formattedNode.NameEquals, formattedNode));
            return Equal;
        }
      private CompareResult CompareArgumentListSyntax(ArgumentListSyntax originalNode, ArgumentListSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Arguments, formattedNode.Arguments, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Arguments.GetSeparators().Take(originalNode.Arguments.Count() - 1).ToList(), formattedNode.Arguments.GetSeparators().Take(formattedNode.Arguments.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareArgumentSyntax(ArgumentSyntax originalNode, ArgumentSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.NameColon, originalNode));
            formattedStack.Push((formattedNode.NameColon, formattedNode));
            result = this.Compare(originalNode.RefKindKeyword, formattedNode.RefKindKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.RefOrOutKeyword, formattedNode.RefOrOutKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareArrayCreationExpressionSyntax(ArrayCreationExpressionSyntax originalNode, ArrayCreationExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.NewKeyword, formattedNode.NewKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareArrayRankSpecifierSyntax(ArrayRankSpecifierSyntax originalNode, ArrayRankSpecifierSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.Rank != formattedNode.Rank) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Sizes, formattedNode.Sizes, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Sizes.GetSeparators().Take(originalNode.Sizes.Count() - 1).ToList(), formattedNode.Sizes.GetSeparators().Take(formattedNode.Sizes.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareArrayTypeSyntax(ArrayTypeSyntax originalNode, ArrayTypeSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ElementType, originalNode));
            formattedStack.Push((formattedNode.ElementType, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.RankSpecifiers, formattedNode.RankSpecifiers, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareArrowExpressionClauseSyntax(ArrowExpressionClauseSyntax originalNode, ArrowExpressionClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ArrowToken, formattedNode.ArrowToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareAssignmentExpressionSyntax(AssignmentExpressionSyntax originalNode, AssignmentExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Left, originalNode));
            formattedStack.Push((formattedNode.Left, formattedNode));
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Right, originalNode));
            formattedStack.Push((formattedNode.Right, formattedNode));
            return Equal;
        }
      private CompareResult CompareAttributeArgumentListSyntax(AttributeArgumentListSyntax originalNode, AttributeArgumentListSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Arguments, formattedNode.Arguments, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Arguments.GetSeparators().Take(originalNode.Arguments.Count() - 1).ToList(), formattedNode.Arguments.GetSeparators().Take(formattedNode.Arguments.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareAttributeArgumentSyntax(AttributeArgumentSyntax originalNode, AttributeArgumentSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.NameColon, originalNode));
            formattedStack.Push((formattedNode.NameColon, formattedNode));
            originalStack.Push((originalNode.NameEquals, originalNode));
            formattedStack.Push((formattedNode.NameEquals, formattedNode));
            return Equal;
        }
      private CompareResult CompareAttributeListSyntax(AttributeListSyntax originalNode, AttributeListSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Attributes, formattedNode.Attributes, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Attributes.GetSeparators().Take(originalNode.Attributes.Count() - 1).ToList(), formattedNode.Attributes.GetSeparators().Take(formattedNode.Attributes.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Target, originalNode));
            formattedStack.Push((formattedNode.Target, formattedNode));
            return Equal;
        }
      private CompareResult CompareAttributeSyntax(AttributeSyntax originalNode, AttributeSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            return Equal;
        }
      private CompareResult CompareAttributeTargetSpecifierSyntax(AttributeTargetSpecifierSyntax originalNode, AttributeTargetSpecifierSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareAwaitExpressionSyntax(AwaitExpressionSyntax originalNode, AwaitExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.AwaitKeyword, formattedNode.AwaitKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareBadDirectiveTriviaSyntax(BadDirectiveTriviaSyntax originalNode, BadDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareBaseExpressionSyntax(BaseExpressionSyntax originalNode, BaseExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Token, formattedNode.Token, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareBaseListSyntax(BaseListSyntax originalNode, BaseListSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Types, formattedNode.Types, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Types.GetSeparators().Take(originalNode.Types.Count() - 1).ToList(), formattedNode.Types.GetSeparators().Take(formattedNode.Types.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareBinaryExpressionSyntax(BinaryExpressionSyntax originalNode, BinaryExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Left, originalNode));
            formattedStack.Push((formattedNode.Left, formattedNode));
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Right, originalNode));
            formattedStack.Push((formattedNode.Right, formattedNode));
            return Equal;
        }
      private CompareResult CompareBinaryPatternSyntax(BinaryPatternSyntax originalNode, BinaryPatternSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Left, originalNode));
            formattedStack.Push((formattedNode.Left, formattedNode));
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Right, originalNode));
            formattedStack.Push((formattedNode.Right, formattedNode));
            return Equal;
        }
      private CompareResult CompareBlockSyntax(BlockSyntax originalNode, BlockSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Statements, formattedNode.Statements, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareBracketedArgumentListSyntax(BracketedArgumentListSyntax originalNode, BracketedArgumentListSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Arguments, formattedNode.Arguments, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Arguments.GetSeparators().Take(originalNode.Arguments.Count() - 1).ToList(), formattedNode.Arguments.GetSeparators().Take(formattedNode.Arguments.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareBracketedParameterListSyntax(BracketedParameterListSyntax originalNode, BracketedParameterListSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters, formattedNode.Parameters, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters.GetSeparators().Take(originalNode.Parameters.Count() - 1).ToList(), formattedNode.Parameters.GetSeparators().Take(formattedNode.Parameters.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareBreakStatementSyntax(BreakStatementSyntax originalNode, BreakStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.BreakKeyword, formattedNode.BreakKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareCasePatternSwitchLabelSyntax(CasePatternSwitchLabelSyntax originalNode, CasePatternSwitchLabelSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Pattern, originalNode));
            formattedStack.Push((formattedNode.Pattern, formattedNode));
            originalStack.Push((originalNode.WhenClause, originalNode));
            formattedStack.Push((formattedNode.WhenClause, formattedNode));
            return Equal;
        }
      private CompareResult CompareCaseSwitchLabelSyntax(CaseSwitchLabelSyntax originalNode, CaseSwitchLabelSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Value, originalNode));
            formattedStack.Push((formattedNode.Value, formattedNode));
            return Equal;
        }
      private CompareResult CompareCastExpressionSyntax(CastExpressionSyntax originalNode, CastExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareCatchClauseSyntax(CatchClauseSyntax originalNode, CatchClauseSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Block, originalNode));
            formattedStack.Push((formattedNode.Block, formattedNode));
            result = this.Compare(originalNode.CatchKeyword, formattedNode.CatchKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Declaration, originalNode));
            formattedStack.Push((formattedNode.Declaration, formattedNode));
            originalStack.Push((originalNode.Filter, originalNode));
            formattedStack.Push((formattedNode.Filter, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareCatchDeclarationSyntax(CatchDeclarationSyntax originalNode, CatchDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareCatchFilterClauseSyntax(CatchFilterClauseSyntax originalNode, CatchFilterClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.FilterExpression, originalNode));
            formattedStack.Push((formattedNode.FilterExpression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.WhenKeyword, formattedNode.WhenKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareCheckedExpressionSyntax(CheckedExpressionSyntax originalNode, CheckedExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareCheckedStatementSyntax(CheckedStatementSyntax originalNode, CheckedStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Block, originalNode));
            formattedStack.Push((formattedNode.Block, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareClassDeclarationSyntax(ClassDeclarationSyntax originalNode, ClassDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.BaseList, originalNode));
            formattedStack.Push((formattedNode.BaseList, formattedNode));
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.ConstraintClauses, formattedNode.ConstraintClauses, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Members, formattedNode.Members, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.TypeParameterList, originalNode));
            formattedStack.Push((formattedNode.TypeParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareClassOrStructConstraintSyntax(ClassOrStructConstraintSyntax originalNode, ClassOrStructConstraintSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ClassOrStructKeyword, formattedNode.ClassOrStructKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.QuestionToken, formattedNode.QuestionToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareCollectionExpressionSyntax(CollectionExpressionSyntax originalNode, CollectionExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Elements, formattedNode.Elements, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Elements.GetSeparators().Take(originalNode.Elements.Count() - 1).ToList(), formattedNode.Elements.GetSeparators().Take(formattedNode.Elements.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareCompilationUnitSyntax(CompilationUnitSyntax originalNode, CompilationUnitSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfFileToken, formattedNode.EndOfFileToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Externs, formattedNode.Externs, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Members, formattedNode.Members, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareUsingDirectives(originalNode.Usings, formattedNode.Usings, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareConditionalAccessExpressionSyntax(ConditionalAccessExpressionSyntax originalNode, ConditionalAccessExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.WhenNotNull, originalNode));
            formattedStack.Push((formattedNode.WhenNotNull, formattedNode));
            return Equal;
        }
      private CompareResult CompareConditionalExpressionSyntax(ConditionalExpressionSyntax originalNode, ConditionalExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.QuestionToken, formattedNode.QuestionToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.WhenFalse, originalNode));
            formattedStack.Push((formattedNode.WhenFalse, formattedNode));
            originalStack.Push((originalNode.WhenTrue, originalNode));
            formattedStack.Push((formattedNode.WhenTrue, formattedNode));
            return Equal;
        }
      private CompareResult CompareConstantPatternSyntax(ConstantPatternSyntax originalNode, ConstantPatternSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareConstructorConstraintSyntax(ConstructorConstraintSyntax originalNode, ConstructorConstraintSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.NewKeyword, formattedNode.NewKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareConstructorDeclarationSyntax(ConstructorDeclarationSyntax originalNode, ConstructorDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareConstructorInitializerSyntax(ConstructorInitializerSyntax originalNode, ConstructorInitializerSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ThisOrBaseKeyword, formattedNode.ThisOrBaseKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareContinueStatementSyntax(ContinueStatementSyntax originalNode, ContinueStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ContinueKeyword, formattedNode.ContinueKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareConversionOperatorDeclarationSyntax(ConversionOperatorDeclarationSyntax originalNode, ConversionOperatorDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            result = this.Compare(originalNode.CheckedKeyword, formattedNode.CheckedKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ExplicitInterfaceSpecifier, originalNode));
            formattedStack.Push((formattedNode.ExplicitInterfaceSpecifier, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            result = this.Compare(originalNode.ImplicitOrExplicitKeyword, formattedNode.ImplicitOrExplicitKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OperatorKeyword, formattedNode.OperatorKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareConversionOperatorMemberCrefSyntax(ConversionOperatorMemberCrefSyntax originalNode, ConversionOperatorMemberCrefSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CheckedKeyword, formattedNode.CheckedKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ImplicitOrExplicitKeyword, formattedNode.ImplicitOrExplicitKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OperatorKeyword, formattedNode.OperatorKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Parameters, originalNode));
            formattedStack.Push((formattedNode.Parameters, formattedNode));
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareCrefBracketedParameterListSyntax(CrefBracketedParameterListSyntax originalNode, CrefBracketedParameterListSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters, formattedNode.Parameters, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters.GetSeparators().Take(originalNode.Parameters.Count() - 1).ToList(), formattedNode.Parameters.GetSeparators().Take(formattedNode.Parameters.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareCrefParameterListSyntax(CrefParameterListSyntax originalNode, CrefParameterListSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters, formattedNode.Parameters, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters.GetSeparators().Take(originalNode.Parameters.Count() - 1).ToList(), formattedNode.Parameters.GetSeparators().Take(formattedNode.Parameters.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareCrefParameterSyntax(CrefParameterSyntax originalNode, CrefParameterSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ReadOnlyKeyword, formattedNode.ReadOnlyKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.RefKindKeyword, formattedNode.RefKindKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.RefOrOutKeyword, formattedNode.RefOrOutKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareDeclarationExpressionSyntax(DeclarationExpressionSyntax originalNode, DeclarationExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Designation, originalNode));
            formattedStack.Push((formattedNode.Designation, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareDeclarationPatternSyntax(DeclarationPatternSyntax originalNode, DeclarationPatternSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Designation, originalNode));
            formattedStack.Push((formattedNode.Designation, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareDefaultConstraintSyntax(DefaultConstraintSyntax originalNode, DefaultConstraintSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DefaultKeyword, formattedNode.DefaultKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareDefaultExpressionSyntax(DefaultExpressionSyntax originalNode, DefaultExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareDefaultSwitchLabelSyntax(DefaultSwitchLabelSyntax originalNode, DefaultSwitchLabelSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareDefineDirectiveTriviaSyntax(DefineDirectiveTriviaSyntax originalNode, DefineDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DefineKeyword, formattedNode.DefineKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Name, formattedNode.Name, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareDelegateDeclarationSyntax(DelegateDeclarationSyntax originalNode, DelegateDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.ConstraintClauses, formattedNode.ConstraintClauses, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.DelegateKeyword, formattedNode.DelegateKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            originalStack.Push((originalNode.ReturnType, originalNode));
            formattedStack.Push((formattedNode.ReturnType, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.TypeParameterList, originalNode));
            formattedStack.Push((formattedNode.TypeParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareDestructorDeclarationSyntax(DestructorDeclarationSyntax originalNode, DestructorDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.TildeToken, formattedNode.TildeToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareDiscardDesignationSyntax(DiscardDesignationSyntax originalNode, DiscardDesignationSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.UnderscoreToken, formattedNode.UnderscoreToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareDiscardPatternSyntax(DiscardPatternSyntax originalNode, DiscardPatternSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.UnderscoreToken, formattedNode.UnderscoreToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareDocumentationCommentTriviaSyntax(DocumentationCommentTriviaSyntax originalNode, DocumentationCommentTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Content, formattedNode.Content, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfComment, formattedNode.EndOfComment, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareDoStatementSyntax(DoStatementSyntax originalNode, DoStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            result = this.Compare(originalNode.DoKeyword, formattedNode.DoKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            result = this.Compare(originalNode.WhileKeyword, formattedNode.WhileKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareElementAccessExpressionSyntax(ElementAccessExpressionSyntax originalNode, ElementAccessExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareElementBindingExpressionSyntax(ElementBindingExpressionSyntax originalNode, ElementBindingExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareElifDirectiveTriviaSyntax(ElifDirectiveTriviaSyntax originalNode, ElifDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.BranchTaken != formattedNode.BranchTaken) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            if (originalNode.ConditionValue != formattedNode.ConditionValue) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ElifKeyword, formattedNode.ElifKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareElseClauseSyntax(ElseClauseSyntax originalNode, ElseClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ElseKeyword, formattedNode.ElseKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            return Equal;
        }
      private CompareResult CompareElseDirectiveTriviaSyntax(ElseDirectiveTriviaSyntax originalNode, ElseDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.BranchTaken != formattedNode.BranchTaken) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ElseKeyword, formattedNode.ElseKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareEmptyStatementSyntax(EmptyStatementSyntax originalNode, EmptyStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareEndIfDirectiveTriviaSyntax(EndIfDirectiveTriviaSyntax originalNode, EndIfDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndIfKeyword, formattedNode.EndIfKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareEndRegionDirectiveTriviaSyntax(EndRegionDirectiveTriviaSyntax originalNode, EndRegionDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndRegionKeyword, formattedNode.EndRegionKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareEnumDeclarationSyntax(EnumDeclarationSyntax originalNode, EnumDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.BaseList, originalNode));
            formattedStack.Push((formattedNode.BaseList, formattedNode));
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EnumKeyword, formattedNode.EnumKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Members, formattedNode.Members, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Members.GetSeparators().Take(originalNode.Members.Count() - 1).ToList(), formattedNode.Members.GetSeparators().Take(formattedNode.Members.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareEnumMemberDeclarationSyntax(EnumMemberDeclarationSyntax originalNode, EnumMemberDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.EqualsValue, originalNode));
            formattedStack.Push((formattedNode.EqualsValue, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareEqualsValueClauseSyntax(EqualsValueClauseSyntax originalNode, EqualsValueClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EqualsToken, formattedNode.EqualsToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Value, originalNode));
            formattedStack.Push((formattedNode.Value, formattedNode));
            return Equal;
        }
      private CompareResult CompareErrorDirectiveTriviaSyntax(ErrorDirectiveTriviaSyntax originalNode, ErrorDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ErrorKeyword, formattedNode.ErrorKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareEventDeclarationSyntax(EventDeclarationSyntax originalNode, EventDeclarationSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.AccessorList, originalNode));
            formattedStack.Push((formattedNode.AccessorList, formattedNode));
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EventKeyword, formattedNode.EventKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ExplicitInterfaceSpecifier, originalNode));
            formattedStack.Push((formattedNode.ExplicitInterfaceSpecifier, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareEventFieldDeclarationSyntax(EventFieldDeclarationSyntax originalNode, EventFieldDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Declaration, originalNode));
            formattedStack.Push((formattedNode.Declaration, formattedNode));
            result = this.Compare(originalNode.EventKeyword, formattedNode.EventKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareExplicitInterfaceSpecifierSyntax(ExplicitInterfaceSpecifierSyntax originalNode, ExplicitInterfaceSpecifierSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DotToken, formattedNode.DotToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            return Equal;
        }
      private CompareResult CompareExpressionColonSyntax(ExpressionColonSyntax originalNode, ExpressionColonSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareExpressionElementSyntax(ExpressionElementSyntax originalNode, ExpressionElementSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareExpressionStatementSyntax(ExpressionStatementSyntax originalNode, ExpressionStatementSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.AllowsAnyExpression != formattedNode.AllowsAnyExpression) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareExternAliasDirectiveSyntax(ExternAliasDirectiveSyntax originalNode, ExternAliasDirectiveSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.AliasKeyword, formattedNode.AliasKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ExternKeyword, formattedNode.ExternKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareFieldDeclarationSyntax(FieldDeclarationSyntax originalNode, FieldDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Declaration, originalNode));
            formattedStack.Push((formattedNode.Declaration, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareFileScopedNamespaceDeclarationSyntax(FileScopedNamespaceDeclarationSyntax originalNode, FileScopedNamespaceDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Externs, formattedNode.Externs, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Members, formattedNode.Members, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.NamespaceKeyword, formattedNode.NamespaceKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareUsingDirectives(originalNode.Usings, formattedNode.Usings, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareFinallyClauseSyntax(FinallyClauseSyntax originalNode, FinallyClauseSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Block, originalNode));
            formattedStack.Push((formattedNode.Block, formattedNode));
            result = this.Compare(originalNode.FinallyKeyword, formattedNode.FinallyKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareFixedStatementSyntax(FixedStatementSyntax originalNode, FixedStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Declaration, originalNode));
            formattedStack.Push((formattedNode.Declaration, formattedNode));
            result = this.Compare(originalNode.FixedKeyword, formattedNode.FixedKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            return Equal;
        }
      private CompareResult CompareForEachStatementSyntax(ForEachStatementSyntax originalNode, ForEachStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.AwaitKeyword, formattedNode.AwaitKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            result = this.Compare(originalNode.ForEachKeyword, formattedNode.ForEachKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.InKeyword, formattedNode.InKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareForEachVariableStatementSyntax(ForEachVariableStatementSyntax originalNode, ForEachVariableStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.AwaitKeyword, formattedNode.AwaitKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            result = this.Compare(originalNode.ForEachKeyword, formattedNode.ForEachKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.InKeyword, formattedNode.InKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            originalStack.Push((originalNode.Variable, originalNode));
            formattedStack.Push((formattedNode.Variable, formattedNode));
            return Equal;
        }
      private CompareResult CompareForStatementSyntax(ForStatementSyntax originalNode, ForStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            originalStack.Push((originalNode.Declaration, originalNode));
            formattedStack.Push((formattedNode.Declaration, formattedNode));
            result = this.Compare(originalNode.FirstSemicolonToken, formattedNode.FirstSemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ForKeyword, formattedNode.ForKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Incrementors, formattedNode.Incrementors, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Incrementors.GetSeparators().Take(originalNode.Incrementors.Count() - 1).ToList(), formattedNode.Incrementors.GetSeparators().Take(formattedNode.Incrementors.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Initializers, formattedNode.Initializers, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Initializers.GetSeparators().Take(originalNode.Initializers.Count() - 1).ToList(), formattedNode.Initializers.GetSeparators().Take(formattedNode.Initializers.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SecondSemicolonToken, formattedNode.SecondSemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            return Equal;
        }
      private CompareResult CompareFromClauseSyntax(FromClauseSyntax originalNode, FromClauseSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            result = this.Compare(originalNode.FromKeyword, formattedNode.FromKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.InKeyword, formattedNode.InKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareFunctionPointerCallingConventionSyntax(FunctionPointerCallingConventionSyntax originalNode, FunctionPointerCallingConventionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ManagedOrUnmanagedKeyword, formattedNode.ManagedOrUnmanagedKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.UnmanagedCallingConventionList, originalNode));
            formattedStack.Push((formattedNode.UnmanagedCallingConventionList, formattedNode));
            return Equal;
        }
      private CompareResult CompareFunctionPointerParameterListSyntax(FunctionPointerParameterListSyntax originalNode, FunctionPointerParameterListSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.GreaterThanToken, formattedNode.GreaterThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LessThanToken, formattedNode.LessThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters, formattedNode.Parameters, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters.GetSeparators().Take(originalNode.Parameters.Count() - 1).ToList(), formattedNode.Parameters.GetSeparators().Take(formattedNode.Parameters.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareFunctionPointerParameterSyntax(FunctionPointerParameterSyntax originalNode, FunctionPointerParameterSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareFunctionPointerTypeSyntax(FunctionPointerTypeSyntax originalNode, FunctionPointerTypeSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.AsteriskToken, formattedNode.AsteriskToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.CallingConvention, originalNode));
            formattedStack.Push((formattedNode.CallingConvention, formattedNode));
            result = this.Compare(originalNode.DelegateKeyword, formattedNode.DelegateKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareFunctionPointerUnmanagedCallingConventionListSyntax(FunctionPointerUnmanagedCallingConventionListSyntax originalNode, FunctionPointerUnmanagedCallingConventionListSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.CallingConventions, formattedNode.CallingConventions, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.CallingConventions.GetSeparators().Take(originalNode.CallingConventions.Count() - 1).ToList(), formattedNode.CallingConventions.GetSeparators().Take(formattedNode.CallingConventions.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareFunctionPointerUnmanagedCallingConventionSyntax(FunctionPointerUnmanagedCallingConventionSyntax originalNode, FunctionPointerUnmanagedCallingConventionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Name, formattedNode.Name, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareGenericNameSyntax(GenericNameSyntax originalNode, GenericNameSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnboundGenericName != formattedNode.IsUnboundGenericName) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.TypeArgumentList, originalNode));
            formattedStack.Push((formattedNode.TypeArgumentList, formattedNode));
            return Equal;
        }
      private CompareResult CompareGlobalStatementSyntax(GlobalStatementSyntax originalNode, GlobalStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            return Equal;
        }
      private CompareResult CompareGotoStatementSyntax(GotoStatementSyntax originalNode, GotoStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CaseOrDefaultKeyword, formattedNode.CaseOrDefaultKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            result = this.Compare(originalNode.GotoKeyword, formattedNode.GotoKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareGroupClauseSyntax(GroupClauseSyntax originalNode, GroupClauseSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ByExpression, originalNode));
            formattedStack.Push((formattedNode.ByExpression, formattedNode));
            result = this.Compare(originalNode.ByKeyword, formattedNode.ByKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.GroupExpression, originalNode));
            formattedStack.Push((formattedNode.GroupExpression, formattedNode));
            result = this.Compare(originalNode.GroupKeyword, formattedNode.GroupKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareIdentifierNameSyntax(IdentifierNameSyntax originalNode, IdentifierNameSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareIfDirectiveTriviaSyntax(IfDirectiveTriviaSyntax originalNode, IfDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.BranchTaken != formattedNode.BranchTaken) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            if (originalNode.ConditionValue != formattedNode.ConditionValue) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.IfKeyword, formattedNode.IfKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareIfStatementSyntax(IfStatementSyntax originalNode, IfStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            originalStack.Push((originalNode.Else, originalNode));
            formattedStack.Push((formattedNode.Else, formattedNode));
            result = this.Compare(originalNode.IfKeyword, formattedNode.IfKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            return Equal;
        }
      private CompareResult CompareImplicitArrayCreationExpressionSyntax(ImplicitArrayCreationExpressionSyntax originalNode, ImplicitArrayCreationExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Commas, formattedNode.Commas, Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.NewKeyword, formattedNode.NewKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareImplicitElementAccessSyntax(ImplicitElementAccessSyntax originalNode, ImplicitElementAccessSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareImplicitObjectCreationExpressionSyntax(ImplicitObjectCreationExpressionSyntax originalNode, ImplicitObjectCreationExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.NewKeyword, formattedNode.NewKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareImplicitStackAllocArrayCreationExpressionSyntax(ImplicitStackAllocArrayCreationExpressionSyntax originalNode, ImplicitStackAllocArrayCreationExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.StackAllocKeyword, formattedNode.StackAllocKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareIncompleteMemberSyntax(IncompleteMemberSyntax originalNode, IncompleteMemberSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareIndexerDeclarationSyntax(IndexerDeclarationSyntax originalNode, IndexerDeclarationSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.AccessorList, originalNode));
            formattedStack.Push((formattedNode.AccessorList, formattedNode));
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ExplicitInterfaceSpecifier, originalNode));
            formattedStack.Push((formattedNode.ExplicitInterfaceSpecifier, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ThisKeyword, formattedNode.ThisKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareIndexerMemberCrefSyntax(IndexerMemberCrefSyntax originalNode, IndexerMemberCrefSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Parameters, originalNode));
            formattedStack.Push((formattedNode.Parameters, formattedNode));
            result = this.Compare(originalNode.ThisKeyword, formattedNode.ThisKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareInitializerExpressionSyntax(InitializerExpressionSyntax originalNode, InitializerExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Expressions, formattedNode.Expressions, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Expressions.GetSeparators().Take(originalNode.Expressions.Count() - 1).ToList(), formattedNode.Expressions.GetSeparators().Take(formattedNode.Expressions.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareInterfaceDeclarationSyntax(InterfaceDeclarationSyntax originalNode, InterfaceDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.BaseList, originalNode));
            formattedStack.Push((formattedNode.BaseList, formattedNode));
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.ConstraintClauses, formattedNode.ConstraintClauses, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Members, formattedNode.Members, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.TypeParameterList, originalNode));
            formattedStack.Push((formattedNode.TypeParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareInterpolatedStringExpressionSyntax(InterpolatedStringExpressionSyntax originalNode, InterpolatedStringExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Contents, formattedNode.Contents, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.StringEndToken, formattedNode.StringEndToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.StringStartToken, formattedNode.StringStartToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareInterpolatedStringTextSyntax(InterpolatedStringTextSyntax originalNode, InterpolatedStringTextSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.TextToken, formattedNode.TextToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareInterpolationAlignmentClauseSyntax(InterpolationAlignmentClauseSyntax originalNode, InterpolationAlignmentClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CommaToken, formattedNode.CommaToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Value, originalNode));
            formattedStack.Push((formattedNode.Value, formattedNode));
            return Equal;
        }
      private CompareResult CompareInterpolationFormatClauseSyntax(InterpolationFormatClauseSyntax originalNode, InterpolationFormatClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.FormatStringToken, formattedNode.FormatStringToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareInterpolationSyntax(InterpolationSyntax originalNode, InterpolationSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.AlignmentClause, originalNode));
            formattedStack.Push((formattedNode.AlignmentClause, formattedNode));
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            originalStack.Push((originalNode.FormatClause, originalNode));
            formattedStack.Push((formattedNode.FormatClause, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareInvocationExpressionSyntax(InvocationExpressionSyntax originalNode, InvocationExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareIsPatternExpressionSyntax(IsPatternExpressionSyntax originalNode, IsPatternExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            result = this.Compare(originalNode.IsKeyword, formattedNode.IsKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Pattern, originalNode));
            formattedStack.Push((formattedNode.Pattern, formattedNode));
            return Equal;
        }
      private CompareResult CompareJoinClauseSyntax(JoinClauseSyntax originalNode, JoinClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EqualsKeyword, formattedNode.EqualsKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.InExpression, originalNode));
            formattedStack.Push((formattedNode.InExpression, formattedNode));
            result = this.Compare(originalNode.InKeyword, formattedNode.InKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Into, originalNode));
            formattedStack.Push((formattedNode.Into, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.JoinKeyword, formattedNode.JoinKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.LeftExpression, originalNode));
            formattedStack.Push((formattedNode.LeftExpression, formattedNode));
            result = this.Compare(originalNode.OnKeyword, formattedNode.OnKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.RightExpression, originalNode));
            formattedStack.Push((formattedNode.RightExpression, formattedNode));
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareJoinIntoClauseSyntax(JoinIntoClauseSyntax originalNode, JoinIntoClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.IntoKeyword, formattedNode.IntoKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareLabeledStatementSyntax(LabeledStatementSyntax originalNode, LabeledStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            return Equal;
        }
      private CompareResult CompareLetClauseSyntax(LetClauseSyntax originalNode, LetClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EqualsToken, formattedNode.EqualsToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LetKeyword, formattedNode.LetKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareLineDirectivePositionSyntax(LineDirectivePositionSyntax originalNode, LineDirectivePositionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.Character, formattedNode.Character, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CommaToken, formattedNode.CommaToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Line, formattedNode.Line, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareLineDirectiveTriviaSyntax(LineDirectiveTriviaSyntax originalNode, LineDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.File, formattedNode.File, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Line, formattedNode.Line, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.LineKeyword, formattedNode.LineKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareLineSpanDirectiveTriviaSyntax(LineSpanDirectiveTriviaSyntax originalNode, LineSpanDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CharacterOffset, formattedNode.CharacterOffset, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.End, originalNode));
            formattedStack.Push((formattedNode.End, formattedNode));
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.File, formattedNode.File, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LineKeyword, formattedNode.LineKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.MinusToken, formattedNode.MinusToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Start, originalNode));
            formattedStack.Push((formattedNode.Start, formattedNode));
            return Equal;
        }
      private CompareResult CompareListPatternSyntax(ListPatternSyntax originalNode, ListPatternSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBracketToken, formattedNode.CloseBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Designation, originalNode));
            formattedStack.Push((formattedNode.Designation, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBracketToken, formattedNode.OpenBracketToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Patterns, formattedNode.Patterns, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Patterns.GetSeparators().Take(originalNode.Patterns.Count() - 1).ToList(), formattedNode.Patterns.GetSeparators().Take(formattedNode.Patterns.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareLiteralExpressionSyntax(LiteralExpressionSyntax originalNode, LiteralExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Token, formattedNode.Token, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareLoadDirectiveTriviaSyntax(LoadDirectiveTriviaSyntax originalNode, LoadDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.File, formattedNode.File, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LoadKeyword, formattedNode.LoadKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareLocalDeclarationStatementSyntax(LocalDeclarationStatementSyntax originalNode, LocalDeclarationStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.AwaitKeyword, formattedNode.AwaitKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Declaration, originalNode));
            formattedStack.Push((formattedNode.Declaration, formattedNode));
            if (originalNode.IsConst != formattedNode.IsConst) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.UsingKeyword, formattedNode.UsingKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareLocalFunctionStatementSyntax(LocalFunctionStatementSyntax originalNode, LocalFunctionStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            result = this.CompareLists(originalNode.ConstraintClauses, formattedNode.ConstraintClauses, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            originalStack.Push((originalNode.ReturnType, originalNode));
            formattedStack.Push((formattedNode.ReturnType, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.TypeParameterList, originalNode));
            formattedStack.Push((formattedNode.TypeParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareLockStatementSyntax(LockStatementSyntax originalNode, LockStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LockKeyword, formattedNode.LockKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            return Equal;
        }
      private CompareResult CompareMakeRefExpressionSyntax(MakeRefExpressionSyntax originalNode, MakeRefExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareMemberAccessExpressionSyntax(MemberAccessExpressionSyntax originalNode, MemberAccessExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareMemberBindingExpressionSyntax(MemberBindingExpressionSyntax originalNode, MemberBindingExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareMethodDeclarationSyntax(MethodDeclarationSyntax originalNode, MethodDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            result = this.CompareLists(originalNode.ConstraintClauses, formattedNode.ConstraintClauses, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ExplicitInterfaceSpecifier, originalNode));
            formattedStack.Push((formattedNode.ExplicitInterfaceSpecifier, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            originalStack.Push((originalNode.ReturnType, originalNode));
            formattedStack.Push((formattedNode.ReturnType, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.TypeParameterList, originalNode));
            formattedStack.Push((formattedNode.TypeParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareNameColonSyntax(NameColonSyntax originalNode, NameColonSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            return Equal;
        }
      private CompareResult CompareNameEqualsSyntax(NameEqualsSyntax originalNode, NameEqualsSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EqualsToken, formattedNode.EqualsToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            return Equal;
        }
      private CompareResult CompareNameMemberCrefSyntax(NameMemberCrefSyntax originalNode, NameMemberCrefSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            originalStack.Push((originalNode.Parameters, originalNode));
            formattedStack.Push((formattedNode.Parameters, formattedNode));
            return Equal;
        }
      private CompareResult CompareNamespaceDeclarationSyntax(NamespaceDeclarationSyntax originalNode, NamespaceDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Externs, formattedNode.Externs, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Members, formattedNode.Members, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.NamespaceKeyword, formattedNode.NamespaceKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareUsingDirectives(originalNode.Usings, formattedNode.Usings, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareNullableDirectiveTriviaSyntax(NullableDirectiveTriviaSyntax originalNode, NullableDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.NullableKeyword, formattedNode.NullableKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SettingToken, formattedNode.SettingToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.TargetToken, formattedNode.TargetToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareNullableTypeSyntax(NullableTypeSyntax originalNode, NullableTypeSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ElementType, originalNode));
            formattedStack.Push((formattedNode.ElementType, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.QuestionToken, formattedNode.QuestionToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareObjectCreationExpressionSyntax(ObjectCreationExpressionSyntax originalNode, ObjectCreationExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.NewKeyword, formattedNode.NewKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareOmittedArraySizeExpressionSyntax(OmittedArraySizeExpressionSyntax originalNode, OmittedArraySizeExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OmittedArraySizeExpressionToken, formattedNode.OmittedArraySizeExpressionToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareOmittedTypeArgumentSyntax(OmittedTypeArgumentSyntax originalNode, OmittedTypeArgumentSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OmittedTypeArgumentToken, formattedNode.OmittedTypeArgumentToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareOperatorDeclarationSyntax(OperatorDeclarationSyntax originalNode, OperatorDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            result = this.Compare(originalNode.CheckedKeyword, formattedNode.CheckedKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ExplicitInterfaceSpecifier, originalNode));
            formattedStack.Push((formattedNode.ExplicitInterfaceSpecifier, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OperatorKeyword, formattedNode.OperatorKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            originalStack.Push((originalNode.ReturnType, originalNode));
            formattedStack.Push((formattedNode.ReturnType, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareOperatorMemberCrefSyntax(OperatorMemberCrefSyntax originalNode, OperatorMemberCrefSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CheckedKeyword, formattedNode.CheckedKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OperatorKeyword, formattedNode.OperatorKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Parameters, originalNode));
            formattedStack.Push((formattedNode.Parameters, formattedNode));
            return Equal;
        }
      private CompareResult CompareOrderByClauseSyntax(OrderByClauseSyntax originalNode, OrderByClauseSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OrderByKeyword, formattedNode.OrderByKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Orderings, formattedNode.Orderings, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Orderings.GetSeparators().Take(originalNode.Orderings.Count() - 1).ToList(), formattedNode.Orderings.GetSeparators().Take(formattedNode.Orderings.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareOrderingSyntax(OrderingSyntax originalNode, OrderingSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.AscendingOrDescendingKeyword, formattedNode.AscendingOrDescendingKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareParameterListSyntax(ParameterListSyntax originalNode, ParameterListSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters, formattedNode.Parameters, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters.GetSeparators().Take(originalNode.Parameters.Count() - 1).ToList(), formattedNode.Parameters.GetSeparators().Take(formattedNode.Parameters.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareParameterSyntax(ParameterSyntax originalNode, ParameterSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Default, originalNode));
            formattedStack.Push((formattedNode.Default, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareParenthesizedExpressionSyntax(ParenthesizedExpressionSyntax originalNode, ParenthesizedExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareParenthesizedLambdaExpressionSyntax(ParenthesizedLambdaExpressionSyntax originalNode, ParenthesizedLambdaExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ArrowToken, formattedNode.ArrowToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.AsyncKeyword, formattedNode.AsyncKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Block, originalNode));
            formattedStack.Push((formattedNode.Block, formattedNode));
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            originalStack.Push((originalNode.ReturnType, originalNode));
            formattedStack.Push((formattedNode.ReturnType, formattedNode));
            return Equal;
        }
      private CompareResult CompareParenthesizedPatternSyntax(ParenthesizedPatternSyntax originalNode, ParenthesizedPatternSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Pattern, originalNode));
            formattedStack.Push((formattedNode.Pattern, formattedNode));
            return Equal;
        }
      private CompareResult CompareParenthesizedVariableDesignationSyntax(ParenthesizedVariableDesignationSyntax originalNode, ParenthesizedVariableDesignationSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Variables, formattedNode.Variables, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Variables.GetSeparators().Take(originalNode.Variables.Count() - 1).ToList(), formattedNode.Variables.GetSeparators().Take(formattedNode.Variables.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult ComparePointerTypeSyntax(PointerTypeSyntax originalNode, PointerTypeSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.AsteriskToken, formattedNode.AsteriskToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ElementType, originalNode));
            formattedStack.Push((formattedNode.ElementType, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult ComparePositionalPatternClauseSyntax(PositionalPatternClauseSyntax originalNode, PositionalPatternClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Subpatterns, formattedNode.Subpatterns, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Subpatterns.GetSeparators().Take(originalNode.Subpatterns.Count() - 1).ToList(), formattedNode.Subpatterns.GetSeparators().Take(formattedNode.Subpatterns.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult ComparePostfixUnaryExpressionSyntax(PostfixUnaryExpressionSyntax originalNode, PostfixUnaryExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Operand, originalNode));
            formattedStack.Push((formattedNode.Operand, formattedNode));
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult ComparePragmaChecksumDirectiveTriviaSyntax(PragmaChecksumDirectiveTriviaSyntax originalNode, PragmaChecksumDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.Bytes, formattedNode.Bytes, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ChecksumKeyword, formattedNode.ChecksumKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.File, formattedNode.File, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Guid, formattedNode.Guid, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.PragmaKeyword, formattedNode.PragmaKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult ComparePragmaWarningDirectiveTriviaSyntax(PragmaWarningDirectiveTriviaSyntax originalNode, PragmaWarningDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.DisableOrRestoreKeyword, formattedNode.DisableOrRestoreKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.ErrorCodes, formattedNode.ErrorCodes, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.ErrorCodes.GetSeparators().Take(originalNode.ErrorCodes.Count() - 1).ToList(), formattedNode.ErrorCodes.GetSeparators().Take(formattedNode.ErrorCodes.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.PragmaKeyword, formattedNode.PragmaKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.WarningKeyword, formattedNode.WarningKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult ComparePredefinedTypeSyntax(PredefinedTypeSyntax originalNode, PredefinedTypeSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult ComparePrefixUnaryExpressionSyntax(PrefixUnaryExpressionSyntax originalNode, PrefixUnaryExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Operand, originalNode));
            formattedStack.Push((formattedNode.Operand, formattedNode));
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult ComparePrimaryConstructorBaseTypeSyntax(PrimaryConstructorBaseTypeSyntax originalNode, PrimaryConstructorBaseTypeSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult ComparePropertyDeclarationSyntax(PropertyDeclarationSyntax originalNode, PropertyDeclarationSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.AccessorList, originalNode));
            formattedStack.Push((formattedNode.AccessorList, formattedNode));
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ExplicitInterfaceSpecifier, originalNode));
            formattedStack.Push((formattedNode.ExplicitInterfaceSpecifier, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult ComparePropertyPatternClauseSyntax(PropertyPatternClauseSyntax originalNode, PropertyPatternClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Subpatterns, formattedNode.Subpatterns, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Subpatterns.GetSeparators().Take(originalNode.Subpatterns.Count() - 1).ToList(), formattedNode.Subpatterns.GetSeparators().Take(formattedNode.Subpatterns.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareQualifiedCrefSyntax(QualifiedCrefSyntax originalNode, QualifiedCrefSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Container, originalNode));
            formattedStack.Push((formattedNode.Container, formattedNode));
            result = this.Compare(originalNode.DotToken, formattedNode.DotToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Member, originalNode));
            formattedStack.Push((formattedNode.Member, formattedNode));
            return Equal;
        }
      private CompareResult CompareQualifiedNameSyntax(QualifiedNameSyntax originalNode, QualifiedNameSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DotToken, formattedNode.DotToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Left, originalNode));
            formattedStack.Push((formattedNode.Left, formattedNode));
            originalStack.Push((originalNode.Right, originalNode));
            formattedStack.Push((formattedNode.Right, formattedNode));
            return Equal;
        }
      private CompareResult CompareQueryBodySyntax(QueryBodySyntax originalNode, QueryBodySyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Clauses, formattedNode.Clauses, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Continuation, originalNode));
            formattedStack.Push((formattedNode.Continuation, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.SelectOrGroup, originalNode));
            formattedStack.Push((formattedNode.SelectOrGroup, formattedNode));
            return Equal;
        }
      private CompareResult CompareQueryContinuationSyntax(QueryContinuationSyntax originalNode, QueryContinuationSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.IntoKeyword, formattedNode.IntoKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareQueryExpressionSyntax(QueryExpressionSyntax originalNode, QueryExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            originalStack.Push((originalNode.FromClause, originalNode));
            formattedStack.Push((formattedNode.FromClause, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareRangeExpressionSyntax(RangeExpressionSyntax originalNode, RangeExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.LeftOperand, originalNode));
            formattedStack.Push((formattedNode.LeftOperand, formattedNode));
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.RightOperand, originalNode));
            formattedStack.Push((formattedNode.RightOperand, formattedNode));
            return Equal;
        }
      private CompareResult CompareRecordDeclarationSyntax(RecordDeclarationSyntax originalNode, RecordDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.BaseList, originalNode));
            formattedStack.Push((formattedNode.BaseList, formattedNode));
            result = this.Compare(originalNode.ClassOrStructKeyword, formattedNode.ClassOrStructKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.ConstraintClauses, formattedNode.ConstraintClauses, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Members, formattedNode.Members, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.TypeParameterList, originalNode));
            formattedStack.Push((formattedNode.TypeParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareRecursivePatternSyntax(RecursivePatternSyntax originalNode, RecursivePatternSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Designation, originalNode));
            formattedStack.Push((formattedNode.Designation, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.PositionalPatternClause, originalNode));
            formattedStack.Push((formattedNode.PositionalPatternClause, formattedNode));
            originalStack.Push((originalNode.PropertyPatternClause, originalNode));
            formattedStack.Push((formattedNode.PropertyPatternClause, formattedNode));
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareReferenceDirectiveTriviaSyntax(ReferenceDirectiveTriviaSyntax originalNode, ReferenceDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.File, formattedNode.File, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ReferenceKeyword, formattedNode.ReferenceKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareRefExpressionSyntax(RefExpressionSyntax originalNode, RefExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.RefKeyword, formattedNode.RefKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareRefStructConstraintSyntax(RefStructConstraintSyntax originalNode, RefStructConstraintSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.RefKeyword, formattedNode.RefKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.StructKeyword, formattedNode.StructKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareRefTypeExpressionSyntax(RefTypeExpressionSyntax originalNode, RefTypeExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareRefTypeSyntax(RefTypeSyntax originalNode, RefTypeSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ReadOnlyKeyword, formattedNode.ReadOnlyKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.RefKeyword, formattedNode.RefKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareRefValueExpressionSyntax(RefValueExpressionSyntax originalNode, RefValueExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Comma, formattedNode.Comma, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareRegionDirectiveTriviaSyntax(RegionDirectiveTriviaSyntax originalNode, RegionDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.RegionKeyword, formattedNode.RegionKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareRelationalPatternSyntax(RelationalPatternSyntax originalNode, RelationalPatternSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareReturnStatementSyntax(ReturnStatementSyntax originalNode, ReturnStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ReturnKeyword, formattedNode.ReturnKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareScopedTypeSyntax(ScopedTypeSyntax originalNode, ScopedTypeSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ScopedKeyword, formattedNode.ScopedKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareSelectClauseSyntax(SelectClauseSyntax originalNode, SelectClauseSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.SelectKeyword, formattedNode.SelectKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareShebangDirectiveTriviaSyntax(ShebangDirectiveTriviaSyntax originalNode, ShebangDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ExclamationToken, formattedNode.ExclamationToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareSimpleBaseTypeSyntax(SimpleBaseTypeSyntax originalNode, SimpleBaseTypeSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareSimpleLambdaExpressionSyntax(SimpleLambdaExpressionSyntax originalNode, SimpleLambdaExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ArrowToken, formattedNode.ArrowToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.AsyncKeyword, formattedNode.AsyncKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Block, originalNode));
            formattedStack.Push((formattedNode.Block, formattedNode));
            originalStack.Push((originalNode.Body, originalNode));
            formattedStack.Push((formattedNode.Body, formattedNode));
            originalStack.Push((originalNode.ExpressionBody, originalNode));
            formattedStack.Push((formattedNode.ExpressionBody, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Parameter, originalNode));
            formattedStack.Push((formattedNode.Parameter, formattedNode));
            return Equal;
        }
      private CompareResult CompareSingleVariableDesignationSyntax(SingleVariableDesignationSyntax originalNode, SingleVariableDesignationSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareSizeOfExpressionSyntax(SizeOfExpressionSyntax originalNode, SizeOfExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareSkippedTokensTriviaSyntax(SkippedTokensTriviaSyntax originalNode, SkippedTokensTriviaSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Tokens, formattedNode.Tokens, Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareSlicePatternSyntax(SlicePatternSyntax originalNode, SlicePatternSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DotDotToken, formattedNode.DotDotToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Pattern, originalNode));
            formattedStack.Push((formattedNode.Pattern, formattedNode));
            return Equal;
        }
      private CompareResult CompareSpreadElementSyntax(SpreadElementSyntax originalNode, SpreadElementSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareStackAllocArrayCreationExpressionSyntax(StackAllocArrayCreationExpressionSyntax originalNode, StackAllocArrayCreationExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.StackAllocKeyword, formattedNode.StackAllocKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareStructDeclarationSyntax(StructDeclarationSyntax originalNode, StructDeclarationSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.BaseList, originalNode));
            formattedStack.Push((formattedNode.BaseList, formattedNode));
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.ConstraintClauses, formattedNode.ConstraintClauses, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Members, formattedNode.Members, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Modifiers.OrderBy(o => o.Text).ToList(), formattedNode.Modifiers.OrderBy(o => o.Text).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.ParameterList, originalNode));
            formattedStack.Push((formattedNode.ParameterList, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.TypeParameterList, originalNode));
            formattedStack.Push((formattedNode.TypeParameterList, formattedNode));
            return Equal;
        }
      private CompareResult CompareSubpatternSyntax(SubpatternSyntax originalNode, SubpatternSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ExpressionColon, originalNode));
            formattedStack.Push((formattedNode.ExpressionColon, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.NameColon, originalNode));
            formattedStack.Push((formattedNode.NameColon, formattedNode));
            originalStack.Push((originalNode.Pattern, originalNode));
            formattedStack.Push((formattedNode.Pattern, formattedNode));
            return Equal;
        }
      private CompareResult CompareSwitchExpressionArmSyntax(SwitchExpressionArmSyntax originalNode, SwitchExpressionArmSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EqualsGreaterThanToken, formattedNode.EqualsGreaterThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Pattern, originalNode));
            formattedStack.Push((formattedNode.Pattern, formattedNode));
            originalStack.Push((originalNode.WhenClause, originalNode));
            formattedStack.Push((formattedNode.WhenClause, formattedNode));
            return Equal;
        }
      private CompareResult CompareSwitchExpressionSyntax(SwitchExpressionSyntax originalNode, SwitchExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Arms, formattedNode.Arms, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Arms.GetSeparators().Take(originalNode.Arms.Count() - 1).ToList(), formattedNode.Arms.GetSeparators().Take(formattedNode.Arms.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.GoverningExpression, originalNode));
            formattedStack.Push((formattedNode.GoverningExpression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SwitchKeyword, formattedNode.SwitchKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareSwitchSectionSyntax(SwitchSectionSyntax originalNode, SwitchSectionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.Labels, formattedNode.Labels, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Statements, formattedNode.Statements, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareSwitchStatementSyntax(SwitchStatementSyntax originalNode, SwitchStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseBraceToken, formattedNode.CloseBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenBraceToken, formattedNode.OpenBraceToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Sections, formattedNode.Sections, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SwitchKeyword, formattedNode.SwitchKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareThisExpressionSyntax(ThisExpressionSyntax originalNode, ThisExpressionSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Token, formattedNode.Token, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareThrowExpressionSyntax(ThrowExpressionSyntax originalNode, ThrowExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ThrowKeyword, formattedNode.ThrowKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareThrowStatementSyntax(ThrowStatementSyntax originalNode, ThrowStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.ThrowKeyword, formattedNode.ThrowKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareTryStatementSyntax(TryStatementSyntax originalNode, TryStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Block, originalNode));
            formattedStack.Push((formattedNode.Block, formattedNode));
            result = this.CompareLists(originalNode.Catches, formattedNode.Catches, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Finally, originalNode));
            formattedStack.Push((formattedNode.Finally, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.TryKeyword, formattedNode.TryKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareTupleElementSyntax(TupleElementSyntax originalNode, TupleElementSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareTupleExpressionSyntax(TupleExpressionSyntax originalNode, TupleExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Arguments, formattedNode.Arguments, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Arguments.GetSeparators().Take(originalNode.Arguments.Count() - 1).ToList(), formattedNode.Arguments.GetSeparators().Take(formattedNode.Arguments.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareTupleTypeSyntax(TupleTypeSyntax originalNode, TupleTypeSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Elements, formattedNode.Elements, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Elements.GetSeparators().Take(originalNode.Elements.Count() - 1).ToList(), formattedNode.Elements.GetSeparators().Take(formattedNode.Elements.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareTypeArgumentListSyntax(TypeArgumentListSyntax originalNode, TypeArgumentListSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Arguments, formattedNode.Arguments, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Arguments.GetSeparators().Take(originalNode.Arguments.Count() - 1).ToList(), formattedNode.Arguments.GetSeparators().Take(formattedNode.Arguments.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.GreaterThanToken, formattedNode.GreaterThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LessThanToken, formattedNode.LessThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareTypeConstraintSyntax(TypeConstraintSyntax originalNode, TypeConstraintSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareTypeCrefSyntax(TypeCrefSyntax originalNode, TypeCrefSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareTypeOfExpressionSyntax(TypeOfExpressionSyntax originalNode, TypeOfExpressionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Keyword, formattedNode.Keyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareTypeParameterConstraintClauseSyntax(TypeParameterConstraintClauseSyntax originalNode, TypeParameterConstraintClauseSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Constraints, formattedNode.Constraints, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Constraints.GetSeparators().Take(originalNode.Constraints.Count() - 1).ToList(), formattedNode.Constraints.GetSeparators().Take(formattedNode.Constraints.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.WhereKeyword, formattedNode.WhereKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareTypeParameterListSyntax(TypeParameterListSyntax originalNode, TypeParameterListSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.GreaterThanToken, formattedNode.GreaterThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LessThanToken, formattedNode.LessThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters, formattedNode.Parameters, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Parameters.GetSeparators().Take(originalNode.Parameters.Count() - 1).ToList(), formattedNode.Parameters.GetSeparators().Take(formattedNode.Parameters.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareTypeParameterSyntax(TypeParameterSyntax originalNode, TypeParameterSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.VarianceKeyword, formattedNode.VarianceKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareTypePatternSyntax(TypePatternSyntax originalNode, TypePatternSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            return Equal;
        }
      private CompareResult CompareUnaryPatternSyntax(UnaryPatternSyntax originalNode, UnaryPatternSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OperatorToken, formattedNode.OperatorToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Pattern, originalNode));
            formattedStack.Push((formattedNode.Pattern, formattedNode));
            return Equal;
        }
      private CompareResult CompareUndefDirectiveTriviaSyntax(UndefDirectiveTriviaSyntax originalNode, UndefDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Name, formattedNode.Name, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.UndefKeyword, formattedNode.UndefKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareUnsafeStatementSyntax(UnsafeStatementSyntax originalNode, UnsafeStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Block, originalNode));
            formattedStack.Push((formattedNode.Block, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.UnsafeKeyword, formattedNode.UnsafeKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareUsingDirectiveSyntax(UsingDirectiveSyntax originalNode, UsingDirectiveSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Alias, originalNode));
            formattedStack.Push((formattedNode.Alias, formattedNode));
            result = this.Compare(originalNode.GlobalKeyword, formattedNode.GlobalKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            originalStack.Push((originalNode.NamespaceOrType, originalNode));
            formattedStack.Push((formattedNode.NamespaceOrType, formattedNode));
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.StaticKeyword, formattedNode.StaticKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.UnsafeKeyword, formattedNode.UnsafeKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.UsingKeyword, formattedNode.UsingKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareUsingStatementSyntax(UsingStatementSyntax originalNode, UsingStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.AwaitKeyword, formattedNode.AwaitKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Declaration, originalNode));
            formattedStack.Push((formattedNode.Declaration, formattedNode));
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            result = this.Compare(originalNode.UsingKeyword, formattedNode.UsingKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareVariableDeclarationSyntax(VariableDeclarationSyntax originalNode, VariableDeclarationSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Type, originalNode));
            formattedStack.Push((formattedNode.Type, formattedNode));
            result = this.CompareLists(originalNode.Variables, formattedNode.Variables, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.Variables.GetSeparators().Take(originalNode.Variables.Count() - 1).ToList(), formattedNode.Variables.GetSeparators().Take(formattedNode.Variables.Count() - 1).ToList(), Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareVariableDeclaratorSyntax(VariableDeclaratorSyntax originalNode, VariableDeclaratorSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.ArgumentList, originalNode));
            formattedStack.Push((formattedNode.ArgumentList, formattedNode));
            result = this.Compare(originalNode.Identifier, formattedNode.Identifier, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            return Equal;
        }
      private CompareResult CompareVarPatternSyntax(VarPatternSyntax originalNode, VarPatternSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Designation, originalNode));
            formattedStack.Push((formattedNode.Designation, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.VarKeyword, formattedNode.VarKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareWarningDirectiveTriviaSyntax(WarningDirectiveTriviaSyntax originalNode, WarningDirectiveTriviaSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.DirectiveNameToken, formattedNode.DirectiveNameToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EndOfDirectiveToken, formattedNode.EndOfDirectiveToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.HashToken, formattedNode.HashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsActive != formattedNode.IsActive) return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.WarningKeyword, formattedNode.WarningKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareWhenClauseSyntax(WhenClauseSyntax originalNode, WhenClauseSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.WhenKeyword, formattedNode.WhenKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareWhereClauseSyntax(WhereClauseSyntax originalNode, WhereClauseSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.WhereKeyword, formattedNode.WhereKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareWhileStatementSyntax(WhileStatementSyntax originalNode, WhileStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.CloseParenToken, formattedNode.CloseParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Condition, originalNode));
            formattedStack.Push((formattedNode.Condition, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.OpenParenToken, formattedNode.OpenParenToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Statement, originalNode));
            formattedStack.Push((formattedNode.Statement, formattedNode));
            result = this.Compare(originalNode.WhileKeyword, formattedNode.WhileKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareWithExpressionSyntax(WithExpressionSyntax originalNode, WithExpressionSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            originalStack.Push((originalNode.Initializer, originalNode));
            formattedStack.Push((formattedNode.Initializer, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.WithKeyword, formattedNode.WithKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlCDataSectionSyntax(XmlCDataSectionSyntax originalNode, XmlCDataSectionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EndCDataToken, formattedNode.EndCDataToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.StartCDataToken, formattedNode.StartCDataToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.TextTokens, formattedNode.TextTokens, Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlCommentSyntax(XmlCommentSyntax originalNode, XmlCommentSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LessThanExclamationMinusMinusToken, formattedNode.LessThanExclamationMinusMinusToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.MinusMinusGreaterThanToken, formattedNode.MinusMinusGreaterThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.TextTokens, formattedNode.TextTokens, Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlCrefAttributeSyntax(XmlCrefAttributeSyntax originalNode, XmlCrefAttributeSyntax formattedNode)
      {
          CompareResult result;
            originalStack.Push((originalNode.Cref, originalNode));
            formattedStack.Push((formattedNode.Cref, formattedNode));
            result = this.Compare(originalNode.EndQuoteToken, formattedNode.EndQuoteToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EqualsToken, formattedNode.EqualsToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.StartQuoteToken, formattedNode.StartQuoteToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlElementEndTagSyntax(XmlElementEndTagSyntax originalNode, XmlElementEndTagSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.GreaterThanToken, formattedNode.GreaterThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LessThanSlashToken, formattedNode.LessThanSlashToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            return Equal;
        }
      private CompareResult CompareXmlElementStartTagSyntax(XmlElementStartTagSyntax originalNode, XmlElementStartTagSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Attributes, formattedNode.Attributes, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.GreaterThanToken, formattedNode.GreaterThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LessThanToken, formattedNode.LessThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            return Equal;
        }
      private CompareResult CompareXmlElementSyntax(XmlElementSyntax originalNode, XmlElementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Content, formattedNode.Content, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.EndTag, originalNode));
            formattedStack.Push((formattedNode.EndTag, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.StartTag, originalNode));
            formattedStack.Push((formattedNode.StartTag, formattedNode));
            return Equal;
        }
      private CompareResult CompareXmlEmptyElementSyntax(XmlEmptyElementSyntax originalNode, XmlEmptyElementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.Attributes, formattedNode.Attributes, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LessThanToken, formattedNode.LessThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.SlashGreaterThanToken, formattedNode.SlashGreaterThanToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlNameAttributeSyntax(XmlNameAttributeSyntax originalNode, XmlNameAttributeSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EndQuoteToken, formattedNode.EndQuoteToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EqualsToken, formattedNode.EqualsToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Identifier, originalNode));
            formattedStack.Push((formattedNode.Identifier, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.StartQuoteToken, formattedNode.StartQuoteToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlNameSyntax(XmlNameSyntax originalNode, XmlNameSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.LocalName, formattedNode.LocalName, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Prefix, originalNode));
            formattedStack.Push((formattedNode.Prefix, formattedNode));
            return Equal;
        }
      private CompareResult CompareXmlPrefixSyntax(XmlPrefixSyntax originalNode, XmlPrefixSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.ColonToken, formattedNode.ColonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.Prefix, formattedNode.Prefix, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlProcessingInstructionSyntax(XmlProcessingInstructionSyntax originalNode, XmlProcessingInstructionSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EndProcessingInstructionToken, formattedNode.EndProcessingInstructionToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.StartProcessingInstructionToken, formattedNode.StartProcessingInstructionToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.TextTokens, formattedNode.TextTokens, Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlTextAttributeSyntax(XmlTextAttributeSyntax originalNode, XmlTextAttributeSyntax formattedNode)
      {
          CompareResult result;
            result = this.Compare(originalNode.EndQuoteToken, formattedNode.EndQuoteToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.EqualsToken, formattedNode.EqualsToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            originalStack.Push((originalNode.Name, originalNode));
            formattedStack.Push((formattedNode.Name, formattedNode));
            result = this.Compare(originalNode.StartQuoteToken, formattedNode.StartQuoteToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.CompareLists(originalNode.TextTokens, formattedNode.TextTokens, Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareXmlTextSyntax(XmlTextSyntax originalNode, XmlTextSyntax formattedNode)
      {
          CompareResult result;
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.CompareLists(originalNode.TextTokens, formattedNode.TextTokens, Compare, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            return Equal;
        }
      private CompareResult CompareYieldStatementSyntax(YieldStatementSyntax originalNode, YieldStatementSyntax formattedNode)
      {
          CompareResult result;
            result = this.CompareLists(originalNode.AttributeLists, formattedNode.AttributeLists, null, o => o.Span, originalNode.Span, formattedNode.Span);
            if (result.IsInvalid) return result;
            originalStack.Push((originalNode.Expression, originalNode));
            formattedStack.Push((formattedNode.Expression, formattedNode));
            if (originalNode.IsMissing != formattedNode.IsMissing) return NotEqual(originalNode, formattedNode);
            result = this.Compare(originalNode.ReturnOrBreakKeyword, formattedNode.ReturnOrBreakKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.SemicolonToken, formattedNode.SemicolonToken, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            result = this.Compare(originalNode.YieldKeyword, formattedNode.YieldKeyword, originalNode, formattedNode);
            if (result.IsInvalid) return result;
            return Equal;
        }
    }
}
