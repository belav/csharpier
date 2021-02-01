import { Doc } from "prettier";
import { hardline } from "./Builders";
import { HasLeadingTrivia } from "./PrintComments";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

export function printExtraNewLines<T extends SyntaxTreeNode, K extends keyof T>(node: T, parts: Doc[], ...properties: K[]) {
    for (const propertyKey of properties) {
        const property = node[propertyKey] as HasLeadingTrivia;

        let foundNewLine = false;

        const doWork = (value?: HasLeadingTrivia) => {
            if (!value?.leadingTrivia) {
                return;
            }
            for (const trivia of value.leadingTrivia) {
                if (trivia.kind === "EndOfLineTrivia") {
                    foundNewLine = true;
                    parts.push(hardline);
                }
                if (trivia.kind === "SingleLineCommentTrivia") {
                    return;
                }
            }
        }

        if (Array.isArray(property)) {
            for (const actualProperty of property) {
                doWork(actualProperty);
                if (foundNewLine) {
                    return;
                }
            }
        } else {
            doWork(property);
        }

        if (foundNewLine) {
            return;
        }
    }
}
