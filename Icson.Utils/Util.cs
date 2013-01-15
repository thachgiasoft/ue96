using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;

using System.Net;
using System.Web.Security;
using System.Web.UI;

namespace Icson.Utils
{
	/// <summary>
	/// Summary description for Util.
	/// </summary>
	public class Util
	{
		public Util()
		{
		}

		private static string []ChineseNum = {"零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖"};

		public static bool IsDate(string date)
		{
			try
			{
				DateTime.Parse(date);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool IsZipCode(string zip)
		{
			if(IsNaturalNumber(zip)&&zip.Length==6)
				return true;
			else
				return false;
		}

		public static bool IsMoney(string money)
		{
			try
			{
				Decimal.Parse(money);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool HasMoreRow(DataSet ds)
		{
			if ( ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public static bool HasMoreRow(DataTable dt)
		{
			if(dt==null||dt.Rows.Count==0)
				return false;
			else
				return true;
		}

		//整理字符串到安全格式
		public static string SafeFormat(string strInput)
		{
			return strInput.Trim().Replace("'","''");
		}

		public static string ToSqlString(string paramStr)
		{
			return "'" + SafeFormat(paramStr) + "'";
		}

		public static string ToSqlLikeString(string paramStr)
		{
			return "'%" + SafeFormat(paramStr) + "%'";
		}
		public static string ToSqlLikeStringR(string paramStr)
		{
			return "'" + SafeFormat(paramStr) + "%'";
		}

        /// <summary>
        /// 就是一组数字或文字拼接到SQL文中的IN Clause中去。比如传入一个数组，得到拼接好的(a,b,c,d)之类的
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToSqlInString(ICollection list)
        {
            StringBuilder res = new StringBuilder();

            int i = 0;
            foreach (object o in list)
            {
                if (i != 0)
                    res.Append(',');

                res.Append(o.ToString());
                i++;
            }
            return "(" + res.ToString() + ")";
        }        

		/// <summary>
		/// 传入的参数必须是'yyyy-MM-dd' 格式. 不另外检查
		/// </summary>
		/// <param name="paramDate"></param>
		/// <returns></returns>
		public static string ToSqlEndDate(string paramDate)
		{
			return ToSqlString(paramDate + " 23:59:59");
		}

		public static string TrimNull(Object obj)
		{
			if(obj is System.DBNull)
			{
				return AppConst.StringNull;
			}
			else
			{
				return obj.ToString().Trim();
			}
		}

		public static int TrimIntNull(Object obj)
		{
			if(obj is System.DBNull)
			{
				return AppConst.IntNull;
			}
			else
			{
				return Int32.Parse(obj.ToString());
			}
		}

		public static decimal TrimDecimalNull(Object obj)
		{
			if(obj is System.DBNull)
			{
				return AppConst.DecimalNull;
			}
			else
			{
				return decimal.Parse(obj.ToString());
			}
		}
		public static DateTime TrimDateNull(Object obj)
		{
			if(obj is System.DBNull)
			{
				return AppConst.DateTimeNull;
			}
			else
			{
				return DateTime.Parse(obj.ToString());
			}
		}
		public static string  GetMD5(String str) 
		{
			return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str,"md5").ToLower();
		}

        public static string MakeMD5(string str)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }

		// Function to test for Positive Integers.  
		public static bool IsNaturalNumber(String strNumber)
		{
			Regex objNotNaturalPattern=new Regex("[^0-9]");
			Regex objNaturalPattern=new Regex("0*[1-9][0-9]*");
			return  !objNotNaturalPattern.IsMatch(strNumber) &&
				objNaturalPattern.IsMatch(strNumber);
		}  

		// Function to test for Positive Integers with zero inclusive  

		public static bool IsWholeNumber(String strNumber)
		{
			Regex objNotWholePattern=new Regex("[^0-9]");
			return !objNotWholePattern.IsMatch(strNumber);
		}  

		// Function to Test for Integers both Positive & Negative  

		public static bool IsInteger(String strNumber)
		{
			Regex objNotIntPattern=new Regex("[^0-9-]");
			Regex objIntPattern=new Regex("^-[0-9]+$|^[0-9]+$");
			return  !objNotIntPattern.IsMatch(strNumber) &&  objIntPattern.IsMatch(strNumber);
		} 

		// Function to Test for Positive Number both Integer & Real 

		public static bool IsPositiveNumber(String strNumber)
		{
			Regex objNotPositivePattern=new Regex("[^0-9.]");
			Regex objPositivePattern=new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
			Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
			return !objNotPositivePattern.IsMatch(strNumber) &&
				objPositivePattern.IsMatch(strNumber)  &&
				!objTwoDotPattern.IsMatch(strNumber);
		}  

		// Function to test whether the string is valid number or not
		public static bool IsNumber(String strNumber)
		{
			Regex objNotNumberPattern=new Regex("[^0-9.-]");
			Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
			Regex objTwoMinusPattern=new Regex("[0-9]*[-][0-9]*[-][0-9]*");
			String strValidRealPattern="^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
			String strValidIntegerPattern="^([-]|[0-9])[0-9]*$";
			Regex objNumberPattern =new Regex("(" + strValidRealPattern +")|(" + strValidIntegerPattern + ")");
			return !objNotNumberPattern.IsMatch(strNumber) &&
				!objTwoDotPattern.IsMatch(strNumber) &&
				!objTwoMinusPattern.IsMatch(strNumber) &&
				objNumberPattern.IsMatch(strNumber);
		}  

		// Function To test for Alphabets. 
		public static bool IsAlpha(String strToCheck)
		{
			Regex objAlphaPattern=new Regex("[^a-zA-Z]");
			return !objAlphaPattern.IsMatch(strToCheck);
		}
		// Function to Check for AlphaNumeric.
		public static bool IsAlphaNumeric(String strToCheck)
		{
			Regex objAlphaNumericPattern=new Regex("[^a-zA-Z0-9]");
			return !objAlphaNumericPattern.IsMatch(strToCheck);    
		}
		public static bool IsEmailAddress(string strEmailAddress)
		{
			Regex objNotEmailAddress = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
			return objNotEmailAddress.IsMatch(strEmailAddress);
		}
		// Function Format Money 
		public static decimal ToMoney(string moneyStr)
		{
			return decimal.Round(decimal.Parse(moneyStr),2);
		}
		public static decimal ToMoney(decimal moneyValue)
		{
			return decimal.Round(moneyValue,2);
		}
		/// <summary>
		/// 舍去金额的分,直接舍去,非四舍五入
		/// </summary>
		/// <param name="moneyValue"></param>
		/// <returns></returns>
		public static decimal TruncMoney(decimal moneyValue)
		{
			int tempAmt = Convert.ToInt32(moneyValue*100)%10;
			moneyValue = (decimal)((moneyValue*100 - tempAmt)/100m);
			return moneyValue;
		}
		/// <summary>
		/// 判断是否手机号码
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		public static bool IsCellNumber(string cell)
		{
			if ( cell.Length != 11)
			{
				return false;
			}
			long number;
			try
			{
				number = Convert.ToInt64(cell);
				if ( number < 13000000000)
					return false;
				else if ( number >= 16000000000)
					return false;
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static void ToExcel(System.Web.UI.WebControls.DataGrid DataGrid2Excel,string FileName,string Title)
		{
			ToExcel(DataGrid2Excel, FileName, Title, "");
		}

		public static void ToExcel(System.Web.UI.WebControls.DataGrid DataGrid2Excel,string FileName,string Title, string Head)
		{
			HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment; filename=" + FileName + ".xls");  //filename=Report.xls		
			HttpContext.Current.Response.Charset = "GB2312"; //UTF-8 
			HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			HttpContext.Current.Response.ContentType="application/vnd.ms-excel";
			//msword;image/JPEG;text/HTML;image/GIF;vnd.ms-excel

			//HttpContext.Current.Application.Page.EnableViewState = false; //Turn off the view state.

			System.IO.StringWriter tw = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

			hw.WriteLine(@"<HTML>");
			hw.WriteLine(@"<BODY>");
			hw.WriteLine("<b>" + Title + "</b>");
			
			//string Head = @"<table border=1><tr><td>1</td><td>2</td></tr><tr><td>3</td><td>4</td></tr></table>";
			if ( Head != "" )
				hw.WriteLine(Head);

			DataGrid2Excel.RenderControl(hw); //Get the HTML for the control.
			hw.WriteLine(@"</BODY>");
			hw.WriteLine(@"</HTML>");
			hw.Flush();
			hw.Close();
			HttpContext.Current.Response.Write(tw.ToString()); //Write the HTML back to the browser.
			HttpContext.Current.Response.End();
		}

		public static void ToExcelD(System.Web.UI.WebControls.DataGrid DataGrid2Excel,string FileName,string Title, string Head,string foot)
		{
			HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment; filename=" + FileName + ".xls");  //filename=Report.xls		
            HttpContext.Current.Response.Charset = "UTF-8"; //UTF-8 
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
			HttpContext.Current.Response.ContentType="application/vnd.ms-excel";
			//msword;image/JPEG;text/HTML;image/GIF;vnd.ms-excel

			//HttpContext.Current.Application.Page.EnableViewState = false; //Turn off the view state.

			System.IO.StringWriter tw = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

			hw.WriteLine(@"<HTML>");
			hw.WriteLine(@"<BODY>");
			hw.WriteLine("<b>" + Title + "</b>");

			//string Head = @"<table border=1><tr><td>1</td><td>2</td></tr><tr><td>3</td><td>4</td></tr></table>";
			if ( Head != "" )
				hw.WriteLine(Head);

			DataGrid2Excel.RenderControl(hw); //Get the HTML for the control.
			if ( foot != "" )
				hw.WriteLine(foot);
			hw.WriteLine(@"</BODY>");
			hw.WriteLine(@"</HTML>");
			hw.Flush();
			hw.Close();
			HttpContext.Current.Response.Write(tw.ToString()); //Write the HTML back to the browser.
			HttpContext.Current.Response.End();
		}

		public static void ToExcelE(System.Web.UI.WebControls.DataGrid DataGrid2Excel,string FileName)
		{
			HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment; filename=" + FileName + ".xls");  //filename=Report.xls		
			HttpContext.Current.Response.Charset = "GB2312"; //UTF-8 
			HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			HttpContext.Current.Response.ContentType="application/vnd.ms-excel";

			System.IO.StringWriter tw = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

			DataGrid2Excel.RenderControl(hw); //Get the HTML for the control.
			hw.Flush();
			hw.Close();
			HttpContext.Current.Response.Write(tw.ToString()); //Write the HTML back to the browser.
			HttpContext.Current.Response.End();
		}

        public static void ToExcelF(System.Web.UI.HtmlControls.HtmlTable Table1, string FileName, string Title, string Head, string foot)
        {
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + ".xls");  //filename=Report.xls		
            HttpContext.Current.Response.Charset = "UTF-8"; //UTF-8 
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //msword;image/JPEG;text/HTML;image/GIF;vnd.ms-excel

            //HttpContext.Current.Application.Page.EnableViewState = false; //Turn off the view state.

            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

            hw.WriteLine(@"<HTML>");
            hw.WriteLine(@"<BODY>");
            hw.WriteLine("<b>" + Title + "</b>");

            //string Head = @"<table border=1><tr><td>1</td><td>2</td></tr><tr><td>3</td><td>4</td></tr></table>";
            if (Head != "")
                hw.WriteLine(Head);
            Table1.RenderControl(hw);

            if (foot != "")
                hw.WriteLine(foot);
            hw.WriteLine(@"</BODY>");
            hw.WriteLine(@"</HTML>");
            hw.Flush();
            hw.Close();
            HttpContext.Current.Response.Write(tw.ToString()); //Write the HTML back to the browser.
            HttpContext.Current.Response.End();
        }

        public static void ToExcelG(System.Web.UI.WebControls.DataGrid DataGrid2Excel, string FileName, string Title, string Head, string Foot)//add by zachary 2009-04-07 
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

            ToExcelFrontDecorator(hw);
            if (Title != "")
                hw.Write(Title + "<br>");
            if (Head != "")
                hw.Write(Head);

            DataGrid2Excel.EnableViewState = false;
            DataGrid2Excel.RenderControl(hw);

            if (Foot != "")
                hw.Write(Foot + "<br>");

            ToExelRearDecorator(hw);

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.Buffer = true;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(FileName) + ".xls");
            response.Charset = "UTF-8";
            response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            response.Write(sw.ToString());
            response.End();
        }

        /// <summary>
        /// Renders the html text before the datagrid.
        /// </summary>
        /// <param name="writer">A HtmlTextWriter to write html to output stream</param>
        private static void ToExcelFrontDecorator(HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("HTML");
            writer.WriteFullBeginTag("Head");
            //			writer.RenderBeginTag(HtmlTextWriterTag.Style);
            //			writer.Write("<!--");
            //			
            //			StreamReader sr = File.OpenText(CurrentPage.MapPath(MY_CSS_FILE));
            //			String input;
            //			while ((input=sr.ReadLine())!=null) 
            //			{
            //				writer.WriteLine(input);
            //			}
            //			sr.Close();
            //			writer.Write("-->");
            //			writer.RenderEndTag();
            writer.WriteEndTag("Head");
            writer.WriteFullBeginTag("Body");
        }

        /// <summary>
        /// Renders the html text after the datagrid.
        /// </summary>
        /// <param name="writer">A HtmlTextWriter to write html to output stream</param>
        private static void ToExelRearDecorator(HtmlTextWriter writer)
        {
            writer.WriteEndTag("Body");
            writer.WriteEndTag("HTML");
        }

		public static void ToTxtFile(System.Web.UI.WebControls.DataGrid DataGrid2Txt,string FileName,string ColumnIDs,bool AddID,int POSysNo)
		{
			HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment; filename=" + FileName + ".txt");  //filename=Report.xls		
			HttpContext.Current.Response.Charset = "GB2312"; 
			HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			HttpContext.Current.Response.ContentType="application/vnd.ms-txt";

			System.IO.StringWriter tw = new System.IO.StringWriter();

			string[] Columns = ColumnIDs.Split(';');

			string sLine = "";
			if(AddID)
			{
				sLine = "\"ID\",";
			}
			for(int i=0;i<Columns.Length;i++)
			{
				sLine += "\""  +  DataGrid2Txt.Columns[Int32.Parse(Columns[i])].HeaderText + "\",";
			}
			sLine = sLine.Substring(0,sLine.Length-1);

			tw.WriteLine(sLine);

			foreach(System.Web.UI.WebControls.DataGridItem item in DataGrid2Txt.Items)
			{
				if(AddID)
				{
					sLine = "\"" + Convert.ToString(item.ItemIndex + 1) + "\",";
				}
				else
				{
					sLine = "";
				}
				for(int i=0;i<Columns.Length;i++)
				{
					if(item.Cells[Int32.Parse(Columns[i])].Text.Trim().Length > 0)
					{
						sLine += "\"" + item.Cells[Int32.Parse(Columns[i])].Text.Trim().Replace("&nbsp;","");
                        //用于在商品编号后面增加下划线和采购单号
                        if (i == 0)
                        {
                            sLine += "_" + POSysNo.ToString();
                        }
                        sLine += "\",";
					}
					else
					{
						sLine += "\"\",";
					}
				}
				sLine = sLine.Substring(0,sLine.Length-1);

				tw.WriteLine(sLine);
			}

			tw.Flush();
			tw.Close();
			HttpContext.Current.Response.Write(tw.ToString()); //Write the HTML back to the browser.
			HttpContext.Current.Response.End();
		}

        private static bool mailSent = false;
        public static bool SendMail(string MailTo, string MailSubject, string MailBody)
        {
            if (!Icson.Utils.AppConfig.IsSendEMail)
            {
                return false;
            }
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                mail.From = new System.Net.Mail.MailAddress(Icson.Utils.AppConfig.MailFrom,"www.baby1one.com.cn");
                if (MailTo.IndexOf(";") > 0)
                {
                    string[] mMailTo = MailTo.Substring(0,MailTo.Length-1).Split(';');
                    foreach (string sMailTo in mMailTo)
                    {
                        if (!string.IsNullOrEmpty(sMailTo))
                        {
                            mail.To.Add(sMailTo);
                        }
                    }
                }
                else
                {
                    mail.To.Add(MailTo);
                }
                mail.Subject = MailSubject;
                mail.Body = MailBody;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.GetEncoding("GB2312");

                System.Net.Mail.SmtpClient SmtpMail = new System.Net.Mail.SmtpClient(Icson.Utils.AppConfig.MailServer);
                SmtpMail.Credentials = new System.Net.NetworkCredential(Icson.Utils.AppConfig.MailUserName, Icson.Utils.AppConfig.MailUserPassword);

                SmtpMail.SendCompleted +=new System.Net.Mail.SendCompletedEventHandler(SmtpMail_SendCompleted);

                SmtpMail.Send(mail);
                mailSent = true;
            }
            catch 
            {
                mailSent = false;
            }

            return mailSent;
        }

        static void SmtpMail_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
                mailSent = false;
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                mailSent = false;
            }
            else
            {
                Console.WriteLine("Message sent.");
                mailSent = false;
            }

            mailSent = true;
        }

