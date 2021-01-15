import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CaseSwitchLabelNode extends SyntaxTreeNode<"CaseSwitchLabel"> {
    keyword: HasValue;
    value: SyntaxTreeNode;
}

export const print: PrintMethod<CaseSwitchLabelNode> = (path, options, print) => {
    return concat([printPathValue(path, "keyword"), " ", path.call(print, "value"), ":"]);
};
