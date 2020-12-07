import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface YieldBreakStatementNode extends SyntaxTreeNode<"YieldBreakStatement"> {

}

export const print: PrintMethod<YieldBreakStatementNode> = (path, options, print) => {
    return "TODO YieldBreakStatement";
};
