import { PrintMethod } from "../PrintMethod";
import { printPathIdentifier, printPathSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ExternAliasDirectiveNode extends SyntaxTreeNode<"ExternAliasDirective"> {
    externKeyword?: SyntaxToken;
    aliasKeyword?: SyntaxToken;
    identifier: SyntaxToken;
}

export const printExternAliasDirective: PrintMethod<ExternAliasDirectiveNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "externKeyword"),
        " ",
        printPathSyntaxToken(path, "aliasKeyword"),
        " ",
        printPathIdentifier(path),
        ";",
    ]);
};
