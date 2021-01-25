import { Doc } from "prettier";
import { printAttributeLists } from "../PrintAttributeLists";
import { PrintMethod } from "../PrintMethod";
import {
    HasIdentifier,
    HasModifiers,
    printIdentifier,
    printModifiers,
    SyntaxToken,
    SyntaxTreeNode,
} from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { EqualsValueClauseNode, printEqualsValueClause } from "./EqualsValueClause";

export interface EnumMemberDeclarationNode extends SyntaxTreeNode<"EnumMemberDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    identifier: SyntaxToken;
    equalsValue?: EqualsValueClauseNode;
}

export const printEnumMemberDeclaration: PrintMethod<EnumMemberDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printAttributeLists(node, parts, path, options, print);
    parts.push(printModifiers(node));
    parts.push(printIdentifier(node));
    if (node.equalsValue) {
        parts.push(path.call(o => printEqualsValueClause(o, options, print), "equalsValue"));
    }

    return concat(parts);
};
