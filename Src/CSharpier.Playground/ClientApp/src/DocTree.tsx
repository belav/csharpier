import React, { useEffect } from "react";
import { UnControlled as CodeMirror } from "react-codemirror2";
import { useAppContext } from "./AppContext";

const options = {
    mode: {
        name: "javascript",
        json: true,
    },
    foldGutter: true,
    gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
    readOnly: true,
};

export const DocTree = () => {
    const { doc } = useAppContext();

    return (
        <CodeMirror
            value={doc}
            options={options}
            onBeforeChange={() => {}}
            onChange={() => {}}
        />
    );
};
