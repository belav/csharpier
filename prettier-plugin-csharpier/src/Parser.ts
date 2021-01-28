// @ts-ignore
import { spawnSync } from "child_process";
import * as fs from "fs";

// TODO this is probably wrong - what is this?
function loc(prop: any) {
    return function (node: any) {
        return node[prop];
    };
}

function parseText(text: string) {
    // TODO where should this dll really live?
    const executionResult = spawnSync(
        "dotnet",
        ["exec", "..\\netcoreapp3.1\\Parser.dll", text],
        {
            maxBuffer: 1000 * 1000 * 100, // 100 MB
        },
    );
    const error = executionResult.stderr.toString();
    if (error) {
        console.log(error);
        throw new Error(error);
    }

    return executionResult;
}

function parseCSharp(text: string, parsers: object, options: any) {
    const executionResult = parseText(text);
    const ast = JSON.parse(executionResult.stdout.toString());
    if (options.writeParserJson) {
        fs.writeFileSync(options.writeParserJson, JSON.stringify(ast, null, "    "), "utf8");
    }

    return ast;
}

const defaultExport = {
    parse: parseCSharp,
    astFormat: "cs",
    locStart: loc("start"),
    locEnd: loc("end"),
};

export default defaultExport;
