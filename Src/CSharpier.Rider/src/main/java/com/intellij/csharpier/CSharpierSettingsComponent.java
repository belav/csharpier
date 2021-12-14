package com.intellij.csharpier;

import com.intellij.openapi.components.ServiceManager;
import com.intellij.openapi.options.ConfigurationException;
import com.intellij.openapi.options.SearchableConfigurable;
import com.intellij.ui.components.JBLabel;
import com.intellij.util.ui.FormBuilder;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;
import java.awt.*;

public class CSharpierSettingsComponent implements SearchableConfigurable {
    private JCheckBox runOnSaveCheckBox = new JCheckBox("Run on Save");

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
        return CSharpierSettings.getInstance().getRunOnSave() != runOnSaveCheckBox.isSelected();
    }

    @Override
    public void apply() throws ConfigurationException {
        CSharpierSettings settings = CSharpierSettings.getInstance();

        settings.setRunOnSave(runOnSaveCheckBox.isSelected());
    }

    @Override
    public void reset() {
        CSharpierSettings settings = CSharpierSettings.getInstance();
        runOnSaveCheckBox.setSelected(settings.getRunOnSave());
    }
}
