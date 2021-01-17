import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SelectClauseNode extends SyntaxTreeNode<"SelectClause"> {
    selectKeyword: HasValue;
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<SelectClauseNode> = (path, options, print) => {
    return concat([printPathValue(path, "selectKeyword"), " ", path.call(print, "expression")]);
};
