import { SyntaxToken, SyntaxTreeNode } from "./SyntaxTreeNode";

interface AccessorDeclarationNode extends SyntaxTreeNode<"AccessorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
}

interface AccessorListNode extends SyntaxTreeNode<"AccessorList"> {
    accessors: AccessorDeclarationNode[];
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
}

interface AnonymousMethodExpressionNode extends SyntaxTreeNode<"AnonymousMethodExpression"> {
    modifiers: SyntaxToken[];
    delegateKeyword?: SyntaxToken;
    parameterList?: ParameterListNode;
    block?: BlockNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword?: SyntaxToken;
    body?: SyntaxTreeNode;
}

interface AnonymousObjectCreationExpressionNode extends SyntaxTreeNode<"AnonymousObjectCreationExpression"> {
    newKeyword?: SyntaxToken;
    initializers: AnonymousObjectMemberDeclaratorNode[];
}

interface AnonymousObjectMemberDeclaratorNode extends SyntaxTreeNode<"AnonymousObjectMemberDeclarator"> {
    nameEquals?: NameEqualsNode;
    expression?: SyntaxTreeNode;
}

interface ArgumentListNode extends SyntaxTreeNode<"ArgumentList"> {
    openParenToken?: SyntaxToken;
    arguments: ArgumentNode[];
    closeParenToken?: SyntaxToken;
}

interface ArgumentNode extends SyntaxTreeNode<"Argument"> {
    nameColon?: NameColonNode;
    refKindKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
    refOrOutKeyword?: SyntaxToken;
}

interface ArrayCreationExpressionNode extends SyntaxTreeNode<"ArrayCreationExpression"> {
    newKeyword?: SyntaxToken;
    type?: ArrayTypeNode;
    initializer?: InitializerExpressionNode;
}

interface ArrayRankSpecifierNode extends SyntaxTreeNode<"ArrayRankSpecifier"> {
    openBracketToken?: SyntaxToken;
    sizes: SyntaxTreeNode[];
    closeBracketToken?: SyntaxToken;
    rank?: number;
}

interface ArrayTypeNode extends SyntaxTreeNode<"ArrayType"> {
    elementType?: SyntaxTreeNode;
    rankSpecifiers: ArrayRankSpecifierNode[];
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
}

