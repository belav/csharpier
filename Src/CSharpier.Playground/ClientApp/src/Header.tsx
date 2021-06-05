import React from "react";
import styled from "styled-components";
import { Loading } from "./Icons/Loading";
import { useAppContext } from "./AppContext";

export const Header = () => {
    const { isLoading, formatCode, showDoc, setShowDoc, showAst, setShowAst } = useAppContext();
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
            <Right>
                <label>
                    <input type="checkbox" checked={showDoc} onChange={() => {setShowDoc(!showDoc)}} />
                    Show Doc
                </label>
                <label>
                    <input type="checkbox" checked={showAst} onChange={() => {setShowAst(!showAst)}} />
                    Show AST
                </label>
            </Right>
        </HeaderStyle>
    );
};

const HeaderStyle = styled.div`
    height: 60px;
    background-color: #f7f7f7;
    display: flex;
    align-items: center;

    > div {
        display: flex;
    }
`;

const Left = styled.div`
    align-items: center;
    width: 25%;
`;
const Right = styled.div`
    margin-left: auto;
    padding-right: 20px;
    label {
        margin-left: 10px;
        cursor: pointer;
    }
`

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
