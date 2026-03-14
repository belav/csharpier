package com.intellij.csharpier;

import com.intellij.openapi.application.ApplicationManager;
import com.intellij.openapi.components.PersistentStateComponent;
import com.intellij.openapi.components.State;
import com.intellij.openapi.components.Storage;
import com.intellij.util.xmlb.XmlSerializerUtil;
import org.jetbrains.annotations.NotNull;

@State(
    name = "com.intellij.csharpier.global",
    storages = @Storage("$APP_CONFIG$/CSharpierPlugin.xml")
)
public class CSharpierGlobalSettings implements PersistentStateComponent<CSharpierGlobalSettings> {

    @NotNull
    public static CSharpierGlobalSettings getInstance() {
        return ApplicationManager.getApplication().getService(CSharpierGlobalSettings.class);
    }

    private boolean runOnSave;

    public boolean getRunOnSave() {
        return this.runOnSave;
    }

    public void setRunOnSave(boolean value) {
        this.runOnSave = value;
    }

    @Override
    public CSharpierGlobalSettings getState() {
        return this;
    }

    @Override
    public void loadState(@NotNull CSharpierGlobalSettings state) {
        XmlSerializerUtil.copyBean(state, this);
    }
}
