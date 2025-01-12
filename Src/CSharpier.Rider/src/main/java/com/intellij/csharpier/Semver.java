package com.intellij.csharpier;

import java.lang.module.ModuleDescriptor;

public class Semver {

    public static boolean gte(String version, String otherVersion) {
        return (
            ModuleDescriptor.Version.parse(version).compareTo(
                ModuleDescriptor.Version.parse(otherVersion)
            ) >=
            0
        );
    }
}
