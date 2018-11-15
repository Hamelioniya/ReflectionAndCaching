using System;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ReflectionAndCaching
{
    public class CachManager
    {
        private static ConnectionMultiplexer _redis;

        static CachManager()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
        }

        public static void SetInCach<T>(T value, string key)
        {
            var db = _redis.GetDatabase();
            var json = Serealize(value);
            var expiry = GetTimeOut(value);
            db.StringSet(key, json, expiry);
        }

        public static T GetFromCach<T>(string key)
        {
            var db = _redis.GetDatabase();
            var json = db.StringGet(key);

            T value = default(T);

            if (!json.IsNullOrEmpty)
            {
                value = Deserialize<T>(json);
            }

            return value;
        }

        private static string Serealize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        private static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        private static TimeSpan? GetTimeOut<T>(T value)
        {
            TimeSpan? timeOut = null;

            var cachAttribute = (CachAttribute)(typeof(T)).GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType() == typeof(CachAttribute));
            if (!ReferenceEquals(cachAttribute, null))
            {
                timeOut = cachAttribute.CachingTime;
            }

            return timeOut;
        }
    }
}
