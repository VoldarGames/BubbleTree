namespace BubbleTreeAppTest.BubbleTree
{
    public interface ITreeElement
    {
        int ElementId { get; set; }
        int? ParentElementId { get; set; }

        string Description { get; set; }
    }
}