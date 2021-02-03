import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface YieldStatementNode extends SyntaxTreeNode<"YieldStatement"> {
    yieldKeyword: SyntaxToken;
    returnOrBreakKeyword: SyntaxToken;
    expression?: SyntaxTreeNode;
}

export const printYieldStatement: PrintMethod<YieldStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const expression = node.expression ? concat([" ", path.call(print, "expression")]) : "";
    return concat([
        printSyntaxToken(node.yieldKeyword),
        " ",
        printSyntaxToken(node.returnOrBreakKeyword),
        expression,
        ";"
    ]);
};
