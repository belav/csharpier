import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DoStatementNode extends SyntaxTreeNode<"DoStatement"> {}

export const print: PrintMethod<DoStatementNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node DoStatement" : "";
};
