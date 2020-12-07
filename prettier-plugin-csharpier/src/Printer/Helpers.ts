import { Doc, FastPath } from "prettier";
import { concat, join, line } from "./Builders";
import { SyntaxTreeNode } from "./SyntaxTreeNode";

export function printCommaList(list: Doc[]) {
    return join(concat([",", line]), list);
}

export function getParentNode(path: FastPath) {
    const result = path.stack[path.stack.length - 3] as SyntaxTreeNode;
    if (typeof result !== "object" || !result.nodeType){
        throw new Error("The object " + result + " did not appear to be a SyntaxTreeNode");
    }

    return result;
}
