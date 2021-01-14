import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IndexerDeclarationNode extends SyntaxTreeNode<"IndexerDeclaration"> {}

export const print: PrintMethod<IndexerDeclarationNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node IndexerDeclaration" : "";
};
