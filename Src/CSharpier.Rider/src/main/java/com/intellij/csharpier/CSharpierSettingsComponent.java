package com.intellij.csharpier;

import com.intellij.openapi.options.SearchableConfigurable;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.ui.ComboBox;
import com.intellij.ui.components.JBCheckBox;
import com.intellij.ui.components.JBLabel;
import com.intellij.ui.components.JBTextField;
import com.intellij.util.ui.FormBuilder;
import java.awt.*;
import java.util.Arrays;
import javax.swing.*;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

public class CSharpierSettingsComponent implements SearchableConfigurable {

    private final ComboItem[] runOnSaveItems = {
        new ComboItem(null, "Use Global Setting"),
        new ComboItem(true, "True"),
        new ComboItem(false, "False"),
    };
    private final Project project;
    private ComboBox<ComboItem> solutionRunOnSaveComboBox = new ComboBox<>(runOnSaveItems);
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
                this.solutionRunOnSaveComboBox,
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

    private Boolean getSelected() {
        return ((ComboItem) this.solutionRunOnSaveComboBox.getSelectedItem()).value;
    }

    @Override
    public boolean isModified() {
        return (
            CSharpierSettings.getInstance(this.project).getRunOnSave() != this.getSelected() ||
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

        settings.setRunOnSave(this.getSelected());
        settings.setCustomPath(this.customPathTextField.getText());
        settings.setDisableCSharpierServer(this.disableCSharpierServerCheckBox.isSelected());
        settings.setUseCustomPath(this.useCustomPath.isSelected());

        CSharpierGlobalSettings.getInstance()
            .setRunOnSave(this.globalRunOnSaveCheckBox.isSelected());
    }

    @Override
    public void reset() {
        var settings = CSharpierSettings.getInstance(this.project);

        var index = -1;
        for (var i = 0; i < runOnSaveItems.length; i++) {
            if (runOnSaveItems[i].value == settings.getRunOnSave()) {
                index = i;
                break;
            }
        }

        this.solutionRunOnSaveComboBox.setSelectedIndex(index);
        this.globalRunOnSaveCheckBox.setSelected(
                CSharpierGlobalSettings.getInstance().getRunOnSave()
            );
        this.useCustomPath.setSelected(settings.getUseCustomPath());
        this.customPathTextField.setText(settings.getCustomPath());
        this.disableCSharpierServerCheckBox.setSelected(settings.getDisableCSharpierServer());
    }

    public record ComboItem(Boolean value, String label) {
        @Override
        public String toString() {
            return label;
        }
    }
}
