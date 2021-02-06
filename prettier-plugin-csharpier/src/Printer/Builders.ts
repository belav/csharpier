import { Doc, doc } from "prettier";
const { indent, softline, group, concat, line, hardline, join, breakParent } = doc.builders;
const doubleHardline = concat([hardline, hardline]);

export { indent, softline, group, concat, line, hardline, join, doubleHardline, breakParent };
