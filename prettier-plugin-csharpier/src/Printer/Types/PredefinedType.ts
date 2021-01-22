import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode, printPathSyntaxToken } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PredefinedTypeNode extends SyntaxTreeNode<"PredefinedType"> {
    keyword: SyntaxToken;
}

export const printPredefinedType: PrintMethod<PredefinedTypeNode> = (path, options, print) => {
    return printPathSyntaxToken(path, "keyword");
};
