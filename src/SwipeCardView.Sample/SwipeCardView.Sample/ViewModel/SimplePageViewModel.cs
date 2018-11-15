using System.Collections.ObjectModel;
using System.Windows.Input;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;

namespace SwipeCardView.Sample.ViewModel
{
    public class SimplePageViewModel : BasePageViewModel
    {
        private ObservableCollection<string> _cardItems;

        private string _message;

        public ObservableCollection<string> CardItems
        {
            get => _cardItems;
            set
            {
                _cardItems = value;
                this.RaisePropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand SwipedCommand { get; }

        public ICommand ClearItemsCommand { get; }

        public ICommand AddItemsCommand { get; }

        public SimplePageViewModel()
        {
            _cardItems = new ObservableCollection<string>();
            for (var i = 1; i <= 5; i++)
            {
                _cardItems.Add($"Card {i}");
            }

            this.SwipedCommand = new Command<SwipedCardEventArgs>(this.OnSwipedCommand);

            this.ClearItemsCommand = new Command(this.OnClearItemsCommand);
            this.AddItemsCommand = new Command(this.OnAddItemsCommand);
        }

        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
            var item = eventArgs.Item as string;
            this.Message = $"{item} swiped {eventArgs.Direction}";
        }

        private void OnClearItemsCommand()
        {
            this.CardItems.Clear();
            this.Message = string.Empty;
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
