package com.intellij.csharpier;

import com.intellij.openapi.components.PersistentStateComponent;
import com.intellij.openapi.components.ServiceManager;
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
        return runOnSave;
    }

    public void setRunOnSave(boolean runOnSave) {
        this.runOnSave = runOnSave;
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
