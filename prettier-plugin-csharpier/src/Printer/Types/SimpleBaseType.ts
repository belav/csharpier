import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SimpleBaseTypeNode extends SyntaxTreeNode<"SimpleBaseType"> {

}

export const print: PrintMethod<SimpleBaseTypeNode> = (path, options, print) => {
    return path.call(print, "type");
};
