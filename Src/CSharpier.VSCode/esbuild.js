const esbuild = require("esbuild");

const production = process.argv.includes("--production");
const watch = process.argv.includes("--watch");

async function main() {
    const ctx = await esbuild.context({
        entryPoints: ["src/Extension.ts"],
        bundle: true,
        format: "cjs",
        minify: production,
        sourcemap: true,
        sourcesContent: false,
        platform: "node",
        outfile: "build/Extension.js",
        external: ["vscode"],
        logLevel: "warning",
        plugins: [esbuildProblemMatcherPlugin],
    });
    if (watch) {
        await ctx.watch();
    } else {
        await ctx.rebuild();
        await ctx.dispose();
    }
}

const esbuildProblemMatcherPlugin = {
    name: "esbuild-problem-matcher",

    setup(build) {
        build.onStart(() => {
            console.log("[watch] build started");
        });
        build.onEnd(result => {
            result.errors.forEach(({ text, location }) => {
                console.error(`✘ [ERROR] ${text}`);
                if (location == null) return;
                console.error(`    ${location.file}:${location.line}:${location.column}:`);
            });
            console.log("[watch] build finished");
        });
    },
};

main().catch(e => {
    console.error(e);
    process.exit(1);
});
