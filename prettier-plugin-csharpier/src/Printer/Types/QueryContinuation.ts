import { PrintMethod } from "../PrintMethod";
import { printPathIdentifier, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printQueryBody, QueryBodyNode } from "./QueryBody";

export interface QueryContinuationNode extends SyntaxTreeNode<"QueryContinuation"> {
    intoKeyword?: SyntaxToken;
    identifier: SyntaxToken;
    body?: QueryBodyNode;
}

export const printQueryContinuation: PrintMethod<QueryContinuationNode> = (path, options, print) => {
    return concat([
        "into",
        " ",
        printPathIdentifier(path),
        line,
        path.call(o => printQueryBody(o, options, print), "body")
    ]);
};
