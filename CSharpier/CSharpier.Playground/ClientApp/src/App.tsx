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
    showCode: boolean;
}

export class App extends Component<{}, State> {
    constructor(props: {}) {
        super(props);
        this.state = {
            isLoading: false,
            enteredCode: `public class UglyClassName {
            public string MyProperty
            {
             get;  set; }

    public void MethodName(string LongParameter1, string longParameter2, string LongParameter3) { this.MethodName("ajskdf", "kjlasdfkljasldkfklajsdf", "ljkasdfkljaskldfjasdf"; }
}`,
            formattedCode: "",
            json: "",
            showCode: true,
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
            },
        });
        const data = await response.json();
        this.setState({
            formattedCode: data.code,
            isLoading: false,
            json: data.json,
        })
    }

    render() {
        const options = {
            lineNumbers: true,
            matchBrackets: true,
            mode: "text/x-java",
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
                    <div>
                        <Title>
                            CSharpier
                        </Title>
                        <FormatButton onClick={this.formatCode}>
                            {this.state.isLoading &&
                                <LoadingStyle />
                            }
                            {!this.state.isLoading &&
                                <>Format</>
                            }
                        </FormatButton>
                    </div>
                    <Tabs>
                        <Tab data-isactive={this.state.showCode} onClick={() => this.setState({ showCode: true })}>Formatted Code</Tab>
                        <Tab data-isactive={!this.state.showCode} onClick={() => this.setState({ showCode: false })}>AST</Tab>
                    </Tabs>
                </Header>
                <CodeWrapperStyle>
                    <EnteredCodeStyle>
                        <CodeMirror
                            value={this.state.enteredCode}
                            options={options}
                            onBeforeChange={(editor, data, value) => {
                                this.setState({enteredCode: value});
                            }}
                            onChange={() => {}}
                        />
                    </EnteredCodeStyle>
                    <EnteredCodeStyle>
                        {this.state.showCode &&
                            <CodeMirror
                                value={this.state.formattedCode}
                                options={{...options, readOnly: true}}
                                onBeforeChange={() => {
                                }}
                                onChange={() => {
                                }}
                            />
                        }
                        {!this.state.showCode &&
                        <CodeMirror
                            value={this.state.json}
                            options={{...jsonOptions, readOnly: true}}
                            onBeforeChange={() => {
                            }}
                            onChange={() => {
                            }}
                        />
                        }
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

const Title = styled.h1`
    padding-left: 28px;
    font-size: 22px;
    font-style: italic;
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

const Tabs = styled.div`
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
`

const Footer = styled.div`
    height: 20px;
`;
