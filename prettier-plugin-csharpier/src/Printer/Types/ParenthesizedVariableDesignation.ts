import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ParenthesizedVariableDesignationNode extends SyntaxTreeNode<"ParenthesizedVariableDesignation"> {}

export const print: PrintMethod<ParenthesizedVariableDesignationNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node ParenthesizedVariableDesignation" : "";
};
