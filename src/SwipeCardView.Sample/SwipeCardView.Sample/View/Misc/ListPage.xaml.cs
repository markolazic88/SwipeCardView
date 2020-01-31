using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SwipeCardView.Sample.View.Misc
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();

            SwipeCardView.ItemsSource = new List<string>()
            {
                "Baboon",
                "Capuchin Monkey",
                "Blue Monkey"
            };
        }
    }
}