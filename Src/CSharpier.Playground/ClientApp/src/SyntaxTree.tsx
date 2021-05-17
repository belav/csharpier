import React from "react";
import ReactJson, { CollapsedFieldProps } from "react-json-view";
import styled from "styled-components";
import { useAppContext } from "./AppContext";

export const SyntaxTree = () => {
    const { syntaxTree } = useAppContext();
    if (!syntaxTree) {
        return null;
    }

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
