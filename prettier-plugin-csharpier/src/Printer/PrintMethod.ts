import { Doc, ParserOptions } from "prettier";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

export interface FastPath<T = any> {
    stack: T[];
    getName(): null | PropertyKey;
    getValue(): T;
    getNode(count?: number): null | T;
    getParentNode(count?: number): null | T;
    call<U>(callback: (path: FastPath) => U, ...names: PropertyKey[]): U;
    each(callback: (path: FastPath) => void, ...names: PropertyKey[]): void;
    map<U>(callback: (path: FastPath, index: number) => U, ...names: PropertyKey[]): U[];
}

export type Print = (path: FastPath) => Doc;

export type PrintMethod<T = SyntaxTreeNode> = (path: FastPath<T>, options: ParserOptions, print: Print) => Doc;
