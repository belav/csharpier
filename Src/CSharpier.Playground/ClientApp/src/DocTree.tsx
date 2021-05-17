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

let editor: any;

export const DocTree = () => {
    const { tab, doc } = useAppContext();
    // for whatever reason CodeMirror wasn't displaying anything until it was clicked, which refreshes it.
    // this gets it to refresh when the tab is clicked
    useEffect(() => {
        editor?.refresh();
    }, [tab, doc]);

    return (
        <CodeMirror
            value={doc}
            options={options}
            onBeforeChange={() => {}}
            onChange={() => {}}
            editorDidMount={value => {
                setTimeout(() => {
                    editor = value;
                }, 100);
            }}
        />
    );
};
