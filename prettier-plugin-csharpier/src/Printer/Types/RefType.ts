import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RefTypeNode extends SyntaxTreeNode<"RefType"> {
    refKeyword: SyntaxToken;
    readOnlyKeyword: SyntaxToken;
    type: SyntaxTreeNode;
}

export const printRefType: PrintMethod<RefTypeNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        printSyntaxToken(node.refKeyword),
        node.readOnlyKeyword ? " " + printSyntaxToken(node.readOnlyKeyword) : "",
        " ",
        path.call(print, "type")
    ]);
};
