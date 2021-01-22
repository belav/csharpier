import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { ParameterNode } from "./Parameter";

export interface SimpleLambdaExpressionNode extends SyntaxTreeNode<"SimpleLambdaExpression"> {
    parameter: ParameterNode;
    arrowToken: SyntaxToken;
    body: SyntaxTreeNode;
}

export const printSimpleLambdaExpression: PrintMethod<SimpleLambdaExpressionNode> = (path, options, print) => {
    return concat([
        path.call(print, "parameter"),
        " ",
        printPathSyntaxToken(path, "arrowToken"),
        " ",
        path.call(print, "body"),
    ]);
};
