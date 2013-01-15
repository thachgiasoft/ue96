using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Icson.Utils
{
	/// <summary>
	/// Summary description for DES.
	/// </summary>
	public class CryptoUtil
	{
		public CryptoUtil()
		{
		}
		private static byte[] key_64 = {181,44,181,40,46,168,244,49};
		private static byte[] iv_64  = {107,93,249,77,56,159,62,251};

		private static byte[] key_192 = {241,209,75,4,138,97,142,47,78,169,86,189,65,250,87,72,173,14,72,20,155,215,36,139};
		private static byte[] iv_192  = {128,19,107,127,217,148,105,0};

		public static string Encrypt(string str)
		{
			DESCryptoServiceProvider  des  =  new  DESCryptoServiceProvider();  
			//���ַ����ŵ�byte������  
			byte[]  inputByteArray  =  Encoding.Unicode.GetBytes(str);  
 
			//�������ܶ������Կ��ƫ����  
			des.Key  =  key_64;
			des.IV  =  iv_64;
			MemoryStream  ms  =  new  MemoryStream();  
			CryptoStream  cs  =  new  CryptoStream(ms,  des.CreateEncryptor(),CryptoStreamMode.Write);  
			//Write  the  byte  array  into  the  crypto  stream  
			//(It  will  end  up  in  the  memory  stream)  
			cs.Write(inputByteArray,  0,  inputByteArray.Length);  
			cs.FlushFinalBlock();  
			//Get  the  data  back  from  the  memory  stream,  and  into  a  string  
			StringBuilder  ret  =  new  StringBuilder();  
			foreach(byte  b  in  ms.ToArray())  
			{  
				//Format  as  hex  
				ret.AppendFormat("{0:X2}",  b);  
			}  
			return  ret.ToString();
		}

		public static string Decrypt(string str)
		{
			DESCryptoServiceProvider  des  =  new  DESCryptoServiceProvider();  
 
			//Put  the  input  string  into  the  byte  array  
			byte[]  inputByteArray  =  new  byte[str.Length  /  2];  
			for(int  x  =  0;  x  <  str.Length  /  2;  x++)  
			{  
				int  i  =  (Convert.ToInt32(str.Substring(x  *  2,  2),  16));  
				inputByteArray[x]  =  (byte)i;  
			} 
 
			//�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�  
			des.Key  =  key_64;
			des.IV  =  iv_64;
			MemoryStream  ms  =  new  MemoryStream();  
			CryptoStream  cs  =  new  CryptoStream(ms,  des.CreateDecryptor(),CryptoStreamMode.Write);  
			//Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
			cs.Write(inputByteArray,  0,  inputByteArray.Length);  
			cs.FlushFinalBlock();  
 
			return  System.Text.Encoding.Unicode.GetString(ms.ToArray()); 
		}

		public static string EncryptTripleDES(string str)
		{
			TripleDESCryptoServiceProvider  des  =  new  TripleDESCryptoServiceProvider();  
			//���ַ����ŵ�byte������  
			byte[]  inputByteArray  =  Encoding.Unicode.GetBytes(str);  
 
			//�������ܶ������Կ��ƫ����  
			des.Key  =  key_192;
			des.IV  =  iv_192;
			MemoryStream  ms  =  new  MemoryStream();  
			CryptoStream  cs  =  new  CryptoStream(ms,  des.CreateEncryptor(),CryptoStreamMode.Write);  
			//Write  the  byte  array  into  the  crypto  stream  
			//(It  will  end  up  in  the  memory  stream)  
			cs.Write(inputByteArray,  0,  inputByteArray.Length);  
			cs.FlushFinalBlock();  
			//Get  the  data  back  from  the  memory  stream,  and  into  a  string  
			StringBuilder  ret  =  new  StringBuilder();  
			foreach(byte  b  in  ms.ToArray())  
			{  
				//Format  as  hex  
				ret.AppendFormat("{0:X2}",  b);  
			}  
			return  ret.ToString();
		}

		public static string DecryptTripleDES(string str)
		{
			TripleDESCryptoServiceProvider  des  =  new  TripleDESCryptoServiceProvider();  
 
			//Put  the  input  string  into  the  byte  array  
			byte[]  inputByteArray  =  new  byte[str.Length  /  2];  
			for(int  x  =  0;  x  <  str.Length  /  2;  x++)  
			{  
				int  i  =  (Convert.ToInt32(str.Substring(x  *  2,  2),  16));  
				inputByteArray[x]  =  (byte)i;  
			} 
 
			//�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�  
			des.Key  =  key_192;
			des.IV  =  iv_192;
			MemoryStream  ms  =  new  MemoryStream();  
			CryptoStream  cs  =  new  CryptoStream(ms,  des.CreateDecryptor(),CryptoStreamMode.Write);  
			//Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
			cs.Write(inputByteArray,  0,  inputByteArray.Length);  
			cs.FlushFinalBlock();  
 
			return  System.Text.Encoding.Unicode.GetString(ms.ToArray()); 
		}
	}
}
