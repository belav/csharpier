import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UncheckedStatementNode extends SyntaxTreeNode<"UncheckedStatement"> {}

export const print: PrintMethod<UncheckedStatementNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node UncheckedStatement" : "";
};
