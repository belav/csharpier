package com.intellij.csharpier;

import com.intellij.openapi.command.WriteCommandAction;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.vfs.ReadonlyStatusHandler;
import com.intellij.openapi.vfs.VirtualFile;
import com.intellij.psi.PsiDocumentManager;
import com.intellij.psi.PsiFile;
import org.jetbrains.annotations.NotNull;

import java.time.Duration;
import java.time.Instant;
import java.util.Collections;

public class FormattingService {
    Logger logger = CSharpierLogger.getInstance();

    @NotNull
    static FormattingService getInstance(@NotNull Project project) {
        return project.getService(FormattingService.class);
    }

    public void format(@NotNull Document document, @NotNull Project project) {
        var psiFile = PsiDocumentManager.getInstance(project).getPsiFile(document);
        if (psiFile == null) {
            return;
        }

        if (!(psiFile.getLanguage().getID().equals("C#")
                // while testing in intellij it doesn't know about c#
                || (psiFile.getLanguage().getID().equals("TEXT") && psiFile.getName().endsWith(".cs")))
        ) {
            this.logger.debug("Skipping formatting because language was " + psiFile.getLanguage().getDisplayName());
            return;
        }

        var virtualFile = psiFile.getVirtualFile();
        if (ReadonlyStatusHandler.getInstance(project)
                .ensureFilesWritable(Collections.singletonList(virtualFile))
                .hasReadonlyFiles()
        ) {
            return;
        }

        var filePath = virtualFile.getPath();

        if (!this.getCanFormat(filePath, project)) {
            return;
        }

        var currentDocumentText = document.getText();

        var cSharpierProcessProvider = CSharpierProcessProvider.getInstance(project);
        this.logger.info("Formatting started for " + filePath + ".");
        var start = Instant.now();
        var result = cSharpierProcessProvider.getProcessFor(filePath).formatFile(currentDocumentText, filePath);
        var end = Instant.now();
        this.logger.info("Formatted in " + (Duration.between(start, end).toMillis()) + "ms");

        if (result.length() > 0) {
            WriteCommandAction.runWriteCommandAction(project, () -> {
                document.replaceString(0, currentDocumentText.length(), result);
            });
        }
    }

    public boolean getCanFormat(String filePath, Project project) {
        var cSharpierProcess = CSharpierProcessProvider.getInstance(project).getProcessFor(filePath);
        return !NullCSharpierProcess.class.isInstance(cSharpierProcess);
    }
}
