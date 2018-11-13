
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DemoUITest.ViewModels;

namespace DemoUITest.Views
{

    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();
        }
    }
}