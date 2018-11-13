using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DemoUITest.Services
{
    public interface IDataStore<T>
    {
        Task<T> GetItemAsync(int id, CancellationToken token);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh, CancellationToken token);
    }
}
