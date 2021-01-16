import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UnsafeStatementNode extends SyntaxTreeNode<"UnsafeStatement"> {
    unsafeKeyword: HasValue;
    block: SyntaxTreeNode;
}

export const print: PrintMethod<UnsafeStatementNode> = (path, options, print) => {
    return concat([printPathValue(path, "unsafeKeyword"), path.call(print, "block")]);
};
