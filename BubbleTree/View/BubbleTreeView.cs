using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BubbleTreeComponent.NodeTypes;
using BubbleTreeComponent.ViewModel;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("BubbleTree")]
namespace BubbleTreeComponent.View
{

    public class BubbleTreeView<T> : ContentPage where T : ITreeElement
    {
        private StackLayout Container;
        private StackLayout SearchContainer;
        private Grid BubbleContainer;

        public int DisplayNumberOfColumns = 3;
        public int DisplayLimit = 15;

        public string SearchingOnRootText = "Searching on root...";
        public string SearchingInText = "Searching in...";

        private bool _selectionModeEnabled;

        private bool SelectionModeEnabled
        {
            get { return _selectionModeEnabled; }
            set
            {
                _selectionModeEnabled = value;
                ToolBarItemSelectionMode.Text = (value) ? "Selection Mode" : "Navigation Mode";
            }
        }



        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Entry SearchEntry = new Entry()
        {
            //TODO: Style
            Text = "",
            TextColor = Color.Gray,
            PlaceholderColor = Color.Silver
        };

        #region ToolBarItems

        public ToolbarItem ToolBarItemSelectionMode { get; set; }



        public string GoRootName = "To Root";
        public string GoUpName = "To parent";
        public string GoRootText = "Root";
        public string GoUpText = "Up";
        public string GoRootIconFile = "ToRootIcon.png";
        public string GoUpIconFile = "ToParentIcon.png";
        #endregion

        #region BubbleButtonCustomizableProperties

        #region BubbleRootButton

        public Color RootTextColor = Color.White;
        public double RootFontSize = 12;
        public Color RootBorderColor = Color.Navy;
        public double RootBorderWidth = 3.0;
        public int RootBorderRadius = 30;
        public Color RootBackgroundColor = Color.FromRgb(5, 5, 120);
        public FileImageSource RootImageSource = new FileImageSource();
        public Button.ButtonContentLayout RootContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 0);



        #endregion

        #region BubbleInternalButton

