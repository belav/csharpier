import { Doc } from "prettier";
import { printAttributeLists } from "../PrintAttributeLists";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, printPathIdentifier, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { printBlock } from "./Block";

export interface LabeledStatementNode extends SyntaxTreeNode<"LabeledStatement"> {
    attributeLists: AttributeListNode[];
    identifier: SyntaxToken;
    colonToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

export const printLabeledStatement: PrintMethod<LabeledStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    printAttributeLists(node, parts, path, options, print);
    parts.push(printIdentifier(node), ":");
    if (node.statement!.nodeType === "Block") {
        parts.push(path.call(o => printBlock(o, options, print), "statement"));
    } else {
        parts.push(hardline, path.call(print, "statement"));
    }
    return concat(parts);
};
