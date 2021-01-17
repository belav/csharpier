import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NameEqualsNode extends SyntaxTreeNode<"NameEquals"> {
    name: SyntaxTreeNode;
    equalsToken: HasValue;
}

export const print: PrintMethod<NameEqualsNode> = (path, options, print) => {
    return concat([path.call(print, "name"), " ", printPathValue(path, "equalsToken")]);
};
