import { printStatements } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AnonymousObjectCreationExpressionNode extends SyntaxTreeNode<"AnonymousObjectCreationExpression"> {
    newKeyword: SyntaxToken;
    initializers: SyntaxTreeNode;
}

export const printAnonymousObjectCreationExpression: PrintMethod<AnonymousObjectCreationExpressionNode> = (
    path,
    options,
    print
) => {
    const node = path.getValue();
    return concat(["new", printStatements(node, "initializers", line, path, print, ",")]);
};
