import { Doc } from "prettier";
import { concat, group, hardline, indent, join, line } from "./Builders";
import { printComments } from "./Comments";
import { PrintMethod } from "./PrintMethod";
import {
    HasIdentifier,
    HasModifiers,
    HasValue,
    printIdentifier,
    printModifiers,
    printValue,
    SyntaxTreeNode,
} from "./SyntaxTreeNode";
import { IndexerDeclaration } from "./Types";
import { BaseListNode } from "./Types/BaseList";

export interface PropertyDeclarationNode
    extends SyntaxTreeNode<"PropertyDeclaration">,
        HasModifiers,
        HasIdentifier {
    type: SyntaxTreeNode;
    accessorList?: SyntaxTreeNode;
    expressionBody?: SyntaxTreeNode;
}

export interface IndexerDeclarationNode extends SyntaxTreeNode<"IndexerDeclaration">, HasModifiers {
    type: SyntaxTreeNode;
    thisKeyword: HasValue;
    accessorList?: SyntaxTreeNode;
    parameterList: SyntaxTreeNode;
    expressionBody?: SyntaxTreeNode;
}

export const print: PrintMethod<PropertyDeclarationNode | IndexerDeclarationNode> = (path, options, print) => {
    const node = path.getValue();

    let contents: Doc;
    if (node.accessorList) {
        contents = concat(["{", indent(concat(path.map(print, "accessorList", "accessors"))), line, "}"]);
    } else {
        contents = concat([path.call(print, "expressionBody"), ";"]);
    }

    let identifier: Doc = "";
    if (node.nodeType === "PropertyDeclaration") {
        identifier = printIdentifier(node);
    } else if (node.nodeType === "IndexerDeclaration") {
        identifier = concat([printValue(node.thisKeyword), "[", join(", ", path.map(print, "parameterList", "parameters")), "]"]);
    }

    return group(
        concat([
            printModifiers(node),
            path.call(print, "type"),
            " ",
            identifier,
            line,
            contents,
        ]),
    );
};
