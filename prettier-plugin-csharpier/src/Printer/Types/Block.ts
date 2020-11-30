import { PrintMethod } from "../PrintMethod";
import { getValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BlockNode extends SyntaxTreeNode<"Block"> {
    statements: SyntaxTreeNode[];
}

export const print: PrintMethod<BlockNode> = (path, options, print) => {
    return group(concat([line, "{", line, indent(concat(path.map(print, "statements"))), line, "}"]));
};
