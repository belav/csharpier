import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GenericNameNode extends SyntaxTreeNode<"GenericName"> {

}

export const print: PrintMethod<GenericNameNode> = (path, options, print) => {
    return "TODO GenericName";
};
