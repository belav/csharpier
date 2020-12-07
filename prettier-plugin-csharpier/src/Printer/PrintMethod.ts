import { Doc, FastPath, ParserOptions } from "prettier";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

export type Print<T> = (path: FastPath<T>) => Doc;

export type PrintMethod<T = SyntaxTreeNode> = (path: FastPath<T>, options: ParserOptions<T>, print: Print<T>) => Doc;
