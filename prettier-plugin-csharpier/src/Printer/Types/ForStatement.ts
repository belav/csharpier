import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ForStatementNode extends SyntaxTreeNode<"ForStatement"> {}

export const print: PrintMethod<ForStatementNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ForStatement" : "";
};
