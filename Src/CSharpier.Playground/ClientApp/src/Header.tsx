import React from "react";
import styled from "styled-components";
import { Loading } from "./Icons/Loading";
import { useAppContext } from "./AppContext";

export const Header = () => {
    const { tab, setTab, isLoading, hasErrors, formatCode } = useAppContext();
    return (
        <HeaderStyle>
            <Left>
                <Title>CSharpier</Title>
                <a
                    className="github-button"
                    href="https://github.com/belav/csharpier"
                    data-size="large"
                    data-show-count="true"
                    aria-label="Star belav/csharpier on GitHub"
                >
                    Github
                </a>
                <FormatButton onClick={formatCode}>
                    {isLoading && <LoadingStyle />}
                    {!isLoading && <>Format</>}
                </FormatButton>
            </Left>
            <Tabs>
                <Tab data-isactive={tab === "code"} data-haserrors={hasErrors} onClick={() => setTab("code")}>
                    Formatted Code
                </Tab>
                <Tab data-isactive={tab === "ast"} onClick={() => setTab("ast")}>
                    AST
                </Tab>
                <Tab data-isactive={tab === "doc"} onClick={() => setTab("doc")}>
                    Doc
                </Tab>
            </Tabs>
        </HeaderStyle>
    );
};

const HeaderStyle = styled.div`
    height: 60px;
    background-color: #f7f7f7;
    display: flex;
    align-items: center;

    > div {
        width: 50%;
        display: flex;
    }
`;

const Left = styled.div`
    align-items: center;
`;

const Title = styled.h1`
    padding-left: 28px;
    font-size: 22px;
    font-style: italic;
    margin-right: 20px;
`;

const FormatButton = styled.button`
    margin-left: auto;
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

const Tabs = styled.div`
    width: 50%;
    padding-left: 48px;
    height: 100%;
    display: flex;
    align-items: baseline;
`;

const Tab = styled.button`
    font-size: 16px;
    margin-right: 20px;
    margin-top: auto;
    border: 1px solid #ddd;
    margin-bottom: -1px;
    padding: 4px 8px;
    cursor: pointer;

    &[data-isactive="true"] {
        background: white;
        border-bottom: none;
        cursor: default;
    }

    &[data-haserrors="true"] {
        color: red;
    }

    &:focus {
        outline: none;
    }
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
