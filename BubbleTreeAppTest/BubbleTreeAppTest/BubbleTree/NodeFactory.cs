using System;
using System.Collections.Generic;
using System.Linq;
using BubbleTreeAppTest.BubbleTree.NodeTypes;

namespace BubbleTreeAppTest.BubbleTree
{
    public static class NodeFactory<T> where T : ITreeElement
    {
        public static BubbleNodes<T> Result;
        public static BubbleNodes<T> CreateFromSource<TKey>(List<T> sourceList, Func<BaseNode<T>,TKey> keySelector, bool orderDescending)
        {
            Result = new BubbleNodes<T>(sourceList);

            foreach (var sourceItem in sourceList.Where(c => c.ParentElementId == null))
            {
                var parent = new RootNode<T> { Data = sourceItem };
                var internalNodes = SearchInternalNodes(sourceList, parent);
                parent.Children = new List<BaseNode<T>>();
                parent.Children.AddRange(internalNodes);
                Result.Add(parent);
                Result.RawList.Add(parent);
            }
            if(orderDescending) Result.RawList.OrderByDescending(keySelector);
            else Result.RawList.OrderBy(keySelector);
            return Result;

        }

        private static List<BaseNode<T>> SearchInternalNodes(IEnumerable<T> sourceList, BaseNode<T> parent)
        {
            var result = new List<BaseNode<T>>();
            var parentItemChildren = sourceList.Where(c => c.ParentElementId == parent.Data.ElementId);
            foreach (var itemChild in parentItemChildren)
            {
                var internalNode = new InternalNode<T>
                {
                    Data = itemChild,
                    Children = new List<BaseNode<T>>(),
                    Parent = parent

                };

                var descendants = SearchInternalNodes(sourceList, internalNode);

                if (descendants.Count == 0)
                {
                    var interLeafNode = (LeafNode<T>)internalNode;
                    result.Add(interLeafNode);
                    Result.RawList.Add(interLeafNode);
                }
                else
                {
                    internalNode.Children.AddRange(descendants);
                    Result.RawList.Add(internalNode);
                    result.Add(internalNode);
                }
            };



            return result;
        }

    }
}