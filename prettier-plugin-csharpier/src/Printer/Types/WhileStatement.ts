import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface WhileStatementNode extends SyntaxTreeNode<"WhileStatement"> {}

export const print: PrintMethod<WhileStatementNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node WhileStatement" : "";
};
