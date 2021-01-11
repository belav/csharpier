import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SwitchExpressionNode extends SyntaxTreeNode<"SwitchExpression"> {

}

export const print: PrintMethod<SwitchExpressionNode> = (path, options, print) => {
    return "TODO SwitchExpression";
};
