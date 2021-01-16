import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import {
    printValue,
    HasModifiers,
    HasValue,
    SyntaxTreeNode,
    HasIdentifier,
    printIdentifier,
    printModifiers
} from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { ParameterListNode } from "./ParameterList";

export interface MethodDeclarationNode extends SyntaxTreeNode<"MethodDeclaration">, HasModifiers, HasIdentifier {
    returnType: SyntaxTreeNode;
    parameterList: ParameterListNode;
    body?: SyntaxTreeNode;
    typeParameterList?: SyntaxTreeNode;
}

export const print: PrintMethod<MethodDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    parts.push(path.call(print, "returnType"));
    parts.push(" ", printIdentifier(node));
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
