import { PrintMethod } from "../PrintMethod";
import { HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AnonymousObjectCreationExpressionNode extends SyntaxTreeNode<"AnonymousObjectCreationExpression"> {
    newKeyword: HasValue;
    initializer: SyntaxTreeNode;
}

export const print: PrintMethod<AnonymousObjectCreationExpressionNode> = (path, options, print) => {
    return concat(["new", " { ", concat(path.map(print, "initializers")), " }"]);
};
