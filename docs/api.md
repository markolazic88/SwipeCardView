# SwipeCardView API

SwipeCardView inherits from `Xamarin.Forms.ContentView`.

## Properties

Below are the properties for the SwipeCardView:

Property | Type | Default | Description
--- | --- | --- | ---
AnimationLength | `uint` | 250 | The duration in milliseconds of the animation that occurs at the end of dragging movement
BackCardScale | `float` | 0.8f | Scaling of back card while the front is being dragged
CardRotation | `float` | 20 | Rotation adjuster in degrees for dragging to left or right. From 0 (no rotation) to 360 (full circle)
DraggingCommand | `System.Windows.Input.ICommand`  | null | Gets or sets the command to run when a dragging gesture is recognized
DraggingCommandParameter | `System.Object` | null | Gets or sets the parameter to pass to commands that take one
ItemsSource | `System.Collections.IList` | null | Gets or sets the source of items to template and display
ItemTemplate | `Xamarin.Forms.DataTemplate` | null | Gets or sets the DataTemplate to apply to the ItemsSource
SupportedDraggingDirections | `SwipeCardDirection` | Left, Right, Up, Down | Gets or sets supported dragging direction of the top card. User may want to support dragging in all 4 directions but to support swipe movement only in a subset. For example, Tinder support dragging in all 4 directions, but has no swipe down.
SupportedSwipeDirections | `SwipeCardDirection` | Left, Right, Up, Down | Gets or sets direction in which top card could be swiped. User may want to support dragging in all 4 directions but to support swipe movement only in a subset. For example, Tinder support dragging in all 4 directions, but has no swipe down.
SwipeCommand | `System.Windows.Input.ICommand`  | null | Gets or sets the command to run when a swipe gesture is recognized
SwipeCommandParameter | `System.Object` | null | Gets or sets the parameter to pass to commands that take one
Threshold | `uint` | 100 | Gets or sets the minimum card dragging distance that will cause the swipe gesture to be recognized
TopItem | `System.Object` | null | Gets the top items

## Events

Event | Arguments type | Description
--- | --- | ---
Swiped | `SwipedCardEventArgs` | It is raised on the end of the movement (just like SwipeGestureRecognizer)
Dragging | `DraggingCardEventArgs` | It is raised during the dragging movement

### EventArgs

#### SwipedCardEventArgs

Property | Type | Description
-- | -- | --
Item | `System.Object` | Gets the item parameter
Parameter |  `System.Object` | Gets the command parameter
Direction | `SwipeCardDirection` | Gets the direction of the swipe

#### DraggingCardEventArgs

Property | Type | Description
-- | -- | --
Item | `System.Object` | Gets the item parameter
Parameter |  `System.Object` | Gets the command parameter
Direction | `SwipeCardDirection` | Gets the direction of the swipe
Position | `DraggingCardPosition` | Gets the dragging position
DistanceDraggedX | `double` | Gets the distance dragged on X axis
DistanceDraggedY | `double` | Gets the distance dragged on Y axis

## Methods

### InvokeSwipe(SwipeCardDirection, UInt32, UInt32, TimeSpan)

Simulates PanGesture movement to the provided direction

Declaration

`public Task InvokeSwipe(SwipeCardDirection swipeCardDirection, uint numberOfTouches, uint touchDifferenceX, TimeSpan touchDelay, TimeSpan endTouch)`

Parameters
Type | Name | Description
--- | --- | ---
SwipeCardDirection | swipeCardDirection | Direction of the movement. Currently supported Left and Right.
System.UInt32 | numberOfTouches | Number of touch events. It should be a positive number (i.e. 20)
System.UInt32 |touchDifferenceX | Distance passed between two touches. It should be a positive number (i.e. 10)
System.TimeSpan | touchDelay | Delay between two touches. It should be a positive number (i.e. 1 millisecond)
System.TimeSpan | endDelay | End delay. It should be a positive number (i.e. 200 milliseconds)

## Enums

### SwipeCardDirection

Enumerates swipe directions.

This enumeration has a [FlagsAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute) attribute that allows a bitwise combination of its member values.

Name | Value | Description
--- | --- | ---
None | 0 | Indicates an unknown direction
Right | 1 | Indicates a rightward swipe
Left | 2 | Indicates a leftward swipe
Up | 4 | Indicates an upward swipe
Down | 8 | Indicates a downward swipe

### DraggingCardPosition

Enumerates dragging directions.

This enumeration has a [FlagsAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute) attribute that allows a bitwise combination of its member values.

Name | Value | Description
--- | --- | ---
Start | 0 | Indicates a starting position
UnderThreshold | 1 | Indicates a position under threshold
OverThreshold | 2 | Indicates a position over threshold
FinishedUnderThreshold | 4 | Indicates an ending position under threshold
FinishedOverThreshold | 8 | Indicates an ending position over threshold