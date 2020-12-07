import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TupleTypeNode extends SyntaxTreeNode<"TupleType"> {

}

export const print: PrintMethod<TupleTypeNode> = (path, options, print) => {
    return "TODO TupleType";
};
