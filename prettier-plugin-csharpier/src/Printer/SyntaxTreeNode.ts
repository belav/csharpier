import { Doc, FastPath, ParserOptions } from "prettier";
import { concat, hardline } from "./Builders";
import { hasLeadingExtraLine } from "./Helpers";
import { Print } from "./PrintMethod";
import { BlockNode } from "./Types/Block";

export interface HasTrivia {
    leadingTrivia?: SyntaxTreeNode[];
    trailingTrivia?: SyntaxTreeNode[];
}

export interface SyntaxTreeNode<T = string> extends HasTrivia {
    nodeType: T;
}

export interface HasValue extends HasTrivia {
    value: string;
    valueText: string;
    text: string;
}

export interface HasIdentifier {
    identifier: HasValue;
}

export interface HasModifiers {
    modifiers: (HasValue & HasTrivia)[];
}

export function printPathIdentifier<T extends HasIdentifier>(path: FastPath<T>) {
    return printIdentifier(path.getValue());
}

export function printIdentifier(hasIdentifier: HasIdentifier) {
    return printValue(hasIdentifier.identifier);
}

export function printPathValue<T1, T2 extends keyof T1>(path: FastPath<T1>, property: T2) {
    const node = path.getValue();
    return printValue((node[property] as any) as HasValue);
}

export function printValue(hasValue: HasValue) {
    if (typeof hasValue.text === "undefined") {
        throw new Error("There was no text property on " + JSON.stringify(hasValue, null, "    "));
    }

    return hasLeadingExtraLine(hasValue) ? concat([hardline, hasValue.text]) : hasValue.text;
}

export interface LeftRightOperator {
    left: SyntaxTreeNode;
    operatorToken: HasValue;
    right: SyntaxTreeNode;
}

export function printLeftRightOperator<T extends LeftRightOperator>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([
        path.call(print, "left"),
        " ",
        printPathValue(path, "operatorToken"),
        " ",
        path.call(print, "right"),
    ]);
}

export interface Operator {
    operatorToken: HasValue;
    operand: SyntaxTreeNode;
}

export function printPostOperator<T extends Operator>(path: FastPath<T>, options: ParserOptions<T>, print: Print<T>) {
    return concat([path.call(print, "operand"), printPathValue(path, "operatorToken")]);
}

export function printPreOperator<T extends Operator>(path: FastPath<T>, options: ParserOptions<T>, print: Print<T>) {
    return concat([printPathValue(path, "operatorToken"), path.call(print, "operand")]);
}

interface KeywordBlock {
    keyword: HasValue;
    block: BlockNode;
}

export function printKeywordBlock<T extends KeywordBlock>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([printPathValue(path, "keyword"), path.call(print, "block")]);
}

interface KeywordType {
    keyword: HasValue;
    type: SyntaxTreeNode;
}

export function printKeywordType<T extends KeywordType>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([printPathValue(path, "keyword"), "(", path.call(print, "type"), ")"]);
}

interface KeywordExpression {
    keyword: HasValue;
    expression: SyntaxTreeNode;
}

export function printKeywordExpression<T extends KeywordExpression>(
    path: FastPath<T>,
    options: ParserOptions<T>,
    print: Print<T>,
) {
    return concat([printPathValue(path, "keyword"), "(", path.call(print, "expression"), ")"]);
}
