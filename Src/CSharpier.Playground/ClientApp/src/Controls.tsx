import React from "react";
import { useAppContext } from "./AppContext";
import "./Controls.css";

export const Controls = () => {
    const {
        showDoc,
        setShowDoc,
        hideNull,
        setHideNull,
        showAst,
        setShowAst,
        setEmptyMethod,
        setEmptyClass,
        copyLeft,
        printWidth,
        setPrintWidth,
        indentSize,
        setIndentSize,
        useTabs,
        setUseTabs,
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
};
