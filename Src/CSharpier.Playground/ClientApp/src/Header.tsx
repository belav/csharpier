import React, { useEffect, useState } from "react";
import { Loading } from "./Icons/Loading";
import { useAppContext } from "./AppContext";
import "./Header.css";

export const Header = () => {
    const { isLoading, formatCode, showDoc, showAst } = useAppContext();
    const width = showDoc && showAst ? 25 : showDoc || showAst ? 33 : 50;
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
        <div className="header">
            <div className={`left left${width}`}>
                <div className="logo" />
                <h1 className="title">
                    <a href="https://csharpier.com">CSharpier</a>
                    <span className="version">{version}</span>
                </h1>
                <div className="buttons">
                    <button className="formatButton" onClick={formatCode} title="Ctrl-Enter">
                        {isLoading && <Loading className="headerLoading" />}
                        {!isLoading && <>Format</>}
                    </button>
                </div>
            </div>
            <div className="right">
                <a className="header-github-link" href="https://github.com/belav/csharpier"></a>
            </div>
        </div>
    );
};
