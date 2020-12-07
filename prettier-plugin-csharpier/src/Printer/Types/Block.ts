import { Doc } from "prettier";
import { getParentNode, printStatements } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BlockNode extends SyntaxTreeNode<"Block"> {
    statements: SyntaxTreeNode[];
}

export const print: PrintMethod<BlockNode> = (path, options, print) => {
    const node = path.getValue();
    const parent = getParentNode(path);
    const statementSeparator =
        parent.nodeType === "GetAccessorDeclaration" || parent.nodeType === "SetAccessorDeclaration" ? line : hardline;
    return printStatements(node, "statements", statementSeparator, path, print);
};
