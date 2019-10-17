using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SwipeCardView.Sample.ViewModel
{
    public class MainPageViewModel : BasePageViewModel
    {
        public MainPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            NavigateCommand = new Command<Type>(OnNavigateCommand);
        }

        public ICommand NavigateCommand { get; private set; }
        private INavigation Navigation { get; set; }

        private async void OnNavigateCommand(Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            await Navigation.PushAsync(page);
        }
    }
}