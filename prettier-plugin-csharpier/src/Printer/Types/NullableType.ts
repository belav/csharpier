import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NullableTypeNode extends SyntaxTreeNode<"NullableType"> {

}

export const print: PrintMethod<NullableTypeNode> = (path, options, print) => {
    return "TODO NullableType";
};
