import { Doc, FastPath, ParserOptions } from "prettier";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

export type PrintMethod<T = SyntaxTreeNode> = (
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: (path: FastPath<T>) => Doc,
) => Doc;
