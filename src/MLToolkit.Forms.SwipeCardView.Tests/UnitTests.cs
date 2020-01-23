using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MLToolkit.Forms.SwipeCardView.Core;

namespace MLToolkit.Forms.SwipeCardView.Tests
{
    [TestClass]
    public class UnitTests
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
            var swipeCardDirection = SwipeCardDirection.None;
            
            swipeCardView.ItemsSource = cardItems;
            swipeCardView.Swiped += (sender, args) => { swipeCardDirection = args.Direction; };

            await swipeCardView.InvokeSwipe(SwipeCardDirection.Right);

            Assert.AreEqual(swipeCardDirection, SwipeCardDirection.Right);
            Assert.AreEqual(swipeCardView.ItemsSource.Count, 1);
        }
    }
}
