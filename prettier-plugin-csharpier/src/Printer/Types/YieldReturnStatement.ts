import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface YieldReturnStatementNode extends SyntaxTreeNode<"YieldReturnStatement"> {}

export const print: PrintMethod<YieldReturnStatementNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node YieldReturnStatement" : "";
};
