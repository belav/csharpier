Useful links
- https://github.com/JetBrains/intellij-platform-plugin-template
- https://plugins.jetbrains.com/docs/intellij/welcome.html
- https://developerlife.com/2021/03/13/ij-idea-plugin-advanced/

Local testing
- use Run Plugin to run plugin in intellij
- Run  .\gradlew.bat :buildPlugin to create plugin so that it can be used in rider, 
  - may need to change version number in gradle.properties
  - go to settings - plugins - install manually - Src\CSharpier.Rider\build\distributions

## Template ToDo list
- [x] Verify the [plugin ID](/src/main/resources/META-INF/plugin.xml) and [sources package](/src/main/kotlin).
- [x] Review the [Legal Agreements](https://plugins.jetbrains.com/docs/marketplace/legal-agreements.html).
- [x] [Publish a plugin manually](https://plugins.jetbrains.com/docs/intellij/publishing-plugin.html?from=IJPluginTemplate)
  for the first time.
- [ ] Set the Plugin ID in the above README badges.
- [ ] Set the [Deployment Token](https://plugins.jetbrains.com/docs/marketplace/plugin-upload.html).
- [ ] Icon file
- [ ] look into workflows