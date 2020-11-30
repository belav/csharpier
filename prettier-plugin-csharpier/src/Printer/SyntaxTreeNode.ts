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
