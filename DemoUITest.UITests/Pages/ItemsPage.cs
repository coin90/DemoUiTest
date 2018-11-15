using System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace DemoUITest.UITests
{
    public class ItemsPage : BasePage
    {
        readonly Query _addToolbarButton;
        string ItemsPageId = "ItemsPage";

        public ItemsPage(IApp app, Platform platform) : base(app, platform)
        {
            if (OniOS)
                _addToolbarButton = x => x.Class("UIButtonLabel").Index(0);
            else
                _addToolbarButton = x => x.Class("android.support.v7.view.menu.ActionMenuItemView").Index(0);
        }

        public ItemsPage CheckLoadView()
        {
            app.WaitForElement(ItemsPageId);
            Assert.IsNotEmpty(ItemsPageId);

            return this;
        }

        public void SelectFirstCellByAutomationId()
        {
            Func<AppQuery, AppQuery> firstCellInList;

            firstCellInList = x => x.Marked("ItemsListView").Index(0);

            if (firstCellInList != null)
            {
                app.WaitForElement(firstCellInList, "Timed our waiting for the first user to appear", TimeSpan.FromSeconds(10));
            }


            app.Tap(firstCellInList);
        }
    }
}