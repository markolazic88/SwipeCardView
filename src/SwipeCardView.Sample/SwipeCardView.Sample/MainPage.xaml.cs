using Xamarin.Forms;

namespace SwipeCardView.Sample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		    this.BindingContext = new MainViewModel();
		}
	}
}
