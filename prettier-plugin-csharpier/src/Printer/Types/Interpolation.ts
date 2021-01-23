import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { printSyntaxToken, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface InterpolationNode extends SyntaxTreeNode<"Interpolation"> {
    expression?: SyntaxTreeNode;
    alignmentClause?: InterpolationAlignmentClauseNode;
    formatClause?: InterpolationFormatClauseNode;
}

interface InterpolationAlignmentClauseNode extends SyntaxTreeNode<"InterpolationAlignmentClause"> {
    commaToken?: SyntaxToken;
    value?: SyntaxTreeNode;
}

interface InterpolationFormatClauseNode extends SyntaxTreeNode<"InterpolationFormatClause"> {
    colonToken?: SyntaxToken;
    formatStringToken?: SyntaxToken;
}

export const printInterpolation: PrintMethod<InterpolationNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = ["{", path.call(print, "expression")];
    if (node.alignmentClause) {
        parts.push(", ", path.call(print, "alignmentClause", "value"));
    }
    if (node.formatClause) {
        parts.push(":", printSyntaxToken(node.formatClause.formatStringToken));
    }
    parts.push("}");
    return concat(parts);
};
