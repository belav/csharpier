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
    height: 24px;
    cursor: pointer;
    margin-right: 6px;
`;

const FormatButton = styled.button`
    background-color: #666;
    color: white;
    border: none;
    padding: 8px 12px;
    font-size: 18px;
    border-radius: 4px;
    cursor: pointer;
    width: 82px;
    display: flex;
    align-items: center;
    justify-content: center;
`;

const LoadingStyle = styled(Loading)`
    animation-name: spin;
    animation-duration: 2000ms;
    animation-iteration-count: infinite;
    animation-timing-function: linear;

    @keyframes spin {
        from {
            transform: rotate(0deg);
        }
        to {
            transform: rotate(360deg);
        }
    }
`;
