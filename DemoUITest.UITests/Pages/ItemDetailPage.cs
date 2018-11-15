using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace DemoUITest.UITests.Pages
{
    public class ItemDetailPage : BasePage
    {
        readonly string ItemDetailPageId = "ItemDetailPage";

        public ItemDetailPage(IApp app, Platform platform) : base(app, platform)
        {
        }

        public ItemDetailPage CheckLoadView()
        {
            app.WaitForElement(ItemDetailPageId);
            Assert.IsNotEmpty(ItemDetailPageId);

            return this;
        }
    }
}
