import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, HasValue, printIdentifier, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DelegateDeclarationNode extends SyntaxTreeNode<"DelegateDeclaration">, HasIdentifier {
    delegateKeyword: HasValue;
    returnType: SyntaxTreeNode;
    typeParameterList?: SyntaxTreeNode;
    parameterList: SyntaxTreeNode;
    constraintClauses: SyntaxTreeNode[];
}

export const print: PrintMethod<DelegateDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printValue(node.delegateKeyword), " ");
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
