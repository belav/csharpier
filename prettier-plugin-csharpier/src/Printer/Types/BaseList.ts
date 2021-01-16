import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { Doc } from "prettier";
import { printCommaList } from "../Helpers";

export interface BaseListNode extends SyntaxTreeNode<"BaseList"> {}

export const print: PrintMethod<BaseListNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(" ", ":", " ");
    parts.push(printCommaList(path.map(print, "types")));

    return concat(parts);
};
