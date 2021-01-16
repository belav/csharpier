import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AliasQualifiedNameNode extends SyntaxTreeNode<"AliasQualifiedName"> {
    alias: SyntaxTreeNode;
    colonColonToken: HasValue;
    name: SyntaxTreeNode;
}

export const print: PrintMethod<AliasQualifiedNameNode> = (path, options, print) => {
    return concat([path.call(print,  "alias"), printPathValue(path, "colonColonToken"), path.call(print, "name")]);
};
