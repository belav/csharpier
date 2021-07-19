using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class Node
    {
        [ThreadStatic]
        private static int depth;

        public static Doc Print(SyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                return Doc.Null;
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
                    case AliasQualifiedNameSyntax aliasQualifiedNameSyntax:
                        return AliasQualifiedName.Print(aliasQualifiedNameSyntax);
                    case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax:
                        return AnonymousMethodExpression.Print(anonymousMethodExpressionSyntax);
                    case AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax:
                        return AnonymousObjectCreationExpression.Print(
                            anonymousObjectCreationExpressionSyntax
                        );
                    case AnonymousObjectMemberDeclaratorSyntax anonymousObjectMemberDeclaratorSyntax:
                        return AnonymousObjectMemberDeclarator.Print(
                            anonymousObjectMemberDeclaratorSyntax
                        );
                    case ArgumentListSyntax argumentListSyntax:
                        return ArgumentList.Print(argumentListSyntax);
                    case ArgumentSyntax argumentSyntax:
                        return Argument.Print(argumentSyntax);
                    case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax:
                        return ArrayCreationExpression.Print(arrayCreationExpressionSyntax);
                    case ArrayRankSpecifierSyntax arrayRankSpecifierSyntax:
                        return ArrayRankSpecifier.Print(arrayRankSpecifierSyntax);
                    case ArrayTypeSyntax arrayTypeSyntax:
                        return ArrayType.Print(arrayTypeSyntax);
                    case ArrowExpressionClauseSyntax arrowExpressionClauseSyntax:
                        return ArrowExpressionClause.Print(arrowExpressionClauseSyntax);
                    case AssignmentExpressionSyntax assignmentExpressionSyntax:
                        return AssignmentExpression.Print(assignmentExpressionSyntax);
                    case AttributeListSyntax attributeListSyntax:
                        return AttributeList.Print(attributeListSyntax);
                    case AwaitExpressionSyntax awaitExpressionSyntax:
                        return AwaitExpression.Print(awaitExpressionSyntax);
                    case BaseExpressionSyntax baseExpressionSyntax:
                        return BaseExpression.Print(baseExpressionSyntax);
                    case BaseFieldDeclarationSyntax baseFieldDeclarationSyntax:
                        return BaseFieldDeclaration.Print(baseFieldDeclarationSyntax);
                    case BaseListSyntax baseListSyntax:
                        return BaseList.Print(baseListSyntax);
                    case BaseMethodDeclarationSyntax baseMethodDeclarationSyntax:
                        return BaseMethodDeclaration.Print(baseMethodDeclarationSyntax);
                    case BasePropertyDeclarationSyntax basePropertyDeclarationSyntax:
                        return BasePropertyDeclaration.Print(basePropertyDeclarationSyntax);
                    case BaseTypeDeclarationSyntax baseTypeDeclarationSyntax:
                        return BaseTypeDeclaration.Print(baseTypeDeclarationSyntax);
                    case BinaryExpressionSyntax binaryExpressionSyntax:
                        return BinaryExpression.Print(binaryExpressionSyntax);
                    case BinaryPatternSyntax binaryPatternSyntax:
                        return BinaryPattern.Print(binaryPatternSyntax);
                    case BlockSyntax blockSyntax:
                        return Block.Print(blockSyntax);
                    case BracketedArgumentListSyntax bracketedArgumentListSyntax:
                        return BracketedArgumentList.Print(bracketedArgumentListSyntax);
                    case BracketedParameterListSyntax bracketedParameterListSyntax:
                        return BracketedParameterList.Print(bracketedParameterListSyntax);
                    case BreakStatementSyntax breakStatementSyntax:
                        return BreakStatement.Print(breakStatementSyntax);
                    case CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax:
                        return CasePatternSwitchLabel.Print(casePatternSwitchLabelSyntax);
                    case CaseSwitchLabelSyntax caseSwitchLabelSyntax:
                        return CaseSwitchLabel.Print(caseSwitchLabelSyntax);
                    case CastExpressionSyntax castExpressionSyntax:
                        return CastExpression.Print(castExpressionSyntax);
                    case CatchClauseSyntax catchClauseSyntax:
                        return CatchClause.Print(catchClauseSyntax);
                    case CheckedExpressionSyntax checkedExpressionSyntax:
                        return CheckedExpression.Print(checkedExpressionSyntax);
                    case CheckedStatementSyntax checkedStatementSyntax:
                        return CheckedStatement.Print(checkedStatementSyntax);
                    case ClassOrStructConstraintSyntax classOrStructConstraintSyntax:
                        return ClassOrStructConstraint.Print(classOrStructConstraintSyntax);
                    case CommonForEachStatementSyntax commonForEachStatementSyntax:
                        return CommonForEachStatement.Print(commonForEachStatementSyntax);
                    case CompilationUnitSyntax compilationUnitSyntax:
                        return CompilationUnit.Print(compilationUnitSyntax);
                    case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax:
                        return ConditionalAccessExpression.Print(conditionalAccessExpressionSyntax);
                    case ConditionalExpressionSyntax conditionalExpressionSyntax:
                        return ConditionalExpression.Print(conditionalExpressionSyntax);
                    case ConstantPatternSyntax constantPatternSyntax:
                        return ConstantPattern.Print(constantPatternSyntax);
                    case ConstructorConstraintSyntax constructorConstraintSyntax:
                        return ConstructorConstraint.Print(constructorConstraintSyntax);
                    case ConstructorInitializerSyntax constructorInitializerSyntax:
                        return ConstructorInitializer.Print(constructorInitializerSyntax);
                    case ContinueStatementSyntax continueStatementSyntax:
                        return ContinueStatement.Print(continueStatementSyntax);
                    case DeclarationExpressionSyntax declarationExpressionSyntax:
                        return DeclarationExpression.Print(declarationExpressionSyntax);
                    case DeclarationPatternSyntax declarationPatternSyntax:
                        return DeclarationPattern.Print(declarationPatternSyntax);
                    case DefaultConstraintSyntax defaultConstraintSyntax:
                        return DefaultConstraint.Print(defaultConstraintSyntax);
                    case DefaultExpressionSyntax defaultExpressionSyntax:
                        return DefaultExpression.Print(defaultExpressionSyntax);
                    case DefaultSwitchLabelSyntax defaultSwitchLabelSyntax:
                        return DefaultSwitchLabel.Print(defaultSwitchLabelSyntax);
                    case DelegateDeclarationSyntax delegateDeclarationSyntax:
                        return DelegateDeclaration.Print(delegateDeclarationSyntax);
                    case DiscardDesignationSyntax discardDesignationSyntax:
                        return DiscardDesignation.Print(discardDesignationSyntax);
                    case DiscardPatternSyntax discardPatternSyntax:
                        return DiscardPattern.Print(discardPatternSyntax);
                    case DoStatementSyntax doStatementSyntax:
                        return DoStatement.Print(doStatementSyntax);
                    case ElementAccessExpressionSyntax elementAccessExpressionSyntax:
                        return ElementAccessExpression.Print(elementAccessExpressionSyntax);
                    case ElementBindingExpressionSyntax elementBindingExpressionSyntax:
                        return ElementBindingExpression.Print(elementBindingExpressionSyntax);
                    case ElseClauseSyntax elseClauseSyntax:
                        return ElseClause.Print(elseClauseSyntax);
                    case EmptyStatementSyntax emptyStatementSyntax:
                        return EmptyStatement.Print(emptyStatementSyntax);
                    case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax:
                        return EnumMemberDeclaration.Print(enumMemberDeclarationSyntax);
                    case EqualsValueClauseSyntax equalsValueClauseSyntax:
                        return EqualsValueClause.Print(equalsValueClauseSyntax);
                    case ExpressionStatementSyntax expressionStatementSyntax:
                        return ExpressionStatement.Print(expressionStatementSyntax);
                    case ExternAliasDirectiveSyntax externAliasDirectiveSyntax:
                        return ExternAliasDirective.Print(externAliasDirectiveSyntax);
                    case FinallyClauseSyntax finallyClauseSyntax:
                        return FinallyClause.Print(finallyClauseSyntax);
                    case FixedStatementSyntax fixedStatementSyntax:
                        return FixedStatement.Print(fixedStatementSyntax);
                    case ForStatementSyntax forStatementSyntax:
                        return ForStatement.Print(forStatementSyntax);
                    case FromClauseSyntax fromClauseSyntax:
                        return FromClause.Print(fromClauseSyntax);
                    case FunctionPointerTypeSyntax functionPointerTypeSyntax:
                        return FunctionPointerType.Print(functionPointerTypeSyntax);
                    case GenericNameSyntax genericNameSyntax:
                        return GenericName.Print(genericNameSyntax);
                    case GlobalStatementSyntax globalStatementSyntax:
                        return GlobalStatement.Print(globalStatementSyntax);
                    case GotoStatementSyntax gotoStatementSyntax:
                        return GotoStatement.Print(gotoStatementSyntax);
                    case GroupClauseSyntax groupClauseSyntax:
                        return GroupClause.Print(groupClauseSyntax);
                    case IdentifierNameSyntax identifierNameSyntax:
                        return IdentifierName.Print(identifierNameSyntax);
                    case IfStatementSyntax ifStatementSyntax:
                        return IfStatement.Print(ifStatementSyntax);
                    case ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax:
                        return ImplicitArrayCreationExpression.Print(
                            implicitArrayCreationExpressionSyntax
                        );
                    case ImplicitElementAccessSyntax implicitElementAccessSyntax:
                        return ImplicitElementAccess.Print(implicitElementAccessSyntax);
                    case ImplicitObjectCreationExpressionSyntax implicitObjectCreationExpressionSyntax:
                        return ImplicitObjectCreationExpression.Print(
                            implicitObjectCreationExpressionSyntax
                        );
                    case ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax:
                        return ImplicitStackAllocArrayCreationExpression.Print(
                            implicitStackAllocArrayCreationExpressionSyntax
                        );
                    case IncompleteMemberSyntax incompleteMemberSyntax:
                        return IncompleteMember.Print(incompleteMemberSyntax);
                    case InitializerExpressionSyntax initializerExpressionSyntax:
                        return InitializerExpression.Print(initializerExpressionSyntax);
                    case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax:
                        return InterpolatedStringExpression.Print(
                            interpolatedStringExpressionSyntax
                        );
                    case InterpolatedStringTextSyntax interpolatedStringTextSyntax:
                        return InterpolatedStringText.Print(interpolatedStringTextSyntax);
                    case InterpolationSyntax interpolationSyntax:
                        return Interpolation.Print(interpolationSyntax);
                    case InvocationExpressionSyntax invocationExpressionSyntax:
                        return InvocationExpression.Print(invocationExpressionSyntax);
                    case IsPatternExpressionSyntax isPatternExpressionSyntax:
                        return IsPatternExpression.Print(isPatternExpressionSyntax);
                    case JoinClauseSyntax joinClauseSyntax:
                        return JoinClause.Print(joinClauseSyntax);
                    case LabeledStatementSyntax labeledStatementSyntax:
                        return LabeledStatement.Print(labeledStatementSyntax);
                    case LetClauseSyntax letClauseSyntax:
                        return LetClause.Print(letClauseSyntax);
                    case LiteralExpressionSyntax literalExpressionSyntax:
                        return LiteralExpression.Print(literalExpressionSyntax);
                    case LocalDeclarationStatementSyntax localDeclarationStatementSyntax:
                        return LocalDeclarationStatement.Print(localDeclarationStatementSyntax);
                    case LocalFunctionStatementSyntax localFunctionStatementSyntax:
                        return LocalFunctionStatement.Print(localFunctionStatementSyntax);
                    case LockStatementSyntax lockStatementSyntax:
                        return LockStatement.Print(lockStatementSyntax);
                    case MakeRefExpressionSyntax makeRefExpressionSyntax:
                        return MakeRefExpression.Print(makeRefExpressionSyntax);
                    case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
                        return MemberAccessExpression.Print(memberAccessExpressionSyntax);
                    case MemberBindingExpressionSyntax memberBindingExpressionSyntax:
                        return MemberBindingExpression.Print(memberBindingExpressionSyntax);
                    case NameColonSyntax nameColonSyntax:
                        return NameColon.Print(nameColonSyntax);
                    case NameEqualsSyntax nameEqualsSyntax:
                        return NameEquals.Print(nameEqualsSyntax);
                    case NamespaceDeclarationSyntax namespaceDeclarationSyntax:
                        return NamespaceDeclaration.Print(namespaceDeclarationSyntax);
                    case NullableTypeSyntax nullableTypeSyntax:
                        return NullableType.Print(nullableTypeSyntax);
                    case ObjectCreationExpressionSyntax objectCreationExpressionSyntax:
                        return ObjectCreationExpression.Print(objectCreationExpressionSyntax);
                    case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax:
                        return OmittedArraySizeExpression.Print(omittedArraySizeExpressionSyntax);
                    case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax:
                        return OmittedTypeArgument.Print(omittedTypeArgumentSyntax);
                    case OrderByClauseSyntax orderByClauseSyntax:
                        return OrderByClause.Print(orderByClauseSyntax);
                    case ParameterListSyntax parameterListSyntax:
                        return ParameterList.Print(parameterListSyntax);
                    case ParameterSyntax parameterSyntax:
                        return Parameter.Print(parameterSyntax);
                    case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax:
                        return ParenthesizedExpression.Print(parenthesizedExpressionSyntax);
                    case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax:
                        return ParenthesizedLambdaExpression.Print(
                            parenthesizedLambdaExpressionSyntax
                        );
                    case ParenthesizedPatternSyntax parenthesizedPatternSyntax:
                        return ParenthesizedPattern.Print(parenthesizedPatternSyntax);
                    case ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax:
                        return ParenthesizedVariableDesignation.Print(
                            parenthesizedVariableDesignationSyntax
                        );
                    case PointerTypeSyntax pointerTypeSyntax:
                        return PointerType.Print(pointerTypeSyntax);
                    case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax:
                        return PostfixUnaryExpression.Print(postfixUnaryExpressionSyntax);
                    case PredefinedTypeSyntax predefinedTypeSyntax:
                        return PredefinedType.Print(predefinedTypeSyntax);
                    case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax:
                        return PrefixUnaryExpression.Print(prefixUnaryExpressionSyntax);
                    case PrimaryConstructorBaseTypeSyntax primaryConstructorBaseTypeSyntax:
                        return PrimaryConstructorBaseType.Print(primaryConstructorBaseTypeSyntax);
                    case QualifiedNameSyntax qualifiedNameSyntax:
                        return QualifiedName.Print(qualifiedNameSyntax);
                    case QueryBodySyntax queryBodySyntax:
                        return QueryBody.Print(queryBodySyntax);
                    case QueryContinuationSyntax queryContinuationSyntax:
                        return QueryContinuation.Print(queryContinuationSyntax);
                    case QueryExpressionSyntax queryExpressionSyntax:
                        return QueryExpression.Print(queryExpressionSyntax);
                    case RangeExpressionSyntax rangeExpressionSyntax:
                        return RangeExpression.Print(rangeExpressionSyntax);
                    case RecursivePatternSyntax recursivePatternSyntax:
                        return RecursivePattern.Print(recursivePatternSyntax);
                    case RefExpressionSyntax refExpressionSyntax:
                        return RefExpression.Print(refExpressionSyntax);
                    case RefTypeExpressionSyntax refTypeExpressionSyntax:
                        return RefTypeExpression.Print(refTypeExpressionSyntax);
                    case RefTypeSyntax refTypeSyntax:
                        return RefType.Print(refTypeSyntax);
                    case RefValueExpressionSyntax refValueExpressionSyntax:
                        return RefValueExpression.Print(refValueExpressionSyntax);
                    case RelationalPatternSyntax relationalPatternSyntax:
                        return RelationalPattern.Print(relationalPatternSyntax);
                    case ReturnStatementSyntax returnStatementSyntax:
                        return ReturnStatement.Print(returnStatementSyntax);
                    case SelectClauseSyntax selectClauseSyntax:
                        return SelectClause.Print(selectClauseSyntax);
                    case SimpleBaseTypeSyntax simpleBaseTypeSyntax:
                        return SimpleBaseType.Print(simpleBaseTypeSyntax);
                    case SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax:
                        return SimpleLambdaExpression.Print(simpleLambdaExpressionSyntax);
                    case SingleVariableDesignationSyntax singleVariableDesignationSyntax:
                        return SingleVariableDesignation.Print(singleVariableDesignationSyntax);
                    case SizeOfExpressionSyntax sizeOfExpressionSyntax:
                        return SizeOfExpression.Print(sizeOfExpressionSyntax);
                    case StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax:
                        return StackAllocArrayCreationExpression.Print(
                            stackAllocArrayCreationExpressionSyntax
                        );
                    case SwitchExpressionSyntax switchExpressionSyntax:
                        return SwitchExpression.Print(switchExpressionSyntax);
                    case SwitchSectionSyntax switchSectionSyntax:
                        return SwitchSection.Print(switchSectionSyntax);
                    case SwitchStatementSyntax switchStatementSyntax:
                        return SwitchStatement.Print(switchStatementSyntax);
                    case ThisExpressionSyntax thisExpressionSyntax:
                        return ThisExpression.Print(thisExpressionSyntax);
                    case ThrowExpressionSyntax throwExpressionSyntax:
                        return ThrowExpression.Print(throwExpressionSyntax);
                    case ThrowStatementSyntax throwStatementSyntax:
                        return ThrowStatement.Print(throwStatementSyntax);
                    case TryStatementSyntax tryStatementSyntax:
                        return TryStatement.Print(tryStatementSyntax);
                    case TupleElementSyntax tupleElementSyntax:
                        return TupleElement.Print(tupleElementSyntax);
                    case TupleExpressionSyntax tupleExpressionSyntax:
                        return TupleExpression.Print(tupleExpressionSyntax);
                    case TupleTypeSyntax tupleTypeSyntax:
                        return TupleType.Print(tupleTypeSyntax);
                    case TypeArgumentListSyntax typeArgumentListSyntax:
                        return TypeArgumentList.Print(typeArgumentListSyntax);
                    case TypeConstraintSyntax typeConstraintSyntax:
                        return TypeConstraint.Print(typeConstraintSyntax);
                    case TypeOfExpressionSyntax typeOfExpressionSyntax:
                        return TypeOfExpression.Print(typeOfExpressionSyntax);
                    case TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax:
                        return TypeParameterConstraintClause.Print(
                            typeParameterConstraintClauseSyntax
                        );
                    case TypeParameterListSyntax typeParameterListSyntax:
                        return TypeParameterList.Print(typeParameterListSyntax);
                    case TypeParameterSyntax typeParameterSyntax:
                        return TypeParameter.Print(typeParameterSyntax);
                    case TypePatternSyntax typePatternSyntax:
                        return TypePattern.Print(typePatternSyntax);
                    case UnaryPatternSyntax unaryPatternSyntax:
                        return UnaryPattern.Print(unaryPatternSyntax);
                    case UnsafeStatementSyntax unsafeStatementSyntax:
                        return UnsafeStatement.Print(unsafeStatementSyntax);
                    case UsingDirectiveSyntax usingDirectiveSyntax:
                        return UsingDirective.Print(usingDirectiveSyntax);
                    case UsingStatementSyntax usingStatementSyntax:
                        return UsingStatement.Print(usingStatementSyntax);
                    case VariableDeclarationSyntax variableDeclarationSyntax:
                        return VariableDeclaration.Print(variableDeclarationSyntax);
                    case VariableDeclaratorSyntax variableDeclaratorSyntax:
                        return VariableDeclarator.Print(variableDeclaratorSyntax);
                    case VarPatternSyntax varPatternSyntax:
                        return VarPattern.Print(varPatternSyntax);
                    case WhenClauseSyntax whenClauseSyntax:
                        return WhenClause.Print(whenClauseSyntax);
                    case WhereClauseSyntax whereClauseSyntax:
                        return WhereClause.Print(whereClauseSyntax);
                    case WhileStatementSyntax whileStatementSyntax:
                        return WhileStatement.Print(whileStatementSyntax);
                    case WithExpressionSyntax withExpressionSyntax:
                        return WithExpression.Print(withExpressionSyntax);
                    case YieldStatementSyntax yieldStatementSyntax:
                        return YieldStatement.Print(yieldStatementSyntax);
                    default:
                        throw new Exception("Can't handle " + syntaxNode.GetType().Name);
                }
            }

            finally
            {
                depth--;
            }
        }
    }
}
