package com.intellij.csharpier;

import com.intellij.openapi.options.ConfigurationException;
import com.intellij.openapi.options.SearchableConfigurable;
import com.intellij.openapi.project.Project;
import com.intellij.ui.components.JBCheckBox;
import com.intellij.ui.components.JBLabel;
import com.intellij.ui.components.JBTextField;
import com.intellij.util.ui.FormBuilder;
import java.awt.*;
import javax.swing.*;
import javax.swing.border.EmptyBorder;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

public class CSharpierSettingsComponent implements SearchableConfigurable {

  private final Project project;
  private JBCheckBox runOnSaveCheckBox = new JBCheckBox("Run on Save");
  private JBCheckBox disableCSharpierServerCheckBox = new JBCheckBox("Disable CSharpier Server");
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
      .addComponent(this.runOnSaveCheckBox, topInset)
      .setFormLeftIndent(0)
      .addComponent(createSectionHeader("Developer Settings"), 20)
      .setFormLeftIndent(leftIndent)
      .addLabeledComponent(
        new JBLabel("Directory of custom dotnet-csharpier:"),
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
      CSharpierSettings.getInstance(this.project).getRunOnSave() !=
        this.runOnSaveCheckBox.isSelected() ||
      CSharpierSettings.getInstance(this.project).getCustomPath() !=
      this.customPathTextField.getText() ||
      CSharpierSettings.getInstance(this.project).getDisableCSharpierServer() !=
      this.disableCSharpierServerCheckBox.isSelected()
    );
  }

  @Override
  public void apply() {
    var settings = CSharpierSettings.getInstance(this.project);

    settings.setRunOnSave(this.runOnSaveCheckBox.isSelected());
    settings.setCustomPath(this.customPathTextField.getText());
    settings.setDisableCSharpierServer(this.disableCSharpierServerCheckBox.isSelected());
  }

  @Override
  public void reset() {
    var settings = CSharpierSettings.getInstance(this.project);
    this.runOnSaveCheckBox.setSelected(settings.getRunOnSave());
    this.customPathTextField.setText(settings.getCustomPath());
    this.disableCSharpierServerCheckBox.setSelected(settings.getDisableCSharpierServer());
  }
}
