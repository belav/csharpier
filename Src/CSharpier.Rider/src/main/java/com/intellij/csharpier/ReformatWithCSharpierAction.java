package com.intellij.csharpier;

import com.intellij.openapi.actionSystem.AnAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.actionSystem.CommonDataKeys;
import com.intellij.openapi.actionSystem.PlatformDataKeys;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.editor.Editor;
import com.intellij.openapi.project.Project;
import org.jetbrains.annotations.NotNull;

import java.util.Locale;

public class ReformatWithCSharpierAction extends AnAction {
    Logger logger = CSharpierLogger.getInstance();

    @Override
    public void actionPerformed(@NotNull AnActionEvent e) {
        this.logger.info("Running ReformatWithCSharpierAction");
        Project project = e.getProject();
        if (project == null) {
            return;
        }
        Editor editor = e.getData(CommonDataKeys.EDITOR);
        if (editor != null) {
            processFileInEditor(project, editor.getDocument());
        }
    }

    @Override
    public void update(@NotNull AnActionEvent e) {
        var virtualFile = e.getData(PlatformDataKeys.VIRTUAL_FILE).toString();
        var filePrefix = "file://";
        if (!virtualFile.startsWith(filePrefix)) {
            this.logger.debug("VIRTUAL_FILE did not start with file://, was: " + virtualFile);
            e.getPresentation().setVisible(false);
            return;
        }

        String file = virtualFile.substring(filePrefix.length());
        e.getPresentation().setVisible(file.toLowerCase().endsWith(".cs"));
        boolean canFormat = FormattingService.getInstance(e.getProject()).getCanFormat(file, e.getProject());
        e.getPresentation().setEnabled(canFormat);
    }

    private static void processFileInEditor(@NotNull Project project, @NotNull Document document) {
        FormattingService formattingService = FormattingService.getInstance(project);
        formattingService.format(document, project);
    }
}