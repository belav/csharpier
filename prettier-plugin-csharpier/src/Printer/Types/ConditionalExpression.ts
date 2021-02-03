import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConditionalExpressionNode extends SyntaxTreeNode<"ConditionalExpression"> {
    condition: SyntaxTreeNode;
    questionToken: SyntaxTreeNode;
    whenTrue: SyntaxTreeNode;
    colonToken: SyntaxTreeNode;
    whenFalse: SyntaxTreeNode;
}

export const printConditionalExpression: PrintMethod<ConditionalExpressionNode> = (path, options, print) => {
    return concat([
        path.call(print, "condition"),
        " ? ",
        path.call(print, "whenTrue"),
        " : ",
        path.call(print, "whenFalse")
    ]);
};
