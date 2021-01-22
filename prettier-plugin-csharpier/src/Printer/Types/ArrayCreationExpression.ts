import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrayCreationExpressionNode extends SyntaxTreeNode<"ArrayCreationExpression"> {
    newKeyword: SyntaxToken;
    type: SyntaxTreeNode;
}

export const printArrayCreationExpression: PrintMethod<ArrayCreationExpressionNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "newKeyword"), " ", path.call(print, "type")]);
};
