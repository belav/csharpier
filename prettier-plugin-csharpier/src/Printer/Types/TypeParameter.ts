import { Doc } from "prettier";
import { printAttributeLists } from "../PrintAttributeLists";
import { PrintMethod } from "../PrintMethod";
import {
    HasIdentifier,
    SyntaxToken,
    printPathIdentifier,
    SyntaxTreeNode,
    printSyntaxToken,
    printIdentifier,
} from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";

export interface TypeParameterNode extends SyntaxTreeNode<"TypeParameter"> {
    attributeLists: AttributeListNode[];
    varianceKeyword?: SyntaxToken;
    identifier: SyntaxToken;
}

export const printTypeParameter: PrintMethod<TypeParameterNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printAttributeLists(node, parts, path, options, print);
    if (node.varianceKeyword) {
        parts.push(printSyntaxToken(node.varianceKeyword), " ");
    }
    parts.push(printIdentifier(node));
    return concat(parts);
};
