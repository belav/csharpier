import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, HasValue, printPathValue, printValue, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { IdentifierNameNode } from "./IdentifierName";

export interface MemberBindingExpressionNode extends SyntaxTreeNode<"MemberBindingExpression"> {
    operatorToken: HasValue;
    name: IdentifierNameNode;
}

export const print: PrintMethod<MemberBindingExpressionNode> = (path, options, print) => {
    return concat([printPathValue(path, "operatorToken"), path.call(print, "name")]);
};
