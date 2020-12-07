import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UsingDirectiveNode extends SyntaxTreeNode<"UsingDirective"> {
    usingKeyword: {
        text: string;
    };
}

export const print: PrintMethod<UsingDirectiveNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(node.usingKeyword.text);
    parts.push(" ");
    parts.push(group(path.call(print, "name")));
    parts.push(";");
    parts.push(hardline);

    return concat(parts);
};
