using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;

namespace SwipeCardView.Sample.ViewModel
{
    public class ColorsPageViewModel : BasePageViewModel
    {

        private ObservableCollection<string> _cardItems;

        public ObservableCollection<string> CardItems
        {
            get => _cardItems;
            set
            {
                _cardItems = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand SwipedCommand { get; }

        public ICommand DraggingCommand { get; }

        public ICommand ClearItemsCommand { get; }

        public ICommand AddItemsCommand { get; }

        public ColorsPageViewModel()
        {
            _cardItems = new ObservableCollection<string>();
            for (var i = 1; i <= 5; i++)
            {
                _cardItems.Add($"Card {i}");
            }

            this.SwipedCommand = new Command<SwipedCardEventArgs>(this.OnSwipedCommand);
            this.DraggingCommand = new Command<DraggingCardEventArgs>(this.OnDraggingCommand);

            this.ClearItemsCommand = new Command(this.OnClearItemsCommand);
            this.AddItemsCommand = new Command(this.OnAddItemsCommand);
        }

        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
        }

        private void OnDraggingCommand(DraggingCardEventArgs eventArgs)
        {
            switch (eventArgs.Position)
            {
                case DraggingCardPosition.Start:
                    return;
                case DraggingCardPosition.UnderThreshold:
                    break;
                case DraggingCardPosition.OverThreshold:
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    return;
                case DraggingCardPosition.FinishedOverThreshold:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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
