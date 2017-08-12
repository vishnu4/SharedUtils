using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils.Interfaces
{
    public interface ICacheManager : IDisposable
    {
        bool IsCacheActive { get; }
        T Get<T>(string key);
        void Set<T>(string key, T data, int cacheTime);
        bool IsSet(string key);
        void Remove(string key);


        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data, int cacheTime);
        Task<bool> IsSetAsync(string key);
        Task RemoveAsync(string key);

        void Clear();
    }
}
