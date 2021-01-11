import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ReturnStatementNode extends SyntaxTreeNode<"ReturnStatement"> {
    returnKeyword: HasValue;
    expression?: SyntaxTreeNode;
}

export const print: PrintMethod<ReturnStatementNode> = (path, options, print) => {
    const node = path.getValue();
    if (!node.expression) {
        return printValue(node.returnKeyword) + ";";
    }

    return concat([printValue(node.returnKeyword), " ", path.call(print, "expression"), ";"]);
};
