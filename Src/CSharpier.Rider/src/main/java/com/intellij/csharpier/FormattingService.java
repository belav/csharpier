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

import java.util.Collections;

public class FormattingService {
    Logger logger = Logger.getInstance(FormattingService.class);

    @NotNull
    static FormattingService getInstance(@NotNull Project project) {
        return project.getService(FormattingService.class);
    }

    public void format(@NotNull Document document,@NotNull Project project) {
        PsiFile psiFile = PsiDocumentManager.getInstance(project).getPsiFile(document);
        if (psiFile == null) {
            return;
        }



        if (!(psiFile.getLanguage().getID().equals("C#")
                // while testing in intellij it doesn't know about c#
                || (psiFile.getLanguage().getID().equals("TEXT") && psiFile.getName().endsWith(".cs")))
        ) {
            this.logger.info("Skipping formatting because language was " + psiFile.getLanguage().getDisplayName());
            return;
        }

        VirtualFile virtualFile = psiFile.getVirtualFile();
        if (ReadonlyStatusHandler.getInstance(project)
                .ensureFilesWritable(Collections.singletonList(virtualFile))
                .hasReadonlyFiles()
        ) {
            return;
        }

        String filePath = virtualFile.getPath();
        String currentDocumentText = document.getText();

        CSharpierService cSharpierService = CSharpierService.getInstance(project);
        String result = cSharpierService.format(currentDocumentText, filePath);

        if (result.length() > 0) {
            WriteCommandAction.runWriteCommandAction(project, () -> {
                document.replaceString(0, currentDocumentText.length(), result);
            });
        }
    }
}
