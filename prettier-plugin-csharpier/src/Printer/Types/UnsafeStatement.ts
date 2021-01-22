import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UnsafeStatementNode extends SyntaxTreeNode<"UnsafeStatement"> {
    unsafeKeyword: SyntaxToken;
    block: SyntaxTreeNode;
}

export const printUnsafeStatement: PrintMethod<UnsafeStatementNode> = (path, options, print) => {
    return concat([printPathSyntaxToken(path, "unsafeKeyword"), path.call(print, "block")]);
};
