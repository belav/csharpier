import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CheckedExpressionNode extends SyntaxTreeNode<"CheckedExpression"> {
    keyword: HasValue;
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<CheckedExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "keyword"), "(", path.call(print, "expression"), ")"]);
};
