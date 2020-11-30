import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { getValue, HasModifiers, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printModifiers } from "../PrintModifiers";

export interface PropertyDeclarationNode extends SyntaxTreeNode<"PropertyDeclaration">, HasModifiers {
    identifier: HasValue;
    accessorList?: SyntaxTreeNode;
    expressionBody?: SyntaxTreeNode;
}

export const print: PrintMethod<PropertyDeclarationNode> = (path, options, print) => {
    const node = path.getValue();

    let contents: Doc;
    if (node.accessorList) {
        contents = concat(["{", indent(concat(path.map(print, "accessorList", "accessors"))), line, "}");
    } else {
        contents = path.call(print, "expressionBody");
    }

    return group(
        concat([
            printModifiers(node),
            path.call(print, "type"),
            " ",
            getValue(node.identifier),
            line,
            contents,
        ]),
    );
};