interface ArrowExpressionClauseNode extends SyntaxTreeNode<"ArrowExpressionClause"> {
    arrowToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface AssignmentExpressionNode extends SyntaxTreeNode<"AssignmentExpression"> {
    left?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    right?: SyntaxTreeNode;
}

interface AttributeArgumentListNode extends SyntaxTreeNode<"AttributeArgumentList"> {
    openParenToken?: SyntaxToken;
    arguments: AttributeArgumentNode[];
    closeParenToken?: SyntaxToken;
}

interface AttributeArgumentNode extends SyntaxTreeNode<"AttributeArgument"> {
    nameEquals?: NameEqualsNode;
    nameColon?: NameColonNode;
    expression?: SyntaxTreeNode;
}

interface AttributeListNode extends SyntaxTreeNode<"AttributeList"> {
    openBracketToken?: SyntaxToken;
    target?: AttributeTargetSpecifierNode;
    attributes: AttributeNode[];
    closeBracketToken?: SyntaxToken;
}

interface AttributeNode extends SyntaxTreeNode<"Attribute"> {
    name?: SyntaxTreeNode;
    argumentList?: AttributeArgumentListNode;
}

interface AttributeTargetSpecifierNode extends SyntaxTreeNode<"AttributeTargetSpecifier"> {
    identifier?: SyntaxToken;
    colonToken?: SyntaxToken;
}

interface AwaitExpressionNode extends SyntaxTreeNode<"AwaitExpression"> {
    awaitKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface BadDirectiveTriviaNode extends SyntaxTreeNode<"BadDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    identifier?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface BaseExpressionNode extends SyntaxTreeNode<"BaseExpression"> {
    token?: SyntaxToken;
}

interface BaseListNode extends SyntaxTreeNode<"BaseList"> {
    colonToken?: SyntaxToken;
    types: SyntaxTreeNode[];
}

interface BinaryExpressionNode extends SyntaxTreeNode<"BinaryExpression"> {
    left?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    right?: SyntaxTreeNode;
}

interface BinaryPatternNode extends SyntaxTreeNode<"BinaryPattern"> {
    left?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    right?: SyntaxTreeNode;
}

interface BlockNode extends SyntaxTreeNode<"Block"> {
    attributeLists: AttributeListNode[];
    statements: SyntaxTreeNode[];
}

interface BracketedArgumentListNode extends SyntaxTreeNode<"BracketedArgumentList"> {
    openBracketToken?: SyntaxToken;
    arguments: ArgumentNode[];
    closeBracketToken?: SyntaxToken;
}

interface BracketedParameterListNode extends SyntaxTreeNode<"BracketedParameterList"> {
    openBracketToken?: SyntaxToken;
    parameters: ParameterNode[];
    closeBracketToken?: SyntaxToken;
}

interface BreakStatementNode extends SyntaxTreeNode<"BreakStatement"> {
    attributeLists: AttributeListNode[];
    breakKeyword?: SyntaxToken;
}

interface CasePatternSwitchLabelNode extends SyntaxTreeNode<"CasePatternSwitchLabel"> {
    keyword?: SyntaxToken;
    pattern?: SyntaxTreeNode;
    whenClause?: WhenClauseNode;
    colonToken?: SyntaxToken;
}

interface CaseSwitchLabelNode extends SyntaxTreeNode<"CaseSwitchLabel"> {
    keyword?: SyntaxToken;
    value?: SyntaxTreeNode;
    colonToken?: SyntaxToken;
}

interface CastExpressionNode extends SyntaxTreeNode<"CastExpression"> {
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface CatchClauseNode extends SyntaxTreeNode<"CatchClause"> {
    catchKeyword?: SyntaxToken;
    declaration?: CatchDeclarationNode;
    filter?: CatchFilterClauseNode;
    block?: BlockNode;
}

interface CatchDeclarationNode extends SyntaxTreeNode<"CatchDeclaration"> {
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    closeParenToken?: SyntaxToken;
}

interface CatchFilterClauseNode extends SyntaxTreeNode<"CatchFilterClause"> {
    whenKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    filterExpression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface CheckedExpressionNode extends SyntaxTreeNode<"CheckedExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface CheckedStatementNode extends SyntaxTreeNode<"CheckedStatement"> {
    attributeLists: AttributeListNode[];
    keyword?: SyntaxToken;
    block?: BlockNode;
}

interface ClassDeclarationNode extends SyntaxTreeNode<"ClassDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    baseList?: BaseListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    members: SyntaxTreeNode[];
    arity?: number;
}

interface ClassOrStructConstraintNode extends SyntaxTreeNode<"ClassOrStructConstraint"> {
    classOrStructKeyword?: SyntaxToken;
    questionToken?: SyntaxToken;
}

interface CompilationUnitNode extends SyntaxTreeNode<"CompilationUnit"> {
    externs: ExternAliasDirectiveNode[];
    usings: UsingDirectiveNode[];
    attributeLists: AttributeListNode[];
    members: SyntaxTreeNode[];
}

interface ConditionalAccessExpressionNode extends SyntaxTreeNode<"ConditionalAccessExpression"> {
    expression?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    whenNotNull?: SyntaxTreeNode;
}

interface ConditionalExpressionNode extends SyntaxTreeNode<"ConditionalExpression"> {
    condition?: SyntaxTreeNode;
    questionToken?: SyntaxToken;
    whenTrue?: SyntaxTreeNode;
    colonToken?: SyntaxToken;
    whenFalse?: SyntaxTreeNode;
}

interface ConstantPatternNode extends SyntaxTreeNode<"ConstantPattern"> {
    expression?: SyntaxTreeNode;
}

interface ConstructorConstraintNode extends SyntaxTreeNode<"ConstructorConstraint"> {
    newKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    closeParenToken?: SyntaxToken;
}

interface ConstructorDeclarationNode extends SyntaxTreeNode<"ConstructorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    identifier?: SyntaxToken;
    parameterList?: ParameterListNode;
    initializer?: ConstructorInitializerNode;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
}

interface ConstructorInitializerNode extends SyntaxTreeNode<"ConstructorInitializer"> {
    colonToken?: SyntaxToken;
    thisOrBaseKeyword?: SyntaxToken;
    argumentList?: ArgumentListNode;
}

interface ContinueStatementNode extends SyntaxTreeNode<"ContinueStatement"> {
    attributeLists: AttributeListNode[];
    continueKeyword?: SyntaxToken;
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
}

interface ConversionOperatorMemberCrefNode extends SyntaxTreeNode<"ConversionOperatorMemberCref"> {
    implicitOrExplicitKeyword?: SyntaxToken;
    operatorKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    parameters?: CrefParameterListNode;
}

interface CrefBracketedParameterListNode extends SyntaxTreeNode<"CrefBracketedParameterList"> {
    openBracketToken?: SyntaxToken;
    parameters: CrefParameterNode[];
    closeBracketToken?: SyntaxToken;
}

interface CrefParameterListNode extends SyntaxTreeNode<"CrefParameterList"> {
    openParenToken?: SyntaxToken;
    parameters: CrefParameterNode[];
    closeParenToken?: SyntaxToken;
}

interface CrefParameterNode extends SyntaxTreeNode<"CrefParameter"> {
    refKindKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    refOrOutKeyword?: SyntaxToken;
}

interface DeclarationExpressionNode extends SyntaxTreeNode<"DeclarationExpression"> {
    type?: SyntaxTreeNode;
    designation?: SyntaxTreeNode;
}

interface DeclarationPatternNode extends SyntaxTreeNode<"DeclarationPattern"> {
    type?: SyntaxTreeNode;
    designation?: SyntaxTreeNode;
}

