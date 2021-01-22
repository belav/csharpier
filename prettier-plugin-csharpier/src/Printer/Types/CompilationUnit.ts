import { Doc } from "prettier";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { PrintMethod } from "../PrintMethod";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CompilationUnitNode extends SyntaxTreeNode<"ompilationUnit"> {
    usings: SyntaxTreeNode[];
    members: SyntaxTreeNode[];
}

export const printCompilationUnit: PrintMethod<CompilationUnitNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.usings.length > 0) {
        parts.push(join(hardline, path.map(print, "usings")), hardline);
    }

    if (node.members.length > 0) {
        parts.push(join(hardline, path.map(print, "members")));
    }

    if (parts[parts.length - 1] !== hardline) {
        parts.push(hardline);
    }

    return concat(parts);
};
