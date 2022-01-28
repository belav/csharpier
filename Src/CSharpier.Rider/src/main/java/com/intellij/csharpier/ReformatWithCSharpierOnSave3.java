package com.intellij.csharpier;

import com.intellij.ide.actions.SaveAllAction;
import com.intellij.ide.actions.SaveDocumentAction;
import com.intellij.openapi.application.ApplicationManager;
import com.intellij.openapi.application.ModalityState;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileDocumentManagerListener;
import com.intellij.openapi.project.ProjectLocator;
import org.jetbrains.annotations.NotNull;

// TODO try the XamlStlerComponent thing
/*
I have a FileDocumentManagerListener implemented in my rider plugin.
When I launch it into Rider to test, beforeDocumentSaving fires any time I save a file. Along with beforeAllDocumentsSaving
When I build the plugin, and install it in Rider, beforeDocumentSaving fires, but beforeAllDocumentsSaving only fires when rider loses focus.
Probably related, if I implement an ActionsOnSaveFileDocumentManagerListener.ActionOnSave, it works when launching rider to test the plugin, but installing the built plugin in rider it does not work.
Is there some reason rider is behaving differently when I launch it directly vs launching it via run plugin?

And maybe a simpler question, is there a recommended way in rider to modify a file on save? I am digging into all of this because a "move to new class" refactor causes the following exception "Someone is intentionally modifying the document while syncing from backend."
When I modify the document, I use WriteCommandAction.runWriteCommandAction but still get the exception.
If I run all of my formatting logic in ApplicationManager.getApplication().invokeLater then I avoid the exception, but the built plugin ends up formatting it twice, because beforeDocumentSaving will get fired twice. But it will only fire once when I am testing the plugin through intellij.
 */

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
