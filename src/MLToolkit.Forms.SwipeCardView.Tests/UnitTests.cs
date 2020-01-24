using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;

namespace MLToolkit.Forms.SwipeCardView.Tests
{
    [TestClass]
    public class UnitTests : TestBase
    {
        [TestMethod]
        public async Task Swipe_EmptyObservableCollection_ShouldNotInvoke()
        {
            var cardItems = new ObservableCollection<string>();
            var swipeCardView = new SwipeCardView();
            var swipeCardDirection = SwipeCardDirection.None;

            swipeCardView.ItemsSource = cardItems;
            swipeCardView.Swiped += (sender, args) => { swipeCardDirection = args.Direction; };

            await swipeCardView.InvokeSwipe(SwipeCardDirection.Right);

            Assert.AreEqual(swipeCardDirection, SwipeCardDirection.None);
            Assert.AreEqual(swipeCardView.ItemsSource.Count, 0);
        }

        [TestMethod]
        public async Task Swipe_ObservableCollection_UpdatesTopItem()
        {
            var swipeCardView = new SwipeCardView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    var stackLayout = new StackLayout();
                    var label = new Label();
                    label.SetBinding(Label.TextProperty, ".");
                    stackLayout.Children.Add(label);

                    return stackLayout;
                })
            };

            swipeCardView.ItemsSource = new ObservableCollection<string>() { "Item1", "Item2" };

            var swipeCardDirection = SwipeCardDirection.None;
            swipeCardView.Swiped += (sender, args) => { swipeCardDirection = args.Direction; };
            var initialTopItem = swipeCardView.TopItem;

            await swipeCardView.InvokeSwipe(SwipeCardDirection.Right);

            var afterSwipeTopItem = swipeCardView.TopItem;

            Assert.AreEqual(swipeCardDirection, SwipeCardDirection.Right);
            Assert.AreEqual(swipeCardView.ItemsSource.Count, 2);
            Assert.AreNotEqual(initialTopItem, afterSwipeTopItem);
            Assert.AreEqual(initialTopItem, "Item1");
            Assert.AreEqual(afterSwipeTopItem, "Item2");
        }

        [TestMethod]
        public async Task Swipe_SetObservableCollectionTwice()
        {
            var swipeCardView = new SwipeCardView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    var stackLayout = new StackLayout();
                    var label = new Label();
                    label.SetBinding(Label.TextProperty, ".");
                    stackLayout.Children.Add(label);

                    return stackLayout;
                })
            };

            swipeCardView.ItemsSource = new ObservableCollection<string>() { "Item1", "Item2" };
            swipeCardView.ItemsSource = new ObservableCollection<string>() { "Item3", "Item4" };

            var swipeCardDirection = SwipeCardDirection.None;
            swipeCardView.Swiped += (sender, args) => { swipeCardDirection = args.Direction; };
            var initialTopItem = swipeCardView.TopItem;

            await swipeCardView.InvokeSwipe(SwipeCardDirection.Right);

            var afterSwipeTopItem = swipeCardView.TopItem;

            Assert.AreEqual(swipeCardDirection, SwipeCardDirection.Right);
            Assert.AreEqual(swipeCardView.ItemsSource.Count, 2);
            Assert.AreNotEqual(initialTopItem, afterSwipeTopItem);
            Assert.AreEqual(initialTopItem, "Item3");
            Assert.AreEqual(afterSwipeTopItem, "Item4");
        }
    }
}
