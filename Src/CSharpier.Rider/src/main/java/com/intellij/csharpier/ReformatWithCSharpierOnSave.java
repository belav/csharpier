package com.intellij.csharpier;

import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileDocumentManagerListener;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.project.ProjectLocator;
import com.intellij.openapi.vfs.VirtualFile;
import org.jetbrains.annotations.NotNull;

public class ReformatWithCSharpierOnSave implements FileDocumentManagerListener {
    Logger logger = CSharpierLogger.getInstance();

    @Override
    public void beforeDocumentSaving(@NotNull Document document) {
        var file = FileDocumentManager.getInstance().getFile(document);
        if (file == null) {
            return;
        }
        var project = ProjectLocator.getInstance().guessProjectForFile(file);
        if (project == null) {
            this.logger.info("Could not find project for file so not trying to format in save.");
            return;
        }

        var cSharpierSettings = CSharpierSettings.getInstance(project);
        if (!cSharpierSettings.getRunOnSave()) {
            return;
        }

        this.logger.info("Running ReformatWithCSharpierOnSave");

        var formattingService = FormattingService.getInstance(project);
        formattingService.format(document, project);
    }
}
