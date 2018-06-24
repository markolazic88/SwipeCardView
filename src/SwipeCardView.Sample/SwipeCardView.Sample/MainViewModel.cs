using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SwipeCardView.Sample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        ObservableCollection<string> cardItems;

        public ObservableCollection<string> CardItems
        {
            get => cardItems;
            set
            {
                cardItems = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            cardItems = new ObservableCollection<string>();
            for (var i = 1; i < 100; i++)
            {
                cardItems.Add($"Card {i}");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
