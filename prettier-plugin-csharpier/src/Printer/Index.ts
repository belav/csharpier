import { Dictionary } from "../Common/Types";
import { PrintMethod } from "./PrintMethod";
import * as types from "./Types";

let foundFirst = false;
const missingNodes: string[] = [];

// TODO watch doesn't work in here, but we don't often change it
const printNode: PrintMethod = (path, options, print) => {
    const node = path.getValue();

    const thisIsFirst = !foundFirst;
    foundFirst = true;

    const theTypes = (types as any) as Dictionary<PrintMethod>;
    if (!node) {
        throw new Error(
            "There was no nodeType of " +
                path.stack[path.stack.length - 2] +
                " on the node of nodeType " +
                path.stack[path.stack.length - 3].nodeType,
        );
    }

    const thePrint = theTypes[node.nodeType];

    if (thePrint) {
        const result = thePrint(path, options, print);
        if (thisIsFirst) {
            if (missingNodes.length > 0) {
                throw new Error(
                    `Unknown C# nodes, run the following commands:\nplop node ${missingNodes.join("\nplop node ")}`,
                );
            }
        }

        if (typeof result === "undefined") {
            throw new Error("undefined was returned for the " + JSON.stringify(node, null, "    "));
        }

        return result;
    }

    if (!missingNodes.find(o => o === node.nodeType)) {
        missingNodes.push(node.nodeType);
    }

    return "";
};

const defaultExport = {
    print: printNode,
};

export default defaultExport;
