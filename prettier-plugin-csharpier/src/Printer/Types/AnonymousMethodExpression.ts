import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AnonymousMethodExpressionNode extends SyntaxTreeNode<"AnonymousMethodExpression"> {

}

export const print: PrintMethod<AnonymousMethodExpressionNode> = (path, options, print) => {
    return "TODO AnonymousMethodExpression";
};
