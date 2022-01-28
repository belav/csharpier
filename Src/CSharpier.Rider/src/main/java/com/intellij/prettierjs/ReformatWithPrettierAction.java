// Copyright 2000-2021 JetBrains s.r.o. and contributors. Use of this source code is governed by the Apache 2.0 license that can be found in the LICENSE file.
package com.intellij.prettierjs;

import com.intellij.codeInsight.actions.FileTreeIterator;
import com.intellij.codeInsight.actions.VcsFacade;
import com.intellij.codeInsight.hint.HintManager;
import com.intellij.codeInsight.hint.HintManagerImpl;
import com.intellij.codeInsight.hint.HintUtil;
import com.intellij.codeStyle.AbstractConvertLineSeparatorsAction;
import com.intellij.execution.ExecutionException;
import com.intellij.execution.process.ProcessOutput;
import com.intellij.notification.Notification;
import com.intellij.notification.NotificationListener;
import com.intellij.notification.NotificationType;
import com.intellij.openapi.actionSystem.AnAction;
import com.intellij.openapi.actionSystem.AnActionEvent;
import com.intellij.openapi.actionSystem.CommonDataKeys;
import com.intellij.openapi.application.ApplicationManager;
import com.intellij.openapi.application.ModalityState;
import com.intellij.openapi.application.ReadAction;
import com.intellij.openapi.command.WriteCommandAction;
import com.intellij.openapi.diagnostic.Logger;
import com.intellij.openapi.editor.Document;
import com.intellij.openapi.editor.Editor;
import com.intellij.openapi.editor.ex.util.EditorScrollingPositionKeeper;
import com.intellij.openapi.fileEditor.FileDocumentManager;
import com.intellij.openapi.options.ShowSettingsUtil;
import com.intellij.openapi.progress.ProgressIndicator;
import com.intellij.openapi.progress.ProgressManager;
import com.intellij.openapi.project.DumbAware;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.util.Ref;
import com.intellij.openapi.util.TextRange;
import com.intellij.openapi.util.ThrowableComputable;
import com.intellij.openapi.util.text.StringUtil;
import com.intellij.openapi.vfs.ReadonlyStatusHandler;
import com.intellij.openapi.vfs.VirtualFile;
import com.intellij.psi.PsiDirectory;
import com.intellij.psi.PsiDocumentManager;
import com.intellij.psi.PsiFile;
import com.intellij.psi.PsiManager;
import com.intellij.psi.util.PsiUtilCore;
import com.intellij.ui.HyperlinkAdapter;
import com.intellij.ui.LightweightHint;
import com.intellij.util.ArrayUtil;
import com.intellij.util.LineSeparator;
import com.intellij.util.NullableFunction;
import com.intellij.util.SmartList;
import com.intellij.util.containers.ContainerUtil;
import com.intellij.util.text.SemVer;
import org.jetbrains.annotations.Nls;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import javax.swing.*;
import javax.swing.event.HyperlinkEvent;
import javax.swing.event.HyperlinkListener;
import java.util.*;
import java.util.concurrent.CompletableFuture;

public class ReformatWithPrettierAction extends AnAction implements DumbAware {
    private static final @NotNull Logger LOG = Logger.getInstance(ReformatWithPrettierAction.class);
    private static final long EDT_TIMEOUT_MS = 2000;

    private final ErrorHandler myErrorHandler;

    public ReformatWithPrettierAction(@NotNull ErrorHandler errorHandler) {
        myErrorHandler = errorHandler;
    }

    public ReformatWithPrettierAction() {
        this(ErrorHandler.DEFAULT);
    }

    @Override
    public void update(@NotNull AnActionEvent e) {
        var project = e.getProject();
        if (project == null) {
            e.getPresentation().setEnabledAndVisible(false);
            return;
        }
//        NodePackage nodePackage = PrettierConfiguration.getInstance(project).getPackage();
//        e.getPresentation().setEnabledAndVisible(!nodePackage.isEmptyPath() && isAcceptableFileContext(e));
    }

