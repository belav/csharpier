import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CaseSwitchLabelNode extends SyntaxTreeNode<"CaseSwitchLabel"> {
    keyword: SyntaxToken;
    value: SyntaxTreeNode;
}

export const printCaseSwitchLabel: PrintMethod<CaseSwitchLabelNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "keyword"), " ", path.call(print, "value"), ":"]);
};
