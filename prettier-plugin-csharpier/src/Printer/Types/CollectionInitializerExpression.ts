import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CollectionInitializerExpressionNode extends SyntaxTreeNode<"CollectionInitializerExpression"> {
    expressions: SyntaxTreeNode[];
}

export const printCollectionInitializerExpression: PrintMethod<CollectionInitializerExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    const hasExpressions = node.expressions.length > 0;
    let body: Doc = " ";
    if (hasExpressions) {
        body = concat([indent(concat([hardline, join(concat([",", hardline]), path.map(print, "expressions"))])), line]);
    }
    return group(concat([line, "{", body, "}"]));
};
