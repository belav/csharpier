package com.intellij.csharpier;

import com.intellij.AppTopics;
import com.intellij.openapi.application.ApplicationManager;
import com.intellij.openapi.application.ModalityState;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileDocumentManagerListener;
import com.intellij.openapi.project.Project;
import com.intellij.psi.PsiDocumentManager;
import com.intellij.util.messages.MessageBus;
import com.intellij.util.messages.MessageBusConnection;
import org.jetbrains.annotations.NotNull;

import java.util.Arrays;
import java.util.HashSet;
import java.util.Set;

// TODO does the xaml styler run twice too??
public class ReformatWithCSharpierOnSave4 implements FileDocumentManagerListener {
    private final Logger logger = CSharpierLogger.getInstance();

    private final Project project;
    private final PsiDocumentManager psiDocumentManager;
    private final FormattingService formattingService;
    private MessageBusConnection messageBus = ApplicationManager.getApplication().getMessageBus().connect();

    private final Set<Document> documentsToProcess = new HashSet<>();

    public ReformatWithCSharpierOnSave4(Project project) {
        this.project = project;
        this.psiDocumentManager = PsiDocumentManager.getInstance(project);
        this.formattingService = FormattingService.getInstance(project);

        messageBus.subscribe(AppTopics.FILE_DOCUMENT_SYNC, this);
    }

    // TODO this does not get called while in rider until you leave the IDE
    // with the regular way to do it, or the way to inject projects
    // could it be a bug with rider?
    @Override
    public void beforeAllDocumentsSaving() {
        this.logger.debug("ReformatWithCSharpierOnSave4 beforeAllDocumentsSaving");
    }

    @Override
    public void beforeDocumentSaving(@NotNull Document document) {
        this.logger.debug("ReformatWithCSharpierOnSave4 beforeDocumentSaving");

        var file = FileDocumentManager.getInstance().getFile(document);
        if (file == null) {
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
