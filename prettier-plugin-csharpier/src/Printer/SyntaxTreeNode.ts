import { Doc, FastPath } from "prettier";
import { concat, hardline } from "./Builders";
import { hasLeadingExtraLine } from "./Helpers";
import { Print } from "./PrintMethod";

export interface HasTrivia
{
    leadingTrivia?: SyntaxTreeNode[];
    trailingTrivia?: SyntaxTreeNode[];
}

export interface SyntaxTreeNode<T = string> extends HasTrivia{
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

export function printIdentifier(hasIdentifier: HasIdentifier) {
    return printValue(hasIdentifier.identifier);
}

export function printPathValue<T1, T2 extends keyof T1>(path: FastPath<T1>, property: T2) {
    const node = path.getValue();
    return printValue(node[property] as any as HasValue);
}

export function printValue(hasValue: HasValue) {
    if (typeof hasValue.text === "undefined") {
        throw new Error("There was no text property on " + JSON.stringify(hasValue, null, "    "));
    }

    return hasLeadingExtraLine(hasValue) ? concat([hardline, hasValue.text]) : hasValue.text;
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
