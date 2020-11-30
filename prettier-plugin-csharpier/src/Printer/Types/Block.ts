import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BlockNode extends SyntaxTreeNode<"Block"> {
    statements: SyntaxTreeNode[];
}

export const print: PrintMethod<BlockNode> = (path, options, print) => {
    const hasStatements = path.getValue().statements.length > 0;
    let body: Doc = " ";
    if (hasStatements) {
        body = concat([indent(concat([line, join(hardline, path.map(print, "statements"))])), line]);
    }
    return group(concat([line, "{", body, "}"]));
};
