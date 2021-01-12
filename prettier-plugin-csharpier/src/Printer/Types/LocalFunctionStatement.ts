import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { print as methodDeclarationPrint } from "./MethodDeclaration";

export interface LocalFunctionStatementNode extends SyntaxTreeNode<"LocalFunctionStatement"> {}

export const print: PrintMethod<LocalFunctionStatementNode> = (path, options, print) => {
    return methodDeclarationPrint(path as any, options as any, print as any);
};
