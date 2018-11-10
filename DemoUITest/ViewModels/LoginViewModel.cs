using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Content.Res;
using DemoUITest.Views;
using Xamarin.Forms;

namespace DemoUITest.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        private INavigation _navigation { get; set; }

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
            Pass = User = string.Empty;
        }


        async Task ExecuteLoginCommand()
        {
            if (IsBusy)
                return;
            
            IsBusy = true;

            try
            {
                if (User.Equals("user") && Pass.Equals("1234"))
                {
                    
                    await _navigation.PushAsync(new MainPage());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
