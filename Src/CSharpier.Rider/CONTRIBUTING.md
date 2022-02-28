Useful links
- https://github.com/JetBrains/intellij-platform-plugin-template
- https://plugins.jetbrains.com/docs/intellij/welcome.html
- https://developerlife.com/2021/03/13/ij-idea-plugin-advanced/

Local testing
- use "Run Plugin" configuration while in intellij to launch the plugin into a dev instance of rider
- Run  .\gradlew.bat :buildPlugin to create plugin so that it can be used in rider, 
  - may need to change version number in gradle.properties
  - go to settings - plugins - install manually - Src\CSharpier.Rider\build\distributions

Publishing
- Update version in gradle.properties
- Run  .\gradlew.bat :buildPlugin
- Login to https://plugins.jetbrains.com/plugin/18243-csharpier using github account
- upload file from build/distributions
- Automate this if we release a lot