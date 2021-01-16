import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrayCreationExpressionNode extends SyntaxTreeNode<"ArrayCreationExpression"> {
    newKeyword: HasValue;
    type: SyntaxTreeNode;
}

export const print: PrintMethod<ArrayCreationExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "newKeyword"), " ", path.call(print, "type")]);
};