interface DefaultConstraintNode extends SyntaxTreeNode<"DefaultConstraint"> {
    defaultKeyword?: SyntaxToken;
}

interface DefaultExpressionNode extends SyntaxTreeNode<"DefaultExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface DefaultSwitchLabelNode extends SyntaxTreeNode<"DefaultSwitchLabel"> {
    keyword?: SyntaxToken;
    colonToken?: SyntaxToken;
}

interface DefineDirectiveTriviaNode extends SyntaxTreeNode<"DefineDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    defineKeyword?: SyntaxToken;
    name?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
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
    arity?: number;
}

interface DestructorDeclarationNode extends SyntaxTreeNode<"DestructorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    tildeToken?: SyntaxToken;
    identifier?: SyntaxToken;
    parameterList?: ParameterListNode;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
}

interface DiscardDesignationNode extends SyntaxTreeNode<"DiscardDesignation"> {
    underscoreToken?: SyntaxToken;
}

interface DiscardPatternNode extends SyntaxTreeNode<"DiscardPattern"> {
    underscoreToken?: SyntaxToken;
}

interface DocumentationCommentTriviaNode extends SyntaxTreeNode<"DocumentationCommentTrivia"> {
    content: SyntaxTreeNode[];
    endOfComment?: SyntaxToken;
}

interface DoStatementNode extends SyntaxTreeNode<"DoStatement"> {
    attributeLists: AttributeListNode[];
    doKeyword?: SyntaxToken;
    statement?: SyntaxTreeNode;
    whileKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface ElementAccessExpressionNode extends SyntaxTreeNode<"ElementAccessExpression"> {
    expression?: SyntaxTreeNode;
    argumentList?: BracketedArgumentListNode;
}

interface ElementBindingExpressionNode extends SyntaxTreeNode<"ElementBindingExpression"> {
    argumentList?: BracketedArgumentListNode;
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
}

interface ElseClauseNode extends SyntaxTreeNode<"ElseClause"> {
    elseKeyword?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

interface ElseDirectiveTriviaNode extends SyntaxTreeNode<"ElseDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    elseKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    branchTaken?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface EmptyStatementNode extends SyntaxTreeNode<"EmptyStatement"> {
    attributeLists: AttributeListNode[];
}

interface EndIfDirectiveTriviaNode extends SyntaxTreeNode<"EndIfDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    endIfKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface EndRegionDirectiveTriviaNode extends SyntaxTreeNode<"EndRegionDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    endRegionKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface EnumDeclarationNode extends SyntaxTreeNode<"EnumDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    enumKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    baseList?: BaseListNode;
    members: EnumMemberDeclarationNode[];
}

interface EnumMemberDeclarationNode extends SyntaxTreeNode<"EnumMemberDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    identifier?: SyntaxToken;
    equalsValue?: EqualsValueClauseNode;
}

interface EqualsValueClauseNode extends SyntaxTreeNode<"EqualsValueClause"> {
    equalsToken?: SyntaxToken;
    value?: SyntaxTreeNode;
}

interface ErrorDirectiveTriviaNode extends SyntaxTreeNode<"ErrorDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    errorKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface EventDeclarationNode extends SyntaxTreeNode<"EventDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    eventKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: ExplicitInterfaceSpecifierNode;
    identifier?: SyntaxToken;
    accessorList?: AccessorListNode;
}

interface EventFieldDeclarationNode extends SyntaxTreeNode<"EventFieldDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    eventKeyword?: SyntaxToken;
    declaration?: VariableDeclarationNode;
}

interface ExplicitInterfaceSpecifierNode extends SyntaxTreeNode<"ExplicitInterfaceSpecifier"> {
    name?: SyntaxTreeNode;
    dotToken?: SyntaxToken;
}

interface ExpressionStatementNode extends SyntaxTreeNode<"ExpressionStatement"> {
    attributeLists: AttributeListNode[];
    expression?: SyntaxTreeNode;
    allowsAnyExpression?: boolean;
}

interface ExternAliasDirectiveNode extends SyntaxTreeNode<"ExternAliasDirective"> {
    externKeyword?: SyntaxToken;
    aliasKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
}

interface FieldDeclarationNode extends SyntaxTreeNode<"FieldDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    declaration?: VariableDeclarationNode;
}

interface FinallyClauseNode extends SyntaxTreeNode<"FinallyClause"> {
    finallyKeyword?: SyntaxToken;
    block?: BlockNode;
}

interface FixedStatementNode extends SyntaxTreeNode<"FixedStatement"> {
    attributeLists: AttributeListNode[];
    fixedKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    declaration?: VariableDeclarationNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
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
}

