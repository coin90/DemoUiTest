using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemoUITest.Models;

namespace DemoUITest.Services
{
    public class MockDataStore : IDataStore<Character>
    {
        List<Character> items;

        public MockDataStore()
        {
            items = new List<Character>();
            var mockItems = new List<Character>
            {
                new Character(0, "Jerry", CharacterStatus.Alive),
                new Character(0, "Beth", CharacterStatus.Alive),
                new Character(0, "Morty", CharacterStatus.Alive),
                new Character(0, "Rick", CharacterStatus.Alive),
                new Character(0, "Mr President", CharacterStatus.Alive),
                new Character(0, "Mr Poophole", CharacterStatus.Alive),
                new Character(0, "Cop Morty", CharacterStatus.Dead)
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<IEnumerable<Character>> GetItemsAsync(bool forceRefresh, CancellationToken token)
        {
            return await Task.FromResult(items);
        }

        public Task<Character> GetItemAsync(int id, CancellationToken token)
        {
            return Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }
    }
}