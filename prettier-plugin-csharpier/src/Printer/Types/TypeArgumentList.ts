import { printCommaList } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TypeArgumentListNode extends SyntaxTreeNode<"TypeArgumentList"> {}

export const printTypeArgumentList: PrintMethod<TypeArgumentListNode> = (path, options, print) => {
    return indent(group(printCommaList(path.map(print, "arguments"))));
};
