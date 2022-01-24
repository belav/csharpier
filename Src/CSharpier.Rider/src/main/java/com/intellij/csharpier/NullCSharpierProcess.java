package com.intellij.csharpier;

public class NullCSharpierProcess implements ICSharpierProcess {
    @Override
    public String formatFile(String content, String fileName) {
        return "";
    }

    @Override
    public void dispose() {

    }
}
