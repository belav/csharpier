import { Dictionary } from "../Common/Types";
import { PrintMethod } from "./PrintMethod";
import * as types from "./types";

const printNode: PrintMethod = (path, options, print) => {
    const node = path.getValue();

    const theTypes = types as any as Dictionary<PrintMethod>;
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
