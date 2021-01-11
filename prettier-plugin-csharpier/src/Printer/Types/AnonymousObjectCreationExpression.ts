import { printStatements } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AnonymousObjectCreationExpressionNode extends SyntaxTreeNode<"AnonymousObjectCreationExpression"> {
    newKeyword: HasValue;
    initializers: SyntaxTreeNode;
}

export const print: PrintMethod<AnonymousObjectCreationExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return concat(["new", printStatements(node, "initializers", line, path, print, ",")]);
};
