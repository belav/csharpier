package com.intellij.csharpier;

import com.intellij.openapi.components.PersistentStateComponent;
import com.intellij.openapi.components.State;
import com.intellij.openapi.components.Storage;
import com.intellij.openapi.project.Project;
import com.intellij.util.xmlb.XmlSerializerUtil;
import org.jetbrains.annotations.NotNull;

@State(
  name = "com.intellij.csharpier",
  storages = @Storage("CSharpierPlugin.xml")
)
public class CSharpierSettings
  implements PersistentStateComponent<CSharpierSettings> {

  @NotNull
  static CSharpierSettings getInstance(@NotNull Project project) {
    return project.getService(CSharpierSettings.class);
  }

  private boolean runOnSave;

  public boolean getRunOnSave() {
    return this.runOnSave;
  }

  public void setRunOnSave(boolean value) {
    this.runOnSave = value;
  }

  private String customPath;

  public String getCustomPath() {
    return this.customPath;
  }

  public void setCustomPath(String value) {
    this.customPath = value;
  }

  private boolean disableCSharpierServer;

  public boolean getDisableCSharpierServer() {
    return this.disableCSharpierServer;
  }

  public void setDisableCSharpierServer(boolean value) {
    this.disableCSharpierServer = value;
  }

  @Override
  public CSharpierSettings getState() {
    return this;
  }

  @Override
  public void loadState(@NotNull CSharpierSettings state) {
    XmlSerializerUtil.copyBean(state, this);
  }
}
