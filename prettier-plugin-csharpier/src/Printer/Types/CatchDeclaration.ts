import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CatchDeclarationNode extends SyntaxTreeNode<"CatchDeclaration">, HasIdentifier {
    type: SyntaxTreeNode;

}

export const print: PrintMethod<CatchDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    return concat(["(", path.call(print, "type"), printIdentifier(node), ")"]);
};
