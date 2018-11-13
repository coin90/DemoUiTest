using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using DemoUITest.Services;
using DemoUITest.Models;

namespace DemoUITest.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Character> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        readonly IDataStore<Character> _charactersDS;

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Character>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            _charactersDS = DependencyService.Get<IDataStore<Character>>();
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await _charactersDS.GetItemsAsync(true, _dataCTS.Token);
                foreach (var item in items)
                {
                    Items.Add(item);
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