package com.intellij.csharpier;

import com.intellij.ide.actionsOnSave.impl.ActionsOnSaveFileDocumentManagerListener;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.project.Project;
import org.jetbrains.annotations.NotNull;

public class ReformatWithCSharpierOnSave2 extends ActionsOnSaveFileDocumentManagerListener.ActionOnSave {
    Logger logger = CSharpierLogger.getInstance();

    @Override
    public boolean isEnabledForProject(@NotNull Project project) {
        // TODO does this runOnSave setting go away?
        return CSharpierSettings.getInstance(project).getRunOnSave();
    }

    @Override
    public void processDocuments(@NotNull Project project, @NotNull Document @NotNull [] documents) {
        if (!this.isEnabledForProject(project)) {
            return;
        }

        var formattingService = FormattingService.getInstance(project);
        for (var document : documents) {
            formattingService.format(document, project);
        }
    }
}
