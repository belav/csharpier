import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UnaryMinusExpressionNode extends SyntaxTreeNode<"UnaryMinusExpression"> {
    operatorToken: HasValue;
    operand: SyntaxTreeNode;
}

export const print: PrintMethod<UnaryMinusExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "operatorToken"), path.call(print, "operand")]);
};
