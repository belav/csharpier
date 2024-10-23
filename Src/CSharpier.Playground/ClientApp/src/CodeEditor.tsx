import React, { useEffect } from "react";
import { Controlled as CodeMirror } from "react-codemirror2";
import "codemirror/lib/codemirror.css";
import "codemirror/mode/clike/clike";
import { useOptions } from "./Hooks";
import { useAppContext } from "./AppContext";
import { observer } from "mobx-react-lite";

export const CodeEditor = observer(() => {
    const { formatCode, enteredCode, setEnteredCode } = useAppContext();
    useEffect(() => {
        formatCode();
    }, []);

    const options = useOptions();

    return (
        <CodeMirror
            value={enteredCode}
            options={options}
            onBeforeChange={(editor, data, value) => {
                setEnteredCode(value);
            }}
            onChange={() => {}}
        />
    );
});
