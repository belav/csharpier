import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface UsingDirectiveNode extends Node<"UsingDirective"> {
    usingKeyword: {
        text: string;
    }
}

export const print: PrintMethod<UsingDirectiveNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(node.usingKeyword.text);
    parts.push(" ");
    // TODO we should really have something to deal with QualifiedName, IdentifierName, IdentifierToken, etc, this just works with simple examples
    parts.push((node as any).name.identifier.text);
    parts.push(";")
    parts.push(hardline);

    return concat(parts);
};
