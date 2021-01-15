import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrayTypeNode extends SyntaxTreeNode<"ArrayType"> {
    elementType: SyntaxTreeNode;
    rankSpecifiers: SyntaxTreeNode[];
}

export const print: PrintMethod<ArrayTypeNode> = (path, options, print) => {
    return concat([path.call(print, "elementType"), concat(path.map(print, "rankSpecifiers"))]);
};