    private static boolean isAcceptableFileContext(@NotNull AnActionEvent e) {
        var editor = e.getData(CommonDataKeys.EDITOR);
        if (editor != null) {
            return true;
        }
        var virtualFiles = e.getData(CommonDataKeys.VIRTUAL_FILE_ARRAY);
        return !ArrayUtil.isEmpty(virtualFiles);
    }

    @Override
    public void actionPerformed(@NotNull AnActionEvent e) {
        var project = e.getProject();
        if (project == null) {
            return;
        }
        var editor = e.getData(CommonDataKeys.EDITOR);
        if (editor != null) {
            processFileInEditor(project, editor, myErrorHandler);
        }
        else {
            var virtualFiles = e.getData(CommonDataKeys.VIRTUAL_FILE_ARRAY);
            if (!ArrayUtil.isEmpty(virtualFiles)) {
                processVirtualFiles(project, Arrays.asList(virtualFiles), myErrorHandler);
            }
        }
    }

//    private static boolean checkNodeAndPackage(@NotNull Project project,
//                                               @Nullable Editor editor,
//                                               @NotNull NodeJsInterpreterRef interpreterRef,
//                                               @NotNull NodePackage nodePackage,
//                                               @NotNull ErrorHandler errorHandler) {
//        try {
//            NodeInterpreterUtil.getValidInterpreterOrThrow(interpreterRef.resolve(project));
//        }
//        catch (ExecutionException e1) {
//            errorHandler.showError(project, editor, PrettierBundle.message("error.invalid.interpreter"),
//                    () -> NodeSettingsConfigurable.showSettingsDialog(project));
//            return false;
//        }
//
//        if (nodePackage.isEmptyPath()) {
//            errorHandler.showError(project, editor, PrettierBundle.message("error.no.valid.package"),
//                    () -> editSettings(project));
//            return false;
//        }
//        if (!nodePackage.isValid()) {
//            String message = PrettierBundle.message("error.package.is.not.installed",
//                    NpmManager.getInstance(project).getNpmInstallPresentableText());
//            errorHandler.showError(project, editor, message, () -> installPackage(project));
//            return false;
//        }
//        SemVer nodePackageVersion = nodePackage.getVersion(project);
//        if (nodePackageVersion != null && nodePackageVersion.compareTo(PrettierUtil.MIN_VERSION) < 0) {
//            errorHandler.showError(project, editor,
//                    PrettierBundle.message("error.unsupported.version", PrettierUtil.MIN_VERSION.getRawVersion()), null);
//            return false;
//        }
//
//        return true;
//    }

