import { Doc } from "prettier";
import { concat, hardline, indent, join } from "./Builders";
import { printAttributeLists } from "./PrintAttributeLists";
import { printLeadingComments } from "./PrintComments";
import { printConstraintClauses } from "./PrintConstraintClauses";
import { printExtraNewLines } from "./PrintExtraNewLines";
import { PrintMethod } from "./PrintMethod";
import { SyntaxToken, printIdentifier, printModifiers, printSyntaxToken, SyntaxTreeNode } from "./SyntaxTreeNode";
import { AttributeListNode } from "./Types/AttributeList";
import { BaseListNode, printBaseList } from "./Types/BaseList";
import { TypeParameterConstraintClauseNode } from "./Types/TypeParameterConstraintClause";
import { printTypeParameterList } from "./Types/TypeParameterList";

export interface ClassLikeDeclarationNode
    extends SyntaxTreeNode<"ClassDeclaration" | "EnumDeclaration" | "StructDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    identifier: SyntaxToken;
    enumKeyword?: SyntaxToken;
    typeParameterList?: SyntaxTreeNode;
    baseList?: BaseListNode;
    constraintClauses: TypeParameterConstraintClauseNode[];
    members: SyntaxTreeNode[];
    arity?: number;
}

export const printClassLikeDeclaration: PrintMethod<ClassLikeDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];

    printExtraNewLines(node, parts, "attributeLists", "modifiers", "keyword");

    printAttributeLists(node, parts, path, options, print);
    printLeadingComments(node, parts, "modifiers", "keyword", "identifier");
    parts.push(printModifiers(node));
    if (node.keyword) {
        parts.push(printSyntaxToken(node.keyword));
    }
    if (node.enumKeyword) {
        parts.push(printSyntaxToken(node.enumKeyword));
    }
    parts.push(" ", printIdentifier(node));
    if (node.typeParameterList) {
        parts.push(path.call(innerPath => printTypeParameterList(innerPath, options, print), "typeParameterList"));
    }

    if (node.baseList) {
        parts.push(path.call(innerPath => printBaseList(innerPath, options, print), "baseList"));
    }

    printConstraintClauses(node, parts, path, options, print);

    const hasMembers = node.members.length > 0;
    if (hasMembers) {
        parts.push(concat([node.constraintClauses?.length > 0 ? "" : hardline, "{"]));
        let lineSeparator = hardline;
        if (node.nodeType === "EnumDeclaration") {
            lineSeparator = concat([",", hardline]);
        }
        parts.push(indent(concat([hardline, join(lineSeparator, path.map(print, "members"))])));
        parts.push(hardline);
        parts.push("}");
    } else {
        parts.push(node.constraintClauses?.length > 0 ? "" : " ", "{", " ", "}");
    }

    return concat(parts);
};
