using System;
using NUnit.Framework;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;


namespace DemoUITest.UITests.Pages
{
    public class AboutPage : BasePage
    {
        string AboutPageId = "AboutPage";
        string TabAbout = "TabAbout";
        string ButtonOpenWebView = "OpenWebView";
        string ButtonException = "Exception";

        public AboutPage(IApp app, Platform platform) : base(app, platform)
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
            if (OniOS)
                app.Tap(TabAbout);

            if (OnAndroid)
                app.Tap(x => x.Text("About"));
            
            return this;
        }

        public AboutPage OpenWebView()
        {
            app.ScrollDown();
            app.Tap(ButtonOpenWebView);

            return this;
        }

        public AboutPage OpenException()
        {
            app.ScrollDown();
            app.Tap(ButtonException);

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
