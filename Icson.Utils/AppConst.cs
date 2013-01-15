using System;

namespace Icson.Utils
{
	/// <summary>
	/// Summary description for AppConst.
    /// Ӧ�ó�����
	/// </summary>
	public class AppConst
	{

		public AppConst()
		{
		}

		#region ϵͳ���ж�δ��ֵ���жϣ�ֻ�������ڱȽ��жϣ��������ڸ�ֵ

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
		public const string AllSelectString="--- ȫ�� ---";
        public const string AllSelectString_DBC = "--- ȫ�� ---";
        public const string NotSetString_DBC = "--- δ���� ---";
		public const string DecimalToInt = "#########0"; //����point����ʾ��һ����˵currentpriceӦ��û�з֡�
		public const string DecimalFormat = "#########0.00";
        public const string DecimalFormatLong = "#########0.000";
		public const string DecimalFormatWithGroup = "#,###,###,##0.00";
		public const string DecimalFormatWithCurrency = "��#########0.00";

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
        

        #region ���RMA������ѯ��ʼʱ��
        public static DateTime RMA_Initializtion_DateTime = DateTime.Parse("2008-01-01");
        #endregion

		//����DataGrid��ÿҳ�ļ�¼��
		public const int PageSize = 50;

		//����DataGrid��ÿҳ��ť����Ŀ
		public const int PageButtonCount = 5;

		/// <summary>
		/// ���ֺ��ֽ�RMBת������ Point = Cash*ExchangeRate
		/// </summary>
		public static decimal ExchangeRate = 10m;

		// ÿ�ŷ�Ʊ���������
		public const int PageMaxLine = 12;

		// ÿ�����Ƶ���󳤶�
		public const int NameMaxLength = 30;

		// ��ַ����󳤶ȣ����������ᱻ��ȥһ����
		public const int AddressMaxLength = 100;

		// ��Ʒ�۸��ȱʡֵ
		public const decimal DefaultPrice = 999999m;

		// ǰ̨���ɶ�����Ӧ��������Ա���
		public const int IcsonSalesMan = 0;
		public const string IcsonSalesManName = "ORS�̳���";

		// ϵͳ����Log��Ĭ��ip��ַ
		public const string SysIP = "127.0.0.1";

		// ϵͳ����Log��Ĭ��User
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
                                <span class='font-error'>������Ϣ</span></td>
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
                    <span class='ays_font_success'>��ȷ��Ϣ</span></td>
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