		#region ChineseMoney
		private static string getSmallMoney(string moneyValue)
		{
			int intMoney = Convert.ToInt32(moneyValue);
			if(intMoney == 0)
			{
				return "";
			}
			string strMoney = intMoney.ToString();
			int temp;
			StringBuilder strBuf = new StringBuilder(10);
			if(strMoney.Length == 4)
			{
				temp = Convert.ToInt32(strMoney.Substring(0,1));
				strMoney = strMoney.Substring(1,strMoney.Length - 1);
				strBuf.Append(ChineseNum[temp]);
				if(temp != 0)
					strBuf.Append("仟");
			}
			if(strMoney.Length == 3)
			{
				temp = Convert.ToInt32(strMoney.Substring(0,1));
				strMoney = strMoney.Substring(1,strMoney.Length - 1);
				strBuf.Append(ChineseNum[temp]);
				if(temp != 0)
					strBuf.Append("佰");        
			}
			if(strMoney.Length == 2)
			{
				temp = Convert.ToInt32(strMoney.Substring(0,1));
				strMoney = strMoney.Substring(1,strMoney.Length - 1);
				strBuf.Append(ChineseNum[temp]);
				if(temp != 0)
					strBuf.Append("拾");        
			}
			if(strMoney.Length == 1)
			{
				temp = Convert.ToInt32(strMoney);
				strBuf.Append(ChineseNum[temp]);
			}
			return strBuf.ToString();
		}

