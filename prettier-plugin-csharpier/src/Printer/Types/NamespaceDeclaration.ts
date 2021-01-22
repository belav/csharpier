import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import has = Reflect.has;

export interface NamespaceDeclarationNode extends SyntaxTreeNode<"NamespaceDeclaration"> {
    namespaceKeyword: SyntaxToken;
    members: SyntaxTreeNode[];
    usings: SyntaxTreeNode[];
}

export const printNamespaceDeclaration: PrintMethod<NamespaceDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printSyntaxToken(node.namespaceKeyword));
    parts.push(" ");
    parts.push(path.call(print, "name"));

    const hasMembers = node.members.length > 0;
    const hasUsing = node.usings.length > 0;
    if (hasMembers || hasUsing) {
        parts.push(concat([hardline, "{"]));
        parts.push(
            indent(
                concat([
                    hardline,
                    hasUsing ? join(hardline, path.map(print, "usings")) : "",
                    hasUsing && hasMembers ? hardline : "",
                    hasMembers ? join(hardline, path.map(print, "members")) : "",
                ]),
            ),
        );
        parts.push(hardline);
        parts.push("}");
    } else {
        parts.push(" ", "{", "}");
    }

    return concat([concat(parts)]);
};
