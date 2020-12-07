import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ObjectCreationExpressionNode extends SyntaxTreeNode<"ObjectCreationExpression"> {

}

export const print: PrintMethod<ObjectCreationExpressionNode> = (path, options, print) => {
    return "TODO ObjectCreationExpression";
};
