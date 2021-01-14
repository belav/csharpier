import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ThrowStatementNode extends SyntaxTreeNode<"ThrowStatement"> {}

export const print: PrintMethod<ThrowStatementNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ThrowStatement" : "";
};
