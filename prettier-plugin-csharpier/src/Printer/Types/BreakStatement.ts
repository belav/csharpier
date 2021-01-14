import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BreakStatementNode extends SyntaxTreeNode<"BreakStatement"> {
    breakKeyword: HasValue;
}

export const print: PrintMethod<BreakStatementNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([printValue(node.breakKeyword), ";"]);
};
