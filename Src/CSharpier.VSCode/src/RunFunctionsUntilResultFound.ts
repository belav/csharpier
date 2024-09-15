export const runFunctionsUntilResultFound = <T>(...functions: (() => T)[]) => {
    for (const possibleFunction of functions) {
        const result = possibleFunction();
        if (result) {
            return result;
        }
    }

    return undefined;
};
