
namespace MLToolkit.Forms.SwipeCardView.Core
{
    /// <summary>Arguments for swipe events.</summary>
    public class SwipedCardEventArgs : System.EventArgs
    {
        public SwipedCardEventArgs(object item, object parameter, SwipeCardDirection direction)
        {
            this.Item = item;
            this.Parameter = parameter;
            this.Direction = direction;
        }

        /// <summary>Gets the item parameter.</summary>
        public object Item { get; private set; }

        /// <summary>Gets the command parameter.</summary>
        public object Parameter { get; private set; }

        /// <summary>Gets the direction of the swipe.</summary>
        public SwipeCardDirection Direction { get; private set; }
    }
}
