import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "./SyntaxTreeNode";

interface KeywordType {
    keyword: SyntaxToken;
    type: SyntaxTreeNode;
}

export function printKeywordType<T extends KeywordType>(
    path: FastPath<T>,
    options: ParserOptions,
    print: Print,
) {
    return concat([printPathSyntaxToken(path, "keyword"), "(", path.call(print, "type"), ")"]);
}
