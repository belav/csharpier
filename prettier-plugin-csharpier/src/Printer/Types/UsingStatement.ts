import { Doc, FastPath, ParserOptions } from "prettier";
import { Print, PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { printVariableDeclaration, VariableDeclarationNode } from "./VariableDeclaration";

export interface UsingStatementNode extends SyntaxTreeNode<"UsingStatement"> {
    attributeLists: AttributeListNode[];
    awaitKeyword?: SyntaxToken;
    usingKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    declaration?: VariableDeclarationNode;
    expression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

export const printUsingStatement: PrintMethod<UsingStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [
        printPathSyntaxToken(path, "usingKeyword"),
        " (",
        node.declaration ? path.call(o => printVariableDeclaration(o, options, print), "declaration") : "",
        node.expression ? path.call(print, "expression") : "",
        ")"
    ];

    let statement = path.call(print, "statement");
    if (node.statement?.nodeType === "UsingStatement") {
        parts.push(hardline, statement);
    } else if (node.statement?.nodeType === "Block") {
        parts.push(statement);
    } else {
        parts.push(indent(concat([hardline, statement])));
    }

    return concat(parts);
};
