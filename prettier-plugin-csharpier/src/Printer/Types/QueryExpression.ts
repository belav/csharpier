import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface QueryExpressionNode extends SyntaxTreeNode<"QueryExpression"> {

}

export const print: PrintMethod<QueryExpressionNode> = (path, options, print) => {
    return "TODO QueryExpression";
};
