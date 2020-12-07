import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IfStatementNode extends SyntaxTreeNode<"IfStatement"> {
    ifKeyword: HasValue;
}

export const print: PrintMethod<IfStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printValue(node.ifKeyword), " ", "(", path.call(print, "condition"), ")", path.call(print, "statement"));

    return concat(parts);
};
