// TODO 0 we have some work to do with new lines, we just look for them on modifiers, but then our attributes test is screwy
// For nodes of certain types, we need to look for new lines on properties in order, and if we find them, break.
// IE - are there new lines on attributes? Use those, break. Else are there new lines on modifiers? Use those, break. etc
// TODO 0 can we call this from inside of print?
import { Doc } from "prettier";
import { hardline } from "./Builders";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

export type HasLeadingTrivia = {
    leadingTrivia?: {
        kind: "SingleLineCommentTrivia" | "EndOfLineTrivia"
        commentText?: string;
    }[];
}

export function printComments<T extends SyntaxTreeNode, K extends keyof T>(node: T, parts: Doc[], ...properties: K[]) {
    for (const propertyKey of properties) {
        const property = node[propertyKey] as HasLeadingTrivia;

        const doWork = (value?: HasLeadingTrivia) => {
            if (value?.leadingTrivia) {
                for (const trivia of value.leadingTrivia) {
                    if (trivia.kind === "SingleLineCommentTrivia") {
                        parts.push(trivia.commentText!, hardline);
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
