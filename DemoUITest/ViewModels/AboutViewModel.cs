using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace DemoUITest.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
            ExceptionCommand = new Command(() => 
            {
                string value = null;
                var exception = value.Length;
            });
        }

        public ICommand OpenWebCommand { get; }
        public ICommand ExceptionCommand { get; }
    }
}