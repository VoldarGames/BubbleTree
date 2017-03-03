using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BubbleTreeAppTest.Annotations;
using BubbleTreeAppTest.BubbleTree.View;
using Xamarin.Forms;

namespace BubbleTreeAppTest.BubbleTree.ViewModel
{
    public class BubbleTreeViewModel<T> : INotifyPropertyChanged where T : ITreeElement
    {
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

        public virtual Page GetView()
        {
           return new BubbleTreeView<T>(SourceItems)
           {
               BindingContext = this,
               BackgroundColor = Color.Black
           };
        }

        public List<T> SourceItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}