import { printCommaList } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TupleExpressionNode extends SyntaxTreeNode<"TupleExpression"> {
    arguments: SyntaxTreeNode[];
}

export const print: PrintMethod<TupleExpressionNode> = (path, options, print) => {
    return concat(["(", join(", ", path.map(print, "arguments")), ")"]);
};
