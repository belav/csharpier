package com.intellij.csharpier;

import com.intellij.openapi.actionSystem.*;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.project.Project;
import org.jetbrains.annotations.NotNull;

public class ReformatWithCSharpierAction extends AnAction {

    Logger logger = CSharpierLogger.getInstance();

    @Override
    public @NotNull ActionUpdateThread getActionUpdateThread() {
        return ActionUpdateThread.BGT;
    }

    @Override
    public void actionPerformed(@NotNull AnActionEvent e) {
        this.logger.info("Running ReformatWithCSharpierAction");
        var project = e.getProject();
        if (project == null) {
            return;
        }
        var editor = e.getData(CommonDataKeys.EDITOR);
        if (editor != null) {
            processFileInEditor(project, editor.getDocument());
        }
    }

    @Override
    public void update(@NotNull AnActionEvent e) {
        var virtualFile = e.getData(PlatformDataKeys.VIRTUAL_FILE);

        if (
            virtualFile == null ||
            // this update can get hit before CSharpierStartup is done
            // so bail early if we didn't find dotnet yet, it should be there when startup is done
            !DotNetProvider.getInstance(e.getProject()).foundDotNet()
        ) {
            e.getPresentation().setVisible(false);
            return;
        }
        var virtualFileString = virtualFile.toString();
        var filePrefix = "file://";
        if (!virtualFileString.startsWith(filePrefix)) {
            this.logger.debug("VIRTUAL_FILE did not start with file://, was: " + virtualFileString);
            e.getPresentation().setVisible(false);
            return;
        }

        var file = virtualFileString.substring(filePrefix.length());
        var isCSharpFile = file.toLowerCase().endsWith(".cs");
        e.getPresentation().setVisible(isCSharpFile);
        var canFormat =
            isCSharpFile &&
            FormattingService.getInstance(e.getProject()).getCanFormat(file, e.getProject());
        e.getPresentation().setEnabled(canFormat);
    }

    private static void processFileInEditor(@NotNull Project project, @NotNull Document document) {
        var formattingService = FormattingService.getInstance(project);
        formattingService.format(document, project);
    }
}
