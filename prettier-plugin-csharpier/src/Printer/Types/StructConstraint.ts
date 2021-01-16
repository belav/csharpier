import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface StructConstraintNode extends SyntaxTreeNode<"StructConstraint"> {
    classOrStructKeyword: HasValue;
    questionToken: HasValue;
}

export const print: PrintMethod<StructConstraintNode> = (path, options, print) => {
    return concat([printPathValue(path, "classOrStructKeyword"), printPathValue(path, "questionToken")]);
};
