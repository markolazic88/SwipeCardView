using SwipeCardView.Sample.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SwipeCardView.Sample.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomizablePage : ContentPage
	{
		public CustomizablePage ()
		{
			InitializeComponent ();
		    this.BindingContext = new CustomizablePageViewModel();
		}
	}
}