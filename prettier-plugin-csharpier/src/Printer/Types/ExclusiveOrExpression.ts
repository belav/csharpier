import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ExclusiveOrExpressionNode extends SyntaxTreeNode<"ExclusiveOrExpression"> {

}

export const print: PrintMethod<ExclusiveOrExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ExclusiveOrExpression" : "";
};
