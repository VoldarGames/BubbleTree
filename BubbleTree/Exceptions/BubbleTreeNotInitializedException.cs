using System;

namespace BubbleTree.ViewModel
{
    public class BubbleTreeNotInitializedException : Exception
    {
        public BubbleTreeNotInitializedException() : base("You must Begin Bubble Tree Configuration and SetSourceItems.") { }
    }
}