import { Doc, FastPath } from "prettier";
import { concat } from "./Builders";
import { Print } from "./PrintMethod";
import has = Reflect.has;

export interface HasTrivia
{
    leadingTrivia: SyntaxTreeNode[];
    trailingTrivia: SyntaxTreeNode[];
}

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
    modifiers: (HasValue & HasTrivia)[];
}

export function printIdentifier(hasIdentifier: HasIdentifier) {
    return printValue(hasIdentifier.identifier);
}

export function printValue(hasValue: HasValue) {
    if (typeof hasValue.text === "undefined") {
        throw new Error("There was no text property on " + JSON.stringify(hasValue, null, "    "));
    }
    return hasValue.text;
}

export interface LeftRightExpression {
    left: SyntaxTreeNode;
    operatorToken: HasValue;
    right: SyntaxTreeNode;
}

export function printLeftRightExpression<T extends LeftRightExpression>(
    path: FastPath<T>,
    print: Print<T>,
) {
    const node = path.getValue();
    return concat([path.call(print, "left"), " ", printValue(node.operatorToken), " ", path.call(print, "right")]);
}
