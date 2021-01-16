import { Doc, FastPath } from "prettier";
import { concat, group, hardline, indent, join, line } from "./Builders";
import { Print } from "./PrintMethod";
import { HasTrivia, SyntaxTreeNode } from "./SyntaxTreeNode";

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
    let result = path.stack[path.stack.length - 3] as SyntaxTreeNode;
    if (Array.isArray(result)) {
        result = path.stack[path.stack.length - 5] as SyntaxTreeNode;
    }
    if (typeof result !== "object" || !result.nodeType) {
        throw new Error("The object " + result + " did not appear to be a SyntaxTreeNode");
    }

    return result;
}

export function hasLeadingExtraLine(node: HasTrivia) {
    if (node.leadingTrivia) {
        for (const trivia of node.leadingTrivia) {
            if (trivia.nodeType === "EndOfLineTrivia") {
                return true;
            }
        }
    }

    return false;
}

// TODO 0 kill?
export function printExtraLines(parts: Doc[], node: SyntaxTreeNode) {
    let foundStuff = false;

    const nodeAsAny = node as any;

    if (node.leadingTrivia) {
        for (const trivia of node.leadingTrivia) {
            if (trivia.nodeType === "EndOfLineTrivia") {
                parts.push(hardline);
                foundStuff = true;
            }
        }
    }

    if (foundStuff) {
        return;
    }

    if (nodeAsAny["modifiers"]) {
        for (const mightHaveTrivia of nodeAsAny["modifiers"]) {
            if (mightHaveTrivia.leadingTrivia) {
                for (const trivia of mightHaveTrivia.leadingTrivia) {
                    if (trivia.nodeType === "EndOfLineTrivia") {
                        parts.push(hardline);
                        foundStuff = true;
                    }
                }
            }
        }
    }

    if (foundStuff) {
        return;
    }

    if (nodeAsAny["keyword"]) {
        if (nodeAsAny["keyword"].leadingTrivia) {
            for (const trivia of nodeAsAny["keyword"].leadingTrivia) {
                if (trivia.nodeType === "EndOfLineTrivia") {
                    parts.push(hardline);
                    foundStuff = true;
                }
            }
        }
    }
}
