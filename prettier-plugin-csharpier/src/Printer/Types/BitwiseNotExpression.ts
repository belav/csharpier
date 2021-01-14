import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BitwiseNotExpressionNode extends SyntaxTreeNode<"BitwiseNotExpression"> {}

export const print: PrintMethod<BitwiseNotExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node BitwiseNotExpression" : "";
};
