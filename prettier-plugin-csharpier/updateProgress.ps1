git checkout master
git branch -d progress
git checkout -b progress
Copy-Item Samples/AllInOne.cs Samples/AllInOne.Formatted.cs
git add Samples/AllInOne.Formatted.cs
git commit -m"Updating progress"
#TODO need to make sure I have permissions
#TODO update count of missing/complete nodes?
git push --force --set-upstream origin progress --quiet
git checkout master
