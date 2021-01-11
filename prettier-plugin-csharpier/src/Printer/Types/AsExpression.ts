import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AsExpressionNode extends SyntaxTreeNode<"AsExpression"> {

}

export const print: PrintMethod<AsExpressionNode> = (path, options, print) => {
    return "TODO AsExpression";
};
