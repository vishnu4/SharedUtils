using SharedUtils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils.Extensions
{
    public static class CacheExtensions
    {
        public const int DefaultExpireMinutes = 60;
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, DefaultExpireMinutes, acquire);
        }

        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                var result = acquire();
                cacheManager.Set(key, result, cacheTime);
                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key, Func<Task<T>> acquire)
        {
            return await GetAsync(cacheManager, key, DefaultExpireMinutes, acquire);
        }

        public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<Task<T>> acquire)
        {
            if (await cacheManager.IsSetAsync(key))
            {
                return await cacheManager.GetAsync<T>(key);
            }
            else
            {
                var result = await acquire();
                await cacheManager.SetAsync(key, result, cacheTime);
                return result;
            }
        }
    }
}
