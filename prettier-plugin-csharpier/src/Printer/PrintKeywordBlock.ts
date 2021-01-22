import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { SyntaxToken, printPathSyntaxToken } from "./SyntaxTreeNode";
import { BlockNode } from "./Types/Block";

interface KeywordBlock {
    keyword: SyntaxToken;
    block: BlockNode;
}

export function printKeywordBlock<T extends KeywordBlock>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([printPathSyntaxToken(path, "keyword"), path.call(print, "block")]);
}
