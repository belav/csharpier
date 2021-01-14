import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CasePatternSwitchLabelNode extends SyntaxTreeNode<"CasePatternSwitchLabel"> {

}

export const print: PrintMethod<CasePatternSwitchLabelNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node CasePatternSwitchLabel" : "";
};
