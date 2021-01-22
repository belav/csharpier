import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AliasQualifiedNameNode extends SyntaxTreeNode<"AliasQualifiedName"> {
    alias: SyntaxTreeNode;
    colonColonToken: SyntaxToken;
    name: SyntaxTreeNode;
}

export const printAliasQualifiedName: PrintMethod<AliasQualifiedNameNode> = (path, options, print) => {
    return concat([path.call(print,  "alias"), printPathSyntaxToken(path, "colonColonToken"), path.call(print, "name")]);
};
