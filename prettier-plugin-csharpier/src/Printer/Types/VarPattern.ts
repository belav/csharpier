import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface VarPatternNode extends SyntaxTreeNode<"VarPattern"> {
    varKeyword?: SyntaxToken;
    designation?: SyntaxTreeNode;
}

export const printVarPattern: PrintMethod<VarPatternNode> = (path, options, print) => {
    return concat(["var ", path.call(print, "designation")]);
};
