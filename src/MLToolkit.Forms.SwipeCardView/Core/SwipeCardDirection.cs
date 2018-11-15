using System;

namespace MLToolkit.Forms.SwipeCardView.Core
{
    /// <summary>Enumerates swipe directions.</summary>
    [Flags]
    public enum SwipeCardDirection
    {
        /// <summary>Indicates an unknown direction.</summary>
        None = 0,

        /// <summary>Indicates a rightward swipe.</summary>
        Right = 1,

        /// <summary>Indicates a leftward swipe.</summary>
        Left = 2,

        /// <summary>Indicates an upward swipe.</summary>
        Up = 4,

        /// <summary>Indicates a downward swipe.</summary>
        Down = 8,
    }
}
