import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IsPatternExpressionNode extends SyntaxTreeNode<"IsPatternExpression"> {
    expression: SyntaxTreeNode;
    isKeyword: HasValue;
    pattern: SyntaxTreeNode;
}

export const print: PrintMethod<IsPatternExpressionNode> = (path, options, print) => {
    return concat([
        path.call(print, "expression"),
        " ",
        printPathValue(path, "isKeyword"),
        " ",
        path.call(print, "pattern"),
    ]);
};
