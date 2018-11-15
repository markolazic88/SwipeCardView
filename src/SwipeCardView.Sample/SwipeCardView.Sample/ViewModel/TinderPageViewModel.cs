using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SwipeCardView.Sample.Model;
using Xamarin.Forms;

namespace SwipeCardView.Sample.ViewModel
{
    public class TinderPageViewModel : BasePageViewModel
    {
        private ObservableCollection<Profile> _profiles = new ObservableCollection<Profile>();

        private uint _threshold;

        public TinderPageViewModel()
        {
            InitializeProfiles();

            Threshold = (uint)(App.ScreenWidth / 3);

            SwipedCommand = new Command<SwipedCardEventArgs>(OnSwipedCommand);
            DraggingCommand = new Command<DraggingCardEventArgs>(OnDraggingCommand);

            ClearItemsCommand = new Command(OnClearItemsCommand);
            AddItemsCommand = new Command(OnAddItemsCommand);
        }

        public ObservableCollection<Profile> Profiles
        {
            get => _profiles;
            set
            {
                _profiles = value;
                RaisePropertyChanged();
            }
        }

        public uint Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SwipedCommand { get; }

        public ICommand DraggingCommand { get; }

        public ICommand ClearItemsCommand { get; }

        public ICommand AddItemsCommand { get; }

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
            Profiles.Clear();
        }

        private void OnAddItemsCommand()
        {
        }

        private void InitializeProfiles()
        {
            this.Profiles.Add(new Profile { ProfileId = 1, Name = "Laura", Age = 24, Photo = "p705193.jpg"});
            this.Profiles.Add(new Profile { ProfileId = 2, Name = "Sophia", Age = 21, Photo = "p597956.jpg"});
            this.Profiles.Add(new Profile { ProfileId = 3, Name = "Anne", Age = 19, Photo = "p497489.jpg"});
            this.Profiles.Add(new Profile { ProfileId = 4, Name = "Yvonne ", Age = 27, Photo = "p467499.jpg"});
            this.Profiles.Add(new Profile { ProfileId = 5, Name = "Abby ", Age = 25, Photo = "p589739.jpg"});
        }
    }
}