		public static string GetChineseMoney(decimal moneyValue)
		{
			if(moneyValue < 0)
			{
				moneyValue *= -1;
			}			
			int intMoney = Convert.ToInt32(Util.TruncMoney(moneyValue)*100); 
			string strMoney = intMoney.ToString();
			int moneyLength = strMoney.Length;
			StringBuilder strBuf = new StringBuilder(100);
			if(moneyLength > 14)
			{
				throw new Exception("Money Value Is Too Large");
			}
            
			//处理亿部分
			if(moneyLength > 10 && moneyLength <= 14)
			{
				strBuf.Append(getSmallMoney(strMoney.Substring(0,strMoney.Length - 10)));
				strMoney = strMoney.Substring(strMoney.Length - 10,10);
				strBuf.Append("亿");
			}
            
			//处理万部分
			if(moneyLength > 6)
			{
				strBuf.Append(getSmallMoney(strMoney.Substring(0,strMoney.Length - 6)));
				strMoney = strMoney.Substring(strMoney.Length - 6,6);
				strBuf.Append("万");
			}

			//处理元部分
			if(moneyLength > 2)
			{
				strBuf.Append(getSmallMoney(strMoney.Substring(0,strMoney.Length - 2)));
				strMoney = strMoney.Substring(strMoney.Length - 2,2);
				strBuf.Append("元");
			}

			//处理角、分处理分
			if(Convert.ToInt32(strMoney) == 0)
			{
				strBuf.Append("整");
			}
			else
			{
				int intJiao = Convert.ToInt32(strMoney.Substring(0,1));
				strBuf.Append(ChineseNum[intJiao]);
				if(intJiao != 0)
				{
					strBuf.Append("角");
				}
				int intFen = Convert.ToInt32(strMoney.Substring(1,1));
				if(intFen != 0)
				{
					strBuf.Append(ChineseNum[intFen]);
					strBuf.Append("分");
				}                
			}
			string temp = strBuf.ToString();
			while(temp.IndexOf("零零") != -1)
			{
				strBuf.Replace("零零","零");
				temp = strBuf.ToString();
			}
            
			strBuf.Replace("零亿", "亿");
			strBuf.Replace("零万", "万");
			strBuf.Replace("亿万", "亿");
			return strBuf.ToString();
		}
		#endregion

