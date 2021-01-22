import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface QualifiedNameNode extends SyntaxTreeNode<"QualifiedName"> {}

export const printQualifiedName: PrintMethod<QualifiedNameNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([path.call(print, "left"), ".", path.call(print, "right")]);
};
