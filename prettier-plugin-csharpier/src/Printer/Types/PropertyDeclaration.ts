import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printValue, HasModifiers, HasValue, SyntaxTreeNode, HasIdentifier, printIdentifier } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printModifiers } from "../PrintModifiers";

export interface PropertyDeclarationNode extends SyntaxTreeNode<"PropertyDeclaration">, HasModifiers, HasIdentifier {
    accessorList?: SyntaxTreeNode;
    expressionBody?: SyntaxTreeNode;
    semicolon?: HasValue;
}

export const print: PrintMethod<PropertyDeclarationNode> = (path, options, print) => {
    const node = path.getValue();

    let contents: Doc;
    if (node.accessorList) {
        contents = concat(["{", indent(concat(path.map(print, "accessorList", "accessors"))), line, "}"]);
    } else {
        contents = concat([path.call(print, "expressionBody"), ";"]);
    }

    return group(
        concat([
            printModifiers(node),
            path.call(print, "type"),
            " ",
            printIdentifier(node),
            line,
            contents,
        ]),
    );
};
