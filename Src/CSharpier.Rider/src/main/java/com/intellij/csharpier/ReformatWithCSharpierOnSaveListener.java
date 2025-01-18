package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileDocumentManagerListener;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.project.ProjectUtil;
import org.jetbrains.annotations.NotNull;

public class ReformatWithCSharpierOnSaveListener implements FileDocumentManagerListener {

    Logger logger = CSharpierLogger.getInstance();

    @Override
    public void beforeDocumentSaving(@NotNull Document document) {
        var project = this.GetProject(document);
        if (project == null) {
            return;
        }

        var cSharpierSettings = CSharpierSettings.getInstance(project);
        if (!cSharpierSettings.getRunOnSave()) {
            return;
        }

        var formattingService = FormattingService.getInstance(project);
        this.logger.debug("beforeDocumentSaving for " + document);
        formattingService.format(document, project);
    }

    private Project GetProject(@NotNull Document document) {
        var virtualFile = FileDocumentManager.getInstance().getFile(document);
        if (virtualFile == null) {
            return null;
        }

        return ProjectUtil.guessProjectForContentFile(virtualFile);
    }
}
