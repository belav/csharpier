import React from "react";
import ReactJson, { CollapsedFieldProps } from "react-json-view";
import "./SyntaxTree.css";
import { useAppContext } from "./AppContext";
import { observer } from "mobx-react-lite";

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

export const SyntaxTree = observer(() => {
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
});
