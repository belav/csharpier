import org.jetbrains.changelog.markdownToHTML
import org.jetbrains.kotlin.gradle.tasks.KotlinCompile

fun properties(key: String) = project.findProperty(key).toString()

plugins {
    id("java")
    id("org.jetbrains.kotlin.jvm") version "1.9.21"
    id("org.jetbrains.intellij.platform") version "2.2.1"
    id("org.jetbrains.changelog") version "2.2.0"
    id("org.jetbrains.qodana") version "0.1.13"
    id("com.jetbrains.rdgen") version "2024.3.1"
}

repositories {
    mavenCentral()
    intellijPlatform {
        defaultRepositories()
    }
}

intellijPlatform {
    tasks {
        buildPlugin {
            archiveVersion = properties("pluginVersion")
        }
    }
    pluginConfiguration {
        name = "csharpier"
        group = "com.intellij.csharpier"
        ideaVersion.sinceBuild.set(properties("pluginSinceBuild"))
        ideaVersion.untilBuild.set(properties("pluginUntilBuild"))
        description = getProjectDescription()
        version = properties("pluginVersion")
    }
}

dependencies {
    intellijPlatform {
        create("RD", properties("platformVersion"))
    }
}

fun getProjectDescription(): String {
    return projectDir.resolve("README.md").readText().lines().run {
        val start = "<!-- Plugin description -->"
        val end = "<!-- Plugin description end -->"

        if (!containsAll(listOf(start, end))) {
            throw GradleException("Plugin description section not found in README.md:\n$start ... $end")
        }
        subList(indexOf(start) + 1, indexOf(end))
    }.joinToString("\n").run { markdownToHTML(this) }
}