import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasModifiers, printModifiers, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printVariableDeclaration, VariableDeclarationNode } from "./VariableDeclaration";

export interface LocalDeclarationStatementNode extends SyntaxTreeNode<"LocalDeclarationStatement"> {
    awaitKeyword?: SyntaxToken;
    usingKeyword?: SyntaxToken;
    modifiers: SyntaxToken[];
    declaration?: VariableDeclarationNode;
    isConst?: boolean;
}

export const printLocalDeclarationStatement: PrintMethod<LocalDeclarationStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.awaitKeyword) {
        parts.push("await ");
    }
    if (node.usingKeyword) {
        parts.push("using ");
    }
    parts.push(printModifiers(node));
    parts.push(path.call(o => printVariableDeclaration(o, options, print), "declaration"));
    parts.push(";");
    return concat(parts);
};
