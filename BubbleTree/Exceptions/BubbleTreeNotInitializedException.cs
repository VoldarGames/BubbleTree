using System;

namespace BubbleTreeComponent.Exceptions
{
    public class BubbleTreeNotInitializedException : Exception
    {
        public BubbleTreeNotInitializedException() : base("You must Begin Bubble Tree Configuration and SetSourceItems.") { }
    }
}