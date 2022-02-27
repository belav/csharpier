// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require("prism-react-renderer/themes/github");
const darkCodeTheme = require("prism-react-renderer/themes/dracula");

const githubUrl = "https://github.com/belav/csharpier";

/** @type {import("@docusaurus/types").Config} */
const config = {
    title: "CSharpier",
    tagline: "An Opinionated Code Formatter",
    url: "https://csharpier.com",
    baseUrl: "/",
    onBrokenLinks: "throw",
    onBrokenMarkdownLinks: "warn",
    favicon: "img/favicon.ico",
    organizationName: "belav",
    projectName: "csharpier",

    presets: [
        [
            "classic",
            /** @type {import("@docusaurus/preset-classic").Options} */
            ({
                docs: {
                    sidebarPath: require.resolve("./sidebars.js"),
                    editUrl: githubUrl + "/tree/master/Docs/",
                },
                theme: {
                    customCss: require.resolve("./src/css/custom.css"),
                },
            }),
        ],
    ],

    themeConfig:
        /** @type {import("@docusaurus/preset-classic").ThemeConfig} */
        ({
            colorMode: {
                disableSwitch: true,
            },
            navbar: {
                title: "CSharpier",
                logo: {
                    alt: "CSharpier Logo",
                    src: "img/logo.svg",
                },
                items: [
                    {
                        type: "doc",
                        docId: "About",
                        position: "right",
                        label: "Docs",
                    },
                    {
                        href: "https://playground.csharpier.com",
                        label: "Playground",
                        position: "right",
                    },
                    {
                        href: githubUrl,
                        label: "GitHub",
                        position: "right",
                        class: "header-github-link",
                    },
                ],
            },
            footer: {
                links: [
                    {
                        title: "Docs",
                        items: [
                            {
                                label: "About",
                                to: "/docs/About",
                            },
                            {
                                label: "Installation",
                                to: "/docs/Installation",
                            },
                        ],
                    },
                    {
                        title: "Community",
                        items: [
                            {
                                label: "Discord",
                                href: "https://discord.gg/HfAKGEZQcX",
                            },
                            {
                                label: "Stack Overflow",
                                href: "https://stackoverflow.com/questions/tagged/csharpier",
                            },
                        ],
                    },
                    {
                        title: "More",
                        items: [
                            {
                                label: "GitHub",
                                href: githubUrl,
                            },
                            {
                                label: "Issues",
                                href: githubUrl + "/issues",
                            },
                        ],
                    },
                ],
            },
            prism: {
                additionalLanguages: ["csharp"],
                theme: lightCodeTheme,
                darkTheme: darkCodeTheme,
            },
        }),
};

module.exports = config;
