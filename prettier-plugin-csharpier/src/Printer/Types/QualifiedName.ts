import { PrintMethod } from "../PrintMethod";
import { Node } from "../Node";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface QualifiedNameNode extends Node<"QualifiedName"> {

}

export const print: PrintMethod<QualifiedNameNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([path.call(print, "left"), ".", path.call(print, "right")]);
};
