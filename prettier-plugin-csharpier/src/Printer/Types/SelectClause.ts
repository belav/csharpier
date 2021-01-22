import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SelectClauseNode extends SyntaxTreeNode<"SelectClause"> {
    selectKeyword: SyntaxToken;
    expression: SyntaxTreeNode;
}

export const printSelectClause: PrintMethod<SelectClauseNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "selectKeyword"), " ", path.call(print, "expression")]);
};
