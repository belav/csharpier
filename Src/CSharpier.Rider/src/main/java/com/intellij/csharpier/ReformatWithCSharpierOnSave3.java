package com.intellij.csharpier;

import com.intellij.openapi.application.ApplicationManager;
import com.intellij.openapi.application.ModalityState;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileDocumentManagerListener;
import com.intellij.openapi.project.ProjectLocator;
import org.jetbrains.annotations.NotNull;

// TODO read up on the slack channel stuff
// TODO optionally, figure out a way to not hit format twice.
// TODO but more importantly, get out a VS update

public class ReformatWithCSharpierOnSave3 implements FileDocumentManagerListener {
    private final Logger logger = CSharpierLogger.getInstance();

    @Override
    public void beforeAllDocumentsSaving() {
        this.logger.debug("beforeAllDocumentsSaving");
    }

    // TODO if this works, switch to the version that injects the project
    @Override
    public void beforeDocumentSaving(@NotNull Document document) {
        this.logger.debug("ReformatWithCSharpierOnSave3 beforeDocumentSaving");

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

        ApplicationManager.getApplication().invokeLater(() -> {
            formattingService.format(document, project);
        }, ModalityState.NON_MODAL);
    }
}
