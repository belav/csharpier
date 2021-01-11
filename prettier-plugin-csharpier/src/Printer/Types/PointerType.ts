import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PointerTypeNode extends SyntaxTreeNode<"PointerType"> {

}

export const print: PrintMethod<PointerTypeNode> = (path, options, print) => {
    return "TODO PointerType";
};
