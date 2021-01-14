import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LabeledExpressionNode extends SyntaxTreeNode<"LabeledExpression"> {}

export const print: PrintMethod<LabeledExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node LabeledExpression" : "";
};
