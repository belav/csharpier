import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { WhenClauseNode } from "./WhenClause";

export interface SwitchExpressionNode extends SyntaxTreeNode<"SwitchExpression"> {
    governingExpression?: SyntaxTreeNode;
    switchKeyword?: SyntaxToken;
    arms: SwitchExpressionArmNode[];
}

interface SwitchExpressionArmNode extends SyntaxTreeNode<"SwitchExpressionArm"> {
    pattern?: SyntaxTreeNode;
    whenClause?: WhenClauseNode;
    equalsGreaterThanToken?: SyntaxToken;
    expression?: SyntaxTreeNode;
}

export const printSwitchExpression: PrintMethod<SwitchExpressionNode> = (path, options, print) => {
    return concat([
        path.call(print, "governingExpression"),
        " switch",
        hardline,
        "{",
        indent(
            concat([
                hardline,
                join(
                    concat([",", hardline]),
                    path.map(switchExpressionArmPath => {
                        return concat([
                            switchExpressionArmPath.call(print, "pattern"),
                            " => ",
                            switchExpressionArmPath.call(print, "expression")
                        ]);
                    }, "arms")
                )
            ])
        ),
        hardline,
        "}"
    ]);
};