    private static void processFileInEditor(@NotNull Project project, @NotNull Editor editor, @NotNull ErrorHandler errorHandler) {
//        PrettierConfiguration configuration = PrettierConfiguration.getInstance(project);
//        NodePackage nodePackage = configuration.getPackage();
//
//        if (!checkNodeAndPackage(project, editor, configuration.getInterpreterRef(), nodePackage, errorHandler)) return;

//        var file = PsiDocumentManager.getInstance(project).getPsiFile(editor.getDocument());
//        if (file == null) {
//            return;
//        }
//        var vFile = file.getVirtualFile();
//        if (ReadonlyStatusHandler.getInstance(project).ensureFilesWritable(Collections.singletonList(vFile))
//                .hasReadonlyFiles()) {
//            return;
//        }
//
//        var range = editor.getSelectionModel().hasSelection()
//                ? new TextRange(editor.getSelectionModel().getSelectionStart(), editor.getSelectionModel().getSelectionEnd()) : null;
//        ensureConfigsSaved(Collections.singletonList(vFile), project);
//        PrettierLanguageService service = PrettierLanguageService.getInstance(file.getProject());
//        var computable =
//                (ThrowableComputable<PrettierLanguageService.FormatResult, RuntimeException>)
//                        () -> performRequestForFile(project, nodePackage, service, file, range);
//        PrettierLanguageService.FormatResult result = ProgressManager
//                .getInstance()
//                .runProcessWithProgressSynchronously(computable, PrettierBundle.message("progress.title"), true, project);
//        // timed out. show notification?
//        if (result == null) {
//            return;
//        }
//        if (!StringUtil.isEmpty(result.error)) {
//            errorHandler.showErrorWithDetails(project, editor,
//                    PrettierBundle.message("error.while.reformatting.message"), result.error);
//        }
//        else if (result.unsupported) {
//            errorHandler.showError(project, editor, PrettierBundle.message("not.supported.file", file.getName()), null);
//        }
//        else if (result.ignored) {
//            showHintLater(editor, PrettierBundle.message("file.was.ignored.hint", file.getName()), false, null);
//        }
//        else {
//            var document = editor.getDocument();
//            var textBefore = document.getImmutableCharSequence();
//            String newContent = result.result;
//            /*
//             * This checks only the first line break, but given that we don't handle mixed line separators,
//             * this is enough to detect if separators were changed by the external process
//             */
//            var newLineSeparator = StringUtil.detectSeparators(newContent);
//            var newDocumentContent = StringUtil.convertLineSeparators(newContent);
//
//            var lineSeparatorUpdated = new Ref<Boolean>(Boolean.FALSE);
//            EditorScrollingPositionKeeper.perform(editor, true, () -> {
//                runWriteCommandAction(project, () -> {
//                    if (!StringUtil.equals(textBefore, newContent)) {
//                        document.setText(newDocumentContent);
//                    }
//                    lineSeparatorUpdated.set(setDetectedLineSeparator(project, vFile, newLineSeparator));
//                });
//            });
//
//            showHintLater(editor, buildNotificationMessage(document, textBefore, lineSeparatorUpdated.get()), false, null);
//        }
    }

    public static TextRange processFileAsPostFormatProcessor(@NotNull PsiFile file, @NotNull TextRange range) {
//        // PostFormatProcessors are invoked in EDT under rite action. So we can't show progress and need to block for a while waiting for the result.
//        LOG.assertTrue(ApplicationManager.getApplication().isWriteAccessAllowed());
//
//        var project = file.getProject();
//        PrettierConfiguration configuration = PrettierConfiguration.getInstance(project);
//        NodePackage nodePackage = configuration.getPackage();
//
//        if (!checkNodeAndPackage(project, null, configuration.getInterpreterRef(), nodePackage, PrettierActionOnSave.NOOP_ERROR_HANDLER)) {
//            return range;
//        }
//
//        var vFile = file.getVirtualFile();
//        ensureConfigsSaved(Collections.singletonList(vFile), project);
//        PrettierLanguageService service = PrettierLanguageService.getInstance(file.getProject());
//        var result = performRequestForFile(project, nodePackage, service, file, range);
//        if (result != null) {
//            var delta = applyFormatResult(project, vFile, result);
//            if (delta < 0 && range.getLength() < Math.abs(delta)) {
//                return TextRange.from(range.getStartOffset(), 0);
//            }
//            return range.grown(delta);
//        }
//        return range;
        return null;
    }

    private static void ensureConfigsSaved(@NotNull List<VirtualFile> virtualFiles, @NotNull Project project) {
//        var documentManager = FileDocumentManager.getInstance();
//        for (VirtualFile config : PrettierUtil.lookupPossibleConfigFiles(virtualFiles, project)) {
//            var document = documentManager.getCachedDocument(config);
//            if (document != null && documentManager.isDocumentUnsaved(document)) {
//                documentManager.saveDocument(document);
//            }
//        }
    }

