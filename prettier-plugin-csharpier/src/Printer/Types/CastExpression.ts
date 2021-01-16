import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CastExpressionNode extends SyntaxTreeNode<"CastExpression"> {
    type: SyntaxTreeNode;
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<CastExpressionNode> = (path, options, print) => {
    return concat(["(", path.call(print, "type"), ")", path.call(print, "expression")]);
};
