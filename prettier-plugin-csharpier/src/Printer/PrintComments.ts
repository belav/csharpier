// TODO 0 we have some work to do with new lines, we just look for them on modifiers, but then our attributes test is screwy
// For nodes of certain types, we need to look for new lines on properties in order, and if we find them, break.
// IE - are there new lines on attributes? Use those, break. Else are there new lines on modifiers? Use those, break. etc
// TODO 0 can we call this from inside of print?
import { Doc } from "prettier";
import { hardline } from "./Builders";

export type HasTrivia = {
    leadingTrivia?: {
        kind: "SingleLineCommentTrivia" | "EndOfLineTrivia" | "WhitespaceTrivia"
        commentText?: string;
    }[];
    trailingTrivia?: {
        kind: "SingleLineCommentTrivia" | "WhitespaceTrivia"
        commentText?: string;
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
                    if (trivia.kind === "SingleLineCommentTrivia") {
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
