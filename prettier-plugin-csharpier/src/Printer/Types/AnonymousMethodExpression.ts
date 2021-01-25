import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasModifiers, SyntaxToken, printPathSyntaxToken, SyntaxTreeNode, printModifiers } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { BlockNode, printBlock } from "./Block";
import { ParameterListNode, printParameterList } from "./ParameterList";

export interface AnonymousMethodExpressionNode extends SyntaxTreeNode<"AnonymousMethodExpression"> {
    // modifiers: SyntaxToken[]; - these appear to duplicate async
    delegateKeyword?: SyntaxToken;
    parameterList?: ParameterListNode;
    block?: BlockNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword?: SyntaxToken;
    // body?: SyntaxTreeNode; - appears to duplicate block
}

export const printAnonymousMethodExpression: PrintMethod<AnonymousMethodExpressionNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.asyncKeyword) {
        parts.push("async ");
    }
    if (node.delegateKeyword) {
        parts.push("delegate");
    }
    if (node.parameterList) {
        parts.push(path.call(o => printParameterList(o, options, print), "parameterList"));
    }
    if (node.expressionBody) {
        parts.push(path.call(print, "expressionBody"));
    } else if (node.block) {
        parts.push(path.call(o => printBlock(o, options, print), "block"));
    }

    return concat(parts);
};
