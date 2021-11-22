import commandExists = require("command-exists");

export const isInstalled = (commandName: string): Promise<boolean> =>
    new Promise(resolve =>
        commandExists(commandName, async (_, exists: boolean) => resolve(exists)),
    );
