import { getParentNode, printStatements } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BlockNode extends SyntaxTreeNode<"Block"> {
    statements: SyntaxTreeNode[];
}

export const printBlock: PrintMethod<BlockNode> = (path, options, print) => {
    const node = path.getValue();
    const parent = getParentNode(path);
    const statementSeparator =
        parent.nodeType === "AccessorDeclaration" && node.statements.length <= 1 ? line : hardline;
    return printStatements(node, "statements", statementSeparator, path, print);
};
