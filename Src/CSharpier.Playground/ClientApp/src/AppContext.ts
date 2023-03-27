import { createContext, useContext } from "react";
import { makeAutoObservable, runInAction } from "mobx";
import { formatCode, setFormattedCodeEditor } from "./FormatCode";

class AppState {
    fileExtension = window.sessionStorage.getItem("fileExtension") ?? "cs";
    showAst = window.sessionStorage.getItem("showAst") === "true";
    showDoc = window.sessionStorage.getItem("showDoc") === "true";
    hideNull = window.sessionStorage.getItem("hideNull") === "true";
    doc = "";
    isLoading = false;
    hasErrors = false;
    syntaxTree = undefined as object | undefined;
    formattedCode = "";
    enteredCode = window.sessionStorage.getItem("enteredCode") ?? defaultCs;

    constructor() {
        makeAutoObservable(this);
    }

    setFileExtension = (value: string) => {
        window.sessionStorage.setItem("fileExtension", value);
        this.fileExtension = value;
        if (value === "cs") {
            this.setEnteredCode(defaultCs);
        } else {
            this.setEnteredCode(defaultCsProj);
        }
        this.formatCode();
    };

    setShowAst = (value: boolean) => {
        window.sessionStorage.setItem("showAst", value.toString());
        this.showAst = value;
    };

    setShowDoc = (value: boolean) => {
        window.sessionStorage.setItem("showDoc", value.toString());
        this.showDoc = value;
    };

    setHideNull = (value: boolean) => {
        window.sessionStorage.setItem("hideNull", value.toString());
        this.hideNull = value;
    };

    setEnteredCode = (value: string) => {
        window.sessionStorage.setItem("enteredCode", value);
        this.enteredCode = value;
    };

    setEmptyMethod = () => {
        this.setEnteredCode(`class ClassName
{
    void MethodName()
    {
        HERE
    }
}`);
    };

    setEmptyClass = () => {
        this.setEnteredCode(`class ClassName
{
    HERE
}`);
    };

    copyLeft = () => {
        this.setEnteredCode(this.formattedCode);
    };

    formatCode = () => {
        (async () => {
            this.isLoading = true;

            const { syntaxTree, formattedCode, doc, hasErrors } = await formatCode(this.enteredCode, this.fileExtension);

            runInAction(() => {
                this.isLoading = false;
                this.syntaxTree = syntaxTree;
                this.formattedCode = formattedCode;
                this.doc = doc;
                this.hasErrors = hasErrors;
            });
        })();
    };

    setFormattedCodeEditor = setFormattedCodeEditor;
}

export const defaultCs = `public class ClassName {
    public string ShortPropertyName {
        get;
        set; 
    }

    public void LongUglyMethod(string longParameter1, string longParameter2, string longParameter3) { 
        this.LongUglyMethod("1234567890", "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
    }
}`;

const defaultCsProj = `<Project Sdk="Microsoft.NET.Sdk">
      <PropertyGroup>
    <LangVersion>4</LangVersion>
  </PropertyGroup>
</Project>`;

export { AppState };

export const AppStateContext = createContext<AppState>({} as any);

export const useAppContext = () => useContext(AppStateContext);
