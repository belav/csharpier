import { Doc } from "prettier";
import { printLeadingComments, printTrailingComments } from "../PrintComments";
import { printExtraNewLines } from "../PrintExtraNewLines";
import { PrintMethod } from "../PrintMethod";
import { HasModifiers, printModifiers, SyntaxToken, SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { printVariableDeclaration, VariableDeclarationNode } from "./VariableDeclaration";

export interface LocalDeclarationStatementNode extends SyntaxTreeNode<"LocalDeclarationStatement"> {
    awaitKeyword?: SyntaxToken;
    usingKeyword?: SyntaxToken;
    modifiers: SyntaxToken[];
    declaration?: VariableDeclarationNode;
    isConst?: boolean;
}

export const printLocalDeclarationStatement: PrintMethod<LocalDeclarationStatementNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    // TODO 0 this is SLOW, see https://github.com/prettier/prettier/issues/4459 for async progress
    // also just figure out what it is doing that is so slow? Could the parser cache results? Could this cache the results of calling the parser?
    // should I redo this to get rid of some of the abstractions that make my life easier to speed up things?

    // TODO 0 should we make methods for things like declaration? so we don't have to put all these crazy things here
    // in our main print, we look up something fot he node and call
    // node.printLeadingComments, node.print, node.printTrailingComments
    // TODO 0 the other option is to do it more like prettier-java, we just look for comments on everything, and prepend/append them and let prettier format it with the comments where they used to be
    // TODO 0 test that out next! make the print lines/comments stuff do nothing, and see if I can get the tests passing with more generic stuff.
    // TODO 0 the visit pattern https://github.com/prettier/prettier/issues/5747
    // looks like it depends on a class from the parser, I'm not sure if I could do it exactly, plus I do need access to the parent in at least one place
    printExtraNewLines(node, parts, "awaitKeyword", "usingKeyword", "modifiers", ["declaration", "type", "keyword"], ["declaration", "type", "identifier"]);
    printLeadingComments(node, parts, "awaitKeyword", "usingKeyword", "modifiers", ["declaration", "type", "keyword"], ["declaration", "type", "identifier"]);
    if (node.awaitKeyword) {
        parts.push("await ");
    }
    if (node.usingKeyword) {
        parts.push("using ");
    }
    parts.push(printModifiers(node));
    parts.push(path.call(o => printVariableDeclaration(o, options, print), "declaration"));
    parts.push(";");
    printTrailingComments(node, parts, "semicolonToken");
    return concat(parts);
};
