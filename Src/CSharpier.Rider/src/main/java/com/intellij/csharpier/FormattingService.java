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
import org.jetbrains.annotations.NotNull;

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

    if (
      !(psiFile.getLanguage().getID().equals("C#") ||
        // while testing in intellij it doesn't know about c#
        (psiFile.getLanguage().getID().equals("TEXT") && psiFile.getName().endsWith(".cs")))
    ) {
      this.logger.debug(
          "Skipping formatting because language was " + psiFile.getLanguage().getDisplayName()
        );
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

    var csharpierProcess = CSharpierProcessProvider.getInstance(project).getProcessFor(filePath);
    this.logger.info(
        "Formatting started for " + filePath + " using CSharpier " + csharpierProcess.getVersion()
      );
    var start = Instant.now();
    if (csharpierProcess instanceof ICSharpierProcess2) {
      var csharpierProcess2 = (ICSharpierProcess2) csharpierProcess;
      var parameter = new FormatFileParameter();
      parameter.fileContents = currentDocumentText;
      parameter.fileName = filePath;
      var result = csharpierProcess2.formatFile(parameter);

      var end = Instant.now();
      this.logger.info("Formatted in " + (Duration.between(start, end).toMillis()) + "ms");

      if (result != null) {
        switch (result.status) {
          case Formatted -> updateText(
            document,
            project,
            result.formattedFile,
            currentDocumentText
          );
          case Ignored -> this.logger.info("File is ignored by csharpier cli.");
          case Failed -> this.logger.warn(
              "CSharpier cli failed to format the file and returned the following error: " +
              result.errorMessage
            );
        }
      }
    } else {
      var result = csharpierProcess.formatFile(currentDocumentText, filePath);

      var end = Instant.now();
      this.logger.info("Formatted in " + (Duration.between(start, end).toMillis()) + "ms");

      if (result.length() == 0 || currentDocumentText.equals(result)) {
        this.logger.debug(
            "Skipping write because " +
            (result.length() == 0 ? "result is empty" : "current document equals result")
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
    var cSharpierProcess = CSharpierProcessProvider.getInstance(project).getProcessFor(filePath);
    return !NullCSharpierProcess.Instance.equals(cSharpierProcess);
  }
}
