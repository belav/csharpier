import { PrintMethod } from "../PrintMethod";
import { HasModifiers, printModifiers, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LocalDeclarationStatementNode extends SyntaxTreeNode<"LocalDeclarationStatement">, HasModifiers {}

export const printLocalDeclarationStatement: PrintMethod<LocalDeclarationStatementNode> = (path, options, print) => {
    return concat([printModifiers(path.getValue()), path.call(print, "declaration"), ";"]);
};
