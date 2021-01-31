import parser from "./Parser";
import printer from "./Printer";

const languages = [
    {
        name: "C#",
        parsers: ["cs"],
        tmScope: "source.cs",
        aceMode: "csharp",
        codemirrorMode: "clike",
        extensions: [".cs", ".cake", ".cshtml", ".csx"],
        vscodeLanguageIds: ["csharp"],
        linguistLanguageId: 42,
    },
];

const parsers = {
    cs: parser,
};

const printers = {
    cs: printer,
};

const options = {
    printTodo: {
        type: "boolean",
        category: "Global",
        default: false,
    },
    writeParserJson: {
        type: "string",
        category: "Global",
    },
};

// TODO eventually go back to original AllInOne to figure out what kind of formatting we may want to change

module.exports = {
    languages,
    printers,
    parsers,
    options,
    defaultOptions: {
        tabWidth: 4,
    },
};
