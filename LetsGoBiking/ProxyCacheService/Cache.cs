using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System;

namespace ProxyCacheService
{
    public class Cache<T>
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly Func<string, Task<T>> _handler;

        public Cache(Func<string, Task<T>> handler)
        {
            _handler = handler;
        }

        public async Task<T> Get(string nameOfItem) => 
            await Get(nameOfItem, ObjectCache.InfiniteAbsoluteExpiration);

        public async Task<T> Get(string nameOfItem, double dt_seconds) =>
            await Get(nameOfItem, DateTimeOffset.Now.AddSeconds(dt_seconds));

        public async Task<T> Get(string nameOfItem, DateTimeOffset dt)
        {
            if (_cache.Get(nameOfItem) is T station)
            {
                Console.WriteLine("Found informations in the cache for station with key : " + nameOfItem);
                return station;
            }

            T value = await _handler(nameOfItem);
            _cache.Add(nameOfItem, value, dt);
            return value;
        }
        
        /*public Cache(Func<string, Task<string>> retrieveDataAsync)
        {
            throw new NotImplementedException();
        }

        /**
         * Retrieve data from cache or from the source.
         * @CacheItemName is the key of the entry in the cache. If CacheItemName doesn't exist or has a null content then create a new T object and put it in the cache with CacheItemName as the corresponding key. In this case, the Expiration Time is "dt_default" ( public DateTimeOffset dt_default in ProxyCache class).
         */
        /*public T Get(string cacheItemName, time)
        {
            T objCached = (T)_cache.Get(cacheItemName);
            if (objCached != null)
            {
                Console.WriteLine("Retrieved from cache");
                return objCached;
            }
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration
            };
           
            T myobj = await _handler(cacheItemName);
            _cache.Add(cacheItemName, myobj, cacheItemPolicy);
            return myobj;
        }*/

    }
}