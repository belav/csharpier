import { createContext, useContext } from "react";
import { makeAutoObservable, runInAction } from "mobx";
import { formatCode, setFormattedCodeEditor } from "./FormatCode";

const getString = (key: string, defaultValue: string) => {
    const result = window.sessionStorage.getItem(key);
    if (result === null) {
        return defaultValue;
    }

    return result;
};

const getBoolean = (key: string, defaultValue: boolean) => {
    const result = window.sessionStorage.getItem(key);
    if (result === null) {
        return defaultValue;
    }

    return !!result;
};

const getNumber = (key: string, defaultValue: number) => {
    const result = window.sessionStorage.getItem(key);
    if (result === null) {
        return defaultValue;
    }

    return parseInt(result, 10);
};

class AppState {
    printWidth = getNumber("printWidth", 100);
    indentSize = getNumber("indentSize", 4);
    useTabs = getBoolean("useTabs", false);
    formatter = getString("formatter", "CSharp");
    showAst = getBoolean("showAst", false);
    showDoc = getBoolean("showDoc", false);
    hideNull = getBoolean("hideNull", false);
    doc = "";
    isLoading = false;
    hasErrors = false;
    syntaxTree = undefined as object | undefined;
    formattedCode = "";
    enteredCode = window.sessionStorage.getItem("enteredCode") ?? defaultCs;

    constructor() {
        makeAutoObservable(this);
    }

    setFormatter = (value: string) => {
        window.sessionStorage.setItem("formatter", value);
        this.formatter = value;
    };

    setIndentSize = (value: number) => {
        window.sessionStorage.setItem("indentSize", value.toString(10));
        this.indentSize = value;
    };

    setPrintWidth = (value: number) => {
        window.sessionStorage.setItem("printWidth", value.toString(10));
        this.printWidth = value;
    };

    setUseTabs = (value: boolean) => {
        window.sessionStorage.setItem("useTabs", value.toString());
        this.useTabs = value;
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

            const { syntaxTree, formattedCode, doc, hasErrors } = await formatCode(
                this.enteredCode,
                this.printWidth,
                this.indentSize,
                this.useTabs,
                this.formatter,
            );

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
