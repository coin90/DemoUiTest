using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemoUITest.Models;

namespace DemoUITest.Services
{
    public interface IRickAndMortyService
    {
        Task<IEnumerable<Character>> GetAllCharacters(CancellationToken token);
        Task<Character> GetCharacter(int id, CancellationToken token);
    }
}
