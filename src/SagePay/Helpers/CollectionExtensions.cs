using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace SagePay.Helpers
{
    public static class CollectionExtensions
    {
        public static string AsQueryString(this NameValueCollection items)
        {
            var properties = from key in items.AllKeys
                             select key + "=" + HttpUtility.UrlEncode(items[key]);

            return String.Join("&", properties.ToArray());
        }

        public static T SafeFetch<T>(this Dictionary<string, string> data, string key)
        {
            return data.ContainsKey(key) ? (T)Convert.ChangeType(data[key], typeof(T)) : default(T);
        }
    }
}
