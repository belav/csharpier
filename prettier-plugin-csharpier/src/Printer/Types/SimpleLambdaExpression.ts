import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SimpleLambdaExpressionNode extends SyntaxTreeNode<"SimpleLambdaExpression"> {

}

export const print: PrintMethod<SimpleLambdaExpressionNode> = (path, options, print) => {
    return "TODO SimpleLambdaExpression";
};
