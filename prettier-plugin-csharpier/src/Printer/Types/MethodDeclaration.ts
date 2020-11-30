import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printValue, HasModifiers, HasValue, SyntaxTreeNode, HasIdentifier, printIdentifier } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printModifiers } from "../PrintModifiers";

export interface MethodDeclarationNode extends SyntaxTreeNode<"MethodDeclaration">, HasModifiers, HasIdentifier {
    returnType: SyntaxTreeNode;
    parameterList: SyntaxTreeNode;
    body: SyntaxTreeNode;
}

export const print: PrintMethod<MethodDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    parts.push(path.call(print, "returnType"));
    parts.push(" ", printIdentifier(node));

    parts.push("() {}");

    return concat(parts);
};
