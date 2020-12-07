import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrayInitializerExpressionNode extends SyntaxTreeNode<"ArrayInitializerExpression"> {

}

export const print: PrintMethod<ArrayInitializerExpressionNode> = (path, options, print) => {
    return "TODO ArrayInitializerExpression";
};
