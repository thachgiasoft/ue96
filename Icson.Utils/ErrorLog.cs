using System;
using System.Web;
using System.IO;
using System.Configuration;
using System.Text;

namespace Icson.Utils
{
 
	public class ErrorLog
	{
		private static object locker = new object();

		private ErrorLog()
		{
		}

		private static ErrorLog log = new ErrorLog();


		/// <summary>
		/// 返回日志实例
		/// </summary>
		/// <returns></returns>
		public static ErrorLog GetInstance()
		{
			lock( locker)
			{
				if(AppConfig.ErrorLogFolder != null)
				{
					errorLogFolder = AppConfig.ErrorLogFolder;

					try
					{
						if ( ! Directory.Exists(errorLogFolder) )
							Directory.CreateDirectory(errorLogFolder);
					}
					catch(Exception exp)
					{
						Console.Write(exp.ToString());
					}
				}
					
				string str = DateTime.Now.ToString(AppConst.DateFormat);
				errorLogFile = errorLogFolder + str + ".txt";

				try
				{
					if(File.Exists(errorLogFile) == false)
					{
						FileStream stream = File.Create(errorLogFile);
						stream.Close();
					}
				}
				catch(Exception exp)
				{
					Console.Write(exp.ToString());
				}
			}

			return log;
		}

		/// <summary>
		/// write log to file
		/// </summary>
		/// <param name="message"></param>
		public void Write(string message)
		{
			lock(locker)
			{
				StreamWriter writer = null;
				try
				{
					writer = new StreamWriter(errorLogFile,true,System.Text.Encoding.Default);
					StringBuilder sb = new StringBuilder(500);
					sb.Append(DateTime.Now.ToString()).Append("\r\n");
					sb.Append(message).Append("\r\n");
					sb.Append("---------------------------------------").Append("\r\n");
					writer.Write(sb.ToString());
				}
				catch(IOException ioe)
				{
					Console.WriteLine(ioe.ToString());
				}
				finally
				{
					try
					{
						if(writer != null)
							writer.Close();
					}
					catch(Exception e)
					{
						Console.WriteLine(e.ToString());
					}
				}
			}
		}

		private static string errorLogFolder = @"c:\Temp\IPP3Log\";
		private static string errorLogFile;

	}
}
