import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ImplicitArrayCreationExpressionNode extends SyntaxTreeNode<"ImplicitArrayCreationExpression"> {
    newKeyword: SyntaxToken;
    commas: SyntaxTreeNode[];
    initializer: SyntaxTreeNode;
}

export const printImplicitArrayCreationExpression: PrintMethod<ImplicitArrayCreationExpressionNode> = (
    path,
    options,
    print,
) => {
    const node = path.getValue();
    const commas = node.commas.map(o => ",");

    return concat([
        printPathSyntaxToken(path, "newKeyword"),
        "[",
        concat(commas),
        "]",
        " ",
        path.call(print, "initializer"),
    ]);
};
