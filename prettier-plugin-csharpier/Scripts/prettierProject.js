const prettier = require("prettier");
const fs = require("fs");
const path = require("path");

const [, , projectPath] = process.argv;

// TODO 0 do all files in a project, report which files errored, report full time to parse
