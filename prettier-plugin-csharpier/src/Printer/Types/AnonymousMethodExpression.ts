import { PrintMethod } from "../PrintMethod";
import { HasModifiers, SyntaxToken, printPathSyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AnonymousMethodExpressionNode extends SyntaxTreeNode<"AnonymousMethodExpression">, HasModifiers {
    delegateKeyword: SyntaxToken;
    parameterList: SyntaxTreeNode;
    block: SyntaxTreeNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword: SyntaxToken;
    body: SyntaxTreeNode;
}

export const printAnonymousMethodExpression: PrintMethod<AnonymousMethodExpressionNode> = (path, options, print) => {
    return concat([
        printPathSyntaxToken(path, "delegateKeyword"),
        path.call(print, "parameterList"),
        path.call(print, "body"),
    ]);
};
