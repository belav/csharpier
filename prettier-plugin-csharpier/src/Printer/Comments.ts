import { Doc } from "prettier";
import { hardline } from "./Builders";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

// TODO 0 also compilation unit doesn't put lines after things, and we kind of ignore the line breaks in the original file
// TODO 0 can we call this from inside of print?
// TODO 0 and if we figure out comments/line breaks, move on to all the missing node types
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
