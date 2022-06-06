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
    } = useAppContext();
    return (
        <div className="controlsWrapper">
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
    );
};
