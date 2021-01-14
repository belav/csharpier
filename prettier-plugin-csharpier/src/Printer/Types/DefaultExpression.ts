import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DefaultExpressionNode extends SyntaxTreeNode<"DefaultExpression"> {

}

export const print: PrintMethod<DefaultExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node DefaultExpression" : "";
};
