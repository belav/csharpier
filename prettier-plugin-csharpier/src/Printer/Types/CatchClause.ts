import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printPathSyntaxToken, SyntaxTreeNode, HasIdentifier, printIdentifier } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { BlockNode, printBlock } from "./Block";

export interface CatchClauseNode extends SyntaxTreeNode<"CatchClause"> {
    catchKeyword?: SyntaxToken;
    declaration?: CatchDeclarationNode;
    filter?: CatchFilterClauseNode;
    block?: BlockNode;
}

interface CatchDeclarationNode extends SyntaxTreeNode<"CatchDeclaration"> {
    openParenToken?: SyntaxToken;
    type?: SyntaxTreeNode;
    identifier: SyntaxToken;
    closeParenToken?: SyntaxToken;
}

interface CatchFilterClauseNode extends SyntaxTreeNode<"CatchFilterClause"> {
    whenKeyword?: SyntaxToken;
    openParenToken?: SyntaxToken;
    filterExpression?: SyntaxTreeNode;
    closeParenToken?: SyntaxToken;
}

export const printCatchClause: PrintMethod<CatchClauseNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        "catch",
        node.declaration
            ? path.call(catchDeclarationPath => {
                  const catchDeclarationNode = catchDeclarationPath.getValue();
                  return concat([
                      " (",
                      catchDeclarationPath.call(print, "type"),
                      catchDeclarationNode.identifier ? " " : "",
                      printIdentifier(catchDeclarationNode),
                      ")"
                  ]);
              }, "declaration")
            : "",
        node.filter
            ? path.call(catchFilterClausePath => {
                  return concat([" when (", catchFilterClausePath.call(print, "filterExpression"), ")"]);
              }, "filter")
            : "",
        path.call(o => printBlock(o, options, print), "block")
    ]);
};
