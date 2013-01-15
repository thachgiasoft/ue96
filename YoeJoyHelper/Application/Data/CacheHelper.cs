using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace YoeJoyHelper
{
    public class CacheHelper
    {
        public CacheHelper(string key, Object value, int duration)
        {
            Key = key;
            Value = value;
            Duration = duration;
        }

        /// <summary>
        /// 缓存主键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 缓存的数据
        /// </summary>
        public Object Value { get; set; }

        /// <summary>
        /// 缓存秒数
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 添加缓存
        /// </summary>
        public void Add()
        {
            if (IsAlive(Key))
            {
                return;
            }
            else
            {
                System.Web.HttpRuntime.Cache.Insert(Key, Value, null, DateTime.Now.AddSeconds(Duration), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// 换得缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public static bool IsAlive(string key)
        {
            if (Get(key) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        public void RemoveAll()
        {
            foreach (DictionaryEntry entry in HttpRuntime.Cache)
            {
                HttpRuntime.Cache.Remove(entry.Key.ToString());
            }
        }
    }
}
