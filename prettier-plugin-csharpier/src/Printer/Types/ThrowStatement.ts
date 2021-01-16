import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ThrowStatementNode extends SyntaxTreeNode<"ThrowStatement"> {
    throwKeyword: HasValue;
    expression?: SyntaxTreeNode;
}

export const print: PrintMethod<ThrowStatementNode> = (path, options, print) => {
    const node = path.getValue()
    const expression = node.expression ? concat([" ", path.call(print, "expression")]) : "";
    return concat([printValue(node.throwKeyword), expression, ";"]);
};
