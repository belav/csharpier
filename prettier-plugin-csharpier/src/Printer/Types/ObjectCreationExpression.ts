import { PrintMethod } from "../PrintMethod";
import { HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ObjectCreationExpressionNode extends SyntaxTreeNode<"ObjectCreationExpression"> {
    newKeyword: HasValue;
    type: SyntaxTreeNode;
    argumentList?: SyntaxTreeNode;
    initializer?: SyntaxTreeNode;
}

export const print: PrintMethod<ObjectCreationExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        "new",
        " ",
        path.call(print, "type"),
        node.argumentList ? path.call(print, "argumentList") : "",
        node.initializer ? path.call(print, "initializer") : "",
    ]);
};
