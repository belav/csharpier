import { Doc, FastPath, ParserOptions } from "prettier";
import { Node } from "./Node";

export type PrintMethod<T = Node> = (
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: (path: FastPath<T>) => Doc,
) => Doc;
