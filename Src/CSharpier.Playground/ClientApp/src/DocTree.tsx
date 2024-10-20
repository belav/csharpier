import React from "react";
import { UnControlled as CodeMirror } from "react-codemirror2";
import { useAppContext } from "./AppContext";
import { observer } from "mobx-react-lite";

const options = {
    mode: {
        name: "javascript",
        json: true,
    },
    foldGutter: true,
    gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
    readOnly: true,
};

const regex = /\s+Doc\.Null,/g;

export const DocTree = observer(() => {
    const { doc, hideNull } = useAppContext();
    const docToDisplay = hideNull ? doc.replaceAll(regex, "") : doc;

    return <CodeMirror value={docToDisplay} options={options} onBeforeChange={() => {}} onChange={() => {}} />;
});
