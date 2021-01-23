import { PrintMethod } from "../PrintMethod";
import { SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { NameEqualsNode } from "./NameEquals";

export interface AttributeListNode extends SyntaxTreeNode<"AttributeList"> {
    openBracketToken?: SyntaxToken;
    target?: AttributeTargetSpecifierNode;
    attributes: AttributeNode[];
    closeBracketToken?: SyntaxToken;
}

interface AttributeTargetSpecifierNode extends SyntaxTreeNode<"AttributeTargetSpecifier"> {
    identifier?: SyntaxToken;
    colonToken?: SyntaxToken;
}

interface AttributeNode extends SyntaxTreeNode<"Attribute"> {
    name?: SyntaxTreeNode;
    argumentList?: AttributeArgumentListNode;
}

interface AttributeArgumentListNode extends SyntaxTreeNode<"AttributeArgumentList"> {
    openParenToken?: SyntaxToken;
    arguments: AttributeArgumentNode[];
    closeParenToken?: SyntaxToken;
}

interface AttributeArgumentNode extends SyntaxTreeNode<"AttributeArgument"> {
    nameEquals?: NameEqualsNode;
    nameColon?: NameColonNode;
    expression?: SyntaxTreeNode;
}

interface NameColonNode extends SyntaxTreeNode<"NameColon"> {
    name?: IdentifierNameNode;
    colonToken?: SyntaxToken;
}

interface IdentifierNameNode extends SyntaxTreeNode<"IdentifierName"> {
    identifier?: SyntaxToken;
    arity?: number;
    isVar?: boolean;
    isUnmanaged?: boolean;
    isNotNull?: boolean;
    isNint?: boolean;
    isNuint?: boolean;
}

export const printAttributeList: PrintMethod<AttributeListNode> = (path, options, print) => {
    return (options as any).printTodo ? "TODO Node AttributeList" : "";
};
