import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ClassOrStructConstraintNode extends SyntaxTreeNode<"ClassOrStructConstraint"> {
    classOrStructKeyword: SyntaxToken;
    questionToken: SyntaxToken;
}

export const printClassOrStructConstraint: PrintMethod<ClassOrStructConstraintNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "classOrStructKeyword"), printPathSyntaxToken(path, "questionToken")]);
};
