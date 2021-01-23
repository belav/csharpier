import { concat } from "../Builders";
import { PrintMethod } from "../PrintMethod";
import { Operator, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";

export interface PrefixUnaryExpressionNode extends SyntaxTreeNode<"PrefixUnaryExpression">, Operator {
}

export const printPrefixUnaryExpression: PrintMethod<PrefixUnaryExpressionNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "operatorToken"), path.call(print, "operand")]);
}
