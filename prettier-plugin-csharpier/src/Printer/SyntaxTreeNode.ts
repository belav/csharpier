export interface SyntaxTreeNode<T = string> {
    nodeType: T;
}

export interface HasValue {
    value: string;
    valueText: string;
    text: string;
}

export interface HasModifiers {
    modifiers: HasValue[];
}

export function getValue(hasValue: HasValue) {
    return hasValue.text;
}
