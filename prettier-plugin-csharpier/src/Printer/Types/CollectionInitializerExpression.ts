import { Doc } from "prettier";
import { printStatements } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { printLeftRightOperator, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CollectionInitializerExpressionNode extends SyntaxTreeNode<"CollectionInitializerExpression"> {
    expressions: SyntaxTreeNode[];
}

export const print: PrintMethod<CollectionInitializerExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    const hasExpressions = node.expressions.length > 0;
    let body: Doc = " ";
    if (hasExpressions) {
        body = concat([indent(concat([hardline, join(concat([",", hardline]), path.map(print, "expressions"))])), line]);
    }
    return group(concat([line, "{", body, "}"]));
};
