import { PrintMethod } from "../PrintMethod";
import { getValue, HasValue, Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface IdentifierNameNode extends Node<"IdentifierName"> {
    identifier: HasValue;
}

export const print: PrintMethod<IdentifierNameNode> = (path, options, print) => {
    return getValue(path.getValue().identifier);
};
