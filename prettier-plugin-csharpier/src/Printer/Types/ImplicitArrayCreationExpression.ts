import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ImplicitArrayCreationExpressionNode extends SyntaxTreeNode<"ImplicitArrayCreationExpression"> {
    newKeyword: HasValue;
    commas: SyntaxTreeNode[];
    initializer: SyntaxTreeNode;
}

export const print: PrintMethod<ImplicitArrayCreationExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    const commas = node.commas.map(o => ",");


    return concat([printPathValue(path, "newKeyword"), "[", concat(commas), "]", " ", path.call(print, "initializer")]);
};
