const prettier = require("prettier");
const { concat, group, join, line, softline, hardline, indent } = prettier.doc.builders;

test("basic test", () => {
    const actual = print(concat(["public", " ", "class", " ", "ClassName"]));
    expect(actual).toBe("public class ClassName");
});

test("indent using", () => {
    const parts = [];
    parts.push("namespace Namespace");
    parts.push(hardline);
    parts.push("{");
    parts.push(indent(concat([hardline, "using One;", hardline, "using Two;"])));
    parts.push(hardline);
    parts.push("}");

    const actual = print(concat(parts));
    expect(actual).toBe(`namespace Namespace
{
    using One;
    using Two;
}`);
});

test("indent numbers", () => {
    const doc = group(
        concat(["[", indent(concat([hardline, join(concat([",", line]), ["1", "2", "3"])])), hardline, "]"]),
    );

    const actual = print(doc);
    expect(actual).toBe(`[
    1,
    2,
    3
]`);
});

test("indent argumentList", () => {
    const parts = [];
    parts.push("this.Method");
    parts.push(
        concat([
            "(",
            indent(
                concat([
                    line,
                    "lkjasdjlkfajklsdfkljasdfjklasjklfjkasdlf",
                    ",",
                    line,
                    "lkjasdlfkjajlsdfjklasdklfaksjldf",
                    ",",
                ]),
            ),
            softline,
            ")",
            ";",
        ]),
    );

    const actual = print(concat(parts));
    expect(actual).toBe(`this.Method(
    lkjasdjlkfajklsdfkljasdfjklasjklfjkasdlf,
    lkjasdlfkjajlsdfjklasdklfaksjldf,
);`);
});

function print(doc) {
    const result = prettier.doc.printer.printDocToString(doc, {
        tabWidth: 4,
    });
    return result.formatted;
}
