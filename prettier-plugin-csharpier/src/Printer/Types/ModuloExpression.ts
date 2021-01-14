import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ModuloExpressionNode extends SyntaxTreeNode<"ModuloExpression"> {

}

export const print: PrintMethod<ModuloExpressionNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ModuloExpression" : "";
};
