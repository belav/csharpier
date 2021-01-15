import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BreakStatementNode extends SyntaxTreeNode<"BreakStatement"> {
    breakKeyword: HasValue;
}

export const print: PrintMethod<BreakStatementNode> = (path, options, print) => {
    return concat([printPathValue(path, "breakKeyword"), ";"]);
};
