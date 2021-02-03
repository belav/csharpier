import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode, printSyntaxToken } from "./SyntaxTreeNode";

interface KeywordExpression {
    keyword: SyntaxToken;
    expression: SyntaxTreeNode;
    type?: SyntaxTreeNode;
}

export function printKeywordExpression<T extends KeywordExpression>(
    path: FastPath<T>,
    options: ParserOptions,
    print: Print
) {
    const node = path.getValue();
    return concat([
        printSyntaxToken(node.keyword),
        "(",
        path.call(print, "expression"),
        node.type ? ", " + path.call(print, "type") : "",
        ")"
    ]);
}
