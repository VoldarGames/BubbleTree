using System;
using System.Collections.Generic;
using System.Globalization;
using BubbleTree.ViewModel;
using Xamarin.Forms;

namespace BubbleTreeAppTest
{
    public class App : Application
    {
        public App()
        {
            var bubbleTreeLabel = new Label
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center
            };


            var bubbleTree = new BubbleTree<BubbleTreeItem>();

            
            bubbleTree
                .BeginConfiguration()
                    .SetSourceItems(GetBubbleTreeItemList())
                        .BeginViewConfiguration()
                            .SetBackgroundColor(Color.Black)
                            .SetTitle("Bubble Tree Title")
                                .BeginSearchEntryConfiguration()
                                    .SetPlaceHolderColor(Color.Yellow)
                                    .SetTextColor(Color.Fuchsia)
                                    .SetBackgroundColor(Color.Orange)
                                    .SetFontSize(12)
                                    .SetKeyBoard(Keyboard.Numeric)
                                    .AddCompletedDelegate((sender, args) => {bubbleTreeLabel.TextColor = Color.Lime;})
                                    .SetSearchingOnRootPlaceholderText("Searching on whole tree")
                                    .SetSearchingInPlaceholderText("Search inside of ")
                                .EndSearchEntryConfiguration()
                                .BeginGridConfiguration()
                                    .SetColumnDisplayCount(3)
                                    .SetBubbleDisplayLimitPerSearch(15)
                                .EndGridConfiguration()
                        .EndViewConfiguration()
                .EndConfiguration();

            
                



            

            BindingContext = bubbleTree;
            bubbleTreeLabel.SetBinding(Label.TextProperty, nameof(BubbleTree<BubbleTreeItem>.SelectedItem),BindingMode.Default,new BubbleTreeConverter());


            // The root page of your application
            var content = new ContentPage
            {
                Title = "BubbleTreeAppTest",
               
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            FontSize = 30,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Bubble Tree Component Test!"
                        },
                        bubbleTreeLabel
                    }
                }
            };

            bubbleTreeLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () => await content.Navigation.PushAsync(bubbleTree.GetView()))
            });

            MainPage = new NavigationPage(content);
            
        }

        private List<BubbleTreeItem> GetBubbleTreeItemList()
        {
            return new List<BubbleTreeItem>()
            {
                new BubbleTreeItem()
                {
                    Description = "1. Root Node First",
                    ElementId = 1,
                    ParentElementId = null
                },
                new BubbleTreeItem()
                {
                    Description = "2. Root Node Second",
                    ElementId = 2,
                    ParentElementId = null
                },
                new BubbleTreeItem()
                {
                    Description = "3. Root Node Third",
                    ElementId = 3,
                    ParentElementId = null
                },
                new BubbleTreeItem()
                {
                    Description = "4. Root Node Fourth",
                    ElementId = 4,
                    ParentElementId = null
                },
                new BubbleTreeItem()
                {
                    Description = "5. Root Node Fifth",
                    ElementId = 5,
                    ParentElementId = null
                },
                new BubbleTreeItem()
                {
                    Description = "1.1 Internal Node",
                    ElementId = 6,
                    ParentElementId = 1
                },
                new BubbleTreeItem()
                {
                    Description = "1.2. Internal Node",
                    ElementId = 7,
                    ParentElementId = 1
                },
                new BubbleTreeItem()
                {
                    Description = "1.3 Internal Node",
                    ElementId = 8,
                    ParentElementId = 1
                },
                new BubbleTreeItem()
                {
                    Description = "2.1 Internal Node",
                    ElementId = 9,
                    ParentElementId = 2
                },
                new BubbleTreeItem()
                {
                    Description = "2.2 Internal Node",
                    ElementId = 10,
                    ParentElementId = 2
                },
                new BubbleTreeItem()
                {
                    Description = "3.1 Internal Node",
                    ElementId = 11,
                    ParentElementId = 3
                },
                new BubbleTreeItem()
                {
                    Description = "4.1 Internal Node",
                    ElementId = 12,
                    ParentElementId = 4
                },
                new BubbleTreeItem()
                {
                    Description = "5.1 Internal Node",
                    ElementId = 13,
                    ParentElementId = 5
                },
                new BubbleTreeItem()
                {
                    Description = "1.1.1 Leaf Node",
                    ElementId = 14,
                    ParentElementId = 6
                },
                new BubbleTreeItem()
                {
                    Description = "1.2.1 Leaf Node",
                    ElementId = 15,
                    ParentElementId = 7
                },new BubbleTreeItem()
                {
                    Description = "1.3.1 Leaf Node",
                    ElementId = 16,
                    ParentElementId = 8
                },new BubbleTreeItem()
                {
                    Description = "2.1.1 Leaf Node",
                    ElementId = 17,
                    ParentElementId = 9
                },new BubbleTreeItem()
                {
                    Description = "2.2.1 Leaf Node",
                    ElementId = 18,
                    ParentElementId = 10
                },new BubbleTreeItem()
                {
                    Description = "3.1.1 Leaf Node",
                    ElementId = 19,
                    ParentElementId = 11
                },new BubbleTreeItem()
                {
                    Description = "4.1.1 Leaf Node",
                    ElementId = 20,
                    ParentElementId = 12
                },new BubbleTreeItem()
                {
                    Description = "5.1.1 Leaf Node",
                    ElementId = 21,
                    ParentElementId = 13
                },
                
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public class BubbleTreeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Click here to select an item on a bubble tree component!";
            return ((BubbleTreeItem) value).Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
