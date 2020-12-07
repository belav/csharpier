import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AliasQualifiedNameNode extends SyntaxTreeNode<"AliasQualifiedName"> {}

export const print: PrintMethod<AliasQualifiedNameNode> = (path, options, print) => {
    return "TODO AliasQualifiedName";
};
