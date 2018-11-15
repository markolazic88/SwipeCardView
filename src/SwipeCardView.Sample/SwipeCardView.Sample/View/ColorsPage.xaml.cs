using System;
using MLToolkit.Forms.SwipeCardView.Core;
using SwipeCardView.Sample.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SwipeCardView.Sample.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorsPage : ContentPage
	{
		public ColorsPage ()
		{
			InitializeComponent ();
		    this.BindingContext = new ColorsPageViewModel();

		    SwipeCardView.Dragging += OnDragging;
		}

	    private void OnDragging(object sender, DraggingCardEventArgs e)
	    {
	        var view = (Xamarin.Forms.View)sender;

            var directionLabel = view.FindByName<Label>("DirectionLabel");
	        directionLabel.Text = e.Direction.ToString();

	        var positionLabel = view.FindByName<Label>("PositionLabel");
	        positionLabel.Text = e.Position.ToString();

	        switch (e.Position)
	        {
	            case DraggingCardPosition.Start:
	                break;
	            case DraggingCardPosition.UnderThreshold:
	                view.BackgroundColor = Color.DarkTurquoise;
	                break;
	            case DraggingCardPosition.OverThreshold:
	                switch (e.Direction)
	                {
	                    case SwipeCardDirection.Left:
	                        view.BackgroundColor = Color.FromHex("#FF6A4F");
	                        break;
	                    case SwipeCardDirection.Right:
	                        view.BackgroundColor = Color.FromHex("#63DD99");
	                        break;
	                    case SwipeCardDirection.Up:
	                        view.BackgroundColor = Color.FromHex("#2196F3");
	                        break;
                        case SwipeCardDirection.Down:
                            view.BackgroundColor = Color.MediumPurple;
                            break;
	                }
	                break;
	            case DraggingCardPosition.FinishedUnderThreshold:
	                view.BackgroundColor = Color.Beige;
	                break;
	            case DraggingCardPosition.FinishedOverThreshold:
	                view.BackgroundColor = Color.Beige;
	                directionLabel.Text = string.Empty;
	                positionLabel.Text = string.Empty;
	                break;
	            default:
	                throw new ArgumentOutOfRangeException();
	        }
        }
    }
}