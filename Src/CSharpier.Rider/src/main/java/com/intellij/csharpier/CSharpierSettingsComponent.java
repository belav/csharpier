package com.intellij.csharpier;

import com.intellij.openapi.options.ConfigurationException;
import com.intellij.openapi.options.SearchableConfigurable;
import com.intellij.openapi.project.Project;
import com.intellij.ui.components.JBCheckBox;
import com.intellij.ui.components.JBLabel;
import com.intellij.ui.components.JBTextField;
import com.intellij.util.ui.FormBuilder;
import org.jdesktop.swingx.JXHeader;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;

// TODO clean up the display of this
public class CSharpierSettingsComponent implements SearchableConfigurable {
    private final Project project;
    private JBCheckBox runOnSaveCheckBox = new JBCheckBox("Run on Save");
    private JBCheckBox useServerCheckBox = new JBCheckBox("Use CSharpier Server - experimental support as of 0.27.2");
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

    @Override
    public @Nullable JComponent createComponent() {
        var leftIndent = 20;
        var topInset = 10;

        return FormBuilder.createFormBuilder()
                .addLabeledComponent(new JBLabel("General Settings"), new JSeparator())
                .setFormLeftIndent(leftIndent)
                .addComponent(this.runOnSaveCheckBox, topInset)
                .setFormLeftIndent(0)
                .addLabeledComponent(new JBLabel("Developer Settings"), new JSeparator(), 20)
                .setFormLeftIndent(leftIndent)
                .addLabeledComponent(new JBLabel("Directory of custom dotnet-csharpier:"), this.customPathTextField, topInset, false)
                .addComponent(this.useServerCheckBox, topInset)
                .addComponentFillVertically(new JPanel(), 0)
                .getPanel();
    }

    @Override
    public boolean isModified() {
        return CSharpierSettings.getInstance(this.project).getRunOnSave() != this.runOnSaveCheckBox.isSelected()
                || CSharpierSettings.getInstance(this.project).getUseServer() != this.useServerCheckBox.isSelected()
                || CSharpierSettings.getInstance(this.project).getCustomPath() != this.customPathTextField.getText();
    }

    @Override
    public void apply() throws ConfigurationException {
        var settings = CSharpierSettings.getInstance(this.project);

        settings.setRunOnSave(this.runOnSaveCheckBox.isSelected());
        settings.setCustomPath(this.customPathTextField.getText());
        settings.setUseServer(this.useServerCheckBox.isSelected());
    }

    @Override
    public void reset() {
        var settings = CSharpierSettings.getInstance(this.project);
        this.runOnSaveCheckBox.setSelected(settings.getRunOnSave());
        this.useServerCheckBox.setSelected(settings.getUseServer());
        this.customPathTextField.setText(settings.getCustomPath());
    }
}
