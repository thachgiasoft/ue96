using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoeJoyHelper
{
    /// <summary>
    /// 共享的缓存对象
    /// 当一个静态的对象被网站的大多数地方共同使用时
    /// 就被称为共享的缓存对象
    /// </summary>
    internal class SharedCacheObj<T>
    {
        /// <summary>
        /// 获取一个共享的缓存对象
        /// 不允许缓存null的对象
        /// </summary>
        /// <param name="settings">缓存设定</param>
        /// <param name="obj">需要缓存的对象</param>
        /// <returns></returns>
        internal static T GetSharedCacheObj(CacheObjSetting settings, T obj)
        {
            string key = settings.CacheKey;
            if (CacheHelper.IsAlive(key))
            {
                return (T)CacheHelper.Get(key);
            }
            else
            {
                if (obj != null)
                {
                    CacheHelper helper = new CacheHelper(key, obj, settings.CacheDuration);
                    helper.Add();
                }
                return obj;
            }
        }
    }
}
