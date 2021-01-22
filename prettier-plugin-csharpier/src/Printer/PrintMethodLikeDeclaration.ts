import { Doc } from "prettier";
import { PrintMethod } from "./PrintMethod";
import {
    printSyntaxToken,
    HasModifiers,
    SyntaxToken,
    SyntaxTreeNode,
    HasIdentifier,
    printIdentifier,
    printModifiers,
} from "./SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "./Builders";
import { ParameterListNode } from "./Types/ParameterList";

export interface MethodLikeDeclarationNode
    extends SyntaxTreeNode<"MethodDeclaration" | "OperatorDeclaration" | "InterfaceDeclaration">,
        HasModifiers,
        HasIdentifier {
    attributeLists: SyntaxTreeNode[];
    returnType?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: { name: HasIdentifier };
    implicitOrExplicitKeyword?: SyntaxToken;
    operatorKeyword?: SyntaxToken;
    operatorToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    parameterList: ParameterListNode;
    body?: SyntaxTreeNode;
    typeParameterList?: SyntaxTreeNode;
}

export const printMethodLikeDeclaration: PrintMethod<MethodLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    if (node.returnType) {
        parts.push(path.call(print, "returnType"), " ");
    }
    if (node.explicitInterfaceSpecifier) {
        parts.push(printIdentifier(node.explicitInterfaceSpecifier.name), ".")
    }
    if (node.identifier) {
        parts.push(printIdentifier(node));
    }
    if (node.implicitOrExplicitKeyword) {
        // TODO there are probably a lot more optimizations to be made like this
        parts.push(printSyntaxToken(node.implicitOrExplicitKeyword), " ");
    }
    if (node.operatorKeyword) {
        parts.push(printSyntaxToken(node.operatorKeyword), " ");
    }
    if (node.operatorToken) {
        parts.push(printSyntaxToken(node.operatorToken));
    }
    if (node.type) {
        parts.push(path.call(print, "type"));
    }

    if (node.typeParameterList) {
        parts.push(path.call(print, "typeParameterList"));
    }
    parts.push(path.call(print, "parameterList"));
    if (node.body) {
        parts.push(path.call(print, "body"));
    } else {
        parts.push(";");
    }

    return concat(parts);
};
