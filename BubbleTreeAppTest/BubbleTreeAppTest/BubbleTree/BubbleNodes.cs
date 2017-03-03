using System.Collections.Generic;
using BubbleTreeAppTest.BubbleTree.NodeTypes;

namespace BubbleTreeAppTest.BubbleTree
{
    public class BubbleNodes<T> : List<BaseNode<T>> where T : ITreeElement
    {
        public readonly List<BaseNode<T>> RawList = new List<BaseNode<T>>();
        public readonly List<T> SourceList;

        public BubbleNodes(List<T> sourceList)
        {
            SourceList = sourceList;
        }

        /// <summary>
        /// Devuelve una lista de los hijos del nodo "node" especificado
        /// </summary>
        /// <param name="node"></param>
        public List<BaseNode<T>> GetAllChildrenRaw(BaseNode<T> node)
        {
            var result = new List<BaseNode<T>>();

            if (node is LeafNode<T>) return result;

            GetChildren(ref result, node);

            return result;
        }

        private void GetChildren(ref List<BaseNode<T>> result, BaseNode<T> node)
        {
            if (node is RootNode<T>)
            {
                result.AddRange(((RootNode<T>) node).Children);
                foreach (var baseNode in ((RootNode<T>)node).Children)
                {
                    GetChildren(ref result,baseNode);
                }
            }
            else if(node is InternalNode<T>)
            {
                result.AddRange(((InternalNode<T>)node).Children);
                foreach (var baseNode in ((InternalNode<T>)node).Children)
                {
                    GetChildren(ref result, baseNode);
                }
            }
        }
    }
}