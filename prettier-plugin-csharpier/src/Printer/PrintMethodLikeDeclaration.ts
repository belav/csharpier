import { Doc } from "prettier";
import { printAttributeLists } from "./PrintAttributeLists";
import { printComments } from "./PrintComments";
import { printConstraintClauses } from "./PrintConstraintClauses";
import { printExtraNewLines } from "./PrintExtraNewLines";
import { PrintMethod } from "./PrintMethod";
import {
    printSyntaxToken,
    HasModifiers,
    SyntaxToken,
    SyntaxTreeNode,
    HasIdentifier,
    printIdentifier,
    printModifiers,
} from "./SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "./Builders";
import { ArrowExpressionClauseNode, printArrowExpressionClause } from "./Types/ArrowExpressionClause";
import { AttributeListNode } from "./Types/AttributeList";
import { BlockNode, printBlock } from "./Types/Block";
import { ParameterListNode } from "./Types/ParameterList";
import { TypeParameterConstraintClauseNode } from "./Types/TypeParameterConstraintClause";
import { printTypeParameterList, TypeParameterListNode } from "./Types/TypeParameterList";

interface MethodLikeDeclarationNode
    extends SyntaxTreeNode<
        "ConversionOperatorDeclaration" | "LocalFunctionStatement" | "MethodDeclaration" | "OperatorDeclaration"
    > {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    returnType?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: { name: HasIdentifier };
    identifier: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    implicitOrExplicitKeyword?: SyntaxToken;
    operatorKeyword?: SyntaxToken;
    operatorToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    parameterList?: ParameterListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    arity?: number;
}

export const printMethodLikeDeclaration: PrintMethod<MethodLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printExtraNewLines(node, parts, "attributeLists", "modifiers")
    printAttributeLists(node, parts, path, options, print);
    printComments(node, parts, "modifiers", "returnType", "identifier")
    parts.push(printModifiers(node));
    if (node.returnType) {
        parts.push(path.call(print, "returnType"), " ");
    }
    if (node.explicitInterfaceSpecifier) {
        parts.push(printIdentifier(node.explicitInterfaceSpecifier.name), ".");
    }
    if (node.identifier) {
        parts.push(printIdentifier(node));
    }
    if (node.implicitOrExplicitKeyword) {
        parts.push(printSyntaxToken(node.implicitOrExplicitKeyword), " ");
    }
    if (node.operatorKeyword) {
        parts.push("operator ");
    }
    if (node.operatorToken) {
        parts.push(printSyntaxToken(node.operatorToken));
    }
    if (node.type) {
        parts.push(path.call(print, "type"));
    }
    if (node.typeParameterList) {
        parts.push(path.call(o => printTypeParameterList(o, options, print), "typeParameterList"));
    }
    parts.push(path.call(print, "parameterList"));
    printConstraintClauses(node, parts, path, options, print);

    if (node.body) {
        parts.push(path.call(o => printBlock(o, options, print), "body"));
    } else {
        if (node.expressionBody) {
            parts.push(path.call(o => printArrowExpressionClause(o, options, print), "expressionBody"));
        }
        parts.push(";");
    }

    return concat(parts);
};
