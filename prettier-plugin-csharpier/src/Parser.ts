// @ts-ignore
import { spawnSync } from "child_process";
import * as fs from "fs";

function parseText(text: string) {
    const executionResult = spawnSync("dotnet", ["exec", PARSER_PATH, text], {
        cwd: __dirname,
        maxBuffer: 1000 * 1000 * 100 // 100 MB
    });
    const error = executionResult.stderr.toString();
    if (error) {
        console.log(error);
        throw new Error(error);
    }

    return executionResult;
}

function parseCSharp(text: string, parsers: object, options: any) {
    const executionResult = parseText(text);
    let stdout = "";
    try {
        stdout = executionResult.stdout.toString();
        const ast = JSON.parse(stdout);
        if (options.writeParserJson) {
            fs.writeFileSync(options.writeParserJson, JSON.stringify(ast, null, "    "), "utf8");
        }
        return ast;
    } catch (exception) {
        if (options.writeParserJson) {
            fs.writeFileSync(options.writeParserJson, stdout, "utf8");
        }
    }
}

const defaultExport = {
    parse: parseCSharp,
    astFormat: "cs"
};

export default defaultExport;
