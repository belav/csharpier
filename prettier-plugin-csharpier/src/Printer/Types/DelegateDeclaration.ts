import { Doc } from "prettier";
import { printConstraintClauses } from "../PrintConstraintClauses";
import { PrintMethod } from "../PrintMethod";
import {
    HasIdentifier,
    SyntaxToken,
    printIdentifier,
    printSyntaxToken,
    SyntaxTreeNode,
    printModifiers,
} from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { ParameterListNode } from "./ParameterList";
import { TypeParameterConstraintClauseNode } from "./TypeParameterConstraintClause";
import { TypeParameterListNode } from "./TypeParameterList";

export interface DelegateDeclarationNode extends SyntaxTreeNode<"DelegateDeclaration"> {
    attributeLists: AttributeListNode[]; // TODO
    modifiers: SyntaxToken[];
    delegateKeyword?: SyntaxToken;
    returnType?: SyntaxTreeNode;
    identifier: SyntaxToken;
    typeParameterList?: TypeParameterListNode;
    parameterList?: ParameterListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    arity?: number;
}

export const printDelegateDeclaration: PrintMethod<DelegateDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    parts.push(printSyntaxToken(node.delegateKeyword), " ");
    parts.push(path.call(print, "returnType"));
    parts.push(" ", printIdentifier(node));
    if (node.typeParameterList) {
        parts.push(path.call(print, "typeParameterList"));
    }
    parts.push(path.call(print, "parameterList"));
    printConstraintClauses(node, parts, path, options, print);
    parts.push(";");

    return concat(parts);
};
