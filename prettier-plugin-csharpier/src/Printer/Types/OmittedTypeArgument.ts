import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface OmittedTypeArgumentNode extends SyntaxTreeNode<"OmittedTypeArgument"> {}

export const printOmittedTypeArgument: PrintMethod<OmittedTypeArgumentNode> = (path, options, print) => {
    return "";
};
