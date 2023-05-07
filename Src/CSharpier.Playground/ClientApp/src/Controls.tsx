import React from "react";
import "./Controls.css";
import { observer } from "mobx-react-lite";
import { useAppContext } from "./AppContext";

export const Controls = observer(() => {
    const {
        fileExtension,
        setFileExtension,
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

    const onChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setFileExtension(event.currentTarget.value);
    };

    return (
        <div className="controlsWrapper">
            <label>
                Language
                <select value={fileExtension} onChange={onChange}>
                    <option value="cs">C#</option>
                    <option value="xml">Xml</option>
                </select>
            </label>
            {fileExtension === "cs" && (
                <>
                    <hr />
                    <button className="smallButton" title="Shift-Ctrl-X" onClick={setEmptyMethod}>
                        Empty Method
                    </button>
                    <button className="smallButton" title="Shift-Ctrl-C" onClick={setEmptyClass}>
                        Empty Class
                    </button>
                    <button className="smallButton" title="Shift-Ctrl-S" onClick={copyLeft}>
                        Copy Left
                    </button>
                </>
                    )}
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
});