    public static void processVirtualFiles(@NotNull Project project,
                                           @NotNull List<VirtualFile> virtualFiles,
                                           @NotNull ErrorHandler errorHandler) {
//        PrettierConfiguration configuration = PrettierConfiguration.getInstance(project);
//        NodePackage nodePackage = configuration.getPackage();
//        if (!checkNodeAndPackage(project, null, configuration.getInterpreterRef(), nodePackage, errorHandler)) return;
        var nodePackage = new NodePackage();

        var readonlyStatus = ReadonlyStatusHandler.getInstance(project).ensureFilesWritable(virtualFiles);
        if (readonlyStatus.hasReadonlyFiles()) {
            return;
        }
        ensureConfigsSaved(virtualFiles, project);
        var psiManager = PsiManager.getInstance(project);
        if (virtualFiles.size() == 1 && virtualFiles.get(0).isDirectory()) {
            var psiDirectory = psiManager.findDirectory(virtualFiles.get(0));
            if (psiDirectory == null) {
                return;
            }
            processFileIterator(project, new FileTreeIterator(psiDirectory), nodePackage, false, errorHandler);
        }
        else {
            processFileIterator(project, new FileTreeIterator(PsiUtilCore.toPsiFiles(psiManager, virtualFiles)),
                    nodePackage, true, errorHandler);
        }
    }

    private static void processFileIterator(@NotNull Project project,
                                            @NotNull final FileTreeIterator fileIterator,
                                            @NotNull NodePackage nodePackage,
                                            boolean reportSkippedFiles,
                                            @NotNull ErrorHandler errorHandler) {
        PrettierLanguageService service = PrettierLanguageService.getInstance(project);
//        var results = executeUnderProgress(project, indicator -> {
//            Map<PsiFile, PrettierLanguageService.FormatResult> reformattedResults = new HashMap<>();
//
//            List<PsiFile> files = new SmartList<>();
//            ReadAction.run(() -> {
//                while (fileIterator.hasNext()) {
//                    files.add(fileIterator.next());
//                }
//            });
//
//            for (var currentFile : files) {
//                indicator.setText(String.format("processing.0.progress", currentFile.getName()));
//                var result = performRequestForFile(project, nodePackage, service, currentFile, null);
//                // timed out. show notification?
//                if (result == null) {
//                    continue;
//                }
//                if (result.unsupported && reportSkippedFiles) {
//                    PrettierLanguageService.FormatResult errorResult = PrettierLanguageService.FormatResult
//                            .error(String.format("not.supported.file", currentFile.getName()));
//                    reformattedResults.put(currentFile, errorResult);
//                }
//                if (result.ignored) {
//                    PrettierLanguageService.FormatResult errorResult =
//                            PrettierLanguageService.FormatResult.error(String.format("file.was.ignored", currentFile.getName()));
//                    reformattedResults.put(currentFile, errorResult);
//                    continue;
//                }
//                reformattedResults.put(currentFile, result);
//            }
//            return reformattedResults;
//        });
//
//        runWriteCommandAction(project, () -> {
//            for (var entry : results.entrySet()) {
//                var virtualFile = entry.getKey().getVirtualFile();
//                if (virtualFile == null) {
//                    continue;
//                }
//                var result = entry.getValue();
//                applyFormatResult(project, virtualFile, result);
//            }
//        });
//        List<String> errors = ContainerUtil.mapNotNull(results.entrySet(), t -> t.getValue().error);
//        if (errors.size() > 0) {
//            errorHandler.showErrorWithDetails(project, null,
//                    String.format("failed.to.reformat.0.files", errors.size()),
//                    StringUtil.join(errors, "\n"));
//        }
    }

