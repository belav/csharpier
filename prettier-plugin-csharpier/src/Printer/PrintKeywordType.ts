import { FastPath, ParserOptions } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "./SyntaxTreeNode";

interface KeywordType {
    keyword: HasValue;
    type: SyntaxTreeNode;
}

export function print<T extends KeywordType>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([printPathValue(path, "keyword"), "(", path.call(print, "type"), ")"]);
}
