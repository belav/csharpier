import { Doc } from "prettier";
import { concat, group, hardline, indent, join, line } from "./Builders";
import { printComments } from "./Comments";
import { printAttributeLists } from "./PrintAttributeLists";
import { PrintMethod } from "./PrintMethod";
import {
    HasIdentifier,
    HasModifiers,
    SyntaxToken,
    printIdentifier,
    printModifiers,
    printSyntaxToken,
    SyntaxTreeNode,
} from "./SyntaxTreeNode";
import { IndexerDeclaration } from "./Types";
import { AccessorDeclarationNode } from "./Types/AccessorDeclaration";
import { ArrowExpressionClauseNode } from "./Types/ArrowExpressionClause";
import { AttributeListNode } from "./Types/AttributeList";
import { EqualsValueClauseNode } from "./Types/EqualsValueClause";
import { ParameterNode } from "./Types/Parameter";

export interface PropertyDeclarationNode extends SyntaxTreeNode<"PropertyDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: ExplicitInterfaceSpecifierNode;
    identifier: SyntaxToken;
    accessorList?: AccessorListNode;
    expressionBody?: ArrowExpressionClauseNode;
    initializer?: EqualsValueClauseNode;
}

interface AccessorListNode extends SyntaxTreeNode<"AccessorList"> {
    accessors: AccessorDeclarationNode[];
}

interface ExplicitInterfaceSpecifierNode extends SyntaxTreeNode<"ExplicitInterfaceSpecifier"> {
    name?: SyntaxTreeNode;
    dotToken?: SyntaxToken;
}

export interface EventDeclarationNode extends SyntaxTreeNode<"EventDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    eventKeyword?: SyntaxToken;
    type?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: ExplicitInterfaceSpecifierNode;
    identifier: SyntaxToken;
    accessorList?: AccessorListNode;
}

export interface IndexerDeclarationNode extends SyntaxTreeNode<"IndexerDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    type?: SyntaxTreeNode;
    explicitInterfaceSpecifier?: ExplicitInterfaceSpecifierNode;
    thisKeyword?: SyntaxToken;
    parameterList?: BracketedParameterListNode;
    accessorList?: AccessorListNode;
    expressionBody?: ArrowExpressionClauseNode;
}

interface BracketedParameterListNode extends SyntaxTreeNode<"BracketedParameterList"> {
    openBracketToken?: SyntaxToken;
    parameters: ParameterNode[];
    closeBracketToken?: SyntaxToken;
}

export const printPropertyLikeDeclaration: PrintMethod<PropertyDeclarationNode | IndexerDeclarationNode | EventDeclarationNode> = (path, options, print) => {
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
        identifier = concat([printSyntaxToken(node.thisKeyword), "[", join(", ", path.map(print, "parameterList", "parameters")), "]"]);
    } else if (node.nodeType === "EventDeclaration") {
        identifier = printIdentifier(node);
    }

    const parts: Doc[] = [];

    printAttributeLists(node, parts, path, options, print);

    return group(
        concat([
            concat(parts),
            printModifiers(node),
            (node.nodeType === "EventDeclaration" ? printSyntaxToken(node.eventKeyword) + " ": ""),
            path.call(print, "type"),
            " ",
            identifier,
            line,
            contents,
        ]),
    );
};
