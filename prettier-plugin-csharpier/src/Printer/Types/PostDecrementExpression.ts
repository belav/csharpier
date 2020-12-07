import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PostDecrementExpressionNode extends SyntaxTreeNode<"PostDecrementExpression"> {

}

export const print: PrintMethod<PostDecrementExpressionNode> = (path, options, print) => {
    return "TODO PostDecrementExpression";
};
