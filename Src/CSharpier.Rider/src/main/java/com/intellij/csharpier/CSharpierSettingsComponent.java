package com.intellij.csharpier;

import com.intellij.openapi.options.SearchableConfigurable;
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

    private JBCheckBox runOnSaveCheckBox = new JBCheckBox("Run on Save");
    private JBCheckBox disableCSharpierServerCheckBox = new JBCheckBox("Disable CSharpier Server");
    private JBCheckBox useCustomPath = new JBCheckBox("Override CSharpier Executable");
    private JBTextField customPathTextField = new JBTextField();


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
            .addComponent(this.runOnSaveCheckBox, topInset)
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
            CSharpierSettings.getInstance().getRunOnSave() !=
                this.runOnSaveCheckBox.isSelected() ||
            CSharpierSettings.getInstance().getCustomPath() !=
            this.customPathTextField.getText() ||
            CSharpierSettings.getInstance().getUseCustomPath() !=
            this.useCustomPath.isSelected() ||
            CSharpierSettings.getInstance().getDisableCSharpierServer() !=
            this.disableCSharpierServerCheckBox.isSelected()
        );
    }

    @Override
    public void apply() {
        var settings = CSharpierSettings.getInstance();

        settings.setRunOnSave(this.runOnSaveCheckBox.isSelected());
        settings.setCustomPath(this.customPathTextField.getText());
        settings.setDisableCSharpierServer(this.disableCSharpierServerCheckBox.isSelected());
        settings.setUseCustomPath(this.useCustomPath.isSelected());
    }

    @Override
    public void reset() {
        var settings = CSharpierSettings.getInstance();
        this.runOnSaveCheckBox.setSelected(settings.getRunOnSave());
        this.useCustomPath.setSelected(settings.getUseCustomPath());
        this.customPathTextField.setText(settings.getCustomPath());
        this.disableCSharpierServerCheckBox.setSelected(settings.getDisableCSharpierServer());
    }
}
