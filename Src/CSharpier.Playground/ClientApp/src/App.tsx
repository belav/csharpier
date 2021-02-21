import React, { Component } from "react";
import { Controlled as CodeMirror } from "react-codemirror2";
import "codemirror/lib/codemirror.css";
import "codemirror/mode/clike/clike";
import styled from "styled-components";
import { Loading } from './Icons/Loading';

interface State {
    enteredCode: string;
    formattedCode: string;
    json: string;
    isLoading: boolean;
    tab: string;
    doc: string;
    hasErrors: boolean;
}

const defaultCode = `public class UglyClassName {
    public string MyProperty {
        get;
        set; 
    }

    public void MethodName(string LongParameter1, string longParameter2, string LongParameter3) { 
        this.MethodName("ajskdf", "kjlasdfkljasldkfklajsdf", "ljkasdfkljaskldfjasdf";
    }
}`

export class App extends Component<{}, State> {
    constructor(props: {}) {
        super(props);

        const existingCode = window.sessionStorage.getItem("enteredCode") ?? defaultCode;

        this.state = {
            isLoading: false,
            enteredCode: existingCode,
            formattedCode: "",
            json: "",
            doc: "",
            tab: "code",
            hasErrors: false,
        };
    }

    componentDidMount() {
        this.formatCode();
    }

    formatCode = async () => {
        this.setState({
            isLoading: true,
        })

        const response = await fetch("/Format", {
            method: "POST",
            body: JSON.stringify(this.state.enteredCode),
            headers: {
                "Content-Type": "application/json",
                "cache-control": "no-cache",
            },
        });
        if (response.status === 200) {
            const data = await response.json();
            this.setState({
                formattedCode: data.code,
                isLoading: false,
                json: data.json,
                doc: data.doc,
                hasErrors: !!data.errors, 
            })
        } else {
            const text = await response.text();
            this.setState({
                formattedCode: text,
                isLoading: false,
                json: text,
                doc: text,
                hasErrors: true,
            })
        }
    }

    render() {
        const options = {
            lineNumbers: true,
            matchBrackets: true,
            mode: "text/x-java",
            indentWithTabs: false,
            smartIndent: false,
            tabSize: 4,
            extraKeys: {
                Tab: (cm: any) => {
                    if (cm.getMode().name === "null") {
                        cm.execCommand("insertTab");
                    } else {
                        if (cm.somethingSelected()) {
                            cm.execCommand("indentMore");
                        } else {
                            cm.execCommand("insertSoftTab");
                        }
                    }
                },
                'Shift-Tab': (cm: any) => cm.execCommand("indentLess")
            }
        };

        const jsonOptions = {
            lineNumbers: true,
            mode: {
                name: "javascript",
                json: true,
            },
            foldGutter: true,
            gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
        }

        return (
            <WrapperStyle>
                <Header>
                    <Left>
                        <Title>
                            CSharpier
                        </Title>
                        <a className="github-button" href="https://github.com/belav/csharpier" data-size="large"
                           data-show-count="true" aria-label="Star belav/csharpier on GitHub">Github</a>
                        <FormatButton onClick={this.formatCode}>
                            {this.state.isLoading &&
                            <LoadingStyle/>
                            }
                            {!this.state.isLoading &&
                            <>Format</>
                            }
                        </FormatButton>
                    </Left>
                    <Tabs>
                        <Tab data-isactive={this.state.tab === "code"} data-haserrors={this.state.hasErrors}
                             onClick={() => this.setState({tab: "code"})}>Formatted
                            Code</Tab>
                        <Tab data-isactive={this.state.tab === "ast"}
                             onClick={() => this.setState({tab: "ast"})}>AST</Tab>
                        <Tab data-isactive={this.state.tab === "doc"}
                             onClick={() => this.setState({tab: "doc"})}>Doc</Tab>
                    </Tabs>
                </Header>
                <CodeWrapperStyle>
                    <EnteredCodeStyle>
                        <CodeMirror
                            value={this.state.enteredCode}
                            options={options}
                            onBeforeChange={(editor, data, value) => {
                                window.sessionStorage.setItem("enteredCode", value);
                                this.setState({enteredCode: value});
                            }}
                            onChange={() => {
                            }}
                        />
                    </EnteredCodeStyle>
                    <EnteredCodeStyle>
                        <TabBody isVisible={this.state.tab === "code"}>
                            <CodeMirror
                                value={this.state.formattedCode}
                                options={{...options, readOnly: true}}
                                onBeforeChange={() => {
                                }}
                                onChange={() => {
                                }}
                            />
                        </TabBody>
                        <TabBody isVisible={this.state.tab === "ast"}>
                            <CodeMirror
                                value={this.state.json}
                                options={{...jsonOptions, readOnly: true}}
                                onBeforeChange={() => {
                                }}
                                onChange={() => {
                                }}
                            />
                        </TabBody>
                        <TabBody isVisible={this.state.tab === "doc"}>
                            <CodeMirror
                                value={this.state.doc}
                                options={{...jsonOptions, readOnly: true}}
                                onBeforeChange={() => {
                                }}
                                onChange={() => {
                                }}
                            />
                        </TabBody>
                    </EnteredCodeStyle>
                </CodeWrapperStyle>
                <Footer/>
            </WrapperStyle>
        );
    }
}

const EnteredCodeStyle = styled.div`
  width: 50%;
  height: 100%;

  .react-codemirror2,
  .CodeMirror {
    height: 100%;
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

const Header = styled.div`
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
`

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

const TabsHeader = styled.div`
  background-color: #f7f7f7;
  display: flex;
`

const Tabs = styled.div`
  width: 50%;
  padding-left: 48px;
  height: 100%;
  display: flex;
  align-items: baseline;
`

const Tab = styled.button`
  font-size: 16px;
  margin-right: 20px;
  margin-top: auto;
  border: 1px solid #ddd;
  margin-bottom: -1px;
  padding: 4px 8px;
  cursor: pointer;

  &[data-isactive=true] {
    background: white;
    border-bottom: none;
    cursor: default;
  }

  &[data-haserrors=true] {
    color: red;
  }

  &:focus {
    outline: none;
  }
`;

const TabBody = styled.div<{ isVisible: boolean }>`
  ${props => props.isVisible ? "" : "display: none;"}
  height: 100%;
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
`

const Footer = styled.div`
  height: 20px;
`;
