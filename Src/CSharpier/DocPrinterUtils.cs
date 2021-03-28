using System;
using System.Collections.Generic;

namespace CSharpier
{
    public static class DocPrinterUtils
    {
        private static readonly Doc traverseDocOnExitStackMarker = new();

        public static void PropagateBreaks(Doc document)
        {
            var alreadyVisitedSet = new HashSet<Group>();
            var groupStack = new Stack<Group>();
            var forceFlat = 0;

            void BreakParentGroup()
            {
                if (groupStack.Count > 0)
                {
                    var parentGroup = groupStack.Peek();
                    // Breaks are not propagated through conditional groups because
                    // the user is expected to manually handle what breaks.
                    if (!parentGroup.ExpandedStates)
                    {
                        parentGroup.Break = true;
                    }
                }
            }

            bool OnEnter(Doc doc)
            {
                if (doc is ForceFlat)
                {
                    forceFlat++;
                }
                if (doc is BreakParent && forceFlat == 0)
                {
                    BreakParentGroup();
                }
                else if (doc is Group group)
                {
                    groupStack.Push(group);
                    if (alreadyVisitedSet.Contains(group))
                    {
                        return false;
                    }

                    alreadyVisitedSet.Add(group);
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

                if (doc == traverseDocOnExitStackMarker)
                {
                    OnExit(docsStack.Pop());
                    continue;
                }

                docsStack.Push(doc);
                docsStack.Push(traverseDocOnExitStackMarker);

                if (!OnEnter(doc))
                {
                    continue;
                }

                if (doc is Concat concat)
                {
                    // push onto stack in reverse order so they are processed in the original order
                    for (var x = concat.Parts.Count - 1; x >= 0; --x)
                    {
                        if (forceFlat > 0 && concat.Parts[x] is LineDoc lineDoc)
                        {
                            concat.Parts[
                                x
                            ] = lineDoc.Type == LineDoc.LineType.Soft
                                ? string.Empty
                                : " ";
                        }

                        docsStack.Push(concat.Parts[x]);
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
