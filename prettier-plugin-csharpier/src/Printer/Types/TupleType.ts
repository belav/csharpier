import { printCommaList } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TupleTypeNode extends SyntaxTreeNode<"TupleType"> {
    elements: SyntaxTreeNode[];
}

export const printTupleType: PrintMethod<TupleTypeNode> = (path, options, print) => {
    return concat(["(", join(", ", path.map(print, "elements")), ")"]);
};
