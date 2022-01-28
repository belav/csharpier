package com.intellij.csharpier;

import com.intellij.ide.actionsOnSave.impl.ActionsOnSaveFileDocumentManagerListener;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.project.Project;
import org.jetbrains.annotations.NotNull;

import java.util.HashSet;
import java.util.Set;

// TODO maybe https://plugins.jetbrains.com/plugin/16815-thread-access-info
public class ReformatWithCSharpierOnSave2 extends ActionsOnSaveFileDocumentManagerListener.ActionOnSave {
    Logger logger = CSharpierLogger.getInstance();

    @Override
    public boolean isEnabledForProject(@NotNull Project project) {
        var runOnSave = CSharpierSettings.getInstance(project).getRunOnSave();
        this.logger.debug("isEnabledForProject - " + runOnSave);
        // TODO does this runOnSave setting go away?
        return runOnSave;
    }

    @Override
    public void processDocuments(@NotNull Project project, @NotNull Document @NotNull [] documents) {
        this.logger.debug("processDocuments");
        if (!this.isEnabledForProject(project)) {
            return;
        }

        var formattingService = FormattingService.getInstance(project);
        for (var document : documents) {
            formattingService.format(document, project);
        }
    }
}
