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

export interface SyntaxToken extends HasTrivia {
    text: string;
}

export interface HasIdentifier {
    identifier: SyntaxToken;
}

export interface HasModifiers {
    modifiers: SyntaxToken[];
}

// TODO move some of these into helpers? or who cares?

export function printPathIdentifier<T extends HasIdentifier>(path: FastPath<T>) {
    return printIdentifier(path.getValue());
}

export function printIdentifier(hasIdentifier: HasIdentifier) {
    return printSyntaxToken(hasIdentifier.identifier);
}

export function printPathSyntaxToken<T1, T2 extends keyof T1>(path: FastPath<T1>, property: T2) {
    const node = path.getValue();
    return printSyntaxToken((node[property] as any) as SyntaxToken);
}

export function printSyntaxToken(syntaxToken: SyntaxToken | undefined) {
    if (!syntaxToken) {
        return "";
    }
    if (typeof syntaxToken.text === "undefined") {
        throw new Error("There was no text property on " + JSON.stringify(syntaxToken, null, "    "));
    }

    return hasLeadingExtraLine(syntaxToken) ? concat([hardline, syntaxToken.text]) : syntaxToken.text;
}

export function printModifiers(node: HasModifiers) {
    if (node.modifiers.length === 0) {
        return "";
    }

    return concat([
        join(
            " ",
            node.modifiers.map(o => printSyntaxToken(o)),
        ),
        " ",
    ]);
}

export interface Operator {
    operatorToken: SyntaxToken;
    operand: SyntaxTreeNode;
}
