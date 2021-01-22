import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, SyntaxToken, printPathIdentifier, SyntaxTreeNode, printSyntaxToken } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TypeParameterNode extends SyntaxTreeNode<"TypeParameter">, HasIdentifier {
    varianceKeyword?: SyntaxToken;
}

export const printTypeParameter: PrintMethod<TypeParameterNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([node.varianceKeyword ? printSyntaxToken(node.varianceKeyword) + " " : "", printPathIdentifier(path)]);
};
