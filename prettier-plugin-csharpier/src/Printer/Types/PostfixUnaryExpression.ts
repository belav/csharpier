import { concat } from "../Builders";
import { PrintMethod } from "../PrintMethod";
import { Operator, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";

export interface PostfixUnaryExpressionNode extends SyntaxTreeNode<"PostfixUnaryExpression">, Operator {}

export const printPostfixUnaryExpression: PrintMethod<PostfixUnaryExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "operand"), printPathSyntaxToken(path, "operatorToken")]);
};
