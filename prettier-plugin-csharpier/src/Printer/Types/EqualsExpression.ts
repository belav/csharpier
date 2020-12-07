import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EqualsExpressionNode extends SyntaxTreeNode<"EqualsExpression"> {

}

export const print: PrintMethod<EqualsExpressionNode> = (path, options, print) => {
    return "TODO EqualsExpression";
};
