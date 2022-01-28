package com.intellij.prettierjs;

import com.intellij.openapi.util.TextRange;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import java.util.concurrent.CompletableFuture;

public class PrettierLanguageServiceImpl implements PrettierLanguageService {
    @Override
    public @Nullable CompletableFuture<FormatResult> format(@NotNull String filePath, String ignoreFilePath, @NotNull String text, @NotNull NodePackage prettierPackage, @Nullable TextRange range) {
        return null;
    }
}