        public static string RemoveHtmlTag(string str)
        {
            Regex reg = new Regex(@"<\/*[^<>]*>");
            return reg.Replace(str, "");
        }
        
        /// <summary>
        /// 计算两个日期的时间间隔,返回天数
        /// </summary>
        /// <param name="DateTime1">第一个日期和时间</param>
        /// <param name="DateTime2">第二个日期和时间</param>
        /// <returns></returns>
        public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            DateTime1 = Convert.ToDateTime(DateTime1.ToString("yyyy-MM-dd"));
            DateTime2 = Convert.ToDateTime(DateTime2.ToString("yyyy-MM-dd"));

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            return ts.Days;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param> 
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
            new System.Drawing.Rectangle(x, y, ow, oh),
            System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        //通过nslookup程序查询MX记录，获取域名对应的mail服务器
        private static string GetMailServer(string strEmail)
        {
            string strDomain = strEmail.Split('@')[1];
            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = false;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.FileName = "nslookup";
            info.CreateNoWindow = true;
            info.Arguments = "-type=mx " + strDomain;
            Process ns = Process.Start(info);
            StreamReader sout = ns.StandardOutput;
            Regex reg = new Regex("mail exchanger = (?<mailServer>[^\\s]+)");
            string strResponse = "";
            while ((strResponse = sout.ReadLine()) != null)
            {
                Match amatch = reg.Match(strResponse);
                if (reg.Match(strResponse).Success)
                    return amatch.Groups["mailServer"].Value;
            }
            return null;
        }

