import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NullableTypeNode extends SyntaxTreeNode<"NullableType"> {
    elementType: SyntaxTreeNode;
}

export const print: PrintMethod<NullableTypeNode> = (path, options, print) => {
    return concat([path.call(print, "elementType"), "?"]);
};
