import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface OmittedTypeArgumentNode extends SyntaxTreeNode<"OmittedTypeArgument"> {

}

export const print: PrintMethod<OmittedTypeArgumentNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node OmittedTypeArgument" : "";
};
