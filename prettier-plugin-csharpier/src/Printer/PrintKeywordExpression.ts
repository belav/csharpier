import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "./SyntaxTreeNode";

interface KeywordExpression {
    keyword: SyntaxToken;
    expression: SyntaxTreeNode;
}

export function printKeywordExpression<T extends KeywordExpression>(
    path: FastPath<T>,
    options: ParserOptions,
    print: Print,
) {
    return concat([printPathSyntaxToken(path, "keyword"), "(", path.call(print, "expression"), ")"]);
}
