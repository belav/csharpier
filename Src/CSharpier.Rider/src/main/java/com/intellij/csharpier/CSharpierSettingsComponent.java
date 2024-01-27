package com.intellij.csharpier;

import com.intellij.openapi.options.ConfigurationException;
import com.intellij.openapi.options.SearchableConfigurable;
import com.intellij.openapi.project.Project;
import com.intellij.util.ui.FormBuilder;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;

// TODO clean up the display of this
// TODO double check and release 0.27.2 with this + csx formatting
// TODO check on linux
public class CSharpierSettingsComponent implements SearchableConfigurable {
    private final Project project;
    private JCheckBox runOnSaveCheckBox = new JCheckBox("Run on Save");
    private JCheckBox useServerCheckBox = new JCheckBox("Use CSharpier Server - experimental support as of 0.27.2");
    private JTextField customPathTextField = new JTextField();

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
        return FormBuilder.createFormBuilder()
                .addComponent(this.runOnSaveCheckBox)
                .addComponent(new JLabel("Path to directory containing dotnet-csharpier - used for testing the extension with new versions of csharpier."))
                .addComponent(this.customPathTextField)
                .addComponent(this.useServerCheckBox)
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
