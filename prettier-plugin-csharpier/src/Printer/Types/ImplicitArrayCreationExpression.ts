import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ImplicitArrayCreationExpressionNode extends SyntaxTreeNode<"ImplicitArrayCreationExpression"> {}

export const print: PrintMethod<ImplicitArrayCreationExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ImplicitArrayCreationExpression" : "";
};
