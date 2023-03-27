import { Controlled as CodeMirror } from "react-codemirror2";
import React from "react";
import { useOptions } from "./Hooks";
import { useAppContext } from "./AppContext";
import { observer } from "mobx-react-lite";

export const FormattedCode = observer(() => {
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
});
