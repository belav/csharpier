import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { BlockNode } from "./Block";
import { IfStatementNode } from "./IfStatement";

export interface ElseClauseNode extends SyntaxTreeNode<"ElseClause"> {
    elseKeyword: HasValue;
    statement: BlockNode | IfStatementNode;
}

export const print: PrintMethod<ElseClauseNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([printValue(node.elseKeyword), " ", path.call(print, "statement")]);
};
