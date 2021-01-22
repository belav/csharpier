import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ImplicitElementAccessNode extends SyntaxTreeNode<"ImplicitElementAccess"> {
    argumentList: SyntaxTreeNode;
}

export const printImplicitElementAccess: PrintMethod<ImplicitElementAccessNode> = (path, options, print) => {
    return path.call(print, "argumentList");
};
