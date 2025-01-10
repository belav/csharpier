package com.intellij.csharpier;

import java.lang.Runtime.Version;

public class Semver {
    public static boolean gte(String version, String otherVersion) {
        return Version.parse(version).compareTo(Version.parse(otherVersion)) >= 0;
    }
}
