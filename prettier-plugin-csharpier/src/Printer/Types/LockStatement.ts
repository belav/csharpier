import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode, printSyntaxToken } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";

export interface LockStatementNode extends SyntaxTreeNode<"LockStatement"> {
    attributeLists: AttributeListNode[];
    lockKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

export const printLockStatement: PrintMethod<LockStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [printSyntaxToken(node.lockKeyword), " ", "(", path.call(print, "expression"), ")"];

    let statement = path.call(print, "statement");
    if (node.statement?.nodeType === "Block") {
        parts.push(statement);
    } else {
        parts.push(indent(concat([hardline, statement])));
    }

    return concat(parts);
};
