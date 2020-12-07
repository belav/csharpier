import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FixedStatementNode extends SyntaxTreeNode<"FixedStatement"> {

}

export const print: PrintMethod<FixedStatementNode> = (path, options, print) => {
    return "TODO FixedStatement";
};
