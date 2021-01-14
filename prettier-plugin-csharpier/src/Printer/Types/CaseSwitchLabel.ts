import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CaseSwitchLabelNode extends SyntaxTreeNode<"CaseSwitchLabel"> {
    keyword: HasValue;
    value: SyntaxTreeNode;
}

export const print: PrintMethod<CaseSwitchLabelNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([printValue(node.keyword), " ", path.call(print, "value"), ":"]);
};
