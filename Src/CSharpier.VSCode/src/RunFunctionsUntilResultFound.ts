export let runFunctionsUntilResultFound = <T>(...functions: (() => T)[]) => {
    for (let possibleFunction of functions) {
        let result = possibleFunction();
        if (result) {
            return result;
        }
    }

    return undefined;
};
