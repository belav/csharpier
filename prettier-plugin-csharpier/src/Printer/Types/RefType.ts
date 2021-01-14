import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface RefTypeNode extends SyntaxTreeNode<"RefType"> {

}

export const print: PrintMethod<RefTypeNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node RefType" : "";
};
