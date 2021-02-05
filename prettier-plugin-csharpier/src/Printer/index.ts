import * as fs from "fs";
import { Doc } from "prettier";
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

            // TODO pass as an option, and write out somewhere better. then it'll be easier to compare to c#
            fs.writeFileSync("c:/temp/blah.json", JSON.stringify(result));

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

export function printDocTree(doc: Doc, indent: string): string {
    if (typeof (doc) === "string")
    {
        return indent + "\"" + doc + "\"";
    }

    switch (doc.type)
    {
        case "concat":
            if (doc.parts.length == 2 && typeof doc.parts[1] !== "string" && doc.parts[1].type === "break-parent") {
                return indent + "hardline";
            }

            let result = indent + "concat([\r\n";
            for (let x = 0; x < doc.parts.length; x++)
            {
                result += printDocTree(doc.parts[x], indent + "    ");
                if (x < doc.parts.length - 1) {
                    result += ",\r\n";
                }
            }
            result += "])"
            return result;
        case "line":
            return indent + (doc.hard ? "hardline" : doc.soft ? "softlife" : "line");
        case "break-parent":
            return indent + "breakParent";
        case "indent":
            return indent + "indent(\r\n" + printDocTree(doc.contents, indent + "    ") + ")";
        case "group":
            return indent + "group(\r\n" + printDocTree(doc.contents, indent + "    ") + ")";
        default:
            throw new Error("Can't handle " + doc.type);
    }
}

const defaultExport = {
    print: printNode
};

export default defaultExport;
