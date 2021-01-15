import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EqualsValueClauseNode extends SyntaxTreeNode<"EqualsValueClause"> {
    equalsToken: HasValue;
}

export const print: PrintMethod<EqualsValueClauseNode> = (path, options, print) => {
    return concat([printPathValue(path, "equalsToken"), " ", path.call(print, "value")]);
};
