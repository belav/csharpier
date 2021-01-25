import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TypeParameterConstraintClauseNode extends SyntaxTreeNode<"TypeParameterConstraintClause"> {
    whereKeyword: SyntaxToken;
    name: SyntaxTreeNode;
    constraints: SyntaxTreeNode[];
}

export const printTypeParameterConstraintClause: PrintMethod<TypeParameterConstraintClauseNode> = (
    path,
    options,
    print,
) => {
    return concat([
        printPathSyntaxToken(path, "whereKeyword"),
        " ",
        path.call(print, "name"),
        " : ",
        join(", ", path.map(print, "constraints")),
    ]);
};
