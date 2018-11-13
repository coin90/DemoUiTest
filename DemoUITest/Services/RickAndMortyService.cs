using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemoUITest.Models;
using Xamarin.Forms;

namespace DemoUITest.Services
{
    public class RickAndMortyService : IRickAndMortyService
    {
        readonly string _charectersEndpoint = "https://rickandmortyapi.com/api/character";
        readonly IHttpConnector _connector;
        public RickAndMortyService()
        {
            _connector = DependencyService.Get<IHttpConnector>();
        }

        public async Task<IEnumerable<Character>> GetAllCharacters(CancellationToken token)
        {
            var response = await _connector.Get<RickAndMortyResponse<Character>>(_charectersEndpoint, token);
            return response.Results;
        }

        public Task<Character> GetCharacter(int id, CancellationToken token)
        {
            return _connector.Get<Character>($"{_charectersEndpoint}/{id}", token);
        }
    }
}
