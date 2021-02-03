import { Doc } from "prettier";
import { printLeadingComments } from "../PrintComments";
import { printExtraNewLines } from "../PrintExtraNewLines";
import { PrintMethod } from "../PrintMethod";
import { printIdentifier, printModifiers, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { ArrowExpressionClauseNode } from "./ArrowExpressionClause";
import { AttributeListNode } from "./AttributeList";
import { BlockNode, printBlock } from "./Block";
import { ParameterListNode, printParameterList } from "./ParameterList";

class ConstructorInitializerNode {}

export interface ConstructorDeclarationNode extends SyntaxTreeNode<"ConstructorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    identifier: SyntaxToken;
    parameterList?: ParameterListNode;
    initializer?: ConstructorInitializerNode;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
    semicolonToken?: SyntaxToken;
}

export const printConstructorDeclaration: PrintMethod<ConstructorDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printExtraNewLines(node, parts, "attributeLists", "modifiers", "identifier");
    printLeadingComments(node, parts, "attributeLists", "modifiers", "identifier");
    parts.push(printModifiers(node));
    parts.push(printIdentifier(node));
    parts.push(path.call(o => printParameterList(o, options, print), "parameterList"));
    parts.push(path.call(o => printBlock(o, options, print), "body"));

    return group(concat(parts));
};
