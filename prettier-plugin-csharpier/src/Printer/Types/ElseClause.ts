import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { BlockNode } from "./Block";
import { IfStatementNode } from "./IfStatement";

export interface ElseClauseNode extends SyntaxTreeNode<"ElseClause"> {
    elseKeyword: HasValue;
    statement: BlockNode | IfStatementNode;
}

export const print: PrintMethod<ElseClauseNode> = (path, options, print) => {
    return concat([printPathValue(path, "elseKeyword"), " ", path.call(print, "statement")]);
};
