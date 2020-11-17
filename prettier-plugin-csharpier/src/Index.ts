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
    writeParserJson: {
        type: "string",
        category: "Global",
    }
};

// TODO readme in the root? make a build process? npm package? so much syntax to support!
module.exports = {
    languages,
    printers,
    parsers,
    options,
    defaultOptions: {
        tabWidth: 4,
    },
};
