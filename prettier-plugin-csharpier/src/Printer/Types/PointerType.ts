import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PointerTypeNode extends SyntaxTreeNode<"PointerType"> {
    elementType: SyntaxTreeNode;
    asteriskToken: SyntaxToken;
}

export const printPointerType: PrintMethod<PointerTypeNode> = (path, options, print) => {
    return concat([path.call(print, "elementType"), printPathSyntaxToken(path, "asteriskToken")]);
};
