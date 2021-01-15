import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface DefaultSwitchLabelNode extends SyntaxTreeNode<"DefaultSwitchLabel"> {
    keyword: HasValue;
}

export const print: PrintMethod<DefaultSwitchLabelNode> = (path, options, print) => {
    return concat([printPathValue(path, "keyword"), ":"]);
};
