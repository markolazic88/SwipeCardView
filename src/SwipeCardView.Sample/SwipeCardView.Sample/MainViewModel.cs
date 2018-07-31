using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace SwipeCardView.Sample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        ObservableCollection<string> cardItems;

        private ICommand clearItemsCommand;

        private ICommand addItemsCommand;
        
        public ObservableCollection<string> CardItems
        {
            get => cardItems;
            set
            {
                cardItems = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ClearItemsCommand
        {
            get { return this.clearItemsCommand ?? (this.clearItemsCommand = new Command(this.OnClearItemsCommand)); }
        }
        public ICommand AddItemsCommand
        {
            get { return this.addItemsCommand ?? (this.addItemsCommand = new Command(this.OnAddItemsCommand)); }
        }

        public MainViewModel()
        {
            cardItems = new ObservableCollection<string>();
            for (var i = 1; i <= 5; i++)
            {
                cardItems.Add($"Card {i}");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void OnClearItemsCommand()
        {
            this.CardItems.Clear();
        }

        private void OnAddItemsCommand()
        {
            for (var i = 1; i <= 5; i++)
            {
                CardItems.Add($"Card {i}");
            }
        }
    }
}
