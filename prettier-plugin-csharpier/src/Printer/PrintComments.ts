
import { Doc } from "prettier";
import { hardline } from "./Builders";

export type HasTrivia = {
    leadingTrivia?: {
        kind: "SingleLineCommentTrivia" | "MultiLineCommentTrivia" | "EndOfLineTrivia" | "WhitespaceTrivia"
        commentText?: string;
        printed?: boolean;
    }[];
    trailingTrivia?: {
        kind: "SingleLineCommentTrivia" | "MultiLineCommentTrivia" | "WhitespaceTrivia"
        commentText?: string;
        printed?: boolean;
    }[];
}

export function getTriviaProperty(node: any, propertyKey: any) {
    let property: any = node;
    if (Array.isArray(propertyKey)) {
        for (const subPropertyKey of propertyKey) {
            property = property[subPropertyKey];
            if (!property) {
                return;
            }
        }
    } else {
        property = node[propertyKey];
    }

    return property;
}

export function printLeadingComments(node: any, parts: Doc[], ...properties: any[]) {
    printComments(node, parts, true, properties);
}

export function printTrailingComments(node: any, parts: Doc[], ...properties: any[]) {
    printComments(node, parts, false, properties);
}

function printComments(node: any, parts: Doc[], isLeading: boolean, properties: any[]) {
    for (const propertyKey of properties) {
        const property = getTriviaProperty(node, propertyKey);
        if (!property) {
            continue;
        }

        const doWork = (value?: HasTrivia) => {
            const triviaArray = isLeading ? value?.leadingTrivia : value?.trailingTrivia;
            if (triviaArray) {
                for (const trivia of triviaArray) {
                    if (trivia.kind === "SingleLineCommentTrivia" || trivia.kind === "MultiLineCommentTrivia") {
                        trivia.printed = true;
                        if (!isLeading) {
                            parts.push(" ");
                        }
                        parts.push(trivia.commentText!);
                        if (isLeading) {
                            parts.push(hardline);
                        }
                    }
                }
            }
        }

        if (Array.isArray(property)) {
            property.forEach(o => doWork(o));
        } else {
            doWork(property);
        }
    }
}
