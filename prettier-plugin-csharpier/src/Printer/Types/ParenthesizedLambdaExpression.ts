import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { BlockNode, printBlock } from "./Block";
import { ParameterListNode, printParameterList } from "./ParameterList";

export interface ParenthesizedLambdaExpressionNode extends SyntaxTreeNode<"ParenthesizedLambdaExpression"> {
    // modifiers: SyntaxToken[]; appears to duplicate async
    parameterList?: ParameterListNode;
    arrowToken?: SyntaxToken;
    block?: BlockNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword?: SyntaxToken;
    // body?: SyntaxTreeNode; appears to duplicate block
}

export const printParenthesizedLambdaExpression: PrintMethod<ParenthesizedLambdaExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.asyncKeyword) {
        parts.push("async ");
    }
    parts.push(path.call(o => printParameterList(o, options, print), "parameterList"), " => ");
    if (node.expressionBody) {
        parts.push(path.call(print, "expressionBody"));
    } else if (node.block) {
        parts.push(path.call(o => printBlock(o, options, print), "block"));
    }

    return concat(parts);
};
