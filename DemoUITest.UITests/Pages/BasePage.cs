using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

namespace DemoUITest.UITests
{
    public abstract class BasePage
    {
        readonly string _pageTitle;

        protected readonly IApp app;
        protected readonly bool OnAndroid;
        protected readonly bool OniOS;
        protected readonly bool OnTablet;

        protected BasePage()
        {
            app = AppInitializer.App;

            OnAndroid = app.GetType() == typeof(AndroidApp);
            OniOS = app.GetType() == typeof(iOSApp);

            if (OniOS)
            {
                var iOSApp = app as iOSApp;
                if (iOSApp.Device.IsPhone)
                    OnTablet = false;
                else
                    OnTablet = true;
            }
            else
            {
                var idiom = app.Invoke("BackdoorGetIdiom");
                OnTablet = !idiom.Equals("Phone");
                // Obtener tablet de Android http://salvoz.com/blog/2016/08/23/detecting-device-idiom-on-xamarin-uitest/
            }

            if (OnTablet)
                app.SetOrientationLandscape();
        }


        protected BasePage(IApp app, Platform platform, string pageTitle)
        {
            this.app = app;

            OnAndroid = platform == Platform.Android;
            OniOS = platform == Platform.iOS;

            _pageTitle = pageTitle;

        }
        public bool IsPageVisible => app.Query(_pageTitle).Length > 0;

        public void WaitForPageToLoad()
        {
            app.WaitForElement(_pageTitle);
        }
    }
}