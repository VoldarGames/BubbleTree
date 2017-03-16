using BubbleTreeComponent;

namespace BubbleTreeAppTest
{
    public class BubbleTreeItem : ITreeElement
    {
        public int ElementId { get; set; }
        public int? ParentElementId { get; set; }
        public string Description { get; set; }
    }
}