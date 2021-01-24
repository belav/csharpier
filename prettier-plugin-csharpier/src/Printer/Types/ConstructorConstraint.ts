import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";

export interface ConstructorConstraintNode extends SyntaxTreeNode<"ConstructorConstraint"> {
    newKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    closeParenToken?: SyntaxToken;
}
// TODO optimize more like this, instead of calling printSyntaxToken when it is a known value
export const printConstructorConstraint: PrintMethod<ConstructorConstraintNode> = (path, options, print) => {
    return "new()";
};
