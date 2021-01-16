import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ImplicitElementAccessNode extends SyntaxTreeNode<"ImplicitElementAccess"> {}

export const print: PrintMethod<ImplicitElementAccessNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ImplicitElementAccess" : "";
};
