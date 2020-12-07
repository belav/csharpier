import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface MakeRefExpressionNode extends SyntaxTreeNode<"MakeRefExpression"> {

}

export const print: PrintMethod<MakeRefExpressionNode> = (path, options, print) => {
    return "TODO MakeRefExpression";
};
