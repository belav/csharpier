import { Doc, FastPath } from "prettier";
import { concat, group, hardline, indent, join, line } from "./Builders";
import { printTrailingComments } from "./PrintComments";
import { Print } from "./PrintMethod";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

export function printStatements<T extends SyntaxTreeNode, T2 extends SyntaxTreeNode>(
    node: T2,
    propertyName: keyof T2,
    separator: Doc,
    path: FastPath<T>,
    print: Print,
    endOfLineDoc?: Doc,
) {
    const actualEndOfLine = endOfLineDoc ? concat([endOfLineDoc, separator]) : separator;

    const hasStatements = node[propertyName] && ((node[propertyName] as unknown) as SyntaxTreeNode[]).length > 0;
    let body: Doc = " ";
    if (hasStatements) {
        body = concat([indent(concat([separator, join(actualEndOfLine, path.map(print, propertyName))])), separator]);
    }

    const parts: Doc[] = [line, "{", body, "}"];
    printTrailingComments(node, parts, "closeBraceToken");
    return group(concat(parts));
}

export function printCommaList(list: Doc[]) {
    return join(concat([",", line]), list);
}

export function getParentNode(path: FastPath) {
    let result = path.stack[path.stack.length - 3] as SyntaxTreeNode;
    if (Array.isArray(result)) {
        result = path.stack[path.stack.length - 5] as SyntaxTreeNode;
    }
    if (typeof result !== "object" || !result.nodeType) {
        throw new Error("The object " + result + " did not appear to be a SyntaxTreeNode");
    }

    return result;
}

export function hasLeadingExtraLine(node: SyntaxTreeNode) {
    if (node.leadingTrivia) {
        for (const trivia of node.leadingTrivia) {
            if (trivia.kind === "SingleLineCommentTrivia") {
                return false;
            }
            if (trivia.kind === "EndOfLineTrivia") {
                return true;
            }
        }
    }

    return false;
}
