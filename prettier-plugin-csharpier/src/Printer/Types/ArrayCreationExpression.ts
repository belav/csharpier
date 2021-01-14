import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrayCreationExpressionNode extends SyntaxTreeNode<"ArrayCreationExpression"> {}

export const print: PrintMethod<ArrayCreationExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ArrayCreationExpression" : "";
};
