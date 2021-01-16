import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { ParameterNode } from "./Parameter";

export interface SimpleLambdaExpressionNode extends SyntaxTreeNode<"SimpleLambdaExpression"> {
    parameter: ParameterNode;
    arrowToken: HasValue;
    body: SyntaxTreeNode;
}

export const print: PrintMethod<SimpleLambdaExpressionNode> = (path, options, print) => {
    return concat([
        path.call(print, "parameter"),
        " ",
        printPathValue(path, "arrowToken"),
        " ",
        path.call(print, "body"),
    ]);
};
