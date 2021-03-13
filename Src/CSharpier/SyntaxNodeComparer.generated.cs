#pragma warning disable CS0168
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class SyntaxNodeComparer
    {
        public CompareResult Compare(
            SyntaxNode originalNode,
            SyntaxNode formattedNode)
        {
            if (originalNode == null && formattedNode == null)
            {
                return Equal;
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
                case IdentifierNameSyntax identifierNameSyntax:
                    return this.CompareIdentifierNameSyntax(
                        identifierNameSyntax,
                        formattedNode as IdentifierNameSyntax);
                case QualifiedNameSyntax qualifiedNameSyntax:
                    return this.CompareQualifiedNameSyntax(
                        qualifiedNameSyntax,
                        formattedNode as QualifiedNameSyntax);
                case GenericNameSyntax genericNameSyntax:
                    return this.CompareGenericNameSyntax(
                        genericNameSyntax,
                        formattedNode as GenericNameSyntax);
                case TypeArgumentListSyntax typeArgumentListSyntax:
                    return this.CompareTypeArgumentListSyntax(
                        typeArgumentListSyntax,
                        formattedNode as TypeArgumentListSyntax);
                case AliasQualifiedNameSyntax aliasQualifiedNameSyntax:
                    return this.CompareAliasQualifiedNameSyntax(
                        aliasQualifiedNameSyntax,
                        formattedNode as AliasQualifiedNameSyntax);
                case PredefinedTypeSyntax predefinedTypeSyntax:
                    return this.ComparePredefinedTypeSyntax(
                        predefinedTypeSyntax,
                        formattedNode as PredefinedTypeSyntax);
                case ArrayTypeSyntax arrayTypeSyntax:
                    return this.CompareArrayTypeSyntax(
                        arrayTypeSyntax,
                        formattedNode as ArrayTypeSyntax);
                case ArrayRankSpecifierSyntax arrayRankSpecifierSyntax:
                    return this.CompareArrayRankSpecifierSyntax(
                        arrayRankSpecifierSyntax,
                        formattedNode as ArrayRankSpecifierSyntax);
                case PointerTypeSyntax pointerTypeSyntax:
                    return this.ComparePointerTypeSyntax(
                        pointerTypeSyntax,
                        formattedNode as PointerTypeSyntax);
                case FunctionPointerTypeSyntax functionPointerTypeSyntax:
                    return this.CompareFunctionPointerTypeSyntax(
                        functionPointerTypeSyntax,
                        formattedNode as FunctionPointerTypeSyntax);
                case FunctionPointerParameterListSyntax functionPointerParameterListSyntax:
                    return this.CompareFunctionPointerParameterListSyntax(
                        functionPointerParameterListSyntax,
                        formattedNode as FunctionPointerParameterListSyntax);
                case FunctionPointerCallingConventionSyntax functionPointerCallingConventionSyntax:
                    return this.CompareFunctionPointerCallingConventionSyntax(
                        functionPointerCallingConventionSyntax,
                        formattedNode as FunctionPointerCallingConventionSyntax);
                case FunctionPointerUnmanagedCallingConventionListSyntax functionPointerUnmanagedCallingConventionListSyntax:
                    return this.CompareFunctionPointerUnmanagedCallingConventionListSyntax(
                        functionPointerUnmanagedCallingConventionListSyntax,
                        formattedNode as FunctionPointerUnmanagedCallingConventionListSyntax);
                case FunctionPointerUnmanagedCallingConventionSyntax functionPointerUnmanagedCallingConventionSyntax:
                    return this.CompareFunctionPointerUnmanagedCallingConventionSyntax(
                        functionPointerUnmanagedCallingConventionSyntax,
                        formattedNode as FunctionPointerUnmanagedCallingConventionSyntax);
                case NullableTypeSyntax nullableTypeSyntax:
                    return this.CompareNullableTypeSyntax(
                        nullableTypeSyntax,
                        formattedNode as NullableTypeSyntax);
                case TupleTypeSyntax tupleTypeSyntax:
                    return this.CompareTupleTypeSyntax(
                        tupleTypeSyntax,
                        formattedNode as TupleTypeSyntax);
                case TupleElementSyntax tupleElementSyntax:
                    return this.CompareTupleElementSyntax(
                        tupleElementSyntax,
                        formattedNode as TupleElementSyntax);
                case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax:
                    return this.CompareOmittedTypeArgumentSyntax(
                        omittedTypeArgumentSyntax,
                        formattedNode as OmittedTypeArgumentSyntax);
                case RefTypeSyntax refTypeSyntax:
                    return this.CompareRefTypeSyntax(
                        refTypeSyntax,
                        formattedNode as RefTypeSyntax);
                case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax:
                    return this.CompareParenthesizedExpressionSyntax(
                        parenthesizedExpressionSyntax,
                        formattedNode as ParenthesizedExpressionSyntax);
                case TupleExpressionSyntax tupleExpressionSyntax:
                    return this.CompareTupleExpressionSyntax(
                        tupleExpressionSyntax,
                        formattedNode as TupleExpressionSyntax);
                case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax:
                    return this.ComparePrefixUnaryExpressionSyntax(
                        prefixUnaryExpressionSyntax,
                        formattedNode as PrefixUnaryExpressionSyntax);
                case AwaitExpressionSyntax awaitExpressionSyntax:
                    return this.CompareAwaitExpressionSyntax(
                        awaitExpressionSyntax,
                        formattedNode as AwaitExpressionSyntax);
                case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax:
                    return this.ComparePostfixUnaryExpressionSyntax(
                        postfixUnaryExpressionSyntax,
                        formattedNode as PostfixUnaryExpressionSyntax);
                case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
                    return this.CompareMemberAccessExpressionSyntax(
                        memberAccessExpressionSyntax,
                        formattedNode as MemberAccessExpressionSyntax);
                case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax:
                    return this.CompareConditionalAccessExpressionSyntax(
                        conditionalAccessExpressionSyntax,
                        formattedNode as ConditionalAccessExpressionSyntax);
                case MemberBindingExpressionSyntax memberBindingExpressionSyntax:
                    return this.CompareMemberBindingExpressionSyntax(
                        memberBindingExpressionSyntax,
                        formattedNode as MemberBindingExpressionSyntax);
                case ElementBindingExpressionSyntax elementBindingExpressionSyntax:
                    return this.CompareElementBindingExpressionSyntax(
                        elementBindingExpressionSyntax,
                        formattedNode as ElementBindingExpressionSyntax);
                case RangeExpressionSyntax rangeExpressionSyntax:
                    return this.CompareRangeExpressionSyntax(
                        rangeExpressionSyntax,
                        formattedNode as RangeExpressionSyntax);
                case ImplicitElementAccessSyntax implicitElementAccessSyntax:
                    return this.CompareImplicitElementAccessSyntax(
                        implicitElementAccessSyntax,
                        formattedNode as ImplicitElementAccessSyntax);
                case BinaryExpressionSyntax binaryExpressionSyntax:
                    return this.CompareBinaryExpressionSyntax(
                        binaryExpressionSyntax,
                        formattedNode as BinaryExpressionSyntax);
                case AssignmentExpressionSyntax assignmentExpressionSyntax:
                    return this.CompareAssignmentExpressionSyntax(
                        assignmentExpressionSyntax,
                        formattedNode as AssignmentExpressionSyntax);
                case ConditionalExpressionSyntax conditionalExpressionSyntax:
                    return this.CompareConditionalExpressionSyntax(
                        conditionalExpressionSyntax,
                        formattedNode as ConditionalExpressionSyntax);
                case ThisExpressionSyntax thisExpressionSyntax:
                    return this.CompareThisExpressionSyntax(
                        thisExpressionSyntax,
                        formattedNode as ThisExpressionSyntax);
                case BaseExpressionSyntax baseExpressionSyntax:
                    return this.CompareBaseExpressionSyntax(
                        baseExpressionSyntax,
                        formattedNode as BaseExpressionSyntax);
                case LiteralExpressionSyntax literalExpressionSyntax:
                    return this.CompareLiteralExpressionSyntax(
                        literalExpressionSyntax,
                        formattedNode as LiteralExpressionSyntax);
                case MakeRefExpressionSyntax makeRefExpressionSyntax:
                    return this.CompareMakeRefExpressionSyntax(
                        makeRefExpressionSyntax,
                        formattedNode as MakeRefExpressionSyntax);
                case RefTypeExpressionSyntax refTypeExpressionSyntax:
                    return this.CompareRefTypeExpressionSyntax(
                        refTypeExpressionSyntax,
                        formattedNode as RefTypeExpressionSyntax);
                case RefValueExpressionSyntax refValueExpressionSyntax:
                    return this.CompareRefValueExpressionSyntax(
                        refValueExpressionSyntax,
                        formattedNode as RefValueExpressionSyntax);
                case CheckedExpressionSyntax checkedExpressionSyntax:
                    return this.CompareCheckedExpressionSyntax(
                        checkedExpressionSyntax,
                        formattedNode as CheckedExpressionSyntax);
                case DefaultExpressionSyntax defaultExpressionSyntax:
                    return this.CompareDefaultExpressionSyntax(
                        defaultExpressionSyntax,
                        formattedNode as DefaultExpressionSyntax);
                case TypeOfExpressionSyntax typeOfExpressionSyntax:
                    return this.CompareTypeOfExpressionSyntax(
                        typeOfExpressionSyntax,
                        formattedNode as TypeOfExpressionSyntax);
                case SizeOfExpressionSyntax sizeOfExpressionSyntax:
                    return this.CompareSizeOfExpressionSyntax(
                        sizeOfExpressionSyntax,
                        formattedNode as SizeOfExpressionSyntax);
                case InvocationExpressionSyntax invocationExpressionSyntax:
                    return this.CompareInvocationExpressionSyntax(
                        invocationExpressionSyntax,
                        formattedNode as InvocationExpressionSyntax);
                case ElementAccessExpressionSyntax elementAccessExpressionSyntax:
                    return this.CompareElementAccessExpressionSyntax(
                        elementAccessExpressionSyntax,
                        formattedNode as ElementAccessExpressionSyntax);
                case ArgumentListSyntax argumentListSyntax:
                    return this.CompareArgumentListSyntax(
                        argumentListSyntax,
                        formattedNode as ArgumentListSyntax);
                case BracketedArgumentListSyntax bracketedArgumentListSyntax:
                    return this.CompareBracketedArgumentListSyntax(
                        bracketedArgumentListSyntax,
                        formattedNode as BracketedArgumentListSyntax);
                case ArgumentSyntax argumentSyntax:
                    return this.CompareArgumentSyntax(
                        argumentSyntax,
                        formattedNode as ArgumentSyntax);
                case NameColonSyntax nameColonSyntax:
                    return this.CompareNameColonSyntax(
                        nameColonSyntax,
                        formattedNode as NameColonSyntax);
                case DeclarationExpressionSyntax declarationExpressionSyntax:
                    return this.CompareDeclarationExpressionSyntax(
                        declarationExpressionSyntax,
                        formattedNode as DeclarationExpressionSyntax);
                case CastExpressionSyntax castExpressionSyntax:
                    return this.CompareCastExpressionSyntax(
                        castExpressionSyntax,
                        formattedNode as CastExpressionSyntax);
                case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax:
                    return this.CompareAnonymousMethodExpressionSyntax(
                        anonymousMethodExpressionSyntax,
                        formattedNode as AnonymousMethodExpressionSyntax);
                case SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax:
                    return this.CompareSimpleLambdaExpressionSyntax(
                        simpleLambdaExpressionSyntax,
                        formattedNode as SimpleLambdaExpressionSyntax);
                case RefExpressionSyntax refExpressionSyntax:
                    return this.CompareRefExpressionSyntax(
                        refExpressionSyntax,
                        formattedNode as RefExpressionSyntax);
                case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax:
                    return this.CompareParenthesizedLambdaExpressionSyntax(
                        parenthesizedLambdaExpressionSyntax,
                        formattedNode as ParenthesizedLambdaExpressionSyntax);
                case InitializerExpressionSyntax initializerExpressionSyntax:
                    return this.CompareInitializerExpressionSyntax(
                        initializerExpressionSyntax,
                        formattedNode as InitializerExpressionSyntax);
                case ImplicitObjectCreationExpressionSyntax implicitObjectCreationExpressionSyntax:
                    return this.CompareImplicitObjectCreationExpressionSyntax(
                        implicitObjectCreationExpressionSyntax,
                        formattedNode as ImplicitObjectCreationExpressionSyntax);
                case ObjectCreationExpressionSyntax objectCreationExpressionSyntax:
                    return this.CompareObjectCreationExpressionSyntax(
                        objectCreationExpressionSyntax,
                        formattedNode as ObjectCreationExpressionSyntax);
                case WithExpressionSyntax withExpressionSyntax:
                    return this.CompareWithExpressionSyntax(
                        withExpressionSyntax,
                        formattedNode as WithExpressionSyntax);
                case AnonymousObjectMemberDeclaratorSyntax anonymousObjectMemberDeclaratorSyntax:
                    return this.CompareAnonymousObjectMemberDeclaratorSyntax(
                        anonymousObjectMemberDeclaratorSyntax,
                        formattedNode as AnonymousObjectMemberDeclaratorSyntax);
                case AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax:
                    return this.CompareAnonymousObjectCreationExpressionSyntax(
                        anonymousObjectCreationExpressionSyntax,
                        formattedNode as AnonymousObjectCreationExpressionSyntax);
                case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax:
                    return this.CompareArrayCreationExpressionSyntax(
                        arrayCreationExpressionSyntax,
                        formattedNode as ArrayCreationExpressionSyntax);
                case ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax:
                    return this.CompareImplicitArrayCreationExpressionSyntax(
                        implicitArrayCreationExpressionSyntax,
                        formattedNode as ImplicitArrayCreationExpressionSyntax);
                case StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax:
                    return this.CompareStackAllocArrayCreationExpressionSyntax(
                        stackAllocArrayCreationExpressionSyntax,
                        formattedNode as StackAllocArrayCreationExpressionSyntax);
                case ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax:
                    return this.CompareImplicitStackAllocArrayCreationExpressionSyntax(
                        implicitStackAllocArrayCreationExpressionSyntax,
                        formattedNode as ImplicitStackAllocArrayCreationExpressionSyntax);
                case QueryExpressionSyntax queryExpressionSyntax:
                    return this.CompareQueryExpressionSyntax(
                        queryExpressionSyntax,
                        formattedNode as QueryExpressionSyntax);
                case QueryBodySyntax queryBodySyntax:
                    return this.CompareQueryBodySyntax(
                        queryBodySyntax,
                        formattedNode as QueryBodySyntax);
                case FromClauseSyntax fromClauseSyntax:
                    return this.CompareFromClauseSyntax(
                        fromClauseSyntax,
                        formattedNode as FromClauseSyntax);
                case LetClauseSyntax letClauseSyntax:
                    return this.CompareLetClauseSyntax(
                        letClauseSyntax,
                        formattedNode as LetClauseSyntax);
                case JoinClauseSyntax joinClauseSyntax:
                    return this.CompareJoinClauseSyntax(
                        joinClauseSyntax,
                        formattedNode as JoinClauseSyntax);
                case JoinIntoClauseSyntax joinIntoClauseSyntax:
                    return this.CompareJoinIntoClauseSyntax(
                        joinIntoClauseSyntax,
                        formattedNode as JoinIntoClauseSyntax);
                case WhereClauseSyntax whereClauseSyntax:
                    return this.CompareWhereClauseSyntax(
                        whereClauseSyntax,
                        formattedNode as WhereClauseSyntax);
                case OrderByClauseSyntax orderByClauseSyntax:
                    return this.CompareOrderByClauseSyntax(
                        orderByClauseSyntax,
                        formattedNode as OrderByClauseSyntax);
                case OrderingSyntax orderingSyntax:
                    return this.CompareOrderingSyntax(
                        orderingSyntax,
                        formattedNode as OrderingSyntax);
                case SelectClauseSyntax selectClauseSyntax:
                    return this.CompareSelectClauseSyntax(
                        selectClauseSyntax,
                        formattedNode as SelectClauseSyntax);
                case GroupClauseSyntax groupClauseSyntax:
                    return this.CompareGroupClauseSyntax(
                        groupClauseSyntax,
                        formattedNode as GroupClauseSyntax);
                case QueryContinuationSyntax queryContinuationSyntax:
                    return this.CompareQueryContinuationSyntax(
                        queryContinuationSyntax,
                        formattedNode as QueryContinuationSyntax);
                case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax:
                    return this.CompareOmittedArraySizeExpressionSyntax(
                        omittedArraySizeExpressionSyntax,
                        formattedNode as OmittedArraySizeExpressionSyntax);
                case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax:
                    return this.CompareInterpolatedStringExpressionSyntax(
                        interpolatedStringExpressionSyntax,
                        formattedNode as InterpolatedStringExpressionSyntax);
                case IsPatternExpressionSyntax isPatternExpressionSyntax:
                    return this.CompareIsPatternExpressionSyntax(
                        isPatternExpressionSyntax,
                        formattedNode as IsPatternExpressionSyntax);
                case ThrowExpressionSyntax throwExpressionSyntax:
                    return this.CompareThrowExpressionSyntax(
                        throwExpressionSyntax,
                        formattedNode as ThrowExpressionSyntax);
                case WhenClauseSyntax whenClauseSyntax:
                    return this.CompareWhenClauseSyntax(
                        whenClauseSyntax,
                        formattedNode as WhenClauseSyntax);
                case DiscardPatternSyntax discardPatternSyntax:
                    return this.CompareDiscardPatternSyntax(
                        discardPatternSyntax,
                        formattedNode as DiscardPatternSyntax);
                case DeclarationPatternSyntax declarationPatternSyntax:
                    return this.CompareDeclarationPatternSyntax(
                        declarationPatternSyntax,
                        formattedNode as DeclarationPatternSyntax);
                case VarPatternSyntax varPatternSyntax:
                    return this.CompareVarPatternSyntax(
                        varPatternSyntax,
                        formattedNode as VarPatternSyntax);
                case RecursivePatternSyntax recursivePatternSyntax:
                    return this.CompareRecursivePatternSyntax(
                        recursivePatternSyntax,
                        formattedNode as RecursivePatternSyntax);
                case PositionalPatternClauseSyntax positionalPatternClauseSyntax:
                    return this.ComparePositionalPatternClauseSyntax(
                        positionalPatternClauseSyntax,
                        formattedNode as PositionalPatternClauseSyntax);
                case PropertyPatternClauseSyntax propertyPatternClauseSyntax:
                    return this.ComparePropertyPatternClauseSyntax(
                        propertyPatternClauseSyntax,
                        formattedNode as PropertyPatternClauseSyntax);
                case SubpatternSyntax subpatternSyntax:
                    return this.CompareSubpatternSyntax(
                        subpatternSyntax,
                        formattedNode as SubpatternSyntax);
                case ConstantPatternSyntax constantPatternSyntax:
                    return this.CompareConstantPatternSyntax(
                        constantPatternSyntax,
                        formattedNode as ConstantPatternSyntax);
                case ParenthesizedPatternSyntax parenthesizedPatternSyntax:
                    return this.CompareParenthesizedPatternSyntax(
                        parenthesizedPatternSyntax,
                        formattedNode as ParenthesizedPatternSyntax);
                case RelationalPatternSyntax relationalPatternSyntax:
                    return this.CompareRelationalPatternSyntax(
                        relationalPatternSyntax,
                        formattedNode as RelationalPatternSyntax);
                case TypePatternSyntax typePatternSyntax:
                    return this.CompareTypePatternSyntax(
                        typePatternSyntax,
                        formattedNode as TypePatternSyntax);
                case BinaryPatternSyntax binaryPatternSyntax:
                    return this.CompareBinaryPatternSyntax(
                        binaryPatternSyntax,
                        formattedNode as BinaryPatternSyntax);
                case UnaryPatternSyntax unaryPatternSyntax:
                    return this.CompareUnaryPatternSyntax(
                        unaryPatternSyntax,
                        formattedNode as UnaryPatternSyntax);
                case InterpolatedStringTextSyntax interpolatedStringTextSyntax:
                    return this.CompareInterpolatedStringTextSyntax(
                        interpolatedStringTextSyntax,
                        formattedNode as InterpolatedStringTextSyntax);
                case InterpolationSyntax interpolationSyntax:
                    return this.CompareInterpolationSyntax(
                        interpolationSyntax,
                        formattedNode as InterpolationSyntax);
                case InterpolationAlignmentClauseSyntax interpolationAlignmentClauseSyntax:
                    return this.CompareInterpolationAlignmentClauseSyntax(
                        interpolationAlignmentClauseSyntax,
                        formattedNode as InterpolationAlignmentClauseSyntax);
                case InterpolationFormatClauseSyntax interpolationFormatClauseSyntax:
                    return this.CompareInterpolationFormatClauseSyntax(
                        interpolationFormatClauseSyntax,
                        formattedNode as InterpolationFormatClauseSyntax);
                case GlobalStatementSyntax globalStatementSyntax:
                    return this.CompareGlobalStatementSyntax(
                        globalStatementSyntax,
                        formattedNode as GlobalStatementSyntax);
                case BlockSyntax blockSyntax:
                    return this.CompareBlockSyntax(
                        blockSyntax,
                        formattedNode as BlockSyntax);
                case LocalFunctionStatementSyntax localFunctionStatementSyntax:
                    return this.CompareLocalFunctionStatementSyntax(
                        localFunctionStatementSyntax,
                        formattedNode as LocalFunctionStatementSyntax);
                case LocalDeclarationStatementSyntax localDeclarationStatementSyntax:
                    return this.CompareLocalDeclarationStatementSyntax(
                        localDeclarationStatementSyntax,
                        formattedNode as LocalDeclarationStatementSyntax);
                case VariableDeclarationSyntax variableDeclarationSyntax:
                    return this.CompareVariableDeclarationSyntax(
                        variableDeclarationSyntax,
                        formattedNode as VariableDeclarationSyntax);
                case VariableDeclaratorSyntax variableDeclaratorSyntax:
                    return this.CompareVariableDeclaratorSyntax(
                        variableDeclaratorSyntax,
                        formattedNode as VariableDeclaratorSyntax);
                case EqualsValueClauseSyntax equalsValueClauseSyntax:
                    return this.CompareEqualsValueClauseSyntax(
                        equalsValueClauseSyntax,
                        formattedNode as EqualsValueClauseSyntax);
                case SingleVariableDesignationSyntax singleVariableDesignationSyntax:
                    return this.CompareSingleVariableDesignationSyntax(
                        singleVariableDesignationSyntax,
                        formattedNode as SingleVariableDesignationSyntax);
                case DiscardDesignationSyntax discardDesignationSyntax:
                    return this.CompareDiscardDesignationSyntax(
                        discardDesignationSyntax,
                        formattedNode as DiscardDesignationSyntax);
                case ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax:
                    return this.CompareParenthesizedVariableDesignationSyntax(
                        parenthesizedVariableDesignationSyntax,
                        formattedNode as ParenthesizedVariableDesignationSyntax);
                case ExpressionStatementSyntax expressionStatementSyntax:
                    return this.CompareExpressionStatementSyntax(
                        expressionStatementSyntax,
                        formattedNode as ExpressionStatementSyntax);
                case EmptyStatementSyntax emptyStatementSyntax:
                    return this.CompareEmptyStatementSyntax(
                        emptyStatementSyntax,
                        formattedNode as EmptyStatementSyntax);
                case LabeledStatementSyntax labeledStatementSyntax:
                    return this.CompareLabeledStatementSyntax(
                        labeledStatementSyntax,
                        formattedNode as LabeledStatementSyntax);
                case GotoStatementSyntax gotoStatementSyntax:
                    return this.CompareGotoStatementSyntax(
                        gotoStatementSyntax,
                        formattedNode as GotoStatementSyntax);
                case BreakStatementSyntax breakStatementSyntax:
                    return this.CompareBreakStatementSyntax(
                        breakStatementSyntax,
                        formattedNode as BreakStatementSyntax);
                case ContinueStatementSyntax continueStatementSyntax:
                    return this.CompareContinueStatementSyntax(
                        continueStatementSyntax,
                        formattedNode as ContinueStatementSyntax);
                case ReturnStatementSyntax returnStatementSyntax:
                    return this.CompareReturnStatementSyntax(
                        returnStatementSyntax,
                        formattedNode as ReturnStatementSyntax);
                case ThrowStatementSyntax throwStatementSyntax:
                    return this.CompareThrowStatementSyntax(
                        throwStatementSyntax,
                        formattedNode as ThrowStatementSyntax);
                case YieldStatementSyntax yieldStatementSyntax:
                    return this.CompareYieldStatementSyntax(
                        yieldStatementSyntax,
                        formattedNode as YieldStatementSyntax);
                case WhileStatementSyntax whileStatementSyntax:
                    return this.CompareWhileStatementSyntax(
                        whileStatementSyntax,
                        formattedNode as WhileStatementSyntax);
                case DoStatementSyntax doStatementSyntax:
                    return this.CompareDoStatementSyntax(
                        doStatementSyntax,
                        formattedNode as DoStatementSyntax);
                case ForStatementSyntax forStatementSyntax:
                    return this.CompareForStatementSyntax(
                        forStatementSyntax,
                        formattedNode as ForStatementSyntax);
                case ForEachStatementSyntax forEachStatementSyntax:
                    return this.CompareForEachStatementSyntax(
                        forEachStatementSyntax,
                        formattedNode as ForEachStatementSyntax);
                case ForEachVariableStatementSyntax forEachVariableStatementSyntax:
                    return this.CompareForEachVariableStatementSyntax(
                        forEachVariableStatementSyntax,
                        formattedNode as ForEachVariableStatementSyntax);
                case UsingStatementSyntax usingStatementSyntax:
                    return this.CompareUsingStatementSyntax(
                        usingStatementSyntax,
                        formattedNode as UsingStatementSyntax);
                case FixedStatementSyntax fixedStatementSyntax:
                    return this.CompareFixedStatementSyntax(
                        fixedStatementSyntax,
                        formattedNode as FixedStatementSyntax);
                case CheckedStatementSyntax checkedStatementSyntax:
                    return this.CompareCheckedStatementSyntax(
                        checkedStatementSyntax,
                        formattedNode as CheckedStatementSyntax);
                case UnsafeStatementSyntax unsafeStatementSyntax:
                    return this.CompareUnsafeStatementSyntax(
                        unsafeStatementSyntax,
                        formattedNode as UnsafeStatementSyntax);
                case LockStatementSyntax lockStatementSyntax:
                    return this.CompareLockStatementSyntax(
                        lockStatementSyntax,
                        formattedNode as LockStatementSyntax);
                case IfStatementSyntax ifStatementSyntax:
                    return this.CompareIfStatementSyntax(
                        ifStatementSyntax,
                        formattedNode as IfStatementSyntax);
                case ElseClauseSyntax elseClauseSyntax:
                    return this.CompareElseClauseSyntax(
                        elseClauseSyntax,
                        formattedNode as ElseClauseSyntax);
                case SwitchStatementSyntax switchStatementSyntax:
                    return this.CompareSwitchStatementSyntax(
                        switchStatementSyntax,
                        formattedNode as SwitchStatementSyntax);
                case SwitchSectionSyntax switchSectionSyntax:
                    return this.CompareSwitchSectionSyntax(
                        switchSectionSyntax,
                        formattedNode as SwitchSectionSyntax);
                case CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax:
                    return this.CompareCasePatternSwitchLabelSyntax(
                        casePatternSwitchLabelSyntax,
                        formattedNode as CasePatternSwitchLabelSyntax);
                case CaseSwitchLabelSyntax caseSwitchLabelSyntax:
                    return this.CompareCaseSwitchLabelSyntax(
                        caseSwitchLabelSyntax,
                        formattedNode as CaseSwitchLabelSyntax);
                case DefaultSwitchLabelSyntax defaultSwitchLabelSyntax:
                    return this.CompareDefaultSwitchLabelSyntax(
                        defaultSwitchLabelSyntax,
                        formattedNode as DefaultSwitchLabelSyntax);
                case SwitchExpressionSyntax switchExpressionSyntax:
                    return this.CompareSwitchExpressionSyntax(
                        switchExpressionSyntax,
                        formattedNode as SwitchExpressionSyntax);
                case SwitchExpressionArmSyntax switchExpressionArmSyntax:
                    return this.CompareSwitchExpressionArmSyntax(
                        switchExpressionArmSyntax,
                        formattedNode as SwitchExpressionArmSyntax);
                case TryStatementSyntax tryStatementSyntax:
                    return this.CompareTryStatementSyntax(
                        tryStatementSyntax,
                        formattedNode as TryStatementSyntax);
                case CatchClauseSyntax catchClauseSyntax:
                    return this.CompareCatchClauseSyntax(
                        catchClauseSyntax,
                        formattedNode as CatchClauseSyntax);
                case CatchDeclarationSyntax catchDeclarationSyntax:
                    return this.CompareCatchDeclarationSyntax(
                        catchDeclarationSyntax,
                        formattedNode as CatchDeclarationSyntax);
                case CatchFilterClauseSyntax catchFilterClauseSyntax:
                    return this.CompareCatchFilterClauseSyntax(
                        catchFilterClauseSyntax,
                        formattedNode as CatchFilterClauseSyntax);
                case FinallyClauseSyntax finallyClauseSyntax:
                    return this.CompareFinallyClauseSyntax(
                        finallyClauseSyntax,
                        formattedNode as FinallyClauseSyntax);
                case CompilationUnitSyntax compilationUnitSyntax:
                    return this.CompareCompilationUnitSyntax(
                        compilationUnitSyntax,
                        formattedNode as CompilationUnitSyntax);
                case ExternAliasDirectiveSyntax externAliasDirectiveSyntax:
                    return this.CompareExternAliasDirectiveSyntax(
                        externAliasDirectiveSyntax,
                        formattedNode as ExternAliasDirectiveSyntax);
                case UsingDirectiveSyntax usingDirectiveSyntax:
                    return this.CompareUsingDirectiveSyntax(
                        usingDirectiveSyntax,
                        formattedNode as UsingDirectiveSyntax);
                case NamespaceDeclarationSyntax namespaceDeclarationSyntax:
                    return this.CompareNamespaceDeclarationSyntax(
                        namespaceDeclarationSyntax,
                        formattedNode as NamespaceDeclarationSyntax);
                case AttributeListSyntax attributeListSyntax:
                    return this.CompareAttributeListSyntax(
                        attributeListSyntax,
                        formattedNode as AttributeListSyntax);
                case AttributeTargetSpecifierSyntax attributeTargetSpecifierSyntax:
                    return this.CompareAttributeTargetSpecifierSyntax(
                        attributeTargetSpecifierSyntax,
                        formattedNode as AttributeTargetSpecifierSyntax);
                case AttributeSyntax attributeSyntax:
                    return this.CompareAttributeSyntax(
                        attributeSyntax,
                        formattedNode as AttributeSyntax);
                case AttributeArgumentListSyntax attributeArgumentListSyntax:
                    return this.CompareAttributeArgumentListSyntax(
                        attributeArgumentListSyntax,
                        formattedNode as AttributeArgumentListSyntax);
                case AttributeArgumentSyntax attributeArgumentSyntax:
                    return this.CompareAttributeArgumentSyntax(
                        attributeArgumentSyntax,
                        formattedNode as AttributeArgumentSyntax);
                case NameEqualsSyntax nameEqualsSyntax:
                    return this.CompareNameEqualsSyntax(
                        nameEqualsSyntax,
                        formattedNode as NameEqualsSyntax);
                case TypeParameterListSyntax typeParameterListSyntax:
                    return this.CompareTypeParameterListSyntax(
                        typeParameterListSyntax,
                        formattedNode as TypeParameterListSyntax);
                case TypeParameterSyntax typeParameterSyntax:
                    return this.CompareTypeParameterSyntax(
                        typeParameterSyntax,
                        formattedNode as TypeParameterSyntax);
                case ClassDeclarationSyntax classDeclarationSyntax:
                    return this.CompareClassDeclarationSyntax(
                        classDeclarationSyntax,
                        formattedNode as ClassDeclarationSyntax);
                case StructDeclarationSyntax structDeclarationSyntax:
                    return this.CompareStructDeclarationSyntax(
                        structDeclarationSyntax,
                        formattedNode as StructDeclarationSyntax);
                case InterfaceDeclarationSyntax interfaceDeclarationSyntax:
                    return this.CompareInterfaceDeclarationSyntax(
                        interfaceDeclarationSyntax,
                        formattedNode as InterfaceDeclarationSyntax);
                case RecordDeclarationSyntax recordDeclarationSyntax:
                    return this.CompareRecordDeclarationSyntax(
                        recordDeclarationSyntax,
                        formattedNode as RecordDeclarationSyntax);
                case EnumDeclarationSyntax enumDeclarationSyntax:
                    return this.CompareEnumDeclarationSyntax(
                        enumDeclarationSyntax,
                        formattedNode as EnumDeclarationSyntax);
                case DelegateDeclarationSyntax delegateDeclarationSyntax:
                    return this.CompareDelegateDeclarationSyntax(
                        delegateDeclarationSyntax,
                        formattedNode as DelegateDeclarationSyntax);
                case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax:
                    return this.CompareEnumMemberDeclarationSyntax(
                        enumMemberDeclarationSyntax,
                        formattedNode as EnumMemberDeclarationSyntax);
                case BaseListSyntax baseListSyntax:
                    return this.CompareBaseListSyntax(
                        baseListSyntax,
                        formattedNode as BaseListSyntax);
                case SimpleBaseTypeSyntax simpleBaseTypeSyntax:
                    return this.CompareSimpleBaseTypeSyntax(
                        simpleBaseTypeSyntax,
                        formattedNode as SimpleBaseTypeSyntax);
                case PrimaryConstructorBaseTypeSyntax primaryConstructorBaseTypeSyntax:
                    return this.ComparePrimaryConstructorBaseTypeSyntax(
                        primaryConstructorBaseTypeSyntax,
                        formattedNode as PrimaryConstructorBaseTypeSyntax);
                case TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax:
                    return this.CompareTypeParameterConstraintClauseSyntax(
                        typeParameterConstraintClauseSyntax,
                        formattedNode as TypeParameterConstraintClauseSyntax);
                case ConstructorConstraintSyntax constructorConstraintSyntax:
                    return this.CompareConstructorConstraintSyntax(
                        constructorConstraintSyntax,
                        formattedNode as ConstructorConstraintSyntax);
                case ClassOrStructConstraintSyntax classOrStructConstraintSyntax:
                    return this.CompareClassOrStructConstraintSyntax(
                        classOrStructConstraintSyntax,
                        formattedNode as ClassOrStructConstraintSyntax);
                case TypeConstraintSyntax typeConstraintSyntax:
                    return this.CompareTypeConstraintSyntax(
                        typeConstraintSyntax,
                        formattedNode as TypeConstraintSyntax);
                case DefaultConstraintSyntax defaultConstraintSyntax:
                    return this.CompareDefaultConstraintSyntax(
                        defaultConstraintSyntax,
                        formattedNode as DefaultConstraintSyntax);
                case FieldDeclarationSyntax fieldDeclarationSyntax:
                    return this.CompareFieldDeclarationSyntax(
                        fieldDeclarationSyntax,
                        formattedNode as FieldDeclarationSyntax);
                case EventFieldDeclarationSyntax eventFieldDeclarationSyntax:
                    return this.CompareEventFieldDeclarationSyntax(
                        eventFieldDeclarationSyntax,
                        formattedNode as EventFieldDeclarationSyntax);
                case ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax:
                    return this.CompareExplicitInterfaceSpecifierSyntax(
                        explicitInterfaceSpecifierSyntax,
                        formattedNode as ExplicitInterfaceSpecifierSyntax);
                case MethodDeclarationSyntax methodDeclarationSyntax:
                    return this.CompareMethodDeclarationSyntax(
                        methodDeclarationSyntax,
                        formattedNode as MethodDeclarationSyntax);
                case OperatorDeclarationSyntax operatorDeclarationSyntax:
                    return this.CompareOperatorDeclarationSyntax(
                        operatorDeclarationSyntax,
                        formattedNode as OperatorDeclarationSyntax);
                case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax:
                    return this.CompareConversionOperatorDeclarationSyntax(
                        conversionOperatorDeclarationSyntax,
                        formattedNode as ConversionOperatorDeclarationSyntax);
                case ConstructorDeclarationSyntax constructorDeclarationSyntax:
                    return this.CompareConstructorDeclarationSyntax(
                        constructorDeclarationSyntax,
                        formattedNode as ConstructorDeclarationSyntax);
                case ConstructorInitializerSyntax constructorInitializerSyntax:
                    return this.CompareConstructorInitializerSyntax(
                        constructorInitializerSyntax,
                        formattedNode as ConstructorInitializerSyntax);
                case DestructorDeclarationSyntax destructorDeclarationSyntax:
                    return this.CompareDestructorDeclarationSyntax(
                        destructorDeclarationSyntax,
                        formattedNode as DestructorDeclarationSyntax);
                case PropertyDeclarationSyntax propertyDeclarationSyntax:
                    return this.ComparePropertyDeclarationSyntax(
                        propertyDeclarationSyntax,
                        formattedNode as PropertyDeclarationSyntax);
                case ArrowExpressionClauseSyntax arrowExpressionClauseSyntax:
                    return this.CompareArrowExpressionClauseSyntax(
                        arrowExpressionClauseSyntax,
                        formattedNode as ArrowExpressionClauseSyntax);
                case EventDeclarationSyntax eventDeclarationSyntax:
                    return this.CompareEventDeclarationSyntax(
                        eventDeclarationSyntax,
                        formattedNode as EventDeclarationSyntax);
                case IndexerDeclarationSyntax indexerDeclarationSyntax:
                    return this.CompareIndexerDeclarationSyntax(
                        indexerDeclarationSyntax,
                        formattedNode as IndexerDeclarationSyntax);
                case AccessorListSyntax accessorListSyntax:
                    return this.CompareAccessorListSyntax(
                        accessorListSyntax,
                        formattedNode as AccessorListSyntax);
                case AccessorDeclarationSyntax accessorDeclarationSyntax:
                    return this.CompareAccessorDeclarationSyntax(
                        accessorDeclarationSyntax,
                        formattedNode as AccessorDeclarationSyntax);
                case ParameterListSyntax parameterListSyntax:
                    return this.CompareParameterListSyntax(
                        parameterListSyntax,
                        formattedNode as ParameterListSyntax);
                case BracketedParameterListSyntax bracketedParameterListSyntax:
                    return this.CompareBracketedParameterListSyntax(
                        bracketedParameterListSyntax,
                        formattedNode as BracketedParameterListSyntax);
                case ParameterSyntax parameterSyntax:
                    return this.CompareParameterSyntax(
                        parameterSyntax,
                        formattedNode as ParameterSyntax);
                case FunctionPointerParameterSyntax functionPointerParameterSyntax:
                    return this.CompareFunctionPointerParameterSyntax(
                        functionPointerParameterSyntax,
                        formattedNode as FunctionPointerParameterSyntax);
                case IncompleteMemberSyntax incompleteMemberSyntax:
                    return this.CompareIncompleteMemberSyntax(
                        incompleteMemberSyntax,
                        formattedNode as IncompleteMemberSyntax);
                case SkippedTokensTriviaSyntax skippedTokensTriviaSyntax:
                    return this.CompareSkippedTokensTriviaSyntax(
                        skippedTokensTriviaSyntax,
                        formattedNode as SkippedTokensTriviaSyntax);
                case DocumentationCommentTriviaSyntax documentationCommentTriviaSyntax:
                    return this.CompareDocumentationCommentTriviaSyntax(
                        documentationCommentTriviaSyntax,
                        formattedNode as DocumentationCommentTriviaSyntax);
                case TypeCrefSyntax typeCrefSyntax:
                    return this.CompareTypeCrefSyntax(
                        typeCrefSyntax,
                        formattedNode as TypeCrefSyntax);
                case QualifiedCrefSyntax qualifiedCrefSyntax:
                    return this.CompareQualifiedCrefSyntax(
                        qualifiedCrefSyntax,
                        formattedNode as QualifiedCrefSyntax);
                case NameMemberCrefSyntax nameMemberCrefSyntax:
                    return this.CompareNameMemberCrefSyntax(
                        nameMemberCrefSyntax,
                        formattedNode as NameMemberCrefSyntax);
                case IndexerMemberCrefSyntax indexerMemberCrefSyntax:
                    return this.CompareIndexerMemberCrefSyntax(
                        indexerMemberCrefSyntax,
                        formattedNode as IndexerMemberCrefSyntax);
                case OperatorMemberCrefSyntax operatorMemberCrefSyntax:
                    return this.CompareOperatorMemberCrefSyntax(
                        operatorMemberCrefSyntax,
                        formattedNode as OperatorMemberCrefSyntax);
                case ConversionOperatorMemberCrefSyntax conversionOperatorMemberCrefSyntax:
                    return this.CompareConversionOperatorMemberCrefSyntax(
                        conversionOperatorMemberCrefSyntax,
                        formattedNode as ConversionOperatorMemberCrefSyntax);
                case CrefParameterListSyntax crefParameterListSyntax:
                    return this.CompareCrefParameterListSyntax(
                        crefParameterListSyntax,
                        formattedNode as CrefParameterListSyntax);
                case CrefBracketedParameterListSyntax crefBracketedParameterListSyntax:
                    return this.CompareCrefBracketedParameterListSyntax(
                        crefBracketedParameterListSyntax,
                        formattedNode as CrefBracketedParameterListSyntax);
                case CrefParameterSyntax crefParameterSyntax:
                    return this.CompareCrefParameterSyntax(
                        crefParameterSyntax,
                        formattedNode as CrefParameterSyntax);
                case XmlElementSyntax xmlElementSyntax:
                    return this.CompareXmlElementSyntax(
                        xmlElementSyntax,
                        formattedNode as XmlElementSyntax);
                case XmlElementStartTagSyntax xmlElementStartTagSyntax:
                    return this.CompareXmlElementStartTagSyntax(
                        xmlElementStartTagSyntax,
                        formattedNode as XmlElementStartTagSyntax);
                case XmlElementEndTagSyntax xmlElementEndTagSyntax:
                    return this.CompareXmlElementEndTagSyntax(
                        xmlElementEndTagSyntax,
                        formattedNode as XmlElementEndTagSyntax);
                case XmlEmptyElementSyntax xmlEmptyElementSyntax:
                    return this.CompareXmlEmptyElementSyntax(
                        xmlEmptyElementSyntax,
                        formattedNode as XmlEmptyElementSyntax);
                case XmlNameSyntax xmlNameSyntax:
                    return this.CompareXmlNameSyntax(
                        xmlNameSyntax,
                        formattedNode as XmlNameSyntax);
                case XmlPrefixSyntax xmlPrefixSyntax:
                    return this.CompareXmlPrefixSyntax(
                        xmlPrefixSyntax,
                        formattedNode as XmlPrefixSyntax);
                case XmlTextAttributeSyntax xmlTextAttributeSyntax:
                    return this.CompareXmlTextAttributeSyntax(
                        xmlTextAttributeSyntax,
                        formattedNode as XmlTextAttributeSyntax);
                case XmlCrefAttributeSyntax xmlCrefAttributeSyntax:
                    return this.CompareXmlCrefAttributeSyntax(
                        xmlCrefAttributeSyntax,
                        formattedNode as XmlCrefAttributeSyntax);
                case XmlNameAttributeSyntax xmlNameAttributeSyntax:
                    return this.CompareXmlNameAttributeSyntax(
                        xmlNameAttributeSyntax,
                        formattedNode as XmlNameAttributeSyntax);
                case XmlTextSyntax xmlTextSyntax:
                    return this.CompareXmlTextSyntax(
                        xmlTextSyntax,
                        formattedNode as XmlTextSyntax);
                case XmlCDataSectionSyntax xmlCDataSectionSyntax:
                    return this.CompareXmlCDataSectionSyntax(
                        xmlCDataSectionSyntax,
                        formattedNode as XmlCDataSectionSyntax);
                case XmlProcessingInstructionSyntax xmlProcessingInstructionSyntax:
                    return this.CompareXmlProcessingInstructionSyntax(
                        xmlProcessingInstructionSyntax,
                        formattedNode as XmlProcessingInstructionSyntax);
                case XmlCommentSyntax xmlCommentSyntax:
                    return this.CompareXmlCommentSyntax(
                        xmlCommentSyntax,
                        formattedNode as XmlCommentSyntax);
                case IfDirectiveTriviaSyntax ifDirectiveTriviaSyntax:
                    return this.CompareIfDirectiveTriviaSyntax(
                        ifDirectiveTriviaSyntax,
                        formattedNode as IfDirectiveTriviaSyntax);
                case ElifDirectiveTriviaSyntax elifDirectiveTriviaSyntax:
                    return this.CompareElifDirectiveTriviaSyntax(
                        elifDirectiveTriviaSyntax,
                        formattedNode as ElifDirectiveTriviaSyntax);
                case ElseDirectiveTriviaSyntax elseDirectiveTriviaSyntax:
                    return this.CompareElseDirectiveTriviaSyntax(
                        elseDirectiveTriviaSyntax,
                        formattedNode as ElseDirectiveTriviaSyntax);
                case EndIfDirectiveTriviaSyntax endIfDirectiveTriviaSyntax:
                    return this.CompareEndIfDirectiveTriviaSyntax(
                        endIfDirectiveTriviaSyntax,
                        formattedNode as EndIfDirectiveTriviaSyntax);
                case RegionDirectiveTriviaSyntax regionDirectiveTriviaSyntax:
                    return this.CompareRegionDirectiveTriviaSyntax(
                        regionDirectiveTriviaSyntax,
                        formattedNode as RegionDirectiveTriviaSyntax);
                case EndRegionDirectiveTriviaSyntax endRegionDirectiveTriviaSyntax:
                    return this.CompareEndRegionDirectiveTriviaSyntax(
                        endRegionDirectiveTriviaSyntax,
                        formattedNode as EndRegionDirectiveTriviaSyntax);
                case ErrorDirectiveTriviaSyntax errorDirectiveTriviaSyntax:
                    return this.CompareErrorDirectiveTriviaSyntax(
                        errorDirectiveTriviaSyntax,
                        formattedNode as ErrorDirectiveTriviaSyntax);
                case WarningDirectiveTriviaSyntax warningDirectiveTriviaSyntax:
                    return this.CompareWarningDirectiveTriviaSyntax(
                        warningDirectiveTriviaSyntax,
                        formattedNode as WarningDirectiveTriviaSyntax);
                case BadDirectiveTriviaSyntax badDirectiveTriviaSyntax:
                    return this.CompareBadDirectiveTriviaSyntax(
                        badDirectiveTriviaSyntax,
                        formattedNode as BadDirectiveTriviaSyntax);
                case DefineDirectiveTriviaSyntax defineDirectiveTriviaSyntax:
                    return this.CompareDefineDirectiveTriviaSyntax(
                        defineDirectiveTriviaSyntax,
                        formattedNode as DefineDirectiveTriviaSyntax);
                case UndefDirectiveTriviaSyntax undefDirectiveTriviaSyntax:
                    return this.CompareUndefDirectiveTriviaSyntax(
                        undefDirectiveTriviaSyntax,
                        formattedNode as UndefDirectiveTriviaSyntax);
                case LineDirectiveTriviaSyntax lineDirectiveTriviaSyntax:
                    return this.CompareLineDirectiveTriviaSyntax(
                        lineDirectiveTriviaSyntax,
                        formattedNode as LineDirectiveTriviaSyntax);
                case PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTriviaSyntax:
                    return this.ComparePragmaWarningDirectiveTriviaSyntax(
                        pragmaWarningDirectiveTriviaSyntax,
                        formattedNode as PragmaWarningDirectiveTriviaSyntax);
                case PragmaChecksumDirectiveTriviaSyntax pragmaChecksumDirectiveTriviaSyntax:
                    return this.ComparePragmaChecksumDirectiveTriviaSyntax(
                        pragmaChecksumDirectiveTriviaSyntax,
                        formattedNode as PragmaChecksumDirectiveTriviaSyntax);
                case ReferenceDirectiveTriviaSyntax referenceDirectiveTriviaSyntax:
                    return this.CompareReferenceDirectiveTriviaSyntax(
                        referenceDirectiveTriviaSyntax,
                        formattedNode as ReferenceDirectiveTriviaSyntax);
                case LoadDirectiveTriviaSyntax loadDirectiveTriviaSyntax:
                    return this.CompareLoadDirectiveTriviaSyntax(
                        loadDirectiveTriviaSyntax,
                        formattedNode as LoadDirectiveTriviaSyntax);
                case ShebangDirectiveTriviaSyntax shebangDirectiveTriviaSyntax:
                    return this.CompareShebangDirectiveTriviaSyntax(
                        shebangDirectiveTriviaSyntax,
                        formattedNode as ShebangDirectiveTriviaSyntax);
                case NullableDirectiveTriviaSyntax nullableDirectiveTriviaSyntax:
                    return this.CompareNullableDirectiveTriviaSyntax(
                        nullableDirectiveTriviaSyntax,
                        formattedNode as NullableDirectiveTriviaSyntax);
                default:
                    throw new Exception(
                        "Can't handle " + originalNode.GetType().Name);
            }
        }

        private CompareResult CompareIdentifierNameSyntax(
            IdentifierNameSyntax originalNode,
            IdentifierNameSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareQualifiedNameSyntax(
            QualifiedNameSyntax originalNode,
            QualifiedNameSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Left);
            formattedStack.Push(formattedNode.Left);
            result = this.Compare(
                originalNode.DotToken,
                formattedNode.DotToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Right);
            formattedStack.Push(formattedNode.Right);
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareGenericNameSyntax(
            GenericNameSyntax originalNode,
            GenericNameSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.TypeArgumentList);
            formattedStack.Push(formattedNode.TypeArgumentList);
            if (
                originalNode.IsUnboundGenericName != formattedNode.IsUnboundGenericName
            )
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTypeArgumentListSyntax(
            TypeArgumentListSyntax originalNode,
            TypeArgumentListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.LessThanToken,
                formattedNode.LessThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments,
                formattedNode.Arguments,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments.GetSeparators().ToList(),
                formattedNode.Arguments.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.GreaterThanToken,
                formattedNode.GreaterThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAliasQualifiedNameSyntax(
            AliasQualifiedNameSyntax originalNode,
            AliasQualifiedNameSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Alias);
            formattedStack.Push(formattedNode.Alias);
            result = this.Compare(
                originalNode.ColonColonToken,
                formattedNode.ColonColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePredefinedTypeSyntax(
            PredefinedTypeSyntax originalNode,
            PredefinedTypeSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareArrayTypeSyntax(
            ArrayTypeSyntax originalNode,
            ArrayTypeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.ElementType);
            formattedStack.Push(formattedNode.ElementType);
            result = this.CompareLists(
                originalNode.RankSpecifiers,
                formattedNode.RankSpecifiers,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareArrayRankSpecifierSyntax(
            ArrayRankSpecifierSyntax originalNode,
            ArrayRankSpecifierSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBracketToken,
                formattedNode.OpenBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Sizes,
                formattedNode.Sizes,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Sizes.GetSeparators().ToList(),
                formattedNode.Sizes.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBracketToken,
                formattedNode.CloseBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.Rank != formattedNode.Rank)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePointerTypeSyntax(
            PointerTypeSyntax originalNode,
            PointerTypeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.ElementType);
            formattedStack.Push(formattedNode.ElementType);
            result = this.Compare(
                originalNode.AsteriskToken,
                formattedNode.AsteriskToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFunctionPointerTypeSyntax(
            FunctionPointerTypeSyntax originalNode,
            FunctionPointerTypeSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.DelegateKeyword,
                formattedNode.DelegateKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.AsteriskToken,
                formattedNode.AsteriskToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.CallingConvention);
            formattedStack.Push(formattedNode.CallingConvention);
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFunctionPointerParameterListSyntax(
            FunctionPointerParameterListSyntax originalNode,
            FunctionPointerParameterListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.LessThanToken,
                formattedNode.LessThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters,
                formattedNode.Parameters,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters.GetSeparators().ToList(),
                formattedNode.Parameters.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.GreaterThanToken,
                formattedNode.GreaterThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFunctionPointerCallingConventionSyntax(
            FunctionPointerCallingConventionSyntax originalNode,
            FunctionPointerCallingConventionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ManagedOrUnmanagedKeyword,
                formattedNode.ManagedOrUnmanagedKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.UnmanagedCallingConventionList);
            formattedStack.Push(formattedNode.UnmanagedCallingConventionList);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFunctionPointerUnmanagedCallingConventionListSyntax(
            FunctionPointerUnmanagedCallingConventionListSyntax originalNode,
            FunctionPointerUnmanagedCallingConventionListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBracketToken,
                formattedNode.OpenBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.CallingConventions,
                formattedNode.CallingConventions,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.CallingConventions.GetSeparators().ToList(),
                formattedNode.CallingConventions.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBracketToken,
                formattedNode.CloseBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFunctionPointerUnmanagedCallingConventionSyntax(
            FunctionPointerUnmanagedCallingConventionSyntax originalNode,
            FunctionPointerUnmanagedCallingConventionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Name,
                formattedNode.Name,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareNullableTypeSyntax(
            NullableTypeSyntax originalNode,
            NullableTypeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.ElementType);
            formattedStack.Push(formattedNode.ElementType);
            result = this.Compare(
                originalNode.QuestionToken,
                formattedNode.QuestionToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTupleTypeSyntax(
            TupleTypeSyntax originalNode,
            TupleTypeSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Elements,
                formattedNode.Elements,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Elements.GetSeparators().ToList(),
                formattedNode.Elements.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTupleElementSyntax(
            TupleElementSyntax originalNode,
            TupleElementSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareOmittedTypeArgumentSyntax(
            OmittedTypeArgumentSyntax originalNode,
            OmittedTypeArgumentSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OmittedTypeArgumentToken,
                formattedNode.OmittedTypeArgumentToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRefTypeSyntax(
            RefTypeSyntax originalNode,
            RefTypeSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.RefKeyword,
                formattedNode.RefKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ReadOnlyKeyword,
                formattedNode.ReadOnlyKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            if (originalNode.IsVar != formattedNode.IsVar)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsUnmanaged != formattedNode.IsUnmanaged)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNotNull != formattedNode.IsNotNull)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNint != formattedNode.IsNint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsNuint != formattedNode.IsNuint)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareParenthesizedExpressionSyntax(
            ParenthesizedExpressionSyntax originalNode,
            ParenthesizedExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTupleExpressionSyntax(
            TupleExpressionSyntax originalNode,
            TupleExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments,
                formattedNode.Arguments,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments.GetSeparators().ToList(),
                formattedNode.Arguments.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePrefixUnaryExpressionSyntax(
            PrefixUnaryExpressionSyntax originalNode,
            PrefixUnaryExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Operand);
            formattedStack.Push(formattedNode.Operand);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAwaitExpressionSyntax(
            AwaitExpressionSyntax originalNode,
            AwaitExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.AwaitKeyword,
                formattedNode.AwaitKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePostfixUnaryExpressionSyntax(
            PostfixUnaryExpressionSyntax originalNode,
            PostfixUnaryExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Operand);
            formattedStack.Push(formattedNode.Operand);
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareMemberAccessExpressionSyntax(
            MemberAccessExpressionSyntax originalNode,
            MemberAccessExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareConditionalAccessExpressionSyntax(
            ConditionalAccessExpressionSyntax originalNode,
            ConditionalAccessExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.WhenNotNull);
            formattedStack.Push(formattedNode.WhenNotNull);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareMemberBindingExpressionSyntax(
            MemberBindingExpressionSyntax originalNode,
            MemberBindingExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareElementBindingExpressionSyntax(
            ElementBindingExpressionSyntax originalNode,
            ElementBindingExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRangeExpressionSyntax(
            RangeExpressionSyntax originalNode,
            RangeExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.LeftOperand);
            formattedStack.Push(formattedNode.LeftOperand);
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.RightOperand);
            formattedStack.Push(formattedNode.RightOperand);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareImplicitElementAccessSyntax(
            ImplicitElementAccessSyntax originalNode,
            ImplicitElementAccessSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBinaryExpressionSyntax(
            BinaryExpressionSyntax originalNode,
            BinaryExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Left);
            formattedStack.Push(formattedNode.Left);
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Right);
            formattedStack.Push(formattedNode.Right);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAssignmentExpressionSyntax(
            AssignmentExpressionSyntax originalNode,
            AssignmentExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Left);
            formattedStack.Push(formattedNode.Left);
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Right);
            formattedStack.Push(formattedNode.Right);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareConditionalExpressionSyntax(
            ConditionalExpressionSyntax originalNode,
            ConditionalExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            result = this.Compare(
                originalNode.QuestionToken,
                formattedNode.QuestionToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.WhenTrue);
            formattedStack.Push(formattedNode.WhenTrue);
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.WhenFalse);
            formattedStack.Push(formattedNode.WhenFalse);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareThisExpressionSyntax(
            ThisExpressionSyntax originalNode,
            ThisExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Token,
                formattedNode.Token,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBaseExpressionSyntax(
            BaseExpressionSyntax originalNode,
            BaseExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Token,
                formattedNode.Token,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareLiteralExpressionSyntax(
            LiteralExpressionSyntax originalNode,
            LiteralExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Token,
                formattedNode.Token,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareMakeRefExpressionSyntax(
            MakeRefExpressionSyntax originalNode,
            MakeRefExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRefTypeExpressionSyntax(
            RefTypeExpressionSyntax originalNode,
            RefTypeExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRefValueExpressionSyntax(
            RefValueExpressionSyntax originalNode,
            RefValueExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.Comma,
                formattedNode.Comma,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCheckedExpressionSyntax(
            CheckedExpressionSyntax originalNode,
            CheckedExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDefaultExpressionSyntax(
            DefaultExpressionSyntax originalNode,
            DefaultExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTypeOfExpressionSyntax(
            TypeOfExpressionSyntax originalNode,
            TypeOfExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSizeOfExpressionSyntax(
            SizeOfExpressionSyntax originalNode,
            SizeOfExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareInvocationExpressionSyntax(
            InvocationExpressionSyntax originalNode,
            InvocationExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareElementAccessExpressionSyntax(
            ElementAccessExpressionSyntax originalNode,
            ElementAccessExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareArgumentListSyntax(
            ArgumentListSyntax originalNode,
            ArgumentListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments,
                formattedNode.Arguments,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments.GetSeparators().ToList(),
                formattedNode.Arguments.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBracketedArgumentListSyntax(
            BracketedArgumentListSyntax originalNode,
            BracketedArgumentListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBracketToken,
                formattedNode.OpenBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments,
                formattedNode.Arguments,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments.GetSeparators().ToList(),
                formattedNode.Arguments.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBracketToken,
                formattedNode.CloseBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareArgumentSyntax(
            ArgumentSyntax originalNode,
            ArgumentSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.NameColon);
            formattedStack.Push(formattedNode.NameColon);
            result = this.Compare(
                originalNode.RefKindKeyword,
                formattedNode.RefKindKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.RefOrOutKeyword,
                formattedNode.RefOrOutKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareNameColonSyntax(
            NameColonSyntax originalNode,
            NameColonSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDeclarationExpressionSyntax(
            DeclarationExpressionSyntax originalNode,
            DeclarationExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.Designation);
            formattedStack.Push(formattedNode.Designation);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCastExpressionSyntax(
            CastExpressionSyntax originalNode,
            CastExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAnonymousMethodExpressionSyntax(
            AnonymousMethodExpressionSyntax originalNode,
            AnonymousMethodExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.DelegateKeyword,
                formattedNode.DelegateKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            originalStack.Push(originalNode.Block);
            formattedStack.Push(formattedNode.Block);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.AsyncKeyword,
                formattedNode.AsyncKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSimpleLambdaExpressionSyntax(
            SimpleLambdaExpressionSyntax originalNode,
            SimpleLambdaExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Parameter);
            formattedStack.Push(formattedNode.Parameter);
            result = this.Compare(
                originalNode.ArrowToken,
                formattedNode.ArrowToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Block);
            formattedStack.Push(formattedNode.Block);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.AsyncKeyword,
                formattedNode.AsyncKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRefExpressionSyntax(
            RefExpressionSyntax originalNode,
            RefExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.RefKeyword,
                formattedNode.RefKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareParenthesizedLambdaExpressionSyntax(
            ParenthesizedLambdaExpressionSyntax originalNode,
            ParenthesizedLambdaExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            result = this.Compare(
                originalNode.ArrowToken,
                formattedNode.ArrowToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Block);
            formattedStack.Push(formattedNode.Block);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.AsyncKeyword,
                formattedNode.AsyncKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareInitializerExpressionSyntax(
            InitializerExpressionSyntax originalNode,
            InitializerExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Expressions,
                formattedNode.Expressions,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Expressions.GetSeparators().ToList(),
                formattedNode.Expressions.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareImplicitObjectCreationExpressionSyntax(
            ImplicitObjectCreationExpressionSyntax originalNode,
            ImplicitObjectCreationExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.NewKeyword,
                formattedNode.NewKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareObjectCreationExpressionSyntax(
            ObjectCreationExpressionSyntax originalNode,
            ObjectCreationExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.NewKeyword,
                formattedNode.NewKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareWithExpressionSyntax(
            WithExpressionSyntax originalNode,
            WithExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.WithKeyword,
                formattedNode.WithKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAnonymousObjectMemberDeclaratorSyntax(
            AnonymousObjectMemberDeclaratorSyntax originalNode,
            AnonymousObjectMemberDeclaratorSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.NameEquals);
            formattedStack.Push(formattedNode.NameEquals);
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAnonymousObjectCreationExpressionSyntax(
            AnonymousObjectCreationExpressionSyntax originalNode,
            AnonymousObjectCreationExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.NewKeyword,
                formattedNode.NewKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Initializers,
                formattedNode.Initializers,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Initializers.GetSeparators().ToList(),
                formattedNode.Initializers.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareArrayCreationExpressionSyntax(
            ArrayCreationExpressionSyntax originalNode,
            ArrayCreationExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.NewKeyword,
                formattedNode.NewKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareImplicitArrayCreationExpressionSyntax(
            ImplicitArrayCreationExpressionSyntax originalNode,
            ImplicitArrayCreationExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.NewKeyword,
                formattedNode.NewKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBracketToken,
                formattedNode.OpenBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Commas,
                formattedNode.Commas,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBracketToken,
                formattedNode.CloseBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareStackAllocArrayCreationExpressionSyntax(
            StackAllocArrayCreationExpressionSyntax originalNode,
            StackAllocArrayCreationExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.StackAllocKeyword,
                formattedNode.StackAllocKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareImplicitStackAllocArrayCreationExpressionSyntax(
            ImplicitStackAllocArrayCreationExpressionSyntax originalNode,
            ImplicitStackAllocArrayCreationExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.StackAllocKeyword,
                formattedNode.StackAllocKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBracketToken,
                formattedNode.OpenBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBracketToken,
                formattedNode.CloseBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareQueryExpressionSyntax(
            QueryExpressionSyntax originalNode,
            QueryExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.FromClause);
            formattedStack.Push(formattedNode.FromClause);
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareQueryBodySyntax(
            QueryBodySyntax originalNode,
            QueryBodySyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.Clauses,
                formattedNode.Clauses,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.SelectOrGroup);
            formattedStack.Push(formattedNode.SelectOrGroup);
            originalStack.Push(originalNode.Continuation);
            formattedStack.Push(formattedNode.Continuation);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFromClauseSyntax(
            FromClauseSyntax originalNode,
            FromClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.FromKeyword,
                formattedNode.FromKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.InKeyword,
                formattedNode.InKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareLetClauseSyntax(
            LetClauseSyntax originalNode,
            LetClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.LetKeyword,
                formattedNode.LetKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EqualsToken,
                formattedNode.EqualsToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareJoinClauseSyntax(
            JoinClauseSyntax originalNode,
            JoinClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.JoinKeyword,
                formattedNode.JoinKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.InKeyword,
                formattedNode.InKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.InExpression);
            formattedStack.Push(formattedNode.InExpression);
            result = this.Compare(
                originalNode.OnKeyword,
                formattedNode.OnKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.LeftExpression);
            formattedStack.Push(formattedNode.LeftExpression);
            result = this.Compare(
                originalNode.EqualsKeyword,
                formattedNode.EqualsKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.RightExpression);
            formattedStack.Push(formattedNode.RightExpression);
            originalStack.Push(originalNode.Into);
            formattedStack.Push(formattedNode.Into);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareJoinIntoClauseSyntax(
            JoinIntoClauseSyntax originalNode,
            JoinIntoClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.IntoKeyword,
                formattedNode.IntoKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareWhereClauseSyntax(
            WhereClauseSyntax originalNode,
            WhereClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.WhereKeyword,
                formattedNode.WhereKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareOrderByClauseSyntax(
            OrderByClauseSyntax originalNode,
            OrderByClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OrderByKeyword,
                formattedNode.OrderByKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Orderings,
                formattedNode.Orderings,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Orderings.GetSeparators().ToList(),
                formattedNode.Orderings.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareOrderingSyntax(
            OrderingSyntax originalNode,
            OrderingSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.AscendingOrDescendingKeyword,
                formattedNode.AscendingOrDescendingKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSelectClauseSyntax(
            SelectClauseSyntax originalNode,
            SelectClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.SelectKeyword,
                formattedNode.SelectKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareGroupClauseSyntax(
            GroupClauseSyntax originalNode,
            GroupClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.GroupKeyword,
                formattedNode.GroupKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.GroupExpression);
            formattedStack.Push(formattedNode.GroupExpression);
            result = this.Compare(
                originalNode.ByKeyword,
                formattedNode.ByKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ByExpression);
            formattedStack.Push(formattedNode.ByExpression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareQueryContinuationSyntax(
            QueryContinuationSyntax originalNode,
            QueryContinuationSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.IntoKeyword,
                formattedNode.IntoKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareOmittedArraySizeExpressionSyntax(
            OmittedArraySizeExpressionSyntax originalNode,
            OmittedArraySizeExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OmittedArraySizeExpressionToken,
                formattedNode.OmittedArraySizeExpressionToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareInterpolatedStringExpressionSyntax(
            InterpolatedStringExpressionSyntax originalNode,
            InterpolatedStringExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.StringStartToken,
                formattedNode.StringStartToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Contents,
                formattedNode.Contents,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.StringEndToken,
                formattedNode.StringEndToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareIsPatternExpressionSyntax(
            IsPatternExpressionSyntax originalNode,
            IsPatternExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.IsKeyword,
                formattedNode.IsKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Pattern);
            formattedStack.Push(formattedNode.Pattern);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareThrowExpressionSyntax(
            ThrowExpressionSyntax originalNode,
            ThrowExpressionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ThrowKeyword,
                formattedNode.ThrowKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareWhenClauseSyntax(
            WhenClauseSyntax originalNode,
            WhenClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.WhenKeyword,
                formattedNode.WhenKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDiscardPatternSyntax(
            DiscardPatternSyntax originalNode,
            DiscardPatternSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.UnderscoreToken,
                formattedNode.UnderscoreToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDeclarationPatternSyntax(
            DeclarationPatternSyntax originalNode,
            DeclarationPatternSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.Designation);
            formattedStack.Push(formattedNode.Designation);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareVarPatternSyntax(
            VarPatternSyntax originalNode,
            VarPatternSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.VarKeyword,
                formattedNode.VarKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Designation);
            formattedStack.Push(formattedNode.Designation);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRecursivePatternSyntax(
            RecursivePatternSyntax originalNode,
            RecursivePatternSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.PositionalPatternClause);
            formattedStack.Push(formattedNode.PositionalPatternClause);
            originalStack.Push(originalNode.PropertyPatternClause);
            formattedStack.Push(formattedNode.PropertyPatternClause);
            originalStack.Push(originalNode.Designation);
            formattedStack.Push(formattedNode.Designation);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePositionalPatternClauseSyntax(
            PositionalPatternClauseSyntax originalNode,
            PositionalPatternClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Subpatterns,
                formattedNode.Subpatterns,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Subpatterns.GetSeparators().ToList(),
                formattedNode.Subpatterns.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePropertyPatternClauseSyntax(
            PropertyPatternClauseSyntax originalNode,
            PropertyPatternClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Subpatterns,
                formattedNode.Subpatterns,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Subpatterns.GetSeparators().ToList(),
                formattedNode.Subpatterns.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSubpatternSyntax(
            SubpatternSyntax originalNode,
            SubpatternSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.NameColon);
            formattedStack.Push(formattedNode.NameColon);
            originalStack.Push(originalNode.Pattern);
            formattedStack.Push(formattedNode.Pattern);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareConstantPatternSyntax(
            ConstantPatternSyntax originalNode,
            ConstantPatternSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareParenthesizedPatternSyntax(
            ParenthesizedPatternSyntax originalNode,
            ParenthesizedPatternSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Pattern);
            formattedStack.Push(formattedNode.Pattern);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRelationalPatternSyntax(
            RelationalPatternSyntax originalNode,
            RelationalPatternSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTypePatternSyntax(
            TypePatternSyntax originalNode,
            TypePatternSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBinaryPatternSyntax(
            BinaryPatternSyntax originalNode,
            BinaryPatternSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Left);
            formattedStack.Push(formattedNode.Left);
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Right);
            formattedStack.Push(formattedNode.Right);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareUnaryPatternSyntax(
            UnaryPatternSyntax originalNode,
            UnaryPatternSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Pattern);
            formattedStack.Push(formattedNode.Pattern);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareInterpolatedStringTextSyntax(
            InterpolatedStringTextSyntax originalNode,
            InterpolatedStringTextSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.TextToken,
                formattedNode.TextToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareInterpolationSyntax(
            InterpolationSyntax originalNode,
            InterpolationSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            originalStack.Push(originalNode.AlignmentClause);
            formattedStack.Push(formattedNode.AlignmentClause);
            originalStack.Push(originalNode.FormatClause);
            formattedStack.Push(formattedNode.FormatClause);
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareInterpolationAlignmentClauseSyntax(
            InterpolationAlignmentClauseSyntax originalNode,
            InterpolationAlignmentClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.CommaToken,
                formattedNode.CommaToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Value);
            formattedStack.Push(formattedNode.Value);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareInterpolationFormatClauseSyntax(
            InterpolationFormatClauseSyntax originalNode,
            InterpolationFormatClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.FormatStringToken,
                formattedNode.FormatStringToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareGlobalStatementSyntax(
            GlobalStatementSyntax originalNode,
            GlobalStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBlockSyntax(
            BlockSyntax originalNode,
            BlockSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Statements,
                formattedNode.Statements,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareLocalFunctionStatementSyntax(
            LocalFunctionStatementSyntax originalNode,
            LocalFunctionStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ReturnType);
            formattedStack.Push(formattedNode.ReturnType);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.TypeParameterList);
            formattedStack.Push(formattedNode.TypeParameterList);
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            result = this.CompareLists(
                originalNode.ConstraintClauses,
                formattedNode.ConstraintClauses,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareLocalDeclarationStatementSyntax(
            LocalDeclarationStatementSyntax originalNode,
            LocalDeclarationStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.AwaitKeyword,
                formattedNode.AwaitKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.UsingKeyword,
                formattedNode.UsingKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Declaration);
            formattedStack.Push(formattedNode.Declaration);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsConst != formattedNode.IsConst)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareVariableDeclarationSyntax(
            VariableDeclarationSyntax originalNode,
            VariableDeclarationSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.CompareLists(
                originalNode.Variables,
                formattedNode.Variables,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Variables.GetSeparators().ToList(),
                formattedNode.Variables.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareVariableDeclaratorSyntax(
            VariableDeclaratorSyntax originalNode,
            VariableDeclaratorSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareEqualsValueClauseSyntax(
            EqualsValueClauseSyntax originalNode,
            EqualsValueClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.EqualsToken,
                formattedNode.EqualsToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Value);
            formattedStack.Push(formattedNode.Value);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSingleVariableDesignationSyntax(
            SingleVariableDesignationSyntax originalNode,
            SingleVariableDesignationSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDiscardDesignationSyntax(
            DiscardDesignationSyntax originalNode,
            DiscardDesignationSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.UnderscoreToken,
                formattedNode.UnderscoreToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareParenthesizedVariableDesignationSyntax(
            ParenthesizedVariableDesignationSyntax originalNode,
            ParenthesizedVariableDesignationSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Variables,
                formattedNode.Variables,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Variables.GetSeparators().ToList(),
                formattedNode.Variables.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareExpressionStatementSyntax(
            ExpressionStatementSyntax originalNode,
            ExpressionStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (
                originalNode.AllowsAnyExpression != formattedNode.AllowsAnyExpression
            )
                return NotEqual(originalNode, formattedNode);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareEmptyStatementSyntax(
            EmptyStatementSyntax originalNode,
            EmptyStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareLabeledStatementSyntax(
            LabeledStatementSyntax originalNode,
            LabeledStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareGotoStatementSyntax(
            GotoStatementSyntax originalNode,
            GotoStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.GotoKeyword,
                formattedNode.GotoKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CaseOrDefaultKeyword,
                formattedNode.CaseOrDefaultKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBreakStatementSyntax(
            BreakStatementSyntax originalNode,
            BreakStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.BreakKeyword,
                formattedNode.BreakKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareContinueStatementSyntax(
            ContinueStatementSyntax originalNode,
            ContinueStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ContinueKeyword,
                formattedNode.ContinueKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareReturnStatementSyntax(
            ReturnStatementSyntax originalNode,
            ReturnStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ReturnKeyword,
                formattedNode.ReturnKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareThrowStatementSyntax(
            ThrowStatementSyntax originalNode,
            ThrowStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ThrowKeyword,
                formattedNode.ThrowKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareYieldStatementSyntax(
            YieldStatementSyntax originalNode,
            YieldStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.YieldKeyword,
                formattedNode.YieldKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ReturnOrBreakKeyword,
                formattedNode.ReturnOrBreakKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareWhileStatementSyntax(
            WhileStatementSyntax originalNode,
            WhileStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.WhileKeyword,
                formattedNode.WhileKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDoStatementSyntax(
            DoStatementSyntax originalNode,
            DoStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.DoKeyword,
                formattedNode.DoKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            result = this.Compare(
                originalNode.WhileKeyword,
                formattedNode.WhileKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareForStatementSyntax(
            ForStatementSyntax originalNode,
            ForStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ForKeyword,
                formattedNode.ForKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Declaration);
            formattedStack.Push(formattedNode.Declaration);
            result = this.CompareLists(
                originalNode.Initializers,
                formattedNode.Initializers,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Initializers.GetSeparators().ToList(),
                formattedNode.Initializers.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.FirstSemicolonToken,
                formattedNode.FirstSemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            result = this.Compare(
                originalNode.SecondSemicolonToken,
                formattedNode.SecondSemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Incrementors,
                formattedNode.Incrementors,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Incrementors.GetSeparators().ToList(),
                formattedNode.Incrementors.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareForEachStatementSyntax(
            ForEachStatementSyntax originalNode,
            ForEachStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.AwaitKeyword,
                formattedNode.AwaitKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ForEachKeyword,
                formattedNode.ForEachKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.InKeyword,
                formattedNode.InKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareForEachVariableStatementSyntax(
            ForEachVariableStatementSyntax originalNode,
            ForEachVariableStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.AwaitKeyword,
                formattedNode.AwaitKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ForEachKeyword,
                formattedNode.ForEachKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Variable);
            formattedStack.Push(formattedNode.Variable);
            result = this.Compare(
                originalNode.InKeyword,
                formattedNode.InKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareUsingStatementSyntax(
            UsingStatementSyntax originalNode,
            UsingStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.AwaitKeyword,
                formattedNode.AwaitKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.UsingKeyword,
                formattedNode.UsingKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Declaration);
            formattedStack.Push(formattedNode.Declaration);
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFixedStatementSyntax(
            FixedStatementSyntax originalNode,
            FixedStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.FixedKeyword,
                formattedNode.FixedKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Declaration);
            formattedStack.Push(formattedNode.Declaration);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCheckedStatementSyntax(
            CheckedStatementSyntax originalNode,
            CheckedStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Block);
            formattedStack.Push(formattedNode.Block);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareUnsafeStatementSyntax(
            UnsafeStatementSyntax originalNode,
            UnsafeStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.UnsafeKeyword,
                formattedNode.UnsafeKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Block);
            formattedStack.Push(formattedNode.Block);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareLockStatementSyntax(
            LockStatementSyntax originalNode,
            LockStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.LockKeyword,
                formattedNode.LockKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareIfStatementSyntax(
            IfStatementSyntax originalNode,
            IfStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.IfKeyword,
                formattedNode.IfKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            originalStack.Push(originalNode.Else);
            formattedStack.Push(formattedNode.Else);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareElseClauseSyntax(
            ElseClauseSyntax originalNode,
            ElseClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ElseKeyword,
                formattedNode.ElseKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Statement);
            formattedStack.Push(formattedNode.Statement);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSwitchStatementSyntax(
            SwitchStatementSyntax originalNode,
            SwitchStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SwitchKeyword,
                formattedNode.SwitchKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Sections,
                formattedNode.Sections,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSwitchSectionSyntax(
            SwitchSectionSyntax originalNode,
            SwitchSectionSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.Labels,
                formattedNode.Labels,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Statements,
                formattedNode.Statements,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCasePatternSwitchLabelSyntax(
            CasePatternSwitchLabelSyntax originalNode,
            CasePatternSwitchLabelSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Pattern);
            formattedStack.Push(formattedNode.Pattern);
            originalStack.Push(originalNode.WhenClause);
            formattedStack.Push(formattedNode.WhenClause);
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCaseSwitchLabelSyntax(
            CaseSwitchLabelSyntax originalNode,
            CaseSwitchLabelSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Value);
            formattedStack.Push(formattedNode.Value);
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDefaultSwitchLabelSyntax(
            DefaultSwitchLabelSyntax originalNode,
            DefaultSwitchLabelSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSwitchExpressionSyntax(
            SwitchExpressionSyntax originalNode,
            SwitchExpressionSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.GoverningExpression);
            formattedStack.Push(formattedNode.GoverningExpression);
            result = this.Compare(
                originalNode.SwitchKeyword,
                formattedNode.SwitchKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arms,
                formattedNode.Arms,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arms.GetSeparators().ToList(),
                formattedNode.Arms.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSwitchExpressionArmSyntax(
            SwitchExpressionArmSyntax originalNode,
            SwitchExpressionArmSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Pattern);
            formattedStack.Push(formattedNode.Pattern);
            originalStack.Push(originalNode.WhenClause);
            formattedStack.Push(formattedNode.WhenClause);
            result = this.Compare(
                originalNode.EqualsGreaterThanToken,
                formattedNode.EqualsGreaterThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTryStatementSyntax(
            TryStatementSyntax originalNode,
            TryStatementSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.TryKeyword,
                formattedNode.TryKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Block);
            formattedStack.Push(formattedNode.Block);
            result = this.CompareLists(
                originalNode.Catches,
                formattedNode.Catches,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Finally);
            formattedStack.Push(formattedNode.Finally);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCatchClauseSyntax(
            CatchClauseSyntax originalNode,
            CatchClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.CatchKeyword,
                formattedNode.CatchKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Declaration);
            formattedStack.Push(formattedNode.Declaration);
            originalStack.Push(originalNode.Filter);
            formattedStack.Push(formattedNode.Filter);
            originalStack.Push(originalNode.Block);
            formattedStack.Push(formattedNode.Block);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCatchDeclarationSyntax(
            CatchDeclarationSyntax originalNode,
            CatchDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCatchFilterClauseSyntax(
            CatchFilterClauseSyntax originalNode,
            CatchFilterClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.WhenKeyword,
                formattedNode.WhenKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.FilterExpression);
            formattedStack.Push(formattedNode.FilterExpression);
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFinallyClauseSyntax(
            FinallyClauseSyntax originalNode,
            FinallyClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.FinallyKeyword,
                formattedNode.FinallyKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Block);
            formattedStack.Push(formattedNode.Block);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCompilationUnitSyntax(
            CompilationUnitSyntax originalNode,
            CompilationUnitSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.Externs,
                formattedNode.Externs,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Usings,
                formattedNode.Usings,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Members,
                formattedNode.Members,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfFileToken,
                formattedNode.EndOfFileToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareExternAliasDirectiveSyntax(
            ExternAliasDirectiveSyntax originalNode,
            ExternAliasDirectiveSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ExternKeyword,
                formattedNode.ExternKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.AliasKeyword,
                formattedNode.AliasKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareUsingDirectiveSyntax(
            UsingDirectiveSyntax originalNode,
            UsingDirectiveSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.UsingKeyword,
                formattedNode.UsingKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.StaticKeyword,
                formattedNode.StaticKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Alias);
            formattedStack.Push(formattedNode.Alias);
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareNamespaceDeclarationSyntax(
            NamespaceDeclarationSyntax originalNode,
            NamespaceDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.NamespaceKeyword,
                formattedNode.NamespaceKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Externs,
                formattedNode.Externs,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Usings,
                formattedNode.Usings,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Members,
                formattedNode.Members,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAttributeListSyntax(
            AttributeListSyntax originalNode,
            AttributeListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBracketToken,
                formattedNode.OpenBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Target);
            formattedStack.Push(formattedNode.Target);
            result = this.CompareLists(
                originalNode.Attributes,
                formattedNode.Attributes,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Attributes.GetSeparators().ToList(),
                formattedNode.Attributes.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBracketToken,
                formattedNode.CloseBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAttributeTargetSpecifierSyntax(
            AttributeTargetSpecifierSyntax originalNode,
            AttributeTargetSpecifierSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAttributeSyntax(
            AttributeSyntax originalNode,
            AttributeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAttributeArgumentListSyntax(
            AttributeArgumentListSyntax originalNode,
            AttributeArgumentListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments,
                formattedNode.Arguments,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Arguments.GetSeparators().ToList(),
                formattedNode.Arguments.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAttributeArgumentSyntax(
            AttributeArgumentSyntax originalNode,
            AttributeArgumentSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.NameEquals);
            formattedStack.Push(formattedNode.NameEquals);
            originalStack.Push(originalNode.NameColon);
            formattedStack.Push(formattedNode.NameColon);
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareNameEqualsSyntax(
            NameEqualsSyntax originalNode,
            NameEqualsSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.EqualsToken,
                formattedNode.EqualsToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTypeParameterListSyntax(
            TypeParameterListSyntax originalNode,
            TypeParameterListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.LessThanToken,
                formattedNode.LessThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters,
                formattedNode.Parameters,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters.GetSeparators().ToList(),
                formattedNode.Parameters.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.GreaterThanToken,
                formattedNode.GreaterThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTypeParameterSyntax(
            TypeParameterSyntax originalNode,
            TypeParameterSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.VarianceKeyword,
                formattedNode.VarianceKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareClassDeclarationSyntax(
            ClassDeclarationSyntax originalNode,
            ClassDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.TypeParameterList);
            formattedStack.Push(formattedNode.TypeParameterList);
            originalStack.Push(originalNode.BaseList);
            formattedStack.Push(formattedNode.BaseList);
            result = this.CompareLists(
                originalNode.ConstraintClauses,
                formattedNode.ConstraintClauses,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Members,
                formattedNode.Members,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareStructDeclarationSyntax(
            StructDeclarationSyntax originalNode,
            StructDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.TypeParameterList);
            formattedStack.Push(formattedNode.TypeParameterList);
            originalStack.Push(originalNode.BaseList);
            formattedStack.Push(formattedNode.BaseList);
            result = this.CompareLists(
                originalNode.ConstraintClauses,
                formattedNode.ConstraintClauses,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Members,
                formattedNode.Members,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareInterfaceDeclarationSyntax(
            InterfaceDeclarationSyntax originalNode,
            InterfaceDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.TypeParameterList);
            formattedStack.Push(formattedNode.TypeParameterList);
            originalStack.Push(originalNode.BaseList);
            formattedStack.Push(formattedNode.BaseList);
            result = this.CompareLists(
                originalNode.ConstraintClauses,
                formattedNode.ConstraintClauses,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Members,
                formattedNode.Members,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRecordDeclarationSyntax(
            RecordDeclarationSyntax originalNode,
            RecordDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.TypeParameterList);
            formattedStack.Push(formattedNode.TypeParameterList);
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            originalStack.Push(originalNode.BaseList);
            formattedStack.Push(formattedNode.BaseList);
            result = this.CompareLists(
                originalNode.ConstraintClauses,
                formattedNode.ConstraintClauses,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Members,
                formattedNode.Members,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareEnumDeclarationSyntax(
            EnumDeclarationSyntax originalNode,
            EnumDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EnumKeyword,
                formattedNode.EnumKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.BaseList);
            formattedStack.Push(formattedNode.BaseList);
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Members,
                formattedNode.Members,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Members.GetSeparators().ToList(),
                formattedNode.Members.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDelegateDeclarationSyntax(
            DelegateDeclarationSyntax originalNode,
            DelegateDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.DelegateKeyword,
                formattedNode.DelegateKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ReturnType);
            formattedStack.Push(formattedNode.ReturnType);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.TypeParameterList);
            formattedStack.Push(formattedNode.TypeParameterList);
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            result = this.CompareLists(
                originalNode.ConstraintClauses,
                formattedNode.ConstraintClauses,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareEnumMemberDeclarationSyntax(
            EnumMemberDeclarationSyntax originalNode,
            EnumMemberDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.EqualsValue);
            formattedStack.Push(formattedNode.EqualsValue);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBaseListSyntax(
            BaseListSyntax originalNode,
            BaseListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Types,
                formattedNode.Types,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Types.GetSeparators().ToList(),
                formattedNode.Types.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSimpleBaseTypeSyntax(
            SimpleBaseTypeSyntax originalNode,
            SimpleBaseTypeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePrimaryConstructorBaseTypeSyntax(
            PrimaryConstructorBaseTypeSyntax originalNode,
            PrimaryConstructorBaseTypeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTypeParameterConstraintClauseSyntax(
            TypeParameterConstraintClauseSyntax originalNode,
            TypeParameterConstraintClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.WhereKeyword,
                formattedNode.WhereKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Constraints,
                formattedNode.Constraints,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Constraints.GetSeparators().ToList(),
                formattedNode.Constraints.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareConstructorConstraintSyntax(
            ConstructorConstraintSyntax originalNode,
            ConstructorConstraintSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.NewKeyword,
                formattedNode.NewKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareClassOrStructConstraintSyntax(
            ClassOrStructConstraintSyntax originalNode,
            ClassOrStructConstraintSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ClassOrStructKeyword,
                formattedNode.ClassOrStructKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.QuestionToken,
                formattedNode.QuestionToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTypeConstraintSyntax(
            TypeConstraintSyntax originalNode,
            TypeConstraintSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDefaultConstraintSyntax(
            DefaultConstraintSyntax originalNode,
            DefaultConstraintSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.DefaultKeyword,
                formattedNode.DefaultKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFieldDeclarationSyntax(
            FieldDeclarationSyntax originalNode,
            FieldDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Declaration);
            formattedStack.Push(formattedNode.Declaration);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareEventFieldDeclarationSyntax(
            EventFieldDeclarationSyntax originalNode,
            EventFieldDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EventKeyword,
                formattedNode.EventKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Declaration);
            formattedStack.Push(formattedNode.Declaration);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareExplicitInterfaceSpecifierSyntax(
            ExplicitInterfaceSpecifierSyntax originalNode,
            ExplicitInterfaceSpecifierSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.DotToken,
                formattedNode.DotToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareMethodDeclarationSyntax(
            MethodDeclarationSyntax originalNode,
            MethodDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ReturnType);
            formattedStack.Push(formattedNode.ReturnType);
            originalStack.Push(originalNode.ExplicitInterfaceSpecifier);
            formattedStack.Push(formattedNode.ExplicitInterfaceSpecifier);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.TypeParameterList);
            formattedStack.Push(formattedNode.TypeParameterList);
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            result = this.CompareLists(
                originalNode.ConstraintClauses,
                formattedNode.ConstraintClauses,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareOperatorDeclarationSyntax(
            OperatorDeclarationSyntax originalNode,
            OperatorDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ReturnType);
            formattedStack.Push(formattedNode.ReturnType);
            result = this.Compare(
                originalNode.OperatorKeyword,
                formattedNode.OperatorKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareConversionOperatorDeclarationSyntax(
            ConversionOperatorDeclarationSyntax originalNode,
            ConversionOperatorDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ImplicitOrExplicitKeyword,
                formattedNode.ImplicitOrExplicitKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OperatorKeyword,
                formattedNode.OperatorKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareConstructorDeclarationSyntax(
            ConstructorDeclarationSyntax originalNode,
            ConstructorDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareConstructorInitializerSyntax(
            ConstructorInitializerSyntax originalNode,
            ConstructorInitializerSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ThisOrBaseKeyword,
                formattedNode.ThisOrBaseKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ArgumentList);
            formattedStack.Push(formattedNode.ArgumentList);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDestructorDeclarationSyntax(
            DestructorDeclarationSyntax originalNode,
            DestructorDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.TildeToken,
                formattedNode.TildeToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePropertyDeclarationSyntax(
            PropertyDeclarationSyntax originalNode,
            PropertyDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.ExplicitInterfaceSpecifier);
            formattedStack.Push(formattedNode.ExplicitInterfaceSpecifier);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.AccessorList);
            formattedStack.Push(formattedNode.AccessorList);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            originalStack.Push(originalNode.Initializer);
            formattedStack.Push(formattedNode.Initializer);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareArrowExpressionClauseSyntax(
            ArrowExpressionClauseSyntax originalNode,
            ArrowExpressionClauseSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ArrowToken,
                formattedNode.ArrowToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Expression);
            formattedStack.Push(formattedNode.Expression);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareEventDeclarationSyntax(
            EventDeclarationSyntax originalNode,
            EventDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EventKeyword,
                formattedNode.EventKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.ExplicitInterfaceSpecifier);
            formattedStack.Push(formattedNode.ExplicitInterfaceSpecifier);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.AccessorList);
            formattedStack.Push(formattedNode.AccessorList);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareIndexerDeclarationSyntax(
            IndexerDeclarationSyntax originalNode,
            IndexerDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.ExplicitInterfaceSpecifier);
            formattedStack.Push(formattedNode.ExplicitInterfaceSpecifier);
            result = this.Compare(
                originalNode.ThisKeyword,
                formattedNode.ThisKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.ParameterList);
            formattedStack.Push(formattedNode.ParameterList);
            originalStack.Push(originalNode.AccessorList);
            formattedStack.Push(formattedNode.AccessorList);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAccessorListSyntax(
            AccessorListSyntax originalNode,
            AccessorListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBraceToken,
                formattedNode.OpenBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Accessors,
                formattedNode.Accessors,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBraceToken,
                formattedNode.CloseBraceToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareAccessorDeclarationSyntax(
            AccessorDeclarationSyntax originalNode,
            AccessorDeclarationSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Keyword,
                formattedNode.Keyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Body);
            formattedStack.Push(formattedNode.Body);
            originalStack.Push(originalNode.ExpressionBody);
            formattedStack.Push(formattedNode.ExpressionBody);
            result = this.Compare(
                originalNode.SemicolonToken,
                formattedNode.SemicolonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareParameterListSyntax(
            ParameterListSyntax originalNode,
            ParameterListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters,
                formattedNode.Parameters,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters.GetSeparators().ToList(),
                formattedNode.Parameters.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBracketedParameterListSyntax(
            BracketedParameterListSyntax originalNode,
            BracketedParameterListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBracketToken,
                formattedNode.OpenBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters,
                formattedNode.Parameters,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters.GetSeparators().ToList(),
                formattedNode.Parameters.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBracketToken,
                formattedNode.CloseBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareParameterSyntax(
            ParameterSyntax originalNode,
            ParameterSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Default);
            formattedStack.Push(formattedNode.Default);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareFunctionPointerParameterSyntax(
            FunctionPointerParameterSyntax originalNode,
            FunctionPointerParameterSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareIncompleteMemberSyntax(
            IncompleteMemberSyntax originalNode,
            IncompleteMemberSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.AttributeLists,
                formattedNode.AttributeLists,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Modifiers,
                formattedNode.Modifiers,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareSkippedTokensTriviaSyntax(
            SkippedTokensTriviaSyntax originalNode,
            SkippedTokensTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.Tokens,
                formattedNode.Tokens,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDocumentationCommentTriviaSyntax(
            DocumentationCommentTriviaSyntax originalNode,
            DocumentationCommentTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.Content,
                formattedNode.Content,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfComment,
                formattedNode.EndOfComment,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareTypeCrefSyntax(
            TypeCrefSyntax originalNode,
            TypeCrefSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareQualifiedCrefSyntax(
            QualifiedCrefSyntax originalNode,
            QualifiedCrefSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Container);
            formattedStack.Push(formattedNode.Container);
            result = this.Compare(
                originalNode.DotToken,
                formattedNode.DotToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Member);
            formattedStack.Push(formattedNode.Member);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareNameMemberCrefSyntax(
            NameMemberCrefSyntax originalNode,
            NameMemberCrefSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            originalStack.Push(originalNode.Parameters);
            formattedStack.Push(formattedNode.Parameters);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareIndexerMemberCrefSyntax(
            IndexerMemberCrefSyntax originalNode,
            IndexerMemberCrefSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ThisKeyword,
                formattedNode.ThisKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Parameters);
            formattedStack.Push(formattedNode.Parameters);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareOperatorMemberCrefSyntax(
            OperatorMemberCrefSyntax originalNode,
            OperatorMemberCrefSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OperatorKeyword,
                formattedNode.OperatorKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OperatorToken,
                formattedNode.OperatorToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Parameters);
            formattedStack.Push(formattedNode.Parameters);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareConversionOperatorMemberCrefSyntax(
            ConversionOperatorMemberCrefSyntax originalNode,
            ConversionOperatorMemberCrefSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.ImplicitOrExplicitKeyword,
                formattedNode.ImplicitOrExplicitKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.OperatorKeyword,
                formattedNode.OperatorKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            originalStack.Push(originalNode.Parameters);
            formattedStack.Push(formattedNode.Parameters);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCrefParameterListSyntax(
            CrefParameterListSyntax originalNode,
            CrefParameterListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenParenToken,
                formattedNode.OpenParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters,
                formattedNode.Parameters,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters.GetSeparators().ToList(),
                formattedNode.Parameters.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseParenToken,
                formattedNode.CloseParenToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCrefBracketedParameterListSyntax(
            CrefBracketedParameterListSyntax originalNode,
            CrefBracketedParameterListSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.OpenBracketToken,
                formattedNode.OpenBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters,
                formattedNode.Parameters,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.Parameters.GetSeparators().ToList(),
                formattedNode.Parameters.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.CloseBracketToken,
                formattedNode.CloseBracketToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareCrefParameterSyntax(
            CrefParameterSyntax originalNode,
            CrefParameterSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.RefKindKeyword,
                formattedNode.RefKindKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Type);
            formattedStack.Push(formattedNode.Type);
            result = this.Compare(
                originalNode.RefOrOutKeyword,
                formattedNode.RefOrOutKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlElementSyntax(
            XmlElementSyntax originalNode,
            XmlElementSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.StartTag);
            formattedStack.Push(formattedNode.StartTag);
            result = this.CompareLists(
                originalNode.Content,
                formattedNode.Content,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.EndTag);
            formattedStack.Push(formattedNode.EndTag);
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlElementStartTagSyntax(
            XmlElementStartTagSyntax originalNode,
            XmlElementStartTagSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.LessThanToken,
                formattedNode.LessThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.CompareLists(
                originalNode.Attributes,
                formattedNode.Attributes,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.GreaterThanToken,
                formattedNode.GreaterThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlElementEndTagSyntax(
            XmlElementEndTagSyntax originalNode,
            XmlElementEndTagSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.LessThanSlashToken,
                formattedNode.LessThanSlashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.GreaterThanToken,
                formattedNode.GreaterThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlEmptyElementSyntax(
            XmlEmptyElementSyntax originalNode,
            XmlEmptyElementSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.LessThanToken,
                formattedNode.LessThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.CompareLists(
                originalNode.Attributes,
                formattedNode.Attributes,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SlashGreaterThanToken,
                formattedNode.SlashGreaterThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlNameSyntax(
            XmlNameSyntax originalNode,
            XmlNameSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Prefix);
            formattedStack.Push(formattedNode.Prefix);
            result = this.Compare(
                originalNode.LocalName,
                formattedNode.LocalName,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlPrefixSyntax(
            XmlPrefixSyntax originalNode,
            XmlPrefixSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.Prefix,
                formattedNode.Prefix,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ColonToken,
                formattedNode.ColonToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlTextAttributeSyntax(
            XmlTextAttributeSyntax originalNode,
            XmlTextAttributeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.EqualsToken,
                formattedNode.EqualsToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.StartQuoteToken,
                formattedNode.StartQuoteToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.TextTokens,
                formattedNode.TextTokens,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndQuoteToken,
                formattedNode.EndQuoteToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlCrefAttributeSyntax(
            XmlCrefAttributeSyntax originalNode,
            XmlCrefAttributeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.EqualsToken,
                formattedNode.EqualsToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.StartQuoteToken,
                formattedNode.StartQuoteToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Cref);
            formattedStack.Push(formattedNode.Cref);
            result = this.Compare(
                originalNode.EndQuoteToken,
                formattedNode.EndQuoteToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlNameAttributeSyntax(
            XmlNameAttributeSyntax originalNode,
            XmlNameAttributeSyntax formattedNode)
        {
            CompareResult result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.Compare(
                originalNode.EqualsToken,
                formattedNode.EqualsToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.StartQuoteToken,
                formattedNode.StartQuoteToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Identifier);
            formattedStack.Push(formattedNode.Identifier);
            result = this.Compare(
                originalNode.EndQuoteToken,
                formattedNode.EndQuoteToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlTextSyntax(
            XmlTextSyntax originalNode,
            XmlTextSyntax formattedNode)
        {
            CompareResult result;
            result = this.CompareLists(
                originalNode.TextTokens,
                formattedNode.TextTokens,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlCDataSectionSyntax(
            XmlCDataSectionSyntax originalNode,
            XmlCDataSectionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.StartCDataToken,
                formattedNode.StartCDataToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.TextTokens,
                formattedNode.TextTokens,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndCDataToken,
                formattedNode.EndCDataToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlProcessingInstructionSyntax(
            XmlProcessingInstructionSyntax originalNode,
            XmlProcessingInstructionSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.StartProcessingInstructionToken,
                formattedNode.StartProcessingInstructionToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Name);
            formattedStack.Push(formattedNode.Name);
            result = this.CompareLists(
                originalNode.TextTokens,
                formattedNode.TextTokens,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndProcessingInstructionToken,
                formattedNode.EndProcessingInstructionToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareXmlCommentSyntax(
            XmlCommentSyntax originalNode,
            XmlCommentSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.LessThanExclamationMinusMinusToken,
                formattedNode.LessThanExclamationMinusMinusToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.TextTokens,
                formattedNode.TextTokens,
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.MinusMinusGreaterThanToken,
                formattedNode.MinusMinusGreaterThanToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareIfDirectiveTriviaSyntax(
            IfDirectiveTriviaSyntax originalNode,
            IfDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.IfKeyword,
                formattedNode.IfKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.BranchTaken != formattedNode.BranchTaken)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.ConditionValue != formattedNode.ConditionValue)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareElifDirectiveTriviaSyntax(
            ElifDirectiveTriviaSyntax originalNode,
            ElifDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ElifKeyword,
                formattedNode.ElifKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            originalStack.Push(originalNode.Condition);
            formattedStack.Push(formattedNode.Condition);
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.BranchTaken != formattedNode.BranchTaken)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.ConditionValue != formattedNode.ConditionValue)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareElseDirectiveTriviaSyntax(
            ElseDirectiveTriviaSyntax originalNode,
            ElseDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ElseKeyword,
                formattedNode.ElseKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.BranchTaken != formattedNode.BranchTaken)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareEndIfDirectiveTriviaSyntax(
            EndIfDirectiveTriviaSyntax originalNode,
            EndIfDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndIfKeyword,
                formattedNode.EndIfKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareRegionDirectiveTriviaSyntax(
            RegionDirectiveTriviaSyntax originalNode,
            RegionDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.RegionKeyword,
                formattedNode.RegionKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareEndRegionDirectiveTriviaSyntax(
            EndRegionDirectiveTriviaSyntax originalNode,
            EndRegionDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndRegionKeyword,
                formattedNode.EndRegionKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareErrorDirectiveTriviaSyntax(
            ErrorDirectiveTriviaSyntax originalNode,
            ErrorDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ErrorKeyword,
                formattedNode.ErrorKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareWarningDirectiveTriviaSyntax(
            WarningDirectiveTriviaSyntax originalNode,
            WarningDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.WarningKeyword,
                formattedNode.WarningKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareBadDirectiveTriviaSyntax(
            BadDirectiveTriviaSyntax originalNode,
            BadDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Identifier,
                formattedNode.Identifier,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareDefineDirectiveTriviaSyntax(
            DefineDirectiveTriviaSyntax originalNode,
            DefineDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.DefineKeyword,
                formattedNode.DefineKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Name,
                formattedNode.Name,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareUndefDirectiveTriviaSyntax(
            UndefDirectiveTriviaSyntax originalNode,
            UndefDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.UndefKeyword,
                formattedNode.UndefKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Name,
                formattedNode.Name,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareLineDirectiveTriviaSyntax(
            LineDirectiveTriviaSyntax originalNode,
            LineDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.LineKeyword,
                formattedNode.LineKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Line,
                formattedNode.Line,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.File,
                formattedNode.File,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePragmaWarningDirectiveTriviaSyntax(
            PragmaWarningDirectiveTriviaSyntax originalNode,
            PragmaWarningDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.PragmaKeyword,
                formattedNode.PragmaKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.WarningKeyword,
                formattedNode.WarningKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.DisableOrRestoreKeyword,
                formattedNode.DisableOrRestoreKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.ErrorCodes,
                formattedNode.ErrorCodes,
                null,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.CompareLists(
                originalNode.ErrorCodes.GetSeparators().ToList(),
                formattedNode.ErrorCodes.GetSeparators().ToList(),
                Compare,
                o => o.Span,
                originalNode.Span,
                formattedNode.Span);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult ComparePragmaChecksumDirectiveTriviaSyntax(
            PragmaChecksumDirectiveTriviaSyntax originalNode,
            PragmaChecksumDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.PragmaKeyword,
                formattedNode.PragmaKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ChecksumKeyword,
                formattedNode.ChecksumKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.File,
                formattedNode.File,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Guid,
                formattedNode.Guid,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.Bytes,
                formattedNode.Bytes,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareReferenceDirectiveTriviaSyntax(
            ReferenceDirectiveTriviaSyntax originalNode,
            ReferenceDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ReferenceKeyword,
                formattedNode.ReferenceKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.File,
                formattedNode.File,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareLoadDirectiveTriviaSyntax(
            LoadDirectiveTriviaSyntax originalNode,
            LoadDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.LoadKeyword,
                formattedNode.LoadKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.File,
                formattedNode.File,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareShebangDirectiveTriviaSyntax(
            ShebangDirectiveTriviaSyntax originalNode,
            ShebangDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.ExclamationToken,
                formattedNode.ExclamationToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
        private CompareResult CompareNullableDirectiveTriviaSyntax(
            NullableDirectiveTriviaSyntax originalNode,
            NullableDirectiveTriviaSyntax formattedNode)
        {
            CompareResult result;
            result = this.Compare(
                originalNode.HashToken,
                formattedNode.HashToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.NullableKeyword,
                formattedNode.NullableKeyword,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.SettingToken,
                formattedNode.SettingToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.TargetToken,
                formattedNode.TargetToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            result = this.Compare(
                originalNode.EndOfDirectiveToken,
                formattedNode.EndOfDirectiveToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.IsActive != formattedNode.IsActive)
                return NotEqual(originalNode, formattedNode);
            result = this.Compare(
                originalNode.DirectiveNameToken,
                formattedNode.DirectiveNameToken,
                originalNode,
                formattedNode);
            if (result.IsInvalid)
                return result;
            if (originalNode.RawKind != formattedNode.RawKind)
                return NotEqual(originalNode, formattedNode);
            if (originalNode.IsMissing != formattedNode.IsMissing)
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.IsStructuredTrivia != formattedNode.IsStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.HasStructuredTrivia != formattedNode.HasStructuredTrivia
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsSkippedText != formattedNode.ContainsSkippedText
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDirectives != formattedNode.ContainsDirectives
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsDiagnostics != formattedNode.ContainsDiagnostics
            )
                return NotEqual(originalNode, formattedNode);
            if (
                originalNode.ContainsAnnotations != formattedNode.ContainsAnnotations
            )
                return NotEqual(originalNode, formattedNode);
            return Equal;
        }
    }
}
