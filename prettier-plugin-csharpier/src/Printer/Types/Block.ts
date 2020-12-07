import { Doc } from "prettier";
import { getParentNode } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BlockNode extends SyntaxTreeNode<"Block"> {
    statements: SyntaxTreeNode[];
}

export const print: PrintMethod<BlockNode> = (path, options, print) => {
    const node = path.getValue();
    const parent = getParentNode(path);
    const statementSeparator = parent.nodeType === "GetAccessorDeclaration" || parent.nodeType === "SetAccessorDeclaration" ? line : hardline;
    const hasStatements = node.statements.length > 0;
    let body: Doc = " ";
    if (hasStatements) {
        body = concat([indent(concat([statementSeparator, join(statementSeparator, path.map(print, "statements"))])), statementSeparator]);
    }
    return group(concat([line, "{", body, "}"]));
};
