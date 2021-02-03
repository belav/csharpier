import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printBlock } from "./Block";

export interface SwitchSectionNode extends SyntaxTreeNode<"SwitchSection"> {
    labels: SyntaxTreeNode[];
    statements: SyntaxTreeNode[];
}

export const printSwitchSection: PrintMethod<SwitchSectionNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [join(hardline, path.map(print, "labels"))];
    if (node.statements.length === 1 && node.statements[0].nodeType === "Block") {
        parts.push(path.call(o => printBlock(o, options, print), "statements", 0));
    } else {
        parts.push(indent(concat([hardline, concat(path.map(print, "statements"))])));
    }
    return concat(parts);
};
