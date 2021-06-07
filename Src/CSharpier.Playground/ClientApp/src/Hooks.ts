import { useAppContext } from "./AppContext";

export const useOptions = () => {
    const { formatCode, setEmptyMethod, setEmptyClass, copyLeft } = useAppContext();

    return {
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
            "Ctrl-Enter": () => {
                formatCode();
            },
            "Shift-Ctrl-C": () => {
                setEmptyClass();
            },
            "Shift-Ctrl-X": () => {
                setEmptyMethod();
            },
            "Shift-Ctrl-S": () => {
                copyLeft();
            },
            "Shift-Tab": (cm: any) => cm.execCommand("indentLess"),
        },
        gutters: ["gutter-errors"],
    };
};
