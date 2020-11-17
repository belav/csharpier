import { doc } from "prettier";
const { indent, softline, group, concat, line, hardline, join } = doc.builders;
const doubleHardline = concat([hardline, hardline]);

export { indent, softline, group, concat, line, hardline, join, doubleHardline };
