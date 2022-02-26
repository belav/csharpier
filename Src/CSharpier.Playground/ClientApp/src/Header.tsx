import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { Loading } from "./Icons/Loading";
import { useAppContext } from "./AppContext";

export const Header = () => {
    const { isLoading, formatCode, showDoc, showAst } = useAppContext();
    const width = showDoc && showAst ? 25 : showDoc || showAst ? 33.3 : 50;
    const [version, setVersion] = useState<string | undefined>();

    useEffect(() => {
        (async () => {
            const response = await fetch("/Version", {
                method: "Get",
            });
            const version = await response.text();

            setVersion(version);
        })();
    }, []);

    return (
        <HeaderStyle>
            <Left width={width}>
                <LogoStyle />
                <Title>
                    <a href="https://csharpier.com">CSharpier</a><Version> {version}</Version>
                </Title>
                <Buttons>
                    <FormatButton onClick={formatCode} title="Ctrl-Enter">
                        {isLoading && <LoadingStyle />}
                        {!isLoading && <>Format</>}
                    </FormatButton>
                </Buttons>
            </Left>
            <Right>
                <a className="header-github-link" href="https://github.com/belav/csharpier"></a>
            </Right>
        </HeaderStyle>
    );
};

const HeaderStyle = styled.div`
    height: 60px;
    background-color: #32576C;
    display: flex;
    align-items: center;

    > div {
        display: flex;
    }
`;

const Left = styled.div<{ width: number }>`
    align-items: center;
    width: calc(${props => props.width}% + 64px);
    @media (max-width: 768px) {
        width: 100%;
    }
`;
const Right = styled.div`
    display: flex;
    justify-content: end;
    flex-grow: 1;
    padding-right: 20px;
`;

const LogoStyle = styled.div`
    margin-left: 10px;
    background: url("/logo.svg");
    height: 40px;
    width: 40px;
`;

const Title = styled.h1`
    color: #fff;
    font-size: 16px;
    font-weight: bold;
    margin-left: 8px;
    margin-right: 20px;
    a {
        color: #fff;
        text-decoration: none;
        margin-right: 5px;
    }
`;

const Version = styled.span`
    font-size: 12px;
`;

const Buttons = styled.div`
    margin-left: auto;
    display: flex;
    align-items: center;
`;

const FormatButton = styled.button`
    background-color: #fff;
    color: #0C2B3E;
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
