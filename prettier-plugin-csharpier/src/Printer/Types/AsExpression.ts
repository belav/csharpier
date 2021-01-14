import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AsExpressionNode extends SyntaxTreeNode<"AsExpression"> {
    left: SyntaxTreeNode;
    operatorToken: HasValue;
    right: SyntaxTreeNode;
}

export const print: PrintMethod<AsExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([path.call(print, "left"), " ", printValue(node.operatorToken), " ", path.call(print, "right")])
};
