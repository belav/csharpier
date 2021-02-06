using System;
using System.Collections.Generic;

namespace CSharpier
{
    public static class DocPrinterUtils
    {
        public static void PropagateBreaks(Doc document)
        {
            var alreadyVisitedSet = new HashSet<Group>();
            var groupStack = new Stack<Group>();

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

            bool PropagateBreaksOnEnterFn(Doc doc)
            {
                if (doc is BreakParent)
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

            void PropagateBreaksOnExitFn(Doc doc)
            {
                if (doc is Group)
                {
                    var group = groupStack.Pop();
                    if (group.Break)
                    {
                        BreakParentGroup();
                    }
                }
            }

            TraverseDoc(document, PropagateBreaksOnEnterFn, PropagateBreaksOnExitFn, true);
        }


        private static Doc traverseDocOnExitStackMarker = new Doc();

        private static void TraverseDoc(Doc document, Func<Doc, bool> onEnter, Action<Doc> onExit, bool shouldTraverseConditionalGroups)
        {
            var docsStack = new Stack<Doc>();
            docsStack.Push(document);

            while (docsStack.Count > 0)
            {
                var doc = docsStack.Pop();

                if (doc == traverseDocOnExitStackMarker)
                {
                    onExit(docsStack.Pop());
                    continue;
                }
                
                docsStack.Push(doc);
                docsStack.Push(traverseDocOnExitStackMarker);

                if (!onEnter(doc)) continue;

                // When there are multiple parts to process,
                // the parts need to be pushed onto the stack in reverse order,
                // so that they are processed in the original order
                // when the stack is popped.
                if (doc is Concat concat) // TODO fill || doc.type == = "fill")
                {
                    for (var x = concat.Parts.Count - 1; x >= 0; --x)
                    {
                        docsStack.Push(concat.Parts[x]);
                    }
                }
                // TODO ifbreak
                // else if (doc.type == = "if-break")
                // {
                //     if (doc.flatContents)
                //     {
                //         docsStack.push(doc.flatContents);
                //     }
                //
                //     if (doc.breakContents)
                //     {
                //         docsStack.push(doc.breakContents);
                //     }
                // }
                else if (doc is Group group)
                {
                    // if (group.ExpandedStates) {
                    // if (shouldTraverseConditionalGroups)
                    // {
                    //     for (let ic = doc.expandedStates.length, i = ic - 1; i >= 0; --i)
                    //     {
                    //         docsStack.push(doc.expandedStates[i]);
                    //     }
                    // }
                    // else
                    // {
                    //     docsStack.push(doc.contents);
                    // }
                    // }
                }

                if (doc is IHasContents hasContents)
                {
                    docsStack.Push(hasContents.Contents);
                }
            }
        }
    }
}