import { PrintMethod } from "../PrintMethod";
import {
    SyntaxToken,
    printPathSyntaxToken,
    SyntaxTreeNode,
    HasIdentifier,
    printPathIdentifier,
    printIdentifier,
} from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";

export interface NameEqualsNode extends SyntaxTreeNode<"NameEquals"> {
    name: HasIdentifier;
    colonToken: SyntaxToken;
}

export const printNameColon: PrintMethod<NameEqualsNode> = (path, options, print) => {
    const node = path.getValue();
    return concat([printIdentifier(node.name), ": "]);
};
