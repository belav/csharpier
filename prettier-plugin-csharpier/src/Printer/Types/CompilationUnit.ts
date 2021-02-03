import { Doc } from "prettier";
import { printAttributeLists } from "../PrintAttributeLists";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { PrintMethod } from "../PrintMethod";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { AttributeListNode } from "./AttributeList";
import { ExternAliasDirectiveNode, printExternAliasDirective } from "./ExternAliasDirective";
import { printUsingDirective, UsingDirectiveNode } from "./UsingDirective";

export interface CompilationUnitNode extends SyntaxTreeNode<"ompilationUnit"> {
    externs: ExternAliasDirectiveNode[];
    usings: UsingDirectiveNode[];
    attributeLists: AttributeListNode[];
    members: SyntaxTreeNode[];
}

export const printCompilationUnit: PrintMethod<CompilationUnitNode> = (path, options, print) => {
    const node = path.getValue();
    const parts: Doc[] = [];
    if (node.externs.length > 0) {
        parts.push(
            join(
                hardline,
                path.map(innerPath => printExternAliasDirective(innerPath, options, print), "externs")
            ),
            hardline
        );
    }

    if (node.usings.length > 0) {
        parts.push(
            join(
                hardline,
                path.map(innerPath => printUsingDirective(innerPath, options, print), "usings")
            ),
            hardline
        );
    }

    printAttributeLists(node, parts, path, options, print);

    if (node.members.length > 0) {
        parts.push(join(hardline, path.map(print, "members")));
    }

    if (parts[parts.length - 1] !== hardline) {
        parts.push(hardline);
    }

    return concat(parts);
};
