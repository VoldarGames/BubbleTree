![BubbleTreeIcon](BubbleTreeIcon.png)
# BubbleTree

Xamarin Forms Tree View Component 
Created and written by Dani Garcia Sanchez (https://www.linkedin.com/in/dani-garcia-sanchez-30b684138)
## Introduction

BubbleTree helps XamarinForms programmers to represent 'Tree Structures' in mobile apps with a new component.
It allows you to search an element in a big tree structure in an easy and intuitive way.
With BubbleTree you can navigate through your nodes by simply clicking them and selecting the leaf node you want.

I hope you like it !

## Getting started

Download from nuget.org [BubbleTree](https://www.nuget.org/packages/BubbleTree/1.0.0) Component

## How to use it

First you need to have a class that implements ITreeElement interface.

  ```csharp
    public class BubbleTreeItem : ITreeElement
    {
        public int ElementId { get; set; }
        public int? ParentElementId { get; set; }
        public string Description { get; set; }
    }
  ```
  
  #### ElementId is the primary key.
  #### ParentElementId is the primary key of its parent.  (null if it is a root element)
  #### Description is the text shown on bubbles.
Next, create a source list from scratch or from a typically database tree representation.

```csharp
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
                    Description = "1.1 Internal Node",
                    ElementId = 2,
                    ParentElementId = 1
                },

                new BubbleTreeItem()
                {
                    Description = "1.1.1 Leaf Node",
                    ElementId = 3,
                    ParentElementId = 2
                }
                
            };
        }
```
## Configuration

BubbleTree can be configured or not but setting source items is mandatory, if you don't set it bubble tree will throw an Exception.

### Default configuration

BubbleTree will show with its default style. This is how you initialize it.

  ```csharp
  var defaultBubbleTree = new BubbleTree<BubbleTreeItem>();
  
      defaultBubbleTree
          .BeginConfiguration()
              .SetSourceItems(GetBubbleTreeItemList())
          .EndConfiguration();
  ```
  
### Custom configuration

BubbleTree can be customizable with a powerful guided fluent interface. This is how it looks! Easy to understand ! Awesome!

```csharp
var customizedBubbleTree = new BubbleTree<BubbleTreeItem>();

            customizedBubbleTree
                .BeginConfiguration()
                    .SetSourceItems(GetBubbleTreeItemList())
                        .BeginViewConfiguration()
                            .SetBackgroundColor(Color.FromRgb(180, 180, 130))
                            .SetTitle("Bubble Tree Title")
                            .OnSelectedItemChanged(() => customizedBubbleTreeLabel.TextColor = Color.Lime)
                                .BeginToolBarItemsConfiguration()
                                    .SetGoRootIconFile("ToRootIcon.png")
                                    .SetGoUpIconFile("ToParentIcon.png")
                                    .SetGoRootText("Go Root")
                                    .SetGoUpText("Go Up")
                                .EndToolBarItemsConfiguration()
                                .BeginSearchEntryConfiguration()
                                    .SetPlaceHolderColor(Color.FromRgb(20, 20, 20))
                                    .SetTextColor(Color.Black)
                                    .SetBackgroundColor(Color.FromRgb(170, 170, 120))
                                    .SetFontSize(14)
                                    .SetKeyBoard(Keyboard.Text)
                                    .AddCompletedDelegate((sender, args) => {customizedBubbleTreeLabel.TextColor = Color.Blue;})
                                    .SetSearchingOnRootPlaceholderText("Searching on whole tree")
                                    .SetSearchingInPlaceholderText("Searching inside of ")
                                .EndSearchEntryConfiguration()
                                .BeginGridConfiguration()
                                    .SetColumnDisplayCount(3)
                                    .SetBubbleDisplayLimitPerSearch(15)
                                .EndGridConfiguration()
                                .BeginRootNodesConfiguration()
                                    .SetTextColor(Color.White)
                                    .SetFontSize(14)
                                    .SetBorderColor(Color.FromRgb(120, 0, 0))
                                    .SetBorderWidth(3)
                                    .SetBorderRadius(50)
                                    .SetBackgroundColor(Color.FromRgb(60, 0, 0))
                                    .SetImageConfiguration("RootNodeIcon.png", Button.ButtonContentLayout.ImagePosition.Top)
                                .EndRootNodesConfiguration()
                                .BeginInternalNodesConfiguration()
                                    .SetTextColor(Color.White)
                                    .SetFontSize(13)
                                    .SetBorderColor(Color.FromRgb(0, 120, 0))
                                    .SetBorderWidth(3)
                                    .SetBorderRadius(30)
                                    .SetBackgroundColor(Color.FromRgb(0, 60, 0))
                                    .SetImageConfiguration("InternalNodeIcon.png", Button.ButtonContentLayout.ImagePosition.Right)
                                .EndInternalNodesConfiguration()
                                .BeginLeafNodesConfiguration()
                                    .SetTextColor(Color.White)
                                    .SetFontSize(12)
                                    .SetBorderColor(Color.FromRgb(0, 0, 120))
                                    .SetBorderWidth(3)
                                    .SetBorderRadius(5)
                                    .SetBackgroundColor(Color.FromRgb(0, 0, 60))
                                    .SetImageConfiguration("LeafNodeIcon.png", Button.ButtonContentLayout.ImagePosition.Bottom)
                                .EndLeafNodesConfiguration()
                        .EndViewConfiguration()
                .EndConfiguration();
```

## Show it !

Finally you need to call the view with your navigation whatever you want ! Here's an simple example shown in this repository sample.

```csharp
var defaultBubbleTreeLabel = new Label(){Text = "Go To Tree!"};

defaultBubbleTreeLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () => await content.Navigation.PushAsync(defaultBubbleTree.GetView()))
            });
```

## Bind selected item!

Here's an useful example of binding our selected bubble node to a label text property!

#### Binding
```csharp
defaultBubbleTreeLabel.BindingContext = defaultBubbleTree;
            defaultBubbleTreeLabel.SetBinding(Label.TextProperty, nameof(BubbleTree<BubbleTreeItem>.SelectedItem), BindingMode.Default, new BubbleTreeConverter());
```

#### Converter

```csharp
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
  ```
# Warning 

Due to a known bug of Xamarin Forms, you will need to put this code on your Droid project if you want to customize bubble border radius!

```csharp
//This line outside of namespace 
[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(GenericButtonRenderer))]

//This line whatever you want on your Droid Project.
 public class GenericButtonRenderer : Xamarin.Forms.Platform.Android.ButtonRenderer{}
```
  
