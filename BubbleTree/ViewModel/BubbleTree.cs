using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BubbleTree.Properties;
using BubbleTree.View;
using Xamarin.Forms;

namespace BubbleTree.ViewModel
{
    /// <summary>
    /// BubbleTree ViewModel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BubbleTree<T> : INotifyPropertyChanged where T : ITreeElement
    {
        private BubbleTreeConfiguration<T> _bubbleTreeConfiguration;

        private BubbleTreeView<T> _bubbleTreeView;

        private T _selectedItem;
        public T SelectedItem
        {
            get { return _selectedItem;}
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(SelectedItem.Description));
            }
        }

        public BubbleTreeView<T> GetView()
        {
            return _bubbleTreeView;
        }

       

        private List<T> SourceItems { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BubbleTreeConfiguration<T> BeginConfiguration()
        {
            _bubbleTreeConfiguration = new BubbleTreeConfiguration<T>(this);
            return _bubbleTreeConfiguration;
        }

        public class BubbleTreeConfiguration<T> where T : ITreeElement
        {
            private readonly BubbleTree<T> _bubbleTreeViewModel;
            private BubbleTreeViewConfiguration<T> _bubbleTreeViewConfiguration;

            public BubbleTreeConfiguration(BubbleTree<T> bubbleTreeViewModel)
            {
                _bubbleTreeViewModel = bubbleTreeViewModel;
            }
            public BubbleTreeConfiguration<T> SetSourceItems(List<T> treeElements)
            {
                _bubbleTreeViewModel.SourceItems = treeElements;

                _bubbleTreeViewModel._bubbleTreeView = new BubbleTreeView<T>(treeElements)
                {
                    BindingContext = _bubbleTreeViewModel
                };
                return this;
            }

            public BubbleTreeViewConfiguration<T> BeginViewConfiguration()
            {
                _bubbleTreeViewConfiguration = new BubbleTreeViewConfiguration<T>(_bubbleTreeViewModel._bubbleTreeView, this);
                return _bubbleTreeViewConfiguration;
            }

            public BubbleTree<T> EndConfiguration()
            {
                return _bubbleTreeViewModel;
            }
        }

        public class BubbleTreeViewConfiguration<T> where T : ITreeElement
        {
            private readonly BubbleTreeView<T> _bubbleTreeView;
            private readonly BubbleTreeConfiguration<T> _previousConfiguration;
            private BubbleTreeSearchEntryConfiguration<T> _bubbleTreeSearchEntryConfiguration;
            private BubbleTreeGridConfiguration<T> _bubbleTreeGridConfiguration;

            public BubbleTreeViewConfiguration(BubbleTreeView<T> bubbleTreeView, BubbleTreeConfiguration<T> previousConfiguration)
            {
                if (bubbleTreeView == null) throw new BubbleTreeNotInitializedException();
                _bubbleTreeView = bubbleTreeView;
                _previousConfiguration = previousConfiguration;
            }

            public BubbleTreeViewConfiguration<T> SetBackgroundColor(Color color)
            {
                _bubbleTreeView.BackgroundColor = color;
                return this;
            }

            public BubbleTreeViewConfiguration<T> SetIcon(FileImageSource fileImageSource)
            {
                _bubbleTreeView.Icon = fileImageSource;
                return this;
            }

            public BubbleTreeViewConfiguration<T> SetTitle(string title)
            {
                _bubbleTreeView.Title = title;
                return this;
            }

            public BubbleTreeViewConfiguration<T> SetBackgroundImage(string backgroundImageFile)
            {
                _bubbleTreeView.BackgroundImage = backgroundImageFile;
                return this;
            }

            public BubbleTreeConfiguration<T> EndViewConfiguration()
            {
                return _previousConfiguration;
            }


            public BubbleTreeSearchEntryConfiguration<T> BeginSearchEntryConfiguration()
            {
                _bubbleTreeSearchEntryConfiguration = new BubbleTreeSearchEntryConfiguration<T>(_bubbleTreeView.SearchEntry, this);
                return _bubbleTreeSearchEntryConfiguration;
            }

            public BubbleTreeGridConfiguration<T> BeginGridConfiguration()
            {
                _bubbleTreeGridConfiguration = new BubbleTreeGridConfiguration<T>(_bubbleTreeView, this);
                return _bubbleTreeGridConfiguration;
            }

            public class BubbleTreeSearchEntryConfiguration<T> where T : ITreeElement
            {
                private readonly BubbleTreeViewConfiguration<T> _previousConfiguration;
                private readonly Entry _searchEntry;
                public BubbleTreeSearchEntryConfiguration(Entry searchEntry, BubbleTreeViewConfiguration<T> previousConfiguration)
                {
                    _previousConfiguration = previousConfiguration;
                    _searchEntry = searchEntry;
                }

                public BubbleTreeSearchEntryConfiguration<T> SetPlaceHolderColor(Color color)
                {
                    _searchEntry.PlaceholderColor = color;
                    return this;
                }

                public BubbleTreeSearchEntryConfiguration<T> SetTextColor(Color color)
                {
                    _searchEntry.TextColor = color;
                    return this;
                }

                public BubbleTreeSearchEntryConfiguration<T> SetBackgroundColor(Color color)
                {
                    _searchEntry.BackgroundColor = color;
                    return this;
                }

                public BubbleTreeSearchEntryConfiguration<T> SetFontSize(double fontSize)
                {
                    _searchEntry.FontSize = fontSize;
                    return this;
                }

                
                public BubbleTreeSearchEntryConfiguration<T> SetKeyBoard(Keyboard keyboard)
                {
                    _searchEntry.Keyboard = keyboard;
                    return this;
                }

                public BubbleTreeSearchEntryConfiguration<T> AddCompletedDelegate(EventHandler completedDelegate)
                {
                    if (completedDelegate == null) throw new NullReferenceException("Completed Delegate must be initialized.");
                    _searchEntry.Completed += completedDelegate;
                    return this;
                }

                public BubbleTreeSearchEntryConfiguration<T> AddTextChangedDelegate(EventHandler textChangedDelegate)
                {
                    if (textChangedDelegate == null) throw new NullReferenceException("TextChanged Delegate must be initialized.");
                    _searchEntry.Completed += textChangedDelegate;
                    return this;
                }

                public BubbleTreeSearchEntryConfiguration<T> SetSearchingOnRootPlaceholderText(string text)
                {
                    _previousConfiguration._bubbleTreeView.SearchingOnRootText = text;
                    return this;
                }

                public BubbleTreeSearchEntryConfiguration<T> SetSearchingInPlaceholderText(string text)
                {
                    _previousConfiguration._bubbleTreeView.SearchingInText = text;
                    return this;
                }

                public BubbleTreeViewConfiguration<T> EndSearchEntryConfiguration()
                {
                    _previousConfiguration._bubbleTreeView.SearchEntry.Placeholder = _previousConfiguration._bubbleTreeView.SearchingOnRootText;
                    return _previousConfiguration;
                }
            }
            public class BubbleTreeGridConfiguration<T> where T : ITreeElement
            {
                private readonly BubbleTreeViewConfiguration<T> _previousConfiguration;
                private readonly BubbleTreeView<T> _bubbleTreeView;
                public BubbleTreeGridConfiguration(BubbleTreeView<T> bubbleTreeView, BubbleTreeViewConfiguration<T> previousConfiguration)
                {
                    _bubbleTreeView = bubbleTreeView;
                    _previousConfiguration = previousConfiguration;
                }

                public BubbleTreeGridConfiguration<T> SetColumnDisplayCount(int nColumns)
                {
                    _bubbleTreeView.DisplayNumberOfColumns = nColumns;
                    return this;
                }

                public BubbleTreeGridConfiguration<T> SetBubbleDisplayLimitPerSearch(int nBubbles)
                {
                    _bubbleTreeView.DisplayLimit = nBubbles;
                    return this;
                }

                public BubbleTreeViewConfiguration<T> EndGridConfiguration()
                {
                    return _previousConfiguration;
                }
            }

        }
    }

   
}
