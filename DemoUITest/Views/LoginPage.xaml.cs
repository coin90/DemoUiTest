using System;
using System.Collections.Generic;
using DemoUITest.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoUITest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LoginViewModel(Navigation);
        }

        async void Login_Clicked(object sender, EventArgs e)
        {               
            viewModel.LoginCommand.Execute(null);
            Navigation.PushAsync(new MainPage());
        }
    }

}
