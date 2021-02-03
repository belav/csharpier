import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GotoCaseStatementNode extends SyntaxTreeNode<"GotoCaseStatement"> {
    gotoKeyword: SyntaxToken;
    caseOrDefaultKeyword?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

export const printGotoCaseStatement: PrintMethod<GotoCaseStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const expression = node.expression ? " " + path.call(print, "expression") : "";
    return concat([
        printSyntaxToken(node.gotoKeyword),
        node.caseOrDefaultKeyword ? " " : "",
        printSyntaxToken(node.caseOrDefaultKeyword),
        expression,
        ";"
    ]);
};
