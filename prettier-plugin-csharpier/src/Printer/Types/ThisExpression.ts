import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ThisExpressionNode extends SyntaxTreeNode<"ThisExpression"> {
    token: HasValue;
}

export const print: PrintMethod<ThisExpressionNode> = (path, options, print) => {
    return printValue(path.getValue().token);
};
