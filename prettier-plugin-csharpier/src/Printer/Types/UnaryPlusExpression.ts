import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UnaryPlusExpressionNode extends SyntaxTreeNode<"UnaryPlusExpression"> {}

export const print: PrintMethod<UnaryPlusExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node UnaryPlusExpression" : "";
};