interface FromClauseNode extends SyntaxTreeNode<"FromClause"> {
    fromKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    inKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface FunctionPointerCallingConventionNode extends SyntaxTreeNode<"FunctionPointerCallingConvention"> {
    managedOrUnmanagedKeyword?: SyntaxToken;
    unmanagedCallingConventionList?: FunctionPointerUnmanagedCallingConventionListNode;
}

interface FunctionPointerParameterListNode extends SyntaxTreeNode<"FunctionPointerParameterList"> {
    lessThanToken?: SyntaxToken;
    parameters: FunctionPointerParameterNode[];
    greaterThanToken?: SyntaxToken;
}

interface FunctionPointerParameterNode extends SyntaxTreeNode<"FunctionPointerParameter"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
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
}

interface FunctionPointerUnmanagedCallingConventionListNode extends SyntaxTreeNode<"FunctionPointerUnmanagedCallingConventionList"> {
    openBracketToken?: SyntaxToken;
    callingConventions: FunctionPointerUnmanagedCallingConventionNode[];
    closeBracketToken?: SyntaxToken;
}

interface FunctionPointerUnmanagedCallingConventionNode extends SyntaxTreeNode<"FunctionPointerUnmanagedCallingConvention"> {
    name?: SyntaxToken;
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
}

interface GlobalStatementNode extends SyntaxTreeNode<"GlobalStatement"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    statement?: SyntaxTreeNode;
}

interface GotoStatementNode extends SyntaxTreeNode<"GotoStatement"> {
    attributeLists: AttributeListNode[];
    gotoKeyword?: SyntaxToken;
    caseOrDefaultKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface GroupClauseNode extends SyntaxTreeNode<"GroupClause"> {
    groupKeyword?: SyntaxToken;
    groupExpression?: SyntaxTreeNode;
    byKeyword?: SyntaxToken;
    byExpression?: SyntaxTreeNode;
}

interface IdentifierNameNode extends SyntaxTreeNode<"IdentifierName"> {
    identifier?: SyntaxToken;
    arity?: number;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
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
}

interface IfStatementNode extends SyntaxTreeNode<"IfStatement"> {
    attributeLists: AttributeListNode[];
    ifKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    else?: ElseClauseNode;
}

interface ImplicitArrayCreationExpressionNode extends SyntaxTreeNode<"ImplicitArrayCreationExpression"> {
    newKeyword?: SyntaxToken;
    openBracketToken?: SyntaxToken;
    commas: SyntaxToken[];
    closeBracketToken?: SyntaxToken;
    initializer?: InitializerExpressionNode;
}

interface ImplicitElementAccessNode extends SyntaxTreeNode<"ImplicitElementAccess"> {
    argumentList?: BracketedArgumentListNode;
}

interface ImplicitObjectCreationExpressionNode extends SyntaxTreeNode<"ImplicitObjectCreationExpression"> {
    newKeyword?: SyntaxToken;
    argumentList?: ArgumentListNode;
    initializer?: InitializerExpressionNode;
}

interface ImplicitStackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"ImplicitStackAllocArrayCreationExpression"> {
    stackAllocKeyword?: SyntaxToken;
    openBracketToken?: SyntaxToken;
    closeBracketToken?: SyntaxToken;
    initializer?: InitializerExpressionNode;
}

interface IncompleteMemberNode extends SyntaxTreeNode<"IncompleteMember"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
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
}

interface IndexerMemberCrefNode extends SyntaxTreeNode<"IndexerMemberCref"> {
    thisKeyword?: SyntaxToken;
    parameters?: CrefBracketedParameterListNode;
}

interface InitializerExpressionNode extends SyntaxTreeNode<"InitializerExpression"> {
    expressions: SyntaxTreeNode[];
}

interface InterfaceDeclarationNode extends SyntaxTreeNode<"InterfaceDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    baseList?: BaseListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    members: SyntaxTreeNode[];
    arity?: number;
}

interface InterpolatedStringExpressionNode extends SyntaxTreeNode<"InterpolatedStringExpression"> {
    stringStartToken?: SyntaxToken;
    contents: SyntaxTreeNode[];
    stringEndToken?: SyntaxToken;
}

interface InterpolatedStringTextNode extends SyntaxTreeNode<"InterpolatedStringText"> {
    textToken?: SyntaxToken;
}

interface InterpolationAlignmentClauseNode extends SyntaxTreeNode<"InterpolationAlignmentClause"> {
    commaToken?: SyntaxToken;
    value?: SyntaxTreeNode;
}

interface InterpolationFormatClauseNode extends SyntaxTreeNode<"InterpolationFormatClause"> {
    colonToken?: SyntaxToken;
    formatStringToken?: SyntaxToken;
}

interface InterpolationNode extends SyntaxTreeNode<"Interpolation"> {
    expression?: SyntaxTreeNode;
    alignmentClause?: InterpolationAlignmentClauseNode;
    formatClause?: InterpolationFormatClauseNode;
}

interface InvocationExpressionNode extends SyntaxTreeNode<"InvocationExpression"> {
    expression?: SyntaxTreeNode;
    argumentList?: ArgumentListNode;
}

