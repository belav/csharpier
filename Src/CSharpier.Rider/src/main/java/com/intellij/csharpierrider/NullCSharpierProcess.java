package com.intellij.csharpierrider;

public class NullCSharpierProcess implements ICSharpierProcess {
    @Override
    public String formatFile(String content, String fileName) {
        return "";
    }
}
