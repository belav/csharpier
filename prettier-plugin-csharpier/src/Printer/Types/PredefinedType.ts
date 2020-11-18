import { PrintMethod } from "../PrintMethod";
import { getValue, HasValue, Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PredefinedTypeNode extends Node<"PredefinedType"> {
    keyword: HasValue;
}

export const print: PrintMethod<PredefinedTypeNode> = (path, options, print) => {
    const node = path.getValue();
    return getValue(node.keyword);
};
