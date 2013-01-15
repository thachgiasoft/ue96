using System;
using System.Web;

namespace Icson.Utils
{
	/// <summary>
	/// Summary description for CookieUtil.
	/// </summary>
	public class CookieUtil
	{
		public CookieUtil()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//SetTripleDESEncryptedCookie （只针对密钥和Cookie数据）
		public static void SetTripleDESEncryptedCookie(string key, string value)
		{
			SetCookie(key, value, AppConst.DateTimeNull, "TripleDES");
		}
		//SetTripleDESEncryptedCookie （增加了Cookie数据的有效期参数）
		public static void SetTripleDESEncryptedCookie(string key, string value, DateTime expires)
		{
			SetCookie(key, value, expires, "TripleDES");
		}
		//SetEncryptedCookie（只针对密钥和Cookie数据）
		public static void SetDESEncryptedCookie(string key, string value)
		{
			SetCookie(key, value, AppConst.DateTimeNull, "DES");
		}
		public static void SetDESEncryptedCookie(string key, string value, DateTime expires)
		{
			SetCookie(key, value, expires, "DES");
		}

		private static void SetCookie(string key, string value, DateTime expires, string desType)
		{
			if ( desType.ToLower() == "des" )
			{
				key = CryptoUtil.Encrypt(key);
				value = CryptoUtil.Encrypt(value);
			}
			else
			{
				key = CryptoUtil.EncryptTripleDES(key);
				value = CryptoUtil.EncryptTripleDES(value);
			}

			key = HttpContext.Current.Server.UrlEncode(key);
			value = HttpContext.Current.Server.UrlEncode(value);

			//HttpCookie cookie = new HttpCookie("IcsonCookie");
            HttpCookie cookie = new HttpCookie(key);
			cookie.Values[key] = value;

			if ( expires != AppConst.DateTimeNull )
				cookie.Expires = expires;

			HttpContext.Current.Response.Cookies.Set(cookie);			
		}

		private static string GetCookie(string key, string desType)
		{
			if ( desType.ToLower() == "des")
			{
				key = CryptoUtil.Encrypt(key);
			}
			else
			{
				key = CryptoUtil.EncryptTripleDES(key);
			}
			key = HttpContext.Current.Server.UrlEncode(key);
			//HttpCookie cookie = HttpContext.Current.Request.Cookies["IcsonCookie"];
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
			string value = "";
			try
			{
				value = cookie[key];
				value = HttpContext.Current.Server.UrlDecode(value);

				if ( desType.ToLower() == "des")
				{
					value = CryptoUtil.Decrypt(value);
				}
				else
				{
					value = CryptoUtil.DecryptTripleDES(value);
				}
			}
			catch
			{
			}
			return value;
		}

		//获取COOKIE
		public static string GetTripleDESEncryptedCookieValue(string key)
		{
			return GetCookie(key, "TripleDES");
		}
		public static string GetDESEncryptedCookieValue(string key)
		{
			return GetCookie(key, "DES");
		}

        public const string Cookie_BrowseHistory = "IcsonBH";
        //public static void SetCookieByDES(string cookieName, string val, DateTime expires)
        //{
        //    HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
        //    if (cookie == null)
        //        cookie = new HttpCookie(cookieName);

        //    if (expires != AppConst.DateTimeNull)
        //        cookie.Expires = expires;

        //    val = CryptoUtil.Encrypt(val);
        //    val = HttpContext.Current.Server.UrlEncode(val);

        //    cookie.Value = val;

        //    HttpContext.Current.Response.Cookies.Set(cookie);
        //}

        //public static string GetCookieByDES(string cookieName)
        //{
        //    HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
        //    if (cookie == null)
        //        return "";
        //    else
        //    {
        //        string val = cookie.Value;
        //        val = HttpContext.Current.Server.UrlDecode(val);
        //        val = CryptoUtil.Decrypt(val);
        //        return val;
        //    }
        //}
	}
}
