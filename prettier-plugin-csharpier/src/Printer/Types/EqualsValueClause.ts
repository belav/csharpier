import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EqualsValueClauseNode extends SyntaxTreeNode<"EqualsValueClause"> {
    equalsToken: HasValue;
}

export const print: PrintMethod<EqualsValueClauseNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([printValue(node.equalsToken), " ", path.call(print, "value")])
};
