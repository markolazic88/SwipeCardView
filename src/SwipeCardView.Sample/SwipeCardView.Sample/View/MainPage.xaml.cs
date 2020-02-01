using SwipeCardView.Sample.ViewModel;
using Xamarin.Forms;

namespace SwipeCardView.Sample.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(Navigation);
        }
    }
}