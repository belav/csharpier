import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, HasValue, printIdentifier, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ForEachStatementNode extends SyntaxTreeNode<"ForEachStatement">, HasIdentifier {
    forEachKeyword: HasValue;
    inKeyword: HasValue;
}

export const print: PrintMethod<ForEachStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printValue(node.forEachKeyword), " ", "(");
    parts.push(path.call(print, "type"), " ", printIdentifier(node), " ", printValue(node.inKeyword), " ", path.call(print, "expression"));
    parts.push(")");
    parts.push(path.call(print, "statement"));

    return concat(parts);
};
