import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface LocalFunctionStatementNode extends SyntaxTreeNode<"LocalFunctionStatement"> {

}

export const print: PrintMethod<LocalFunctionStatementNode> = (path, options, print) => {
    return "TODO LocalFunctionStatement";
};
