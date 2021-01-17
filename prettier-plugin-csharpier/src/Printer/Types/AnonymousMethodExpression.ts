import { PrintMethod } from "../PrintMethod";
import { HasModifiers, HasValue, printPathValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface AnonymousMethodExpressionNode extends SyntaxTreeNode<"AnonymousMethodExpression">, HasModifiers {
    delegateKeyword: HasValue;
    parameterList: SyntaxTreeNode;
    block: SyntaxTreeNode;
    expressionBody?: SyntaxTreeNode;
    asyncKeyword: HasValue;
    body: SyntaxTreeNode;
}

export const print: PrintMethod<AnonymousMethodExpressionNode> = (path, options, print) => {
    return concat([
        printPathValue(path, "delegateKeyword"),
        path.call(print, "parameterList"),
        path.call(print, "body"),
    ]);
};
