import { Doc } from "prettier";
import { concat, hardline, indent, join } from "./Builders";
import { printComments } from "./Comments";
import { printAttributeLists } from "./PrintAttributeLists";
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
    printComments(parts, node);
    printAttributeLists(node, parts, path, options, print);
    parts.push(printModifiers(node));
    if (node.keyword) {
        parts.push(printSyntaxToken(node.keyword));
    }
    if (node.enumKeyword) {
        parts.push(printSyntaxToken(node.enumKeyword));
    }
    parts.push(" ", printIdentifier(node));
    if (node.typeParameterList) {
        // TODO 1 go through each node, copy interface from the generated one, figure out which path.calls can be optimized to this version
        parts.push(path.call(innerPath => printTypeParameterList(innerPath, options, print), "typeParameterList"));
    }

    if (node.baseList) {
        parts.push(path.call(innerPath => printBaseList(innerPath, options, print), "baseList"));
    }

    const hasMembers = node.members.length > 0;
    if (hasMembers) {
        parts.push(concat([hardline, "{"]));
        let lineSeparator = hardline;
        if (node.nodeType === "EnumDeclaration") {
            lineSeparator = concat([",", hardline]);
        }
        parts.push(indent(concat([hardline, join(lineSeparator, path.map(print, "members"))])));
        parts.push(hardline);
        parts.push("}");
    } else {
        parts.push(" ", "{", " ", "}");
    }

    return concat(parts);
};
