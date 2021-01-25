import { Doc } from "prettier";
import { printAttributeLists } from "../PrintAttributeLists";
import { PrintMethod } from "../PrintMethod";
import { printModifiers, printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { ArrowExpressionClauseNode } from "./ArrowExpressionClause";
import { AttributeListNode } from "./AttributeList";
import { BlockNode } from "./Block";

// TODO 1 go through each node, copy interface from the generated one, figure out which path.calls can be optimized to this version
// also for some of the methods outside of the Types folder.
export interface AccessorDeclarationNode extends SyntaxTreeNode<"AccessorDeclaration"> {
    attributeLists: AttributeListNode[];
    modifiers: SyntaxToken[];
    keyword?: SyntaxToken;
    body?: BlockNode;
    expressionBody?: ArrowExpressionClauseNode;
}

export const printAccessorDeclaration: PrintMethod<AccessorDeclarationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.modifiers.length > 0 || node.attributeLists.length > 0 || node.body || node.expressionBody) {
        parts.push(hardline);
    } else {
        parts.push(line);
    }

    printAttributeLists(node, parts, path, options, print);
    parts.push(printModifiers(node));
    parts.push(printSyntaxToken(node.keyword));
    if (!node.body && !node.expressionBody) {
        parts.push(";");
    } else {
        if (node.body) {
            parts.push(path.call(print, "body"));
        } else {
            parts.push(" ");
            parts.push(path.call(print, "expressionBody"));
            parts.push(";");
        }
    }

    return concat(parts);
};
