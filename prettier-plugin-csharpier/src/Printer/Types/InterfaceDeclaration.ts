import { Doc } from "prettier";
import { printValue, HasModifiers, HasValue, SyntaxTreeNode, HasIdentifier, printIdentifier } from "../SyntaxTreeNode";
import { PrintMethod } from "../PrintMethod";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printModifiers } from "../PrintModifiers";

export interface InterfaceDeclarationNode extends SyntaxTreeNode<"InterfaceDeclaration">, HasModifiers, HasIdentifier {
    members: SyntaxTreeNode[];
}

// TODO combine this with class?
export const print: PrintMethod<InterfaceDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printModifiers(node));
    parts.push("interface");
    parts.push(" ", printIdentifier(node));

    const hasMembers = node.members.length > 0;
    if (hasMembers) {
        parts.push(concat([hardline, "{"]));
        parts.push(indent(concat([hardline, join(doubleHardline, path.map(print, "members"))])));
        parts.push(hardline);
        parts.push("}");
    } else {
        parts.push(" ", "{", " ", "}");
    }

    return concat(parts);
};
