using System;
using System.Web;
using System.Web.Caching;
using PureGym.Core.Interfaces;

namespace PureRide.Web.Providers
{
    public class HttpRuntimeCacheProvider : ICacheProvider
    {
        public T GetValue<T>(string key)
        {
            var result = HttpRuntime.Cache.Get(key);
            return (T)result;
        }

        public void Add<T>(string key, T value, TimeSpan expiration)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.Add(expiration), Cache.NoSlidingExpiration);
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }
    }
}
