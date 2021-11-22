const { spawn } = require("child_process");
const fs = require("fs");

// TODO can this plus the powershell one just use c#?
// TODO test with different line endings?
// TODO test with ignored file?
const formatted = "public class ClassName { }\n";
const unformatted1 = "public class ClassName  { }";
const unformatted2 = "public class ClassName  {         }";
let failed = false;

(async() => {
    const callbacks = [];
    const longRunner = spawn("Src\\CSharpier.Cli\\bin\\release\\net6.0\\dotnet-csharpier.exe", ["--pipe-multiple-files"], {
        stdio: "pipe"
    });

    longRunner.stdout.on("data", (chunk) => {
        const callback = callbacks.shift();
        if (callback) {
            callback(chunk.toString());
        }
    });

    function sendMessage(fileName, content) {
        longRunner.stdin.write(fileName);
        longRunner.stdin.write("\u0003");
        longRunner.stdin.write(content);
        longRunner.stdin.write("\u0003");
        return new Promise((resolve) => {
            callbacks.push(resolve);
        });
    }
    
    let result = await sendMessage("Test.cs", unformatted1);
    if (result !== formatted) {
        console.log("The result of formatting a basic class was")
        console.log(result);
        console.log("Expected");
        console.log(formatted);
        console.log("");
        failed = true;
    }
    result = await sendMessage("Test.cs", unformatted2);
    if (result !== formatted) {
        console.log("The result of formatting a second basic class was")
        console.log(result);
        console.log("Expected");
        console.log(formatted);
        console.log("");
        failed = true;
    }
    
    longRunner.stdin.pause();
    longRunner.kill();
    
    if (failed) {
        process.exit(1);
    }
})();