import { PrintMethod } from "../PrintMethod";
import { HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface CatchClauseNode extends SyntaxTreeNode<"CatchClause"> {
    catchKeyword: HasValue;
    declaration: SyntaxTreeNode;
    block: SyntaxTreeNode;
}

export const print: PrintMethod<CatchClauseNode> = (path, options, print) => {
    return concat([printPathValue(path, "catchKeyword"), " ", path.call(print, "declaration"), path.call(print, "block")]);
};
