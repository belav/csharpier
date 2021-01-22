import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, printSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SwitchStatementNode extends SyntaxTreeNode<"SwitchStatement"> {
    switchKeyword: SyntaxToken;
    expression: SyntaxTreeNode;
    sections: SyntaxTreeNode[];
}

export const printSwitchStatement: PrintMethod<SwitchStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const sections =
        node.sections.length === 0
            ? " { }"
            : concat([
                  hardline,
                  "{",
                  indent(concat([hardline, join(hardline, path.map(print, "sections"))])),
                  hardline,
                  "}",
              ]);
    return concat([printSyntaxToken(node.switchKeyword), " (", path.call(print, "expression"), ")", sections]);
};
