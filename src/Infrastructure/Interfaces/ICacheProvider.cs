using System;

namespace PureGym.Core.Interfaces
{
    public interface ICacheProvider
    {
        T GetValue<T>(string key);
        void Add<T>(string key, T value, TimeSpan expiration);
        void Delete(string key);
    }
}
