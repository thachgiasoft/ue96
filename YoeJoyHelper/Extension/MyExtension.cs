using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace YoeJoyHelper.Extension
{
    public static class MyExtension
    {
        /// <summary>
        /// 判断字符串是否是安全值的扩展方法
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsSafeString(this string target)
        {
            bool isSafeString = true;
            if (String.IsNullOrEmpty(target))
            {
                isSafeString = false;
            }
            return isSafeString;
        }

        /// <summary>
        /// 获得安全字符串的扩展方法
        /// 防止字符串在输入时，出现空指针异常
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetSafeString(this string target)
        {
            if (String.IsNullOrEmpty(target))
            {
                return String.Empty;
            }
            else
            {
                return target.Trim();
            }
        }

        /// <summary>
        /// 编码URL中文参数
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetUrlEncodeStr(this string target)
        {
            if (String.IsNullOrEmpty(target))
            {
                return String.Empty;
            }
            else
            {
                return HttpUtility.UrlEncode(target);
            }
        }

        /// <summary>
        /// 解码URL中文参数
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetUrlDecodeStr(this string target)
        {
            if (String.IsNullOrEmpty(target))
            {
                return String.Empty;
            }
            else
            {
                return HttpUtility.UrlDecode(target);
            }
        }

    }
}