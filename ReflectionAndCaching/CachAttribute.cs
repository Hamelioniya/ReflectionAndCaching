using System;

namespace ReflectionAndCaching
{
    public class CachAttribute : Attribute
    {
        public CachAttribute()
        {
        }

        public CachAttribute(int hour, int min, int sec)
        {
            CachingTime = new TimeSpan(hour, min, sec);
        }

        public TimeSpan CachingTime { get; set; }
    }
}
