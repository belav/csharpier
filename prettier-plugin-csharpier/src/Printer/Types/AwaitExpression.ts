import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AwaitExpressionNode extends SyntaxTreeNode<"AwaitExpression"> {
    awaitKeyword: HasValue;
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<AwaitExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "awaitKeyword"), " ", path.call(print, "expression")]);
};
