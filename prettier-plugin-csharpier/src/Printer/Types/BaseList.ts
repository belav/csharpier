import { PrintMethod } from "../PrintMethod";
import { SyntaxTreeNode } from "../SyntaxTreeNode";
import { concat, group, hardline, indent, join, softline, line, doubleHardline } from "../Builders";
import { Doc } from "prettier";
import { printCommaList } from "../Helpers";

export interface BaseListNode extends SyntaxTreeNode<"BaseList"> {}

export const printBaseList: PrintMethod<BaseListNode> = (path, options, print) => {
    /* TODO this should format like this instead
public class ThisIsSomeLongNameAndItShouldFormatWell
    : AnotherLongClassName<T>,
        AndYetAnotherLongClassName
{
    void MethodName() { }
}
    */
    return group(indent(concat([line, ":", " ", printCommaList(path.map(print, "types"))])));
};
