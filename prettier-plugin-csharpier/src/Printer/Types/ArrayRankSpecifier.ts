import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrayRankSpecifierNode extends SyntaxTreeNode<"ArrayRankSpecifier"> {}

export const print: PrintMethod<ArrayRankSpecifierNode> = (path, options, print) => {
    return concat(["[", join(", ", path.map(print, "sizes")), "]"]);
};
