
namespace MLToolkit.Forms.SwipeCardView.Core
{
    /// <summary>Arguments for swipe events.</summary>
    public class DraggingCardEventArgs : System.EventArgs
    {
        public DraggingCardEventArgs(object item, object parameter, SwipeCardDirection direction, DraggingCardPosition position, double distanceDraggedX, double distanceDraggedY)
        {
            this.Item = item;
            this.Parameter = parameter;
            this.Direction = direction;
            this.Position = position;
            this.DistanceDraggedX = distanceDraggedX;
            this.DistanceDraggedY = distanceDraggedY;
        }

        /// <summary>Gets the item parameter.</summary>
        public object Item { get; private set; }

        /// <summary>Gets the command parameter.</summary>
        public object Parameter { get; private set; }

        /// <summary>Gets the direction of the swipe.</summary>
        public SwipeCardDirection Direction { get; private set; }

        /// <summary>Gets the dragging position.</summary>
        public DraggingCardPosition Position { get; private set; }

        /// <summary>Gets the distance dragged on X axis.</summary>
        public double DistanceDraggedX { get; private set; }

        /// <summary>Gets the distance dragged on Y axis.</summary>
        public double DistanceDraggedY { get; private set; }
    }
}