    /**
     * @param result (new text length) - (old text length)
     */
    private static int applyFormatResult(@NotNull Project project,
                                         @NotNull VirtualFile virtualFile,
                                         @NotNull PrettierLanguageService.FormatResult result) {
        var document = FileDocumentManager.getInstance().getDocument(virtualFile);
        var delta = 0;
        if (document != null && StringUtil.isEmpty(result.error) && !result.ignored && !result.unsupported) {
            var textBefore = document.getCharsSequence();
            var immutableCharSequence = document.getImmutableCharSequence();
            var newlineSeparator = StringUtil.detectSeparators(result.result);
            var newContent = StringUtil.convertLineSeparators(result.result);
            if (!StringUtil.equals(textBefore, newContent)) {
                var lengthBefore = textBefore.length();
                document.setText(newContent);
                delta = newContent.length() - lengthBefore;
            }
            setDetectedLineSeparator(project, virtualFile, newlineSeparator);
        }
        return delta;
    }

    @Nullable
    private static PrettierLanguageService.FormatResult performRequestForFile(@NotNull Project project,
                                                                              @NotNull NodePackage nodePackage,
                                                                              @NotNull PrettierLanguageService service,
                                                                              @NotNull PsiFile currentFile,
                                                                              @Nullable TextRange range) {
        var edt = ApplicationManager.getApplication().isDispatchThread();
        if (!edt && ApplicationManager.getApplication().isReadAccessAllowed()) {
            LOG.error("JSLanguageServiceUtil.awaitFuture() under read action may cause deadlock");
        }

        Ref<String> text = Ref.create();
        Ref<String> filePath = Ref.create();
        Ref<String> ignoreFilePath = Ref.create();

        ReadAction.run(() -> {
            if (!currentFile.isValid()) return;

            var currentVFile = currentFile.getVirtualFile();
            // todo require JS
//            filePath.set(LocalFilePath.asLocalFilePath(currentVFile.toNioPath()));
            // PsiFile might be not committed at this point, take text from document
            var document = PsiDocumentManager.getInstance(project).getDocument(currentFile);
            if (document == null) return;
            var content = document.getImmutableCharSequence();
            // todo require JS
//            text.set(JSLanguageServiceUtil.convertLineSeparatorsToFileOriginal(project, content, currentVFile).toString());
//            VirtualFile ignoreVFile = PrettierUtil.findIgnoreFile(currentVFile, project);
//            if (ignoreVFile != null) {
//                ignoreFilePath.set(ignoreVFile.getPath());
//            }
        });

        if (text.isNull()) {
            return PrettierLanguageService.FormatResult.UNSUPPORTED;
        }

        var formatFuture =
                service.format(filePath.get(), ignoreFilePath.get(), text.get(), nodePackage, range);
//        var timeout = edt ? EDT_TIMEOUT_MS : JSLanguageServiceUtil.getTimeout();
//        return JSLanguageServiceUtil.awaitFuture(formatFuture, timeout, JSLanguageServiceUtil.QUOTA_MILLS, null, true, null, edt);
        return null;
    }

//    private static <T> T executeUnderProgress(@NotNull Project project, @NotNull NullableFunction<ProgressIndicator, T> handler) {
////        return ProgressManager
////                .getInstance()
////                .runProcessWithProgressSynchronously(() -> handler.fun(ProgressManager.getInstance().getProgressIndicator()),
////                        PrettierBundle.message("progress.title"), true, project);
//        return null;
//    }

    private static void runWriteCommandAction(@NotNull Project project, @NotNull Runnable runnable) {
//        WriteCommandAction.runWriteCommandAction(project, PrettierBundle.message("reformat.with.prettier.command.name"), null, runnable);
    }

    private static @NotNull @Nls String buildNotificationMessage(@NotNull Document document,
                                                                 @NotNull CharSequence textBefore,
                                                                 boolean lineSeparatorsUpdated) {
//        var number = VcsFacade.getInstance().calculateChangedLinesNumber(document, textBefore);
//        if (number == 0) {
//            return lineSeparatorsUpdated ? PrettierBundle.message("line.endings.were.updated")
//                    : PrettierBundle.message("no.lines.changed");
//        }
//        return PrettierBundle.message("formatted.0.lines", number);
        return "";
    }

