import React from "react";
import "codemirror/lib/codemirror.css";
import "codemirror/mode/clike/clike";
import styled from "styled-components";
import { SyntaxTree } from "./SyntaxTree";
import { DocTree } from "./DocTree";
import { Header } from "./Header";
import { useAppContext } from "./AppContext";
import { CodeEditor } from "./CodeEditor";
import { FormattedCode } from "./FormattedCode";
import { Controls } from "./Controls";

export const Layout = () => {
    const { showDoc, showAst } = useAppContext();
    const width = showDoc && showAst ? 25 : showDoc || showAst ? 33.3 : 50;
    return (
        <WrapperStyle>
            <Header />
            <OuterWrapper>
                <Controls />
                <PanelWrapperStyle columnWidth={width}>
                    <EnteredCodeStyle>
                        <CodeEditor />
                    </EnteredCodeStyle>
                    <EnteredCodeStyle>
                        <FormattedCode />
                    </EnteredCodeStyle>
                    {showDoc && <DocTree />}
                    {showAst && <SyntaxTree />}
                </PanelWrapperStyle>
            </OuterWrapper>
            <Footer />
        </WrapperStyle>
    );
};

const WrapperStyle = styled.div`
    height: 100%;
`;

const OuterWrapper = styled.div`
    height: calc(100vh - 80px);
    display: flex;
    flex-wrap: wrap;
    width: 100%;
    > div:first-child {
        width: 130px;
        @media (max-width: 768px) {
            display: none;
        }
    }
`;

const PanelWrapperStyle = styled.div<{ columnWidth: number }>`
    display: flex;
    flex-wrap: wrap;
    width: calc(100% - 130px);
    height: 100%;
    border-top: 1px solid #ccc;
    border-bottom: 1px solid #ccc;
    > div {
        width: ${props => props.columnWidth}%;

        height: 100%;
    }

    .react-codemirror2,
    .CodeMirror {
        height: 100%;
    }
    font-size: 12px;

    @media (max-width: 768px) {
        width: 100%;
        > div {
            width: 100%;
            height: 50%;
        }
    }
`;

const EnteredCodeStyle = styled.div`
    height: 100%;

    .compilationError {
        border-bottom: solid 1px red;
    }

    .gutter-errors {
        width: 16px;
    }

    .errorGutter {
        text-align: center;
        &:hover {
            div {
                display: block;
            }
        }
        span {
            color: red;
            font-weight: bold;
        }

        div {
            position: absolute;
            left: 32px;
            top: -10px;
            width: auto;
            background-color: #ef6464;
            padding: 2px;
            white-space: nowrap;
            display: none;
        }
    }

    @media only screen and (max-width: 768px) {
        width: 100%;
        height: 50%;
        border-bottom: 1px solid #ccc;
    }
`;

const Footer = styled.div`
    height: 20px;
`;
