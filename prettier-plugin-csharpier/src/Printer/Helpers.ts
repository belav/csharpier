import { Doc } from "prettier";
import { concat, join, line } from "./Builders";

export function printCommaList(list: Doc[]) {
    return join(concat([",", line]), list);
}
