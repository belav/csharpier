import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PostIncrementExpressionNode extends SyntaxTreeNode<"PostIncrementExpression"> {
    operatorToken: HasValue;
    operand: SyntaxTreeNode;
}

export const print: PrintMethod<PostIncrementExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "operand"), printPathValue(path, "operatorToken")]);
};
