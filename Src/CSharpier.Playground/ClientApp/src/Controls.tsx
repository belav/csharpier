import React from "react";
import styled from "styled-components";
import { Loading } from "./Icons/Loading";
import { useAppContext } from "./AppContext";

export const Controls = () => {
    const {
        isLoading,
        formatCode,
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
    const width = showDoc && showAst ? 25 : showDoc || showAst ? 33.3 : 50;
    return (
        <Wrapper>
            <SmallButton title="Shift-Ctrl-X" onClick={setEmptyMethod}>
                Empty Method
            </SmallButton>
            <SmallButton title="Shift-Ctrl-C" onClick={setEmptyClass}>
                Empty Class
            </SmallButton>
            <SmallButton title="Shift-Ctrl-S" onClick={copyLeft}>
                Copy Left
            </SmallButton>
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
        </Wrapper>
    );
};

const Wrapper = styled.div`
    border-top: 1px solid #ccc;
    border-bottom: 1px solid #ccc;
    background-color: #f7f7f7;
    padding: 10px;
    > label {
        display: block;
        margin-top: 10px;
    }
    > button {
        width: 100%;
        margin-top: 10px;
    }
`;

const SmallButton = styled.button`
    background-color: #ccc;
    border-radius: 4px;
    color: #000;
    border: none;
    height: 24px;
    cursor: pointer;
    margin-right: 6px;
`;
