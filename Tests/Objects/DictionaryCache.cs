using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedUtils.Tests.Objects
{
    public class DictionaryCache : SharedUtils.Interfaces.ICacheManager
    {
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public DictionaryCache()
        {
            tmpCache = new Dictionary<string, object>();
        }

        public bool IsCacheActive
        {
            get
            {
                return !(tmpCache == null);
            }
        }

        private Dictionary<string, object> tmpCache;

        public void Clear()
        {
            tmpCache.Clear();
        }

        public void Dispose()
        {
            tmpCache = null;
        }

        public T Get<T>(string key)
        {
            if (IsSet(key))
            {
                return (T)tmpCache[key];
            }
            return default(T);
        }

        public bool IsSet(string key)
        {
            if (tmpCache.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        public void Remove(string key)
        {
            if (IsSet(key))
            {
                tmpCache.Remove(key);
            }
        }

        public void Set<T>(string key, T data, int cacheTime)
        {
            if (IsSet(key))
            {
                tmpCache[key] = data;
            }
            else
            {
                tmpCache.Add(key, data);
            }
        }

        public Task<T> GetAsync<T>(string key)
        {
            cacheLock.EnterReadLock();
            try
            {
                return Task.FromResult((T)tmpCache[key]);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public async Task SetAsync<T>(string key, T data, int cacheTime)
        {
            if (data == null)
            {
                await RemoveAsync(key);
                return;
            }
            if (await IsSetAsync(key))
            {
                cacheLock.EnterWriteLock();
                try
                {
                    tmpCache[key] = data;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
            else
            {
                cacheLock.EnterWriteLock();
                try
                {
                    tmpCache.Add(key, data);
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }

        public Task<bool> IsSetAsync(string key)
        {
            cacheLock.EnterReadLock();
            try
            {
                if (tmpCache == null) { return Task.FromResult(false); }
                if (tmpCache.ContainsKey(key))
                {
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public async Task RemoveAsync(string key)
        {
            if (await IsSetAsync(key))
            {
                cacheLock.EnterWriteLock();
                try
                {
                    tmpCache.Remove(key);
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }
    }
}