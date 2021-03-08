using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private int depth = 0;
        public Doc Print(SyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                return null;
            }

            // TODO 0 kill? runtime repo has files that will fail on deep recursion
            if (depth > 200)
            {
                throw new InTooDeepException();
            }

            depth++;
            try
            {
                switch (syntaxNode)
                {
                    case AccessorDeclarationSyntax accessorDeclarationSyntax:
                        return this.PrintAccessorDeclarationSyntax(
                            accessorDeclarationSyntax);
                    case AliasQualifiedNameSyntax aliasQualifiedNameSyntax:
                        return this.PrintAliasQualifiedNameSyntax(
                            aliasQualifiedNameSyntax);
                    case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax:
                        return this.PrintAnonymousMethodExpressionSyntax(
                            anonymousMethodExpressionSyntax);
                    case AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax:
                        return this.PrintAnonymousObjectCreationExpressionSyntax(
                            anonymousObjectCreationExpressionSyntax);
                    case AnonymousObjectMemberDeclaratorSyntax anonymousObjectMemberDeclaratorSyntax:
                        return this.PrintAnonymousObjectMemberDeclaratorSyntax(
                            anonymousObjectMemberDeclaratorSyntax);
                    case ArgumentListSyntax argumentListSyntax:
                        return this.PrintArgumentListSyntax(argumentListSyntax);
                    case ArgumentSyntax argumentSyntax:
                        return this.PrintArgumentSyntax(argumentSyntax);
                    case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax:
                        return this.PrintArrayCreationExpressionSyntax(
                            arrayCreationExpressionSyntax);
                    case ArrayRankSpecifierSyntax arrayRankSpecifierSyntax:
                        return this.PrintArrayRankSpecifierSyntax(
                            arrayRankSpecifierSyntax);
                    case ArrayTypeSyntax arrayTypeSyntax:
                        return this.PrintArrayTypeSyntax(arrayTypeSyntax);
                    case ArrowExpressionClauseSyntax arrowExpressionClauseSyntax:
                        return this.PrintArrowExpressionClauseSyntax(
                            arrowExpressionClauseSyntax);
                    case AssignmentExpressionSyntax assignmentExpressionSyntax:
                        return this.PrintAssignmentExpressionSyntax(
                            assignmentExpressionSyntax);
                    case AttributeListSyntax attributeListSyntax:
                        return this.PrintAttributeListSyntax(
                            attributeListSyntax);
                    case AwaitExpressionSyntax awaitExpressionSyntax:
                        return this.PrintAwaitExpressionSyntax(
                            awaitExpressionSyntax);
                    case BaseExpressionSyntax baseExpressionSyntax:
                        return this.PrintBaseExpressionSyntax(
                            baseExpressionSyntax);
                    case BaseListSyntax baseListSyntax:
                        return this.PrintBaseListSyntax(baseListSyntax);
                    case BinaryExpressionSyntax binaryExpressionSyntax:
                        return this.PrintBinaryExpressionSyntax(
                            binaryExpressionSyntax);
                    case BinaryPatternSyntax binaryPatternSyntax:
                        return this.PrintBinaryPatternSyntax(
                            binaryPatternSyntax);
                    case BlockSyntax blockSyntax:
                        return this.PrintBlockSyntax(blockSyntax);
                    case BracketedArgumentListSyntax bracketedArgumentListSyntax:
                        return this.PrintBracketedArgumentListSyntax(
                            bracketedArgumentListSyntax);
                    case BracketedParameterListSyntax bracketedParameterListSyntax:
                        return this.PrintBracketedParameterListSyntax(
                            bracketedParameterListSyntax);
                    case BreakStatementSyntax breakStatementSyntax:
                        return this.PrintBreakStatementSyntax(
                            breakStatementSyntax);
                    case CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax:
                        return this.PrintCasePatternSwitchLabelSyntax(
                            casePatternSwitchLabelSyntax);
                    case CaseSwitchLabelSyntax caseSwitchLabelSyntax:
                        return this.PrintCaseSwitchLabelSyntax(
                            caseSwitchLabelSyntax);
                    case CastExpressionSyntax castExpressionSyntax:
                        return this.PrintCastExpressionSyntax(
                            castExpressionSyntax);
                    case CatchClauseSyntax catchClauseSyntax:
                        return this.PrintCatchClauseSyntax(catchClauseSyntax);
                    case CheckedExpressionSyntax checkedExpressionSyntax:
                        return this.PrintCheckedExpressionSyntax(
                            checkedExpressionSyntax);
                    case CheckedStatementSyntax checkedStatementSyntax:
                        return this.PrintCheckedStatementSyntax(
                            checkedStatementSyntax);
                    case ClassDeclarationSyntax classDeclarationSyntax:
                        return this.PrintClassDeclarationSyntax(
                            classDeclarationSyntax);
                    case ClassOrStructConstraintSyntax classOrStructConstraintSyntax:
                        return this.PrintClassOrStructConstraintSyntax(
                            classOrStructConstraintSyntax);
                    case CompilationUnitSyntax compilationUnitSyntax:
                        return this.PrintCompilationUnitSyntax(
                            compilationUnitSyntax);
                    case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax:
                        return this.PrintConditionalAccessExpressionSyntax(
                            conditionalAccessExpressionSyntax);
                    case ConditionalExpressionSyntax conditionalExpressionSyntax:
                        return this.PrintConditionalExpressionSyntax(
                            conditionalExpressionSyntax);
                    case ConstantPatternSyntax constantPatternSyntax:
                        return this.PrintConstantPatternSyntax(
                            constantPatternSyntax);
                    case ConstructorConstraintSyntax constructorConstraintSyntax:
                        return this.PrintConstructorConstraintSyntax(
                            constructorConstraintSyntax);
                    case ConstructorDeclarationSyntax constructorDeclarationSyntax:
                        return this.PrintConstructorDeclarationSyntax(
                            constructorDeclarationSyntax);
                    case ConstructorInitializerSyntax constructorInitializerSyntax:
                        return this.PrintConstructorInitializerSyntax(
                            constructorInitializerSyntax);
                    case ContinueStatementSyntax continueStatementSyntax:
                        return this.PrintContinueStatementSyntax(
                            continueStatementSyntax);
                    case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax:
                        return this.PrintConversionOperatorDeclarationSyntax(
                            conversionOperatorDeclarationSyntax);
                    case DeclarationExpressionSyntax declarationExpressionSyntax:
                        return this.PrintDeclarationExpressionSyntax(
                            declarationExpressionSyntax);
                    case DeclarationPatternSyntax declarationPatternSyntax:
                        return this.PrintDeclarationPatternSyntax(
                            declarationPatternSyntax);
                    case DefaultConstraintSyntax defaultConstraintSyntax:
                        return this.PrintDefaultConstraintSyntax(
                            defaultConstraintSyntax);
                    case DefaultExpressionSyntax defaultExpressionSyntax:
                        return this.PrintDefaultExpressionSyntax(
                            defaultExpressionSyntax);
                    case DefaultSwitchLabelSyntax defaultSwitchLabelSyntax:
                        return this.PrintDefaultSwitchLabelSyntax(
                            defaultSwitchLabelSyntax);
                    case DelegateDeclarationSyntax delegateDeclarationSyntax:
                        return this.PrintDelegateDeclarationSyntax(
                            delegateDeclarationSyntax);
                    case DestructorDeclarationSyntax destructorDeclarationSyntax:
                        return this.PrintDestructorDeclarationSyntax(
                            destructorDeclarationSyntax);
                    case DiscardDesignationSyntax discardDesignationSyntax:
                        return this.PrintDiscardDesignationSyntax(
                            discardDesignationSyntax);
                    case DiscardPatternSyntax discardPatternSyntax:
                        return this.PrintDiscardPatternSyntax(
                            discardPatternSyntax);
                    case DoStatementSyntax doStatementSyntax:
                        return this.PrintDoStatementSyntax(doStatementSyntax);
                    case ElementAccessExpressionSyntax elementAccessExpressionSyntax:
                        return this.PrintElementAccessExpressionSyntax(
                            elementAccessExpressionSyntax);
                    case ElementBindingExpressionSyntax elementBindingExpressionSyntax:
                        return this.PrintElementBindingExpressionSyntax(
                            elementBindingExpressionSyntax);
                    case ElseClauseSyntax elseClauseSyntax:
                        return this.PrintElseClauseSyntax(elseClauseSyntax);
                    case EmptyStatementSyntax emptyStatementSyntax:
                        return this.PrintEmptyStatementSyntax(
                            emptyStatementSyntax);
                    case EnumDeclarationSyntax enumDeclarationSyntax:
                        return this.PrintEnumDeclarationSyntax(
                            enumDeclarationSyntax);
                    case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax:
                        return this.PrintEnumMemberDeclarationSyntax(
                            enumMemberDeclarationSyntax);
                    case EqualsValueClauseSyntax equalsValueClauseSyntax:
                        return this.PrintEqualsValueClauseSyntax(
                            equalsValueClauseSyntax);
                    case EventDeclarationSyntax eventDeclarationSyntax:
                        return this.PrintEventDeclarationSyntax(
                            eventDeclarationSyntax);
                    case EventFieldDeclarationSyntax eventFieldDeclarationSyntax:
                        return this.PrintEventFieldDeclarationSyntax(
                            eventFieldDeclarationSyntax);
                    case ExpressionStatementSyntax expressionStatementSyntax:
                        return this.PrintExpressionStatementSyntax(
                            expressionStatementSyntax);
                    case ExternAliasDirectiveSyntax externAliasDirectiveSyntax:
                        return this.PrintExternAliasDirectiveSyntax(
                            externAliasDirectiveSyntax);
                    case FieldDeclarationSyntax fieldDeclarationSyntax:
                        return this.PrintFieldDeclarationSyntax(
                            fieldDeclarationSyntax);
                    case FinallyClauseSyntax finallyClauseSyntax:
                        return this.PrintFinallyClauseSyntax(
                            finallyClauseSyntax);
                    case FixedStatementSyntax fixedStatementSyntax:
                        return this.PrintFixedStatementSyntax(
                            fixedStatementSyntax);
                    case ForEachStatementSyntax forEachStatementSyntax:
                        return this.PrintForEachStatementSyntax(
                            forEachStatementSyntax);
                    case ForEachVariableStatementSyntax forEachVariableStatementSyntax:
                        return this.PrintForEachVariableStatementSyntax(
                            forEachVariableStatementSyntax);
                    case ForStatementSyntax forStatementSyntax:
                        return this.PrintForStatementSyntax(forStatementSyntax);
                    case FromClauseSyntax fromClauseSyntax:
                        return this.PrintFromClauseSyntax(fromClauseSyntax);
                    case FunctionPointerTypeSyntax functionPointerTypeSyntax:
                        return this.PrintFunctionPointerTypeSyntax(
                            functionPointerTypeSyntax);
                    case GenericNameSyntax genericNameSyntax:
                        return this.PrintGenericNameSyntax(genericNameSyntax);
                    case GlobalStatementSyntax globalStatementSyntax:
                        return this.PrintGlobalStatementSyntax(
                            globalStatementSyntax);
                    case GotoStatementSyntax gotoStatementSyntax:
                        return this.PrintGotoStatementSyntax(
                            gotoStatementSyntax);
                    case GroupClauseSyntax groupClauseSyntax:
                        return this.PrintGroupClauseSyntax(groupClauseSyntax);
                    case IdentifierNameSyntax identifierNameSyntax:
                        return this.PrintIdentifierNameSyntax(
                            identifierNameSyntax);
                    case IfStatementSyntax ifStatementSyntax:
                        return this.PrintIfStatementSyntax(ifStatementSyntax);
                    case ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax:
                        return this.PrintImplicitArrayCreationExpressionSyntax(
                            implicitArrayCreationExpressionSyntax);
                    case ImplicitElementAccessSyntax implicitElementAccessSyntax:
                        return this.PrintImplicitElementAccessSyntax(
                            implicitElementAccessSyntax);
                    case ImplicitObjectCreationExpressionSyntax implicitObjectCreationExpressionSyntax:
                        return this.PrintImplicitObjectCreationExpressionSyntax(
                            implicitObjectCreationExpressionSyntax);
                    case ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax:
                        return this.PrintImplicitStackAllocArrayCreationExpressionSyntax(
                            implicitStackAllocArrayCreationExpressionSyntax);
                    case IncompleteMemberSyntax incompleteMemberSyntax:
                        return this.PrintIncompleteMemberSyntax(
                            incompleteMemberSyntax);
                    case IndexerDeclarationSyntax indexerDeclarationSyntax:
                        return this.PrintIndexerDeclarationSyntax(
                            indexerDeclarationSyntax);
                    case InitializerExpressionSyntax initializerExpressionSyntax:
                        return this.PrintInitializerExpressionSyntax(
                            initializerExpressionSyntax);
                    case InterfaceDeclarationSyntax interfaceDeclarationSyntax:
                        return this.PrintInterfaceDeclarationSyntax(
                            interfaceDeclarationSyntax);
                    case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax:
                        return this.PrintInterpolatedStringExpressionSyntax(
                            interpolatedStringExpressionSyntax);
                    case InterpolatedStringTextSyntax interpolatedStringTextSyntax:
                        return this.PrintInterpolatedStringTextSyntax(
                            interpolatedStringTextSyntax);
                    case InterpolationSyntax interpolationSyntax:
                        return this.PrintInterpolationSyntax(
                            interpolationSyntax);
                    case InvocationExpressionSyntax invocationExpressionSyntax:
                        return this.PrintInvocationExpressionSyntax(
                            invocationExpressionSyntax);
                    case IsPatternExpressionSyntax isPatternExpressionSyntax:
                        return this.PrintIsPatternExpressionSyntax(
                            isPatternExpressionSyntax);
                    case JoinClauseSyntax joinClauseSyntax:
                        return this.PrintJoinClauseSyntax(joinClauseSyntax);
                    case LabeledStatementSyntax labeledStatementSyntax:
                        return this.PrintLabeledStatementSyntax(
                            labeledStatementSyntax);
                    case LetClauseSyntax letClauseSyntax:
                        return this.PrintLetClauseSyntax(letClauseSyntax);
                    case LiteralExpressionSyntax literalExpressionSyntax:
                        return this.PrintLiteralExpressionSyntax(
                            literalExpressionSyntax);
                    case LocalDeclarationStatementSyntax localDeclarationStatementSyntax:
                        return this.PrintLocalDeclarationStatementSyntax(
                            localDeclarationStatementSyntax);
                    case LocalFunctionStatementSyntax localFunctionStatementSyntax:
                        return this.PrintLocalFunctionStatementSyntax(
                            localFunctionStatementSyntax);
                    case LockStatementSyntax lockStatementSyntax:
                        return this.PrintLockStatementSyntax(
                            lockStatementSyntax);
                    case MakeRefExpressionSyntax makeRefExpressionSyntax:
                        return this.PrintMakeRefExpressionSyntax(
                            makeRefExpressionSyntax);
                    case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
                        return this.PrintMemberAccessExpressionSyntax(
                            memberAccessExpressionSyntax);
                    case MemberBindingExpressionSyntax memberBindingExpressionSyntax:
                        return this.PrintMemberBindingExpressionSyntax(
                            memberBindingExpressionSyntax);
                    case MethodDeclarationSyntax methodDeclarationSyntax:
                        return this.PrintMethodDeclarationSyntax(
                            methodDeclarationSyntax);
                    case NameColonSyntax nameColonSyntax:
                        return this.PrintNameColonSyntax(nameColonSyntax);
                    case NameEqualsSyntax nameEqualsSyntax:
                        return this.PrintNameEqualsSyntax(nameEqualsSyntax);
                    case NamespaceDeclarationSyntax namespaceDeclarationSyntax:
                        return this.PrintNamespaceDeclarationSyntax(
                            namespaceDeclarationSyntax);
                    case NullableTypeSyntax nullableTypeSyntax:
                        return this.PrintNullableTypeSyntax(nullableTypeSyntax);
                    case ObjectCreationExpressionSyntax objectCreationExpressionSyntax:
                        return this.PrintObjectCreationExpressionSyntax(
                            objectCreationExpressionSyntax);
                    case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax:
                        return this.PrintOmittedArraySizeExpressionSyntax(
                            omittedArraySizeExpressionSyntax);
                    case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax:
                        return this.PrintOmittedTypeArgumentSyntax(
                            omittedTypeArgumentSyntax);
                    case OperatorDeclarationSyntax operatorDeclarationSyntax:
                        return this.PrintOperatorDeclarationSyntax(
                            operatorDeclarationSyntax);
                    case OrderByClauseSyntax orderByClauseSyntax:
                        return this.PrintOrderByClauseSyntax(
                            orderByClauseSyntax);
                    case ParameterListSyntax parameterListSyntax:
                        return this.PrintParameterListSyntax(
                            parameterListSyntax);
                    case ParameterSyntax parameterSyntax:
                        return this.PrintParameterSyntax(parameterSyntax);
                    case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax:
                        return this.PrintParenthesizedExpressionSyntax(
                            parenthesizedExpressionSyntax);
                    case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax:
                        return this.PrintParenthesizedLambdaExpressionSyntax(
                            parenthesizedLambdaExpressionSyntax);
                    case ParenthesizedPatternSyntax parenthesizedPatternSyntax:
                        return this.PrintParenthesizedPatternSyntax(
                            parenthesizedPatternSyntax);
                    case ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax:
                        return this.PrintParenthesizedVariableDesignationSyntax(
                            parenthesizedVariableDesignationSyntax);
                    case PointerTypeSyntax pointerTypeSyntax:
                        return this.PrintPointerTypeSyntax(pointerTypeSyntax);
                    case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax:
                        return this.PrintPostfixUnaryExpressionSyntax(
                            postfixUnaryExpressionSyntax);
                    case PredefinedTypeSyntax predefinedTypeSyntax:
                        return this.PrintPredefinedTypeSyntax(
                            predefinedTypeSyntax);
                    case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax:
                        return this.PrintPrefixUnaryExpressionSyntax(
                            prefixUnaryExpressionSyntax);
                    case PropertyDeclarationSyntax propertyDeclarationSyntax:
                        return this.PrintPropertyDeclarationSyntax(
                            propertyDeclarationSyntax);
                    case QualifiedNameSyntax qualifiedNameSyntax:
                        return this.PrintQualifiedNameSyntax(
                            qualifiedNameSyntax);
                    case QueryBodySyntax queryBodySyntax:
                        return this.PrintQueryBodySyntax(queryBodySyntax);
                    case QueryContinuationSyntax queryContinuationSyntax:
                        return this.PrintQueryContinuationSyntax(
                            queryContinuationSyntax);
                    case QueryExpressionSyntax queryExpressionSyntax:
                        return this.PrintQueryExpressionSyntax(
                            queryExpressionSyntax);
                    case RangeExpressionSyntax rangeExpressionSyntax:
                        return this.PrintRangeExpressionSyntax(
                            rangeExpressionSyntax);
                    case RecordDeclarationSyntax recordDeclarationSyntax:
                        return this.PrintRecordDeclarationSyntax(
                            recordDeclarationSyntax);
                    case RecursivePatternSyntax recursivePatternSyntax:
                        return this.PrintRecursivePatternSyntax(
                            recursivePatternSyntax);
                    case RefExpressionSyntax refExpressionSyntax:
                        return this.PrintRefExpressionSyntax(
                            refExpressionSyntax);
                    case RefTypeExpressionSyntax refTypeExpressionSyntax:
                        return this.PrintRefTypeExpressionSyntax(
                            refTypeExpressionSyntax);
                    case RefTypeSyntax refTypeSyntax:
                        return this.PrintRefTypeSyntax(refTypeSyntax);
                    case RefValueExpressionSyntax refValueExpressionSyntax:
                        return this.PrintRefValueExpressionSyntax(
                            refValueExpressionSyntax);
                    case RelationalPatternSyntax relationalPatternSyntax:
                        return this.PrintRelationalPatternSyntax(
                            relationalPatternSyntax);
                    case ReturnStatementSyntax returnStatementSyntax:
                        return this.PrintReturnStatementSyntax(
                            returnStatementSyntax);
                    case SelectClauseSyntax selectClauseSyntax:
                        return this.PrintSelectClauseSyntax(selectClauseSyntax);
                    case SimpleBaseTypeSyntax simpleBaseTypeSyntax:
                        return this.PrintSimpleBaseTypeSyntax(
                            simpleBaseTypeSyntax);
                    case SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax:
                        return this.PrintSimpleLambdaExpressionSyntax(
                            simpleLambdaExpressionSyntax);
                    case SingleVariableDesignationSyntax singleVariableDesignationSyntax:
                        return this.PrintSingleVariableDesignationSyntax(
                            singleVariableDesignationSyntax);
                    case SizeOfExpressionSyntax sizeOfExpressionSyntax:
                        return this.PrintSizeOfExpressionSyntax(
                            sizeOfExpressionSyntax);
                    case StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax:
                        return this.PrintStackAllocArrayCreationExpressionSyntax(
                            stackAllocArrayCreationExpressionSyntax);
                    case StructDeclarationSyntax structDeclarationSyntax:
                        return this.PrintStructDeclarationSyntax(
                            structDeclarationSyntax);
                    case SwitchExpressionSyntax switchExpressionSyntax:
                        return this.PrintSwitchExpressionSyntax(
                            switchExpressionSyntax);
                    case SwitchSectionSyntax switchSectionSyntax:
                        return this.PrintSwitchSectionSyntax(
                            switchSectionSyntax);
                    case SwitchStatementSyntax switchStatementSyntax:
                        return this.PrintSwitchStatementSyntax(
                            switchStatementSyntax);
                    case ThisExpressionSyntax thisExpressionSyntax:
                        return this.PrintThisExpressionSyntax(
                            thisExpressionSyntax);
                    case ThrowExpressionSyntax throwExpressionSyntax:
                        return this.PrintThrowExpressionSyntax(
                            throwExpressionSyntax);
                    case ThrowStatementSyntax throwStatementSyntax:
                        return this.PrintThrowStatementSyntax(
                            throwStatementSyntax);
                    case TryStatementSyntax tryStatementSyntax:
                        return this.PrintTryStatementSyntax(tryStatementSyntax);
                    case TupleElementSyntax tupleElementSyntax:
                        return this.PrintTupleElementSyntax(tupleElementSyntax);
                    case TupleExpressionSyntax tupleExpressionSyntax:
                        return this.PrintTupleExpressionSyntax(
                            tupleExpressionSyntax);
                    case TupleTypeSyntax tupleTypeSyntax:
                        return this.PrintTupleTypeSyntax(tupleTypeSyntax);
                    case TypeArgumentListSyntax typeArgumentListSyntax:
                        return this.PrintTypeArgumentListSyntax(
                            typeArgumentListSyntax);
                    case TypeConstraintSyntax typeConstraintSyntax:
                        return this.PrintTypeConstraintSyntax(
                            typeConstraintSyntax);
                    case TypeOfExpressionSyntax typeOfExpressionSyntax:
                        return this.PrintTypeOfExpressionSyntax(
                            typeOfExpressionSyntax);
                    case TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax:
                        return this.PrintTypeParameterConstraintClauseSyntax(
                            typeParameterConstraintClauseSyntax);
                    case TypeParameterListSyntax typeParameterListSyntax:
                        return this.PrintTypeParameterListSyntax(
                            typeParameterListSyntax);
                    case TypeParameterSyntax typeParameterSyntax:
                        return this.PrintTypeParameterSyntax(
                            typeParameterSyntax);
                    case TypePatternSyntax typePatternSyntax:
                        return this.PrintTypePatternSyntax(typePatternSyntax);
                    case UnaryPatternSyntax unaryPatternSyntax:
                        return this.PrintUnaryPatternSyntax(unaryPatternSyntax);
                    case UnsafeStatementSyntax unsafeStatementSyntax:
                        return this.PrintUnsafeStatementSyntax(
                            unsafeStatementSyntax);
                    case UsingDirectiveSyntax usingDirectiveSyntax:
                        return this.PrintUsingDirectiveSyntax(
                            usingDirectiveSyntax);
                    case UsingStatementSyntax usingStatementSyntax:
                        return this.PrintUsingStatementSyntax(
                            usingStatementSyntax);
                    case VariableDeclarationSyntax variableDeclarationSyntax:
                        return this.PrintVariableDeclarationSyntax(
                            variableDeclarationSyntax);
                    case VariableDeclaratorSyntax variableDeclaratorSyntax:
                        return this.PrintVariableDeclaratorSyntax(
                            variableDeclaratorSyntax);
                    case VarPatternSyntax varPatternSyntax:
                        return this.PrintVarPatternSyntax(varPatternSyntax);
                    case WhenClauseSyntax whenClauseSyntax:
                        return this.PrintWhenClauseSyntax(whenClauseSyntax);
                    case WhereClauseSyntax whereClauseSyntax:
                        return this.PrintWhereClauseSyntax(whereClauseSyntax);
                    case WhileStatementSyntax whileStatementSyntax:
                        return this.PrintWhileStatementSyntax(
                            whileStatementSyntax);
                    case WithExpressionSyntax withExpressionSyntax:
                        return this.PrintWithExpressionSyntax(
                            withExpressionSyntax);
                    case YieldStatementSyntax yieldStatementSyntax:
                        return this.PrintYieldStatementSyntax(
                            yieldStatementSyntax);
                    default:
                        throw new Exception(
                            "Can't handle " + syntaxNode.GetType().Name);
                }
            }

            finally
            {
                depth--;
            }
        }
    }
}
