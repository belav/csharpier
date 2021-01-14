import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EventFieldDeclarationNode extends SyntaxTreeNode<"EventFieldDeclaration"> {}

export const print: PrintMethod<EventFieldDeclarationNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node EventFieldDeclaration" : "";
};
