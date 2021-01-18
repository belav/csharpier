import { Doc, FastPath, ParserOptions } from "prettier";
import { concat, hardline, join } from "./Builders";
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

// TODO 0 this is really SyntaxToken and we no longer give it value/valueText
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

// TODO move some of these into helpers? or who cares?

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

export function printValue(hasValue: HasValue | undefined) {
    if (!hasValue) {
        return "";
    }
    if (typeof hasValue.text === "undefined") {
        throw new Error("There was no text property on " + JSON.stringify(hasValue, null, "    "));
    }

    return hasLeadingExtraLine(hasValue) ? concat([hardline, hasValue.text]) : hasValue.text;
}

export function printModifiers(node: HasModifiers) {
    if (node.modifiers.length === 0) {
        return "";
    }

    return concat([
        join(
            " ",
            node.modifiers.map(o => printValue(o)),
        ),
        " ",
    ]);
}

export interface Operator {
    operatorToken: HasValue;
    operand: SyntaxTreeNode;
}