        //连接邮件服务器，确认服务器的可用性和用户是否存在
        /// <summary>
        /// return 200 = valid email address
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public static int CheckEmail(string mailAddress)  
        {
            Regex reg = new Regex("^[a-zA-Z0-9_-]+@([a-zA-Z0-9-]+\\.){1,}(com|net|edu|miz|biz|cn|cc)$");

            if (!reg.IsMatch(mailAddress))
                return 405;//Email地址形式上就不对

            string mailServer = GetMailServer(mailAddress);
            if (mailServer == null)
            {
                return 404; //邮件服务器探测错误
            }
            TcpClient tcpc = new TcpClient();
            tcpc.NoDelay = true;
            tcpc.ReceiveTimeout = 3000;
            tcpc.SendTimeout = 3000;
            try
            {
                tcpc.Connect(mailServer, 25);
                NetworkStream s = tcpc.GetStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                StreamWriter sw = new StreamWriter(s, Encoding.Default);
                string strResponse = "";
                string strTestFrom = "service@baby1one.com.cn";
                sw.WriteLine("helo " + mailServer);
                sw.WriteLine("mail from:<" + mailAddress + ">");
                sw.WriteLine("rcpt to:<" + strTestFrom + ">");
                strResponse = sr.ReadLine();
                if (!strResponse.StartsWith("2")) return 403; //用户名有误
                sw.WriteLine("quit");
                return 200; //Email地址检查无误
            }
            catch
            {
                return 403;//发生错误或邮件服务器不可达
            }
        }

