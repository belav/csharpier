import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AwaitExpressionNode extends SyntaxTreeNode<"AwaitExpression"> {

}

export const print: PrintMethod<AwaitExpressionNode> = (path, options, print) => {
    return "TODO AwaitExpression";
};
