import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ElementBindingExpressionNode extends SyntaxTreeNode<"ElementBindingExpression"> {

}

export const print: PrintMethod<ElementBindingExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ElementBindingExpression" : "";
};
