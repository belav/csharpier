import { Doc } from "prettier";
import { printStatements } from "../Helpers";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ObjectInitializerExpressionNode extends SyntaxTreeNode<"ObjectInitializerExpression"> {
    expressions?: SyntaxTreeNode[];
}

export const printObjectInitializerExpression: PrintMethod<ObjectInitializerExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    return printStatements(node, "expressions", line, path, print, ",");
};
