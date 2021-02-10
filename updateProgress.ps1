git checkout master
git branch -d progress
git checkout -b progress
Copy-Item CSharpier/CSharpier.Core.Tests/Samples/AllInOne.cst CSharpier/CSharpier.Core.Tests/Samples/AllInOne.Formatted.cst
git add CSharpier/CSharpier.Core.Tests/Samples/AllInOne.Formatted.cst
git commit -m"Updating progress"
git push --force --set-upstream origin progress --quiet
git checkout master
