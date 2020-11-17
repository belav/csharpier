import { PrintMethod } from "../PrintMethod";

export const print: PrintMethod = (path, options, print) => {
    return path.call(print, "members", 0);
};
