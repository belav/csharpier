import { Doc } from "prettier";
import { hardline } from "./Builders";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

// TODO 0 we have some work to do with new lines, we just look for them on modifiers, but then our attributes test is screwy
// For nodes of certain types, we need to look for new lines on properties in order, and if we find them, break.
// IE - are there new lines on attributes? Use those, break. Else are there new lines on modifiers? Use those, break. etc
// TODO 0 can we call this from inside of print?
export function printComments(parts: Doc[], node: SyntaxTreeNode) {
    for (const key in node) {
        const asAny = node as any;
        if (asAny[key]?.leadingTrivia) {
            for (const trivia of asAny[key].leadingTrivia) {
                if (trivia.nodeType === "SingleLineCommentTrivia") {
                    parts.push((trivia as any).commentText, hardline);
                }
            }
        }
    }
}