        public static string GetWeekName(int id)
        {
            string name = "";
            switch (id)
            {
                case 1:
                    name = "星期一";
                    break;
                case 2:
                    name = "星期二";
                    break;
                case 3:
                    name = "星期三";
                    break;
                case 4:
                    name = "星期四";
                    break;
                case 5:
                    name = "星期五";
                    break;
                case 6:
                    name = "星期六";
                    break;
                case 7:
                    name = "星期日";
                    break;
            }
            return name;
        }

        public static int GetWeekID(DayOfWeek week)
        {
            int id = 0;
            switch (week)
            {
                case DayOfWeek.Monday:
                    id = 1;
                    break;
                case DayOfWeek.Tuesday:
                    id = 2;
                    break;
                case DayOfWeek.Wednesday:
                    id = 3;
                    break;
                case DayOfWeek.Thursday:
                    id = 4;
                    break;
                case DayOfWeek.Friday:
                    id = 5;
                    break;
                case DayOfWeek.Saturday:
                    id = 6;
                    break;
                case DayOfWeek.Sunday:
                    id = 7;
                    break;
            }
            return id;
        }

        /// <summary>
        /// 计算文件的绝对路径, 在类库中调用，一般是不能使用Server.MapPath的时候
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetAbsoluteFilePath(string filePath)
        {
            string file = filePath;
            if (!filePath.Substring(1, 1).Equals(":")
                && !filePath.StartsWith("\\"))
            {
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            }

            return file.Replace("/", "\\");
        }