    private static void showHintLater(@NotNull Editor editor,
                                      @NotNull @Nls String text,
                                      boolean isError,
                                      @Nullable HyperlinkListener hyperlinkListener) {
        ApplicationManager.getApplication().invokeLater(() -> {
            final var component = isError ? HintUtil.createErrorLabel(text, hyperlinkListener, null)
                    : HintUtil.createInformationLabel(text, hyperlinkListener, null, null);
            final var hint = new LightweightHint(component);
            HintManagerImpl.getInstanceImpl()
                    .showEditorHint(hint, editor, HintManager.UNDER, HintManager.HIDE_BY_ANY_KEY | HintManager.HIDE_BY_TEXT_CHANGE |
                            HintManager.HIDE_BY_SCROLLING, 0, false);
        }, ModalityState.NON_MODAL, o -> editor.isDisposed() || !editor.getComponent().isShowing());
    }

    private static void installPackage(@NotNull Project project) {
//        final VirtualFile packageJson = PackageJsonUtil.findChildPackageJsonFile(project.getBaseDir());
//        if (packageJson != null) {
//            InstallNodeLocalDependenciesAction.runAndShowConsole(project, packageJson);
//        }
    }

    private static void showErrorDetails(@NotNull Project project, @NotNull String text) {
//        var output = new ProcessOutput();
//        output.appendStderr(text);
//        JsqtProcessOutputViewer
//                .show(project, PrettierBundle.message("prettier.formatter.notification.title"), PrettierUtil.ICON, null, null, output);
    }

    private static void editSettings(@NotNull Project project) {
//        ShowSettingsUtil.getInstance().editConfigurable(project, new PrettierConfigurable(project));
    }

    /**
     * @returns true if line separator was updated
     */
    private static boolean setDetectedLineSeparator(@NotNull Project project,
                                                    @NotNull VirtualFile vFile,
                                                    @Nullable LineSeparator newSeparator) {
        if (newSeparator != null) {
            var newSeparatorString = newSeparator.getSeparatorString();
            if (!StringUtil.equals(vFile.getDetectedLineSeparator(), newSeparatorString)) {
                AbstractConvertLineSeparatorsAction.changeLineSeparators(project, vFile, newSeparatorString);
                return true;
            }
        }
        return false;
    }

    public interface ErrorHandler {
        ErrorHandler DEFAULT = new DefaultErrorHandler();

        void showError(@NotNull Project project,
                       @Nullable Editor editor,
                       @NotNull @Nls String text,
                       @Nullable Runnable onLinkClick);

        default void showErrorWithDetails(@NotNull Project project,
                                          @Nullable Editor editor,
                                          @NotNull @Nls String text,
                                          @NotNull String details) {
            showError(project, editor, text, () -> showErrorDetails(project, details));
        }
    }

    private static class DefaultErrorHandler implements ErrorHandler {
        @Override
        public void showError(@NotNull Project project, @Nullable Editor editor, @NotNull @Nls String text, @Nullable Runnable onLinkClick) {
//            if (editor != null) {
//                HyperlinkListener listener = onLinkClick == null ? null : new HyperlinkAdapter() {
//                    @Override
//                    protected void hyperlinkActivated(HyperlinkEvent e) {
//                        onLinkClick.run();
//                    }
//                };
//                showHintLater(editor, PrettierBundle.message("prettier.formatter.hint.0", text), true, listener);
//            }
//            else {
//                Notification notification = JSLinterGuesser.NOTIFICATION_GROUP.createNotification(PrettierBundle.message("prettier.formatter.notification.title"), text, NotificationType.ERROR);
//                if (onLinkClick != null) {
//                    notification.setListener(new NotificationListener.Adapter() {
//                        @Override
//                        protected void hyperlinkActivated(@NotNull Notification notification1, @NotNull HyperlinkEvent e) {
//                            onLinkClick.run();
//                        }
//                    });
//                }
//                notification.notify(project);
//            }
        }
    }
}
