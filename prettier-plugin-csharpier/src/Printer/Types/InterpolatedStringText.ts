import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InterpolatedStringTextNode extends SyntaxTreeNode<"InterpolatedStringText"> {
    textToken: SyntaxToken;
}

export const printInterpolatedStringText: PrintMethod<InterpolatedStringTextNode> = (path, options, print) => {
    return printPathSyntaxToken(path, "textToken");
};
