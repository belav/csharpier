import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { print as otherPrint } from "./SetAccessorDeclaration"

export interface GetAccessorDeclarationNode extends SyntaxTreeNode<"GetAccessorDeclaration"> {
    keyword: HasValue;
    body: unknown;
}

export const print: PrintMethod<GetAccessorDeclarationNode> = (path, options, print) => {
    return otherPrint(path as any, options as any, print as any);
};
