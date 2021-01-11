import { Doc, FastPath } from "prettier";
import { concat, group, indent, join, line } from "./Builders";
import { Print } from "./PrintMethod";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

export function printStatements<T extends SyntaxTreeNode, T2 extends SyntaxTreeNode>(
    node: T2,
    propertyName: keyof T2,
    separator: Doc,
    path: FastPath<T>,
    print: Print<T>,
    endOfLineDoc?: Doc,
) {
    const actualEndOfLine = endOfLineDoc ? concat([endOfLineDoc, separator]) : separator;

    const hasStatements = node[propertyName] && ((node[propertyName] as unknown) as SyntaxTreeNode[]).length > 0;
    let body: Doc = " ";
    if (hasStatements) {
        body = concat([indent(concat([separator, join(actualEndOfLine, path.map(print, propertyName))])), separator]);
    }
    return group(concat([line, "{", body, "}"]));
}

export function printCommaList(list: Doc[]) {
    return join(concat([",", line]), list);
}

export function getParentNode(path: FastPath) {
    const result = path.stack[path.stack.length - 3] as SyntaxTreeNode;
    if (typeof result !== "object" || !result.nodeType) {
        throw new Error("The object " + result + " did not appear to be a SyntaxTreeNode");
    }

    return result;
}
