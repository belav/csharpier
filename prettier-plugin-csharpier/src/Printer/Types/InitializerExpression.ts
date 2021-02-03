import { printCommaList } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InitializerExpressionNode
    extends SyntaxTreeNode<
        "InitializerExpression",
        | "ComplexElementInitializerExpression"
        | "ArrayInitializerExpression"
        | "ObjectInitializerExpression"
        | "CollectionInitializerExpression"
    > {
    expressions: SyntaxTreeNode[];
}

export const printInitializerExpression: PrintMethod<InitializerExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return group(
        concat([
            node.kind === "ArrayInitializerExpression"
                ? ""
                : node.kind === "ComplexElementInitializerExpression"
                ? softline
                : line,
            "{",
            indent(concat([line, printCommaList(path.map(print, "expressions"))])),
            line,
            "}"
        ])
    );
};
