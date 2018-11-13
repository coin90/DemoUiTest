using Xamarin.Forms;
using DemoUITest.Views;
using Xamarin.Forms.Xaml;
using DemoUITest.Services;
using DemoUITest.Models;
using System.Net.Http;
using ModernHttpClient;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DemoUITest
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<HttpClientHandler, NativeMessageHandler>();
            DependencyService.Register<IHttpConnector, HttpConnector>();
            DependencyService.Register<IRickAndMortyService, RickAndMortyService>();
            DependencyService.Register<IDataStore<Character>, CharactersDataStore>();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
