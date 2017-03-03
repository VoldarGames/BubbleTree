namespace BubbleTreeAppTest.BubbleTree.NodeTypes
{
    public class LeafNode<T> : BaseNode<T>
    {
        public BaseNode<T> Parent;
        public static explicit operator LeafNode<T>(InternalNode<T> v)
        {
            return new LeafNode<T>()
            {
                Data = v.Data,
                Parent = v.Parent
            };
        }
    }
}