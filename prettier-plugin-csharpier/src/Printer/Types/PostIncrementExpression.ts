import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PostIncrementExpressionNode extends SyntaxTreeNode<"PostIncrementExpression"> {}

export const print: PrintMethod<PostIncrementExpressionNode> = (path, options, print) => {
    return "TODO PostIncrementExpression";
};
