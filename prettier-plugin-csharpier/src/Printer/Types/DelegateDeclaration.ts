import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, SyntaxToken, printIdentifier, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DelegateDeclarationNode extends SyntaxTreeNode<"DelegateDeclaration">, HasIdentifier {
    delegateKeyword: SyntaxToken;
    returnType: SyntaxTreeNode;
    typeParameterList?: SyntaxTreeNode;
    parameterList: SyntaxTreeNode;
    constraintClauses: SyntaxTreeNode[];
}

export const printDelegateDeclaration: PrintMethod<DelegateDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printSyntaxToken(node.delegateKeyword), " ");
    parts.push(path.call(print, "returnType"));
    parts.push(" ", printIdentifier(node));
    if (node.typeParameterList) {
        parts.push(path.call(print, "typeParameterList"));
    }
    parts.push(path.call(print, "parameterList"));
    parts.push(concat(path.map(print, "constraintClauses")));
    parts.push(";");

    return concat(parts);
};
