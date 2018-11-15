using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using MLToolkit.Forms.SwipeCardView.Core;

namespace MLToolkit.Forms.SwipeCardView
{
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

        public static readonly BindableProperty TopItemProperty =
            BindableProperty.Create(
                nameof(TopItem),
                typeof(object),
                typeof(SwipeCardView),
                null,
                BindingMode.OneWayToSource);

        public static readonly BindableProperty SwipedCommandProperty =
            BindableProperty.Create(
                nameof(SwipedCommand),
                typeof(ICommand),
                typeof(SwipeCardView));

        public static readonly BindableProperty SwipedCommandParameterProperty =
            BindableProperty.Create(
                nameof(SwipedCommandParameter),
                typeof(object),
                    typeof(SwipeCardView));

        public static readonly BindableProperty DraggingCommandProperty =
            BindableProperty.Create(
                nameof(DraggingCommand),
                typeof(ICommand),
                typeof(SwipeCardView));

        public static readonly BindableProperty DraggingCommandParameterProperty =
            BindableProperty.Create(
                nameof(DraggingCommandParameter),
                typeof(object),
                typeof(SwipeCardView));

        public static readonly BindableProperty ThresholdProperty =
            BindableProperty.Create(
                nameof(Threshold),
                typeof(uint),
                typeof(SwipeCardView),
                DefaultThreshold);

        public static readonly BindableProperty SupportedSwipeDirectionsProperty =
            BindableProperty.Create(
                nameof(SupportedSwipeDirections),
                typeof(SwipeCardDirection),
                typeof(SwipeCardView),
                DefaultSupportedSwipeDirections);

        public static readonly BindableProperty SupportedDraggingDirectionsProperty =
            BindableProperty.Create(
                nameof(SupportedDraggingDirections),
                typeof(SwipeCardDirection),
                typeof(SwipeCardView),
                DefaultSupportedDraggingDirections);

        public static readonly BindableProperty BackCardScaleProperty =
            BindableProperty.Create(
                nameof(BackCardScale),
                typeof(float),
                typeof(SwipeCardView),
                DefaultBackCardScale);

        public static readonly BindableProperty CardRotationProperty =
            BindableProperty.Create(
                nameof(CardRotation),
                typeof(float),
                typeof(SwipeCardView),
                DefaultCardRotation);

        public static readonly BindableProperty AnimationLengthProperty =
            BindableProperty.Create(
                nameof(AnimationLength),
                typeof(uint),
                typeof(SwipeCardView),
                DefaultAnimationLength);

        private const uint DefaultThreshold = 100;

        private const SwipeCardDirection DefaultSupportedSwipeDirections = SwipeCardDirection.Left | SwipeCardDirection.Right | SwipeCardDirection.Up | SwipeCardDirection.Down;
        
        private const SwipeCardDirection DefaultSupportedDraggingDirections = SwipeCardDirection.Left | SwipeCardDirection.Right | SwipeCardDirection.Up | SwipeCardDirection.Down;

        private const float DefaultBackCardScale = 0.8f;
        
        private const float DefaultCardRotation = 20;
        
        private const uint DefaultAnimationLength = 250; // Speed of the animations
        
        private const int NumCards = 2; // Number of cards in stack

        private const uint InvokeSwipeDefaultNumberOfTouches = 20;
        private const uint InvokeSwipeDefaultTouchDifference = 10;
        private const uint InvokeSwipeDefaultTouchDelay = 1;
        private const uint InvokeSwipeDefaultEndDelay = 200;


        private readonly View[] _cards = new View[NumCards];

        private int _topCardIndex;  // The card at the top of the stack

        private float _cardDistanceX = 0;   // Distance the card has been moved on X axis

        private float _cardDistanceY = 0;   // Distance the card has been moved on Y axis

        private int _itemIndex = 0; // The last items index added to the stack of the cards

        private bool _ignoreTouch = false;

        public SwipeCardView()
        {
            var view = new RelativeLayout();

            this.Content = view;

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += this.OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);

