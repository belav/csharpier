import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NameEqualsNode extends SyntaxTreeNode<"NameEquals"> {
    name: SyntaxTreeNode;
    equalsToken: SyntaxToken;
}

export const printNameEquals: PrintMethod<NameEqualsNode> = (path, options, print) => {
    return concat([path.call(print, "name"), " ", printPathSyntaxToken(path, "equalsToken")]);
};
