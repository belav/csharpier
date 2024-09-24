// @ts-check

import React from "react";
import Layout from "@theme/Layout";

function HomepageHeader() {
    return (
        <header className="hero">
            <div className="container">
                <div className="hero__left">
                    <h1 className="hero__title">CSharpier</h1>
                    <p className="hero__subtitle">An Opinionated Code Formatter</p>
                    <a className="hero__getStarted" href="/docs/About">
                        Get started
                    </a>
                </div>
                <div className="hero__right">
                    <img className="hero__image" src="img/logo.svg" />
                </div>
            </div>
        </header>
    );
}

export default function Home() {
    return (
        <Layout>
            <HomepageHeader />
            <main className="container">
                <div className="home__text">
                    <p>
                        CSharpier is an opinionated code formatter for c#. It uses Roslyn to parse your code and
                        re-prints it using its own rules. The printing process was ported from{" "}
                        <a href="https://prettier.io/">prettier</a> but has evolved over time.
                    </p>
                    <p>
                        CSharpier provides a few basic options that affect formatting and has no plans to add more. It
                        follows the <a href="https://prettier.io/docs/en/option-philosophy.html">Option Philosophy</a>{" "}
                        of prettier.
                    </p>
                    <h4>Why adopt CSharpier?</h4>
                    <ul>
                        <li>It is fast.</li>
                        <li>Provides a single option to debate - width</li>
                        <li>Integrates with the major c# IDEs</li>
                        <li>Works with c# &lt;= 12</li>
                        <li>Supports validating the changes it makes.</li>
                    </ul>
                </div>
                <div className="codeContainer">
                    <div className="codeExample" />
                </div>
                <div className="sponsor">
                    <h2>Sponsors</h2>
                    Special thanks to the <a href="https://github.com/aws/dotnet-foss">.NET on AWS Open Source Software Fund</a> for sponsoring the ongoing development CSharpier.
                    <div>
                        <a href="https://github.com/aws/dotnet-foss"><img src="/img/aws.png" alt="aws logo" /></a>
                    </div>
                </div>
            </main>
        </Layout>
    );
}
