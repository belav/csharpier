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

export const Layout = () => {
    const { tab } = useAppContext();

    return (
        <WrapperStyle>
            <Header />
            <CodeWrapperStyle>
                <EnteredCodeStyle>
                    <CodeEditor />
                </EnteredCodeStyle>
                <EnteredCodeStyle>
                    <TabBody isVisible={tab === "code"}>
                        <FormattedCode />
                    </TabBody>
                    <TabBody isVisible={tab === "ast"}>
                        <SyntaxTree />
                    </TabBody>
                    <TabBody isVisible={tab === "doc"}>
                        <DocTree />
                    </TabBody>
                </EnteredCodeStyle>
            </CodeWrapperStyle>
            <Footer />
        </WrapperStyle>
    );
};

const EnteredCodeStyle = styled.div`
    width: 50%;
    height: 100%;

    .react-codemirror2,
    .CodeMirror {
        height: 100%;
    }

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

const WrapperStyle = styled.div`
    height: 100%;
`;

const CodeWrapperStyle = styled.div`
    display: flex;
    flex-wrap: wrap;
    width: 100%;
    height: calc(100vh - 80px);
    border-top: 1px solid #ccc;
    border-bottom: 1px solid #ccc;
`;

const TabBody = styled.div<{ isVisible: boolean }>`
    ${props => (props.isVisible ? "" : "display: none;")}
    height: 100%;
`;

const Footer = styled.div`
    height: 20px;
`;
