import { Dictionary } from "../Common/Types";
import { PrintMethod } from "./PrintMethod";
import * as types from "./Types";

const printNode: PrintMethod = (path, options, print) => {
    const node = path.getValue();

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
        return thePrint(path, options, print);
    }

    throw new Error(`Unknown C# node: ${node.nodeType}`);
};

const defaultExport = {
    print: printNode,
};

export default defaultExport;
