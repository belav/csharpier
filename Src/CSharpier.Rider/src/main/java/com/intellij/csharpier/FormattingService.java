package com.intellij.csharpier;

import com.intellij.openapi.command.WriteCommandAction;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.vfs.ReadonlyStatusHandler;
import com.intellij.psi.PsiDocumentManager;
import java.time.Duration;
import java.time.Instant;
import java.util.Collections;
import java.util.concurrent.Callable;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;
import org.jetbrains.annotations.NotNull;

public class FormattingService {

    Logger logger = CSharpierLogger.getInstance();

    @NotNull
    static FormattingService getInstance(@NotNull Project project) {
        return project.getService(FormattingService.class);
    }

    static boolean isSupportedLanguageId(String languageId) {
        return (
            languageId.equals("C#") ||
            languageId.equals("MSBuild") ||
            languageId.equals("XML") ||
            languageId.equals("Xaml")
        );
    }

    public void format(@NotNull Document document, @NotNull Project project) {
        var psiFile = PsiDocumentManager.getInstance(project).getPsiFile(document);
        if (psiFile == null) {
            return;
        }

        var languageId = psiFile.getLanguage().getID();
        if (!isSupportedLanguageId(languageId)) {
            this.logger.debug("Skipping formatting because language was " + languageId);
            return;
        }

        var virtualFile = psiFile.getVirtualFile();
        if (
            ReadonlyStatusHandler.getInstance(project)
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

        var csharpierProcess = CSharpierProcessProvider.getInstance(project).getProcessFor(
            filePath
        );
        this.logger.info(
                "Formatting started for " +
                filePath +
                " using CSharpier " +
                csharpierProcess.getVersion()
            );
        var start = Instant.now();
        if (csharpierProcess instanceof ICSharpierProcess2) {
            var csharpierProcess2 = (ICSharpierProcess2) csharpierProcess;
            var parameter = new FormatFileParameter();
            parameter.fileContents = currentDocumentText;
            parameter.fileName = filePath;

            var executor = Executors.newSingleThreadExecutor();
            FormatFileResult result = null;

            Callable<FormatFileResult> task = () -> csharpierProcess2.formatFile(parameter);

            var future = executor.submit(task);

            try {
                result = future.get(2000, TimeUnit.MILLISECONDS);
            } catch (Exception e) {
                this.logger.warn("Took more than two seconds or something went wrong " + e);
                future.cancel(true);
            }

            if (result == null) {
                return;
            }

            switch (result.status) {
                case Formatted -> {
                    var end = Instant.now();
                    this.logger.info(
                            "Formatted in " + (Duration.between(start, end).toMillis()) + "ms"
                        );
                    updateText(document, project, result.formattedFile, currentDocumentText);
                }
                case Ignored -> this.logger.info("File is ignored by csharpier cli.");
                case Failed -> this.logger.warn(
                        "CSharpier cli failed to format the file and returned the following error: " +
                        result.errorMessage
                    );
                case UnsupportedFile -> this.logger.warn(
                        "CSharpier does not support formatting the file " + filePath
                    );
                default -> this.logger.error("Unable to handle for status of " + result.status);
            }
        } else {
            var result = csharpierProcess.formatFile(currentDocumentText, filePath);

            var end = Instant.now();
            this.logger.info("Formatted in " + (Duration.between(start, end).toMillis()) + "ms");

            if (result.length() == 0 || currentDocumentText.equals(result)) {
                this.logger.debug(
                        "Skipping write because " +
                        (result.length() == 0
                                ? "result is empty"
                                : "current document equals result")
                    );
            } else {
                updateText(document, project, result, currentDocumentText);
            }
        }
    }

    private static void updateText(
        @NotNull Document document,
        @NotNull Project project,
        String result,
        String currentDocumentText
    ) {
        WriteCommandAction.runWriteCommandAction(project, () -> {
            var finalResult = result;
            if (result.indexOf('\r') >= 0) {
                // rider always wants \n in files so remove any \r
                finalResult = result.replaceAll("\\r", "");
            }

            document.replaceString(0, currentDocumentText.length(), finalResult);
        });
    }

    public boolean getCanFormat(String filePath, Project project) {
        var cSharpierProcess = CSharpierProcessProvider.getInstance(project).getProcessFor(
            filePath
        );
        return !NullCSharpierProcess.Instance.equals(cSharpierProcess);
    }
}
