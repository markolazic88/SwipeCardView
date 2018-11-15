using System;

namespace MLToolkit.Forms.SwipeCardView.Core
{
    /// <summary>Enumerates dragging directions.</summary>
    [Flags]
    public enum DraggingCardPosition
    {
        /// <summary>Indicates a starting position.</summary>
        Start = 0,

        /// <summary>Indicates a position under threshold.</summary>
        UnderThreshold = 1,

        /// <summary>Indicates a position over threshold.</summary>
        OverThreshold = 2,

        /// <summary>Indicates an ending position under threshold.</summary>
        FinishedUnderThreshold = 4,

        /// <summary>Indicates an ending position over threshold.</summary>
        FinishedOverThreshold = 8
    }
}
