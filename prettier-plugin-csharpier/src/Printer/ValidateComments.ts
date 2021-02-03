import { SyntaxTreeNode } from "./SyntaxTreeNode";

export function validateComments(node: SyntaxTreeNode) {
    const path = "(" + node.nodeType + ")";
    const missingComments: any[] = [];
    checkComments(node, path, missingComments);

    if (missingComments.length > 0) {
        console.log("The following comments were lost");
        for (const missingComment of missingComments) {
            console.log("    " + missingComment.commentText);
            console.log("    " + missingComment.path);
            console.log();
        }
    }
}

function checkComments(node: any, path: string, missingComments: any[]) {
    for (const key of Object.keys(node)) {
        const property = node[key];
        if (typeof property !== "object") {
            continue;
        }
        if (Array.isArray(property)) {
            for (let x = 0; x < property.length; x++) {
                let newPath = path + "." + key + "[" + x + "]";
                if (property[x].nodeType) {
                    newPath += `(${property[x].nodeType})`;
                }

                checkComments(property[x], newPath, missingComments);
            }
        }

        if (property.nodeType === "SyntaxTrivia" && property.commentText) {
            if (!property.printed) {
                missingComments.push({
                    commentText: property.commentText,
                    path: path
                });
            }
        } else {
            checkComments(property, path + "." + key, missingComments);
        }
    }
}