interface IsPatternExpressionNode extends SyntaxTreeNode<"IsPatternExpression"> {
    expression?: SyntaxTreeNode;
    isKeyword?: SyntaxToken;
    pattern?: SyntaxTreeNode;
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
}

interface JoinIntoClauseNode extends SyntaxTreeNode<"JoinIntoClause"> {
    intoKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
}

interface LabeledStatementNode extends SyntaxTreeNode<"LabeledStatement"> {
    attributeLists: AttributeListNode[];
    identifier?: SyntaxToken;
    colonToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

interface LetClauseNode extends SyntaxTreeNode<"LetClause"> {
    letKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    equalsToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface LineDirectiveTriviaNode extends SyntaxTreeNode<"LineDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    lineKeyword?: SyntaxToken;
    line?: SyntaxToken;
    file?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface LiteralExpressionNode extends SyntaxTreeNode<"LiteralExpression"> {
    token?: SyntaxToken;
}

interface LoadDirectiveTriviaNode extends SyntaxTreeNode<"LoadDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    loadKeyword?: SyntaxToken;
    file?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface LocalDeclarationStatementNode extends SyntaxTreeNode<"LocalDeclarationStatement"> {
    attributeLists: AttributeListNode[];
    awaitKeyword?: SyntaxToken;
    usingKeyword?: SyntaxToken;
    modifiers: SyntaxToken[];
    declaration?: VariableDeclarationNode;
    isConst?: boolean;
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
}

interface LockStatementNode extends SyntaxTreeNode<"LockStatement"> {
    attributeLists: AttributeListNode[];
    lockKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

interface MakeRefExpressionNode extends SyntaxTreeNode<"MakeRefExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface MemberAccessExpressionNode extends SyntaxTreeNode<"MemberAccessExpression"> {
    expression?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    name?: SyntaxTreeNode;
}

interface MemberBindingExpressionNode extends SyntaxTreeNode<"MemberBindingExpression"> {
    operatorToken?: SyntaxToken;
    name?: SyntaxTreeNode;
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
    arity?: number;
}

interface NameColonNode extends SyntaxTreeNode<"NameColon"> {
    name?: IdentifierNameNode;
    colonToken?: SyntaxToken;
}

interface NameEqualsNode extends SyntaxTreeNode<"NameEquals"> {
    name?: IdentifierNameNode;
    equalsToken?: SyntaxToken;
}

interface NameMemberCrefNode extends SyntaxTreeNode<"NameMemberCref"> {
    name?: SyntaxTreeNode;
    parameters?: CrefParameterListNode;
}

interface NamespaceDeclarationNode extends SyntaxTreeNode<"NamespaceDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    namespaceKeyword?: SyntaxToken;
    name?: SyntaxTreeNode;
    externs: ExternAliasDirectiveNode[];
    usings: UsingDirectiveNode[];
    members: SyntaxTreeNode[];
}

interface NullableDirectiveTriviaNode extends SyntaxTreeNode<"NullableDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    nullableKeyword?: SyntaxToken;
    settingToken?: SyntaxToken;
    targetToken?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface NullableTypeNode extends SyntaxTreeNode<"NullableType"> {
    elementType?: SyntaxTreeNode;
    questionToken?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
}

interface ObjectCreationExpressionNode extends SyntaxTreeNode<"ObjectCreationExpression"> {
    newKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    argumentList?: ArgumentListNode;
    initializer?: InitializerExpressionNode;
}

interface OmittedArraySizeExpressionNode extends SyntaxTreeNode<"OmittedArraySizeExpression"> {
    omittedArraySizeExpressionToken?: SyntaxToken;
}

interface OmittedTypeArgumentNode extends SyntaxTreeNode<"OmittedTypeArgument"> {
    omittedTypeArgumentToken?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
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
}

interface OperatorMemberCrefNode extends SyntaxTreeNode<"OperatorMemberCref"> {
    operatorKeyword?: SyntaxToken;
    operatorToken?: SyntaxToken;
    parameters?: CrefParameterListNode;
}

interface OrderByClauseNode extends SyntaxTreeNode<"OrderByClause"> {
    orderByKeyword?: SyntaxToken;
    orderings: OrderingNode[];
}

interface OrderingNode extends SyntaxTreeNode<"Ordering"> {
    expression?: SyntaxTreeNode;
    ascendingOrDescendingKeyword?: SyntaxToken;
}

interface ParameterListNode extends SyntaxTreeNode<"ParameterList"> {
    openParenToken?: SyntaxToken;
    parameters: ParameterNode[];
    closeParenToken?: SyntaxToken;
}

interface ParameterNode extends SyntaxTreeNode<"Parameter"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
    default?: EqualsValueClauseNode;
}

