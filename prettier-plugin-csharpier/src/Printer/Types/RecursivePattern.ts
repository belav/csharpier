import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { NameColonNode, printNameColon } from "./NameColon";

export interface RecursivePatternNode extends SyntaxTreeNode<"RecursivePattern"> {
    type?: SyntaxTreeNode;
    positionalPatternClause?: PositionalPatternClauseNode;
    propertyPatternClause?: PropertyPatternClauseNode;
    designation?: SyntaxTreeNode;
}

interface PositionalPatternClauseNode extends SyntaxTreeNode<"PositionalPatternClause"> {
    openParenToken?: SyntaxToken;
    subpatterns: SubpatternNode[];
    closeParenToken?: SyntaxToken;
}

interface PropertyPatternClauseNode extends SyntaxTreeNode<"PropertyPatternClause"> {
    subpatterns: SubpatternNode[];
}

interface SubpatternNode extends SyntaxTreeNode<"Subpattern"> {
    nameColon?: NameColonNode;
    pattern?: SyntaxTreeNode;
}

export const printRecursivePattern: PrintMethod<RecursivePatternNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([
        node.type ? path.call(print, "type") : "",
        node.positionalPatternClause
            ? concat([
                  "(",
                  join(
                      ", ",
                      path.map(
                          subpatternPath => {
                              const subpatternNode = subpatternPath.getValue();
                              return concat([
                                  subpatternNode.nameColon
                                      ? subpatternPath.call(o => printNameColon(o, options, print), "nameColon")
                                      : "",
                                  subpatternPath.call(print, "pattern")
                              ]);
                          },
                          "positionalPatternClause",
                          "subpatterns"
                      )
                  ),
                  ")"
              ])
            : "",
        node.propertyPatternClause
            ? concat([
                  " { ",
                  join(
                      ", ",
                      path.map(
                          subpatternPath => {
                              return concat([
                                  subpatternPath.call(o => printNameColon(o, options, print), "nameColon"),
                                  subpatternPath.call(print, "pattern")
                              ]);
                          },
                          "propertyPatternClause",
                          "subpatterns"
                      )
                  ),
                  " } "
              ])
            : "",
        node.designation ? path.call(print, "designation") : ""
    ]);
};
