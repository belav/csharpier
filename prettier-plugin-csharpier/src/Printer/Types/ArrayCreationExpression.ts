import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { InitializerExpressionNode } from "./InitializerExpression";

export interface ArrayCreationExpressionNode extends SyntaxTreeNode<"ArrayCreationExpression"> {
    newKeyword: SyntaxToken;
    type: SyntaxTreeNode;
    initializer?: InitializerExpressionNode;
}

export const printArrayCreationExpression: PrintMethod<ArrayCreationExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return group(
        concat([
            printPathSyntaxToken(path, "newKeyword"),
            " ",
            path.call(print, "type"),
            node.initializer ? concat([line, path.call(print, "initializer")]) : ""
        ])
    );
};
