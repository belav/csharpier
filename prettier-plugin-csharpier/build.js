const esbuild = require("esbuild");
const chokidar = require("chokidar");

const noop = () => {};

const updateLine = (input, isBuiltInput = false) => {
    const numberOfLines = (input.match(/\n/g) || []).length;
    process.stdout.cursorTo(0, 2);
    process.stdout.clearScreenDown();
    process.stdout.write(input);
    isBuiltInput ? process.stdout.moveCursor(0, -numberOfLines) : noop();
};

// TODO this doesn't work yet, it doesn't actually include anything beyond index.ts. It also doesn't support umd, so I'm not sure if it can really work for prettier plugins
const build = async () => {
    const service = await esbuild.startService();
    try {
        process.stdout.cursorTo(0, 2);
        process.stdout.clearLine(0);

        const timerStart = Date.now();

        await service.build({
            color: true,
            entryPoints: ["./src/Index.ts"],
            outfile: "./dist/index.js",
            // minify: true,
            //bundle: true,
            sourcemap: true,
            tsconfig: "./tsconfig.json",
            platform: "node",
            logLevel: "error",
        });

        const timerEnd = Date.now();
        updateLine(`Built in ${timerEnd - timerStart}ms.`, true);
    } catch (e) {
        throw e;
    } finally {
        service.stop();
    }
};

const watcher = chokidar.watch(["src/**/*"]);
console.log("Watching files... \n");
build();
watcher.on("change", () => {
    build();
});
