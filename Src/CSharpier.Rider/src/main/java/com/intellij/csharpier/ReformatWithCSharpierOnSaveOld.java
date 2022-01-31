package com.intellij.csharpier;

import com.intellij.openapi.application.ApplicationManager;
import com.intellij.openapi.application.ModalityState;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.fileEditor.FileDocumentManagerListener;
import com.intellij.openapi.project.Project;
import com.intellij.psi.PsiDocumentManager;
import org.jetbrains.annotations.NotNull;

import java.util.Arrays;
import java.util.HashSet;
import java.util.Set;

public class ReformatWithCSharpierOnSaveOld implements FileDocumentManagerListener {
    private final Logger logger = CSharpierLogger.getInstance();

    private final Project project;
    private final PsiDocumentManager psiDocumentManager;
    private final FormattingService formattingService;

    private final Set<Document> documentsToProcess = new HashSet<>();

    public ReformatWithCSharpierOnSaveOld(Project project) {
        this.project = project;
        this.psiDocumentManager = PsiDocumentManager.getInstance(project);
        this.formattingService = FormattingService.getInstance(project);
    }

    // TODO this does not get called while in rider until you leave the IDE
    // with the regular way to do it, or the way to inject projects
    // could it be a bug with rider?
    @Override
    public void beforeAllDocumentsSaving() {
        this.logger.debug("beforeAllDocumentsSaving");
//        if (!CSharpierSettings.getInstance(project).getRunOnSave()) {
//            this.logger.debug("runOnSaveNotEnabled");
//            return;
//        }
//
////         this.logger.debug("bailing because this doesn't seem to work in rider");
//        var unsavedDocuments = FileDocumentManager.getInstance().getUnsavedDocuments();
//        this.scheduleDocumentsProcessing(unsavedDocuments);
    }

    private void scheduleDocumentsProcessing(Document ... documents) {
        var processingAlreadyScheduled = !documentsToProcess.isEmpty();

        this.documentsToProcess.addAll(Arrays.asList(documents));

        if (!processingAlreadyScheduled) {
            ApplicationManager.getApplication().invokeLater(() -> processSavedDocuments(), ModalityState.NON_MODAL);
        }
    }

    private void processSavedDocuments() {
        var documents = this.documentsToProcess.toArray(Document.EMPTY_ARRAY);
        this.documentsToProcess.clear();

        // Although invokeLater() is called with ModalityState.NON_MODAL argument, somehow this might be called in modal context (for example on Commit File action)
        // It's quite weird if save action progress appears or documents get changed in modal context, let's ignore the request.
        if (ModalityState.current() != ModalityState.NON_MODAL) {
            return;
        }

        var manager = FileDocumentManager.getInstance();

        for (var document : documents) {
            this.formattingService.format(document, this.project);
            //manager.saveDocument(document);
        }
    }

    // TODO if this is used, then we get a double format
    @Override
    public void beforeDocumentSaving(@NotNull Document document) {
        this.logger.debug("beforeDocumentSaving");
//        if (!CSharpierSettings.getInstance(project).getRunOnSave()) {
//            this.logger.debug("runOnSaveNotEnabled");
//            return;
//        }
//        this.scheduleDocumentsProcessing(document);

//        var project = ProjectLocator.getInstance().guessProjectForFile(file);
//        if (project == null) {
//            this.logger.info("Could not find project for file so not trying to format in save.");
//            return;
//        }
//
//        var cSharpierSettings = CSharpierSettings.getInstance(project);
//        if (!cSharpierSettings.getRunOnSave()) {
//            return;
//        }
//
//        this.logger.info("Running ReformatWithCSharpierOnSave");
//
//        var formattingService = FormattingService.getInstance(project);
//        formattingService.format(document, project);
    }
}
