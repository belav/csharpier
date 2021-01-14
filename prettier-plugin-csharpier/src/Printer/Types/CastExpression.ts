import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CastExpressionNode extends SyntaxTreeNode<"CastExpression"> {}

export const print: PrintMethod<CastExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node CastExpression" : "";
};
