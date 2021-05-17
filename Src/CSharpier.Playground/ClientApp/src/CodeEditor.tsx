import React, { useEffect } from "react";
import { Controlled as CodeMirror } from "react-codemirror2";
import "codemirror/lib/codemirror.css";
import "codemirror/mode/clike/clike";
import { useAppContext } from "./AppContext";
import { useOptions } from "./Hooks";

export const CodeEditor = () => {
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
};
