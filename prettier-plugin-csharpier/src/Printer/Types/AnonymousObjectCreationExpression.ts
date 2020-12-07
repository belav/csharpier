import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AnonymousObjectCreationExpressionNode extends SyntaxTreeNode<"AnonymousObjectCreationExpression"> {

}

export const print: PrintMethod<AnonymousObjectCreationExpressionNode> = (path, options, print) => {
    return "TODO AnonymousObjectCreationExpression";
};
