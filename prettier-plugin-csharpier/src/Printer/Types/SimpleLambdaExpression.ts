import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { ParameterNode } from "./Parameter";

export interface SimpleLambdaExpressionNode extends SyntaxTreeNode<"SimpleLambdaExpression"> {
    parameter: ParameterNode;
    arrowToken: HasValue;
}

export const print: PrintMethod<SimpleLambdaExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([path.call(print, "parameter"), " ", printValue(node.arrowToken), " ", path.call(print, "body")]);
};
