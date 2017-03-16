using System.Collections.Generic;

namespace BubbleTreeComponent.NodeTypes
{
    public class RootNode<T> : BaseNode<T>
    {
        public List<BaseNode<T>> Children { get; set; }

    }
}