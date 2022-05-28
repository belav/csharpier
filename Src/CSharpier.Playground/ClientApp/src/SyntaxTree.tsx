import React from "react";
import ReactJson, { CollapsedFieldProps } from "react-json-view";
import { useAppContext } from "./AppContext";
import "./SyntaxTree.css";

const shouldCollapse = (field: CollapsedFieldProps) => {
    if (
        field.name === "leadingTrivia" ||
        field.name === "trailingTrivia" ||
        (field.src as any).nodeType === "SyntaxToken"
    ) {
        return true;
    }

    return false;
};

export const SyntaxTree = () => {
    const { syntaxTree } = useAppContext();
    if (!syntaxTree) {
        return null;
    }

    return (
        <div className="reactJson">
            <ReactJson
                src={syntaxTree}
                enableClipboard={false}
                displayDataTypes={false}
                displayObjectSize={false}
                quotesOnKeys={false}
                shouldCollapse={shouldCollapse}
                name={false}
            />
        </div>
    );
};
