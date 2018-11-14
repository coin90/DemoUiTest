using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace DemoUITest.UITests.Tests
{
    public class ItemsTest : BaseTest
    {
        public ItemsTest(Platform platform) : base(platform)
        {
        }

        [Test]
        public void GetItems()
        {
            var itemsPage = new ItemsPage();

            itemsPage.CheckLoadView();
            app.Screenshot("Items page loaded");
        }
    }
}
