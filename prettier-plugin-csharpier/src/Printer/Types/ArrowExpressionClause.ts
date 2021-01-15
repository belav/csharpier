import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode, printPathValue } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrowExpressionClauseNode extends SyntaxTreeNode<"ArrowExpressionClause"> {
    arrowToken: HasValue;
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<ArrowExpressionClauseNode> = (path, options, print) => {
    return concat([printPathValue(path, "arrowToken"), " ", path.call(print, "expression")]);
};
