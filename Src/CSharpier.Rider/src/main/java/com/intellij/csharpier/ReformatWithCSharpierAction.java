package com.intellij.csharpier;

import com.intellij.openapi.actionSystem.AnAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.actionSystem.CommonDataKeys;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.editor.Editor;
import com.intellij.openapi.project.Project;
import org.jetbrains.annotations.NotNull;

public class ReformatWithCSharpierAction extends AnAction {
    Logger LOG = Logger.getInstance(ReformatWithCSharpierAction.class);

    @Override
    public void actionPerformed(@NotNull AnActionEvent e) {
        LOG.info("Running ReformatWithCSharpierAction");
        Project project = e.getProject();
        if (project == null) {
            return;
        }
        Editor editor = e.getData(CommonDataKeys.EDITOR);
        if (editor != null) {
            processFileInEditor(project, editor.getDocument());
        }
    }

    private static void processFileInEditor(@NotNull Project project, @NotNull Document document) {
        FormattingService formattingService = FormattingService.getInstance(project);
        formattingService.format(document, project);
    }
}