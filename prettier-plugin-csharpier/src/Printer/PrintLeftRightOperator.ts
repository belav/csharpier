import { Doc, FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { printLeadingComments } from "./PrintComments";
import { printExtraNewLines } from "./PrintExtraNewLines";
import { Print } from "./PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "./SyntaxTreeNode";

export interface LeftRightOperator extends SyntaxTreeNode {
    left: SyntaxTreeNode;
    operatorToken: SyntaxToken;
    right: SyntaxTreeNode;
}

export function printLeftRightOperator<T extends LeftRightOperator>(
    path: FastPath<T>,
    options: ParserOptions,
    print: Print,
) {
    const node = path.getValue();
    const parts: Doc[] = [];
    printExtraNewLines(node, parts, ["left", "identifier"])
    printLeadingComments(node, parts, ["left", "identifier"]);
    parts.push(path.call(print, "left"), " ", printPathSyntaxToken(path, "operatorToken"), " ", path.call(print, "right"));
    return concat(parts);
}
