import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DeclarationExpressionNode extends SyntaxTreeNode<"DeclarationExpression"> {
    type: SyntaxTreeNode;
    designation: SyntaxTreeNode;
}

export const print: PrintMethod<DeclarationExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "type"), " ", path.call(print, "designation")]);
};
