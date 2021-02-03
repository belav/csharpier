import { Dictionary } from "../Common/Types";
import { concat, hardline } from "./Builders";
import { hasLeadingExtraLine } from "./Helpers";
import { PrintMethod } from "./PrintMethod";
import * as types from "./Types";
import { validateComments } from "./ValidateComments";

let foundFirst = false;
const missingNodes: string[] = [];

const printNode: PrintMethod = (path, options, print) => {
    const node = path.getValue();

    const thisIsFirst = !foundFirst;
    foundFirst = true;

    const theTypes = (types as any) as Dictionary<PrintMethod>;
    if (!node) {
        console.warn(
            "There was no nodeType of " +
                path.stack[path.stack.length - 2] +
                " on the node of nodeType " +
                path.stack[path.stack.length - 3].nodeType
        );

        return "";
    }

    const thePrint = theTypes[node.nodeType];

    if (thePrint) {
        const result = thePrint(path, options, print);
        if (thisIsFirst) {
            if ((options as any).validateComments) {
                validateComments(path.getValue());
            }

            if (missingNodes.length > 0) {
                throw new Error(
                    `Unknown C# nodes, run the following commands:\nplop node ${missingNodes.join("\nplop node ")}`
                );
            }
        }

        if (typeof result === "undefined") {
            throw new Error("undefined was by the print call for the nodeType of " + node.nodeType);
        }

        return hasLeadingExtraLine(node) ? concat([hardline, result]) : result;
    }

    if (!missingNodes.find(o => o === node.nodeType)) {
        missingNodes.push(node.nodeType);
    }

    return "";
};

const defaultExport = {
    print: printNode
};

export default defaultExport;
