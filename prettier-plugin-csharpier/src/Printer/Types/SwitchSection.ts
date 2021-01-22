import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SwitchSectionNode extends SyntaxTreeNode<"SwitchSection"> {
    labels: SyntaxTreeNode[];
    statements: SyntaxTreeNode[];
}

export const printSwitchSection: PrintMethod<SwitchSectionNode> = (path, options, print) => {
    return concat([join(hardline, path.map(print, "labels")), concat(path.map(print, "statements"))]);
};
