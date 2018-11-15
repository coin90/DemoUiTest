using System;
using DemoUITest.UITests.Pages;
using NUnit.Framework;
using Xamarin.UITest;

namespace DemoUITest.UITests.Tests
{
    public class AboutTest : BaseTest
    {
        public AboutTest(Platform platform) : base(platform)
        {
        }

        [Test]
        public void LoadAbout()
        {
            var aboutPage = new AboutPage(app, platform);

            aboutPage.NavigateToAbout()
                     .CheckLoadView();

            app.Screenshot("About page loadded");
        }

        [Test]
        public void OpenWebView()
        {
            var aboutPage = new AboutPage(app, platform);

            aboutPage.NavigateToAbout()
                     .OpenWebView();

            app.Screenshot("WebView page loadded");
        }

        [Test]
        public void TestException()
        {
            var aboutPage = new AboutPage(app, platform);

            aboutPage.NavigateToAbout()
                     .OpenException();
        }
    }
}
