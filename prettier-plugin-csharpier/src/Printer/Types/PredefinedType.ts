import { PrintMethod } from "../PrintMethod";
import { printValue, HasValue, SyntaxTreeNode, printPathValue } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface PredefinedTypeNode extends SyntaxTreeNode<"PredefinedType"> {
    keyword: HasValue;
}

export const print: PrintMethod<PredefinedTypeNode> = (path, options, print) => {
    return printPathValue(path, "keyword");
};
