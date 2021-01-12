import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ContinueStatementNode extends SyntaxTreeNode<"ContinueStatement"> {
    continueKeyword: HasValue;
}

export const print: PrintMethod<ContinueStatementNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([printValue(node.continueKeyword), ";"]);
};
