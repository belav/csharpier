import { Doc } from "prettier";
import { hardline } from "./Builders";
import { getTriviaProperty, HasTrivia } from "./PrintComments";

export function printExtraNewLines(node: any, parts: Doc[], ...properties: any[]) {
    for (const propertyKey of properties) {
        const property = getTriviaProperty(node, propertyKey);

        let foundNewLine = false;

        const doWork = (value?: HasTrivia) => {
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
        };

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
