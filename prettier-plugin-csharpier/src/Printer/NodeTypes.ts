import { SyntaxToken, SyntaxTreeNode } from "./SyntaxTreeNode";

interface AccessorDeclarationNode extends SyntaxTreeNode<"AccessorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface AccessorListNode extends SyntaxTreeNode<"AccessorList"> {
    openBraceToken?: SyntaxToken;
    accessors: AccessorDeclarationNode[];
    closeBraceToken?: SyntaxToken;
    rawKind?: number;
}

interface AliasQualifiedNameNode extends SyntaxTreeNode<"AliasQualifiedName"> {
    alias?: IdentifierNameNode;
    colonColonToken?: SyntaxToken;
    name?: SyntaxTreeNode;
    arity?: number;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface AnonymousMethodExpressionNode extends SyntaxTreeNode<"AnonymousMethodExpression"> {
    modifiers: SyntaxToken[];
    delegateKeyword?: SyntaxToken;
    parameterList?: ParameterListNode;
    block?: BlockNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword?: SyntaxToken;
    body?: SyntaxTreeNode;
    rawKind?: number;
}

interface AnonymousObjectCreationExpressionNode extends SyntaxTreeNode<"AnonymousObjectCreationExpression"> {
    newKeyword?: SyntaxToken;
    openBraceToken?: SyntaxToken;
    initializers: AnonymousObjectMemberDeclaratorNode[];
    closeBraceToken?: SyntaxToken;
    rawKind?: number;
}

interface AnonymousObjectMemberDeclaratorNode extends SyntaxTreeNode<"AnonymousObjectMemberDeclarator"> {
    nameEquals?: NameEqualsNode;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface ArgumentListNode extends SyntaxTreeNode<"ArgumentList"> {
    openParenToken?: SyntaxToken;
    arguments: ArgumentNode[];
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface ArgumentNode extends SyntaxTreeNode<"Argument"> {
    nameColon?: NameColonNode;
    refKindKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    refOrOutKeyword?: SyntaxToken;
    rawKind?: number;
}

interface ArrayCreationExpressionNode extends SyntaxTreeNode<"ArrayCreationExpression"> {
    newKeyword?: SyntaxToken;
    type?: ArrayTypeNode;
    initializer?: InitializerExpressionNode;
    rawKind?: number;
}

interface ArrayRankSpecifierNode extends SyntaxTreeNode<"ArrayRankSpecifier"> {
    openBracketToken?: SyntaxToken;
    sizes: SyntaxTreeNode[];
    closeBracketToken?: SyntaxToken;
    rank?: number;
    rawKind?: number;
}

interface ArrayTypeNode extends SyntaxTreeNode<"ArrayType"> {
    elementType?: SyntaxTreeNode;
    rankSpecifiers: ArrayRankSpecifierNode[];
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface ArrowExpressionClauseNode extends SyntaxTreeNode<"ArrowExpressionClause"> {
    arrowToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface AssignmentExpressionNode extends SyntaxTreeNode<"AssignmentExpression"> {
    left?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    right?: SyntaxTreeNode;
    rawKind?: number;
}

interface AttributeArgumentListNode extends SyntaxTreeNode<"AttributeArgumentList"> {
    openParenToken?: SyntaxToken;
    arguments: AttributeArgumentNode[];
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface AttributeArgumentNode extends SyntaxTreeNode<"AttributeArgument"> {
    nameEquals?: NameEqualsNode;
    nameColon?: NameColonNode;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface AttributeListNode extends SyntaxTreeNode<"AttributeList"> {
    openBracketToken?: SyntaxToken;
    target?: AttributeTargetSpecifierNode;
    attributes: AttributeNode[];
    closeBracketToken?: SyntaxToken;
    rawKind?: number;
}

interface AttributeNode extends SyntaxTreeNode<"Attribute"> {
    name?: SyntaxTreeNode;
    argumentList?: AttributeArgumentListNode;
    rawKind?: number;
}

interface AttributeTargetSpecifierNode extends SyntaxTreeNode<"AttributeTargetSpecifier"> {
    identifier?: SyntaxToken;
    colonToken?: SyntaxToken;
    rawKind?: number;
}

interface AwaitExpressionNode extends SyntaxTreeNode<"AwaitExpression"> {
    awaitKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface BadDirectiveTriviaNode extends SyntaxTreeNode<"BadDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    identifier?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface BaseExpressionNode extends SyntaxTreeNode<"BaseExpression"> {
    token?: SyntaxToken;
    rawKind?: number;
}

interface BaseListNode extends SyntaxTreeNode<"BaseList"> {
    colonToken?: SyntaxToken;
    types: SyntaxTreeNode[];
    rawKind?: number;
}

interface BinaryExpressionNode extends SyntaxTreeNode<"BinaryExpression"> {
    left?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    right?: SyntaxTreeNode;
    rawKind?: number;
}

interface BinaryPatternNode extends SyntaxTreeNode<"BinaryPattern"> {
    left?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    right?: SyntaxTreeNode;
    rawKind?: number;
}

interface BlockNode extends SyntaxTreeNode<"Block"> {
    attributeLists: AttributeListNode[];
    openBraceToken?: SyntaxToken;
    statements: SyntaxTreeNode[];
    closeBraceToken?: SyntaxToken;
    rawKind?: number;
}

interface BracketedArgumentListNode extends SyntaxTreeNode<"BracketedArgumentList"> {
    openBracketToken?: SyntaxToken;
    arguments: ArgumentNode[];
    closeBracketToken?: SyntaxToken;
    rawKind?: number;
}

interface BracketedParameterListNode extends SyntaxTreeNode<"BracketedParameterList"> {
    openBracketToken?: SyntaxToken;
    parameters: ParameterNode[];
    closeBracketToken?: SyntaxToken;
    rawKind?: number;
}

interface BreakStatementNode extends SyntaxTreeNode<"BreakStatement"> {
    attributeLists: AttributeListNode[];
    breakKeyword?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface CasePatternSwitchLabelNode extends SyntaxTreeNode<"CasePatternSwitchLabel"> {
    keyword?: SyntaxToken;
    pattern?: SyntaxTreeNode;
    whenClause?: WhenClauseNode;
    colonToken?: SyntaxToken;
    rawKind?: number;
}

interface CaseSwitchLabelNode extends SyntaxTreeNode<"CaseSwitchLabel"> {
    keyword?: SyntaxToken;
    value?: SyntaxTreeNode;
    colonToken?: SyntaxToken;
    rawKind?: number;
}

interface CastExpressionNode extends SyntaxTreeNode<"CastExpression"> {
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface CatchClauseNode extends SyntaxTreeNode<"CatchClause"> {
    catchKeyword?: SyntaxToken;
    declaration?: CatchDeclarationNode;
    filter?: CatchFilterClauseNode;
    block?: BlockNode;
    rawKind?: number;
}

interface CatchDeclarationNode extends SyntaxTreeNode<"CatchDeclaration"> {
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface CatchFilterClauseNode extends SyntaxTreeNode<"CatchFilterClause"> {
    whenKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    filterExpression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface CheckedExpressionNode extends SyntaxTreeNode<"CheckedExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface CheckedStatementNode extends SyntaxTreeNode<"CheckedStatement"> {
    attributeLists: AttributeListNode[];
    keyword?: SyntaxToken;
    block?: BlockNode;
    rawKind?: number;
}

interface ClassDeclarationNode extends SyntaxTreeNode<"ClassDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    baseList?: BaseListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    openBraceToken?: SyntaxToken;
    members: SyntaxTreeNode[];
    closeBraceToken?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    arity?: number;
    rawKind?: number;
}

interface ClassOrStructConstraintNode extends SyntaxTreeNode<"ClassOrStructConstraint"> {
    classOrStructKeyword?: SyntaxToken;
    questionToken?: SyntaxToken;
    rawKind?: number;
}

interface CompilationUnitNode extends SyntaxTreeNode<"CompilationUnit"> {
    externs: ExternAliasDirectiveNode[];
    usings: UsingDirectiveNode[];
    attributeLists: AttributeListNode[];
    members: SyntaxTreeNode[];
    endOfFileToken?: SyntaxToken;
    rawKind?: number;
}

interface ConditionalAccessExpressionNode extends SyntaxTreeNode<"ConditionalAccessExpression"> {
    expression?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    whenNotNull?: SyntaxTreeNode;
    rawKind?: number;
}

interface ConditionalExpressionNode extends SyntaxTreeNode<"ConditionalExpression"> {
    condition?: SyntaxTreeNode;
    questionToken?: SyntaxToken;
    whenTrue?: SyntaxTreeNode;
    colonToken?: SyntaxToken;
    whenFalse?: SyntaxTreeNode;
    rawKind?: number;
}

interface ConstantPatternNode extends SyntaxTreeNode<"ConstantPattern"> {
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface ConstructorConstraintNode extends SyntaxTreeNode<"ConstructorConstraint"> {
    newKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface ConstructorDeclarationNode extends SyntaxTreeNode<"ConstructorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    identifier?: SyntaxToken;
    parameterList?: ParameterListNode;
    initializer?: ConstructorInitializerNode;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface ConstructorInitializerNode extends SyntaxTreeNode<"ConstructorInitializer"> {
    colonToken?: SyntaxToken;
    thisOrBaseKeyword?: SyntaxToken;
    argumentList?: ArgumentListNode;
    rawKind?: number;
}

interface ContinueStatementNode extends SyntaxTreeNode<"ContinueStatement"> {
    attributeLists: AttributeListNode[];
    continueKeyword?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface ConversionOperatorDeclarationNode extends SyntaxTreeNode<"ConversionOperatorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    implicitOrExplicitKeyword?: SyntaxToken;
    operatorKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    parameterList?: ParameterListNode;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface ConversionOperatorMemberCrefNode extends SyntaxTreeNode<"ConversionOperatorMemberCref"> {
    implicitOrExplicitKeyword?: SyntaxToken;
    operatorKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    parameters?: CrefParameterListNode;
    rawKind?: number;
}

interface CrefBracketedParameterListNode extends SyntaxTreeNode<"CrefBracketedParameterList"> {
    openBracketToken?: SyntaxToken;
    parameters: CrefParameterNode[];
    closeBracketToken?: SyntaxToken;
    rawKind?: number;
}

interface CrefParameterListNode extends SyntaxTreeNode<"CrefParameterList"> {
    openParenToken?: SyntaxToken;
    parameters: CrefParameterNode[];
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface CrefParameterNode extends SyntaxTreeNode<"CrefParameter"> {
    refKindKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    refOrOutKeyword?: SyntaxToken;
    rawKind?: number;
}

interface DeclarationExpressionNode extends SyntaxTreeNode<"DeclarationExpression"> {
    type?: SyntaxTreeNode;
    designation?: SyntaxTreeNode;
    rawKind?: number;
}

interface DeclarationPatternNode extends SyntaxTreeNode<"DeclarationPattern"> {
    type?: SyntaxTreeNode;
    designation?: SyntaxTreeNode;
    rawKind?: number;
}

interface DefaultConstraintNode extends SyntaxTreeNode<"DefaultConstraint"> {
    defaultKeyword?: SyntaxToken;
    rawKind?: number;
}

interface DefaultExpressionNode extends SyntaxTreeNode<"DefaultExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface DefaultSwitchLabelNode extends SyntaxTreeNode<"DefaultSwitchLabel"> {
    keyword?: SyntaxToken;
    colonToken?: SyntaxToken;
    rawKind?: number;
}

interface DefineDirectiveTriviaNode extends SyntaxTreeNode<"DefineDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    defineKeyword?: SyntaxToken;
    name?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface DelegateDeclarationNode extends SyntaxTreeNode<"DelegateDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    delegateKeyword?: SyntaxToken;
    returnType?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    parameterList?: ParameterListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    semicolonToken?: SyntaxToken;
    arity?: number;
    rawKind?: number;
}

interface DestructorDeclarationNode extends SyntaxTreeNode<"DestructorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    tildeToken?: SyntaxToken;
    identifier?: SyntaxToken;
    parameterList?: ParameterListNode;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface DiscardDesignationNode extends SyntaxTreeNode<"DiscardDesignation"> {
    underscoreToken?: SyntaxToken;
    rawKind?: number;
}

interface DiscardPatternNode extends SyntaxTreeNode<"DiscardPattern"> {
    underscoreToken?: SyntaxToken;
    rawKind?: number;
}

interface DocumentationCommentTriviaNode extends SyntaxTreeNode<"DocumentationCommentTrivia"> {
    content: SyntaxTreeNode[];
    endOfComment?: SyntaxToken;
    rawKind?: number;
}

interface DoStatementNode extends SyntaxTreeNode<"DoStatement"> {
    attributeLists: AttributeListNode[];
    doKeyword?: SyntaxToken;
    statement?: SyntaxTreeNode;
    whileKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface ElementAccessExpressionNode extends SyntaxTreeNode<"ElementAccessExpression"> {
    expression?: SyntaxTreeNode;
    argumentList?: BracketedArgumentListNode;
    rawKind?: number;
}

interface ElementBindingExpressionNode extends SyntaxTreeNode<"ElementBindingExpression"> {
    argumentList?: BracketedArgumentListNode;
    rawKind?: number;
}

interface ElifDirectiveTriviaNode extends SyntaxTreeNode<"ElifDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    elifKeyword?: SyntaxToken;
    condition?: SyntaxTreeNode;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    branchTaken?: boolean;
    conditionValue?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface ElseClauseNode extends SyntaxTreeNode<"ElseClause"> {
    elseKeyword?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface ElseDirectiveTriviaNode extends SyntaxTreeNode<"ElseDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    elseKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    branchTaken?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface EmptyStatementNode extends SyntaxTreeNode<"EmptyStatement"> {
    attributeLists: AttributeListNode[];
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface EndIfDirectiveTriviaNode extends SyntaxTreeNode<"EndIfDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    endIfKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface EndRegionDirectiveTriviaNode extends SyntaxTreeNode<"EndRegionDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    endRegionKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface EnumDeclarationNode extends SyntaxTreeNode<"EnumDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    enumKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    baseList?: BaseListNode;
    openBraceToken?: SyntaxToken;
    members: EnumMemberDeclarationNode[];
    closeBraceToken?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface EnumMemberDeclarationNode extends SyntaxTreeNode<"EnumMemberDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    identifier?: SyntaxToken;
    equalsValue?: EqualsValueClauseNode;
    rawKind?: number;
}

interface EqualsValueClauseNode extends SyntaxTreeNode<"EqualsValueClause"> {
    equalsToken?: SyntaxToken;
    value?: SyntaxTreeNode;
    rawKind?: number;
}

interface ErrorDirectiveTriviaNode extends SyntaxTreeNode<"ErrorDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    errorKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface EventDeclarationNode extends SyntaxTreeNode<"EventDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    eventKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: ExplicitInterfaceSpecifierNode;
    identifier?: SyntaxToken;
    accessorList?: AccessorListNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface EventFieldDeclarationNode extends SyntaxTreeNode<"EventFieldDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    eventKeyword?: SyntaxToken;
    declaration?: VariableDeclarationNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface ExplicitInterfaceSpecifierNode extends SyntaxTreeNode<"ExplicitInterfaceSpecifier"> {
    name?: SyntaxTreeNode;
    dotToken?: SyntaxToken;
    rawKind?: number;
}

interface ExpressionStatementNode extends SyntaxTreeNode<"ExpressionStatement"> {
    attributeLists: AttributeListNode[];
    expression?: SyntaxTreeNode;
    semicolonToken?: SyntaxToken;
    allowsAnyExpression?: boolean;
    rawKind?: number;
}

interface ExternAliasDirectiveNode extends SyntaxTreeNode<"ExternAliasDirective"> {
    externKeyword?: SyntaxToken;
    aliasKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface FieldDeclarationNode extends SyntaxTreeNode<"FieldDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    declaration?: VariableDeclarationNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface FinallyClauseNode extends SyntaxTreeNode<"FinallyClause"> {
    finallyKeyword?: SyntaxToken;
    block?: BlockNode;
    rawKind?: number;
}

interface FixedStatementNode extends SyntaxTreeNode<"FixedStatement"> {
    attributeLists: AttributeListNode[];
    fixedKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    declaration?: VariableDeclarationNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface ForEachStatementNode extends SyntaxTreeNode<"ForEachStatement"> {
    attributeLists: AttributeListNode[];
    awaitKeyword?: SyntaxToken;
    forEachKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    inKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface ForEachVariableStatementNode extends SyntaxTreeNode<"ForEachVariableStatement"> {
    attributeLists: AttributeListNode[];
    awaitKeyword?: SyntaxToken;
    forEachKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    variable?: SyntaxTreeNode;
    inKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface ForStatementNode extends SyntaxTreeNode<"ForStatement"> {
    attributeLists: AttributeListNode[];
    forKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    declaration?: VariableDeclarationNode;
    initializers: SyntaxTreeNode[];
    firstSemicolonToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    secondSemicolonToken?: SyntaxToken;
    incrementors: SyntaxTreeNode[];
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface FromClauseNode extends SyntaxTreeNode<"FromClause"> {
    fromKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    inKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface FunctionPointerCallingConventionNode extends SyntaxTreeNode<"FunctionPointerCallingConvention"> {
    managedOrUnmanagedKeyword?: SyntaxToken;
    unmanagedCallingConventionList?: FunctionPointerUnmanagedCallingConventionListNode;
    rawKind?: number;
}

interface FunctionPointerParameterListNode extends SyntaxTreeNode<"FunctionPointerParameterList"> {
    lessThanToken?: SyntaxToken;
    parameters: FunctionPointerParameterNode[];
    greaterThanToken?: SyntaxToken;
    rawKind?: number;
}

interface FunctionPointerParameterNode extends SyntaxTreeNode<"FunctionPointerParameter"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    rawKind?: number;
}

interface FunctionPointerTypeNode extends SyntaxTreeNode<"FunctionPointerType"> {
    delegateKeyword?: SyntaxToken;
    asteriskToken?: SyntaxToken;
    callingConvention?: FunctionPointerCallingConventionNode;
    parameterList?: FunctionPointerParameterListNode;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface FunctionPointerUnmanagedCallingConventionListNode extends SyntaxTreeNode<"FunctionPointerUnmanagedCallingConventionList"> {
    openBracketToken?: SyntaxToken;
    callingConventions: FunctionPointerUnmanagedCallingConventionNode[];
    closeBracketToken?: SyntaxToken;
    rawKind?: number;
}

interface FunctionPointerUnmanagedCallingConventionNode extends SyntaxTreeNode<"FunctionPointerUnmanagedCallingConvention"> {
    name?: SyntaxToken;
    rawKind?: number;
}

interface GenericNameNode extends SyntaxTreeNode<"GenericName"> {
    identifier?: SyntaxToken;
    typeArgumentList?: TypeArgumentListNode;
    isUnboundGenericName?: boolean;
    arity?: number;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface GlobalStatementNode extends SyntaxTreeNode<"GlobalStatement"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface GotoStatementNode extends SyntaxTreeNode<"GotoStatement"> {
    attributeLists: AttributeListNode[];
    gotoKeyword?: SyntaxToken;
    caseOrDefaultKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface GroupClauseNode extends SyntaxTreeNode<"GroupClause"> {
    groupKeyword?: SyntaxToken;
    groupExpression?: SyntaxTreeNode;
    byKeyword?: SyntaxToken;
    byExpression?: SyntaxTreeNode;
    rawKind?: number;
}

interface IdentifierNameNode extends SyntaxTreeNode<"IdentifierName"> {
    identifier?: SyntaxToken;
    arity?: number;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface IfDirectiveTriviaNode extends SyntaxTreeNode<"IfDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    ifKeyword?: SyntaxToken;
    condition?: SyntaxTreeNode;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    branchTaken?: boolean;
    conditionValue?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface IfStatementNode extends SyntaxTreeNode<"IfStatement"> {
    attributeLists: AttributeListNode[];
    ifKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    else?: ElseClauseNode;
    rawKind?: number;
}

interface ImplicitArrayCreationExpressionNode extends SyntaxTreeNode<"ImplicitArrayCreationExpression"> {
    newKeyword?: SyntaxToken;
    openBracketToken?: SyntaxToken;
    commas: SyntaxToken[];
    closeBracketToken?: SyntaxToken;
    initializer?: InitializerExpressionNode;
    rawKind?: number;
}

interface ImplicitElementAccessNode extends SyntaxTreeNode<"ImplicitElementAccess"> {
    argumentList?: BracketedArgumentListNode;
    rawKind?: number;
}

interface ImplicitObjectCreationExpressionNode extends SyntaxTreeNode<"ImplicitObjectCreationExpression"> {
    newKeyword?: SyntaxToken;
    argumentList?: ArgumentListNode;
    initializer?: InitializerExpressionNode;
    rawKind?: number;
}

interface ImplicitStackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"ImplicitStackAllocArrayCreationExpression"> {
    stackAllocKeyword?: SyntaxToken;
    openBracketToken?: SyntaxToken;
    closeBracketToken?: SyntaxToken;
    initializer?: InitializerExpressionNode;
    rawKind?: number;
}

interface IncompleteMemberNode extends SyntaxTreeNode<"IncompleteMember"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    rawKind?: number;
}

interface IndexerDeclarationNode extends SyntaxTreeNode<"IndexerDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: ExplicitInterfaceSpecifierNode;
    thisKeyword?: SyntaxToken;
    parameterList?: BracketedParameterListNode;
    accessorList?: AccessorListNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface IndexerMemberCrefNode extends SyntaxTreeNode<"IndexerMemberCref"> {
    thisKeyword?: SyntaxToken;
    parameters?: CrefBracketedParameterListNode;
    rawKind?: number;
}

interface InitializerExpressionNode extends SyntaxTreeNode<"InitializerExpression"> {
    openBraceToken?: SyntaxToken;
    expressions: SyntaxTreeNode[];
    closeBraceToken?: SyntaxToken;
    rawKind?: number;
}

interface InterfaceDeclarationNode extends SyntaxTreeNode<"InterfaceDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    baseList?: BaseListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    openBraceToken?: SyntaxToken;
    members: SyntaxTreeNode[];
    closeBraceToken?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    arity?: number;
    rawKind?: number;
}

interface InterpolatedStringExpressionNode extends SyntaxTreeNode<"InterpolatedStringExpression"> {
    stringStartToken?: SyntaxToken;
    contents: SyntaxTreeNode[];
    stringEndToken?: SyntaxToken;
    rawKind?: number;
}

interface InterpolatedStringTextNode extends SyntaxTreeNode<"InterpolatedStringText"> {
    textToken?: SyntaxToken;
    rawKind?: number;
}

interface InterpolationAlignmentClauseNode extends SyntaxTreeNode<"InterpolationAlignmentClause"> {
    commaToken?: SyntaxToken;
    value?: SyntaxTreeNode;
    rawKind?: number;
}

interface InterpolationFormatClauseNode extends SyntaxTreeNode<"InterpolationFormatClause"> {
    colonToken?: SyntaxToken;
    formatStringToken?: SyntaxToken;
    rawKind?: number;
}

interface InterpolationNode extends SyntaxTreeNode<"Interpolation"> {
    openBraceToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    alignmentClause?: InterpolationAlignmentClauseNode;
    formatClause?: InterpolationFormatClauseNode;
    closeBraceToken?: SyntaxToken;
    rawKind?: number;
}

interface InvocationExpressionNode extends SyntaxTreeNode<"InvocationExpression"> {
    expression?: SyntaxTreeNode;
    argumentList?: ArgumentListNode;
    rawKind?: number;
}

interface IsPatternExpressionNode extends SyntaxTreeNode<"IsPatternExpression"> {
    expression?: SyntaxTreeNode;
    isKeyword?: SyntaxToken;
    pattern?: SyntaxTreeNode;
    rawKind?: number;
}

interface JoinClauseNode extends SyntaxTreeNode<"JoinClause"> {
    joinKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    inKeyword?: SyntaxToken;
    inExpression?: SyntaxTreeNode;
    onKeyword?: SyntaxToken;
    leftExpression?: SyntaxTreeNode;
    equalsKeyword?: SyntaxToken;
    rightExpression?: SyntaxTreeNode;
    into?: JoinIntoClauseNode;
    rawKind?: number;
}

interface JoinIntoClauseNode extends SyntaxTreeNode<"JoinIntoClause"> {
    intoKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    rawKind?: number;
}

interface LabeledStatementNode extends SyntaxTreeNode<"LabeledStatement"> {
    attributeLists: AttributeListNode[];
    identifier?: SyntaxToken;
    colonToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface LetClauseNode extends SyntaxTreeNode<"LetClause"> {
    letKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    equalsToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface LineDirectiveTriviaNode extends SyntaxTreeNode<"LineDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    lineKeyword?: SyntaxToken;
    line?: SyntaxToken;
    file?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface LiteralExpressionNode extends SyntaxTreeNode<"LiteralExpression"> {
    token?: SyntaxToken;
    rawKind?: number;
}

interface LoadDirectiveTriviaNode extends SyntaxTreeNode<"LoadDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    loadKeyword?: SyntaxToken;
    file?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface LocalDeclarationStatementNode extends SyntaxTreeNode<"LocalDeclarationStatement"> {
    attributeLists: AttributeListNode[];
    awaitKeyword?: SyntaxToken;
    usingKeyword?: SyntaxToken;
    modifiers: SyntaxToken[];
    declaration?: VariableDeclarationNode;
    semicolonToken?: SyntaxToken;
    isConst?: boolean;
    rawKind?: number;
}

interface LocalFunctionStatementNode extends SyntaxTreeNode<"LocalFunctionStatement"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    returnType?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    parameterList?: ParameterListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface LockStatementNode extends SyntaxTreeNode<"LockStatement"> {
    attributeLists: AttributeListNode[];
    lockKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface MakeRefExpressionNode extends SyntaxTreeNode<"MakeRefExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface MemberAccessExpressionNode extends SyntaxTreeNode<"MemberAccessExpression"> {
    expression?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    name?: SyntaxTreeNode;
    rawKind?: number;
}

interface MemberBindingExpressionNode extends SyntaxTreeNode<"MemberBindingExpression"> {
    operatorToken?: SyntaxToken;
    name?: SyntaxTreeNode;
    rawKind?: number;
}

interface MethodDeclarationNode extends SyntaxTreeNode<"MethodDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    returnType?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: ExplicitInterfaceSpecifierNode;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    parameterList?: ParameterListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
    arity?: number;
    rawKind?: number;
}

interface NameColonNode extends SyntaxTreeNode<"NameColon"> {
    name?: IdentifierNameNode;
    colonToken?: SyntaxToken;
    rawKind?: number;
}

interface NameEqualsNode extends SyntaxTreeNode<"NameEquals"> {
    name?: IdentifierNameNode;
    equalsToken?: SyntaxToken;
    rawKind?: number;
}

interface NameMemberCrefNode extends SyntaxTreeNode<"NameMemberCref"> {
    name?: SyntaxTreeNode;
    parameters?: CrefParameterListNode;
    rawKind?: number;
}

interface NamespaceDeclarationNode extends SyntaxTreeNode<"NamespaceDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    namespaceKeyword?: SyntaxToken;
    name?: SyntaxTreeNode;
    openBraceToken?: SyntaxToken;
    externs: ExternAliasDirectiveNode[];
    usings: UsingDirectiveNode[];
    members: SyntaxTreeNode[];
    closeBraceToken?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface NullableDirectiveTriviaNode extends SyntaxTreeNode<"NullableDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    nullableKeyword?: SyntaxToken;
    settingToken?: SyntaxToken;
    targetToken?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface NullableTypeNode extends SyntaxTreeNode<"NullableType"> {
    elementType?: SyntaxTreeNode;
    questionToken?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface ObjectCreationExpressionNode extends SyntaxTreeNode<"ObjectCreationExpression"> {
    newKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    argumentList?: ArgumentListNode;
    initializer?: InitializerExpressionNode;
    rawKind?: number;
}

interface OmittedArraySizeExpressionNode extends SyntaxTreeNode<"OmittedArraySizeExpression"> {
    omittedArraySizeExpressionToken?: SyntaxToken;
    rawKind?: number;
}

interface OmittedTypeArgumentNode extends SyntaxTreeNode<"OmittedTypeArgument"> {
    omittedTypeArgumentToken?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface OperatorDeclarationNode extends SyntaxTreeNode<"OperatorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    returnType?: SyntaxTreeNode;
    operatorKeyword?: SyntaxToken;
    operatorToken?: SyntaxToken;
    parameterList?: ParameterListNode;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface OperatorMemberCrefNode extends SyntaxTreeNode<"OperatorMemberCref"> {
    operatorKeyword?: SyntaxToken;
    operatorToken?: SyntaxToken;
    parameters?: CrefParameterListNode;
    rawKind?: number;
}

interface OrderByClauseNode extends SyntaxTreeNode<"OrderByClause"> {
    orderByKeyword?: SyntaxToken;
    orderings: OrderingNode[];
    rawKind?: number;
}

interface OrderingNode extends SyntaxTreeNode<"Ordering"> {
    expression?: SyntaxTreeNode;
    ascendingOrDescendingKeyword?: SyntaxToken;
    rawKind?: number;
}

interface ParameterListNode extends SyntaxTreeNode<"ParameterList"> {
    openParenToken?: SyntaxToken;
    parameters: ParameterNode[];
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface ParameterNode extends SyntaxTreeNode<"Parameter"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    default?: EqualsValueClauseNode;
    rawKind?: number;
}

interface ParenthesizedExpressionNode extends SyntaxTreeNode<"ParenthesizedExpression"> {
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface ParenthesizedLambdaExpressionNode extends SyntaxTreeNode<"ParenthesizedLambdaExpression"> {
    modifiers: SyntaxToken[];
    parameterList?: ParameterListNode;
    arrowToken?: SyntaxToken;
    block?: BlockNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword?: SyntaxToken;
    body?: SyntaxTreeNode;
    rawKind?: number;
}

interface ParenthesizedPatternNode extends SyntaxTreeNode<"ParenthesizedPattern"> {
    openParenToken?: SyntaxToken;
    pattern?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface ParenthesizedVariableDesignationNode extends SyntaxTreeNode<"ParenthesizedVariableDesignation"> {
    openParenToken?: SyntaxToken;
    variables: SyntaxTreeNode[];
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface PointerTypeNode extends SyntaxTreeNode<"PointerType"> {
    elementType?: SyntaxTreeNode;
    asteriskToken?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface PositionalPatternClauseNode extends SyntaxTreeNode<"PositionalPatternClause"> {
    openParenToken?: SyntaxToken;
    subpatterns: SubpatternNode[];
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface PostfixUnaryExpressionNode extends SyntaxTreeNode<"PostfixUnaryExpression"> {
    operand?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    rawKind?: number;
}

interface PragmaChecksumDirectiveTriviaNode extends SyntaxTreeNode<"PragmaChecksumDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    pragmaKeyword?: SyntaxToken;
    checksumKeyword?: SyntaxToken;
    file?: SyntaxToken;
    guid?: SyntaxToken;
    bytes?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface PragmaWarningDirectiveTriviaNode extends SyntaxTreeNode<"PragmaWarningDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    pragmaKeyword?: SyntaxToken;
    warningKeyword?: SyntaxToken;
    disableOrRestoreKeyword?: SyntaxToken;
    errorCodes: SyntaxTreeNode[];
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface PredefinedTypeNode extends SyntaxTreeNode<"PredefinedType"> {
    keyword?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface PrefixUnaryExpressionNode extends SyntaxTreeNode<"PrefixUnaryExpression"> {
    operatorToken?: SyntaxToken;
    operand?: SyntaxTreeNode;
    rawKind?: number;
}

interface PrimaryConstructorBaseTypeNode extends SyntaxTreeNode<"PrimaryConstructorBaseType"> {
    type?: SyntaxTreeNode;
    argumentList?: ArgumentListNode;
    rawKind?: number;
}

interface PropertyDeclarationNode extends SyntaxTreeNode<"PropertyDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: ExplicitInterfaceSpecifierNode;
    identifier?: SyntaxToken;
    accessorList?: AccessorListNode;
    expressionBody?: ArrowExpressionClauseNode;
    initializer?: EqualsValueClauseNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface PropertyPatternClauseNode extends SyntaxTreeNode<"PropertyPatternClause"> {
    openBraceToken?: SyntaxToken;
    subpatterns: SubpatternNode[];
    closeBraceToken?: SyntaxToken;
    rawKind?: number;
}

interface QualifiedCrefNode extends SyntaxTreeNode<"QualifiedCref"> {
    container?: SyntaxTreeNode;
    dotToken?: SyntaxToken;
    member?: SyntaxTreeNode;
    rawKind?: number;
}

interface QualifiedNameNode extends SyntaxTreeNode<"QualifiedName"> {
    left?: SyntaxTreeNode;
    dotToken?: SyntaxToken;
    right?: SyntaxTreeNode;
    arity?: number;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface QueryBodyNode extends SyntaxTreeNode<"QueryBody"> {
    clauses: SyntaxTreeNode[];
    selectOrGroup?: SyntaxTreeNode;
    continuation?: QueryContinuationNode;
    rawKind?: number;
}

interface QueryContinuationNode extends SyntaxTreeNode<"QueryContinuation"> {
    intoKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    body?: QueryBodyNode;
    rawKind?: number;
}

interface QueryExpressionNode extends SyntaxTreeNode<"QueryExpression"> {
    fromClause?: FromClauseNode;
    body?: QueryBodyNode;
    rawKind?: number;
}

interface RangeExpressionNode extends SyntaxTreeNode<"RangeExpression"> {
    leftOperand?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    rightOperand?: SyntaxTreeNode;
    rawKind?: number;
}

interface RecordDeclarationNode extends SyntaxTreeNode<"RecordDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    parameterList?: ParameterListNode;
    baseList?: BaseListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    openBraceToken?: SyntaxToken;
    members: SyntaxTreeNode[];
    closeBraceToken?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    arity?: number;
    rawKind?: number;
}

interface RecursivePatternNode extends SyntaxTreeNode<"RecursivePattern"> {
    type?: SyntaxTreeNode;
    positionalPatternClause?: PositionalPatternClauseNode;
    propertyPatternClause?: PropertyPatternClauseNode;
    designation?: SyntaxTreeNode;
    rawKind?: number;
}

interface ReferenceDirectiveTriviaNode extends SyntaxTreeNode<"ReferenceDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    referenceKeyword?: SyntaxToken;
    file?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface RefExpressionNode extends SyntaxTreeNode<"RefExpression"> {
    refKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface RefTypeExpressionNode extends SyntaxTreeNode<"RefTypeExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface RefTypeNode extends SyntaxTreeNode<"RefType"> {
    refKeyword?: SyntaxToken;
    readOnlyKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface RefValueExpressionNode extends SyntaxTreeNode<"RefValueExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    comma?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface RegionDirectiveTriviaNode extends SyntaxTreeNode<"RegionDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    regionKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface RelationalPatternNode extends SyntaxTreeNode<"RelationalPattern"> {
    operatorToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface ReturnStatementNode extends SyntaxTreeNode<"ReturnStatement"> {
    attributeLists: AttributeListNode[];
    returnKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface SelectClauseNode extends SyntaxTreeNode<"SelectClause"> {
    selectKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface ShebangDirectiveTriviaNode extends SyntaxTreeNode<"ShebangDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    exclamationToken?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface SimpleBaseTypeNode extends SyntaxTreeNode<"SimpleBaseType"> {
    type?: SyntaxTreeNode;
    rawKind?: number;
}

interface SimpleLambdaExpressionNode extends SyntaxTreeNode<"SimpleLambdaExpression"> {
    modifiers: SyntaxToken[];
    parameter?: ParameterNode;
    arrowToken?: SyntaxToken;
    block?: BlockNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword?: SyntaxToken;
    body?: SyntaxTreeNode;
    rawKind?: number;
}

interface SingleVariableDesignationNode extends SyntaxTreeNode<"SingleVariableDesignation"> {
    identifier?: SyntaxToken;
    rawKind?: number;
}

interface SizeOfExpressionNode extends SyntaxTreeNode<"SizeOfExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface SkippedTokensTriviaNode extends SyntaxTreeNode<"SkippedTokensTrivia"> {
    tokens: SyntaxToken[];
    rawKind?: number;
}

interface StackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"StackAllocArrayCreationExpression"> {
    stackAllocKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    initializer?: InitializerExpressionNode;
    rawKind?: number;
}

interface StructDeclarationNode extends SyntaxTreeNode<"StructDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    baseList?: BaseListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    openBraceToken?: SyntaxToken;
    members: SyntaxTreeNode[];
    closeBraceToken?: SyntaxToken;
    semicolonToken?: SyntaxToken;
    arity?: number;
    rawKind?: number;
}

interface SubpatternNode extends SyntaxTreeNode<"Subpattern"> {
    nameColon?: NameColonNode;
    pattern?: SyntaxTreeNode;
    rawKind?: number;
}

interface SwitchExpressionArmNode extends SyntaxTreeNode<"SwitchExpressionArm"> {
    pattern?: SyntaxTreeNode;
    whenClause?: WhenClauseNode;
    equalsGreaterThanToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface SwitchExpressionNode extends SyntaxTreeNode<"SwitchExpression"> {
    governingExpression?: SyntaxTreeNode;
    switchKeyword?: SyntaxToken;
    openBraceToken?: SyntaxToken;
    arms: SwitchExpressionArmNode[];
    closeBraceToken?: SyntaxToken;
    rawKind?: number;
}

interface SwitchSectionNode extends SyntaxTreeNode<"SwitchSection"> {
    labels: SyntaxTreeNode[];
    statements: SyntaxTreeNode[];
    rawKind?: number;
}

interface SwitchStatementNode extends SyntaxTreeNode<"SwitchStatement"> {
    attributeLists: AttributeListNode[];
    switchKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    openBraceToken?: SyntaxToken;
    sections: SwitchSectionNode[];
    closeBraceToken?: SyntaxToken;
    rawKind?: number;
}

interface ThisExpressionNode extends SyntaxTreeNode<"ThisExpression"> {
    token?: SyntaxToken;
    rawKind?: number;
}

interface ThrowExpressionNode extends SyntaxTreeNode<"ThrowExpression"> {
    throwKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    rawKind?: number;
}

interface ThrowStatementNode extends SyntaxTreeNode<"ThrowStatement"> {
    attributeLists: AttributeListNode[];
    throwKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface TryStatementNode extends SyntaxTreeNode<"TryStatement"> {
    attributeLists: AttributeListNode[];
    tryKeyword?: SyntaxToken;
    block?: BlockNode;
    catches: CatchClauseNode[];
    finally?: FinallyClauseNode;
    rawKind?: number;
}

interface TupleElementNode extends SyntaxTreeNode<"TupleElement"> {
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    rawKind?: number;
}

interface TupleExpressionNode extends SyntaxTreeNode<"TupleExpression"> {
    openParenToken?: SyntaxToken;
    arguments: ArgumentNode[];
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface TupleTypeNode extends SyntaxTreeNode<"TupleType"> {
    openParenToken?: SyntaxToken;
    elements: TupleElementNode[];
    closeParenToken?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
    rawKind?: number;
}

interface TypeArgumentListNode extends SyntaxTreeNode<"TypeArgumentList"> {
    lessThanToken?: SyntaxToken;
    arguments: SyntaxTreeNode[];
    greaterThanToken?: SyntaxToken;
    rawKind?: number;
}

interface TypeConstraintNode extends SyntaxTreeNode<"TypeConstraint"> {
    type?: SyntaxTreeNode;
    rawKind?: number;
}

interface TypeCrefNode extends SyntaxTreeNode<"TypeCref"> {
    type?: SyntaxTreeNode;
    rawKind?: number;
}

interface TypeOfExpressionNode extends SyntaxTreeNode<"TypeOfExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    rawKind?: number;
}

interface TypeParameterConstraintClauseNode extends SyntaxTreeNode<"TypeParameterConstraintClause"> {
    whereKeyword?: SyntaxToken;
    name?: IdentifierNameNode;
    colonToken?: SyntaxToken;
    constraints: SyntaxTreeNode[];
    rawKind?: number;
}

interface TypeParameterListNode extends SyntaxTreeNode<"TypeParameterList"> {
    lessThanToken?: SyntaxToken;
    parameters: TypeParameterNode[];
    greaterThanToken?: SyntaxToken;
    rawKind?: number;
}

interface TypeParameterNode extends SyntaxTreeNode<"TypeParameter"> {
    attributeLists: AttributeListNode[];
    varianceKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    rawKind?: number;
}

interface TypePatternNode extends SyntaxTreeNode<"TypePattern"> {
    type?: SyntaxTreeNode;
    rawKind?: number;
}

interface UnaryPatternNode extends SyntaxTreeNode<"UnaryPattern"> {
    operatorToken?: SyntaxToken;
    pattern?: SyntaxTreeNode;
    rawKind?: number;
}

interface UndefDirectiveTriviaNode extends SyntaxTreeNode<"UndefDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    undefKeyword?: SyntaxToken;
    name?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface UnsafeStatementNode extends SyntaxTreeNode<"UnsafeStatement"> {
    attributeLists: AttributeListNode[];
    unsafeKeyword?: SyntaxToken;
    block?: BlockNode;
    rawKind?: number;
}

interface UsingDirectiveNode extends SyntaxTreeNode<"UsingDirective"> {
    usingKeyword?: SyntaxToken;
    staticKeyword?: SyntaxToken;
    alias?: NameEqualsNode;
    name?: SyntaxTreeNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

interface UsingStatementNode extends SyntaxTreeNode<"UsingStatement"> {
    attributeLists: AttributeListNode[];
    awaitKeyword?: SyntaxToken;
    usingKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    declaration?: VariableDeclarationNode;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface VariableDeclarationNode extends SyntaxTreeNode<"VariableDeclaration"> {
    type?: SyntaxTreeNode;
    variables: VariableDeclaratorNode[];
    rawKind?: number;
}

interface VariableDeclaratorNode extends SyntaxTreeNode<"VariableDeclarator"> {
    identifier?: SyntaxToken;
    argumentList?: BracketedArgumentListNode;
    initializer?: EqualsValueClauseNode;
    rawKind?: number;
}

interface VarPatternNode extends SyntaxTreeNode<"VarPattern"> {
    varKeyword?: SyntaxToken;
    designation?: SyntaxTreeNode;
    rawKind?: number;
}

interface WarningDirectiveTriviaNode extends SyntaxTreeNode<"WarningDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    warningKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
    rawKind?: number;
}

interface WhenClauseNode extends SyntaxTreeNode<"WhenClause"> {
    whenKeyword?: SyntaxToken;
    condition?: SyntaxTreeNode;
    rawKind?: number;
}

interface WhereClauseNode extends SyntaxTreeNode<"WhereClause"> {
    whereKeyword?: SyntaxToken;
    condition?: SyntaxTreeNode;
    rawKind?: number;
}

interface WhileStatementNode extends SyntaxTreeNode<"WhileStatement"> {
    attributeLists: AttributeListNode[];
    whileKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    rawKind?: number;
}

interface WithExpressionNode extends SyntaxTreeNode<"WithExpression"> {
    expression?: SyntaxTreeNode;
    withKeyword?: SyntaxToken;
    initializer?: InitializerExpressionNode;
    rawKind?: number;
}

interface XmlCDataSectionNode extends SyntaxTreeNode<"XmlCDataSection"> {
    startCDataToken?: SyntaxToken;
    textTokens: SyntaxToken[];
    endCDataToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlCommentNode extends SyntaxTreeNode<"XmlComment"> {
    lessThanExclamationMinusMinusToken?: SyntaxToken;
    textTokens: SyntaxToken[];
    minusMinusGreaterThanToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlCrefAttributeNode extends SyntaxTreeNode<"XmlCrefAttribute"> {
    name?: XmlNameNode;
    equalsToken?: SyntaxToken;
    startQuoteToken?: SyntaxToken;
    cref?: SyntaxTreeNode;
    endQuoteToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlElementEndTagNode extends SyntaxTreeNode<"XmlElementEndTag"> {
    lessThanSlashToken?: SyntaxToken;
    name?: XmlNameNode;
    greaterThanToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlElementStartTagNode extends SyntaxTreeNode<"XmlElementStartTag"> {
    lessThanToken?: SyntaxToken;
    name?: XmlNameNode;
    attributes: SyntaxTreeNode[];
    greaterThanToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlElementNode extends SyntaxTreeNode<"XmlElement"> {
    startTag?: XmlElementStartTagNode;
    content: SyntaxTreeNode[];
    endTag?: XmlElementEndTagNode;
    rawKind?: number;
}

interface XmlEmptyElementNode extends SyntaxTreeNode<"XmlEmptyElement"> {
    lessThanToken?: SyntaxToken;
    name?: XmlNameNode;
    attributes: SyntaxTreeNode[];
    slashGreaterThanToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlNameAttributeNode extends SyntaxTreeNode<"XmlNameAttribute"> {
    name?: XmlNameNode;
    equalsToken?: SyntaxToken;
    startQuoteToken?: SyntaxToken;
    identifier?: IdentifierNameNode;
    endQuoteToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlNameNode extends SyntaxTreeNode<"XmlName"> {
    prefix?: XmlPrefixNode;
    localName?: SyntaxToken;
    rawKind?: number;
}

interface XmlPrefixNode extends SyntaxTreeNode<"XmlPrefix"> {
    prefix?: SyntaxToken;
    colonToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlProcessingInstructionNode extends SyntaxTreeNode<"XmlProcessingInstruction"> {
    startProcessingInstructionToken?: SyntaxToken;
    name?: XmlNameNode;
    textTokens: SyntaxToken[];
    endProcessingInstructionToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlTextAttributeNode extends SyntaxTreeNode<"XmlTextAttribute"> {
    name?: XmlNameNode;
    equalsToken?: SyntaxToken;
    startQuoteToken?: SyntaxToken;
    textTokens: SyntaxToken[];
    endQuoteToken?: SyntaxToken;
    rawKind?: number;
}

interface XmlTextNode extends SyntaxTreeNode<"XmlText"> {
    textTokens: SyntaxToken[];
    rawKind?: number;
}

interface YieldStatementNode extends SyntaxTreeNode<"YieldStatement"> {
    attributeLists: AttributeListNode[];
    yieldKeyword?: SyntaxToken;
    returnOrBreakKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    semicolonToken?: SyntaxToken;
    rawKind?: number;
}

