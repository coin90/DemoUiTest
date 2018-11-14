using System;
using NUnit.Framework;

namespace DemoUITest.UITests.Pages
{
    public class ItemDetailPage : BasePage
    {
        readonly string ItemDetailPageId = "ItemDetailPage";

        public ItemDetailPage() : base()
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
