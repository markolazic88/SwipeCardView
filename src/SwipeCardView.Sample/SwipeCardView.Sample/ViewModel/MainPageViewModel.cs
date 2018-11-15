using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SwipeCardView.Sample.ViewModel
{
    public class MainPageViewModel : BasePageViewModel
    {
        public ICommand NavigateCommand { get; private set; }

        public MainPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            
            NavigateCommand = new Command<Type>(OnNavigateCommand);
        }

        private INavigation Navigation { get; set; }

        private async void OnNavigateCommand(Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            await this.Navigation.PushAsync(page);
        }
    }
}
