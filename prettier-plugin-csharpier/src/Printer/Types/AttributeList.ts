import { Doc } from "prettier";
import { PrintMethod } from "../PrintMethod";
import { HasIdentifier, printPathIdentifier, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printNameColon } from "./NameColon";
import { NameEqualsNode, printNameEquals } from "./NameEquals";

export interface AttributeListNode extends SyntaxTreeNode<"AttributeList"> {
    openBracketToken?: SyntaxToken;
    target?: AttributeTargetSpecifierNode;
    attributes: AttributeNode[];
    closeBracketToken?: SyntaxToken;
}

interface AttributeTargetSpecifierNode extends SyntaxTreeNode<"AttributeTargetSpecifier"> {
    identifier: SyntaxToken;
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
    name?: HasIdentifier;
    colonToken?: SyntaxToken;
}

export const printAttributeList: PrintMethod<AttributeListNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = ["["];

    if (node.target) {
        parts.push(
            path.call(attributeTargetSpecifierPath => printPathIdentifier(attributeTargetSpecifierPath), "target"),
            ": ",
        );
    }

    const attributes = path.map(attributePath => {
        const attributeNode = attributePath.getValue();
        const name = attributePath.call(print, "name");
        if (!attributeNode.argumentList) {
            return name;
        }

        const parts: Doc[] = [name, "("];
        parts.push(
            join(
                ", ",
                attributePath.map(
                    attributeArgumentPath => {
                        const attributeArgumentNode = attributeArgumentPath.getValue();
                        return concat([
                            attributeArgumentNode.nameEquals
                                ? attributeArgumentPath.call(o => printNameEquals(o, options, print), "nameEquals")
                                : "",
                            attributeArgumentNode.nameColon
                                ? attributeArgumentPath.call(o => printNameColon(o, options, print), "nameColon")
                                : "",
                            attributeArgumentPath.call(print, "expression"),
                        ]);
                    },
                    "argumentList",
                    "arguments",
                ),
            ),
        );
        parts.push(")");
        return concat(parts);
    }, "attributes");

    parts.push(join(", ", attributes), "]");
    return concat(parts);
};
