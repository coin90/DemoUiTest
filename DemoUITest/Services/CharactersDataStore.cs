using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemoUITest.Models;
using Xamarin.Forms;

namespace DemoUITest.Services
{
    public class CharactersDataStore : IDataStore<Character>
    {
        readonly IRickAndMortyService service;

        public CharactersDataStore()
        {
            service = DependencyService.Get<IRickAndMortyService>();
        }

        public Task<Character> GetItemAsync(int id, CancellationToken token)
        {
            return service.GetCharacter(id, token);
        }

        IEnumerable<Character> characters;

        public async Task<IEnumerable<Character>> GetItemsAsync(bool forceRefresh, CancellationToken token)
        {
            if(forceRefresh || characters == null)
                characters = await service.GetAllCharacters(token);
            return characters;
        }
    }
}
