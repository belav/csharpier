import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ReturnStatementNode extends SyntaxTreeNode<"ReturnStatement"> {
    returnKeyword: SyntaxToken;
    expression?: SyntaxTreeNode;
}

export const printReturnStatement: PrintMethod<ReturnStatementNode> = (path, options, print) => {
    const node = path.getValue();
    if (!node.expression) {
        return printSyntaxToken(node.returnKeyword) + ";";
    }

    return concat([printSyntaxToken(node.returnKeyword), " ", path.call(print, "expression"), ";"]);
};
