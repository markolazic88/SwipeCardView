# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/).

## 2.0.0

- Introduced SwipeCommand and Swiped event, SwipedEventArgs and SwipedCardDirection
- Introduced DraggingCommand, Dragging event, DraggingCardEventArgs and DraggingCardPosition
- Introduced InvokeSwipeCommand that replaces InvokeSwipeLeft and InvokeSwipeRight
- Support for swiping and dragging to all 4 directions
- Removed updating card background on in favor of Dragging command/event
- Updated Samples (TinderPage, ColorsPage)
- Added documentation
- Implemented CI pipeline
- Exposed many consts to be parameters (card rotation adjuster, back card scale, animation length etc.)

## 1.2.0

- Updated library to target netstandard 2.0
- Updated Xamarin Forms dependency to 3.0.0.561731
- Added InvokeSwipeLeft and InvokeSwipeRight methods

## 1.1.0

- Updated Xamarin Forms dependency to 2.5.0.91635.

## 1.0.0

- Initial release