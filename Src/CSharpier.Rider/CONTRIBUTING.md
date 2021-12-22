Useful links
- https://github.com/JetBrains/intellij-platform-plugin-template
- https://plugins.jetbrains.com/docs/intellij/welcome.html
- https://developerlife.com/2021/03/13/ij-idea-plugin-advanced/

Local testing
- use Run Plugin to run plugin in intellij
  - there may be a way to run it in rider - https://jetbrains-platform.slack.com/archives/C5NMWKBJ4/p1640080931056300
- Run  .\gradlew.bat :buildPlugin to create plugin so that it can be used in rider, 
  - may need to change version number in gradle.properties
  - go to settings - plugins - install manually - Src\CSharpier.Rider\build\distributions