using System;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;


namespace DemoUITest.UITests.Pages
{
    public class AboutPage : BasePage
    {
        string AboutPageId = "AboutPage";
        string TabAbout = "TabAbout";
        string ButtonOpenWebView = "OpenWebView";

        public AboutPage() : base()
        {
        }

        public AboutPage CheckLoadView()
        {
            app.WaitForElement(AboutPageId);
            Assert.IsNotEmpty(AboutPageId);

            return this;
        }

        public AboutPage NavigateToAbout()
        {
            app.Tap(TabAbout);
            return this;
        }

        public AboutPage OpenWebView()
        {
            app.ScrollDown();
            app.Tap(ButtonOpenWebView);

            return this;
        }

        public AboutPage CheckLoadWebView()
        {
            Query query;
            if (OniOS)
            {
                query = x => x.Class("UIWebView");
            }
            else
            {
               query = x => x.Id("agentWebView");
            }

            app.WaitForElement(query);

            return this;
        }
    }
}
