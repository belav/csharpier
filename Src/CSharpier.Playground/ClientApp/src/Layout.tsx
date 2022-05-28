import React from "react";
import "codemirror/lib/codemirror.css";
import "codemirror/mode/clike/clike";
import { SyntaxTree } from "./SyntaxTree";
import { DocTree } from "./DocTree";
import { Header } from "./Header";
import { useAppContext } from "./AppContext";
import { CodeEditor } from "./CodeEditor";
import { FormattedCode } from "./FormattedCode";
import { Controls } from "./Controls";
import "./Layout.scss";

export const Layout = () => {
    const { showDoc, showAst } = useAppContext();
    const width = showDoc && showAst ? 25 : showDoc || showAst ? 33 : 50;
    return (
        <div className="layoutWrapper">
            <Header />
            <div className="outerWrapper">
                <Controls />
                <div className={`panelWrapper panelWrapper${width}`}>
                    <div className="enteredCode">
                        <CodeEditor />
                    </div>
                    <div className="enteredCode">
                        <FormattedCode />
                    </div>
                    {showDoc && <DocTree />}
                    {showAst && <SyntaxTree />}
                </div>
            </div>
            <div className="footer" />
        </div>
    );
};
