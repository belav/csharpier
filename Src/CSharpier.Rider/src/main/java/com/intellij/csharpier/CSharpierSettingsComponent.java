package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.options.SearchableConfigurable;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.ui.ComboBox;
import com.intellij.ui.components.JBCheckBox;
import com.intellij.ui.components.JBLabel;
import com.intellij.ui.components.JBTextField;
import com.intellij.util.ui.FormBuilder;
import java.awt.*;
import javax.swing.*;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

public class CSharpierSettingsComponent implements SearchableConfigurable {

    Logger logger = CSharpierLogger.getInstance();

    private final Project project;
    private final String[] runOnSaveOptions = new String[] {
        "Use Global Setting",
        "False",
        "True",
    };
    private ComboBox<String> runOnSaveComboBox = new ComboBox<String>(runOnSaveOptions);
    private JBCheckBox globalRunOnSaveCheckBox = new JBCheckBox("Run on Save (Global)");
    private JBCheckBox disableCSharpierServerCheckBox = new JBCheckBox("Disable CSharpier Server");
    private JBCheckBox useCustomPath = new JBCheckBox("Override CSharpier Executable");
    private JBTextField customPathTextField = new JBTextField();

    public CSharpierSettingsComponent(@NotNull Project project) {
        this.project = project;
    }

    @NotNull
    @Override
    public String getId() {
        return "CSharpier";
    }

    @Nls
    @Override
    public String getDisplayName() {
        return "CSharpier";
    }

    private JComponent createSectionHeader(String label) {
        var panel = new JPanel(new GridBagLayout());
        var gbc = new GridBagConstraints();
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.anchor = GridBagConstraints.WEST;

        panel.add(new JBLabel(label), gbc);

        gbc = new GridBagConstraints();
        gbc.gridy = 0;
        gbc.gridx = 1;
        gbc.weightx = 1.0;
        gbc.insets = new Insets(1, 6, 0, 0);
        gbc.fill = GridBagConstraints.HORIZONTAL;

        var separator = new JSeparator(SwingConstants.HORIZONTAL);
        panel.add(separator, gbc);

        return panel;
    }

    @Override
    public @Nullable JComponent createComponent() {
        var leftIndent = 20;
        var topInset = 10;

        return FormBuilder.createFormBuilder()
            .addComponent(createSectionHeader("General Settings"))
            .setFormLeftIndent(leftIndent)
            .addLabeledComponent(
                new JBLabel("Run on save (Solution):"),
                this.runOnSaveComboBox,
                topInset,
                false
            )
            .addComponent(this.globalRunOnSaveCheckBox, topInset)
            .setFormLeftIndent(0)
            .addComponent(createSectionHeader("Developer Settings"), 20)
            .setFormLeftIndent(leftIndent)
            .addComponent(this.useCustomPath, topInset)
            .addLabeledComponent(
                new JBLabel("Directory of custom CSharpier executable:"),
                this.customPathTextField,
                topInset,
                false
            )
            .addComponent(this.disableCSharpierServerCheckBox, topInset)
            .addComponentFillVertically(new JPanel(), 0)
            .getPanel();
    }

    @Override
    public boolean isModified() {
        return (
            CSharpierSettings.getInstance(this.project).getSolutionRunOnSave() !=
                this.toRunOnSaveOption() ||
            CSharpierGlobalSettings.getInstance(this.project).getRunOnSave() !=
            this.globalRunOnSaveCheckBox.isSelected() ||
            CSharpierSettings.getInstance(this.project).getCustomPath() !=
            this.customPathTextField.getText() ||
            CSharpierSettings.getInstance(this.project).getUseCustomPath() !=
            this.useCustomPath.isSelected() ||
            CSharpierSettings.getInstance(this.project).getDisableCSharpierServer() !=
            this.disableCSharpierServerCheckBox.isSelected()
        );
    }

    @Override
    public void apply() {
        var settings = CSharpierSettings.getInstance(this.project);

        settings.setSolutionRunOnSave(this.toRunOnSaveOption());
        settings.setCustomPath(this.customPathTextField.getText());
        settings.setDisableCSharpierServer(this.disableCSharpierServerCheckBox.isSelected());
        settings.setUseCustomPath(this.useCustomPath.isSelected());

        var globalSettings = CSharpierGlobalSettings.getInstance(this.project);
        globalSettings.setRunOnSave(this.globalRunOnSaveCheckBox.isSelected());
    }

    @Override
    public void reset() {
        var settings = CSharpierSettings.getInstance(this.project);
        this.setSelectedRunOnSave(settings.getSolutionRunOnSave());
        this.useCustomPath.setSelected(settings.getUseCustomPath());
        this.customPathTextField.setText(settings.getCustomPath());
        this.disableCSharpierServerCheckBox.setSelected(settings.getDisableCSharpierServer());

        var globalSettings = CSharpierGlobalSettings.getInstance(this.project);
        this.globalRunOnSaveCheckBox.setSelected(globalSettings.getRunOnSave());
    }

    private SolutionRunOnSaveOption toRunOnSaveOption() {
        String selectedItem = (String) runOnSaveComboBox.getSelectedItem();

        if (selectedItem == runOnSaveOptions[0]) {
            return SolutionRunOnSaveOption.UseGlobalSetting;
        } else if (selectedItem == runOnSaveOptions[1]) {
            return SolutionRunOnSaveOption.False;
        } else if (selectedItem == runOnSaveOptions[2]) {
            return SolutionRunOnSaveOption.True;
        }

        this.logger.debug("invalid runOnSaveComboBox selection: " + selectedItem);
        return SolutionRunOnSaveOption.UseGlobalSetting;
    }

    private void setSelectedRunOnSave(SolutionRunOnSaveOption saveOption) {
        if (saveOption == SolutionRunOnSaveOption.UseGlobalSetting) {
            this.runOnSaveComboBox.setSelectedItem(runOnSaveOptions[0]);
        } else if (saveOption == SolutionRunOnSaveOption.False) {
            this.runOnSaveComboBox.setSelectedItem(runOnSaveOptions[1]);
        } else if (saveOption == SolutionRunOnSaveOption.True) {
            this.runOnSaveComboBox.setSelectedItem(runOnSaveOptions[2]);
        }

        this.logger.debug("tried to set invalid SolutionRunOnSaveOption: " + saveOption);
    }
}
