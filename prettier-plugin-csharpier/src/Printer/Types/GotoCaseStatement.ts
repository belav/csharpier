import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface GotoCaseStatementNode extends SyntaxTreeNode<"GotoCaseStatement"> {
    gotoKeyword: HasValue;
    caseOrDefaultKeyword: HasValue;
    expression?: SyntaxTreeNode;
}

export const print: PrintMethod<GotoCaseStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const expression = node.expression ? " " + path.call(print, "expression") : "";
    return concat([printValue(node.gotoKeyword), " ", printValue(node.caseOrDefaultKeyword), expression, ";"]);
};
