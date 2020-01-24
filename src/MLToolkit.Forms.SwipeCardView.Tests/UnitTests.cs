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
        public async Task ShouldNotInvokeSwipeIfItemsSourceIsEmptyTest()
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
        public async Task ShouldInvokeSwipeIfItemsSourceHasItemsTest()
        {
            var cardItems = new ObservableCollection<string>() { "Item1", "Item2" };
            var swipeCardView = new SwipeCardView();
            swipeCardView.ItemTemplate = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout();
                var label = new Label();
                label.SetBinding(Label.TextProperty, ".");
                stackLayout.Children.Add(label);

                return stackLayout;
            });
            var swipeCardDirection = SwipeCardDirection.None;
            
            swipeCardView.ItemsSource = cardItems;
            swipeCardView.Swiped += (sender, args) => { swipeCardDirection = args.Direction; };
            var initialTopItem = swipeCardView.TopItem;

            await swipeCardView.InvokeSwipe(SwipeCardDirection.Right);

            var afterSwipeTopItem = swipeCardView.TopItem;

            Assert.AreEqual(swipeCardDirection, SwipeCardDirection.Right);
            Assert.AreEqual(swipeCardView.ItemsSource.Count, 2);
            Assert.AreNotEqual(initialTopItem, afterSwipeTopItem);
            Assert.AreEqual(initialTopItem, cardItems[0]);
            Assert.AreEqual(afterSwipeTopItem, cardItems[1]);
        }
    }
}
