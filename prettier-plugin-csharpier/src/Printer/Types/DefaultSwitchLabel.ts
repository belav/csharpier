import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DefaultSwitchLabelNode extends SyntaxTreeNode<"DefaultSwitchLabel"> {
    keyword?: SyntaxToken;
    colonToken?: SyntaxToken;
}

export const printDefaultSwitchLabel: PrintMethod<DefaultSwitchLabelNode> = (path, options, print) => {
    return concat(["default:"]);
};
