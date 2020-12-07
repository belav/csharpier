import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SwitchStatementNode extends SyntaxTreeNode<"SwitchStatement"> {

}

export const print: PrintMethod<SwitchStatementNode> = (path, options, print) => {
    return "TODO SwitchStatement";
};
