import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CasePatternSwitchLabelNode extends SyntaxTreeNode<"CasePatternSwitchLabel"> {
    keyword: SyntaxToken;
    pattern: SyntaxTreeNode;
    whenClause: SyntaxTreeNode;
}

export const printCasePatternSwitchLabel: PrintMethod<CasePatternSwitchLabelNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "keyword"),
        " ",
        path.call(print, "pattern"),
        " ",
        path.call(print, "whenClause"),
        ":",
    ]);
};
