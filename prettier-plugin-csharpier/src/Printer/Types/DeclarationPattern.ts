import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DeclarationPatternNode extends SyntaxTreeNode<"DeclarationPattern"> {
    type: SyntaxTreeNode;
    designation: SyntaxTreeNode;
}

export const print: PrintMethod<DeclarationPatternNode> = (path, options, print) => {
    return concat([path.call(print, "type"), " ", path.call(print, "designation")]);
};
