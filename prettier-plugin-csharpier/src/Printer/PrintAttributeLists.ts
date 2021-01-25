import { Doc, ParserOptions } from "prettier";
import { hardline, join, line } from "./Builders";
import { FastPath, Print } from "./PrintMethod";
import { SyntaxTreeNode } from "./SyntaxTreeNode";
import { AttributeListNode, printAttributeList } from "./Types/AttributeList";

interface HasAttributeLists extends SyntaxTreeNode {
    attributeLists: AttributeListNode[];
}

// TODO what else has attributeLists??
export function printAttributeLists(
    node: HasAttributeLists,
    parts: Doc[],
    path: FastPath,
    options: ParserOptions,
    print: Print,
) {
    if (node.attributeLists.length === 0) {
        return;
    }
    const seperator = node.nodeType === "TypeParameter" || node.nodeType === "Parameter" ? line : hardline;
    parts.push(
        join(
            seperator,
            path.map(innerPath => printAttributeList(innerPath, options, print), "attributeLists"),
        ),
    );

    if (node.nodeType !== "Parameter") {
        parts.push(seperator);
    }
}
