import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CasePatternSwitchLabelNode extends SyntaxTreeNode<"CasePatternSwitchLabel"> {
    keyword: HasValue;
    pattern: SyntaxTreeNode;
    whenClause: SyntaxTreeNode;
}

export const print: PrintMethod<CasePatternSwitchLabelNode> = (path, options, print) => {
    return concat([
        printPathValue(path, "keyword"),
        " ",
        path.call(print, "pattern"),
        " ",
        path.call(print, "whenClause"),
        ":",
    ]);
};
