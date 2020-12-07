import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InterpolatedStringExpressionNode extends SyntaxTreeNode<"InterpolatedStringExpression"> {
    stringStartToken: HasValue;
    contents: SyntaxTreeNode[];
    stringEndToken: HasValue;
}

export const print: PrintMethod<InterpolatedStringExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        printValue(node.stringStartToken),
        concat(path.map(print, "contents")),
        printValue(node.stringEndToken),
    ]);
};
