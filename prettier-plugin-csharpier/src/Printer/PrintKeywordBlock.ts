import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { HasValue, printPathValue } from "./SyntaxTreeNode";
import { BlockNode } from "./Types/Block";

interface KeywordBlock {
    keyword: HasValue;
    block: BlockNode;
}

export function print<T extends KeywordBlock>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([printPathValue(path, "keyword"), path.call(print, "block")]);
}
