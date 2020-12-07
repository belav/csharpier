import { printCommaList } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface BracketedArgumentListNode extends SyntaxTreeNode<"BracketedArgumentList"> {
    arguments: SyntaxTreeNode[];
}

export const print: PrintMethod<BracketedArgumentListNode> = (path, options, print) => {
    return group(
        concat(["[", indent(concat([softline, printCommaList(path.map(print, "arguments"))])), softline, "]"]),
    );
};
