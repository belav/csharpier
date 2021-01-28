import { FastPath, ParserOptions } from "prettier";
import { LeftRightOperator } from "../PrintLeftRightOperator";
import { Print, PrintMethod } from "../PrintMethod";
import { printPathSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RangeExpressionNode extends SyntaxTreeNode<"RangeExpression"> {
    leftOperand?: SyntaxTreeNode;
    operatorToken?: SyntaxToken;
    rightOperand?: SyntaxTreeNode;
}

export const printRangeExpression: PrintMethod<RangeExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        node.leftOperand ? path.call(print, "leftOperand") : "",
        printPathSyntaxToken(path, "operatorToken"),
        node.rightOperand ? path.call(print, "rightOperand") : "",
    ]);
};
