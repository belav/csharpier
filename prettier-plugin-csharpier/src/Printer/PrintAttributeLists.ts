import { Doc, ParserOptions } from "prettier";
import { hardline, join } from "./Builders";
import { FastPath, Print } from "./PrintMethod";
import { AttributeListNode, printAttributeList } from "./Types/AttributeList";

interface HasAttributeLists {
    attributeLists: AttributeListNode[];
}

// TODO what else has attributeLists??
export function printAttributeLists(node: HasAttributeLists, parts: Doc[], path: FastPath, options: ParserOptions, print: Print) {
    parts.push(
        join(
            hardline,
            path.map(innerPath => printAttributeList(innerPath, options, print), "attributeLists"),
        ),
    );
    if (node.attributeLists.length > 0) {
        parts.push(hardline);
    }
}
