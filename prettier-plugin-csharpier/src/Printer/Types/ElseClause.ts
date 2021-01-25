import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { BlockNode } from "./Block";
import { IfStatementNode } from "./IfStatement";

export interface ElseClauseNode extends SyntaxTreeNode<"ElseClause"> {
    elseKeyword?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

export const printElseClause: PrintMethod<ElseClauseNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [printPathSyntaxToken(path, "elseKeyword")];
    let statement = path.call(print, "statement");
    if (node.statement?.nodeType === "Block") {
        parts.push(statement);
    } else if (node.statement?.nodeType === "IfStatement") {
        parts.push(" ", statement);
    } else {
        parts.push(indent(concat([hardline, statement])));
    }

    return concat(parts);
};
