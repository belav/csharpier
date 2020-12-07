import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IsExpressionNode extends SyntaxTreeNode<"IsExpression"> {

}

export const print: PrintMethod<IsExpressionNode> = (path, options, print) => {
    return "TODO IsExpression";
};
