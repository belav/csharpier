import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RangeExpressionNode extends SyntaxTreeNode<"RangeExpression"> {}

export const print: PrintMethod<RangeExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node RangeExpression" : "";
};
