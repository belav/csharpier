import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface WhenClauseNode extends SyntaxTreeNode<"WhenClause"> {
    whenKeyword: HasValue;
    condition: SyntaxTreeNode;
}

export const print: PrintMethod<WhenClauseNode> = (path, options, print) => {
    return concat([printPathValue(path, "whenKeyword"), " ", path.call(print, "condition")]);
};
