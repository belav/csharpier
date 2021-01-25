import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printQueryContinuation, QueryContinuationNode } from "./QueryContinuation";

export interface QueryBodyNode extends SyntaxTreeNode<"QueryBody"> {
    clauses: SyntaxTreeNode[];
    selectOrGroup: SyntaxTreeNode;
    continuation?: QueryContinuationNode;
}

export const printQueryBody: PrintMethod<QueryBodyNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(join(line, path.map(print, "clauses")));
    if (node.clauses.length > 0) {
        parts.push(line);
    }
    parts.push(path.call(print, "selectOrGroup"));
    if (node.continuation) {
        // TODO indent when there is a group by before the into?
        parts.push(" ", path.call(o => printQueryContinuation(o, options, print), "continuation"));
    }

    return concat(parts);
};