            this.DraggingCardPosition = DraggingCardPosition.Start;
        }

        public IList ItemsSource
        {
            get => (IList)this.GetValue(ItemsSourceProperty);
            set
            {
                this.SetValue(ItemsSourceProperty, value);
                _itemIndex = 0;
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

        public ICommand SwipedCommand
        {
            get => (ICommand)this.GetValue(SwipedCommandProperty);
            set => this.SetValue(SwipedCommandProperty, value);
        }

        public object SwipedCommandParameter
        {
            get => GetValue(SwipedCommandParameterProperty);
            set => SetValue(SwipedCommandParameterProperty, value);
        }

        public ICommand DraggingCommand
        {
            get => (ICommand)this.GetValue(DraggingCommandProperty);
            set => this.SetValue(DraggingCommandProperty, value);
        }

        public object DraggingCommandParameter
        {
            get => GetValue(DraggingCommandParameterProperty);
            set => SetValue(DraggingCommandParameterProperty, value);
        }

        public uint Threshold
        {
            get => (uint)this.GetValue(ThresholdProperty);
            set => this.SetValue(ThresholdProperty, value);
        }

        public SwipeCardDirection SupportedSwipeDirections
        {
            get => (SwipeCardDirection)this.GetValue(SupportedSwipeDirectionsProperty);
            set => this.SetValue(SupportedSwipeDirectionsProperty, value);
        }

        public SwipeCardDirection SupportedDraggingDirections
        {
            get => (SwipeCardDirection)this.GetValue(SupportedDraggingDirectionsProperty);
            set => this.SetValue(SupportedDraggingDirectionsProperty, value);
        }

        public float BackCardScale
        {
            get => (float)this.GetValue(BackCardScaleProperty);
            set => this.SetValue(BackCardScaleProperty, value);
        }

        public float CardRotation
        {
            get => (float)this.GetValue(CardRotationProperty);
            set => this.SetValue(CardRotationProperty, value);
        }

        public uint AnimationLength
        {
            get => (uint)this.GetValue(AnimationLengthProperty);
            set => this.SetValue(AnimationLengthProperty, value);
        }

        public event EventHandler<SwipedCardEventArgs> Swiped;

        public event EventHandler<DraggingCardEventArgs> Dragging;

        private DraggingCardPosition DraggingCardPosition { get; set; }

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

                swipeCardView._cards[i] = card;
                card.IsVisible = false;

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
                _itemIndex = 0;
                foreach (var card in _cards)
                {
                    card.IsVisible = false;
                }

                return;
            }

            if (_cards[0].IsVisible == false && _cards[1].IsVisible == false)
            {
                this.Setup();
            }
        }

        private void Setup()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            // Set the top card
            _topCardIndex = 0;

            // Create a stack of cards
            for (var i = 0; i < Math.Min(NumCards, this.ItemsSource.Count); i++)
            {
                if (_itemIndex >= this.ItemsSource.Count)
                {
                    break;
                }

                var card = _cards[i];
                card.BindingContext = this.ItemsSource[_itemIndex];

                if (i == 0)
                {
                    this.TopItem = this.ItemsSource[_itemIndex];
                }

                card.IsVisible = true;
                card.Scale = this.GetScale(i);
                card.RotateTo(0, 0);
                card.TranslateTo(0, -card.Y, 0);
                ((RelativeLayout)this.Content).LowerChild(card);
                _itemIndex++;
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
                    this.HandleTouch((float)e.TotalX, (float)e.TotalY);
                    break;
                case GestureStatus.Completed:
                    this.HandleTouchEnd();
                    break;
            }
        }

        // Handle when a touch event begins
        private void HandleTouchStart()
        {
            _cardDistanceX = 0;
            _cardDistanceY = 0;
            var topCard = _cards[_topCardIndex];
            this.SendDragging(topCard, SwipeCardDirection.None, DraggingCardPosition.Start, 0, 0);
        }

        // Handle the ongoing touch event as the card is moved
        private void HandleTouch(float differenceX, float differenceY)
        {
            if (_ignoreTouch)
            {
                return;
            }

            var topCard = _cards[_topCardIndex];
            var backCard = _cards[this.PrevCardIndex(_topCardIndex)];

            // Move the top card
            if (topCard.IsVisible)
            {
                // Move the card
                if (differenceX > 0 && SupportedDraggingDirections.IsRight() || differenceX < 0 && SupportedDraggingDirections.IsLeft())
                {
                    topCard.TranslationX = differenceX;

                    // Calculate a angle for the card
                    var rotationAngle = (float)(CardRotation * Math.Min(differenceX / this.Width, 1.0f));
                    topCard.Rotation = rotationAngle;
                }

                if (differenceY > 0 && SupportedDraggingDirections.IsDown() || differenceY < 0 && SupportedDraggingDirections.IsUp())
                {
                    topCard.TranslationY = differenceY;
                }

                // Keep a record of how far its moved
                _cardDistanceX = differenceX;
                _cardDistanceY = differenceY;

                SwipeCardDirection direction;
                DraggingCardPosition position;
                if (Math.Abs(differenceX) > Math.Abs(differenceY))
                {
                    direction = differenceX > 0 ? SwipeCardDirection.Right : SwipeCardDirection.Left;
                    position = Math.Abs(differenceX) > this.Threshold ? DraggingCardPosition.OverThreshold : DraggingCardPosition.UnderThreshold;
                }
                else
                {
                    direction = differenceY > 0 ? SwipeCardDirection.Down : SwipeCardDirection.Up;
                    position = Math.Abs(differenceY) > this.Threshold ? DraggingCardPosition.OverThreshold : DraggingCardPosition.UnderThreshold;
                }

                if (SupportedDraggingDirections.IsSupported(direction))
                {
                    this.SendDragging(topCard, direction, position, differenceX, differenceY);
                }
            }

            // Scale the back card
            if (backCard.IsVisible)
            {
                var cardDistance = Math.Abs(differenceX) > Math.Abs(differenceY) ? differenceX : differenceY;
                backCard.Scale = Math.Min(BackCardScale + Math.Abs((cardDistance / this.Threshold) * (1.0f - BackCardScale)), 1.0f);
            }
        }

        // Handle the end of the touch event
        private async void HandleTouchEnd()
        {
            _ignoreTouch = true;

            var topCard = _cards[_topCardIndex];

            SwipeCardDirection direction;
            DraggingCardPosition position;
            if (Math.Abs(_cardDistanceX) > Math.Abs(_cardDistanceY))
            {
                direction = _cardDistanceX > 0 ? SwipeCardDirection.Right : SwipeCardDirection.Left;
                position = Math.Abs(_cardDistanceX) > this.Threshold ? DraggingCardPosition.FinishedOverThreshold : DraggingCardPosition.FinishedUnderThreshold;
            }
            else
            {
                direction = _cardDistanceY > 0 ? SwipeCardDirection.Down : SwipeCardDirection.Up;
                position = Math.Abs(_cardDistanceY) > this.Threshold ? DraggingCardPosition.FinishedOverThreshold : DraggingCardPosition.FinishedUnderThreshold;
            }

            if (SupportedDraggingDirections.IsSupported(direction))
            {
                this.SendDragging(topCard, direction, position, _cardDistanceX, _cardDistanceY);
            }

            if (position == DraggingCardPosition.FinishedOverThreshold && this.SupportedSwipeDirections.IsSupported(direction))
            {
                // Move the top card off the screen
                if (direction.IsLeft() || direction.IsRight())
                {
                    await topCard.TranslateTo(_cardDistanceX > 0 ? this.Width : -this.Width, 0, AnimationLength / 2, Easing.SpringOut);
                }
                else
                {
                    await topCard.TranslateTo(0, _cardDistanceY > 0 ? this.Height : -this.Height, AnimationLength / 2, Easing.SpringOut);
                }

                topCard.IsVisible = false;

                this.SendSwiped(topCard, direction);

                this.ShowNextCard();
            }
            else
            {
                // Move the top card back to the center
                // Not awaiting on purpose to allow TranslateTo, RotateTo and ScaleTo to happen simultaneously 
                topCard.TranslateTo((-topCard.X), -topCard.Y, AnimationLength, Easing.SpringOut);
                topCard.RotateTo(0, AnimationLength, Easing.SpringOut);

                // Scale the back card down
                var prevCard = _cards[this.PrevCardIndex(_topCardIndex)];
                await prevCard.ScaleTo(BackCardScale, AnimationLength, Easing.SpringOut);
            }

            _ignoreTouch = false;
        }

        private void ShowNextCard()
        {
            if (_cards[0].IsVisible == false && _cards[1].IsVisible == false)
            {
                this.Setup();
                return;
            }

            this.TopItem = this.ItemsSource[_itemIndex - 1];

            var topCard = _cards[_topCardIndex];
            _topCardIndex = this.NextCardIndex(_topCardIndex);

            // If there are more cards to show, show the next card in to place of 
            // the card that was swiped off the screen
            if (_itemIndex < this.ItemsSource.Count)
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

                topCard.BindingContext = this.ItemsSource[_itemIndex];

                topCard.IsVisible = true;
                _itemIndex++;
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
            return index == _topCardIndex ? 1.0f : BackCardScale;
        }

        private void SendSwiped(View sender, SwipeCardDirection direction)
        {
            var cmd = this.SwipedCommand;
            if (cmd != null && cmd.CanExecute(SwipedCommandParameter))
            {
                cmd.Execute(new SwipedCardEventArgs(sender.BindingContext, SwipedCommandParameter, direction));
            }

            Swiped?.Invoke(sender, new SwipedCardEventArgs(sender.BindingContext, SwipedCommandParameter, direction));
        }

        private void SendDragging(View sender, SwipeCardDirection direction, DraggingCardPosition position, double distanceDraggedX, double distanceDraggedY)
        {
            var cmd = this.DraggingCommand;
            if (cmd != null && cmd.CanExecute(SwipedCommandParameter))
            {
                cmd.Execute(new DraggingCardEventArgs(sender.BindingContext, DraggingCommandParameter, direction, position, distanceDraggedX, distanceDraggedY));
            }

            Dragging?.Invoke(sender, new DraggingCardEventArgs(sender.BindingContext, DraggingCommandParameter, direction, position, distanceDraggedX, distanceDraggedY));
        }

        /// <summary>
        /// Simulates PanGesture movement to left or right
        /// </summary>
        /// <param name="swipeCardDirection">Direction of the movement. Currently supported Left and Right.</param>
        public async Task InvokeSwipe(SwipeCardDirection swipeCardDirection)
        {
            await this.InvokeSwipe(swipeCardDirection, InvokeSwipeDefaultNumberOfTouches,
                InvokeSwipeDefaultTouchDifference, TimeSpan.FromMilliseconds(InvokeSwipeDefaultTouchDelay),
                TimeSpan.FromMilliseconds(InvokeSwipeDefaultEndDelay));
        }

        /// <summary>
        /// Simulates PanGesture movement to left or right
        /// </summary>
        /// <param name="swipeCardDirection">Direction of the movement. Currently supported Left and Right.</param>
        /// <param name="numberOfTouches">Number of touch events. It should be a positive number (i.e. 20)</param>
        /// <param name="touchDifference">Distance passed between two touches. It should be a positive number (i.e. 10)</param>
        /// <param name="touchDelay">Delay between two touches. It should be a positive number (i.e. 1 millisecond)</param>
        /// <param name="endDelay">End delay. It should be a positive number (i.e. 200 milliseconds)</param>
        public async Task InvokeSwipe(SwipeCardDirection swipeCardDirection, uint numberOfTouches, uint touchDifference, TimeSpan touchDelay, TimeSpan endDelay)
        {
            if (numberOfTouches == 0 || touchDifference == 0)
            {
                return;
            }

            var differenceX = 0;
            var differenceY = 0;

            switch (swipeCardDirection)
            {
                case SwipeCardDirection.Right:
                    differenceX = (int)touchDifference;
                    break;
                case SwipeCardDirection.Left:
                    differenceX = (int) (-1 * touchDifference);
                    break;
                case SwipeCardDirection.Up:
                    differenceY = (int) (-1 * touchDifference);
                    break;
                case SwipeCardDirection.Down:
                    differenceY = (int) touchDifference;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeCardDirection), swipeCardDirection, null);
            }

            this.HandleTouchStart();

            for (var i = 1; i < numberOfTouches; i++)
            {
                this.HandleTouch(differenceX * i, differenceY * i);
                await Task.Delay(touchDelay);
            }

            await Task.Delay(endDelay);

            this.HandleTouchEnd();
        }
    }

    internal static class SwipeCardDirectionExtensions
    {
        public static bool IsLeft(this SwipeCardDirection self)
        {
            return (self & SwipeCardDirection.Left) == SwipeCardDirection.Left;
        }
        public static bool IsRight(this SwipeCardDirection self)
        {
            return (self & SwipeCardDirection.Right) == SwipeCardDirection.Right;
        }
        public static bool IsUp(this SwipeCardDirection self)
        {
            return (self & SwipeCardDirection.Up) == SwipeCardDirection.Up;
        }
        public static bool IsDown(this SwipeCardDirection self)
        {
            return (self & SwipeCardDirection.Down) == SwipeCardDirection.Down;
        }

        public static bool IsSupported(this SwipeCardDirection self, SwipeCardDirection other)
        {
            return (self & other) == other;
        }
    }
}
