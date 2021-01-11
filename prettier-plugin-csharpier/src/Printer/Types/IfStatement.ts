import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { ElseClauseNode } from "./ElseClause";

export interface IfStatementNode extends SyntaxTreeNode<"IfStatement"> {
    ifKeyword: HasValue;
    else?: ElseClauseNode;
}

export const print: PrintMethod<IfStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printValue(node.ifKeyword), " ", "(", path.call(print, "condition"), ")", path.call(print, "statement"));
    if (node.else) {
        parts.push(hardline, path.call(print, "else"));
    }

    return concat(parts);
};
