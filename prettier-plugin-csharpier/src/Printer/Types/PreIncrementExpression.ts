import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PreIncrementExpressionNode extends SyntaxTreeNode<"PreIncrementExpression"> {
    operatorToken: HasValue;
    operand: SyntaxTreeNode;
}

export const print: PrintMethod<PreIncrementExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "operatorToken"), path.call(print, "operand")]);
};
