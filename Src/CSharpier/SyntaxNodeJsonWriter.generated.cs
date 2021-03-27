using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class SyntaxNodeJsonWriter
    {
        public static void WriteSyntaxNode(
            StringBuilder builder,
            SyntaxNode syntaxNode)
        {
            if (syntaxNode is IdentifierNameSyntax)
                WriteIdentifierNameSyntax(
                    builder,
                    syntaxNode as IdentifierNameSyntax
                );
            if (syntaxNode is QualifiedNameSyntax)
                WriteQualifiedNameSyntax(
                    builder,
                    syntaxNode as QualifiedNameSyntax
                );
            if (syntaxNode is GenericNameSyntax)
                WriteGenericNameSyntax(
                    builder,
                    syntaxNode as GenericNameSyntax
                );
            if (syntaxNode is TypeArgumentListSyntax)
                WriteTypeArgumentListSyntax(
                    builder,
                    syntaxNode as TypeArgumentListSyntax
                );
            if (syntaxNode is AliasQualifiedNameSyntax)
                WriteAliasQualifiedNameSyntax(
                    builder,
                    syntaxNode as AliasQualifiedNameSyntax
                );
            if (syntaxNode is PredefinedTypeSyntax)
                WritePredefinedTypeSyntax(
                    builder,
                    syntaxNode as PredefinedTypeSyntax
                );
            if (syntaxNode is ArrayTypeSyntax)
                WriteArrayTypeSyntax(builder, syntaxNode as ArrayTypeSyntax);
            if (syntaxNode is ArrayRankSpecifierSyntax)
                WriteArrayRankSpecifierSyntax(
                    builder,
                    syntaxNode as ArrayRankSpecifierSyntax
                );
            if (syntaxNode is PointerTypeSyntax)
                WritePointerTypeSyntax(
                    builder,
                    syntaxNode as PointerTypeSyntax
                );
            if (syntaxNode is FunctionPointerTypeSyntax)
                WriteFunctionPointerTypeSyntax(
                    builder,
                    syntaxNode as FunctionPointerTypeSyntax
                );
            if (syntaxNode is FunctionPointerParameterListSyntax)
                WriteFunctionPointerParameterListSyntax(
                    builder,
                    syntaxNode as FunctionPointerParameterListSyntax
                );
            if (syntaxNode is FunctionPointerCallingConventionSyntax)
                WriteFunctionPointerCallingConventionSyntax(
                    builder,
                    syntaxNode as FunctionPointerCallingConventionSyntax
                );
            if (
                syntaxNode is FunctionPointerUnmanagedCallingConventionListSyntax
            )
                WriteFunctionPointerUnmanagedCallingConventionListSyntax(
                    builder,
                    syntaxNode as FunctionPointerUnmanagedCallingConventionListSyntax
                );
            if (syntaxNode is FunctionPointerUnmanagedCallingConventionSyntax)
                WriteFunctionPointerUnmanagedCallingConventionSyntax(
                    builder,
                    syntaxNode as FunctionPointerUnmanagedCallingConventionSyntax
                );
            if (syntaxNode is NullableTypeSyntax)
                WriteNullableTypeSyntax(
                    builder,
                    syntaxNode as NullableTypeSyntax
                );
            if (syntaxNode is TupleTypeSyntax)
                WriteTupleTypeSyntax(builder, syntaxNode as TupleTypeSyntax);
            if (syntaxNode is TupleElementSyntax)
                WriteTupleElementSyntax(
                    builder,
                    syntaxNode as TupleElementSyntax
                );
            if (syntaxNode is OmittedTypeArgumentSyntax)
                WriteOmittedTypeArgumentSyntax(
                    builder,
                    syntaxNode as OmittedTypeArgumentSyntax
                );
            if (syntaxNode is RefTypeSyntax)
                WriteRefTypeSyntax(builder, syntaxNode as RefTypeSyntax);
            if (syntaxNode is ParenthesizedExpressionSyntax)
                WriteParenthesizedExpressionSyntax(
                    builder,
                    syntaxNode as ParenthesizedExpressionSyntax
                );
            if (syntaxNode is TupleExpressionSyntax)
                WriteTupleExpressionSyntax(
                    builder,
                    syntaxNode as TupleExpressionSyntax
                );
            if (syntaxNode is PrefixUnaryExpressionSyntax)
                WritePrefixUnaryExpressionSyntax(
                    builder,
                    syntaxNode as PrefixUnaryExpressionSyntax
                );
            if (syntaxNode is AwaitExpressionSyntax)
                WriteAwaitExpressionSyntax(
                    builder,
                    syntaxNode as AwaitExpressionSyntax
                );
            if (syntaxNode is PostfixUnaryExpressionSyntax)
                WritePostfixUnaryExpressionSyntax(
                    builder,
                    syntaxNode as PostfixUnaryExpressionSyntax
                );
            if (syntaxNode is MemberAccessExpressionSyntax)
                WriteMemberAccessExpressionSyntax(
                    builder,
                    syntaxNode as MemberAccessExpressionSyntax
                );
            if (syntaxNode is ConditionalAccessExpressionSyntax)
                WriteConditionalAccessExpressionSyntax(
                    builder,
                    syntaxNode as ConditionalAccessExpressionSyntax
                );
            if (syntaxNode is MemberBindingExpressionSyntax)
                WriteMemberBindingExpressionSyntax(
                    builder,
                    syntaxNode as MemberBindingExpressionSyntax
                );
            if (syntaxNode is ElementBindingExpressionSyntax)
                WriteElementBindingExpressionSyntax(
                    builder,
                    syntaxNode as ElementBindingExpressionSyntax
                );
            if (syntaxNode is RangeExpressionSyntax)
                WriteRangeExpressionSyntax(
                    builder,
                    syntaxNode as RangeExpressionSyntax
                );
            if (syntaxNode is ImplicitElementAccessSyntax)
                WriteImplicitElementAccessSyntax(
                    builder,
                    syntaxNode as ImplicitElementAccessSyntax
                );
            if (syntaxNode is BinaryExpressionSyntax)
                WriteBinaryExpressionSyntax(
                    builder,
                    syntaxNode as BinaryExpressionSyntax
                );
            if (syntaxNode is AssignmentExpressionSyntax)
                WriteAssignmentExpressionSyntax(
                    builder,
                    syntaxNode as AssignmentExpressionSyntax
                );
            if (syntaxNode is ConditionalExpressionSyntax)
                WriteConditionalExpressionSyntax(
                    builder,
                    syntaxNode as ConditionalExpressionSyntax
                );
            if (syntaxNode is ThisExpressionSyntax)
                WriteThisExpressionSyntax(
                    builder,
                    syntaxNode as ThisExpressionSyntax
                );
            if (syntaxNode is BaseExpressionSyntax)
                WriteBaseExpressionSyntax(
                    builder,
                    syntaxNode as BaseExpressionSyntax
                );
            if (syntaxNode is LiteralExpressionSyntax)
                WriteLiteralExpressionSyntax(
                    builder,
                    syntaxNode as LiteralExpressionSyntax
                );
            if (syntaxNode is MakeRefExpressionSyntax)
                WriteMakeRefExpressionSyntax(
                    builder,
                    syntaxNode as MakeRefExpressionSyntax
                );
            if (syntaxNode is RefTypeExpressionSyntax)
                WriteRefTypeExpressionSyntax(
                    builder,
                    syntaxNode as RefTypeExpressionSyntax
                );
            if (syntaxNode is RefValueExpressionSyntax)
                WriteRefValueExpressionSyntax(
                    builder,
                    syntaxNode as RefValueExpressionSyntax
                );
            if (syntaxNode is CheckedExpressionSyntax)
                WriteCheckedExpressionSyntax(
                    builder,
                    syntaxNode as CheckedExpressionSyntax
                );
            if (syntaxNode is DefaultExpressionSyntax)
                WriteDefaultExpressionSyntax(
                    builder,
                    syntaxNode as DefaultExpressionSyntax
                );
            if (syntaxNode is TypeOfExpressionSyntax)
                WriteTypeOfExpressionSyntax(
                    builder,
                    syntaxNode as TypeOfExpressionSyntax
                );
            if (syntaxNode is SizeOfExpressionSyntax)
                WriteSizeOfExpressionSyntax(
                    builder,
                    syntaxNode as SizeOfExpressionSyntax
                );
            if (syntaxNode is InvocationExpressionSyntax)
                WriteInvocationExpressionSyntax(
                    builder,
                    syntaxNode as InvocationExpressionSyntax
                );
            if (syntaxNode is ElementAccessExpressionSyntax)
                WriteElementAccessExpressionSyntax(
                    builder,
                    syntaxNode as ElementAccessExpressionSyntax
                );
            if (syntaxNode is ArgumentListSyntax)
                WriteArgumentListSyntax(
                    builder,
                    syntaxNode as ArgumentListSyntax
                );
            if (syntaxNode is BracketedArgumentListSyntax)
                WriteBracketedArgumentListSyntax(
                    builder,
                    syntaxNode as BracketedArgumentListSyntax
                );
            if (syntaxNode is ArgumentSyntax)
                WriteArgumentSyntax(builder, syntaxNode as ArgumentSyntax);
            if (syntaxNode is NameColonSyntax)
                WriteNameColonSyntax(builder, syntaxNode as NameColonSyntax);
            if (syntaxNode is DeclarationExpressionSyntax)
                WriteDeclarationExpressionSyntax(
                    builder,
                    syntaxNode as DeclarationExpressionSyntax
                );
            if (syntaxNode is CastExpressionSyntax)
                WriteCastExpressionSyntax(
                    builder,
                    syntaxNode as CastExpressionSyntax
                );
            if (syntaxNode is AnonymousMethodExpressionSyntax)
                WriteAnonymousMethodExpressionSyntax(
                    builder,
                    syntaxNode as AnonymousMethodExpressionSyntax
                );
            if (syntaxNode is SimpleLambdaExpressionSyntax)
                WriteSimpleLambdaExpressionSyntax(
                    builder,
                    syntaxNode as SimpleLambdaExpressionSyntax
                );
            if (syntaxNode is RefExpressionSyntax)
                WriteRefExpressionSyntax(
                    builder,
                    syntaxNode as RefExpressionSyntax
                );
            if (syntaxNode is ParenthesizedLambdaExpressionSyntax)
                WriteParenthesizedLambdaExpressionSyntax(
                    builder,
                    syntaxNode as ParenthesizedLambdaExpressionSyntax
                );
            if (syntaxNode is InitializerExpressionSyntax)
                WriteInitializerExpressionSyntax(
                    builder,
                    syntaxNode as InitializerExpressionSyntax
                );
            if (syntaxNode is ImplicitObjectCreationExpressionSyntax)
                WriteImplicitObjectCreationExpressionSyntax(
                    builder,
                    syntaxNode as ImplicitObjectCreationExpressionSyntax
                );
            if (syntaxNode is ObjectCreationExpressionSyntax)
                WriteObjectCreationExpressionSyntax(
                    builder,
                    syntaxNode as ObjectCreationExpressionSyntax
                );
            if (syntaxNode is WithExpressionSyntax)
                WriteWithExpressionSyntax(
                    builder,
                    syntaxNode as WithExpressionSyntax
                );
            if (syntaxNode is AnonymousObjectMemberDeclaratorSyntax)
                WriteAnonymousObjectMemberDeclaratorSyntax(
                    builder,
                    syntaxNode as AnonymousObjectMemberDeclaratorSyntax
                );
            if (syntaxNode is AnonymousObjectCreationExpressionSyntax)
                WriteAnonymousObjectCreationExpressionSyntax(
                    builder,
                    syntaxNode as AnonymousObjectCreationExpressionSyntax
                );
            if (syntaxNode is ArrayCreationExpressionSyntax)
                WriteArrayCreationExpressionSyntax(
                    builder,
                    syntaxNode as ArrayCreationExpressionSyntax
                );
            if (syntaxNode is ImplicitArrayCreationExpressionSyntax)
                WriteImplicitArrayCreationExpressionSyntax(
                    builder,
                    syntaxNode as ImplicitArrayCreationExpressionSyntax
                );
            if (syntaxNode is StackAllocArrayCreationExpressionSyntax)
                WriteStackAllocArrayCreationExpressionSyntax(
                    builder,
                    syntaxNode as StackAllocArrayCreationExpressionSyntax
                );
            if (syntaxNode is ImplicitStackAllocArrayCreationExpressionSyntax)
                WriteImplicitStackAllocArrayCreationExpressionSyntax(
                    builder,
                    syntaxNode as ImplicitStackAllocArrayCreationExpressionSyntax
                );
            if (syntaxNode is QueryExpressionSyntax)
                WriteQueryExpressionSyntax(
                    builder,
                    syntaxNode as QueryExpressionSyntax
                );
            if (syntaxNode is QueryBodySyntax)
                WriteQueryBodySyntax(builder, syntaxNode as QueryBodySyntax);
            if (syntaxNode is FromClauseSyntax)
                WriteFromClauseSyntax(builder, syntaxNode as FromClauseSyntax);
            if (syntaxNode is LetClauseSyntax)
                WriteLetClauseSyntax(builder, syntaxNode as LetClauseSyntax);
            if (syntaxNode is JoinClauseSyntax)
                WriteJoinClauseSyntax(builder, syntaxNode as JoinClauseSyntax);
            if (syntaxNode is JoinIntoClauseSyntax)
                WriteJoinIntoClauseSyntax(
                    builder,
                    syntaxNode as JoinIntoClauseSyntax
                );
            if (syntaxNode is WhereClauseSyntax)
                WriteWhereClauseSyntax(
                    builder,
                    syntaxNode as WhereClauseSyntax
                );
            if (syntaxNode is OrderByClauseSyntax)
                WriteOrderByClauseSyntax(
                    builder,
                    syntaxNode as OrderByClauseSyntax
                );
            if (syntaxNode is OrderingSyntax)
                WriteOrderingSyntax(builder, syntaxNode as OrderingSyntax);
            if (syntaxNode is SelectClauseSyntax)
                WriteSelectClauseSyntax(
                    builder,
                    syntaxNode as SelectClauseSyntax
                );
            if (syntaxNode is GroupClauseSyntax)
                WriteGroupClauseSyntax(
                    builder,
                    syntaxNode as GroupClauseSyntax
                );
            if (syntaxNode is QueryContinuationSyntax)
                WriteQueryContinuationSyntax(
                    builder,
                    syntaxNode as QueryContinuationSyntax
                );
            if (syntaxNode is OmittedArraySizeExpressionSyntax)
                WriteOmittedArraySizeExpressionSyntax(
                    builder,
                    syntaxNode as OmittedArraySizeExpressionSyntax
                );
            if (syntaxNode is InterpolatedStringExpressionSyntax)
                WriteInterpolatedStringExpressionSyntax(
                    builder,
                    syntaxNode as InterpolatedStringExpressionSyntax
                );
            if (syntaxNode is IsPatternExpressionSyntax)
                WriteIsPatternExpressionSyntax(
                    builder,
                    syntaxNode as IsPatternExpressionSyntax
                );
            if (syntaxNode is ThrowExpressionSyntax)
                WriteThrowExpressionSyntax(
                    builder,
                    syntaxNode as ThrowExpressionSyntax
                );
            if (syntaxNode is WhenClauseSyntax)
                WriteWhenClauseSyntax(builder, syntaxNode as WhenClauseSyntax);
            if (syntaxNode is DiscardPatternSyntax)
                WriteDiscardPatternSyntax(
                    builder,
                    syntaxNode as DiscardPatternSyntax
                );
            if (syntaxNode is DeclarationPatternSyntax)
                WriteDeclarationPatternSyntax(
                    builder,
                    syntaxNode as DeclarationPatternSyntax
                );
            if (syntaxNode is VarPatternSyntax)
                WriteVarPatternSyntax(builder, syntaxNode as VarPatternSyntax);
            if (syntaxNode is RecursivePatternSyntax)
                WriteRecursivePatternSyntax(
                    builder,
                    syntaxNode as RecursivePatternSyntax
                );
            if (syntaxNode is PositionalPatternClauseSyntax)
                WritePositionalPatternClauseSyntax(
                    builder,
                    syntaxNode as PositionalPatternClauseSyntax
                );
            if (syntaxNode is PropertyPatternClauseSyntax)
                WritePropertyPatternClauseSyntax(
                    builder,
                    syntaxNode as PropertyPatternClauseSyntax
                );
            if (syntaxNode is SubpatternSyntax)
                WriteSubpatternSyntax(builder, syntaxNode as SubpatternSyntax);
            if (syntaxNode is ConstantPatternSyntax)
                WriteConstantPatternSyntax(
                    builder,
                    syntaxNode as ConstantPatternSyntax
                );
            if (syntaxNode is ParenthesizedPatternSyntax)
                WriteParenthesizedPatternSyntax(
                    builder,
                    syntaxNode as ParenthesizedPatternSyntax
                );
            if (syntaxNode is RelationalPatternSyntax)
                WriteRelationalPatternSyntax(
                    builder,
                    syntaxNode as RelationalPatternSyntax
                );
            if (syntaxNode is TypePatternSyntax)
                WriteTypePatternSyntax(
                    builder,
                    syntaxNode as TypePatternSyntax
                );
            if (syntaxNode is BinaryPatternSyntax)
                WriteBinaryPatternSyntax(
                    builder,
                    syntaxNode as BinaryPatternSyntax
                );
            if (syntaxNode is UnaryPatternSyntax)
                WriteUnaryPatternSyntax(
                    builder,
                    syntaxNode as UnaryPatternSyntax
                );
            if (syntaxNode is InterpolatedStringTextSyntax)
                WriteInterpolatedStringTextSyntax(
                    builder,
                    syntaxNode as InterpolatedStringTextSyntax
                );
            if (syntaxNode is InterpolationSyntax)
                WriteInterpolationSyntax(
                    builder,
                    syntaxNode as InterpolationSyntax
                );
            if (syntaxNode is InterpolationAlignmentClauseSyntax)
                WriteInterpolationAlignmentClauseSyntax(
                    builder,
                    syntaxNode as InterpolationAlignmentClauseSyntax
                );
            if (syntaxNode is InterpolationFormatClauseSyntax)
                WriteInterpolationFormatClauseSyntax(
                    builder,
                    syntaxNode as InterpolationFormatClauseSyntax
                );
            if (syntaxNode is GlobalStatementSyntax)
                WriteGlobalStatementSyntax(
                    builder,
                    syntaxNode as GlobalStatementSyntax
                );
            if (syntaxNode is BlockSyntax)
                WriteBlockSyntax(builder, syntaxNode as BlockSyntax);
            if (syntaxNode is LocalFunctionStatementSyntax)
                WriteLocalFunctionStatementSyntax(
                    builder,
                    syntaxNode as LocalFunctionStatementSyntax
                );
            if (syntaxNode is LocalDeclarationStatementSyntax)
                WriteLocalDeclarationStatementSyntax(
                    builder,
                    syntaxNode as LocalDeclarationStatementSyntax
                );
            if (syntaxNode is VariableDeclarationSyntax)
                WriteVariableDeclarationSyntax(
                    builder,
                    syntaxNode as VariableDeclarationSyntax
                );
            if (syntaxNode is VariableDeclaratorSyntax)
                WriteVariableDeclaratorSyntax(
                    builder,
                    syntaxNode as VariableDeclaratorSyntax
                );
            if (syntaxNode is EqualsValueClauseSyntax)
                WriteEqualsValueClauseSyntax(
                    builder,
                    syntaxNode as EqualsValueClauseSyntax
                );
            if (syntaxNode is SingleVariableDesignationSyntax)
                WriteSingleVariableDesignationSyntax(
                    builder,
                    syntaxNode as SingleVariableDesignationSyntax
                );
            if (syntaxNode is DiscardDesignationSyntax)
                WriteDiscardDesignationSyntax(
                    builder,
                    syntaxNode as DiscardDesignationSyntax
                );
            if (syntaxNode is ParenthesizedVariableDesignationSyntax)
                WriteParenthesizedVariableDesignationSyntax(
                    builder,
                    syntaxNode as ParenthesizedVariableDesignationSyntax
                );
            if (syntaxNode is ExpressionStatementSyntax)
                WriteExpressionStatementSyntax(
                    builder,
                    syntaxNode as ExpressionStatementSyntax
                );
            if (syntaxNode is EmptyStatementSyntax)
                WriteEmptyStatementSyntax(
                    builder,
                    syntaxNode as EmptyStatementSyntax
                );
            if (syntaxNode is LabeledStatementSyntax)
                WriteLabeledStatementSyntax(
                    builder,
                    syntaxNode as LabeledStatementSyntax
                );
            if (syntaxNode is GotoStatementSyntax)
                WriteGotoStatementSyntax(
                    builder,
                    syntaxNode as GotoStatementSyntax
                );
            if (syntaxNode is BreakStatementSyntax)
                WriteBreakStatementSyntax(
                    builder,
                    syntaxNode as BreakStatementSyntax
                );
            if (syntaxNode is ContinueStatementSyntax)
                WriteContinueStatementSyntax(
                    builder,
                    syntaxNode as ContinueStatementSyntax
                );
            if (syntaxNode is ReturnStatementSyntax)
                WriteReturnStatementSyntax(
                    builder,
                    syntaxNode as ReturnStatementSyntax
                );
            if (syntaxNode is ThrowStatementSyntax)
                WriteThrowStatementSyntax(
                    builder,
                    syntaxNode as ThrowStatementSyntax
                );
            if (syntaxNode is YieldStatementSyntax)
                WriteYieldStatementSyntax(
                    builder,
                    syntaxNode as YieldStatementSyntax
                );
            if (syntaxNode is WhileStatementSyntax)
                WriteWhileStatementSyntax(
                    builder,
                    syntaxNode as WhileStatementSyntax
                );
            if (syntaxNode is DoStatementSyntax)
                WriteDoStatementSyntax(
                    builder,
                    syntaxNode as DoStatementSyntax
                );
            if (syntaxNode is ForStatementSyntax)
                WriteForStatementSyntax(
                    builder,
                    syntaxNode as ForStatementSyntax
                );
            if (syntaxNode is ForEachStatementSyntax)
                WriteForEachStatementSyntax(
                    builder,
                    syntaxNode as ForEachStatementSyntax
                );
            if (syntaxNode is ForEachVariableStatementSyntax)
                WriteForEachVariableStatementSyntax(
                    builder,
                    syntaxNode as ForEachVariableStatementSyntax
                );
            if (syntaxNode is UsingStatementSyntax)
                WriteUsingStatementSyntax(
                    builder,
                    syntaxNode as UsingStatementSyntax
                );
            if (syntaxNode is FixedStatementSyntax)
                WriteFixedStatementSyntax(
                    builder,
                    syntaxNode as FixedStatementSyntax
                );
            if (syntaxNode is CheckedStatementSyntax)
                WriteCheckedStatementSyntax(
                    builder,
                    syntaxNode as CheckedStatementSyntax
                );
            if (syntaxNode is UnsafeStatementSyntax)
                WriteUnsafeStatementSyntax(
                    builder,
                    syntaxNode as UnsafeStatementSyntax
                );
            if (syntaxNode is LockStatementSyntax)
                WriteLockStatementSyntax(
                    builder,
                    syntaxNode as LockStatementSyntax
                );
            if (syntaxNode is IfStatementSyntax)
                WriteIfStatementSyntax(
                    builder,
                    syntaxNode as IfStatementSyntax
                );
            if (syntaxNode is ElseClauseSyntax)
                WriteElseClauseSyntax(builder, syntaxNode as ElseClauseSyntax);
            if (syntaxNode is SwitchStatementSyntax)
                WriteSwitchStatementSyntax(
                    builder,
                    syntaxNode as SwitchStatementSyntax
                );
            if (syntaxNode is SwitchSectionSyntax)
                WriteSwitchSectionSyntax(
                    builder,
                    syntaxNode as SwitchSectionSyntax
                );
            if (syntaxNode is CasePatternSwitchLabelSyntax)
                WriteCasePatternSwitchLabelSyntax(
                    builder,
                    syntaxNode as CasePatternSwitchLabelSyntax
                );
            if (syntaxNode is CaseSwitchLabelSyntax)
                WriteCaseSwitchLabelSyntax(
                    builder,
                    syntaxNode as CaseSwitchLabelSyntax
                );
            if (syntaxNode is DefaultSwitchLabelSyntax)
                WriteDefaultSwitchLabelSyntax(
                    builder,
                    syntaxNode as DefaultSwitchLabelSyntax
                );
            if (syntaxNode is SwitchExpressionSyntax)
                WriteSwitchExpressionSyntax(
                    builder,
                    syntaxNode as SwitchExpressionSyntax
                );
            if (syntaxNode is SwitchExpressionArmSyntax)
                WriteSwitchExpressionArmSyntax(
                    builder,
                    syntaxNode as SwitchExpressionArmSyntax
                );
            if (syntaxNode is TryStatementSyntax)
                WriteTryStatementSyntax(
                    builder,
                    syntaxNode as TryStatementSyntax
                );
            if (syntaxNode is CatchClauseSyntax)
                WriteCatchClauseSyntax(
                    builder,
                    syntaxNode as CatchClauseSyntax
                );
            if (syntaxNode is CatchDeclarationSyntax)
                WriteCatchDeclarationSyntax(
                    builder,
                    syntaxNode as CatchDeclarationSyntax
                );
            if (syntaxNode is CatchFilterClauseSyntax)
                WriteCatchFilterClauseSyntax(
                    builder,
                    syntaxNode as CatchFilterClauseSyntax
                );
            if (syntaxNode is FinallyClauseSyntax)
                WriteFinallyClauseSyntax(
                    builder,
                    syntaxNode as FinallyClauseSyntax
                );
            if (syntaxNode is CompilationUnitSyntax)
                WriteCompilationUnitSyntax(
                    builder,
                    syntaxNode as CompilationUnitSyntax
                );
            if (syntaxNode is ExternAliasDirectiveSyntax)
                WriteExternAliasDirectiveSyntax(
                    builder,
                    syntaxNode as ExternAliasDirectiveSyntax
                );
            if (syntaxNode is UsingDirectiveSyntax)
                WriteUsingDirectiveSyntax(
                    builder,
                    syntaxNode as UsingDirectiveSyntax
                );
            if (syntaxNode is NamespaceDeclarationSyntax)
                WriteNamespaceDeclarationSyntax(
                    builder,
                    syntaxNode as NamespaceDeclarationSyntax
                );
            if (syntaxNode is AttributeListSyntax)
                WriteAttributeListSyntax(
                    builder,
                    syntaxNode as AttributeListSyntax
                );
            if (syntaxNode is AttributeTargetSpecifierSyntax)
                WriteAttributeTargetSpecifierSyntax(
                    builder,
                    syntaxNode as AttributeTargetSpecifierSyntax
                );
            if (syntaxNode is AttributeSyntax)
                WriteAttributeSyntax(builder, syntaxNode as AttributeSyntax);
            if (syntaxNode is AttributeArgumentListSyntax)
                WriteAttributeArgumentListSyntax(
                    builder,
                    syntaxNode as AttributeArgumentListSyntax
                );
            if (syntaxNode is AttributeArgumentSyntax)
                WriteAttributeArgumentSyntax(
                    builder,
                    syntaxNode as AttributeArgumentSyntax
                );
            if (syntaxNode is NameEqualsSyntax)
                WriteNameEqualsSyntax(builder, syntaxNode as NameEqualsSyntax);
            if (syntaxNode is TypeParameterListSyntax)
                WriteTypeParameterListSyntax(
                    builder,
                    syntaxNode as TypeParameterListSyntax
                );
            if (syntaxNode is TypeParameterSyntax)
                WriteTypeParameterSyntax(
                    builder,
                    syntaxNode as TypeParameterSyntax
                );
            if (syntaxNode is ClassDeclarationSyntax)
                WriteClassDeclarationSyntax(
                    builder,
                    syntaxNode as ClassDeclarationSyntax
                );
            if (syntaxNode is StructDeclarationSyntax)
                WriteStructDeclarationSyntax(
                    builder,
                    syntaxNode as StructDeclarationSyntax
                );
            if (syntaxNode is InterfaceDeclarationSyntax)
                WriteInterfaceDeclarationSyntax(
                    builder,
                    syntaxNode as InterfaceDeclarationSyntax
                );
            if (syntaxNode is RecordDeclarationSyntax)
                WriteRecordDeclarationSyntax(
                    builder,
                    syntaxNode as RecordDeclarationSyntax
                );
            if (syntaxNode is EnumDeclarationSyntax)
                WriteEnumDeclarationSyntax(
                    builder,
                    syntaxNode as EnumDeclarationSyntax
                );
            if (syntaxNode is DelegateDeclarationSyntax)
                WriteDelegateDeclarationSyntax(
                    builder,
                    syntaxNode as DelegateDeclarationSyntax
                );
            if (syntaxNode is EnumMemberDeclarationSyntax)
                WriteEnumMemberDeclarationSyntax(
                    builder,
                    syntaxNode as EnumMemberDeclarationSyntax
                );
            if (syntaxNode is BaseListSyntax)
                WriteBaseListSyntax(builder, syntaxNode as BaseListSyntax);
            if (syntaxNode is SimpleBaseTypeSyntax)
                WriteSimpleBaseTypeSyntax(
                    builder,
                    syntaxNode as SimpleBaseTypeSyntax
                );
            if (syntaxNode is PrimaryConstructorBaseTypeSyntax)
                WritePrimaryConstructorBaseTypeSyntax(
                    builder,
                    syntaxNode as PrimaryConstructorBaseTypeSyntax
                );
            if (syntaxNode is TypeParameterConstraintClauseSyntax)
                WriteTypeParameterConstraintClauseSyntax(
                    builder,
                    syntaxNode as TypeParameterConstraintClauseSyntax
                );
            if (syntaxNode is ConstructorConstraintSyntax)
                WriteConstructorConstraintSyntax(
                    builder,
                    syntaxNode as ConstructorConstraintSyntax
                );
            if (syntaxNode is ClassOrStructConstraintSyntax)
                WriteClassOrStructConstraintSyntax(
                    builder,
                    syntaxNode as ClassOrStructConstraintSyntax
                );
            if (syntaxNode is TypeConstraintSyntax)
                WriteTypeConstraintSyntax(
                    builder,
                    syntaxNode as TypeConstraintSyntax
                );
            if (syntaxNode is DefaultConstraintSyntax)
                WriteDefaultConstraintSyntax(
                    builder,
                    syntaxNode as DefaultConstraintSyntax
                );
            if (syntaxNode is FieldDeclarationSyntax)
                WriteFieldDeclarationSyntax(
                    builder,
                    syntaxNode as FieldDeclarationSyntax
                );
            if (syntaxNode is EventFieldDeclarationSyntax)
                WriteEventFieldDeclarationSyntax(
                    builder,
                    syntaxNode as EventFieldDeclarationSyntax
                );
            if (syntaxNode is ExplicitInterfaceSpecifierSyntax)
                WriteExplicitInterfaceSpecifierSyntax(
                    builder,
                    syntaxNode as ExplicitInterfaceSpecifierSyntax
                );
            if (syntaxNode is MethodDeclarationSyntax)
                WriteMethodDeclarationSyntax(
                    builder,
                    syntaxNode as MethodDeclarationSyntax
                );
            if (syntaxNode is OperatorDeclarationSyntax)
                WriteOperatorDeclarationSyntax(
                    builder,
                    syntaxNode as OperatorDeclarationSyntax
                );
            if (syntaxNode is ConversionOperatorDeclarationSyntax)
                WriteConversionOperatorDeclarationSyntax(
                    builder,
                    syntaxNode as ConversionOperatorDeclarationSyntax
                );
            if (syntaxNode is ConstructorDeclarationSyntax)
                WriteConstructorDeclarationSyntax(
                    builder,
                    syntaxNode as ConstructorDeclarationSyntax
                );
            if (syntaxNode is ConstructorInitializerSyntax)
                WriteConstructorInitializerSyntax(
                    builder,
                    syntaxNode as ConstructorInitializerSyntax
                );
            if (syntaxNode is DestructorDeclarationSyntax)
                WriteDestructorDeclarationSyntax(
                    builder,
                    syntaxNode as DestructorDeclarationSyntax
                );
            if (syntaxNode is PropertyDeclarationSyntax)
                WritePropertyDeclarationSyntax(
                    builder,
                    syntaxNode as PropertyDeclarationSyntax
                );
            if (syntaxNode is ArrowExpressionClauseSyntax)
                WriteArrowExpressionClauseSyntax(
                    builder,
                    syntaxNode as ArrowExpressionClauseSyntax
                );
            if (syntaxNode is EventDeclarationSyntax)
                WriteEventDeclarationSyntax(
                    builder,
                    syntaxNode as EventDeclarationSyntax
                );
            if (syntaxNode is IndexerDeclarationSyntax)
                WriteIndexerDeclarationSyntax(
                    builder,
                    syntaxNode as IndexerDeclarationSyntax
                );
            if (syntaxNode is AccessorListSyntax)
                WriteAccessorListSyntax(
                    builder,
                    syntaxNode as AccessorListSyntax
                );
            if (syntaxNode is AccessorDeclarationSyntax)
                WriteAccessorDeclarationSyntax(
                    builder,
                    syntaxNode as AccessorDeclarationSyntax
                );
            if (syntaxNode is ParameterListSyntax)
                WriteParameterListSyntax(
                    builder,
                    syntaxNode as ParameterListSyntax
                );
            if (syntaxNode is BracketedParameterListSyntax)
                WriteBracketedParameterListSyntax(
                    builder,
                    syntaxNode as BracketedParameterListSyntax
                );
            if (syntaxNode is ParameterSyntax)
                WriteParameterSyntax(builder, syntaxNode as ParameterSyntax);
            if (syntaxNode is FunctionPointerParameterSyntax)
                WriteFunctionPointerParameterSyntax(
                    builder,
                    syntaxNode as FunctionPointerParameterSyntax
                );
            if (syntaxNode is IncompleteMemberSyntax)
                WriteIncompleteMemberSyntax(
                    builder,
                    syntaxNode as IncompleteMemberSyntax
                );
            if (syntaxNode is SkippedTokensTriviaSyntax)
                WriteSkippedTokensTriviaSyntax(
                    builder,
                    syntaxNode as SkippedTokensTriviaSyntax
                );
            if (syntaxNode is DocumentationCommentTriviaSyntax)
                WriteDocumentationCommentTriviaSyntax(
                    builder,
                    syntaxNode as DocumentationCommentTriviaSyntax
                );
            if (syntaxNode is TypeCrefSyntax)
                WriteTypeCrefSyntax(builder, syntaxNode as TypeCrefSyntax);
            if (syntaxNode is QualifiedCrefSyntax)
                WriteQualifiedCrefSyntax(
                    builder,
                    syntaxNode as QualifiedCrefSyntax
                );
            if (syntaxNode is NameMemberCrefSyntax)
                WriteNameMemberCrefSyntax(
                    builder,
                    syntaxNode as NameMemberCrefSyntax
                );
            if (syntaxNode is IndexerMemberCrefSyntax)
                WriteIndexerMemberCrefSyntax(
                    builder,
                    syntaxNode as IndexerMemberCrefSyntax
                );
            if (syntaxNode is OperatorMemberCrefSyntax)
                WriteOperatorMemberCrefSyntax(
                    builder,
                    syntaxNode as OperatorMemberCrefSyntax
                );
            if (syntaxNode is ConversionOperatorMemberCrefSyntax)
                WriteConversionOperatorMemberCrefSyntax(
                    builder,
                    syntaxNode as ConversionOperatorMemberCrefSyntax
                );
            if (syntaxNode is CrefParameterListSyntax)
                WriteCrefParameterListSyntax(
                    builder,
                    syntaxNode as CrefParameterListSyntax
                );
            if (syntaxNode is CrefBracketedParameterListSyntax)
                WriteCrefBracketedParameterListSyntax(
                    builder,
                    syntaxNode as CrefBracketedParameterListSyntax
                );
            if (syntaxNode is CrefParameterSyntax)
                WriteCrefParameterSyntax(
                    builder,
                    syntaxNode as CrefParameterSyntax
                );
            if (syntaxNode is XmlElementSyntax)
                WriteXmlElementSyntax(builder, syntaxNode as XmlElementSyntax);
            if (syntaxNode is XmlElementStartTagSyntax)
                WriteXmlElementStartTagSyntax(
                    builder,
                    syntaxNode as XmlElementStartTagSyntax
                );
            if (syntaxNode is XmlElementEndTagSyntax)
                WriteXmlElementEndTagSyntax(
                    builder,
                    syntaxNode as XmlElementEndTagSyntax
                );
            if (syntaxNode is XmlEmptyElementSyntax)
                WriteXmlEmptyElementSyntax(
                    builder,
                    syntaxNode as XmlEmptyElementSyntax
                );
            if (syntaxNode is XmlNameSyntax)
                WriteXmlNameSyntax(builder, syntaxNode as XmlNameSyntax);
            if (syntaxNode is XmlPrefixSyntax)
                WriteXmlPrefixSyntax(builder, syntaxNode as XmlPrefixSyntax);
            if (syntaxNode is XmlTextAttributeSyntax)
                WriteXmlTextAttributeSyntax(
                    builder,
                    syntaxNode as XmlTextAttributeSyntax
                );
            if (syntaxNode is XmlCrefAttributeSyntax)
                WriteXmlCrefAttributeSyntax(
                    builder,
                    syntaxNode as XmlCrefAttributeSyntax
                );
            if (syntaxNode is XmlNameAttributeSyntax)
                WriteXmlNameAttributeSyntax(
                    builder,
                    syntaxNode as XmlNameAttributeSyntax
                );
            if (syntaxNode is XmlTextSyntax)
                WriteXmlTextSyntax(builder, syntaxNode as XmlTextSyntax);
            if (syntaxNode is XmlCDataSectionSyntax)
                WriteXmlCDataSectionSyntax(
                    builder,
                    syntaxNode as XmlCDataSectionSyntax
                );
            if (syntaxNode is XmlProcessingInstructionSyntax)
                WriteXmlProcessingInstructionSyntax(
                    builder,
                    syntaxNode as XmlProcessingInstructionSyntax
                );
            if (syntaxNode is XmlCommentSyntax)
                WriteXmlCommentSyntax(builder, syntaxNode as XmlCommentSyntax);
            if (syntaxNode is IfDirectiveTriviaSyntax)
                WriteIfDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as IfDirectiveTriviaSyntax
                );
            if (syntaxNode is ElifDirectiveTriviaSyntax)
                WriteElifDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as ElifDirectiveTriviaSyntax
                );
            if (syntaxNode is ElseDirectiveTriviaSyntax)
                WriteElseDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as ElseDirectiveTriviaSyntax
                );
            if (syntaxNode is EndIfDirectiveTriviaSyntax)
                WriteEndIfDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as EndIfDirectiveTriviaSyntax
                );
            if (syntaxNode is RegionDirectiveTriviaSyntax)
                WriteRegionDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as RegionDirectiveTriviaSyntax
                );
            if (syntaxNode is EndRegionDirectiveTriviaSyntax)
                WriteEndRegionDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as EndRegionDirectiveTriviaSyntax
                );
            if (syntaxNode is ErrorDirectiveTriviaSyntax)
                WriteErrorDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as ErrorDirectiveTriviaSyntax
                );
            if (syntaxNode is WarningDirectiveTriviaSyntax)
                WriteWarningDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as WarningDirectiveTriviaSyntax
                );
            if (syntaxNode is BadDirectiveTriviaSyntax)
                WriteBadDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as BadDirectiveTriviaSyntax
                );
            if (syntaxNode is DefineDirectiveTriviaSyntax)
                WriteDefineDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as DefineDirectiveTriviaSyntax
                );
            if (syntaxNode is UndefDirectiveTriviaSyntax)
                WriteUndefDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as UndefDirectiveTriviaSyntax
                );
            if (syntaxNode is LineDirectiveTriviaSyntax)
                WriteLineDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as LineDirectiveTriviaSyntax
                );
            if (syntaxNode is PragmaWarningDirectiveTriviaSyntax)
                WritePragmaWarningDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as PragmaWarningDirectiveTriviaSyntax
                );
            if (syntaxNode is PragmaChecksumDirectiveTriviaSyntax)
                WritePragmaChecksumDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as PragmaChecksumDirectiveTriviaSyntax
                );
            if (syntaxNode is ReferenceDirectiveTriviaSyntax)
                WriteReferenceDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as ReferenceDirectiveTriviaSyntax
                );
            if (syntaxNode is LoadDirectiveTriviaSyntax)
                WriteLoadDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as LoadDirectiveTriviaSyntax
                );
            if (syntaxNode is ShebangDirectiveTriviaSyntax)
                WriteShebangDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as ShebangDirectiveTriviaSyntax
                );
            if (syntaxNode is NullableDirectiveTriviaSyntax)
                WriteNullableDirectiveTriviaSyntax(
                    builder,
                    syntaxNode as NullableDirectiveTriviaSyntax
                );
        }

        public static void WriteIdentifierNameSyntax(
            StringBuilder builder,
            IdentifierNameSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteQualifiedNameSyntax(
            StringBuilder builder,
            QualifiedNameSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Left != default(NameSyntax))
            {
                var leftBuilder = new StringBuilder();
                WriteSyntaxNode(leftBuilder, syntaxNode.Left);
                properties.Add($"\"left\":{leftBuilder.ToString()}");
            }
            if (syntaxNode.DotToken != default(SyntaxToken))
            {
                var dotTokenBuilder = new StringBuilder();
                WriteSyntaxToken(dotTokenBuilder, syntaxNode.DotToken);
                properties.Add($"\"dotToken\":{dotTokenBuilder.ToString()}");
            }
            if (syntaxNode.Right != default(SimpleNameSyntax))
            {
                var rightBuilder = new StringBuilder();
                WriteSyntaxNode(rightBuilder, syntaxNode.Right);
                properties.Add($"\"right\":{rightBuilder.ToString()}");
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteGenericNameSyntax(
            StringBuilder builder,
            GenericNameSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.TypeArgumentList != default(TypeArgumentListSyntax))
            {
                var typeArgumentListBuilder = new StringBuilder();
                WriteTypeArgumentListSyntax(
                    typeArgumentListBuilder,
                    syntaxNode.TypeArgumentList
                );
                properties.Add(
                    $"\"typeArgumentList\":{typeArgumentListBuilder.ToString()}"
                );
            }
            properties.Add(
                WriteBoolean(
                    "isUnboundGenericName",
                    syntaxNode.IsUnboundGenericName
                )
            );
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTypeArgumentListSyntax(
            StringBuilder builder,
            TypeArgumentListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.LessThanToken != default(SyntaxToken))
            {
                var lessThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    lessThanTokenBuilder,
                    syntaxNode.LessThanToken
                );
                properties.Add(
                    $"\"lessThanToken\":{lessThanTokenBuilder.ToString()}"
                );
            }
            var arguments = new List<string>();
            foreach (var node in syntaxNode.Arguments)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                arguments.Add(innerBuilder.ToString());
            }
            properties.Add($"\"arguments\":[{string.Join(",", arguments)}]");
            if (syntaxNode.GreaterThanToken != default(SyntaxToken))
            {
                var greaterThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    greaterThanTokenBuilder,
                    syntaxNode.GreaterThanToken
                );
                properties.Add(
                    $"\"greaterThanToken\":{greaterThanTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAliasQualifiedNameSyntax(
            StringBuilder builder,
            AliasQualifiedNameSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Alias != default(IdentifierNameSyntax))
            {
                var aliasBuilder = new StringBuilder();
                WriteIdentifierNameSyntax(aliasBuilder, syntaxNode.Alias);
                properties.Add($"\"alias\":{aliasBuilder.ToString()}");
            }
            if (syntaxNode.ColonColonToken != default(SyntaxToken))
            {
                var colonColonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    colonColonTokenBuilder,
                    syntaxNode.ColonColonToken
                );
                properties.Add(
                    $"\"colonColonToken\":{colonColonTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(SimpleNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxNode(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePredefinedTypeSyntax(
            StringBuilder builder,
            PredefinedTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteArrayTypeSyntax(
            StringBuilder builder,
            ArrayTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ElementType != default(TypeSyntax))
            {
                var elementTypeBuilder = new StringBuilder();
                WriteSyntaxNode(elementTypeBuilder, syntaxNode.ElementType);
                properties.Add(
                    $"\"elementType\":{elementTypeBuilder.ToString()}"
                );
            }
            var rankSpecifiers = new List<string>();
            foreach (var node in syntaxNode.RankSpecifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteArrayRankSpecifierSyntax(innerBuilder, node);
                rankSpecifiers.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"rankSpecifiers\":[{string.Join(",", rankSpecifiers)}]"
            );
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteArrayRankSpecifierSyntax(
            StringBuilder builder,
            ArrayRankSpecifierSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBracketToken != default(SyntaxToken))
            {
                var openBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBracketTokenBuilder,
                    syntaxNode.OpenBracketToken
                );
                properties.Add(
                    $"\"openBracketToken\":{openBracketTokenBuilder.ToString()}"
                );
            }
            var sizes = new List<string>();
            foreach (var node in syntaxNode.Sizes)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                sizes.Add(innerBuilder.ToString());
            }
            properties.Add($"\"sizes\":[{string.Join(",", sizes)}]");
            if (syntaxNode.CloseBracketToken != default(SyntaxToken))
            {
                var closeBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBracketTokenBuilder,
                    syntaxNode.CloseBracketToken
                );
                properties.Add(
                    $"\"closeBracketToken\":{closeBracketTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rank", syntaxNode.Rank));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePointerTypeSyntax(
            StringBuilder builder,
            PointerTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ElementType != default(TypeSyntax))
            {
                var elementTypeBuilder = new StringBuilder();
                WriteSyntaxNode(elementTypeBuilder, syntaxNode.ElementType);
                properties.Add(
                    $"\"elementType\":{elementTypeBuilder.ToString()}"
                );
            }
            if (syntaxNode.AsteriskToken != default(SyntaxToken))
            {
                var asteriskTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    asteriskTokenBuilder,
                    syntaxNode.AsteriskToken
                );
                properties.Add(
                    $"\"asteriskToken\":{asteriskTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFunctionPointerTypeSyntax(
            StringBuilder builder,
            FunctionPointerTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.DelegateKeyword != default(SyntaxToken))
            {
                var delegateKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    delegateKeywordBuilder,
                    syntaxNode.DelegateKeyword
                );
                properties.Add(
                    $"\"delegateKeyword\":{delegateKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.AsteriskToken != default(SyntaxToken))
            {
                var asteriskTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    asteriskTokenBuilder,
                    syntaxNode.AsteriskToken
                );
                properties.Add(
                    $"\"asteriskToken\":{asteriskTokenBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.CallingConvention != default(FunctionPointerCallingConventionSyntax)
            )
            {
                var callingConventionBuilder = new StringBuilder();
                WriteFunctionPointerCallingConventionSyntax(
                    callingConventionBuilder,
                    syntaxNode.CallingConvention
                );
                properties.Add(
                    $"\"callingConvention\":{callingConventionBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.ParameterList != default(FunctionPointerParameterListSyntax)
            )
            {
                var parameterListBuilder = new StringBuilder();
                WriteFunctionPointerParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFunctionPointerParameterListSyntax(
            StringBuilder builder,
            FunctionPointerParameterListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.LessThanToken != default(SyntaxToken))
            {
                var lessThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    lessThanTokenBuilder,
                    syntaxNode.LessThanToken
                );
                properties.Add(
                    $"\"lessThanToken\":{lessThanTokenBuilder.ToString()}"
                );
            }
            var parameters = new List<string>();
            foreach (var node in syntaxNode.Parameters)
            {
                var innerBuilder = new StringBuilder();
                WriteFunctionPointerParameterSyntax(innerBuilder, node);
                parameters.Add(innerBuilder.ToString());
            }
            properties.Add($"\"parameters\":[{string.Join(",", parameters)}]");
            if (syntaxNode.GreaterThanToken != default(SyntaxToken))
            {
                var greaterThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    greaterThanTokenBuilder,
                    syntaxNode.GreaterThanToken
                );
                properties.Add(
                    $"\"greaterThanToken\":{greaterThanTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFunctionPointerCallingConventionSyntax(
            StringBuilder builder,
            FunctionPointerCallingConventionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ManagedOrUnmanagedKeyword != default(SyntaxToken))
            {
                var managedOrUnmanagedKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    managedOrUnmanagedKeywordBuilder,
                    syntaxNode.ManagedOrUnmanagedKeyword
                );
                properties.Add(
                    $"\"managedOrUnmanagedKeyword\":{managedOrUnmanagedKeywordBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.UnmanagedCallingConventionList != default(FunctionPointerUnmanagedCallingConventionListSyntax)
            )
            {
                var unmanagedCallingConventionListBuilder = new StringBuilder();
                WriteFunctionPointerUnmanagedCallingConventionListSyntax(
                    unmanagedCallingConventionListBuilder,
                    syntaxNode.UnmanagedCallingConventionList
                );
                properties.Add(
                    $"\"unmanagedCallingConventionList\":{unmanagedCallingConventionListBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFunctionPointerUnmanagedCallingConventionListSyntax(
            StringBuilder builder,
            FunctionPointerUnmanagedCallingConventionListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBracketToken != default(SyntaxToken))
            {
                var openBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBracketTokenBuilder,
                    syntaxNode.OpenBracketToken
                );
                properties.Add(
                    $"\"openBracketToken\":{openBracketTokenBuilder.ToString()}"
                );
            }
            var callingConventions = new List<string>();
            foreach (var node in syntaxNode.CallingConventions)
            {
                var innerBuilder = new StringBuilder();
                WriteFunctionPointerUnmanagedCallingConventionSyntax(
                    innerBuilder,
                    node
                );
                callingConventions.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"callingConventions\":[{string.Join(",", callingConventions)}]"
            );
            if (syntaxNode.CloseBracketToken != default(SyntaxToken))
            {
                var closeBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBracketTokenBuilder,
                    syntaxNode.CloseBracketToken
                );
                properties.Add(
                    $"\"closeBracketToken\":{closeBracketTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFunctionPointerUnmanagedCallingConventionSyntax(
            StringBuilder builder,
            FunctionPointerUnmanagedCallingConventionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(SyntaxToken))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxToken(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteNullableTypeSyntax(
            StringBuilder builder,
            NullableTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ElementType != default(TypeSyntax))
            {
                var elementTypeBuilder = new StringBuilder();
                WriteSyntaxNode(elementTypeBuilder, syntaxNode.ElementType);
                properties.Add(
                    $"\"elementType\":{elementTypeBuilder.ToString()}"
                );
            }
            if (syntaxNode.QuestionToken != default(SyntaxToken))
            {
                var questionTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    questionTokenBuilder,
                    syntaxNode.QuestionToken
                );
                properties.Add(
                    $"\"questionToken\":{questionTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTupleTypeSyntax(
            StringBuilder builder,
            TupleTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            var elements = new List<string>();
            foreach (var node in syntaxNode.Elements)
            {
                var innerBuilder = new StringBuilder();
                WriteTupleElementSyntax(innerBuilder, node);
                elements.Add(innerBuilder.ToString());
            }
            properties.Add($"\"elements\":[{string.Join(",", elements)}]");
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTupleElementSyntax(
            StringBuilder builder,
            TupleElementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteOmittedTypeArgumentSyntax(
            StringBuilder builder,
            OmittedTypeArgumentSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OmittedTypeArgumentToken != default(SyntaxToken))
            {
                var omittedTypeArgumentTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    omittedTypeArgumentTokenBuilder,
                    syntaxNode.OmittedTypeArgumentToken
                );
                properties.Add(
                    $"\"omittedTypeArgumentToken\":{omittedTypeArgumentTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRefTypeSyntax(
            StringBuilder builder,
            RefTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.RefKeyword != default(SyntaxToken))
            {
                var refKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(refKeywordBuilder, syntaxNode.RefKeyword);
                properties.Add(
                    $"\"refKeyword\":{refKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ReadOnlyKeyword != default(SyntaxToken))
            {
                var readOnlyKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    readOnlyKeywordBuilder,
                    syntaxNode.ReadOnlyKeyword
                );
                properties.Add(
                    $"\"readOnlyKeyword\":{readOnlyKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            properties.Add(WriteBoolean("isVar", syntaxNode.IsVar));
            properties.Add(WriteBoolean("isUnmanaged", syntaxNode.IsUnmanaged));
            properties.Add(WriteBoolean("isNotNull", syntaxNode.IsNotNull));
            properties.Add(WriteBoolean("isNint", syntaxNode.IsNint));
            properties.Add(WriteBoolean("isNuint", syntaxNode.IsNuint));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteParenthesizedExpressionSyntax(
            StringBuilder builder,
            ParenthesizedExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTupleExpressionSyntax(
            StringBuilder builder,
            TupleExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            var arguments = new List<string>();
            foreach (var node in syntaxNode.Arguments)
            {
                var innerBuilder = new StringBuilder();
                WriteArgumentSyntax(innerBuilder, node);
                arguments.Add(innerBuilder.ToString());
            }
            properties.Add($"\"arguments\":[{string.Join(",", arguments)}]");
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePrefixUnaryExpressionSyntax(
            StringBuilder builder,
            PrefixUnaryExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Operand != default(ExpressionSyntax))
            {
                var operandBuilder = new StringBuilder();
                WriteSyntaxNode(operandBuilder, syntaxNode.Operand);
                properties.Add($"\"operand\":{operandBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAwaitExpressionSyntax(
            StringBuilder builder,
            AwaitExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.AwaitKeyword != default(SyntaxToken))
            {
                var awaitKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(awaitKeywordBuilder, syntaxNode.AwaitKeyword);
                properties.Add(
                    $"\"awaitKeyword\":{awaitKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePostfixUnaryExpressionSyntax(
            StringBuilder builder,
            PostfixUnaryExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Operand != default(ExpressionSyntax))
            {
                var operandBuilder = new StringBuilder();
                WriteSyntaxNode(operandBuilder, syntaxNode.Operand);
                properties.Add($"\"operand\":{operandBuilder.ToString()}");
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteMemberAccessExpressionSyntax(
            StringBuilder builder,
            MemberAccessExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(SimpleNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxNode(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteConditionalAccessExpressionSyntax(
            StringBuilder builder,
            ConditionalAccessExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.WhenNotNull != default(ExpressionSyntax))
            {
                var whenNotNullBuilder = new StringBuilder();
                WriteSyntaxNode(whenNotNullBuilder, syntaxNode.WhenNotNull);
                properties.Add(
                    $"\"whenNotNull\":{whenNotNullBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteMemberBindingExpressionSyntax(
            StringBuilder builder,
            MemberBindingExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(SimpleNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxNode(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteElementBindingExpressionSyntax(
            StringBuilder builder,
            ElementBindingExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ArgumentList != default(BracketedArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteBracketedArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRangeExpressionSyntax(
            StringBuilder builder,
            RangeExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.LeftOperand != default(ExpressionSyntax))
            {
                var leftOperandBuilder = new StringBuilder();
                WriteSyntaxNode(leftOperandBuilder, syntaxNode.LeftOperand);
                properties.Add(
                    $"\"leftOperand\":{leftOperandBuilder.ToString()}"
                );
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.RightOperand != default(ExpressionSyntax))
            {
                var rightOperandBuilder = new StringBuilder();
                WriteSyntaxNode(rightOperandBuilder, syntaxNode.RightOperand);
                properties.Add(
                    $"\"rightOperand\":{rightOperandBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteImplicitElementAccessSyntax(
            StringBuilder builder,
            ImplicitElementAccessSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ArgumentList != default(BracketedArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteBracketedArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBinaryExpressionSyntax(
            StringBuilder builder,
            BinaryExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Left != default(ExpressionSyntax))
            {
                var leftBuilder = new StringBuilder();
                WriteSyntaxNode(leftBuilder, syntaxNode.Left);
                properties.Add($"\"left\":{leftBuilder.ToString()}");
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Right != default(ExpressionSyntax))
            {
                var rightBuilder = new StringBuilder();
                WriteSyntaxNode(rightBuilder, syntaxNode.Right);
                properties.Add($"\"right\":{rightBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAssignmentExpressionSyntax(
            StringBuilder builder,
            AssignmentExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Left != default(ExpressionSyntax))
            {
                var leftBuilder = new StringBuilder();
                WriteSyntaxNode(leftBuilder, syntaxNode.Left);
                properties.Add($"\"left\":{leftBuilder.ToString()}");
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Right != default(ExpressionSyntax))
            {
                var rightBuilder = new StringBuilder();
                WriteSyntaxNode(rightBuilder, syntaxNode.Right);
                properties.Add($"\"right\":{rightBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteConditionalExpressionSyntax(
            StringBuilder builder,
            ConditionalExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            if (syntaxNode.QuestionToken != default(SyntaxToken))
            {
                var questionTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    questionTokenBuilder,
                    syntaxNode.QuestionToken
                );
                properties.Add(
                    $"\"questionToken\":{questionTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.WhenTrue != default(ExpressionSyntax))
            {
                var whenTrueBuilder = new StringBuilder();
                WriteSyntaxNode(whenTrueBuilder, syntaxNode.WhenTrue);
                properties.Add($"\"whenTrue\":{whenTrueBuilder.ToString()}");
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.WhenFalse != default(ExpressionSyntax))
            {
                var whenFalseBuilder = new StringBuilder();
                WriteSyntaxNode(whenFalseBuilder, syntaxNode.WhenFalse);
                properties.Add($"\"whenFalse\":{whenFalseBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteThisExpressionSyntax(
            StringBuilder builder,
            ThisExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Token != default(SyntaxToken))
            {
                var tokenBuilder = new StringBuilder();
                WriteSyntaxToken(tokenBuilder, syntaxNode.Token);
                properties.Add($"\"token\":{tokenBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBaseExpressionSyntax(
            StringBuilder builder,
            BaseExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Token != default(SyntaxToken))
            {
                var tokenBuilder = new StringBuilder();
                WriteSyntaxToken(tokenBuilder, syntaxNode.Token);
                properties.Add($"\"token\":{tokenBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteLiteralExpressionSyntax(
            StringBuilder builder,
            LiteralExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Token != default(SyntaxToken))
            {
                var tokenBuilder = new StringBuilder();
                WriteSyntaxToken(tokenBuilder, syntaxNode.Token);
                properties.Add($"\"token\":{tokenBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteMakeRefExpressionSyntax(
            StringBuilder builder,
            MakeRefExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRefTypeExpressionSyntax(
            StringBuilder builder,
            RefTypeExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRefValueExpressionSyntax(
            StringBuilder builder,
            RefValueExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.Comma != default(SyntaxToken))
            {
                var commaBuilder = new StringBuilder();
                WriteSyntaxToken(commaBuilder, syntaxNode.Comma);
                properties.Add($"\"comma\":{commaBuilder.ToString()}");
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCheckedExpressionSyntax(
            StringBuilder builder,
            CheckedExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDefaultExpressionSyntax(
            StringBuilder builder,
            DefaultExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTypeOfExpressionSyntax(
            StringBuilder builder,
            TypeOfExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSizeOfExpressionSyntax(
            StringBuilder builder,
            SizeOfExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteInvocationExpressionSyntax(
            StringBuilder builder,
            InvocationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.ArgumentList != default(ArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteElementAccessExpressionSyntax(
            StringBuilder builder,
            ElementAccessExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.ArgumentList != default(BracketedArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteBracketedArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteArgumentListSyntax(
            StringBuilder builder,
            ArgumentListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            var arguments = new List<string>();
            foreach (var node in syntaxNode.Arguments)
            {
                var innerBuilder = new StringBuilder();
                WriteArgumentSyntax(innerBuilder, node);
                arguments.Add(innerBuilder.ToString());
            }
            properties.Add($"\"arguments\":[{string.Join(",", arguments)}]");
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBracketedArgumentListSyntax(
            StringBuilder builder,
            BracketedArgumentListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBracketToken != default(SyntaxToken))
            {
                var openBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBracketTokenBuilder,
                    syntaxNode.OpenBracketToken
                );
                properties.Add(
                    $"\"openBracketToken\":{openBracketTokenBuilder.ToString()}"
                );
            }
            var arguments = new List<string>();
            foreach (var node in syntaxNode.Arguments)
            {
                var innerBuilder = new StringBuilder();
                WriteArgumentSyntax(innerBuilder, node);
                arguments.Add(innerBuilder.ToString());
            }
            properties.Add($"\"arguments\":[{string.Join(",", arguments)}]");
            if (syntaxNode.CloseBracketToken != default(SyntaxToken))
            {
                var closeBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBracketTokenBuilder,
                    syntaxNode.CloseBracketToken
                );
                properties.Add(
                    $"\"closeBracketToken\":{closeBracketTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteArgumentSyntax(
            StringBuilder builder,
            ArgumentSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NameColon != default(NameColonSyntax))
            {
                var nameColonBuilder = new StringBuilder();
                WriteNameColonSyntax(nameColonBuilder, syntaxNode.NameColon);
                properties.Add($"\"nameColon\":{nameColonBuilder.ToString()}");
            }
            if (syntaxNode.RefKindKeyword != default(SyntaxToken))
            {
                var refKindKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    refKindKeywordBuilder,
                    syntaxNode.RefKindKeyword
                );
                properties.Add(
                    $"\"refKindKeyword\":{refKindKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.RefOrOutKeyword != default(SyntaxToken))
            {
                var refOrOutKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    refOrOutKeywordBuilder,
                    syntaxNode.RefOrOutKeyword
                );
                properties.Add(
                    $"\"refOrOutKeyword\":{refOrOutKeywordBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteNameColonSyntax(
            StringBuilder builder,
            NameColonSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(IdentifierNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteIdentifierNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDeclarationExpressionSyntax(
            StringBuilder builder,
            DeclarationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Designation != default(VariableDesignationSyntax))
            {
                var designationBuilder = new StringBuilder();
                WriteSyntaxNode(designationBuilder, syntaxNode.Designation);
                properties.Add(
                    $"\"designation\":{designationBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCastExpressionSyntax(
            StringBuilder builder,
            CastExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAnonymousMethodExpressionSyntax(
            StringBuilder builder,
            AnonymousMethodExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.DelegateKeyword != default(SyntaxToken))
            {
                var delegateKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    delegateKeywordBuilder,
                    syntaxNode.DelegateKeyword
                );
                properties.Add(
                    $"\"delegateKeyword\":{delegateKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.Block != default(BlockSyntax))
            {
                var blockBuilder = new StringBuilder();
                WriteBlockSyntax(blockBuilder, syntaxNode.Block);
                properties.Add($"\"block\":{blockBuilder.ToString()}");
            }
            if (syntaxNode.ExpressionBody != default(ExpressionSyntax))
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteSyntaxNode(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.AsyncKeyword != default(SyntaxToken))
            {
                var asyncKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(asyncKeywordBuilder, syntaxNode.AsyncKeyword);
                properties.Add(
                    $"\"asyncKeyword\":{asyncKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(CSharpSyntaxNode))
            {
                var bodyBuilder = new StringBuilder();
                WriteSyntaxNode(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSimpleLambdaExpressionSyntax(
            StringBuilder builder,
            SimpleLambdaExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Parameter != default(ParameterSyntax))
            {
                var parameterBuilder = new StringBuilder();
                WriteParameterSyntax(parameterBuilder, syntaxNode.Parameter);
                properties.Add($"\"parameter\":{parameterBuilder.ToString()}");
            }
            if (syntaxNode.ArrowToken != default(SyntaxToken))
            {
                var arrowTokenBuilder = new StringBuilder();
                WriteSyntaxToken(arrowTokenBuilder, syntaxNode.ArrowToken);
                properties.Add(
                    $"\"arrowToken\":{arrowTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Block != default(BlockSyntax))
            {
                var blockBuilder = new StringBuilder();
                WriteBlockSyntax(blockBuilder, syntaxNode.Block);
                properties.Add($"\"block\":{blockBuilder.ToString()}");
            }
            if (syntaxNode.ExpressionBody != default(ExpressionSyntax))
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteSyntaxNode(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.AsyncKeyword != default(SyntaxToken))
            {
                var asyncKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(asyncKeywordBuilder, syntaxNode.AsyncKeyword);
                properties.Add(
                    $"\"asyncKeyword\":{asyncKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(CSharpSyntaxNode))
            {
                var bodyBuilder = new StringBuilder();
                WriteSyntaxNode(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRefExpressionSyntax(
            StringBuilder builder,
            RefExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.RefKeyword != default(SyntaxToken))
            {
                var refKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(refKeywordBuilder, syntaxNode.RefKeyword);
                properties.Add(
                    $"\"refKeyword\":{refKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteParenthesizedLambdaExpressionSyntax(
            StringBuilder builder,
            ParenthesizedLambdaExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.ArrowToken != default(SyntaxToken))
            {
                var arrowTokenBuilder = new StringBuilder();
                WriteSyntaxToken(arrowTokenBuilder, syntaxNode.ArrowToken);
                properties.Add(
                    $"\"arrowToken\":{arrowTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Block != default(BlockSyntax))
            {
                var blockBuilder = new StringBuilder();
                WriteBlockSyntax(blockBuilder, syntaxNode.Block);
                properties.Add($"\"block\":{blockBuilder.ToString()}");
            }
            if (syntaxNode.ExpressionBody != default(ExpressionSyntax))
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteSyntaxNode(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.AsyncKeyword != default(SyntaxToken))
            {
                var asyncKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(asyncKeywordBuilder, syntaxNode.AsyncKeyword);
                properties.Add(
                    $"\"asyncKeyword\":{asyncKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(CSharpSyntaxNode))
            {
                var bodyBuilder = new StringBuilder();
                WriteSyntaxNode(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteInitializerExpressionSyntax(
            StringBuilder builder,
            InitializerExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var expressions = new List<string>();
            foreach (var node in syntaxNode.Expressions)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                expressions.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"expressions\":[{string.Join(",", expressions)}]"
            );
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteImplicitObjectCreationExpressionSyntax(
            StringBuilder builder,
            ImplicitObjectCreationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NewKeyword != default(SyntaxToken))
            {
                var newKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(newKeywordBuilder, syntaxNode.NewKeyword);
                properties.Add(
                    $"\"newKeyword\":{newKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ArgumentList != default(ArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            if (syntaxNode.Initializer != default(InitializerExpressionSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteInitializerExpressionSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteObjectCreationExpressionSyntax(
            StringBuilder builder,
            ObjectCreationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NewKeyword != default(SyntaxToken))
            {
                var newKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(newKeywordBuilder, syntaxNode.NewKeyword);
                properties.Add(
                    $"\"newKeyword\":{newKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.ArgumentList != default(ArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            if (syntaxNode.Initializer != default(InitializerExpressionSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteInitializerExpressionSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteWithExpressionSyntax(
            StringBuilder builder,
            WithExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.WithKeyword != default(SyntaxToken))
            {
                var withKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(withKeywordBuilder, syntaxNode.WithKeyword);
                properties.Add(
                    $"\"withKeyword\":{withKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Initializer != default(InitializerExpressionSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteInitializerExpressionSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAnonymousObjectMemberDeclaratorSyntax(
            StringBuilder builder,
            AnonymousObjectMemberDeclaratorSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NameEquals != default(NameEqualsSyntax))
            {
                var nameEqualsBuilder = new StringBuilder();
                WriteNameEqualsSyntax(nameEqualsBuilder, syntaxNode.NameEquals);
                properties.Add(
                    $"\"nameEquals\":{nameEqualsBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAnonymousObjectCreationExpressionSyntax(
            StringBuilder builder,
            AnonymousObjectCreationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NewKeyword != default(SyntaxToken))
            {
                var newKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(newKeywordBuilder, syntaxNode.NewKeyword);
                properties.Add(
                    $"\"newKeyword\":{newKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var initializers = new List<string>();
            foreach (var node in syntaxNode.Initializers)
            {
                var innerBuilder = new StringBuilder();
                WriteAnonymousObjectMemberDeclaratorSyntax(innerBuilder, node);
                initializers.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"initializers\":[{string.Join(",", initializers)}]"
            );
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteArrayCreationExpressionSyntax(
            StringBuilder builder,
            ArrayCreationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NewKeyword != default(SyntaxToken))
            {
                var newKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(newKeywordBuilder, syntaxNode.NewKeyword);
                properties.Add(
                    $"\"newKeyword\":{newKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(ArrayTypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteArrayTypeSyntax(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Initializer != default(InitializerExpressionSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteInitializerExpressionSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteImplicitArrayCreationExpressionSyntax(
            StringBuilder builder,
            ImplicitArrayCreationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NewKeyword != default(SyntaxToken))
            {
                var newKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(newKeywordBuilder, syntaxNode.NewKeyword);
                properties.Add(
                    $"\"newKeyword\":{newKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenBracketToken != default(SyntaxToken))
            {
                var openBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBracketTokenBuilder,
                    syntaxNode.OpenBracketToken
                );
                properties.Add(
                    $"\"openBracketToken\":{openBracketTokenBuilder.ToString()}"
                );
            }
            var commas = new List<string>();
            foreach (var node in syntaxNode.Commas)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                commas.Add(innerBuilder.ToString());
            }
            properties.Add($"\"commas\":[{string.Join(",", commas)}]");
            if (syntaxNode.CloseBracketToken != default(SyntaxToken))
            {
                var closeBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBracketTokenBuilder,
                    syntaxNode.CloseBracketToken
                );
                properties.Add(
                    $"\"closeBracketToken\":{closeBracketTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Initializer != default(InitializerExpressionSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteInitializerExpressionSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteStackAllocArrayCreationExpressionSyntax(
            StringBuilder builder,
            StackAllocArrayCreationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.StackAllocKeyword != default(SyntaxToken))
            {
                var stackAllocKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    stackAllocKeywordBuilder,
                    syntaxNode.StackAllocKeyword
                );
                properties.Add(
                    $"\"stackAllocKeyword\":{stackAllocKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Initializer != default(InitializerExpressionSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteInitializerExpressionSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteImplicitStackAllocArrayCreationExpressionSyntax(
            StringBuilder builder,
            ImplicitStackAllocArrayCreationExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.StackAllocKeyword != default(SyntaxToken))
            {
                var stackAllocKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    stackAllocKeywordBuilder,
                    syntaxNode.StackAllocKeyword
                );
                properties.Add(
                    $"\"stackAllocKeyword\":{stackAllocKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenBracketToken != default(SyntaxToken))
            {
                var openBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBracketTokenBuilder,
                    syntaxNode.OpenBracketToken
                );
                properties.Add(
                    $"\"openBracketToken\":{openBracketTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseBracketToken != default(SyntaxToken))
            {
                var closeBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBracketTokenBuilder,
                    syntaxNode.CloseBracketToken
                );
                properties.Add(
                    $"\"closeBracketToken\":{closeBracketTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Initializer != default(InitializerExpressionSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteInitializerExpressionSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteQueryExpressionSyntax(
            StringBuilder builder,
            QueryExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.FromClause != default(FromClauseSyntax))
            {
                var fromClauseBuilder = new StringBuilder();
                WriteFromClauseSyntax(fromClauseBuilder, syntaxNode.FromClause);
                properties.Add(
                    $"\"fromClause\":{fromClauseBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(QueryBodySyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteQueryBodySyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteQueryBodySyntax(
            StringBuilder builder,
            QueryBodySyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var clauses = new List<string>();
            foreach (var node in syntaxNode.Clauses)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                clauses.Add(innerBuilder.ToString());
            }
            properties.Add($"\"clauses\":[{string.Join(",", clauses)}]");
            if (syntaxNode.SelectOrGroup != default(SelectOrGroupClauseSyntax))
            {
                var selectOrGroupBuilder = new StringBuilder();
                WriteSyntaxNode(selectOrGroupBuilder, syntaxNode.SelectOrGroup);
                properties.Add(
                    $"\"selectOrGroup\":{selectOrGroupBuilder.ToString()}"
                );
            }
            if (syntaxNode.Continuation != default(QueryContinuationSyntax))
            {
                var continuationBuilder = new StringBuilder();
                WriteQueryContinuationSyntax(
                    continuationBuilder,
                    syntaxNode.Continuation
                );
                properties.Add(
                    $"\"continuation\":{continuationBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFromClauseSyntax(
            StringBuilder builder,
            FromClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.FromKeyword != default(SyntaxToken))
            {
                var fromKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(fromKeywordBuilder, syntaxNode.FromKeyword);
                properties.Add(
                    $"\"fromKeyword\":{fromKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.InKeyword != default(SyntaxToken))
            {
                var inKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(inKeywordBuilder, syntaxNode.InKeyword);
                properties.Add($"\"inKeyword\":{inKeywordBuilder.ToString()}");
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteLetClauseSyntax(
            StringBuilder builder,
            LetClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.LetKeyword != default(SyntaxToken))
            {
                var letKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(letKeywordBuilder, syntaxNode.LetKeyword);
                properties.Add(
                    $"\"letKeyword\":{letKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.EqualsToken != default(SyntaxToken))
            {
                var equalsTokenBuilder = new StringBuilder();
                WriteSyntaxToken(equalsTokenBuilder, syntaxNode.EqualsToken);
                properties.Add(
                    $"\"equalsToken\":{equalsTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteJoinClauseSyntax(
            StringBuilder builder,
            JoinClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.JoinKeyword != default(SyntaxToken))
            {
                var joinKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(joinKeywordBuilder, syntaxNode.JoinKeyword);
                properties.Add(
                    $"\"joinKeyword\":{joinKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.InKeyword != default(SyntaxToken))
            {
                var inKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(inKeywordBuilder, syntaxNode.InKeyword);
                properties.Add($"\"inKeyword\":{inKeywordBuilder.ToString()}");
            }
            if (syntaxNode.InExpression != default(ExpressionSyntax))
            {
                var inExpressionBuilder = new StringBuilder();
                WriteSyntaxNode(inExpressionBuilder, syntaxNode.InExpression);
                properties.Add(
                    $"\"inExpression\":{inExpressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.OnKeyword != default(SyntaxToken))
            {
                var onKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(onKeywordBuilder, syntaxNode.OnKeyword);
                properties.Add($"\"onKeyword\":{onKeywordBuilder.ToString()}");
            }
            if (syntaxNode.LeftExpression != default(ExpressionSyntax))
            {
                var leftExpressionBuilder = new StringBuilder();
                WriteSyntaxNode(
                    leftExpressionBuilder,
                    syntaxNode.LeftExpression
                );
                properties.Add(
                    $"\"leftExpression\":{leftExpressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.EqualsKeyword != default(SyntaxToken))
            {
                var equalsKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    equalsKeywordBuilder,
                    syntaxNode.EqualsKeyword
                );
                properties.Add(
                    $"\"equalsKeyword\":{equalsKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.RightExpression != default(ExpressionSyntax))
            {
                var rightExpressionBuilder = new StringBuilder();
                WriteSyntaxNode(
                    rightExpressionBuilder,
                    syntaxNode.RightExpression
                );
                properties.Add(
                    $"\"rightExpression\":{rightExpressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.Into != default(JoinIntoClauseSyntax))
            {
                var intoBuilder = new StringBuilder();
                WriteJoinIntoClauseSyntax(intoBuilder, syntaxNode.Into);
                properties.Add($"\"into\":{intoBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteJoinIntoClauseSyntax(
            StringBuilder builder,
            JoinIntoClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.IntoKeyword != default(SyntaxToken))
            {
                var intoKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(intoKeywordBuilder, syntaxNode.IntoKeyword);
                properties.Add(
                    $"\"intoKeyword\":{intoKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteWhereClauseSyntax(
            StringBuilder builder,
            WhereClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.WhereKeyword != default(SyntaxToken))
            {
                var whereKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(whereKeywordBuilder, syntaxNode.WhereKeyword);
                properties.Add(
                    $"\"whereKeyword\":{whereKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteOrderByClauseSyntax(
            StringBuilder builder,
            OrderByClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OrderByKeyword != default(SyntaxToken))
            {
                var orderByKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    orderByKeywordBuilder,
                    syntaxNode.OrderByKeyword
                );
                properties.Add(
                    $"\"orderByKeyword\":{orderByKeywordBuilder.ToString()}"
                );
            }
            var orderings = new List<string>();
            foreach (var node in syntaxNode.Orderings)
            {
                var innerBuilder = new StringBuilder();
                WriteOrderingSyntax(innerBuilder, node);
                orderings.Add(innerBuilder.ToString());
            }
            properties.Add($"\"orderings\":[{string.Join(",", orderings)}]");
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteOrderingSyntax(
            StringBuilder builder,
            OrderingSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.AscendingOrDescendingKeyword != default(SyntaxToken))
            {
                var ascendingOrDescendingKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    ascendingOrDescendingKeywordBuilder,
                    syntaxNode.AscendingOrDescendingKeyword
                );
                properties.Add(
                    $"\"ascendingOrDescendingKeyword\":{ascendingOrDescendingKeywordBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSelectClauseSyntax(
            StringBuilder builder,
            SelectClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.SelectKeyword != default(SyntaxToken))
            {
                var selectKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    selectKeywordBuilder,
                    syntaxNode.SelectKeyword
                );
                properties.Add(
                    $"\"selectKeyword\":{selectKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteGroupClauseSyntax(
            StringBuilder builder,
            GroupClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.GroupKeyword != default(SyntaxToken))
            {
                var groupKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(groupKeywordBuilder, syntaxNode.GroupKeyword);
                properties.Add(
                    $"\"groupKeyword\":{groupKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.GroupExpression != default(ExpressionSyntax))
            {
                var groupExpressionBuilder = new StringBuilder();
                WriteSyntaxNode(
                    groupExpressionBuilder,
                    syntaxNode.GroupExpression
                );
                properties.Add(
                    $"\"groupExpression\":{groupExpressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.ByKeyword != default(SyntaxToken))
            {
                var byKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(byKeywordBuilder, syntaxNode.ByKeyword);
                properties.Add($"\"byKeyword\":{byKeywordBuilder.ToString()}");
            }
            if (syntaxNode.ByExpression != default(ExpressionSyntax))
            {
                var byExpressionBuilder = new StringBuilder();
                WriteSyntaxNode(byExpressionBuilder, syntaxNode.ByExpression);
                properties.Add(
                    $"\"byExpression\":{byExpressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteQueryContinuationSyntax(
            StringBuilder builder,
            QueryContinuationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.IntoKeyword != default(SyntaxToken))
            {
                var intoKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(intoKeywordBuilder, syntaxNode.IntoKeyword);
                properties.Add(
                    $"\"intoKeyword\":{intoKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(QueryBodySyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteQueryBodySyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteOmittedArraySizeExpressionSyntax(
            StringBuilder builder,
            OmittedArraySizeExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (
                syntaxNode.OmittedArraySizeExpressionToken != default(SyntaxToken)
            )
            {
                var omittedArraySizeExpressionTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    omittedArraySizeExpressionTokenBuilder,
                    syntaxNode.OmittedArraySizeExpressionToken
                );
                properties.Add(
                    $"\"omittedArraySizeExpressionToken\":{omittedArraySizeExpressionTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteInterpolatedStringExpressionSyntax(
            StringBuilder builder,
            InterpolatedStringExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.StringStartToken != default(SyntaxToken))
            {
                var stringStartTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    stringStartTokenBuilder,
                    syntaxNode.StringStartToken
                );
                properties.Add(
                    $"\"stringStartToken\":{stringStartTokenBuilder.ToString()}"
                );
            }
            var contents = new List<string>();
            foreach (var node in syntaxNode.Contents)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                contents.Add(innerBuilder.ToString());
            }
            properties.Add($"\"contents\":[{string.Join(",", contents)}]");
            if (syntaxNode.StringEndToken != default(SyntaxToken))
            {
                var stringEndTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    stringEndTokenBuilder,
                    syntaxNode.StringEndToken
                );
                properties.Add(
                    $"\"stringEndToken\":{stringEndTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteIsPatternExpressionSyntax(
            StringBuilder builder,
            IsPatternExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.IsKeyword != default(SyntaxToken))
            {
                var isKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(isKeywordBuilder, syntaxNode.IsKeyword);
                properties.Add($"\"isKeyword\":{isKeywordBuilder.ToString()}");
            }
            if (syntaxNode.Pattern != default(PatternSyntax))
            {
                var patternBuilder = new StringBuilder();
                WriteSyntaxNode(patternBuilder, syntaxNode.Pattern);
                properties.Add($"\"pattern\":{patternBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteThrowExpressionSyntax(
            StringBuilder builder,
            ThrowExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ThrowKeyword != default(SyntaxToken))
            {
                var throwKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(throwKeywordBuilder, syntaxNode.ThrowKeyword);
                properties.Add(
                    $"\"throwKeyword\":{throwKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteWhenClauseSyntax(
            StringBuilder builder,
            WhenClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.WhenKeyword != default(SyntaxToken))
            {
                var whenKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(whenKeywordBuilder, syntaxNode.WhenKeyword);
                properties.Add(
                    $"\"whenKeyword\":{whenKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDiscardPatternSyntax(
            StringBuilder builder,
            DiscardPatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.UnderscoreToken != default(SyntaxToken))
            {
                var underscoreTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    underscoreTokenBuilder,
                    syntaxNode.UnderscoreToken
                );
                properties.Add(
                    $"\"underscoreToken\":{underscoreTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDeclarationPatternSyntax(
            StringBuilder builder,
            DeclarationPatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Designation != default(VariableDesignationSyntax))
            {
                var designationBuilder = new StringBuilder();
                WriteSyntaxNode(designationBuilder, syntaxNode.Designation);
                properties.Add(
                    $"\"designation\":{designationBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteVarPatternSyntax(
            StringBuilder builder,
            VarPatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.VarKeyword != default(SyntaxToken))
            {
                var varKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(varKeywordBuilder, syntaxNode.VarKeyword);
                properties.Add(
                    $"\"varKeyword\":{varKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Designation != default(VariableDesignationSyntax))
            {
                var designationBuilder = new StringBuilder();
                WriteSyntaxNode(designationBuilder, syntaxNode.Designation);
                properties.Add(
                    $"\"designation\":{designationBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRecursivePatternSyntax(
            StringBuilder builder,
            RecursivePatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (
                syntaxNode.PositionalPatternClause != default(PositionalPatternClauseSyntax)
            )
            {
                var positionalPatternClauseBuilder = new StringBuilder();
                WritePositionalPatternClauseSyntax(
                    positionalPatternClauseBuilder,
                    syntaxNode.PositionalPatternClause
                );
                properties.Add(
                    $"\"positionalPatternClause\":{positionalPatternClauseBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.PropertyPatternClause != default(PropertyPatternClauseSyntax)
            )
            {
                var propertyPatternClauseBuilder = new StringBuilder();
                WritePropertyPatternClauseSyntax(
                    propertyPatternClauseBuilder,
                    syntaxNode.PropertyPatternClause
                );
                properties.Add(
                    $"\"propertyPatternClause\":{propertyPatternClauseBuilder.ToString()}"
                );
            }
            if (syntaxNode.Designation != default(VariableDesignationSyntax))
            {
                var designationBuilder = new StringBuilder();
                WriteSyntaxNode(designationBuilder, syntaxNode.Designation);
                properties.Add(
                    $"\"designation\":{designationBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePositionalPatternClauseSyntax(
            StringBuilder builder,
            PositionalPatternClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            var subpatterns = new List<string>();
            foreach (var node in syntaxNode.Subpatterns)
            {
                var innerBuilder = new StringBuilder();
                WriteSubpatternSyntax(innerBuilder, node);
                subpatterns.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"subpatterns\":[{string.Join(",", subpatterns)}]"
            );
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePropertyPatternClauseSyntax(
            StringBuilder builder,
            PropertyPatternClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var subpatterns = new List<string>();
            foreach (var node in syntaxNode.Subpatterns)
            {
                var innerBuilder = new StringBuilder();
                WriteSubpatternSyntax(innerBuilder, node);
                subpatterns.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"subpatterns\":[{string.Join(",", subpatterns)}]"
            );
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSubpatternSyntax(
            StringBuilder builder,
            SubpatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NameColon != default(NameColonSyntax))
            {
                var nameColonBuilder = new StringBuilder();
                WriteNameColonSyntax(nameColonBuilder, syntaxNode.NameColon);
                properties.Add($"\"nameColon\":{nameColonBuilder.ToString()}");
            }
            if (syntaxNode.Pattern != default(PatternSyntax))
            {
                var patternBuilder = new StringBuilder();
                WriteSyntaxNode(patternBuilder, syntaxNode.Pattern);
                properties.Add($"\"pattern\":{patternBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteConstantPatternSyntax(
            StringBuilder builder,
            ConstantPatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteParenthesizedPatternSyntax(
            StringBuilder builder,
            ParenthesizedPatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Pattern != default(PatternSyntax))
            {
                var patternBuilder = new StringBuilder();
                WriteSyntaxNode(patternBuilder, syntaxNode.Pattern);
                properties.Add($"\"pattern\":{patternBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRelationalPatternSyntax(
            StringBuilder builder,
            RelationalPatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTypePatternSyntax(
            StringBuilder builder,
            TypePatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBinaryPatternSyntax(
            StringBuilder builder,
            BinaryPatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Left != default(PatternSyntax))
            {
                var leftBuilder = new StringBuilder();
                WriteSyntaxNode(leftBuilder, syntaxNode.Left);
                properties.Add($"\"left\":{leftBuilder.ToString()}");
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Right != default(PatternSyntax))
            {
                var rightBuilder = new StringBuilder();
                WriteSyntaxNode(rightBuilder, syntaxNode.Right);
                properties.Add($"\"right\":{rightBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteUnaryPatternSyntax(
            StringBuilder builder,
            UnaryPatternSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Pattern != default(PatternSyntax))
            {
                var patternBuilder = new StringBuilder();
                WriteSyntaxNode(patternBuilder, syntaxNode.Pattern);
                properties.Add($"\"pattern\":{patternBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteInterpolatedStringTextSyntax(
            StringBuilder builder,
            InterpolatedStringTextSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.TextToken != default(SyntaxToken))
            {
                var textTokenBuilder = new StringBuilder();
                WriteSyntaxToken(textTokenBuilder, syntaxNode.TextToken);
                properties.Add($"\"textToken\":{textTokenBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteInterpolationSyntax(
            StringBuilder builder,
            InterpolationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.AlignmentClause != default(InterpolationAlignmentClauseSyntax)
            )
            {
                var alignmentClauseBuilder = new StringBuilder();
                WriteInterpolationAlignmentClauseSyntax(
                    alignmentClauseBuilder,
                    syntaxNode.AlignmentClause
                );
                properties.Add(
                    $"\"alignmentClause\":{alignmentClauseBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.FormatClause != default(InterpolationFormatClauseSyntax)
            )
            {
                var formatClauseBuilder = new StringBuilder();
                WriteInterpolationFormatClauseSyntax(
                    formatClauseBuilder,
                    syntaxNode.FormatClause
                );
                properties.Add(
                    $"\"formatClause\":{formatClauseBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteInterpolationAlignmentClauseSyntax(
            StringBuilder builder,
            InterpolationAlignmentClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.CommaToken != default(SyntaxToken))
            {
                var commaTokenBuilder = new StringBuilder();
                WriteSyntaxToken(commaTokenBuilder, syntaxNode.CommaToken);
                properties.Add(
                    $"\"commaToken\":{commaTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Value != default(ExpressionSyntax))
            {
                var valueBuilder = new StringBuilder();
                WriteSyntaxNode(valueBuilder, syntaxNode.Value);
                properties.Add($"\"value\":{valueBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteInterpolationFormatClauseSyntax(
            StringBuilder builder,
            InterpolationFormatClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.FormatStringToken != default(SyntaxToken))
            {
                var formatStringTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    formatStringTokenBuilder,
                    syntaxNode.FormatStringToken
                );
                properties.Add(
                    $"\"formatStringToken\":{formatStringTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteGlobalStatementSyntax(
            StringBuilder builder,
            GlobalStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBlockSyntax(
            StringBuilder builder,
            BlockSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var statements = new List<string>();
            foreach (var node in syntaxNode.Statements)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                statements.Add(innerBuilder.ToString());
            }
            properties.Add($"\"statements\":[{string.Join(",", statements)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteLocalFunctionStatementSyntax(
            StringBuilder builder,
            LocalFunctionStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.ReturnType != default(TypeSyntax))
            {
                var returnTypeBuilder = new StringBuilder();
                WriteSyntaxNode(returnTypeBuilder, syntaxNode.ReturnType);
                properties.Add(
                    $"\"returnType\":{returnTypeBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.TypeParameterList != default(TypeParameterListSyntax)
            )
            {
                var typeParameterListBuilder = new StringBuilder();
                WriteTypeParameterListSyntax(
                    typeParameterListBuilder,
                    syntaxNode.TypeParameterList
                );
                properties.Add(
                    $"\"typeParameterList\":{typeParameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            var constraintClauses = new List<string>();
            foreach (var node in syntaxNode.ConstraintClauses)
            {
                var innerBuilder = new StringBuilder();
                WriteTypeParameterConstraintClauseSyntax(innerBuilder, node);
                constraintClauses.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"constraintClauses\":[{string.Join(",", constraintClauses)}]"
            );
            if (syntaxNode.Body != default(BlockSyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteBlockSyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteLocalDeclarationStatementSyntax(
            StringBuilder builder,
            LocalDeclarationStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.AwaitKeyword != default(SyntaxToken))
            {
                var awaitKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(awaitKeywordBuilder, syntaxNode.AwaitKeyword);
                properties.Add(
                    $"\"awaitKeyword\":{awaitKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.UsingKeyword != default(SyntaxToken))
            {
                var usingKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(usingKeywordBuilder, syntaxNode.UsingKeyword);
                properties.Add(
                    $"\"usingKeyword\":{usingKeywordBuilder.ToString()}"
                );
            }
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Declaration != default(VariableDeclarationSyntax))
            {
                var declarationBuilder = new StringBuilder();
                WriteVariableDeclarationSyntax(
                    declarationBuilder,
                    syntaxNode.Declaration
                );
                properties.Add(
                    $"\"declaration\":{declarationBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isConst", syntaxNode.IsConst));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteVariableDeclarationSyntax(
            StringBuilder builder,
            VariableDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            var variables = new List<string>();
            foreach (var node in syntaxNode.Variables)
            {
                var innerBuilder = new StringBuilder();
                WriteVariableDeclaratorSyntax(innerBuilder, node);
                variables.Add(innerBuilder.ToString());
            }
            properties.Add($"\"variables\":[{string.Join(",", variables)}]");
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteVariableDeclaratorSyntax(
            StringBuilder builder,
            VariableDeclaratorSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.ArgumentList != default(BracketedArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteBracketedArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            if (syntaxNode.Initializer != default(EqualsValueClauseSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteEqualsValueClauseSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteEqualsValueClauseSyntax(
            StringBuilder builder,
            EqualsValueClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.EqualsToken != default(SyntaxToken))
            {
                var equalsTokenBuilder = new StringBuilder();
                WriteSyntaxToken(equalsTokenBuilder, syntaxNode.EqualsToken);
                properties.Add(
                    $"\"equalsToken\":{equalsTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Value != default(ExpressionSyntax))
            {
                var valueBuilder = new StringBuilder();
                WriteSyntaxNode(valueBuilder, syntaxNode.Value);
                properties.Add($"\"value\":{valueBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSingleVariableDesignationSyntax(
            StringBuilder builder,
            SingleVariableDesignationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDiscardDesignationSyntax(
            StringBuilder builder,
            DiscardDesignationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.UnderscoreToken != default(SyntaxToken))
            {
                var underscoreTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    underscoreTokenBuilder,
                    syntaxNode.UnderscoreToken
                );
                properties.Add(
                    $"\"underscoreToken\":{underscoreTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteParenthesizedVariableDesignationSyntax(
            StringBuilder builder,
            ParenthesizedVariableDesignationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            var variables = new List<string>();
            foreach (var node in syntaxNode.Variables)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                variables.Add(innerBuilder.ToString());
            }
            properties.Add($"\"variables\":[{string.Join(",", variables)}]");
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteExpressionStatementSyntax(
            StringBuilder builder,
            ExpressionStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(
                WriteBoolean(
                    "allowsAnyExpression",
                    syntaxNode.AllowsAnyExpression
                )
            );
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteEmptyStatementSyntax(
            StringBuilder builder,
            EmptyStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteLabeledStatementSyntax(
            StringBuilder builder,
            LabeledStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteGotoStatementSyntax(
            StringBuilder builder,
            GotoStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.GotoKeyword != default(SyntaxToken))
            {
                var gotoKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(gotoKeywordBuilder, syntaxNode.GotoKeyword);
                properties.Add(
                    $"\"gotoKeyword\":{gotoKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.CaseOrDefaultKeyword != default(SyntaxToken))
            {
                var caseOrDefaultKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    caseOrDefaultKeywordBuilder,
                    syntaxNode.CaseOrDefaultKeyword
                );
                properties.Add(
                    $"\"caseOrDefaultKeyword\":{caseOrDefaultKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBreakStatementSyntax(
            StringBuilder builder,
            BreakStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.BreakKeyword != default(SyntaxToken))
            {
                var breakKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(breakKeywordBuilder, syntaxNode.BreakKeyword);
                properties.Add(
                    $"\"breakKeyword\":{breakKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteContinueStatementSyntax(
            StringBuilder builder,
            ContinueStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.ContinueKeyword != default(SyntaxToken))
            {
                var continueKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    continueKeywordBuilder,
                    syntaxNode.ContinueKeyword
                );
                properties.Add(
                    $"\"continueKeyword\":{continueKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteReturnStatementSyntax(
            StringBuilder builder,
            ReturnStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.ReturnKeyword != default(SyntaxToken))
            {
                var returnKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    returnKeywordBuilder,
                    syntaxNode.ReturnKeyword
                );
                properties.Add(
                    $"\"returnKeyword\":{returnKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteThrowStatementSyntax(
            StringBuilder builder,
            ThrowStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.ThrowKeyword != default(SyntaxToken))
            {
                var throwKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(throwKeywordBuilder, syntaxNode.ThrowKeyword);
                properties.Add(
                    $"\"throwKeyword\":{throwKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteYieldStatementSyntax(
            StringBuilder builder,
            YieldStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.YieldKeyword != default(SyntaxToken))
            {
                var yieldKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(yieldKeywordBuilder, syntaxNode.YieldKeyword);
                properties.Add(
                    $"\"yieldKeyword\":{yieldKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ReturnOrBreakKeyword != default(SyntaxToken))
            {
                var returnOrBreakKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    returnOrBreakKeywordBuilder,
                    syntaxNode.ReturnOrBreakKeyword
                );
                properties.Add(
                    $"\"returnOrBreakKeyword\":{returnOrBreakKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteWhileStatementSyntax(
            StringBuilder builder,
            WhileStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.WhileKeyword != default(SyntaxToken))
            {
                var whileKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(whileKeywordBuilder, syntaxNode.WhileKeyword);
                properties.Add(
                    $"\"whileKeyword\":{whileKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDoStatementSyntax(
            StringBuilder builder,
            DoStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.DoKeyword != default(SyntaxToken))
            {
                var doKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(doKeywordBuilder, syntaxNode.DoKeyword);
                properties.Add($"\"doKeyword\":{doKeywordBuilder.ToString()}");
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            if (syntaxNode.WhileKeyword != default(SyntaxToken))
            {
                var whileKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(whileKeywordBuilder, syntaxNode.WhileKeyword);
                properties.Add(
                    $"\"whileKeyword\":{whileKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteForStatementSyntax(
            StringBuilder builder,
            ForStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.ForKeyword != default(SyntaxToken))
            {
                var forKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(forKeywordBuilder, syntaxNode.ForKeyword);
                properties.Add(
                    $"\"forKeyword\":{forKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Declaration != default(VariableDeclarationSyntax))
            {
                var declarationBuilder = new StringBuilder();
                WriteVariableDeclarationSyntax(
                    declarationBuilder,
                    syntaxNode.Declaration
                );
                properties.Add(
                    $"\"declaration\":{declarationBuilder.ToString()}"
                );
            }
            var initializers = new List<string>();
            foreach (var node in syntaxNode.Initializers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                initializers.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"initializers\":[{string.Join(",", initializers)}]"
            );
            if (syntaxNode.FirstSemicolonToken != default(SyntaxToken))
            {
                var firstSemicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    firstSemicolonTokenBuilder,
                    syntaxNode.FirstSemicolonToken
                );
                properties.Add(
                    $"\"firstSemicolonToken\":{firstSemicolonTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            if (syntaxNode.SecondSemicolonToken != default(SyntaxToken))
            {
                var secondSemicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    secondSemicolonTokenBuilder,
                    syntaxNode.SecondSemicolonToken
                );
                properties.Add(
                    $"\"secondSemicolonToken\":{secondSemicolonTokenBuilder.ToString()}"
                );
            }
            var incrementors = new List<string>();
            foreach (var node in syntaxNode.Incrementors)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                incrementors.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"incrementors\":[{string.Join(",", incrementors)}]"
            );
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteForEachStatementSyntax(
            StringBuilder builder,
            ForEachStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.AwaitKeyword != default(SyntaxToken))
            {
                var awaitKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(awaitKeywordBuilder, syntaxNode.AwaitKeyword);
                properties.Add(
                    $"\"awaitKeyword\":{awaitKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ForEachKeyword != default(SyntaxToken))
            {
                var forEachKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    forEachKeywordBuilder,
                    syntaxNode.ForEachKeyword
                );
                properties.Add(
                    $"\"forEachKeyword\":{forEachKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.InKeyword != default(SyntaxToken))
            {
                var inKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(inKeywordBuilder, syntaxNode.InKeyword);
                properties.Add($"\"inKeyword\":{inKeywordBuilder.ToString()}");
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteForEachVariableStatementSyntax(
            StringBuilder builder,
            ForEachVariableStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.AwaitKeyword != default(SyntaxToken))
            {
                var awaitKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(awaitKeywordBuilder, syntaxNode.AwaitKeyword);
                properties.Add(
                    $"\"awaitKeyword\":{awaitKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ForEachKeyword != default(SyntaxToken))
            {
                var forEachKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    forEachKeywordBuilder,
                    syntaxNode.ForEachKeyword
                );
                properties.Add(
                    $"\"forEachKeyword\":{forEachKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Variable != default(ExpressionSyntax))
            {
                var variableBuilder = new StringBuilder();
                WriteSyntaxNode(variableBuilder, syntaxNode.Variable);
                properties.Add($"\"variable\":{variableBuilder.ToString()}");
            }
            if (syntaxNode.InKeyword != default(SyntaxToken))
            {
                var inKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(inKeywordBuilder, syntaxNode.InKeyword);
                properties.Add($"\"inKeyword\":{inKeywordBuilder.ToString()}");
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteUsingStatementSyntax(
            StringBuilder builder,
            UsingStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.AwaitKeyword != default(SyntaxToken))
            {
                var awaitKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(awaitKeywordBuilder, syntaxNode.AwaitKeyword);
                properties.Add(
                    $"\"awaitKeyword\":{awaitKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.UsingKeyword != default(SyntaxToken))
            {
                var usingKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(usingKeywordBuilder, syntaxNode.UsingKeyword);
                properties.Add(
                    $"\"usingKeyword\":{usingKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Declaration != default(VariableDeclarationSyntax))
            {
                var declarationBuilder = new StringBuilder();
                WriteVariableDeclarationSyntax(
                    declarationBuilder,
                    syntaxNode.Declaration
                );
                properties.Add(
                    $"\"declaration\":{declarationBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFixedStatementSyntax(
            StringBuilder builder,
            FixedStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.FixedKeyword != default(SyntaxToken))
            {
                var fixedKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(fixedKeywordBuilder, syntaxNode.FixedKeyword);
                properties.Add(
                    $"\"fixedKeyword\":{fixedKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Declaration != default(VariableDeclarationSyntax))
            {
                var declarationBuilder = new StringBuilder();
                WriteVariableDeclarationSyntax(
                    declarationBuilder,
                    syntaxNode.Declaration
                );
                properties.Add(
                    $"\"declaration\":{declarationBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCheckedStatementSyntax(
            StringBuilder builder,
            CheckedStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.Block != default(BlockSyntax))
            {
                var blockBuilder = new StringBuilder();
                WriteBlockSyntax(blockBuilder, syntaxNode.Block);
                properties.Add($"\"block\":{blockBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteUnsafeStatementSyntax(
            StringBuilder builder,
            UnsafeStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.UnsafeKeyword != default(SyntaxToken))
            {
                var unsafeKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    unsafeKeywordBuilder,
                    syntaxNode.UnsafeKeyword
                );
                properties.Add(
                    $"\"unsafeKeyword\":{unsafeKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Block != default(BlockSyntax))
            {
                var blockBuilder = new StringBuilder();
                WriteBlockSyntax(blockBuilder, syntaxNode.Block);
                properties.Add($"\"block\":{blockBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteLockStatementSyntax(
            StringBuilder builder,
            LockStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.LockKeyword != default(SyntaxToken))
            {
                var lockKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(lockKeywordBuilder, syntaxNode.LockKeyword);
                properties.Add(
                    $"\"lockKeyword\":{lockKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteIfStatementSyntax(
            StringBuilder builder,
            IfStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.IfKeyword != default(SyntaxToken))
            {
                var ifKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(ifKeywordBuilder, syntaxNode.IfKeyword);
                properties.Add($"\"ifKeyword\":{ifKeywordBuilder.ToString()}");
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            if (syntaxNode.Else != default(ElseClauseSyntax))
            {
                var elseBuilder = new StringBuilder();
                WriteElseClauseSyntax(elseBuilder, syntaxNode.Else);
                properties.Add($"\"else\":{elseBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteElseClauseSyntax(
            StringBuilder builder,
            ElseClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ElseKeyword != default(SyntaxToken))
            {
                var elseKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(elseKeywordBuilder, syntaxNode.ElseKeyword);
                properties.Add(
                    $"\"elseKeyword\":{elseKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Statement != default(StatementSyntax))
            {
                var statementBuilder = new StringBuilder();
                WriteSyntaxNode(statementBuilder, syntaxNode.Statement);
                properties.Add($"\"statement\":{statementBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSwitchStatementSyntax(
            StringBuilder builder,
            SwitchStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.SwitchKeyword != default(SyntaxToken))
            {
                var switchKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    switchKeywordBuilder,
                    syntaxNode.SwitchKeyword
                );
                properties.Add(
                    $"\"switchKeyword\":{switchKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var sections = new List<string>();
            foreach (var node in syntaxNode.Sections)
            {
                var innerBuilder = new StringBuilder();
                WriteSwitchSectionSyntax(innerBuilder, node);
                sections.Add(innerBuilder.ToString());
            }
            properties.Add($"\"sections\":[{string.Join(",", sections)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSwitchSectionSyntax(
            StringBuilder builder,
            SwitchSectionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var labels = new List<string>();
            foreach (var node in syntaxNode.Labels)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                labels.Add(innerBuilder.ToString());
            }
            properties.Add($"\"labels\":[{string.Join(",", labels)}]");
            var statements = new List<string>();
            foreach (var node in syntaxNode.Statements)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                statements.Add(innerBuilder.ToString());
            }
            properties.Add($"\"statements\":[{string.Join(",", statements)}]");
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCasePatternSwitchLabelSyntax(
            StringBuilder builder,
            CasePatternSwitchLabelSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.Pattern != default(PatternSyntax))
            {
                var patternBuilder = new StringBuilder();
                WriteSyntaxNode(patternBuilder, syntaxNode.Pattern);
                properties.Add($"\"pattern\":{patternBuilder.ToString()}");
            }
            if (syntaxNode.WhenClause != default(WhenClauseSyntax))
            {
                var whenClauseBuilder = new StringBuilder();
                WriteWhenClauseSyntax(whenClauseBuilder, syntaxNode.WhenClause);
                properties.Add(
                    $"\"whenClause\":{whenClauseBuilder.ToString()}"
                );
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCaseSwitchLabelSyntax(
            StringBuilder builder,
            CaseSwitchLabelSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.Value != default(ExpressionSyntax))
            {
                var valueBuilder = new StringBuilder();
                WriteSyntaxNode(valueBuilder, syntaxNode.Value);
                properties.Add($"\"value\":{valueBuilder.ToString()}");
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDefaultSwitchLabelSyntax(
            StringBuilder builder,
            DefaultSwitchLabelSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSwitchExpressionSyntax(
            StringBuilder builder,
            SwitchExpressionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.GoverningExpression != default(ExpressionSyntax))
            {
                var governingExpressionBuilder = new StringBuilder();
                WriteSyntaxNode(
                    governingExpressionBuilder,
                    syntaxNode.GoverningExpression
                );
                properties.Add(
                    $"\"governingExpression\":{governingExpressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.SwitchKeyword != default(SyntaxToken))
            {
                var switchKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    switchKeywordBuilder,
                    syntaxNode.SwitchKeyword
                );
                properties.Add(
                    $"\"switchKeyword\":{switchKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var arms = new List<string>();
            foreach (var node in syntaxNode.Arms)
            {
                var innerBuilder = new StringBuilder();
                WriteSwitchExpressionArmSyntax(innerBuilder, node);
                arms.Add(innerBuilder.ToString());
            }
            properties.Add($"\"arms\":[{string.Join(",", arms)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSwitchExpressionArmSyntax(
            StringBuilder builder,
            SwitchExpressionArmSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Pattern != default(PatternSyntax))
            {
                var patternBuilder = new StringBuilder();
                WriteSyntaxNode(patternBuilder, syntaxNode.Pattern);
                properties.Add($"\"pattern\":{patternBuilder.ToString()}");
            }
            if (syntaxNode.WhenClause != default(WhenClauseSyntax))
            {
                var whenClauseBuilder = new StringBuilder();
                WriteWhenClauseSyntax(whenClauseBuilder, syntaxNode.WhenClause);
                properties.Add(
                    $"\"whenClause\":{whenClauseBuilder.ToString()}"
                );
            }
            if (syntaxNode.EqualsGreaterThanToken != default(SyntaxToken))
            {
                var equalsGreaterThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    equalsGreaterThanTokenBuilder,
                    syntaxNode.EqualsGreaterThanToken
                );
                properties.Add(
                    $"\"equalsGreaterThanToken\":{equalsGreaterThanTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTryStatementSyntax(
            StringBuilder builder,
            TryStatementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.TryKeyword != default(SyntaxToken))
            {
                var tryKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(tryKeywordBuilder, syntaxNode.TryKeyword);
                properties.Add(
                    $"\"tryKeyword\":{tryKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Block != default(BlockSyntax))
            {
                var blockBuilder = new StringBuilder();
                WriteBlockSyntax(blockBuilder, syntaxNode.Block);
                properties.Add($"\"block\":{blockBuilder.ToString()}");
            }
            var catches = new List<string>();
            foreach (var node in syntaxNode.Catches)
            {
                var innerBuilder = new StringBuilder();
                WriteCatchClauseSyntax(innerBuilder, node);
                catches.Add(innerBuilder.ToString());
            }
            properties.Add($"\"catches\":[{string.Join(",", catches)}]");
            if (syntaxNode.Finally != default(FinallyClauseSyntax))
            {
                var finallyBuilder = new StringBuilder();
                WriteFinallyClauseSyntax(finallyBuilder, syntaxNode.Finally);
                properties.Add($"\"finally\":{finallyBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCatchClauseSyntax(
            StringBuilder builder,
            CatchClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.CatchKeyword != default(SyntaxToken))
            {
                var catchKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(catchKeywordBuilder, syntaxNode.CatchKeyword);
                properties.Add(
                    $"\"catchKeyword\":{catchKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Declaration != default(CatchDeclarationSyntax))
            {
                var declarationBuilder = new StringBuilder();
                WriteCatchDeclarationSyntax(
                    declarationBuilder,
                    syntaxNode.Declaration
                );
                properties.Add(
                    $"\"declaration\":{declarationBuilder.ToString()}"
                );
            }
            if (syntaxNode.Filter != default(CatchFilterClauseSyntax))
            {
                var filterBuilder = new StringBuilder();
                WriteCatchFilterClauseSyntax(filterBuilder, syntaxNode.Filter);
                properties.Add($"\"filter\":{filterBuilder.ToString()}");
            }
            if (syntaxNode.Block != default(BlockSyntax))
            {
                var blockBuilder = new StringBuilder();
                WriteBlockSyntax(blockBuilder, syntaxNode.Block);
                properties.Add($"\"block\":{blockBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCatchDeclarationSyntax(
            StringBuilder builder,
            CatchDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCatchFilterClauseSyntax(
            StringBuilder builder,
            CatchFilterClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.WhenKeyword != default(SyntaxToken))
            {
                var whenKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(whenKeywordBuilder, syntaxNode.WhenKeyword);
                properties.Add(
                    $"\"whenKeyword\":{whenKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.FilterExpression != default(ExpressionSyntax))
            {
                var filterExpressionBuilder = new StringBuilder();
                WriteSyntaxNode(
                    filterExpressionBuilder,
                    syntaxNode.FilterExpression
                );
                properties.Add(
                    $"\"filterExpression\":{filterExpressionBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFinallyClauseSyntax(
            StringBuilder builder,
            FinallyClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.FinallyKeyword != default(SyntaxToken))
            {
                var finallyKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    finallyKeywordBuilder,
                    syntaxNode.FinallyKeyword
                );
                properties.Add(
                    $"\"finallyKeyword\":{finallyKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Block != default(BlockSyntax))
            {
                var blockBuilder = new StringBuilder();
                WriteBlockSyntax(blockBuilder, syntaxNode.Block);
                properties.Add($"\"block\":{blockBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCompilationUnitSyntax(
            StringBuilder builder,
            CompilationUnitSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var externs = new List<string>();
            foreach (var node in syntaxNode.Externs)
            {
                var innerBuilder = new StringBuilder();
                WriteExternAliasDirectiveSyntax(innerBuilder, node);
                externs.Add(innerBuilder.ToString());
            }
            properties.Add($"\"externs\":[{string.Join(",", externs)}]");
            var usings = new List<string>();
            foreach (var node in syntaxNode.Usings)
            {
                var innerBuilder = new StringBuilder();
                WriteUsingDirectiveSyntax(innerBuilder, node);
                usings.Add(innerBuilder.ToString());
            }
            properties.Add($"\"usings\":[{string.Join(",", usings)}]");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var members = new List<string>();
            foreach (var node in syntaxNode.Members)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                members.Add(innerBuilder.ToString());
            }
            properties.Add($"\"members\":[{string.Join(",", members)}]");
            if (syntaxNode.EndOfFileToken != default(SyntaxToken))
            {
                var endOfFileTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfFileTokenBuilder,
                    syntaxNode.EndOfFileToken
                );
                properties.Add(
                    $"\"endOfFileToken\":{endOfFileTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteExternAliasDirectiveSyntax(
            StringBuilder builder,
            ExternAliasDirectiveSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ExternKeyword != default(SyntaxToken))
            {
                var externKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    externKeywordBuilder,
                    syntaxNode.ExternKeyword
                );
                properties.Add(
                    $"\"externKeyword\":{externKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.AliasKeyword != default(SyntaxToken))
            {
                var aliasKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(aliasKeywordBuilder, syntaxNode.AliasKeyword);
                properties.Add(
                    $"\"aliasKeyword\":{aliasKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteUsingDirectiveSyntax(
            StringBuilder builder,
            UsingDirectiveSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.UsingKeyword != default(SyntaxToken))
            {
                var usingKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(usingKeywordBuilder, syntaxNode.UsingKeyword);
                properties.Add(
                    $"\"usingKeyword\":{usingKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.StaticKeyword != default(SyntaxToken))
            {
                var staticKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    staticKeywordBuilder,
                    syntaxNode.StaticKeyword
                );
                properties.Add(
                    $"\"staticKeyword\":{staticKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Alias != default(NameEqualsSyntax))
            {
                var aliasBuilder = new StringBuilder();
                WriteNameEqualsSyntax(aliasBuilder, syntaxNode.Alias);
                properties.Add($"\"alias\":{aliasBuilder.ToString()}");
            }
            if (syntaxNode.Name != default(NameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxNode(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteNamespaceDeclarationSyntax(
            StringBuilder builder,
            NamespaceDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.NamespaceKeyword != default(SyntaxToken))
            {
                var namespaceKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    namespaceKeywordBuilder,
                    syntaxNode.NamespaceKeyword
                );
                properties.Add(
                    $"\"namespaceKeyword\":{namespaceKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(NameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxNode(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var externs = new List<string>();
            foreach (var node in syntaxNode.Externs)
            {
                var innerBuilder = new StringBuilder();
                WriteExternAliasDirectiveSyntax(innerBuilder, node);
                externs.Add(innerBuilder.ToString());
            }
            properties.Add($"\"externs\":[{string.Join(",", externs)}]");
            var usings = new List<string>();
            foreach (var node in syntaxNode.Usings)
            {
                var innerBuilder = new StringBuilder();
                WriteUsingDirectiveSyntax(innerBuilder, node);
                usings.Add(innerBuilder.ToString());
            }
            properties.Add($"\"usings\":[{string.Join(",", usings)}]");
            var members = new List<string>();
            foreach (var node in syntaxNode.Members)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                members.Add(innerBuilder.ToString());
            }
            properties.Add($"\"members\":[{string.Join(",", members)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAttributeListSyntax(
            StringBuilder builder,
            AttributeListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBracketToken != default(SyntaxToken))
            {
                var openBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBracketTokenBuilder,
                    syntaxNode.OpenBracketToken
                );
                properties.Add(
                    $"\"openBracketToken\":{openBracketTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Target != default(AttributeTargetSpecifierSyntax))
            {
                var targetBuilder = new StringBuilder();
                WriteAttributeTargetSpecifierSyntax(
                    targetBuilder,
                    syntaxNode.Target
                );
                properties.Add($"\"target\":{targetBuilder.ToString()}");
            }
            var attributes = new List<string>();
            foreach (var node in syntaxNode.Attributes)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeSyntax(innerBuilder, node);
                attributes.Add(innerBuilder.ToString());
            }
            properties.Add($"\"attributes\":[{string.Join(",", attributes)}]");
            if (syntaxNode.CloseBracketToken != default(SyntaxToken))
            {
                var closeBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBracketTokenBuilder,
                    syntaxNode.CloseBracketToken
                );
                properties.Add(
                    $"\"closeBracketToken\":{closeBracketTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAttributeTargetSpecifierSyntax(
            StringBuilder builder,
            AttributeTargetSpecifierSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAttributeSyntax(
            StringBuilder builder,
            AttributeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(NameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxNode(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.ArgumentList != default(AttributeArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteAttributeArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAttributeArgumentListSyntax(
            StringBuilder builder,
            AttributeArgumentListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            var arguments = new List<string>();
            foreach (var node in syntaxNode.Arguments)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeArgumentSyntax(innerBuilder, node);
                arguments.Add(innerBuilder.ToString());
            }
            properties.Add($"\"arguments\":[{string.Join(",", arguments)}]");
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAttributeArgumentSyntax(
            StringBuilder builder,
            AttributeArgumentSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NameEquals != default(NameEqualsSyntax))
            {
                var nameEqualsBuilder = new StringBuilder();
                WriteNameEqualsSyntax(nameEqualsBuilder, syntaxNode.NameEquals);
                properties.Add(
                    $"\"nameEquals\":{nameEqualsBuilder.ToString()}"
                );
            }
            if (syntaxNode.NameColon != default(NameColonSyntax))
            {
                var nameColonBuilder = new StringBuilder();
                WriteNameColonSyntax(nameColonBuilder, syntaxNode.NameColon);
                properties.Add($"\"nameColon\":{nameColonBuilder.ToString()}");
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteNameEqualsSyntax(
            StringBuilder builder,
            NameEqualsSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(IdentifierNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteIdentifierNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.EqualsToken != default(SyntaxToken))
            {
                var equalsTokenBuilder = new StringBuilder();
                WriteSyntaxToken(equalsTokenBuilder, syntaxNode.EqualsToken);
                properties.Add(
                    $"\"equalsToken\":{equalsTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTypeParameterListSyntax(
            StringBuilder builder,
            TypeParameterListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.LessThanToken != default(SyntaxToken))
            {
                var lessThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    lessThanTokenBuilder,
                    syntaxNode.LessThanToken
                );
                properties.Add(
                    $"\"lessThanToken\":{lessThanTokenBuilder.ToString()}"
                );
            }
            var parameters = new List<string>();
            foreach (var node in syntaxNode.Parameters)
            {
                var innerBuilder = new StringBuilder();
                WriteTypeParameterSyntax(innerBuilder, node);
                parameters.Add(innerBuilder.ToString());
            }
            properties.Add($"\"parameters\":[{string.Join(",", parameters)}]");
            if (syntaxNode.GreaterThanToken != default(SyntaxToken))
            {
                var greaterThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    greaterThanTokenBuilder,
                    syntaxNode.GreaterThanToken
                );
                properties.Add(
                    $"\"greaterThanToken\":{greaterThanTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTypeParameterSyntax(
            StringBuilder builder,
            TypeParameterSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            if (syntaxNode.VarianceKeyword != default(SyntaxToken))
            {
                var varianceKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    varianceKeywordBuilder,
                    syntaxNode.VarianceKeyword
                );
                properties.Add(
                    $"\"varianceKeyword\":{varianceKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteClassDeclarationSyntax(
            StringBuilder builder,
            ClassDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.TypeParameterList != default(TypeParameterListSyntax)
            )
            {
                var typeParameterListBuilder = new StringBuilder();
                WriteTypeParameterListSyntax(
                    typeParameterListBuilder,
                    syntaxNode.TypeParameterList
                );
                properties.Add(
                    $"\"typeParameterList\":{typeParameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.BaseList != default(BaseListSyntax))
            {
                var baseListBuilder = new StringBuilder();
                WriteBaseListSyntax(baseListBuilder, syntaxNode.BaseList);
                properties.Add($"\"baseList\":{baseListBuilder.ToString()}");
            }
            var constraintClauses = new List<string>();
            foreach (var node in syntaxNode.ConstraintClauses)
            {
                var innerBuilder = new StringBuilder();
                WriteTypeParameterConstraintClauseSyntax(innerBuilder, node);
                constraintClauses.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"constraintClauses\":[{string.Join(",", constraintClauses)}]"
            );
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var members = new List<string>();
            foreach (var node in syntaxNode.Members)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                members.Add(innerBuilder.ToString());
            }
            properties.Add($"\"members\":[{string.Join(",", members)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteStructDeclarationSyntax(
            StringBuilder builder,
            StructDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.TypeParameterList != default(TypeParameterListSyntax)
            )
            {
                var typeParameterListBuilder = new StringBuilder();
                WriteTypeParameterListSyntax(
                    typeParameterListBuilder,
                    syntaxNode.TypeParameterList
                );
                properties.Add(
                    $"\"typeParameterList\":{typeParameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.BaseList != default(BaseListSyntax))
            {
                var baseListBuilder = new StringBuilder();
                WriteBaseListSyntax(baseListBuilder, syntaxNode.BaseList);
                properties.Add($"\"baseList\":{baseListBuilder.ToString()}");
            }
            var constraintClauses = new List<string>();
            foreach (var node in syntaxNode.ConstraintClauses)
            {
                var innerBuilder = new StringBuilder();
                WriteTypeParameterConstraintClauseSyntax(innerBuilder, node);
                constraintClauses.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"constraintClauses\":[{string.Join(",", constraintClauses)}]"
            );
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var members = new List<string>();
            foreach (var node in syntaxNode.Members)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                members.Add(innerBuilder.ToString());
            }
            properties.Add($"\"members\":[{string.Join(",", members)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteInterfaceDeclarationSyntax(
            StringBuilder builder,
            InterfaceDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.TypeParameterList != default(TypeParameterListSyntax)
            )
            {
                var typeParameterListBuilder = new StringBuilder();
                WriteTypeParameterListSyntax(
                    typeParameterListBuilder,
                    syntaxNode.TypeParameterList
                );
                properties.Add(
                    $"\"typeParameterList\":{typeParameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.BaseList != default(BaseListSyntax))
            {
                var baseListBuilder = new StringBuilder();
                WriteBaseListSyntax(baseListBuilder, syntaxNode.BaseList);
                properties.Add($"\"baseList\":{baseListBuilder.ToString()}");
            }
            var constraintClauses = new List<string>();
            foreach (var node in syntaxNode.ConstraintClauses)
            {
                var innerBuilder = new StringBuilder();
                WriteTypeParameterConstraintClauseSyntax(innerBuilder, node);
                constraintClauses.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"constraintClauses\":[{string.Join(",", constraintClauses)}]"
            );
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var members = new List<string>();
            foreach (var node in syntaxNode.Members)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                members.Add(innerBuilder.ToString());
            }
            properties.Add($"\"members\":[{string.Join(",", members)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRecordDeclarationSyntax(
            StringBuilder builder,
            RecordDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.TypeParameterList != default(TypeParameterListSyntax)
            )
            {
                var typeParameterListBuilder = new StringBuilder();
                WriteTypeParameterListSyntax(
                    typeParameterListBuilder,
                    syntaxNode.TypeParameterList
                );
                properties.Add(
                    $"\"typeParameterList\":{typeParameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.BaseList != default(BaseListSyntax))
            {
                var baseListBuilder = new StringBuilder();
                WriteBaseListSyntax(baseListBuilder, syntaxNode.BaseList);
                properties.Add($"\"baseList\":{baseListBuilder.ToString()}");
            }
            var constraintClauses = new List<string>();
            foreach (var node in syntaxNode.ConstraintClauses)
            {
                var innerBuilder = new StringBuilder();
                WriteTypeParameterConstraintClauseSyntax(innerBuilder, node);
                constraintClauses.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"constraintClauses\":[{string.Join(",", constraintClauses)}]"
            );
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var members = new List<string>();
            foreach (var node in syntaxNode.Members)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                members.Add(innerBuilder.ToString());
            }
            properties.Add($"\"members\":[{string.Join(",", members)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteEnumDeclarationSyntax(
            StringBuilder builder,
            EnumDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.EnumKeyword != default(SyntaxToken))
            {
                var enumKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(enumKeywordBuilder, syntaxNode.EnumKeyword);
                properties.Add(
                    $"\"enumKeyword\":{enumKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.BaseList != default(BaseListSyntax))
            {
                var baseListBuilder = new StringBuilder();
                WriteBaseListSyntax(baseListBuilder, syntaxNode.BaseList);
                properties.Add($"\"baseList\":{baseListBuilder.ToString()}");
            }
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var members = new List<string>();
            foreach (var node in syntaxNode.Members)
            {
                var innerBuilder = new StringBuilder();
                WriteEnumMemberDeclarationSyntax(innerBuilder, node);
                members.Add(innerBuilder.ToString());
            }
            properties.Add($"\"members\":[{string.Join(",", members)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDelegateDeclarationSyntax(
            StringBuilder builder,
            DelegateDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.DelegateKeyword != default(SyntaxToken))
            {
                var delegateKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    delegateKeywordBuilder,
                    syntaxNode.DelegateKeyword
                );
                properties.Add(
                    $"\"delegateKeyword\":{delegateKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ReturnType != default(TypeSyntax))
            {
                var returnTypeBuilder = new StringBuilder();
                WriteSyntaxNode(returnTypeBuilder, syntaxNode.ReturnType);
                properties.Add(
                    $"\"returnType\":{returnTypeBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.TypeParameterList != default(TypeParameterListSyntax)
            )
            {
                var typeParameterListBuilder = new StringBuilder();
                WriteTypeParameterListSyntax(
                    typeParameterListBuilder,
                    syntaxNode.TypeParameterList
                );
                properties.Add(
                    $"\"typeParameterList\":{typeParameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            var constraintClauses = new List<string>();
            foreach (var node in syntaxNode.ConstraintClauses)
            {
                var innerBuilder = new StringBuilder();
                WriteTypeParameterConstraintClauseSyntax(innerBuilder, node);
                constraintClauses.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"constraintClauses\":[{string.Join(",", constraintClauses)}]"
            );
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteEnumMemberDeclarationSyntax(
            StringBuilder builder,
            EnumMemberDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.EqualsValue != default(EqualsValueClauseSyntax))
            {
                var equalsValueBuilder = new StringBuilder();
                WriteEqualsValueClauseSyntax(
                    equalsValueBuilder,
                    syntaxNode.EqualsValue
                );
                properties.Add(
                    $"\"equalsValue\":{equalsValueBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBaseListSyntax(
            StringBuilder builder,
            BaseListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            var types = new List<string>();
            foreach (var node in syntaxNode.Types)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                types.Add(innerBuilder.ToString());
            }
            properties.Add($"\"types\":[{string.Join(",", types)}]");
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSimpleBaseTypeSyntax(
            StringBuilder builder,
            SimpleBaseTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePrimaryConstructorBaseTypeSyntax(
            StringBuilder builder,
            PrimaryConstructorBaseTypeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.ArgumentList != default(ArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTypeParameterConstraintClauseSyntax(
            StringBuilder builder,
            TypeParameterConstraintClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.WhereKeyword != default(SyntaxToken))
            {
                var whereKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(whereKeywordBuilder, syntaxNode.WhereKeyword);
                properties.Add(
                    $"\"whereKeyword\":{whereKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(IdentifierNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteIdentifierNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            var constraints = new List<string>();
            foreach (var node in syntaxNode.Constraints)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                constraints.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"constraints\":[{string.Join(",", constraints)}]"
            );
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteConstructorConstraintSyntax(
            StringBuilder builder,
            ConstructorConstraintSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.NewKeyword != default(SyntaxToken))
            {
                var newKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(newKeywordBuilder, syntaxNode.NewKeyword);
                properties.Add(
                    $"\"newKeyword\":{newKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteClassOrStructConstraintSyntax(
            StringBuilder builder,
            ClassOrStructConstraintSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ClassOrStructKeyword != default(SyntaxToken))
            {
                var classOrStructKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    classOrStructKeywordBuilder,
                    syntaxNode.ClassOrStructKeyword
                );
                properties.Add(
                    $"\"classOrStructKeyword\":{classOrStructKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.QuestionToken != default(SyntaxToken))
            {
                var questionTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    questionTokenBuilder,
                    syntaxNode.QuestionToken
                );
                properties.Add(
                    $"\"questionToken\":{questionTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTypeConstraintSyntax(
            StringBuilder builder,
            TypeConstraintSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDefaultConstraintSyntax(
            StringBuilder builder,
            DefaultConstraintSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.DefaultKeyword != default(SyntaxToken))
            {
                var defaultKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    defaultKeywordBuilder,
                    syntaxNode.DefaultKeyword
                );
                properties.Add(
                    $"\"defaultKeyword\":{defaultKeywordBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFieldDeclarationSyntax(
            StringBuilder builder,
            FieldDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Declaration != default(VariableDeclarationSyntax))
            {
                var declarationBuilder = new StringBuilder();
                WriteVariableDeclarationSyntax(
                    declarationBuilder,
                    syntaxNode.Declaration
                );
                properties.Add(
                    $"\"declaration\":{declarationBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteEventFieldDeclarationSyntax(
            StringBuilder builder,
            EventFieldDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.EventKeyword != default(SyntaxToken))
            {
                var eventKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(eventKeywordBuilder, syntaxNode.EventKeyword);
                properties.Add(
                    $"\"eventKeyword\":{eventKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Declaration != default(VariableDeclarationSyntax))
            {
                var declarationBuilder = new StringBuilder();
                WriteVariableDeclarationSyntax(
                    declarationBuilder,
                    syntaxNode.Declaration
                );
                properties.Add(
                    $"\"declaration\":{declarationBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteExplicitInterfaceSpecifierSyntax(
            StringBuilder builder,
            ExplicitInterfaceSpecifierSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(NameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxNode(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.DotToken != default(SyntaxToken))
            {
                var dotTokenBuilder = new StringBuilder();
                WriteSyntaxToken(dotTokenBuilder, syntaxNode.DotToken);
                properties.Add($"\"dotToken\":{dotTokenBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteMethodDeclarationSyntax(
            StringBuilder builder,
            MethodDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.ReturnType != default(TypeSyntax))
            {
                var returnTypeBuilder = new StringBuilder();
                WriteSyntaxNode(returnTypeBuilder, syntaxNode.ReturnType);
                properties.Add(
                    $"\"returnType\":{returnTypeBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.ExplicitInterfaceSpecifier != default(ExplicitInterfaceSpecifierSyntax)
            )
            {
                var explicitInterfaceSpecifierBuilder = new StringBuilder();
                WriteExplicitInterfaceSpecifierSyntax(
                    explicitInterfaceSpecifierBuilder,
                    syntaxNode.ExplicitInterfaceSpecifier
                );
                properties.Add(
                    $"\"explicitInterfaceSpecifier\":{explicitInterfaceSpecifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.TypeParameterList != default(TypeParameterListSyntax)
            )
            {
                var typeParameterListBuilder = new StringBuilder();
                WriteTypeParameterListSyntax(
                    typeParameterListBuilder,
                    syntaxNode.TypeParameterList
                );
                properties.Add(
                    $"\"typeParameterList\":{typeParameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            var constraintClauses = new List<string>();
            foreach (var node in syntaxNode.ConstraintClauses)
            {
                var innerBuilder = new StringBuilder();
                WriteTypeParameterConstraintClauseSyntax(innerBuilder, node);
                constraintClauses.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"constraintClauses\":[{string.Join(",", constraintClauses)}]"
            );
            if (syntaxNode.Body != default(BlockSyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteBlockSyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("arity", syntaxNode.Arity));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteOperatorDeclarationSyntax(
            StringBuilder builder,
            OperatorDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.ReturnType != default(TypeSyntax))
            {
                var returnTypeBuilder = new StringBuilder();
                WriteSyntaxNode(returnTypeBuilder, syntaxNode.ReturnType);
                properties.Add(
                    $"\"returnType\":{returnTypeBuilder.ToString()}"
                );
            }
            if (syntaxNode.OperatorKeyword != default(SyntaxToken))
            {
                var operatorKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorKeywordBuilder,
                    syntaxNode.OperatorKeyword
                );
                properties.Add(
                    $"\"operatorKeyword\":{operatorKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(BlockSyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteBlockSyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteConversionOperatorDeclarationSyntax(
            StringBuilder builder,
            ConversionOperatorDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.ImplicitOrExplicitKeyword != default(SyntaxToken))
            {
                var implicitOrExplicitKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    implicitOrExplicitKeywordBuilder,
                    syntaxNode.ImplicitOrExplicitKeyword
                );
                properties.Add(
                    $"\"implicitOrExplicitKeyword\":{implicitOrExplicitKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OperatorKeyword != default(SyntaxToken))
            {
                var operatorKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorKeywordBuilder,
                    syntaxNode.OperatorKeyword
                );
                properties.Add(
                    $"\"operatorKeyword\":{operatorKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(BlockSyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteBlockSyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteConstructorDeclarationSyntax(
            StringBuilder builder,
            ConstructorDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.Initializer != default(ConstructorInitializerSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteConstructorInitializerSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(BlockSyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteBlockSyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteConstructorInitializerSyntax(
            StringBuilder builder,
            ConstructorInitializerSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.ThisOrBaseKeyword != default(SyntaxToken))
            {
                var thisOrBaseKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    thisOrBaseKeywordBuilder,
                    syntaxNode.ThisOrBaseKeyword
                );
                properties.Add(
                    $"\"thisOrBaseKeyword\":{thisOrBaseKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ArgumentList != default(ArgumentListSyntax))
            {
                var argumentListBuilder = new StringBuilder();
                WriteArgumentListSyntax(
                    argumentListBuilder,
                    syntaxNode.ArgumentList
                );
                properties.Add(
                    $"\"argumentList\":{argumentListBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDestructorDeclarationSyntax(
            StringBuilder builder,
            DestructorDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.TildeToken != default(SyntaxToken))
            {
                var tildeTokenBuilder = new StringBuilder();
                WriteSyntaxToken(tildeTokenBuilder, syntaxNode.TildeToken);
                properties.Add(
                    $"\"tildeToken\":{tildeTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.ParameterList != default(ParameterListSyntax))
            {
                var parameterListBuilder = new StringBuilder();
                WriteParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.Body != default(BlockSyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteBlockSyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePropertyDeclarationSyntax(
            StringBuilder builder,
            PropertyDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (
                syntaxNode.ExplicitInterfaceSpecifier != default(ExplicitInterfaceSpecifierSyntax)
            )
            {
                var explicitInterfaceSpecifierBuilder = new StringBuilder();
                WriteExplicitInterfaceSpecifierSyntax(
                    explicitInterfaceSpecifierBuilder,
                    syntaxNode.ExplicitInterfaceSpecifier
                );
                properties.Add(
                    $"\"explicitInterfaceSpecifier\":{explicitInterfaceSpecifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.AccessorList != default(AccessorListSyntax))
            {
                var accessorListBuilder = new StringBuilder();
                WriteAccessorListSyntax(
                    accessorListBuilder,
                    syntaxNode.AccessorList
                );
                properties.Add(
                    $"\"accessorList\":{accessorListBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.Initializer != default(EqualsValueClauseSyntax))
            {
                var initializerBuilder = new StringBuilder();
                WriteEqualsValueClauseSyntax(
                    initializerBuilder,
                    syntaxNode.Initializer
                );
                properties.Add(
                    $"\"initializer\":{initializerBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteArrowExpressionClauseSyntax(
            StringBuilder builder,
            ArrowExpressionClauseSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ArrowToken != default(SyntaxToken))
            {
                var arrowTokenBuilder = new StringBuilder();
                WriteSyntaxToken(arrowTokenBuilder, syntaxNode.ArrowToken);
                properties.Add(
                    $"\"arrowToken\":{arrowTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Expression != default(ExpressionSyntax))
            {
                var expressionBuilder = new StringBuilder();
                WriteSyntaxNode(expressionBuilder, syntaxNode.Expression);
                properties.Add(
                    $"\"expression\":{expressionBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteEventDeclarationSyntax(
            StringBuilder builder,
            EventDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.EventKeyword != default(SyntaxToken))
            {
                var eventKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(eventKeywordBuilder, syntaxNode.EventKeyword);
                properties.Add(
                    $"\"eventKeyword\":{eventKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (
                syntaxNode.ExplicitInterfaceSpecifier != default(ExplicitInterfaceSpecifierSyntax)
            )
            {
                var explicitInterfaceSpecifierBuilder = new StringBuilder();
                WriteExplicitInterfaceSpecifierSyntax(
                    explicitInterfaceSpecifierBuilder,
                    syntaxNode.ExplicitInterfaceSpecifier
                );
                properties.Add(
                    $"\"explicitInterfaceSpecifier\":{explicitInterfaceSpecifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.AccessorList != default(AccessorListSyntax))
            {
                var accessorListBuilder = new StringBuilder();
                WriteAccessorListSyntax(
                    accessorListBuilder,
                    syntaxNode.AccessorList
                );
                properties.Add(
                    $"\"accessorList\":{accessorListBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteIndexerDeclarationSyntax(
            StringBuilder builder,
            IndexerDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (
                syntaxNode.ExplicitInterfaceSpecifier != default(ExplicitInterfaceSpecifierSyntax)
            )
            {
                var explicitInterfaceSpecifierBuilder = new StringBuilder();
                WriteExplicitInterfaceSpecifierSyntax(
                    explicitInterfaceSpecifierBuilder,
                    syntaxNode.ExplicitInterfaceSpecifier
                );
                properties.Add(
                    $"\"explicitInterfaceSpecifier\":{explicitInterfaceSpecifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.ThisKeyword != default(SyntaxToken))
            {
                var thisKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(thisKeywordBuilder, syntaxNode.ThisKeyword);
                properties.Add(
                    $"\"thisKeyword\":{thisKeywordBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.ParameterList != default(BracketedParameterListSyntax)
            )
            {
                var parameterListBuilder = new StringBuilder();
                WriteBracketedParameterListSyntax(
                    parameterListBuilder,
                    syntaxNode.ParameterList
                );
                properties.Add(
                    $"\"parameterList\":{parameterListBuilder.ToString()}"
                );
            }
            if (syntaxNode.AccessorList != default(AccessorListSyntax))
            {
                var accessorListBuilder = new StringBuilder();
                WriteAccessorListSyntax(
                    accessorListBuilder,
                    syntaxNode.AccessorList
                );
                properties.Add(
                    $"\"accessorList\":{accessorListBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAccessorListSyntax(
            StringBuilder builder,
            AccessorListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBraceToken != default(SyntaxToken))
            {
                var openBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBraceTokenBuilder,
                    syntaxNode.OpenBraceToken
                );
                properties.Add(
                    $"\"openBraceToken\":{openBraceTokenBuilder.ToString()}"
                );
            }
            var accessors = new List<string>();
            foreach (var node in syntaxNode.Accessors)
            {
                var innerBuilder = new StringBuilder();
                WriteAccessorDeclarationSyntax(innerBuilder, node);
                accessors.Add(innerBuilder.ToString());
            }
            properties.Add($"\"accessors\":[{string.Join(",", accessors)}]");
            if (syntaxNode.CloseBraceToken != default(SyntaxToken))
            {
                var closeBraceTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBraceTokenBuilder,
                    syntaxNode.CloseBraceToken
                );
                properties.Add(
                    $"\"closeBraceToken\":{closeBraceTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteAccessorDeclarationSyntax(
            StringBuilder builder,
            AccessorDeclarationSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Keyword != default(SyntaxToken))
            {
                var keywordBuilder = new StringBuilder();
                WriteSyntaxToken(keywordBuilder, syntaxNode.Keyword);
                properties.Add($"\"keyword\":{keywordBuilder.ToString()}");
            }
            if (syntaxNode.Body != default(BlockSyntax))
            {
                var bodyBuilder = new StringBuilder();
                WriteBlockSyntax(bodyBuilder, syntaxNode.Body);
                properties.Add($"\"body\":{bodyBuilder.ToString()}");
            }
            if (
                syntaxNode.ExpressionBody != default(ArrowExpressionClauseSyntax)
            )
            {
                var expressionBodyBuilder = new StringBuilder();
                WriteArrowExpressionClauseSyntax(
                    expressionBodyBuilder,
                    syntaxNode.ExpressionBody
                );
                properties.Add(
                    $"\"expressionBody\":{expressionBodyBuilder.ToString()}"
                );
            }
            if (syntaxNode.SemicolonToken != default(SyntaxToken))
            {
                var semicolonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    semicolonTokenBuilder,
                    syntaxNode.SemicolonToken
                );
                properties.Add(
                    $"\"semicolonToken\":{semicolonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteParameterListSyntax(
            StringBuilder builder,
            ParameterListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            var parameters = new List<string>();
            foreach (var node in syntaxNode.Parameters)
            {
                var innerBuilder = new StringBuilder();
                WriteParameterSyntax(innerBuilder, node);
                parameters.Add(innerBuilder.ToString());
            }
            properties.Add($"\"parameters\":[{string.Join(",", parameters)}]");
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBracketedParameterListSyntax(
            StringBuilder builder,
            BracketedParameterListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBracketToken != default(SyntaxToken))
            {
                var openBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBracketTokenBuilder,
                    syntaxNode.OpenBracketToken
                );
                properties.Add(
                    $"\"openBracketToken\":{openBracketTokenBuilder.ToString()}"
                );
            }
            var parameters = new List<string>();
            foreach (var node in syntaxNode.Parameters)
            {
                var innerBuilder = new StringBuilder();
                WriteParameterSyntax(innerBuilder, node);
                parameters.Add(innerBuilder.ToString());
            }
            properties.Add($"\"parameters\":[{string.Join(",", parameters)}]");
            if (syntaxNode.CloseBracketToken != default(SyntaxToken))
            {
                var closeBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBracketTokenBuilder,
                    syntaxNode.CloseBracketToken
                );
                properties.Add(
                    $"\"closeBracketToken\":{closeBracketTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteParameterSyntax(
            StringBuilder builder,
            ParameterSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.Default != default(EqualsValueClauseSyntax))
            {
                var defaultBuilder = new StringBuilder();
                WriteEqualsValueClauseSyntax(
                    defaultBuilder,
                    syntaxNode.Default
                );
                properties.Add($"\"default\":{defaultBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteFunctionPointerParameterSyntax(
            StringBuilder builder,
            FunctionPointerParameterSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteIncompleteMemberSyntax(
            StringBuilder builder,
            IncompleteMemberSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var attributeLists = new List<string>();
            foreach (var node in syntaxNode.AttributeLists)
            {
                var innerBuilder = new StringBuilder();
                WriteAttributeListSyntax(innerBuilder, node);
                attributeLists.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"attributeLists\":[{string.Join(",", attributeLists)}]"
            );
            var modifiers = new List<string>();
            foreach (var node in syntaxNode.Modifiers)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                modifiers.Add(innerBuilder.ToString());
            }
            properties.Add($"\"modifiers\":[{string.Join(",", modifiers)}]");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSkippedTokensTriviaSyntax(
            StringBuilder builder,
            SkippedTokensTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var tokens = new List<string>();
            foreach (var node in syntaxNode.Tokens)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                tokens.Add(innerBuilder.ToString());
            }
            properties.Add($"\"tokens\":[{string.Join(",", tokens)}]");
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDocumentationCommentTriviaSyntax(
            StringBuilder builder,
            DocumentationCommentTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var content = new List<string>();
            foreach (var node in syntaxNode.Content)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                content.Add(innerBuilder.ToString());
            }
            properties.Add($"\"content\":[{string.Join(",", content)}]");
            if (syntaxNode.EndOfComment != default(SyntaxToken))
            {
                var endOfCommentBuilder = new StringBuilder();
                WriteSyntaxToken(endOfCommentBuilder, syntaxNode.EndOfComment);
                properties.Add(
                    $"\"endOfComment\":{endOfCommentBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteTypeCrefSyntax(
            StringBuilder builder,
            TypeCrefSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteQualifiedCrefSyntax(
            StringBuilder builder,
            QualifiedCrefSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Container != default(TypeSyntax))
            {
                var containerBuilder = new StringBuilder();
                WriteSyntaxNode(containerBuilder, syntaxNode.Container);
                properties.Add($"\"container\":{containerBuilder.ToString()}");
            }
            if (syntaxNode.DotToken != default(SyntaxToken))
            {
                var dotTokenBuilder = new StringBuilder();
                WriteSyntaxToken(dotTokenBuilder, syntaxNode.DotToken);
                properties.Add($"\"dotToken\":{dotTokenBuilder.ToString()}");
            }
            if (syntaxNode.Member != default(MemberCrefSyntax))
            {
                var memberBuilder = new StringBuilder();
                WriteSyntaxNode(memberBuilder, syntaxNode.Member);
                properties.Add($"\"member\":{memberBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteNameMemberCrefSyntax(
            StringBuilder builder,
            NameMemberCrefSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(TypeSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxNode(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.Parameters != default(CrefParameterListSyntax))
            {
                var parametersBuilder = new StringBuilder();
                WriteCrefParameterListSyntax(
                    parametersBuilder,
                    syntaxNode.Parameters
                );
                properties.Add(
                    $"\"parameters\":{parametersBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteIndexerMemberCrefSyntax(
            StringBuilder builder,
            IndexerMemberCrefSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ThisKeyword != default(SyntaxToken))
            {
                var thisKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(thisKeywordBuilder, syntaxNode.ThisKeyword);
                properties.Add(
                    $"\"thisKeyword\":{thisKeywordBuilder.ToString()}"
                );
            }
            if (
                syntaxNode.Parameters != default(CrefBracketedParameterListSyntax)
            )
            {
                var parametersBuilder = new StringBuilder();
                WriteCrefBracketedParameterListSyntax(
                    parametersBuilder,
                    syntaxNode.Parameters
                );
                properties.Add(
                    $"\"parameters\":{parametersBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteOperatorMemberCrefSyntax(
            StringBuilder builder,
            OperatorMemberCrefSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OperatorKeyword != default(SyntaxToken))
            {
                var operatorKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorKeywordBuilder,
                    syntaxNode.OperatorKeyword
                );
                properties.Add(
                    $"\"operatorKeyword\":{operatorKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OperatorToken != default(SyntaxToken))
            {
                var operatorTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorTokenBuilder,
                    syntaxNode.OperatorToken
                );
                properties.Add(
                    $"\"operatorToken\":{operatorTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Parameters != default(CrefParameterListSyntax))
            {
                var parametersBuilder = new StringBuilder();
                WriteCrefParameterListSyntax(
                    parametersBuilder,
                    syntaxNode.Parameters
                );
                properties.Add(
                    $"\"parameters\":{parametersBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteConversionOperatorMemberCrefSyntax(
            StringBuilder builder,
            ConversionOperatorMemberCrefSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.ImplicitOrExplicitKeyword != default(SyntaxToken))
            {
                var implicitOrExplicitKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    implicitOrExplicitKeywordBuilder,
                    syntaxNode.ImplicitOrExplicitKeyword
                );
                properties.Add(
                    $"\"implicitOrExplicitKeyword\":{implicitOrExplicitKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.OperatorKeyword != default(SyntaxToken))
            {
                var operatorKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    operatorKeywordBuilder,
                    syntaxNode.OperatorKeyword
                );
                properties.Add(
                    $"\"operatorKeyword\":{operatorKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.Parameters != default(CrefParameterListSyntax))
            {
                var parametersBuilder = new StringBuilder();
                WriteCrefParameterListSyntax(
                    parametersBuilder,
                    syntaxNode.Parameters
                );
                properties.Add(
                    $"\"parameters\":{parametersBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCrefParameterListSyntax(
            StringBuilder builder,
            CrefParameterListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenParenToken != default(SyntaxToken))
            {
                var openParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openParenTokenBuilder,
                    syntaxNode.OpenParenToken
                );
                properties.Add(
                    $"\"openParenToken\":{openParenTokenBuilder.ToString()}"
                );
            }
            var parameters = new List<string>();
            foreach (var node in syntaxNode.Parameters)
            {
                var innerBuilder = new StringBuilder();
                WriteCrefParameterSyntax(innerBuilder, node);
                parameters.Add(innerBuilder.ToString());
            }
            properties.Add($"\"parameters\":[{string.Join(",", parameters)}]");
            if (syntaxNode.CloseParenToken != default(SyntaxToken))
            {
                var closeParenTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeParenTokenBuilder,
                    syntaxNode.CloseParenToken
                );
                properties.Add(
                    $"\"closeParenToken\":{closeParenTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCrefBracketedParameterListSyntax(
            StringBuilder builder,
            CrefBracketedParameterListSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.OpenBracketToken != default(SyntaxToken))
            {
                var openBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    openBracketTokenBuilder,
                    syntaxNode.OpenBracketToken
                );
                properties.Add(
                    $"\"openBracketToken\":{openBracketTokenBuilder.ToString()}"
                );
            }
            var parameters = new List<string>();
            foreach (var node in syntaxNode.Parameters)
            {
                var innerBuilder = new StringBuilder();
                WriteCrefParameterSyntax(innerBuilder, node);
                parameters.Add(innerBuilder.ToString());
            }
            properties.Add($"\"parameters\":[{string.Join(",", parameters)}]");
            if (syntaxNode.CloseBracketToken != default(SyntaxToken))
            {
                var closeBracketTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    closeBracketTokenBuilder,
                    syntaxNode.CloseBracketToken
                );
                properties.Add(
                    $"\"closeBracketToken\":{closeBracketTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteCrefParameterSyntax(
            StringBuilder builder,
            CrefParameterSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.RefKindKeyword != default(SyntaxToken))
            {
                var refKindKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    refKindKeywordBuilder,
                    syntaxNode.RefKindKeyword
                );
                properties.Add(
                    $"\"refKindKeyword\":{refKindKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Type != default(TypeSyntax))
            {
                var typeBuilder = new StringBuilder();
                WriteSyntaxNode(typeBuilder, syntaxNode.Type);
                properties.Add($"\"type\":{typeBuilder.ToString()}");
            }
            if (syntaxNode.RefOrOutKeyword != default(SyntaxToken))
            {
                var refOrOutKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    refOrOutKeywordBuilder,
                    syntaxNode.RefOrOutKeyword
                );
                properties.Add(
                    $"\"refOrOutKeyword\":{refOrOutKeywordBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlElementSyntax(
            StringBuilder builder,
            XmlElementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.StartTag != default(XmlElementStartTagSyntax))
            {
                var startTagBuilder = new StringBuilder();
                WriteXmlElementStartTagSyntax(
                    startTagBuilder,
                    syntaxNode.StartTag
                );
                properties.Add($"\"startTag\":{startTagBuilder.ToString()}");
            }
            var content = new List<string>();
            foreach (var node in syntaxNode.Content)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                content.Add(innerBuilder.ToString());
            }
            properties.Add($"\"content\":[{string.Join(",", content)}]");
            if (syntaxNode.EndTag != default(XmlElementEndTagSyntax))
            {
                var endTagBuilder = new StringBuilder();
                WriteXmlElementEndTagSyntax(endTagBuilder, syntaxNode.EndTag);
                properties.Add($"\"endTag\":{endTagBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlElementStartTagSyntax(
            StringBuilder builder,
            XmlElementStartTagSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.LessThanToken != default(SyntaxToken))
            {
                var lessThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    lessThanTokenBuilder,
                    syntaxNode.LessThanToken
                );
                properties.Add(
                    $"\"lessThanToken\":{lessThanTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(XmlNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteXmlNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            var attributes = new List<string>();
            foreach (var node in syntaxNode.Attributes)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                attributes.Add(innerBuilder.ToString());
            }
            properties.Add($"\"attributes\":[{string.Join(",", attributes)}]");
            if (syntaxNode.GreaterThanToken != default(SyntaxToken))
            {
                var greaterThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    greaterThanTokenBuilder,
                    syntaxNode.GreaterThanToken
                );
                properties.Add(
                    $"\"greaterThanToken\":{greaterThanTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlElementEndTagSyntax(
            StringBuilder builder,
            XmlElementEndTagSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.LessThanSlashToken != default(SyntaxToken))
            {
                var lessThanSlashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    lessThanSlashTokenBuilder,
                    syntaxNode.LessThanSlashToken
                );
                properties.Add(
                    $"\"lessThanSlashToken\":{lessThanSlashTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(XmlNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteXmlNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.GreaterThanToken != default(SyntaxToken))
            {
                var greaterThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    greaterThanTokenBuilder,
                    syntaxNode.GreaterThanToken
                );
                properties.Add(
                    $"\"greaterThanToken\":{greaterThanTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlEmptyElementSyntax(
            StringBuilder builder,
            XmlEmptyElementSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.LessThanToken != default(SyntaxToken))
            {
                var lessThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    lessThanTokenBuilder,
                    syntaxNode.LessThanToken
                );
                properties.Add(
                    $"\"lessThanToken\":{lessThanTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(XmlNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteXmlNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            var attributes = new List<string>();
            foreach (var node in syntaxNode.Attributes)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                attributes.Add(innerBuilder.ToString());
            }
            properties.Add($"\"attributes\":[{string.Join(",", attributes)}]");
            if (syntaxNode.SlashGreaterThanToken != default(SyntaxToken))
            {
                var slashGreaterThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    slashGreaterThanTokenBuilder,
                    syntaxNode.SlashGreaterThanToken
                );
                properties.Add(
                    $"\"slashGreaterThanToken\":{slashGreaterThanTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlNameSyntax(
            StringBuilder builder,
            XmlNameSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Prefix != default(XmlPrefixSyntax))
            {
                var prefixBuilder = new StringBuilder();
                WriteXmlPrefixSyntax(prefixBuilder, syntaxNode.Prefix);
                properties.Add($"\"prefix\":{prefixBuilder.ToString()}");
            }
            if (syntaxNode.LocalName != default(SyntaxToken))
            {
                var localNameBuilder = new StringBuilder();
                WriteSyntaxToken(localNameBuilder, syntaxNode.LocalName);
                properties.Add($"\"localName\":{localNameBuilder.ToString()}");
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlPrefixSyntax(
            StringBuilder builder,
            XmlPrefixSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Prefix != default(SyntaxToken))
            {
                var prefixBuilder = new StringBuilder();
                WriteSyntaxToken(prefixBuilder, syntaxNode.Prefix);
                properties.Add($"\"prefix\":{prefixBuilder.ToString()}");
            }
            if (syntaxNode.ColonToken != default(SyntaxToken))
            {
                var colonTokenBuilder = new StringBuilder();
                WriteSyntaxToken(colonTokenBuilder, syntaxNode.ColonToken);
                properties.Add(
                    $"\"colonToken\":{colonTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlTextAttributeSyntax(
            StringBuilder builder,
            XmlTextAttributeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(XmlNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteXmlNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.EqualsToken != default(SyntaxToken))
            {
                var equalsTokenBuilder = new StringBuilder();
                WriteSyntaxToken(equalsTokenBuilder, syntaxNode.EqualsToken);
                properties.Add(
                    $"\"equalsToken\":{equalsTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.StartQuoteToken != default(SyntaxToken))
            {
                var startQuoteTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    startQuoteTokenBuilder,
                    syntaxNode.StartQuoteToken
                );
                properties.Add(
                    $"\"startQuoteToken\":{startQuoteTokenBuilder.ToString()}"
                );
            }
            var textTokens = new List<string>();
            foreach (var node in syntaxNode.TextTokens)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                textTokens.Add(innerBuilder.ToString());
            }
            properties.Add($"\"textTokens\":[{string.Join(",", textTokens)}]");
            if (syntaxNode.EndQuoteToken != default(SyntaxToken))
            {
                var endQuoteTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endQuoteTokenBuilder,
                    syntaxNode.EndQuoteToken
                );
                properties.Add(
                    $"\"endQuoteToken\":{endQuoteTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlCrefAttributeSyntax(
            StringBuilder builder,
            XmlCrefAttributeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(XmlNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteXmlNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.EqualsToken != default(SyntaxToken))
            {
                var equalsTokenBuilder = new StringBuilder();
                WriteSyntaxToken(equalsTokenBuilder, syntaxNode.EqualsToken);
                properties.Add(
                    $"\"equalsToken\":{equalsTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.StartQuoteToken != default(SyntaxToken))
            {
                var startQuoteTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    startQuoteTokenBuilder,
                    syntaxNode.StartQuoteToken
                );
                properties.Add(
                    $"\"startQuoteToken\":{startQuoteTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Cref != default(CrefSyntax))
            {
                var crefBuilder = new StringBuilder();
                WriteSyntaxNode(crefBuilder, syntaxNode.Cref);
                properties.Add($"\"cref\":{crefBuilder.ToString()}");
            }
            if (syntaxNode.EndQuoteToken != default(SyntaxToken))
            {
                var endQuoteTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endQuoteTokenBuilder,
                    syntaxNode.EndQuoteToken
                );
                properties.Add(
                    $"\"endQuoteToken\":{endQuoteTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlNameAttributeSyntax(
            StringBuilder builder,
            XmlNameAttributeSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.Name != default(XmlNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteXmlNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.EqualsToken != default(SyntaxToken))
            {
                var equalsTokenBuilder = new StringBuilder();
                WriteSyntaxToken(equalsTokenBuilder, syntaxNode.EqualsToken);
                properties.Add(
                    $"\"equalsToken\":{equalsTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.StartQuoteToken != default(SyntaxToken))
            {
                var startQuoteTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    startQuoteTokenBuilder,
                    syntaxNode.StartQuoteToken
                );
                properties.Add(
                    $"\"startQuoteToken\":{startQuoteTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Identifier != default(IdentifierNameSyntax))
            {
                var identifierBuilder = new StringBuilder();
                WriteIdentifierNameSyntax(
                    identifierBuilder,
                    syntaxNode.Identifier
                );
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndQuoteToken != default(SyntaxToken))
            {
                var endQuoteTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endQuoteTokenBuilder,
                    syntaxNode.EndQuoteToken
                );
                properties.Add(
                    $"\"endQuoteToken\":{endQuoteTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlTextSyntax(
            StringBuilder builder,
            XmlTextSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            var textTokens = new List<string>();
            foreach (var node in syntaxNode.TextTokens)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                textTokens.Add(innerBuilder.ToString());
            }
            properties.Add($"\"textTokens\":[{string.Join(",", textTokens)}]");
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlCDataSectionSyntax(
            StringBuilder builder,
            XmlCDataSectionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.StartCDataToken != default(SyntaxToken))
            {
                var startCDataTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    startCDataTokenBuilder,
                    syntaxNode.StartCDataToken
                );
                properties.Add(
                    $"\"startCDataToken\":{startCDataTokenBuilder.ToString()}"
                );
            }
            var textTokens = new List<string>();
            foreach (var node in syntaxNode.TextTokens)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                textTokens.Add(innerBuilder.ToString());
            }
            properties.Add($"\"textTokens\":[{string.Join(",", textTokens)}]");
            if (syntaxNode.EndCDataToken != default(SyntaxToken))
            {
                var endCDataTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endCDataTokenBuilder,
                    syntaxNode.EndCDataToken
                );
                properties.Add(
                    $"\"endCDataToken\":{endCDataTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlProcessingInstructionSyntax(
            StringBuilder builder,
            XmlProcessingInstructionSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (
                syntaxNode.StartProcessingInstructionToken != default(SyntaxToken)
            )
            {
                var startProcessingInstructionTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    startProcessingInstructionTokenBuilder,
                    syntaxNode.StartProcessingInstructionToken
                );
                properties.Add(
                    $"\"startProcessingInstructionToken\":{startProcessingInstructionTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(XmlNameSyntax))
            {
                var nameBuilder = new StringBuilder();
                WriteXmlNameSyntax(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            var textTokens = new List<string>();
            foreach (var node in syntaxNode.TextTokens)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                textTokens.Add(innerBuilder.ToString());
            }
            properties.Add($"\"textTokens\":[{string.Join(",", textTokens)}]");
            if (
                syntaxNode.EndProcessingInstructionToken != default(SyntaxToken)
            )
            {
                var endProcessingInstructionTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endProcessingInstructionTokenBuilder,
                    syntaxNode.EndProcessingInstructionToken
                );
                properties.Add(
                    $"\"endProcessingInstructionToken\":{endProcessingInstructionTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteXmlCommentSyntax(
            StringBuilder builder,
            XmlCommentSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (
                syntaxNode.LessThanExclamationMinusMinusToken != default(SyntaxToken)
            )
            {
                var lessThanExclamationMinusMinusTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    lessThanExclamationMinusMinusTokenBuilder,
                    syntaxNode.LessThanExclamationMinusMinusToken
                );
                properties.Add(
                    $"\"lessThanExclamationMinusMinusToken\":{lessThanExclamationMinusMinusTokenBuilder.ToString()}"
                );
            }
            var textTokens = new List<string>();
            foreach (var node in syntaxNode.TextTokens)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxToken(innerBuilder, node);
                textTokens.Add(innerBuilder.ToString());
            }
            properties.Add($"\"textTokens\":[{string.Join(",", textTokens)}]");
            if (syntaxNode.MinusMinusGreaterThanToken != default(SyntaxToken))
            {
                var minusMinusGreaterThanTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    minusMinusGreaterThanTokenBuilder,
                    syntaxNode.MinusMinusGreaterThanToken
                );
                properties.Add(
                    $"\"minusMinusGreaterThanToken\":{minusMinusGreaterThanTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteIfDirectiveTriviaSyntax(
            StringBuilder builder,
            IfDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.IfKeyword != default(SyntaxToken))
            {
                var ifKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(ifKeywordBuilder, syntaxNode.IfKeyword);
                properties.Add($"\"ifKeyword\":{ifKeywordBuilder.ToString()}");
            }
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            properties.Add(WriteBoolean("branchTaken", syntaxNode.BranchTaken));
            properties.Add(
                WriteBoolean("conditionValue", syntaxNode.ConditionValue)
            );
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteElifDirectiveTriviaSyntax(
            StringBuilder builder,
            ElifDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.ElifKeyword != default(SyntaxToken))
            {
                var elifKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(elifKeywordBuilder, syntaxNode.ElifKeyword);
                properties.Add(
                    $"\"elifKeyword\":{elifKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Condition != default(ExpressionSyntax))
            {
                var conditionBuilder = new StringBuilder();
                WriteSyntaxNode(conditionBuilder, syntaxNode.Condition);
                properties.Add($"\"condition\":{conditionBuilder.ToString()}");
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            properties.Add(WriteBoolean("branchTaken", syntaxNode.BranchTaken));
            properties.Add(
                WriteBoolean("conditionValue", syntaxNode.ConditionValue)
            );
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteElseDirectiveTriviaSyntax(
            StringBuilder builder,
            ElseDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.ElseKeyword != default(SyntaxToken))
            {
                var elseKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(elseKeywordBuilder, syntaxNode.ElseKeyword);
                properties.Add(
                    $"\"elseKeyword\":{elseKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            properties.Add(WriteBoolean("branchTaken", syntaxNode.BranchTaken));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteEndIfDirectiveTriviaSyntax(
            StringBuilder builder,
            EndIfDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.EndIfKeyword != default(SyntaxToken))
            {
                var endIfKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(endIfKeywordBuilder, syntaxNode.EndIfKeyword);
                properties.Add(
                    $"\"endIfKeyword\":{endIfKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteRegionDirectiveTriviaSyntax(
            StringBuilder builder,
            RegionDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.RegionKeyword != default(SyntaxToken))
            {
                var regionKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    regionKeywordBuilder,
                    syntaxNode.RegionKeyword
                );
                properties.Add(
                    $"\"regionKeyword\":{regionKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteEndRegionDirectiveTriviaSyntax(
            StringBuilder builder,
            EndRegionDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.EndRegionKeyword != default(SyntaxToken))
            {
                var endRegionKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endRegionKeywordBuilder,
                    syntaxNode.EndRegionKeyword
                );
                properties.Add(
                    $"\"endRegionKeyword\":{endRegionKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteErrorDirectiveTriviaSyntax(
            StringBuilder builder,
            ErrorDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.ErrorKeyword != default(SyntaxToken))
            {
                var errorKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(errorKeywordBuilder, syntaxNode.ErrorKeyword);
                properties.Add(
                    $"\"errorKeyword\":{errorKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteWarningDirectiveTriviaSyntax(
            StringBuilder builder,
            WarningDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.WarningKeyword != default(SyntaxToken))
            {
                var warningKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    warningKeywordBuilder,
                    syntaxNode.WarningKeyword
                );
                properties.Add(
                    $"\"warningKeyword\":{warningKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteBadDirectiveTriviaSyntax(
            StringBuilder builder,
            BadDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.Identifier != default(SyntaxToken))
            {
                var identifierBuilder = new StringBuilder();
                WriteSyntaxToken(identifierBuilder, syntaxNode.Identifier);
                properties.Add(
                    $"\"identifier\":{identifierBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteDefineDirectiveTriviaSyntax(
            StringBuilder builder,
            DefineDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.DefineKeyword != default(SyntaxToken))
            {
                var defineKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    defineKeywordBuilder,
                    syntaxNode.DefineKeyword
                );
                properties.Add(
                    $"\"defineKeyword\":{defineKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(SyntaxToken))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxToken(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteUndefDirectiveTriviaSyntax(
            StringBuilder builder,
            UndefDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.UndefKeyword != default(SyntaxToken))
            {
                var undefKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(undefKeywordBuilder, syntaxNode.UndefKeyword);
                properties.Add(
                    $"\"undefKeyword\":{undefKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Name != default(SyntaxToken))
            {
                var nameBuilder = new StringBuilder();
                WriteSyntaxToken(nameBuilder, syntaxNode.Name);
                properties.Add($"\"name\":{nameBuilder.ToString()}");
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteLineDirectiveTriviaSyntax(
            StringBuilder builder,
            LineDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.LineKeyword != default(SyntaxToken))
            {
                var lineKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(lineKeywordBuilder, syntaxNode.LineKeyword);
                properties.Add(
                    $"\"lineKeyword\":{lineKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.Line != default(SyntaxToken))
            {
                var lineBuilder = new StringBuilder();
                WriteSyntaxToken(lineBuilder, syntaxNode.Line);
                properties.Add($"\"line\":{lineBuilder.ToString()}");
            }
            if (syntaxNode.File != default(SyntaxToken))
            {
                var fileBuilder = new StringBuilder();
                WriteSyntaxToken(fileBuilder, syntaxNode.File);
                properties.Add($"\"file\":{fileBuilder.ToString()}");
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePragmaWarningDirectiveTriviaSyntax(
            StringBuilder builder,
            PragmaWarningDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.PragmaKeyword != default(SyntaxToken))
            {
                var pragmaKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    pragmaKeywordBuilder,
                    syntaxNode.PragmaKeyword
                );
                properties.Add(
                    $"\"pragmaKeyword\":{pragmaKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.WarningKeyword != default(SyntaxToken))
            {
                var warningKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    warningKeywordBuilder,
                    syntaxNode.WarningKeyword
                );
                properties.Add(
                    $"\"warningKeyword\":{warningKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.DisableOrRestoreKeyword != default(SyntaxToken))
            {
                var disableOrRestoreKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    disableOrRestoreKeywordBuilder,
                    syntaxNode.DisableOrRestoreKeyword
                );
                properties.Add(
                    $"\"disableOrRestoreKeyword\":{disableOrRestoreKeywordBuilder.ToString()}"
                );
            }
            var errorCodes = new List<string>();
            foreach (var node in syntaxNode.ErrorCodes)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxNode(innerBuilder, node);
                errorCodes.Add(innerBuilder.ToString());
            }
            properties.Add($"\"errorCodes\":[{string.Join(",", errorCodes)}]");
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WritePragmaChecksumDirectiveTriviaSyntax(
            StringBuilder builder,
            PragmaChecksumDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.PragmaKeyword != default(SyntaxToken))
            {
                var pragmaKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    pragmaKeywordBuilder,
                    syntaxNode.PragmaKeyword
                );
                properties.Add(
                    $"\"pragmaKeyword\":{pragmaKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.ChecksumKeyword != default(SyntaxToken))
            {
                var checksumKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    checksumKeywordBuilder,
                    syntaxNode.ChecksumKeyword
                );
                properties.Add(
                    $"\"checksumKeyword\":{checksumKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.File != default(SyntaxToken))
            {
                var fileBuilder = new StringBuilder();
                WriteSyntaxToken(fileBuilder, syntaxNode.File);
                properties.Add($"\"file\":{fileBuilder.ToString()}");
            }
            if (syntaxNode.Guid != default(SyntaxToken))
            {
                var guidBuilder = new StringBuilder();
                WriteSyntaxToken(guidBuilder, syntaxNode.Guid);
                properties.Add($"\"guid\":{guidBuilder.ToString()}");
            }
            if (syntaxNode.Bytes != default(SyntaxToken))
            {
                var bytesBuilder = new StringBuilder();
                WriteSyntaxToken(bytesBuilder, syntaxNode.Bytes);
                properties.Add($"\"bytes\":{bytesBuilder.ToString()}");
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteReferenceDirectiveTriviaSyntax(
            StringBuilder builder,
            ReferenceDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.ReferenceKeyword != default(SyntaxToken))
            {
                var referenceKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    referenceKeywordBuilder,
                    syntaxNode.ReferenceKeyword
                );
                properties.Add(
                    $"\"referenceKeyword\":{referenceKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.File != default(SyntaxToken))
            {
                var fileBuilder = new StringBuilder();
                WriteSyntaxToken(fileBuilder, syntaxNode.File);
                properties.Add($"\"file\":{fileBuilder.ToString()}");
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteLoadDirectiveTriviaSyntax(
            StringBuilder builder,
            LoadDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.LoadKeyword != default(SyntaxToken))
            {
                var loadKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(loadKeywordBuilder, syntaxNode.LoadKeyword);
                properties.Add(
                    $"\"loadKeyword\":{loadKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.File != default(SyntaxToken))
            {
                var fileBuilder = new StringBuilder();
                WriteSyntaxToken(fileBuilder, syntaxNode.File);
                properties.Add($"\"file\":{fileBuilder.ToString()}");
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteShebangDirectiveTriviaSyntax(
            StringBuilder builder,
            ShebangDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.ExclamationToken != default(SyntaxToken))
            {
                var exclamationTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    exclamationTokenBuilder,
                    syntaxNode.ExclamationToken
                );
                properties.Add(
                    $"\"exclamationToken\":{exclamationTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteNullableDirectiveTriviaSyntax(
            StringBuilder builder,
            NullableDirectiveTriviaSyntax syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            if (syntaxNode.HashToken != default(SyntaxToken))
            {
                var hashTokenBuilder = new StringBuilder();
                WriteSyntaxToken(hashTokenBuilder, syntaxNode.HashToken);
                properties.Add($"\"hashToken\":{hashTokenBuilder.ToString()}");
            }
            if (syntaxNode.NullableKeyword != default(SyntaxToken))
            {
                var nullableKeywordBuilder = new StringBuilder();
                WriteSyntaxToken(
                    nullableKeywordBuilder,
                    syntaxNode.NullableKeyword
                );
                properties.Add(
                    $"\"nullableKeyword\":{nullableKeywordBuilder.ToString()}"
                );
            }
            if (syntaxNode.SettingToken != default(SyntaxToken))
            {
                var settingTokenBuilder = new StringBuilder();
                WriteSyntaxToken(settingTokenBuilder, syntaxNode.SettingToken);
                properties.Add(
                    $"\"settingToken\":{settingTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.TargetToken != default(SyntaxToken))
            {
                var targetTokenBuilder = new StringBuilder();
                WriteSyntaxToken(targetTokenBuilder, syntaxNode.TargetToken);
                properties.Add(
                    $"\"targetToken\":{targetTokenBuilder.ToString()}"
                );
            }
            if (syntaxNode.EndOfDirectiveToken != default(SyntaxToken))
            {
                var endOfDirectiveTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    endOfDirectiveTokenBuilder,
                    syntaxNode.EndOfDirectiveToken
                );
                properties.Add(
                    $"\"endOfDirectiveToken\":{endOfDirectiveTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteBoolean("isActive", syntaxNode.IsActive));
            if (syntaxNode.DirectiveNameToken != default(SyntaxToken))
            {
                var directiveNameTokenBuilder = new StringBuilder();
                WriteSyntaxToken(
                    directiveNameTokenBuilder,
                    syntaxNode.DirectiveNameToken
                );
                properties.Add(
                    $"\"directiveNameToken\":{directiveNameTokenBuilder.ToString()}"
                );
            }
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(
                WriteBoolean(
                    "isStructuredTrivia",
                    syntaxNode.IsStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsSkippedText",
                    syntaxNode.ContainsSkippedText
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSyntaxToken(
            StringBuilder builder,
            SyntaxToken syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(WriteBoolean("isMissing", syntaxNode.IsMissing));
            properties.Add(WriteString("text", syntaxNode.Text));
            properties.Add(
                WriteBoolean("hasLeadingTrivia", syntaxNode.HasLeadingTrivia)
            );
            properties.Add(
                WriteBoolean("hasTrailingTrivia", syntaxNode.HasTrailingTrivia)
            );
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsDirectives",
                    syntaxNode.ContainsDirectives
                )
            );
            properties.Add(
                WriteBoolean(
                    "hasStructuredTrivia",
                    syntaxNode.HasStructuredTrivia
                )
            );
            properties.Add(
                WriteBoolean(
                    "containsAnnotations",
                    syntaxNode.ContainsAnnotations
                )
            );
            var leadingTrivia = new List<string>();
            foreach (var node in syntaxNode.LeadingTrivia)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxTrivia(innerBuilder, node);
                leadingTrivia.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"leadingTrivia\":[{string.Join(",", leadingTrivia)}]"
            );
            var trailingTrivia = new List<string>();
            foreach (var node in syntaxNode.TrailingTrivia)
            {
                var innerBuilder = new StringBuilder();
                WriteSyntaxTrivia(innerBuilder, node);
                trailingTrivia.Add(innerBuilder.ToString());
            }
            properties.Add(
                $"\"trailingTrivia\":[{string.Join(",", trailingTrivia)}]"
            );
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
        public static void WriteSyntaxTrivia(
            StringBuilder builder,
            SyntaxTrivia syntaxNode)
        {
            builder.Append("{");
            var properties = new List<string>();
            properties.Add(
                $"\"nodeType\":\"{GetNodeType(syntaxNode.GetType())}\""
            );
            properties.Add($"\"kind\":\"{syntaxNode.Kind().ToString()}\"");
            properties.Add(WriteString("text", syntaxNode.ToString()));
            properties.Add(WriteInt("rawKind", syntaxNode.RawKind));
            properties.Add(
                WriteBoolean(
                    "containsDiagnostics",
                    syntaxNode.ContainsDiagnostics
                )
            );
            properties.Add(
                WriteBoolean("hasStructure", syntaxNode.HasStructure)
            );
            properties.Add(WriteBoolean("isDirective", syntaxNode.IsDirective));
            builder.Append(string.Join(",", properties.Where(o => o != null)));
            builder.Append("}");
        }
    }
}
