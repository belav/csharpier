import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ForEachStatementNode extends SyntaxTreeNode<"ForEachStatement"> {

}

export const print: PrintMethod<ForEachStatementNode> = (path, options, print) => {
    return "TODO ForEachStatement";
};
