import { runTests } from "@vscode/test-electron";
import * as path from "path";

async function main() {
    try {
        const pathToPackageJson = path.resolve(__dirname, "../../");

        const pathToTestSuite = path.resolve(__dirname, "./");

        await runTests({
            extensionDevelopmentPath: pathToPackageJson,
            extensionTestsPath: pathToTestSuite,
        });
    } catch (err) {
        console.error("Failed to run tests");
        process.exit(1);
    }
}

main();
