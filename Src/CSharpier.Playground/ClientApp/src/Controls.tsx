import React from "react";
import "./Controls.css";
import { observer } from "mobx-react-lite";
import { useAppContext } from "./AppContext";

export const Controls = observer(() => {
    const {
        printWidth,
        setPrintWidth,
        indentSize,
        setIndentSize,
        useTabs,
        setUseTabs,
        formatter,
        setFormatter,
        xmlWhitespaceSensitivity,
        setXmlWhitespaceSensitivity,
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
                <label>
                    <input type="radio" checked={formatter === "CSharp"} onClick={e => setFormatter("CSharp")} />
                    C#
                </label>
                <label>
                    <input
                        type="radio"
                        checked={formatter === "CSharpScript"}
                        onClick={e => setFormatter("CSharpScript")}
                    />
                    C# Script
                </label>
                <label>
                    <input type="radio" checked={formatter === "XML"} onClick={e => setFormatter("XML")} />
                    XML
                </label>
                {formatter === "XML" && (
                    <>
                        <label>Whitespace Sensitivity</label>
                        <label>
                            <input
                                type="radio"
                                checked={xmlWhitespaceSensitivity === "Strict"}
                                onClick={() => setXmlWhitespaceSensitivity("Strict")}
                            />
                            Strict
                        </label>
                        <label>
                            <input
                                type="radio"
                                checked={xmlWhitespaceSensitivity === "Xaml"}
                                onClick={() => setXmlWhitespaceSensitivity("Xaml")}
                            />
                            Xaml
                        </label>
                        <label>
                            <input
                                type="radio"
                                checked={xmlWhitespaceSensitivity === "Ignore"}
                                onClick={() => setXmlWhitespaceSensitivity("Ignore")}
                            />
                            Ignore
                        </label>
                    </>
                )}
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
