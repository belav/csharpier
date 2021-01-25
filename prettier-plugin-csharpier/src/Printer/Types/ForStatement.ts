import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { printVariableDeclaration, VariableDeclarationNode } from "./VariableDeclaration";

export interface ForStatementNode extends SyntaxTreeNode<"ForStatement"> {
    attributeLists: AttributeListNode[];
    forKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    declaration?: VariableDeclarationNode;
    initializers: SyntaxTreeNode[];
    firstSemicolonToken?: SyntaxToken;
    condition?: SyntaxTreeNode;
    secondSemicolonToken?: SyntaxToken;
    incrementors: SyntaxTreeNode[];
    closeParenToken?: SyntaxToken;
    statement?: SyntaxTreeNode;
}

export const printForStatement: PrintMethod<ForStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [printPathSyntaxToken(path, "forKeyword"), " ("];
    if (node.declaration) {
        parts.push(
            path.call(innerPath => printVariableDeclaration(innerPath, options, print), "declaration"),
            "; ",
        );
    } else {
        parts.push(";");
    }
    if (node.condition) {
        parts.push(path.call(print, "condition"), "; ");
    } else {
        parts.push(";");
    }

    parts.push(join(", ", path.map(print, "incrementors")), ")");

    let statement = path.call(print, "statement");
    if (node.statement?.nodeType === "Block") {
        parts.push(statement);
    } else {
        parts.push(indent(concat([hardline, statement])));
    }

    return concat(parts);
};
