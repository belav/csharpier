package com.intellij.csharpier;

import com.intellij.openapi.options.ConfigurationException;
import com.intellij.openapi.options.SearchableConfigurable;
import com.intellij.openapi.project.Project;
import com.intellij.util.ui.FormBuilder;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;

public class CSharpierSettingsComponent implements SearchableConfigurable {
    private final Project project;
    private JCheckBox runOnSaveCheckBox = new JCheckBox("Run on Save");
    private JTextField customPathTextField = new JTextField("Path to directory containing dotnet-csharpier - used for testing the extension with new versions of csharpier.");

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
                .addComponent(this.customPathTextField)
                .addComponentFillVertically(new JPanel(), 0)
                .getPanel();
    }

    @Override
    public boolean isModified() {
        return CSharpierSettings.getInstance(this.project).getRunOnSave() != this.runOnSaveCheckBox.isSelected()
                || CSharpierSettings.getInstance(this.project).getCustomPath() != this.customPathTextField.getText();
    }

    @Override
    public void apply() throws ConfigurationException {
        var settings = CSharpierSettings.getInstance(this.project);

        settings.setRunOnSave(this.runOnSaveCheckBox.isSelected());
        settings.setCustomPath(this.customPathTextField.getText());
    }

    @Override
    public void reset() {
        var settings = CSharpierSettings.getInstance(this.project);
        this.runOnSaveCheckBox.setSelected(settings.getRunOnSave());
        this.customPathTextField.setText(settings.getCustomPath());
    }
}
