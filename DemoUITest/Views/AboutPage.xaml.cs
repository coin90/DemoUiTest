using System;
using DemoUITest.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoUITest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel();
        }
    }
}