import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InterpolatedStringExpressionNode extends SyntaxTreeNode<"InterpolatedStringExpression"> {
    stringStartToken: SyntaxToken;
    contents: SyntaxTreeNode[];
    stringEndToken: SyntaxToken;
}

export const printInterpolatedStringExpression: PrintMethod<InterpolatedStringExpressionNode> = (
    path,
    options,
    print,
) => {
    const node = path.getValue();
    return concat([
        printSyntaxToken(node.stringStartToken),
        concat(path.map(print, "contents")),
        printSyntaxToken(node.stringEndToken),
    ]);
};
