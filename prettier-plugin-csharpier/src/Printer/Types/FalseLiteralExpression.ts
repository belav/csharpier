import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface FalseLiteralExpressionNode extends SyntaxTreeNode<"FalseLiteralExpression"> {

}

export const print: PrintMethod<FalseLiteralExpressionNode> = (path, options, print) => {
    return "false";
};
