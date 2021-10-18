import React from "react";
import ReactJson, { CollapsedFieldProps } from "react-json-view";
import styled from "styled-components";
import { useAppContext } from "./AppContext";

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
        <ReactJsonStyle>
            <ReactJson
                src={syntaxTree}
                enableClipboard={false}
                displayDataTypes={false}
                displayObjectSize={false}
                quotesOnKeys={false}
                shouldCollapse={shouldCollapse}
                name={false}
            />
        </ReactJsonStyle>
    );
};

const ReactJsonStyle = styled.div`
    height: 100%;
    overflow-y: scroll;
    border-left: 1px solid #ccc;
`;
