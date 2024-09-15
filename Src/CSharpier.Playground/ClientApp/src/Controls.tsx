import React from "react";
import "./Controls.css";
import { observer } from "mobx-react-lite";
import { useAppContext } from "./AppContext";

// TODO review this, looks like I was changing it to use mobx
// https://github.com/belav/csharpier/pull/858/files#diff-fe7eb3d9ce08803cc37869645e835dda3ecb5cf2e87991e6b73a7a246091f37b
export const Controls = observer(() => {
    const {
        printWidth,
        setPrintWidth,
        indentSize,
        setIndentSize,
        useTabs,
        setUseTabs,
        parser,
        setParser,
        showDoc,
        setShowDoc,
        hideNull,
        setHideNull,
        showAst,
        setShowAst,
        setEmptyMethod,
        setEmptyClass,
        copyLeft,
    } = useAppContext();
    return (
        <div className="controlsWrapper">
            <div>
                <h3>Options</h3>
                <label>Print Width</label>
                <input type="number" value={printWidth} onChange={e => setPrintWidth(parseInt(e.target.value))} />
                <label>Indent Size</label>
                <input type="number" value={indentSize} onChange={e => setIndentSize(parseInt(e.target.value))} />
                <label>
                    <input
                        type="checkbox"
                        checked={useTabs}
                        onChange={() => {
                            setUseTabs(!useTabs);
                        }}
                    />
                    Use Tabs
                </label>
                <label>Parser</label>
                <select value={parser} onChange={e => setParser(e.target.value)}>
                    <option value="CSharp">C#</option>
                    <option value="CSharpScript">C# Script</option>
                    <option value="XML">XML</option>
                </select>
            </div>
            <div>
                <h3>Debug</h3>
                <button className="smallButton" title="Shift-Ctrl-X" onClick={setEmptyMethod}>
                    Empty Method
                </button>
                <button className="smallButton" title="Shift-Ctrl-C" onClick={setEmptyClass}>
                    Empty Class
                </button>
                <button className="smallButton" title="Shift-Ctrl-S" onClick={copyLeft}>
                    Copy Left
                </button>
                <label>
                    <input
                        type="checkbox"
                        checked={showDoc}
                        onChange={() => {
                            setShowDoc(!showDoc);
                        }}
                    />
                    Show Doc
                </label>
                {showDoc && (
                    <label>
                        <input
                            type="checkbox"
                            checked={hideNull}
                            onChange={() => {
                                setHideNull(!hideNull);
                            }}
                        />
                        Hide Null
                    </label>
                )}
                <label>
                    <input
                        type="checkbox"
                        checked={showAst}
                        onChange={() => {
                            setShowAst(!showAst);
                        }}
                    />
                    Show AST
                </label>
            </div>
        </div>
    );
});
