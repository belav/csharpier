import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface ConversionOperatorDeclarationNode extends SyntaxTreeNode<"ConversionOperatorDeclaration"> {

}

export const print: PrintMethod<ConversionOperatorDeclarationNode> = (path, options, print) => {
    return "TODO ConversionOperatorDeclaration";
};
