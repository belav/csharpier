// @ts-check

/** @type {import('@docusaurus/plugin-content-docs').SidebarsConfig} */
const sidebars = {
  docs: [
      {
          type: "category",
          label: "About",
          collapsible: false,
          items: ["About"]
      },
      {
          type: "category",
          label: "Usage",
          collapsible: false,
          items: ["Installation", "Configuration", "Ignore", "CLI", "API", "MsBuild", "Pre-commit", "ContinuousIntegration", "IntegratingWithLinters"]
      },
      {
          type: "category",
          label: "Editors",
          collapsible: false,
          items: ["Editors", "EditorsTroubleshooting"]
      }
  ],
};

module.exports = sidebars;
