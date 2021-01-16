import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printValue, HasModifiers, HasValue, SyntaxTreeNode, printModifiers } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FieldDeclarationNode extends SyntaxTreeNode<"FieldDeclaration">, HasModifiers {
    declaration: SyntaxTreeNode;
}

export const print: PrintMethod<FieldDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));

    parts.push(path.call(print, "declaration"));
    parts.push(";");
    return concat(parts);
};
