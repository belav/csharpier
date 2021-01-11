import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ParenthesizedLambdaExpressionNode extends SyntaxTreeNode<"ParenthesizedLambdaExpression"> {}

export const print: PrintMethod<ParenthesizedLambdaExpressionNode> = (path, options, print) => {
    // TODO we don't always have an expressionBody, look into AllInOne
    return concat(["()", " => ", path.call(print, "expressionBody")]);
};
