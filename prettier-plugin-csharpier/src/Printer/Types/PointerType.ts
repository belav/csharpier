import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PointerTypeNode extends SyntaxTreeNode<"PointerType"> {
    elementType: SyntaxTreeNode;
    asteriskToken: HasValue;
}

export const print: PrintMethod<PointerTypeNode> = (path, options, print) => {
    return concat([path.call(print, "elementType"), printPathValue(path, "asteriskToken")]);
};
