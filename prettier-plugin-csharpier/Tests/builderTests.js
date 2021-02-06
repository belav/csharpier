const prettier = require("prettier");
const fs = require("fs");
const { concat, group, join, line, softline, hardline, indent, breakParent } = prettier.doc.builders;

test("basic concat", () => {
    const actual = print(concat(["1", "2", "3"]));
    expect(actual).toBe("123");
});

test("concat with hardline", () => {
    const actual = print(concat(["1", hardline, "3"]));
    expect(actual).toBe("1\n3");
});

test("concat with line", () => {
    const actual = print(concat(["1", line, "3"]));
    expect(actual).toBe("1\n3");
});

test("group with line", () => {
    const actual = print(group(concat(["1", line, "3"])));
    expect(actual).toBe("1 3");
});

test("group with hardline", () => {
    const actual = print(group(concat(["1", hardline, "3"])));
    expect(actual).toBe("1\n3");
});

test("group with line and hardline", () => {
    const actual = print(group(concat(["1", line, "2", hardline, "3"])));
    expect(actual).toBe("1\n2\n3");
});

test("group with line and breakParent", () => {
    const actual = print(group(concat(["1", line, "2", line, "3", breakParent])));
    expect(actual).toBe("1\n2\n3");
});

test("indent with breakparent", () => {
    const actual = print(Group(Indent(Concat(softline, "1", Line, "2", Line, "3", BreakParent))));
    expect(actual).toBe("\n    1\n    2\n    3");
});


test("large group concat with line", () => {
    const actual = print(group(concat(["LongTextLongTextLongTextLongText", line, "LongTextLongTextLongTextLongText", line, "LongTextLongTextLongTextLongText"])));
    expect(actual).toBe("LongTextLongTextLongTextLongText\nLongTextLongTextLongTextLongText\nLongTextLongTextLongTextLongText");
});

test("indent with hardline", () => {
    const actual = print(indent(concat([hardline, "1", hardline, "2"])));
    expect(actual).toBe("\n    1\n    2");
});

test("two indents with hardline", () => {
    const actual = print(concat([indent(concat([hardline, "11", hardline, "12"])), hardline, hardline, indent(concat([hardline, "21", hardline, "22"]))]));
    expect(actual).toBe("\n    11\n    12\n\n\n    21\n    22");
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
        concat(["[", indent(concat([hardline, join(concat([",", hardline]), ["1", "2", "3"])])), hardline, "]"]),
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

test("scratch", () => {
    const doc =
        Concat(
            "string Property",
            Group(
                Concat(
                    Line,
                    "{",
                    //Group(
                    Indent(
                        Concat(
                            Line,
                            "protected internal get;",
                            BreakParent,
                            Line,
                            "protected internal set;")), //),
                    Line,
                    "}")));
    const actual = print(doc);
    expect(actual).toBe("");
});

function print(doc) {
    prettier.doc.utils.propagateBreaks(doc);
    const result = prettier.doc.printer.printDocToString(doc, {
        tabWidth: 4,
        endOfLine: "auto",
        printWidth: 80,
    });
    return result.formatted;
}

function Concat() {
    return concat(arguments);
}

function Group(value) {
    return group(value);
}

function Indent(value) {
    return indent(value);
}

const HardLine = hardline;
const Line = line;
const SoftLine = softline;
const BreakParent = breakParent;
