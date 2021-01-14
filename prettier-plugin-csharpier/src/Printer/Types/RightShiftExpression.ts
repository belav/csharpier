import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RightShiftExpressionNode extends SyntaxTreeNode<"RightShiftExpression"> {}

export const print: PrintMethod<RightShiftExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node RightShiftExpression" : "";
};
