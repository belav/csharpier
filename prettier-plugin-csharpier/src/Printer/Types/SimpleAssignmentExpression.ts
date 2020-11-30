import { PrintMethod } from "../PrintMethod";
import { getValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SimpleAssignmentExpressionNode extends SyntaxTreeNode<"SimpleAssignmentExpression"> {
    left: SyntaxTreeNode;
    operatorToken: HasValue;
    right: SyntaxTreeNode;
}

export const print: PrintMethod<SimpleAssignmentExpressionNode> = (path, options, print) => {
    const node = path.getValue();

    return concat([path.call(print, "left"), " ", getValue(node.operatorToken), " ", path.call(print, "right"), ";"])
};
