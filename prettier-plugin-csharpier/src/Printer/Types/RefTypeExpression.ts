import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RefTypeExpressionNode extends SyntaxTreeNode<"RefTypeExpression"> {}

export const print: PrintMethod<RefTypeExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node RefTypeExpression" : "";
};
