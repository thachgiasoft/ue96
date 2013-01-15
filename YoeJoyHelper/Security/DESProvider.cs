using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace YoeJoyHelper.Security
{
    /// <summary>
    /// DES加密/解密
    /// </summary>
    public class DESProvider
    {
        private DESProvider()
        {

        }

        // 默认的初始化密钥
        private static string key = YoeJoyConfig.DESCEncryptKey;

        /// <summary>
        /// 对称加密/解密的密钥
        /// </summary>
        public static string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        #region

        /// <summary>
        /// 采用DES算法对字符串加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptString(string encryptString, string key)
        {
            if (String.IsNullOrEmpty(encryptString))
            {
                throw new ArgumentNullException("encryptString", "不能为空");
            }
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "不能为空");
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            byte[] resultByteArray = EncryptBytes(inputByteArray, keyBytes, keyIV);

            return Convert.ToBase64String(resultByteArray);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <returns></returns>
        public static string EncryptString(string encryptString)
        {
            return EncryptString(encryptString,Key);
        }

        /// <summary>
        /// 采用DES算法对字节数组加密
        /// </summary>
        /// <param name="sourceBytes">要加密的字节数组</param>
        /// <param name="keyBytes">算法的密钥，长度为8的倍数，最大长度64</param>
        /// <param name="keyIV">算法的初始化向量，长度为8的倍数，最大长度64</param>
        /// <returns></returns>
        public static byte[] EncryptBytes(byte[] sourceBytes, byte[] keyBytes, byte[] keyIV)
        {
            if (sourceBytes == null || keyBytes == null)
            {
                throw new ArgumentNullException("sourceBytes和keyBytes", "不能为空");
            }
            else
            {
                //检查密钥数组长度是否是8的倍数并且长度是否小于64
                keyBytes = CheckArrayLength(keyBytes);
                //检查初始化向量数组长度是否是8的倍数并且长度是否小于64
                keyIV = CheckArrayLength(keyIV);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                //实例化内存流
                MemoryStream mStream = new MemoryStream();
                //实例化CryptoStream
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(sourceBytes, 0, sourceBytes.Length);
                cStream.FlushFinalBlock();
                //将内存流转换成字节数组
                byte[] buffer = mStream.ToArray();
                //关闭流
                mStream.Close();
                //关闭流
                cStream.Close();
                return buffer;
            }
        }

        #endregion

        #region

        public static string DecryptString(string decryptString, string key)
        {
            if (String.IsNullOrEmpty(decryptString))
            {
                throw new ArgumentNullException("decryptString", "不能为空");
            }
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "不能为空");
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            byte[] resultByteArray = DecryptBytes(inputByteArray, keyBytes, keyIV);

            return Encoding.UTF8.GetString(resultByteArray);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptString">要解密的字符串</param>
        /// <returns></returns>
        public static string DecryptString(string decryptString)
        {
            return DecryptString(decryptString,Key);
        }

        /// <summary>
        /// 采用DES算法对字节数组解密
        /// </summary>
        /// <param name="sourceBytes">要解密的字节数组</param>
        /// <param name="keyBytes">算法的密钥，长度为8的倍数，最大长度64</param>
        /// <param name="keyIV">算法的初始化向量，长度为8的倍数，最大长度64</param>
        /// <returns></returns>
        public static byte[] DecryptBytes(byte[] sourceBytes, byte[] keyBytes, byte[] keyIV)
        {
            if (sourceBytes == null || keyBytes == null)
            {
                throw new ArgumentNullException("sourceBytes和keyBytes", "不能为空");
            }
            else
            {
                //检查密钥数组长度是否是8的倍数并且长度是否小于64
                keyBytes = CheckArrayLength(keyBytes);
                //检查初始化向量数组长度是否是8的倍数并且长度是否小于64
                keyIV = CheckArrayLength(keyIV);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                //实例化内存流
                MemoryStream mStream = new MemoryStream();
                //实例化CryptoStream
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(sourceBytes, 0, sourceBytes.Length);
                cStream.FlushFinalBlock();
                //将内存流转换成字节数组
                byte[] buffer = mStream.ToArray();
                //关闭流
                mStream.Close();
                //关闭流
                cStream.Close();
                return buffer;
            }
        }

        #endregion

        /// <summary>
        /// 检查密钥或初始化向量的长度，如果不是8的倍数或者长度大于64，则截取前8个元素
        /// </summary>
        /// <param name="byteArray">要检查的数组</param>
        /// <returns></returns>
        private static byte[] CheckArrayLength(byte[] byteArray)
        {
            byte[] resultBytes = new byte[8];
            //如果数组长度小于8
            if (byteArray.Length < 8)
            {
                return Encoding.UTF8.GetBytes("12345678");
            }
            //如果数组长度不是8的倍数
            else if (byteArray.Length % 8 != 0 || byteArray.Length > 64)
            {
                Array.Copy(byteArray, 0, resultBytes, 0, 8);
                return resultBytes;
            }
            else
            {
                return byteArray;
            }
        }
    }
}
