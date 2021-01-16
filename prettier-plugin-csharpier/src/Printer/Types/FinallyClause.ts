import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FinallyClauseNode extends SyntaxTreeNode<"FinallyClause"> {
    finallyKeyword: HasValue;
    block: SyntaxTreeNode;
}

export const print: PrintMethod<FinallyClauseNode> = (path, options, print) => {
    return concat([printPathValue(path, "finallyKeyword"), path.call(print, "block")]);
};
