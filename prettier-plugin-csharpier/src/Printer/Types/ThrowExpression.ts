import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ThrowExpressionNode extends SyntaxTreeNode<"ThrowExpression"> {

}

export const print: PrintMethod<ThrowExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ThrowExpression" : "";
};
