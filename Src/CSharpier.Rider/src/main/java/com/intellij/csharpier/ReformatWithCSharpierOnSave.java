package com.intellij.csharpier;

import com.intellij.ide.actions.SaveAllAction;
import com.intellij.ide.actions.SaveDocumentAction;
import com.intellij.openapi.actionSystem.AnAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.actionSystem.CommonDataKeys;
import com.intellij.openapi.actionSystem.ex.AnActionListener;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import org.jetbrains.annotations.NotNull;

public class ReformatWithCSharpierOnSave implements AnActionListener {

    private final Logger logger = CSharpierLogger.getInstance();

    @Override
    public void beforeActionPerformed(@NotNull AnAction action, @NotNull AnActionEvent event) {
        if (action instanceof SaveDocumentAction) {
            var project = event.getData(CommonDataKeys.PROJECT);
            var cSharpierSettings = CSharpierSettings.getInstance(project);
            if (!cSharpierSettings.getRunOnSave()) {
                return;
            }
            var editor = event.getData(CommonDataKeys.EDITOR);
            if (editor != null) {
                var formattingService = FormattingService.getInstance(project);
                var document = editor.getDocument();
                this.logger.debug("SaveDocumentAction for " + document);
                formattingService.format(document, project);
            }
        } else if (action instanceof SaveAllAction) {
            var project = event.getData(CommonDataKeys.PROJECT);
            var cSharpierSettings = CSharpierSettings.getInstance(project);
            if (!cSharpierSettings.getRunOnSave()) {
                return;
            }
            var unsavedDocuments = FileDocumentManager.getInstance().getUnsavedDocuments();
            var formattingService = FormattingService.getInstance(project);
            this.logger.debug("SaveAllAction for " + unsavedDocuments.length + " Documents");
            for (var document : unsavedDocuments) {
                formattingService.format(document, project);
            }
        }
    }
}