        /// <summary>
        /// 设置输出显示
        /// </summary>
        /// <param name="lbl">显示信息的lab控件ID</param>
        /// <param name="msg">显示信息</param>
        /// <param name="status">信息类型：小于0为错误信息，大于0为正确信息，-1显示自定义错误信息。-2显示默认错误信息</param>
        public static void Assert(Label lbl, string msg, int status)
        {
            if (status <= 0)
            {
                if (status == -1)
                    lbl.Text = AppConst.ErrorTemplet.Replace("@errorMsg", msg);
                else
                    lbl.Text = AppConst.ErrorTemplet.Replace("@errorMsg", "<br>");// + AppConst.WebMaster);
            }
            else
            {
                lbl.Text = AppConst.SuccTemplet.Replace("@succMsg", msg);
            }
        }

        public static bool Assert(Label lbl, ArrayList errorList)
        {
            if (errorList.Count == 0)
                return true;
            else
            {
                string errorShow = "";
                for (int i = 1; i <= errorList.Count; i++)
                {
                    if (i != 1)
                        errorShow += "<br>";
                    errorShow += i + "." + errorList[i - 1];
                }
                Assert(lbl, errorShow, -1);
                return false;
            }
        }

        /// <summary>
        /// 返回1，说明发送成功
        /// </summary>
        /// <returns></returns>
        public static int SendSMSMessage(string CellNumber,string SMSContent )
        {
            //通过移通网络短信平台发送
            string command = "MT_REQUEST";
            string spid = "0000"; 
            string sppassword = ""; 
            string da = "86" + CellNumber;
            string dc = "15"; //GBK编码
            string sm = "";

            Encoding gb = Encoding.GetEncoding("gbk");
            byte[] bytes = gb.GetBytes(SMSContent);
            for (int i = 0; i < bytes.Length; i++)
            {
                sm += Convert.ToString(bytes[i], 16);
            }

            string url = "http://esms1.etonenet.com/sms/mt?command=" + command + "&spid=" + spid + "&sppassword=" + sppassword + "&da=" + da + "&dc=" + dc + "&sm=" + sm;
            System.Net.WebClient wc = new System.Net.WebClient();
            Stream stream = wc.OpenRead(url);

            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            wc.Dispose();
            if (result.IndexOf("mterrcode=000") > 0)
                return 1;
            else
                return 0;
        }

