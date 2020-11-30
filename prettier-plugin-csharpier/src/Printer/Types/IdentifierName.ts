import { PrintMethod } from "../PrintMethod";
import { getValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IdentifierNameNode extends SyntaxTreeNode<"IdentifierName"> {
    identifier: HasValue;
}

export const print: PrintMethod<IdentifierNameNode> = (path, options, print) => {
    return getValue(path.getValue().identifier);
};
