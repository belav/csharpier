import { PrintMethod } from "../PrintMethod";
import { HasValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface SwitchStatementNode extends SyntaxTreeNode<"SwitchStatement"> {
    switchKeyword: HasValue;
    expression: SyntaxTreeNode;
    sections: SyntaxTreeNode[];
}

export const print: PrintMethod<SwitchStatementNode> = (path, options, print) => {
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
    return concat([printValue(node.switchKeyword), " (", path.call(print, "expression"), ")", sections]);
};
