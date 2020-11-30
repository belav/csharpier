import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { getValue, HasModifiers, HasValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printModifiers } from "../PrintModifiers";
import { GetAccessorDeclarationNode } from "./GetAccessorDeclaration";
import { SetAccessorDeclarationNode } from "./SetAccessorDeclaration";

export interface PropertyDeclarationNode extends SyntaxTreeNode<"PropertyDeclaration">, HasModifiers {
    identifier: HasValue;
    accessorList: {
        accessors: (GetAccessorDeclarationNode | SetAccessorDeclarationNode)[];
    };
}

export const print: PrintMethod<PropertyDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    return group(
        concat([
            printModifiers(node),
            path.call(print, "type"),
            " ",
            getValue(node.identifier),
            line,
            "{",
            indent(concat([line, join(line, path.map(print, "accessorList", "accessors"))])),
            line,
            "}",
        ]),
    );
};
