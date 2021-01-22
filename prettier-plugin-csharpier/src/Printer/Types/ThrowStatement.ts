import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ThrowStatementNode extends SyntaxTreeNode<"ThrowStatement"> {
    throwKeyword: SyntaxToken;
    expression?: SyntaxTreeNode;
}

export const printThrowStatement: PrintMethod<ThrowStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const expression = node.expression ? concat([" ", path.call(print, "expression")]) : "";
    return concat([printSyntaxToken(node.throwKeyword), expression, node.nodeType === "ThrowStatement" ? ";" : ""]);
};
