import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IsPatternExpressionNode extends SyntaxTreeNode<"IsPatternExpression"> {

}

export const print: PrintMethod<IsPatternExpressionNode> = (path, options, print) => {
    return "TODO IsPatternExpression";
};
