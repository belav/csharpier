import { Doc } from "prettier";
import { PrintMethod } from "./PrintMethod";
import {
    printValue,
    HasModifiers,
    HasValue,
    SyntaxTreeNode,
    HasIdentifier,
    printIdentifier,
    printModifiers,
} from "./SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "./Builders";
import { ParameterListNode } from "./Types/ParameterList";

export interface MethodLikeDeclarationNode
    extends SyntaxTreeNode<"MethodDeclaration" | "OperatorDeclaration">,
        HasModifiers,
        HasIdentifier {
    attributeLists: SyntaxTreeNode[];
    returnType: SyntaxTreeNode;
    operatorKeyword?: HasValue;
    operatorToken?: HasValue;
    parameterList: ParameterListNode;
    body?: SyntaxTreeNode;
    typeParameterList?: SyntaxTreeNode;
}

export const print: PrintMethod<MethodLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    parts.push(path.call(print, "returnType"));
    if (node.identifier) {
        parts.push(" ", printIdentifier(node));
    } else {
        parts.push(" ", printValue(node.operatorKeyword!), " ", printValue(node.operatorToken!));
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
