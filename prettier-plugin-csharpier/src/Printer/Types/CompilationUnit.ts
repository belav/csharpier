import { Doc } from "prettier";
import { Node } from "../Node";
import { PrintMethod } from "../PrintMethod";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CompilationUnitNode extends Node<"ompilationUnit"> {
    usings: Node[];
    members: Node[];
}

export const print: PrintMethod<CompilationUnitNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.usings.length > 0) {
        parts.push(concat(path.map(print, "usings")));
        parts.push(hardline);
    }

    if (node.members.length > 0) {
        parts.push(concat(path.map(print, "members")));
        parts.push(hardline);
    }


    return concat(parts);
};
