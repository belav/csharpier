import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface YieldReturnStatementNode extends SyntaxTreeNode<"YieldReturnStatement"> {
    yieldKeyword: HasValue;
    returnOrBreakKeyword: HasValue;
    expression?: SyntaxTreeNode;
}

export const print: PrintMethod<YieldReturnStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const expression = node.expression ? concat([" ", path.call(print, "expression")]) : "";
    return concat([printValue(node.yieldKeyword), " ", printValue(node.returnOrBreakKeyword), expression, ";"]);
};
