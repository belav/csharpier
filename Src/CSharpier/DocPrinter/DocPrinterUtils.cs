using System.Collections.Generic;
using CSharpier.DocTypes;

namespace CSharpier.DocPrinter
{
    public static class DocPrinterUtils
    {
        private static readonly Doc TraverseDocOnExitStackMarker = new();

        public static void PropagateBreaks(Doc document)
        {
            var alreadyVisitedSet = new HashSet<Group>();
            var groupStack = new Stack<Group>();
            var forceFlat = 0;
            var newGroup = false;
            var skipNextBreak = false;

            void BreakParentGroup()
            {
                if (groupStack.Count > 0)
                {
                    var parentGroup = groupStack.Peek();
                    parentGroup.Break = true;
                }
            }

            bool OnEnter(Doc doc)
            {
                if (
                    (doc is HardLine { SkipBreakIfFirstInGroup: true }  ||
                    doc is HardLineIfNoPreviousLine { SkipBreakIfFirstInGroup: true } ) &&
                    newGroup
                ) {
                    skipNextBreak = true;
                    return true;
                }
                if (doc is ForceFlat)
                {
                    forceFlat++;
                }
                if (doc is BreakParent && forceFlat == 0)
                {
                    if (!skipNextBreak)
                    {
                        BreakParentGroup();
                    }
                    else
                    {
                        skipNextBreak = false;
                    }
                }
                else if (doc is Group group)
                {
                    newGroup = true;
                    groupStack.Push(group);
                    if (alreadyVisitedSet.Contains(group))
                    {
                        return false;
                    }

                    alreadyVisitedSet.Add(group);
                }
                else if (doc is StringDoc { IsTrivia: false } )
                {
                    newGroup = false;
                }

                return true;
            }

            void OnExit(Doc doc)
            {
                if (doc is ForceFlat)
                {
                    forceFlat--;
                }
                else if (doc is Group)
                {
                    newGroup = false;
                    var group = groupStack.Pop();
                    if (group.Break)
                    {
                        BreakParentGroup();
                    }
                }
            }

            var docsStack = new Stack<Doc>();
            docsStack.Push(document);
            while (docsStack.Count > 0)
            {
                var doc = docsStack.Pop();

                if (doc == TraverseDocOnExitStackMarker)
                {
                    OnExit(docsStack.Pop());
                    continue;
                }

                docsStack.Push(doc);
                docsStack.Push(TraverseDocOnExitStackMarker);

                if (!OnEnter(doc))
                {
                    continue;
                }

                if (doc is Concat concat)
                {
                    // push onto stack in reverse order so they are processed in the original order
                    for (var x = concat.Contents.Count - 1; x >= 0; --x)
                    {
                        if (
                            forceFlat > 0 &&
                            concat.Contents[x] is LineDoc lineDoc
                        ) {
                            concat.Contents[x] = lineDoc.Type ==
                                LineDoc.LineType.Soft
                                ? string.Empty
                                : " ";
                        }

                        docsStack.Push(concat.Contents[x]);
                    }
                }

                if (doc is IHasContents hasContents)
                {
                    docsStack.Push(hasContents.Contents);
                }
            }
        }
    }
}
