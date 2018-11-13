
using DemoUITest.Models;

namespace DemoUITest.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Character Item { get; set; }
        public ItemDetailViewModel(Character item = null)
        {
            Title = item?.Name;
            Item = item;
        }
    }
}
