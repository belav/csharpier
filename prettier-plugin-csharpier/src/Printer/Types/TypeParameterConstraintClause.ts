import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface TypeParameterConstraintClauseNode extends SyntaxTreeNode<"TypeParameterConstraintClause"> {
    whereKeyword: HasValue;
    name: SyntaxTreeNode;
    constraints: SyntaxTreeNode[];
}

export const print: PrintMethod<TypeParameterConstraintClauseNode> = (path, options, print) => {
    return concat([
        " ",
        printPathValue(path, "whereKeyword"),
        " ",
        path.call(print, "name"),
        " : ",
        join(", ", path.map(print, "constraints")),
    ]);
};
