import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RefTypeNode extends SyntaxTreeNode<"RefType"> {
    refKeyword: HasValue;
    readOnlyKeyword: HasValue;
    type: SyntaxTreeNode;
}

export const print: PrintMethod<RefTypeNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        printValue(node.refKeyword),
        node.readOnlyKeyword.value ? " " + printValue(node.readOnlyKeyword) : "",
        " ",
        path.call(print, "type"),
    ]);
};
