package com.intellij.csharpier;

import com.intellij.openapi.components.ServiceManager;
import com.intellij.openapi.options.ConfigurationException;
import com.intellij.openapi.options.SearchableConfigurable;
import com.intellij.openapi.project.Project;
import com.intellij.ui.components.JBLabel;
import com.intellij.util.ui.FormBuilder;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;

public class CSharpierSettingsComponent implements SearchableConfigurable {
    private final Project project;
    private JCheckBox runOnSaveCheckBox = new JCheckBox("Run on Save");

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
                .addComponent(runOnSaveCheckBox)
                .addComponentFillVertically(new JPanel(), 0)
                .getPanel();
    }

    @Override
    public boolean isModified() {
        return CSharpierSettings.getInstance(this.project).getRunOnSave() != runOnSaveCheckBox.isSelected();
    }

    @Override
    public void apply() throws ConfigurationException {
        CSharpierSettings settings = CSharpierSettings.getInstance(this.project);

        settings.setRunOnSave(runOnSaveCheckBox.isSelected());
    }

    @Override
    public void reset() {
        CSharpierSettings settings = CSharpierSettings.getInstance(this.project);
        runOnSaveCheckBox.setSelected(settings.getRunOnSave());
    }
}
