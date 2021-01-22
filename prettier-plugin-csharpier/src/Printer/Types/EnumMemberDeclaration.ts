import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, HasModifiers, printIdentifier, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface EnumMemberDeclarationNode extends SyntaxTreeNode<"EnumMemberDeclaration">, HasModifiers, HasIdentifier {
    attributeList: SyntaxTreeNode[];
    equalValue?: SyntaxTreeNode;
}

export const printEnumMemberDeclaration: PrintMethod<EnumMemberDeclarationNode> = (path, options, print) => {
    return printIdentifier(path.getValue());
};
