import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printIdentifier, printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { BracketedArgumentListNode, printBracketedArgumentList } from "./BracketedArgumentList";
import { EqualsValueClauseNode, printEqualsValueClause } from "./EqualsValueClause";

export interface VariableDeclaratorNode extends SyntaxTreeNode<"VariableDeclarator"> {
    identifier: SyntaxToken;
    argumentList?: BracketedArgumentListNode;
    initializer?: EqualsValueClauseNode;
}

export const printVariableDeclarator: PrintMethod<VariableDeclaratorNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    parts.push(printIdentifier(node));
    if (node.argumentList) {
        parts.push(path.call(o => printBracketedArgumentList(o, options, print), "argumentList"));
    }
    if (node.initializer) {
        parts.push(path.call(o => printEqualsValueClause(o, options, print), "initializer"));
    }

    return concat(parts);
};
