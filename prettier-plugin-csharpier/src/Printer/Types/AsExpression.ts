import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AsExpressionNode extends SyntaxTreeNode<"AsExpression"> {
    left: SyntaxTreeNode;
    operatorToken: HasValue;
    right: SyntaxTreeNode;
}

export const print: PrintMethod<AsExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "left"), " ", printPathValue(path, "operatorToken"), " ", path.call(print, "right")])
};
