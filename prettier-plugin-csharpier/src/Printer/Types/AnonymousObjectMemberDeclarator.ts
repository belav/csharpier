import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AnonymousObjectMemberDeclaratorNode extends SyntaxTreeNode<"AnonymousObjectMemberDeclarator"> {
    nameEquals: {
        name: SyntaxTreeNode;
    } | null;
    expression: SyntaxTreeNode;
}

export const print: PrintMethod<AnonymousObjectMemberDeclaratorNode> = (path, options, print) => {
    const node = path.getValue();
    const hasNameEquals = !!node.nameEquals;
    const parts: Doc[] = [];
    if (hasNameEquals) {
        parts.push(path.call(print, "nameEquals", "name"), " = ");
    }
    parts.push(path.call(print, "expression"));
    return concat(parts);
};