interface ParenthesizedExpressionNode extends SyntaxTreeNode<"ParenthesizedExpression"> {
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface ParenthesizedLambdaExpressionNode extends SyntaxTreeNode<"ParenthesizedLambdaExpression"> {
    modifiers: SyntaxToken[];
    parameterList?: ParameterListNode;
    arrowToken?: SyntaxToken;
    block?: BlockNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword?: SyntaxToken;
    body?: SyntaxTreeNode;
}

interface ParenthesizedPatternNode extends SyntaxTreeNode<"ParenthesizedPattern"> {
    openParenToken?: SyntaxToken;
    pattern?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface ParenthesizedVariableDesignationNode extends SyntaxTreeNode<"ParenthesizedVariableDesignation"> {
    openParenToken?: SyntaxToken;
    variables: SyntaxTreeNode[];
    closeParenToken?: SyntaxToken;
}

interface PointerTypeNode extends SyntaxTreeNode<"PointerType"> {
    elementType?: SyntaxTreeNode;
    asteriskToken?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
}

interface PositionalPatternClauseNode extends SyntaxTreeNode<"PositionalPatternClause"> {
    openParenToken?: SyntaxToken;
    subpatterns: SubpatternNode[];
    closeParenToken?: SyntaxToken;
}

interface PostfixUnaryExpressionNode extends SyntaxTreeNode<"PostfixUnaryExpression"> {
    operand?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
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
}

interface PredefinedTypeNode extends SyntaxTreeNode<"PredefinedType"> {
    keyword?: SyntaxToken;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
}

interface PrefixUnaryExpressionNode extends SyntaxTreeNode<"PrefixUnaryExpression"> {
    operatorToken?: SyntaxToken;
    operand?: SyntaxTreeNode;
}

interface PrimaryConstructorBaseTypeNode extends SyntaxTreeNode<"PrimaryConstructorBaseType"> {
    type?: SyntaxTreeNode;
    argumentList?: ArgumentListNode;
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
}

interface PropertyPatternClauseNode extends SyntaxTreeNode<"PropertyPatternClause"> {
    subpatterns: SubpatternNode[];
}

interface QualifiedCrefNode extends SyntaxTreeNode<"QualifiedCref"> {
    container?: SyntaxTreeNode;
    dotToken?: SyntaxToken;
    member?: SyntaxTreeNode;
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
}

interface QueryBodyNode extends SyntaxTreeNode<"QueryBody"> {
    clauses: SyntaxTreeNode[];
    selectOrGroup?: SyntaxTreeNode;
    continuation?: QueryContinuationNode;
}

interface QueryContinuationNode extends SyntaxTreeNode<"QueryContinuation"> {
    intoKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
    body?: QueryBodyNode;
}

interface QueryExpressionNode extends SyntaxTreeNode<"QueryExpression"> {
    fromClause?: FromClauseNode;
    body?: QueryBodyNode;
}

interface RangeExpressionNode extends SyntaxTreeNode<"RangeExpression"> {
    leftOperand?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    rightOperand?: SyntaxTreeNode;
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
    members: SyntaxTreeNode[];
    arity?: number;
}

interface RecursivePatternNode extends SyntaxTreeNode<"RecursivePattern"> {
    type?: SyntaxTreeNode;
    positionalPatternClause?: PositionalPatternClauseNode;
    propertyPatternClause?: PropertyPatternClauseNode;
    designation?: SyntaxTreeNode;
}

interface ReferenceDirectiveTriviaNode extends SyntaxTreeNode<"ReferenceDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    referenceKeyword?: SyntaxToken;
    file?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface RefExpressionNode extends SyntaxTreeNode<"RefExpression"> {
    refKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface RefTypeExpressionNode extends SyntaxTreeNode<"RefTypeExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
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
}

interface RefValueExpressionNode extends SyntaxTreeNode<"RefValueExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    comma?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface RegionDirectiveTriviaNode extends SyntaxTreeNode<"RegionDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    regionKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface RelationalPatternNode extends SyntaxTreeNode<"RelationalPattern"> {
    operatorToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface ReturnStatementNode extends SyntaxTreeNode<"ReturnStatement"> {
    attributeLists: AttributeListNode[];
    returnKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface SelectClauseNode extends SyntaxTreeNode<"SelectClause"> {
    selectKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface ShebangDirectiveTriviaNode extends SyntaxTreeNode<"ShebangDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    exclamationToken?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface SimpleBaseTypeNode extends SyntaxTreeNode<"SimpleBaseType"> {
    type?: SyntaxTreeNode;
}

interface SimpleLambdaExpressionNode extends SyntaxTreeNode<"SimpleLambdaExpression"> {
    modifiers: SyntaxToken[];
    parameter?: ParameterNode;
    arrowToken?: SyntaxToken;
    block?: BlockNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword?: SyntaxToken;
    body?: SyntaxTreeNode;
}

interface SingleVariableDesignationNode extends SyntaxTreeNode<"SingleVariableDesignation"> {
    identifier?: SyntaxToken;
}

interface SizeOfExpressionNode extends SyntaxTreeNode<"SizeOfExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface SkippedTokensTriviaNode extends SyntaxTreeNode<"SkippedTokensTrivia"> {
    tokens: SyntaxToken[];
}

interface StackAllocArrayCreationExpressionNode extends SyntaxTreeNode<"StackAllocArrayCreationExpression"> {
    stackAllocKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    initializer?: InitializerExpressionNode;
}

interface StructDeclarationNode extends SyntaxTreeNode<"StructDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    identifier?: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    baseList?: BaseListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    members: SyntaxTreeNode[];
    arity?: number;
}

interface SubpatternNode extends SyntaxTreeNode<"Subpattern"> {
    nameColon?: NameColonNode;
    pattern?: SyntaxTreeNode;
}

interface SwitchExpressionArmNode extends SyntaxTreeNode<"SwitchExpressionArm"> {
    pattern?: SyntaxTreeNode;
    whenClause?: WhenClauseNode;
    equalsGreaterThanToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface SwitchExpressionNode extends SyntaxTreeNode<"SwitchExpression"> {
    governingExpression?: SyntaxTreeNode;
    switchKeyword?: SyntaxToken;
    arms: SwitchExpressionArmNode[];
}

interface SwitchSectionNode extends SyntaxTreeNode<"SwitchSection"> {
    labels: SyntaxTreeNode[];
    statements: SyntaxTreeNode[];
}

interface SwitchStatementNode extends SyntaxTreeNode<"SwitchStatement"> {
    attributeLists: AttributeListNode[];
    switchKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    sections: SwitchSectionNode[];
}

interface ThisExpressionNode extends SyntaxTreeNode<"ThisExpression"> {
    token?: SyntaxToken;
}

interface ThrowExpressionNode extends SyntaxTreeNode<"ThrowExpression"> {
    throwKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface ThrowStatementNode extends SyntaxTreeNode<"ThrowStatement"> {
    attributeLists: AttributeListNode[];
    throwKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

interface TryStatementNode extends SyntaxTreeNode<"TryStatement"> {
    attributeLists: AttributeListNode[];
    tryKeyword?: SyntaxToken;
    block?: BlockNode;
    catches: CatchClauseNode[];
    finally?: FinallyClauseNode;
}

interface TupleElementNode extends SyntaxTreeNode<"TupleElement"> {
    type?: SyntaxTreeNode;
    identifier?: SyntaxToken;
}

interface TupleExpressionNode extends SyntaxTreeNode<"TupleExpression"> {
    openParenToken?: SyntaxToken;
    arguments: ArgumentNode[];
    closeParenToken?: SyntaxToken;
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
}

interface TypeArgumentListNode extends SyntaxTreeNode<"TypeArgumentList"> {
    lessThanToken?: SyntaxToken;
    arguments: SyntaxTreeNode[];
    greaterThanToken?: SyntaxToken;
}

interface TypeConstraintNode extends SyntaxTreeNode<"TypeConstraint"> {
    type?: SyntaxTreeNode;
}

interface TypeCrefNode extends SyntaxTreeNode<"TypeCref"> {
    type?: SyntaxTreeNode;
}

interface TypeOfExpressionNode extends SyntaxTreeNode<"TypeOfExpression"> {
    keyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

interface TypeParameterConstraintClauseNode extends SyntaxTreeNode<"TypeParameterConstraintClause"> {
    whereKeyword?: SyntaxToken;
    name?: IdentifierNameNode;
    colonToken?: SyntaxToken;
    constraints: SyntaxTreeNode[];
}

interface TypeParameterListNode extends SyntaxTreeNode<"TypeParameterList"> {
    lessThanToken?: SyntaxToken;
    parameters: TypeParameterNode[];
    greaterThanToken?: SyntaxToken;
}

interface TypeParameterNode extends SyntaxTreeNode<"TypeParameter"> {
    attributeLists: AttributeListNode[];
    varianceKeyword?: SyntaxToken;
    identifier?: SyntaxToken;
}

interface TypePatternNode extends SyntaxTreeNode<"TypePattern"> {
    type?: SyntaxTreeNode;
}

interface UnaryPatternNode extends SyntaxTreeNode<"UnaryPattern"> {
    operatorToken?: SyntaxToken;
    pattern?: SyntaxTreeNode;
}

interface UndefDirectiveTriviaNode extends SyntaxTreeNode<"UndefDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    undefKeyword?: SyntaxToken;
    name?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface UnsafeStatementNode extends SyntaxTreeNode<"UnsafeStatement"> {
    attributeLists: AttributeListNode[];
    unsafeKeyword?: SyntaxToken;
    block?: BlockNode;
}

interface UsingDirectiveNode extends SyntaxTreeNode<"UsingDirective"> {
    usingKeyword?: SyntaxToken;
    staticKeyword?: SyntaxToken;
    alias?: NameEqualsNode;
    name?: SyntaxTreeNode;
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
}

interface VariableDeclarationNode extends SyntaxTreeNode<"VariableDeclaration"> {
    type?: SyntaxTreeNode;
    variables: VariableDeclaratorNode[];
}

interface VariableDeclaratorNode extends SyntaxTreeNode<"VariableDeclarator"> {
    identifier?: SyntaxToken;
    argumentList?: BracketedArgumentListNode;
    initializer?: EqualsValueClauseNode;
}

interface VarPatternNode extends SyntaxTreeNode<"VarPattern"> {
    varKeyword?: SyntaxToken;
    designation?: SyntaxTreeNode;
}

interface WarningDirectiveTriviaNode extends SyntaxTreeNode<"WarningDirectiveTrivia"> {
    hashToken?: SyntaxToken;
    warningKeyword?: SyntaxToken;
    endOfDirectiveToken?: SyntaxToken;
    isActive?: boolean;
    directiveNameToken?: SyntaxToken;
}

interface WhenClauseNode extends SyntaxTreeNode<"WhenClause"> {
    whenKeyword?: SyntaxToken;
    condition?: SyntaxTreeNode;
}

interface WhereClauseNode extends SyntaxTreeNode<"WhereClause"> {
    whereKeyword?: SyntaxToken;
    condition?: SyntaxTreeNode;
}

interface WhileStatementNode extends SyntaxTreeNode<"WhileStatement"> {
    attributeLists: AttributeListNode[];
    whileKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

interface WithExpressionNode extends SyntaxTreeNode<"WithExpression"> {
    expression?: SyntaxTreeNode;
    withKeyword?: SyntaxToken;
    initializer?: InitializerExpressionNode;
}

interface XmlCDataSectionNode extends SyntaxTreeNode<"XmlCDataSection"> {
    startCDataToken?: SyntaxToken;
    textTokens: SyntaxToken[];
    endCDataToken?: SyntaxToken;
}

interface XmlCommentNode extends SyntaxTreeNode<"XmlComment"> {
    lessThanExclamationMinusMinusToken?: SyntaxToken;
    textTokens: SyntaxToken[];
    minusMinusGreaterThanToken?: SyntaxToken;
}

interface XmlCrefAttributeNode extends SyntaxTreeNode<"XmlCrefAttribute"> {
    name?: XmlNameNode;
    equalsToken?: SyntaxToken;
    startQuoteToken?: SyntaxToken;
    cref?: SyntaxTreeNode;
    endQuoteToken?: SyntaxToken;
}

interface XmlElementEndTagNode extends SyntaxTreeNode<"XmlElementEndTag"> {
    lessThanSlashToken?: SyntaxToken;
    name?: XmlNameNode;
    greaterThanToken?: SyntaxToken;
}

interface XmlElementStartTagNode extends SyntaxTreeNode<"XmlElementStartTag"> {
    lessThanToken?: SyntaxToken;
    name?: XmlNameNode;
    attributes: SyntaxTreeNode[];
    greaterThanToken?: SyntaxToken;
}

interface XmlElementNode extends SyntaxTreeNode<"XmlElement"> {
    startTag?: XmlElementStartTagNode;
    content: SyntaxTreeNode[];
    endTag?: XmlElementEndTagNode;
}

interface XmlEmptyElementNode extends SyntaxTreeNode<"XmlEmptyElement"> {
    lessThanToken?: SyntaxToken;
    name?: XmlNameNode;
    attributes: SyntaxTreeNode[];
    slashGreaterThanToken?: SyntaxToken;
}

interface XmlNameAttributeNode extends SyntaxTreeNode<"XmlNameAttribute"> {
    name?: XmlNameNode;
    equalsToken?: SyntaxToken;
    startQuoteToken?: SyntaxToken;
    identifier?: IdentifierNameNode;
    endQuoteToken?: SyntaxToken;
}

interface XmlNameNode extends SyntaxTreeNode<"XmlName"> {
    prefix?: XmlPrefixNode;
    localName?: SyntaxToken;
}

interface XmlPrefixNode extends SyntaxTreeNode<"XmlPrefix"> {
    prefix?: SyntaxToken;
    colonToken?: SyntaxToken;
}

interface XmlProcessingInstructionNode extends SyntaxTreeNode<"XmlProcessingInstruction"> {
    startProcessingInstructionToken?: SyntaxToken;
    name?: XmlNameNode;
    textTokens: SyntaxToken[];
    endProcessingInstructionToken?: SyntaxToken;
}

interface XmlTextAttributeNode extends SyntaxTreeNode<"XmlTextAttribute"> {
    name?: XmlNameNode;
    equalsToken?: SyntaxToken;
    startQuoteToken?: SyntaxToken;
    textTokens: SyntaxToken[];
    endQuoteToken?: SyntaxToken;
}

interface XmlTextNode extends SyntaxTreeNode<"XmlText"> {
    textTokens: SyntaxToken[];
}

interface YieldStatementNode extends SyntaxTreeNode<"YieldStatement"> {
    attributeLists: AttributeListNode[];
    yieldKeyword?: SyntaxToken;
    returnOrBreakKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

