using System;

namespace Icson.Utils
{
	/// <summary>
	/// Summary description for AppConst.
    /// 应用程序常量
	/// </summary>
	public class AppConst
	{

		public AppConst()
		{
		}

		#region 系统中判断未赋值的判断，只可以用于比较判断，不能用于赋值

        /// <summary>
        /// StringNull=""
        /// </summary>
		public const string StringNull="";

        /// <summary>
        /// IntNull=-999999
        /// </summary>
		public const int IntNull=-999999;	
		public const decimal DecimalNull=-999999;

		public static DateTime DateTimeNull = DateTime.Parse("1900-01-01");

		#endregion
		public const string AllSelectString="--- 全部 ---";
        public const string AllSelectString_DBC = "--- 全部 ---";
        public const string NotSetString_DBC = "--- 未设置 ---";
		public const string DecimalToInt = "#########0"; //用于point的显示，一般来说currentprice应该没有分。
		public const string DecimalFormat = "#########0.00";
        public const string DecimalFormatLong = "#########0.000";
		public const string DecimalFormatWithGroup = "#,###,###,##0.00";
		public const string DecimalFormatWithCurrency = "￥#########0.00";

        /// <summary>
        /// DateFormat = "yyyy-MM-dd"
        /// </summary>
		public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// DateFormatLong = "yyyy-MM-dd HH:mm:ss"
        /// </summary>
		public const string DateFormatLong = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// IntFormatWithGroup = "#,###,###,##0"
        /// </summary>
        public const string IntFormatWithGroup = "#,###,###,##0";
        

        #region 添加RMA货卡查询起始时间
        public static DateTime RMA_Initializtion_DateTime = DateTime.Parse("2008-01-01");
        #endregion

		//用于DataGrid中每页的记录数
		public const int PageSize = 50;

		//用于DataGrid中每页按钮的数目
		public const int PageButtonCount = 5;

		/// <summary>
		/// 积分和现金RMB转换比例 Point = Cash*ExchangeRate
		/// </summary>
		public static decimal ExchangeRate = 10m;

		// 每张发票的最大行数
		public const int PageMaxLine = 12;

		// 每行名称的最大长度
		public const int NameMaxLength = 30;

		// 地址的最大长度，超过长都会被截去一部分
		public const int AddressMaxLength = 100;

		// 商品价格的缺省值
		public const decimal DefaultPrice = 999999m;

		// 前台生成订单对应的销售人员编号
		public const int IcsonSalesMan = 0;
		public const string IcsonSalesManName = "ORS商城网";

		// 系统操作Log，默认ip地址
		public const string SysIP = "127.0.0.1";

		// 系统操作Log，默认User
		public const int SysUser = 0;

        public const int PMDUserSysNo = 47;

        public const string ErrorTemplet = @"
          <table width='100%' border='0' cellpadding='1' cellspacing='1'>
            <tr>
                <td bgcolor='#AA0605'>
                    <table width='100%' border='0' cellspacing='0' cellpadding='4' bgcolor='#FFFFD5'>
                        <tr>
                            <td width='17' bgcolor='#FFFFD5'>
                                <img src='../images/error.gif' width='17'
                                    height='17' /></td>
                            <td align='left' bgcolor='#FFFFD5'>
                                <span class='font-error'>错误信息</span></td>
                        </tr>
                        <tr>
                            <td bgcolor='#FFFFD5'>
                                &nbsp;</td>
                            <td align='left' bgcolor='#FFFFD5' class='ays_font_error_2'>
                                @errorMsg</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>";

        public const string SuccTemplet = @"
          <table bgcolor='#ffffd5' border='0' cellpadding='2' cellspacing='0' width='100%'>
            <tr>
                <td bgcolor='#ffffd5' width='17'>
                    &nbsp;</td>
                <td>
                    <span class='ays_font_success'>正确信息</span></td>
            </tr>
            <tr>
                <td bgcolor='#ffffd5'>
                    &nbsp;</td>
                <td class='ays_font_success2' bgcolor='#ffffd5'>
                    @succMsg</td>
            </tr>
        </table>";
	}
}
