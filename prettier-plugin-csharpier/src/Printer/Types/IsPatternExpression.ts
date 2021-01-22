import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IsPatternExpressionNode extends SyntaxTreeNode<"IsPatternExpression"> {
    expression: SyntaxTreeNode;
    isKeyword: SyntaxToken;
    pattern: SyntaxTreeNode;
}

export const printIsPatternExpression: PrintMethod<IsPatternExpressionNode> = (path, options, print) => {
    return concat([
        path.call(print, "expression"),
        " ",
        printPathSyntaxToken(path, "isKeyword"),
        " ",
        path.call(print, "pattern"),
    ]);
};
