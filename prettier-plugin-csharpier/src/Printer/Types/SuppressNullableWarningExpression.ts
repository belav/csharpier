import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SuppressNullableWarningExpressionNode extends SyntaxTreeNode<"SuppressNullableWarningExpression"> {}

export const print: PrintMethod<SuppressNullableWarningExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node SuppressNullableWarningExpression" : "";
};
