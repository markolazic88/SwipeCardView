namespace MLToolkit.Forms.SwipeCardView
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Windows.Input;

    using Xamarin.Forms;

    public class SwipeCardView : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(SwipeCardView),
                null,
                propertyChanged: OnItemsSourcePropertyChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(SwipeCardView),
                propertyChanged: OnItemTemplatePropertyChanged);

        public static readonly BindableProperty SwipedLeftCommandProperty =
            BindableProperty.Create(
                nameof(SwipedLeftCommand),
                typeof(ICommand),
                typeof(SwipeCardView));

        public static readonly BindableProperty SwipedRightCommandProperty =
            BindableProperty.Create(
                nameof(SwipedRightCommand),
                typeof(ICommand),
                typeof(SwipeCardView));

        public static readonly BindableProperty TopItemProperty =
            BindableProperty.Create(
                nameof(TopItem),
                typeof(object),
                typeof(SwipeCardView),
                null,
                BindingMode.OneWayToSource);

        private const float BackCardScale = 0.8f; // Back card scale

        private const int AnimationLength = 250; // Speed of the animations

        private const float DegreesToRadians = 57.2957795f; // 180 / pi

        private const float CardRotationAdjuster = 0.3f; // Higher the number less the rotation effect

        private const int NumCards = 2; // Number of cards in stack

        private const string DefaultBackgroundColor = "#F5F8FA";

        private const string DefaultCardBackgroundColor = "#FFFFFF";

        private const string SwipeLeftBackgroundColor = "#EF5350";

        private const string SwipeRightBackgroundColor = "#66BB6A";

        private readonly View[] cards = new View[NumCards];

        private int topCardIndex;  // The card at the top of the stack

        private float cardDistance = 0;   // Distance the card has been moved

        private int itemIndex = 0; // The last items index added to the stack of the cards

        private bool ignoreTouch = false;

        public SwipeCardView()
        {
            var view = new RelativeLayout();

            this.BackgroundColor = Color.FromHex(DefaultBackgroundColor);
            this.Content = view;

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += this.OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);
        }

        public int CardMoveDistance { get; set; } // Distance a card must be moved to consider to be swiped off

        public IList ItemsSource
        {
            get => (IList)this.GetValue(ItemsSourceProperty);
            set
            {
                this.SetValue(ItemsSourceProperty, value);
                this.itemIndex = 0;
            }
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)this.GetValue(ItemTemplateProperty);
            set => this.SetValue(ItemTemplateProperty, value);
        }

        public object TopItem
        {
            get => (object)this.GetValue(TopItemProperty);
            set => this.SetValue(TopItemProperty, value);
        }

        public ICommand SwipedLeftCommand
        {
            get => (ICommand)this.GetValue(SwipedLeftCommandProperty);
            set => this.SetValue(SwipedLeftCommandProperty, value);
        }

        public ICommand SwipedRightCommand
        {
            get => (ICommand)this.GetValue(SwipedRightCommandProperty);
            set => this.SetValue(SwipedRightCommandProperty, value);
        }

        /// <summary>
        /// Simpulates PanGesture movement on left
        /// </summary>
        /// <param name="numberOfTouches">Number of touch events. It should be positive number (i.e. 50)</param>
        /// <param name="differenceX">Distance betweeen two touches. It should be positive number (i.e. 50)</param>
        public void InvokeSwipeLeft(int numberOfTouches, int differenceX)
        {
            if (numberOfTouches <= 0 || differenceX <= 0)
            {
                return;
            }

            this.HandleTouchStart();

            for (var i = 1; i < numberOfTouches; i++)
            {
                HandleTouch(-differenceX * i);
            }

            this.HandleTouchEnd();
        }

        /// <summary>
        /// Simpulates PanGesture movement on right
        /// </summary>
        /// <param name="numberOfTouches">Number of touch events. It should be positive number (i.e. 50)</param>
        /// <param name="differenceX">Distance betweeen two touches. It should be positive number (i.e. 50)</param>
        public void InvokeSwipeRight(int numberOfTouches, int differenceX)
        {
            if (numberOfTouches <= 0 || differenceX <= 0)
            {
                return;
            }

            this.HandleTouchStart();

            for (var i = 1; i < numberOfTouches; i++)
            {
                HandleTouch(differenceX * i);
            }

            this.HandleTouchEnd();
        }

        private static void OnItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var swipeCardView = (SwipeCardView)bindable;

            if (swipeCardView.ItemTemplate == null)
            {
                return;
            }

            swipeCardView.Content = null;

            var view = new RelativeLayout();

            // create a stack of cards
            for (var i = 0; i < NumCards; i++)
            {
                var content = swipeCardView.ItemTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new Exception($"Invalid visual object {nameof(content)}");
                }

                var card = content is View ? content as View : ((ViewCell)content).View;

                swipeCardView.cards[i] = card;
                card.InputTransparent = true;
                card.IsVisible = false;
                card.BackgroundColor = Color.FromHex(DefaultCardBackgroundColor);

                view.Children.Add(
                    card,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent(parent => parent.Width),
                    Constraint.RelativeToParent(parent => parent.Height));
            }

            swipeCardView.Content = view;
        }

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var swipeCardView = (SwipeCardView)bindable;
            var observable = oldValue as INotifyCollectionChanged;
            if (observable != null)
            {
                observable.CollectionChanged -= swipeCardView.OnItemSourceCollectionChanged;
            }

            observable = newValue as INotifyCollectionChanged;
            swipeCardView.Setup();
            if (observable != null)
            {
                observable.CollectionChanged += swipeCardView.OnItemSourceCollectionChanged;
            }
        }

        private void OnItemSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                itemIndex = 0;
                foreach (var card in cards)
                {
                    card.IsVisible = false;
                }

                return;
            }

            if (this.cards[0].IsVisible == false && this.cards[1].IsVisible == false)
            {
                this.Setup();
            }
        }

        private void Setup()
        {
            // Set the top card
            this.topCardIndex = 0;

            // Create a stack of cards
            for (var i = 0; i < Math.Min(NumCards, this.ItemsSource.Count); i++)
            {
                if (this.itemIndex >= this.ItemsSource.Count)
                {
                    break;
                }

                var card = this.cards[i];
                card.BindingContext = this.ItemsSource[this.itemIndex];

                if (i == 0)
                {
                    this.TopItem = this.ItemsSource[this.itemIndex];
                }

                card.IsVisible = true;
                card.Scale = this.GetScale(i);
                card.RotateTo(0, 0);
                card.TranslateTo(0, -card.Y, 0);
                ((RelativeLayout)this.Content).LowerChild(card);
                this.itemIndex++;
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (this.ItemsSource.Count == 0)
            {
                return;
            }

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    this.HandleTouchStart();
                    break;
                case GestureStatus.Running:
                    this.HandleTouch((float)e.TotalX);
                    break;
                case GestureStatus.Completed:
                    this.HandleTouchEnd();
                    break;
            }
        }

        // Hande when a touch event begins
        private void HandleTouchStart()
        {
            this.cardDistance = 0;
        }

        // Handle the ongoing touch event as the card is moved
        private void HandleTouch(float differenceX)
        {
            if (this.ignoreTouch)
            {
                return;
            }

            var topCard = this.cards[this.topCardIndex];
            var backCard = this.cards[this.PrevCardIndex(this.topCardIndex)];

            // Move the top card
            if (topCard.IsVisible)
            {
                // Move the card
                topCard.TranslationX = differenceX;

                // Calculate a angle for the card
                var rotationAngle = (float)(CardRotationAdjuster * Math.Min(differenceX / this.Width, 1.0f));
                topCard.Rotation = rotationAngle * DegreesToRadians;

                // Keep a record of how far its moved
                this.cardDistance = differenceX;

                if (Math.Abs((int)this.cardDistance) > this.CardMoveDistance)
                {
                    if (this.cardDistance > this.CardMoveDistance)
                    {
                        topCard.BackgroundColor = Color.FromHex(SwipeRightBackgroundColor);
                    }
                    else
                    {
                        topCard.BackgroundColor = Color.FromHex(SwipeLeftBackgroundColor);
                    }
                }
                else
                {
                    topCard.BackgroundColor = Color.FromHex(DefaultCardBackgroundColor);
                }
            }

            // Scale the backcard
            if (backCard.IsVisible)
            {
                backCard.Scale = Math.Min(BackCardScale + Math.Abs((this.cardDistance / this.CardMoveDistance) * (1.0f - BackCardScale)), 1.0f);
            }
        }

        // Handle the end of the touch event
        private async void HandleTouchEnd()
        {
            this.ignoreTouch = true;

            var topCard = this.cards[this.topCardIndex];
            topCard.BackgroundColor = Color.FromHex(DefaultCardBackgroundColor);

            // If the card was move enough to be considered swiped off
            if (Math.Abs((int)this.cardDistance) > this.CardMoveDistance)
            {
                // move off the screen
                await topCard.TranslateTo(this.cardDistance > 0 ? this.Width : -this.Width, 0, AnimationLength / 2, Easing.SpringOut);
                topCard.IsVisible = false;

                if (this.SwipedRightCommand != null && this.cardDistance > 0)
                {
                    this.SwipedRightCommand.Execute(this.ItemsSource.IndexOf(topCard.BindingContext));
                }
                else if (this.SwipedLeftCommand != null)
                {
                    this.SwipedLeftCommand.Execute(this.ItemsSource.IndexOf(topCard.BindingContext));
                }

                this.ShowNextCard();
            }
            else
            {
                // Move the top card back to the center
                topCard.TranslateTo((-topCard.X), -topCard.Y, AnimationLength, Easing.SpringOut);
                topCard.RotateTo(0, AnimationLength, Easing.SpringOut);

                // Scale the back card down
                var prevCard = this.cards[this.PrevCardIndex(this.topCardIndex)];
                await prevCard.ScaleTo(BackCardScale, AnimationLength, Easing.SpringOut);
            }

            this.ignoreTouch = false;
        }

        private void ShowNextCard()
        {
            if (this.cards[0].IsVisible == false && this.cards[1].IsVisible == false)
            {
                this.Setup();
                return;
            }

            this.TopItem = this.ItemsSource[this.itemIndex - 1];

            var topCard = this.cards[this.topCardIndex];
            this.topCardIndex = this.NextCardIndex(this.topCardIndex);

            // If there are more cards to show, show the next card in to place of 
            // the card that was swipped off the screen
            if (this.itemIndex < this.ItemsSource.Count)
            {
                // Push it to the back z order
                ((RelativeLayout)this.Content).LowerChild(topCard);

                try
                {
                    // Reset its scale, opacity and rotation
                    topCard.Scale = BackCardScale;
                    topCard.RotateTo(0, 0);
                    topCard.TranslateTo(0, -topCard.Y, 0);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }

                topCard.BindingContext = this.ItemsSource[this.itemIndex];

                topCard.IsVisible = true;
                this.itemIndex++;
            }
        }

        // Return the next card index from the top
        private int NextCardIndex(int topIndex)
        {
            return topIndex == 0 ? 1 : 0;
        }

        // Return the prev card index from the top
        private int PrevCardIndex(int topIndex)
        {
            return topIndex == 0 ? 1 : 0;
        }

        // Helper to get the scale based on the card index position relative to the top card
        private float GetScale(int index)
        {
            return index == this.topCardIndex ? 1.0f : BackCardScale;
        }
    }
}
