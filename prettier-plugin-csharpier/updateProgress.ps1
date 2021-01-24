git checkout master
git branch -d progress
git checkout -b progress
Copy-Item Samples/AllInOne.cs Samples/AllInOne.Formatted.cs
git add Samples/AllInOne.Formatted.cs
git commit -m"Updating progress"
git push --force --set-upstream origin progress --quiet
git checkout master
