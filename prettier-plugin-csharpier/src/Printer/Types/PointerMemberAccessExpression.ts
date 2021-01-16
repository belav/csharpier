import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PointerMemberAccessExpressionNode extends SyntaxTreeNode<"PointerMemberAccessExpression"> {
    expression: SyntaxTreeNode;
    operatorToken: HasValue;
    name: SyntaxTreeNode;
}

export const print: PrintMethod<PointerMemberAccessExpressionNode> = (path, options, print) => {
    return concat([path.call(print, "expression"), printPathValue(path, "operatorToken"), path.call(print, "name")]);
};
