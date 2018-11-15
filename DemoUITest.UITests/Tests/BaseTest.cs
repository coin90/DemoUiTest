using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

namespace DemoUITest.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]

    public abstract class BaseTest
    {
        protected IApp app;
        protected Platform platform;

        protected bool OnAndroid;
        protected bool OniOS;
        protected bool OnTablet;

        //protected ItemsPage ItemsPage;

        string email = "email";
        string password = "pass";

        string EntryUser = "user";
        string EntryPass = "pass";
        string LoginButton = "LoginButton";

        protected BaseTest(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        virtual public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);

            OnAndroid = app.GetType() == typeof(AndroidApp);
            OniOS = app.GetType() == typeof(iOSApp);

            //if (OniOS)
            //{
            //    var iOSApp = app as iOSApp;
            //    if (iOSApp.Device.IsPhone)
            //        OnTablet = false;
            //    else
            //        OnTablet = true;
            //}
            //else
            //{
            //    var idiom = app.Invoke("BackdoorGetIdiom");
            //    OnTablet = !idiom.Equals("Phone");
            //}

            app.Screenshot("App Initialized");

            Login();
        }

        private void Login()
        {
            try
            {
                app.Tap(EntryUser);
                app.EnterText(EntryUser, email);
                app.Screenshot($"Entered user {email}");

                app.Tap(EntryPass);
                app.EnterText(EntryPass, password);
                app.Screenshot($"Entered password {password}");

                app.Tap(x => x.Button("Login"));

            }
            catch (System.Exception)
            {
                app.Screenshot("ERROR Login");
            }
        }
    }
}