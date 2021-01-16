import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RefExpressionNode extends SyntaxTreeNode<"RefExpression"> {
    refKeyword: HasValue;
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<RefExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "refKeyword"), " ", path.call(print, "expression")]);
};
