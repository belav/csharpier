import { Doc } from "prettier";
import { printAttributeLists } from "../PrintAttributeLists";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { BlockNode, printBlock } from "./Block";
import { CatchClauseNode, printCatchClause } from "./CatchClause";
import { FinallyClauseNode, printFinallyClause } from "./FinallyClause";

export interface TryStatementNode extends SyntaxTreeNode<"TryStatement"> {
    attributeLists: AttributeListNode[];
    tryKeyword?: SyntaxToken;
    block?: BlockNode;
    catches: CatchClauseNode[];
    finally?: FinallyClauseNode;
}

export const printTryStatement: PrintMethod<TryStatementNode> = (path, options, print) => {
    const parts: Doc[] = [];
    const node = path.getValue();
    printAttributeLists(node, parts, path, options, print);
    parts.push(
        "try",
        path.call(o => printBlock(o, options, print), "block"),
        hardline,
        join(hardline, path.map(o => printCatchClause(o, options, print), "catches")),
    );
    if (node.finally) {
        parts.push(hardline, path.call(o => printFinallyClause(o, options, print), "finally"));
    }
    return concat(parts);
};