        public Color InternalTextColor = Color.White;
        public double InternalFontSize = 12;
        public Color InternalBorderColor = Color.Navy;
        public double InternalBorderWidth = 3.0;
        public int InternalBorderRadius = 30;
        public Color InternalBackgroundColor = Color.FromRgb(0, 35, 180);
        public FileImageSource InternalImageSource = new FileImageSource();
        public Button.ButtonContentLayout InternalContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 0);




        #endregion

        #region BubbleLeafButton

        public Color LeafTextColor = Color.White;
        public double LeafFontSize = 12;
        public Color LeafBorderColor = Color.Navy;
        public double LeafBorderWidth = 3.0;
        public int LeafBorderRadius = 5;
        public Color LeafBackgroundColor = Color.FromRgb(30, 120, 200);
        public FileImageSource LeafImageSource = new FileImageSource();
        public Button.ButtonContentLayout LeafContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 0);


        #endregion


        #endregion

        public ToolbarItem GoToParentToolbarItem;

        public BubbleNodes<T> BubbleNodes;

        private BaseNode<T> _currentSearchFilterNode;

        public BaseNode<T> CurrentSearchFilterNode
        {
            get { return _currentSearchFilterNode; }
            set
            {
                _currentSearchFilterNode = value;

                SearchEntry.TextChanged -= OnTextChangedDoSearch();
                SearchEntry.Text = "";
                SearchEntry.TextChanged += OnTextChangedDoSearch();

                SearchEntry.Placeholder = _currentSearchFilterNode != null ? $"{SearchingInText}{_currentSearchFilterNode.Data?.Description}" : SearchingOnRootText;

                RefreshNodeFilter();

                if (_currentSearchFilterNode != null && !ToolbarItems.Contains(GoToParentToolbarItem)) ToolbarItems.Add(GoToParentToolbarItem);
            }
        }

        private void RefreshNodeFilter()
        {
            if (_currentSearchFilterNode == null)
            {
                ToolbarItems.Remove(GoToParentToolbarItem);
                Container.Children.Remove(BubbleContainer);
                BubbleContainer = new Grid();
                PrepareBubbleContainer(BubbleNodes.RawList.OfType<RootNode<T>>().Count(), DisplayNumberOfColumns);
                Container.Children.Add(BubbleContainer);
            }
            else if (_currentSearchFilterNode is InternalNode<T>)
            {
                Container.Children.Remove(BubbleContainer);
                BubbleContainer = new Grid();
                PrepareBubbleContainer(((InternalNode<T>)_currentSearchFilterNode).Children.Count, DisplayNumberOfColumns, ((InternalNode<T>)_currentSearchFilterNode).Children);
                Container.Children.Add(BubbleContainer);
            }
            else if (_currentSearchFilterNode is RootNode<T>)
            {
                Container.Children.Remove(BubbleContainer);
                BubbleContainer = new Grid();
                PrepareBubbleContainer(((RootNode<T>)_currentSearchFilterNode).Children.Count, DisplayNumberOfColumns, ((RootNode<T>)_currentSearchFilterNode).Children);
                Container.Children.Add(BubbleContainer);
            }
        }

        public BubbleTreeView(List<T> sourceItems)
        {
            ToolBarItemSelectionMode = new ToolbarItem("Navigation Mode", "", () =>
            {
                SelectionModeEnabled = !SelectionModeEnabled;
            }, ToolbarItemOrder.Primary);

            ToolbarItems.Add(ToolBarItemSelectionMode);

            var scrollContainer = new ScrollView();

            scrollContainer.Scrolled += (sender, args) =>
            {
                var scrollSpace = scrollContainer.ContentSize.Height - scrollContainer.Height - 20;
                if (scrollSpace <= args.ScrollY)
                {
                    //TODO: Add Scroll Pagination
                    //Container.Children.Add(new Label() { TextColor = Color.White, Text = "Another Element!" });
                    //Container.Children.Add(new Label() { TextColor = Color.White, Text = "Another Element2!" });
                    //Container.Children.Add(new Label() { TextColor = Color.White, Text = "Another Element3!" });
                }
            };

            Container = new StackLayout();
            scrollContainer.Content = Container;
            SearchContainer = new StackLayout();
            BubbleContainer = new Grid();

            Container.Children.Add(SearchContainer);
            Container.Children.Add(BubbleContainer);

            BubbleNodes = NodeFactory<T>.CreateFromSource(sourceItems, node => node.Data.Description, orderDescending: true);
            SearchEntry.TextChanged += OnTextChangedDoSearch();
            SearchEntry.Placeholder = SearchingOnRootText;

            Content = scrollContainer;
            SearchContainer.Children.Add(SearchEntry);

            //var rootNodesCount = BubbleNodes.RawList.OfType<RootNode<T>>().Count();
            //PrepareBubbleContainer(rootNodesCount, DisplayNumberOfColumns);

        }




        private EventHandler<TextChangedEventArgs> OnTextChangedDoSearch()
        {
            return (sender, args) =>
            {
                Interlocked.Exchange(ref _cancellationTokenSource, new CancellationTokenSource()).Cancel();
                Task.Delay(TimeSpan.FromMilliseconds(125), _cancellationTokenSource.Token).ContinueWith(delegate
                {
                    SearchInBubbleTree(DisplayLimit);
                },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext()
                    );

            };
        }

        private void SearchInBubbleTree(int displayLimit)
        {
            IEnumerable<BaseNode<T>> searchResultNodes;

            Container.Children.Remove(BubbleContainer);
            BubbleContainer = new Grid();
            if (SearchEntry.Text == "" && _currentSearchFilterNode == null)
            {
                PrepareBubbleContainer(BubbleNodes.RawList.OfType<RootNode<T>>().Count(), DisplayNumberOfColumns);
            }
            else
            {

                if (_currentSearchFilterNode == null)
                {
                    searchResultNodes =
                        BubbleNodes.RawList.Where(
                            node => node.Data.Description != null && node.Data.Description.ToUpper().Contains(SearchEntry.Text.ToUpper()))
                            .Take(displayLimit).Reverse();
                }
                else
                {
                    var allChildren = BubbleNodes.GetAllChildrenRaw(CurrentSearchFilterNode);
                    searchResultNodes = allChildren.Where(
                        node => node.Data.Description != null && node.Data.Description.ToUpper().Contains(SearchEntry.Text.ToUpper()))
                        .Take(displayLimit).Reverse();
                }
                PrepareBubbleContainer(searchResultNodes.Count(), DisplayNumberOfColumns, searchResultNodes);
            }

            Container.Children.Add(BubbleContainer);


        }

        private void PrepareBubbleContainer(int nodesCount, int numberOfColums, IEnumerable<BaseNode<T>> nodes)
        {
            int nRows = (int)Math.Ceiling((float)nodesCount / numberOfColums);
            for (var i = 0; i < nRows; i++)
            {
                BubbleContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (var i = 0; i < (nodesCount < numberOfColums ? nodesCount : numberOfColums); i++)
            {
                BubbleContainer.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }

            PopulateBubbleContainer(nRows, numberOfColums, nodes);

        }
        internal void PrepareBubbleContainer(int nodesCount, int numberOfColums)
        {
            int nRows = (int)Math.Ceiling((float)nodesCount / numberOfColums);
            for (var i = 0; i < nRows; i++)
            {
                BubbleContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (var i = 0; i < (nodesCount < numberOfColums ? nodesCount : numberOfColums); i++)
            {
                BubbleContainer.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }

            PopulateBubbleContainer(nRows, numberOfColums);

        }

        /// <summary>
        /// Populates the bubble container with Root nodes.
        /// </summary>
        /// <param name="numberOfRows">The number of rows that bubble container have.</param>
        /// <param name="numberOfColums">The number of columns that bubble container have.</param>
        private void PopulateBubbleContainer(int numberOfRows, int numberOfColums)
        {
            var rootNodes = BubbleNodes.RawList.OfType<RootNode<T>>();
            int rowIterator = 0;
            int columnIterator = 0;

            foreach (var node in rootNodes.OrderBy(node => node.Data.Description))
            {
                BubbleContainer.Children.Add(new Bubble<RootNode<T>, T>(node, new Button()
                {
                    Style = BubbleStyle(),
                    TextColor = RootTextColor,
                    FontSize = RootFontSize,
                    Text = node.Data.Description,
                    HeightRequest = 100,
                    WidthRequest = 100,
                    BorderColor = RootBorderColor,
                    BorderWidth = RootBorderWidth,
                    CornerRadius = RootBorderRadius,
                    BackgroundColor = RootBackgroundColor,
                    Image = RootImageSource,
                    ContentLayout = RootContentLayout,
                    Command = new Command(() =>
                    {
                        if (!SelectionModeEnabled)
                        {
                            CurrentSearchFilterNode = node;
                        }
                        else
                        {
                            ((BubbleTree<T>)BindingContext).SelectedItem = node.Data;
                            OnPropertyChanged(nameof(BubbleTree<T>.SelectedItem));
                            Navigation.PopAsync();
                        }
                    })
                }), columnIterator, rowIterator);
                columnIterator++;
                columnIterator %= numberOfColums;
                if (columnIterator == 0)
                {
                    rowIterator++;
                    rowIterator %= numberOfRows;
                }
            }
        }

        private void PopulateBubbleContainer(int numberOfRows, int numberOfColums, IEnumerable<BaseNode<T>> nodes)
        {

            int rowIterator = 0;
            int columnIterator = 0;
            foreach (var node in (SearchEntry.Text == "") ? nodes.OrderBy(node => node.Data.Description) : nodes)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    InsertNode(node, rowIterator, columnIterator).Wait();
                    columnIterator++;
                    columnIterator %= numberOfColums;
                    if (columnIterator == 0)
                    {
                        rowIterator++;
                        rowIterator %= numberOfRows;
                    }
                });

            }
        }

        private async Task<bool> InsertNode(BaseNode<T> node, int rowIterator, int columnIterator)
        {
            var bubbleButton = new Button()
            {
                Style = BubbleStyle(),
                FontSize = InternalFontSize,
                Text = node.Data.Description,
                TextColor = InternalTextColor,
                HeightRequest = 100,
                WidthRequest = 100,
                BorderColor = InternalBorderColor,
                BorderWidth = InternalBorderWidth,
                CornerRadius = InternalBorderRadius,
                BackgroundColor = InternalBackgroundColor,
                Image = InternalImageSource,
                ContentLayout = InternalContentLayout,


                Command = new Command(() =>
                {
                    if (!SelectionModeEnabled)
                    {
                        CurrentSearchFilterNode = node;
                    }
                    else
                    {
                        ((BubbleTree<T>)BindingContext).SelectedItem = node.Data;
                        OnPropertyChanged(nameof(BubbleTree<T>.SelectedItem));
                        Navigation.PopAsync();
                    }
                })
            };
            if (node is RootNode<T>)
            {
                bubbleButton.FontSize = RootFontSize;
                bubbleButton.TextColor = RootTextColor;
                bubbleButton.BorderColor = RootBorderColor;
                bubbleButton.BorderWidth = RootBorderWidth;
                bubbleButton.CornerRadius = RootBorderRadius;
                bubbleButton.BackgroundColor = RootBackgroundColor;
                bubbleButton.Image = RootImageSource;
                bubbleButton.ContentLayout = RootContentLayout;


            }
            else if (node is LeafNode<T>)
            {
                bubbleButton.FontSize = LeafFontSize;
                bubbleButton.TextColor = LeafTextColor;
                bubbleButton.BorderColor = LeafBorderColor;
                bubbleButton.BorderWidth = LeafBorderWidth;
                bubbleButton.CornerRadius = LeafBorderRadius;
                bubbleButton.BackgroundColor = LeafBackgroundColor;
                bubbleButton.Image = LeafImageSource;
                bubbleButton.ContentLayout = LeafContentLayout;
                bubbleButton.Command = new Command(() =>
                {
                    ((BubbleTree<T>)BindingContext).SelectedItem = node.Data;
                    OnPropertyChanged(nameof(BubbleTree<T>.SelectedItem));
                    Navigation.PopAsync();
                });
            }
            BubbleContainer.Children.Add(new Bubble<BaseNode<T>, T>(node, bubbleButton), columnIterator, rowIterator);

            return true;

        }


        internal class Bubble<TNode, TEntity> : StackLayout where TNode : BaseNode<TEntity> where TEntity : ITreeElement
        {
            public Button SelectionButton;
            public TNode Node;

            public Bubble(TNode node, Button selectionButton)
            {
                Node = node;
                SelectionButton = selectionButton;

                Padding = new Thickness(1, 1, 1, 1);

                Children.Add(SelectionButton);
                AppearAnimation();

            }

            private async void AppearAnimation()
            {
                var rand = new Random(DateTime.Now.Millisecond);
                await SelectionButton.ScaleTo(0, 0U);
                await SelectionButton.ScaleTo(0, 100U);
                await SelectionButton.FadeTo(0.0f, 0U);
                await SelectionButton.FadeTo(0.0f, 100U);
                await SelectionButton.FadeTo(1.0, 800U);
                await SelectionButton.ScaleTo(1.2, (uint)rand.Next(300, 600), Easing.CubicInOut);
                await SelectionButton.ScaleTo(0.9, 150U, Easing.Linear);
                await SelectionButton.ScaleTo(1.0, 150U, Easing.Linear);
            }

        }

        public static Style BubbleStyle()
        {
            var bubbleStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter {Property = BackgroundColorProperty, Value = Color.FromRgba(38, 80, 180,255)},
                    new Setter {Property = Button.TextColorProperty, Value = Color.FromRgb(255, 255,255)},
                    new Setter {Property = Button.CornerRadiusProperty, Value = 0},
                    new Setter {Property = HeightRequestProperty, Value = 42},

                }
            };

            return bubbleStyle;
        }
    }
}