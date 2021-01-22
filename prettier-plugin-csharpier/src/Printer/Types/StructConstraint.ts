import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StructConstraintNode extends SyntaxTreeNode<"StructConstraint"> {
    classOrStructKeyword: SyntaxToken;
    questionToken: SyntaxToken;
}

export const printStructConstraint: PrintMethod<StructConstraintNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "classOrStructKeyword"), printPathSyntaxToken(path, "questionToken")]);
};
