import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, HasValue, printPathIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TypeParameterNode extends SyntaxTreeNode<"TypeParameter">, HasIdentifier {
    varianceKeyword: HasValue;
}

export const print: PrintMethod<TypeParameterNode> = (path, options, print) => {
    return printPathIdentifier(path);
};
