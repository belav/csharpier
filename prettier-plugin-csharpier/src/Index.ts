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

// TODO strip json to just this? https://github.com/ashmind/SharpLab/blob/main/source/Server/Decompilation/AstOnly/RoslynAstTarget.cs
// or do something like it
// TODO is this useful? https://raw.githubusercontent.com/dotnet/roslyn/1b0cf5c732062f66b71a3d62a165d6eb5f8b3022/src/Compilers/CSharp/Portable/Syntax/SyntaxKindFacts.cs
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
