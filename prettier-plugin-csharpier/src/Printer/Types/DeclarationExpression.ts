import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DeclarationExpressionNode extends SyntaxTreeNode<"DeclarationExpression"> {

}

export const print: PrintMethod<DeclarationExpressionNode> = (path, options, print) => {
    return "TODO DeclarationExpression";
};
