import { Doc, FastPath } from "prettier";
import { concat } from "./Builders";

export interface SyntaxTreeNode<T = string> {
    nodeType: T;
}

export interface HasValue {
    value: string;
    valueText: string;
    text: string;
}

export interface HasIdentifier {
    identifier: HasValue;
}

export interface HasModifiers {
    modifiers: HasValue[];
}

export function printIdentifier(hasIdentifier: HasIdentifier) {
    return printValue(hasIdentifier.identifier);
}

export function printValue(hasValue: HasValue) {
    return hasValue.text;
}

export interface LeftRightExpression {
    left: SyntaxTreeNode;
    operatorToken: HasValue;
    right: SyntaxTreeNode;
}

export function printLeftRightExpression<T extends LeftRightExpression>(path: FastPath<T>, print: (path: FastPath<T>) => Doc,) {
    const node = path.getValue();
    return concat([path.call(print, "left"), " ", printValue(node.operatorToken), " ", path.call(print, "right")])
}
