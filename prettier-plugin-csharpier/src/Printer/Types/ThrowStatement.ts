import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ThrowStatementNode extends SyntaxTreeNode<"ThrowStatement"> {
    throwKeyword: HasValue;
    expression: SyntaxTreeNode
}

export const print: PrintMethod<ThrowStatementNode> = (path, options, print) => {
    return concat([printPathValue(path, "throwKeyword"), " ", path.call(print, "expression"), ";"])
};
