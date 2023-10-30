import React, { useContext, useState } from "react";
import { formatCode, setFormattedCodeEditor } from "./FormatCode";

export const AppContext = React.createContext({
    showAst: false,
    setShowAst: (value: boolean) => {},
    showDoc: false,
    setShowDoc: (value: boolean) => {},
    hideNull: false,
    setHideNull: (value: boolean) => {},
    doc: "",
    setDoc: (doc: string) => {},
    isLoading: false,
    setIsLoading: (isLoading: boolean) => {},
    hasErrors: false,
    setHasErrors: (hasErrors: boolean) => {},
    syntaxTree: undefined as object | undefined,
    setSyntaxTree: (syntaxTree: undefined | object) => {},
    formattedCode: "",
    setFormattedCode: (formattedCode: string) => {},
    enteredCode: "",
    setEnteredCode: (enteredCode: string) => {},
    formatCode: () => {},
    setFormattedCodeEditor: (value: unknown) => {},
    setEmptyMethod: () => {},
    setEmptyClass: () => {},
    copyLeft: () => {},
});

export const useAppContext = () => useContext(AppContext);

// I regret trying out this approach to managing state....
export const useSetupAppContext = () => {
    const [doc, setDoc] = useState("");
    const [showAst, setShowAst] = useState(getInitialShowAst());
    const [showDoc, setShowDoc] = useState(getInitialShowDoc());
    const [hideNull, setHideNull] = useState(getInitialHideNull());
    const [formattedCode, setFormattedCode] = useState("");
    const [enteredCode, setEnteredCode] = useState(getInitialCode());
    const [isLoading, setIsLoading] = useState(false);
    const [hasErrors, setHasErrors] = useState(false);
    const [syntaxTree, setSyntaxTree] = useState<object | undefined>(undefined);

    return {
        doc,
        showAst,
        setShowAst: (value: boolean) => {
            window.sessionStorage.setItem("showAst", value.toString());
            setShowAst(value);
        },
        showDoc,
        setShowDoc: (value: boolean) => {
            window.sessionStorage.setItem("showDoc", value.toString());
            setShowDoc(value);
        },
        hideNull,
        setHideNull: (value: boolean) => {
            window.sessionStorage.setItem("hideNull", value.toString());
            setHideNull(value);
        },
        setDoc,
        isLoading,
        setIsLoading,
        hasErrors,
        setHasErrors,
        syntaxTree,
        setSyntaxTree,
        formattedCode,
        setFormattedCode,
        enteredCode,
        setEnteredCode: (value: string) => {
            window.sessionStorage.setItem("enteredCode", value);
            setEnteredCode(value);
        },
        formatCode: async () => {
            setIsLoading(true);

            const { syntaxTree, formattedCode, doc, hasErrors, syntaxValidation } = await formatCode(enteredCode);
            
            if (syntaxValidation) {
                console.log(syntaxValidation);
                console.log(new Date());
            }

            setIsLoading(false);
            setSyntaxTree(syntaxTree);
            setFormattedCode(formattedCode);
            setDoc(doc);
            setHasErrors(hasErrors);
        },
        setFormattedCodeEditor: setFormattedCodeEditor,
        setEmptyMethod: () => {
            setEnteredCode(`class ClassName
{
    void MethodName()
    {
        HERE
    }
}`);
        },
        setEmptyClass: () => {
            setEnteredCode(`class ClassName
{
    HERE
}`);
        },
        copyLeft: () => {
            setEnteredCode(formattedCode);
        },
    };
};

const defaultCode = `public class ClassName {
    public string ShortPropertyName {
        get;
        set; 
    }

    public void LongUglyMethod(string longParameter1, string longParameter2, string longParameter3) { 
        this.LongUglyMethod("1234567890", "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
    }
}`;

const getInitialCode = () => {
    return window.sessionStorage.getItem("enteredCode") ?? defaultCode;
};
const getInitialShowAst = () => {
    return window.sessionStorage.getItem("showAst") === "true";
};
const getInitialShowDoc = () => {
    return window.sessionStorage.getItem("showDoc") === "true";
};
const getInitialHideNull = () => {
    return window.sessionStorage.getItem("hideNull") === "true";
};