        public static string FilterCompetitorKeyword(string input)
        {
            return input.Replace("京东", " xx ").Replace("新蛋", " xx ").Replace("某东", " xx ").Replace("某蛋", " xx ").Replace("*东", " xx ").Replace("*蛋", " xx ");
        }

        public decimal Round(decimal x, int len)
        {
            return Decimal.Round(x + 0.000001m, len);
        }

        //显示提示页面
        public static string ShowAlertStr(string paramUrl,string paramBackTitle,string paramTitle,string paramContent,bool paramHaveBtn,string InfoType)
        {
            string _return = "";
            string _strBool = "0";

            if (InfoType == "error")
                paramContent = "<font color=red>" + paramContent + "</font>";

            if (paramHaveBtn) _strBool = "1";
            //_return = "<script language='javascript' type='text/javascript'>parent.document.all.titleFrame.src='SiteMapPath.aspx?url="+paramTitle+"';window.open('../basic/SaveOK.aspx?Url=" + paramUrl.Trim() + "&Content=" + paramContent.Trim() + "&HaveBtn=" + _strBool + "&BackTitle=" + paramBackTitle + "&time=" + DateTime.Now.ToString() + "','_self')</script>";
            _return = "<script language='javascript' type='text/javascript'>window.open('../basic/SaveOK.aspx?Url=" + paramUrl.Trim() + "&Content=" + paramContent.Trim() + "&HaveBtn=" + _strBool + "&BackTitle=" + paramBackTitle + "&time=" + DateTime.Now.ToString() + "','_self')</script>";
            return _return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFullUrl(string url)
        {
            string fullurl = AppConfig.DomainName + url;
            return fullurl;
        }

        public static int DecimalToU_Int32(decimal argument)
        {
            object Int32Value;
            object UInt32Value;

            // Convert the argument to an int value.
            try
            {
                Int32Value = (int)argument;
            }
            catch (Exception ex)
            {
                Int32Value = GetExceptionType(ex);
            }

            // Convert the argument to a uint value.
            try
            {
                UInt32Value = (uint)argument;
            }
            catch (Exception ex)
            {
                UInt32Value = GetExceptionType(ex);
            }

            return Int32.Parse(UInt32Value.ToString());
        }

        private static string GetExceptionType(Exception ex)
        {
            string exceptionType = ex.GetType().ToString();
            return exceptionType.Substring(exceptionType.LastIndexOf('.') + 1);
        }

        public static string TrimString(string s, int len)
        {
            string _s;
            if (s.Length > len)
            {
                _s = s.Substring(0, len - 1) + "...";
            }
            else
            {
                _s = s;
            }

            return _s;
        }

        public static string FormatParagraph(string paragraphs)
        {
            string _paragraph = AppConst.StringNull;
            string[] _paragraphs = paragraphs.Split('\n');
            for (int i = 0; i < _paragraphs.Length; i++)
            {
                _paragraph += "<p>" + _paragraphs[i] + "</p>";
            }

            return _paragraph;
        }

	}
}