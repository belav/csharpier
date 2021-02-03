import { Doc, ParserOptions } from "prettier";
import { concat, hardline, indent, join } from "./Builders";
import { FastPath, Print } from "./PrintMethod";
import { SyntaxTreeNode } from "./SyntaxTreeNode";
import {
    printTypeParameterConstraintClause,
    TypeParameterConstraintClauseNode
} from "./Types/TypeParameterConstraintClause";

interface HasConstraintClauses extends SyntaxTreeNode {
    constraintClauses: TypeParameterConstraintClauseNode[];
}

export function printConstraintClauses(
    node: HasConstraintClauses,
    parts: Doc[],
    path: FastPath,
    options: ParserOptions,
    print: Print
) {
    if (!node.constraintClauses || node.constraintClauses.length == 0) {
        return;
    }

    if (node.constraintClauses.length > 0) {
        parts.push(
            indent(
                concat([
                    hardline,
                    join(
                        hardline,
                        path.map(
                            innerPath => printTypeParameterConstraintClause(innerPath, options, print),
                            "constraintClauses"
                        )
                    )
                ])
            )
        );

        if (
            node.nodeType !== "DelegateDeclaration" &&
            node.nodeType !== "MethodDeclaration" &&
            node.nodeType !== "LocalFunctionStatement"
        ) {
            parts.push(hardline);
        }
    }
}
