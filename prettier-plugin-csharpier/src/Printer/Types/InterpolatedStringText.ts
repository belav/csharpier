import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InterpolatedStringTextNode extends SyntaxTreeNode<"InterpolatedStringText"> {
    textToken: HasValue;
}

export const print: PrintMethod<InterpolatedStringTextNode> = (path, options, print) => {
    return printPathValue(path, "textToken");
};
