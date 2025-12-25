import * as vscode from "vscode";

export interface Options {
    readonly dotNetCliPaths: string[];
    readonly dotnetPath: string;
}

class OptionsImpl implements Options {
    public get dotNetCliPaths() {
        return readOption<string[]>("omnisharp.dotNetCliPaths", []);
    }
    public get dotnetPath() {
        return readOption<string>("dotnet.dotnetPath", "", "omnisharp.dotnetPath");
    }
}

export let options: Options = new OptionsImpl();

function readOptionFromConfig<T>(
    config: vscode.WorkspaceConfiguration,
    option: string,
    defaultValue: T,
    ...backCompatOptionNames: string[]
): T {
    let value = config.get<T>(option);

    if (value === undefined && backCompatOptionNames.length > 0) {
        // Search the back compat options for a defined value.
        value = backCompatOptionNames.map(name => config.get<T>(name)).find(val => val);
    }

    return value ?? defaultValue;
}

function readOption<T>(option: string, defaultValue: T, ...backCompatOptionNames: string[]): T {
    return readOptionFromConfig(
        vscode.workspace.getConfiguration(),
        option,
        defaultValue,
        ...backCompatOptionNames,
    );
}
