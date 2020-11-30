import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IfStatementNode extends SyntaxTreeNode<"IfStatement"> {

}

export const print: PrintMethod<IfStatementNode> = (path, options, print) => {
    return "TODO IfStatement";
};
