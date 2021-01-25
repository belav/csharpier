import { Doc } from "prettier";
import { printAttributeLists } from "../PrintAttributeLists";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, printModifiers, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { EqualsValueClauseNode, printEqualsValueClause } from "./EqualsValueClause";

export interface ParameterNode extends SyntaxTreeNode<"Parameter"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    identifier: SyntaxToken;
    default?: EqualsValueClauseNode;
}

export const printParameter: PrintMethod<ParameterNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.attributeLists) {
        printAttributeLists(node, parts, path, options, print);
    }
    parts.push(printModifiers(node));
    if (node.type) {
        parts.push(path.call(print, "type"), " ");
    }
    parts.push(printIdentifier(node));
    if (node.default) {
        parts.push(path.call(o => printEqualsValueClause(o, options, print), "default"));
    }

    return concat(parts);
};
