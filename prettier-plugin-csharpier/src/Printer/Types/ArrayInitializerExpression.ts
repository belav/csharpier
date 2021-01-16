import { printCommaList } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ArrayInitializerExpressionNode extends SyntaxTreeNode<"ArrayInitializerExpression"> {
    expressions: SyntaxTreeNode[];
}

export const print: PrintMethod<ArrayInitializerExpressionNode> = (path, options, print) => {
    return group(concat(["{", indent(concat([line, printCommaList(path.map(print, "expressions"))])), line, "}"]));
};
