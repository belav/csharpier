import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { ElseClauseNode } from "./ElseClause";

export interface IfStatementNode extends SyntaxTreeNode<"IfStatement"> {
    attributeLists: AttributeListNode[];
    ifKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
    else?: ElseClauseNode;
}

export const printIfStatement: PrintMethod<IfStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [printSyntaxToken(node.ifKeyword), " ", "(", path.call(print, "condition"), ")"];

    let statement = path.call(print, "statement");
    if (node.statement?.nodeType === "Block") {
        parts.push(statement);
    } else {
        parts.push(indent(concat([hardline, statement])));
    }

    if (node.else) {
        parts.push(hardline, path.call(print, "else"));
    }

    return concat(parts);
};
