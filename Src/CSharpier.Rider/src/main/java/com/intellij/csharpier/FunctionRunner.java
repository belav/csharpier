package com.intellij.csharpier;

import java.util.function.Supplier;

public class FunctionRunner {
    public static String runUntilNonNull(Supplier<String>... functions) {
        for (Supplier<String> function : functions) {
            String result = function.get();
            if (result != null) {
                return result;
            }
        }
        return null;
    }
}
