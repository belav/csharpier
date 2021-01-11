import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface MultiplyExpressionNode extends SyntaxTreeNode<"MultiplyExpression"> {

}

export const print: PrintMethod<MultiplyExpressionNode> = (path, options, print) => {
    return "TODO MultiplyExpression";
};
