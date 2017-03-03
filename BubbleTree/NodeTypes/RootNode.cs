using System.Collections.Generic;

namespace BubbleTree.NodeTypes
{
    public class RootNode<T> : BaseNode<T>
    {
        public List<BaseNode<T>> Children { get; set; }

    }
}