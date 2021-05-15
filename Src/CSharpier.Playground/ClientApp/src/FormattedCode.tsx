import { Controlled as CodeMirror } from "react-codemirror2";
import React from "react";
import { useAppContext } from "./AppContext";
import { useOptions } from "./Hooks";

export const FormattedCode = () => {
    const { formattedCode, setFormattedCodeEditor } = useAppContext();
    const options = useOptions();

    return (
        <CodeMirror
            value={formattedCode}
            options={{ ...options, readOnly: true }}
            editorDidMount={editor => {
                setTimeout(() => {
                    setFormattedCodeEditor(editor);
                }, 100);
            }}
            onBeforeChange={() => {}}
            onChange={() => {}}
        />
    );
};
