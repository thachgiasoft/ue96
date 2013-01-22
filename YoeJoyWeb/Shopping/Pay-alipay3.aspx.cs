using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using YoeJoyHelper.Security;

using Icson.Objects.Basic;
using Icson.BLL.Basic;
using Icson.Objects.Sale;
using Icson.BLL.Sale;
using Icson.Objects.Finance;
using Icson.BLL.Finance;
using Com.Alipay;
using YoeJoyHelper;

namespace YoeJoyWeb.Shopping
{
    public partial class Pay_alipay3 : SecurityPageBase
    {

        SOInfo soInfo = new SOInfo();

        protected string soSysNo
        {
            get
            {
                return Request.QueryString["id"].ToString().Trim();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            //首先判断是否需要重新登陆
            //直接调用基类中的函数进行判断			
            //Login(Request.Url.ToString());
            base.CheckProfile(Context);
            this.Master.Page.Title = "支付宝支付";

            if (!IsPostBack)
            {
                int sosysno = -1;
                try
                {
                    sosysno = Convert.ToInt32(Request.QueryString["ID"].ToString());
                    lblSOSysNo.Text = sosysno.ToString();
                }
                catch
                {
                    //ShowError("获取so#出错");
                }

                // 在此处放置用户代码以初始化页面
                string soid = Request.QueryString["sono"];

                if (soid != null)
                {
                    lblSOID.Text = soid;
                    lblView.Text = "<a href='OrderDetail.aspx?ID=" + sosysno.ToString() + "'>查看订单</a>";
                    lblSOID_1.Text = soid;
                    //lblSOAmt.Text = Request.QueryString["soamt"];


                    soInfo = SaleManager.GetInstance().LoadSO(sosysno);
                    lblShipType.Text = GetShipType();
                    lblPayType.Text = GetPayType();
                    //this.CashPay+this.PayPrice+this.ShipPrice+this.PremiumAmt-this.DiscountAmt
                    //lblSOAmt.Text = Convert.ToDecimal(soInfo.CashPay + soInfo.ShipPrice + soInfo.PremiumAmt - soInfo.DiscountAmt).ToString("f2");
                    lblSOAmt.Text = Convert.ToDecimal(soInfo.GetTotalAmt()).ToString("f2");
                    lblSODate.Text = soInfo.OrderDate.ToString(Icson.Utils.AppConst.DateFormatLong);
                }
            }
        }

        /// <summary>
        /// 查询网上支付情况
        /// </summary>
        /// <param name="sosysno">订单编号</param>
        /// <returns>true: 已通过网上支付 / false: 未通过网上支付</returns>
        protected bool IsPayed(int sosysno)
        {
            //查询网上支付情况
            NetPayInfo netpay = NetPayManager.GetInstance().Load(sosysno);
            if (netpay == null)
                return false;
            else
                return true;
        }

        protected void btnPayNow_Click(object sender, EventArgs e)
        {
            

            //假支付环节
            //调试支付时，请去掉一下两行代码的注释
            //string resultURL = "payresultfromAlipay4.aspx?id=" + soSysNo + "&total_fee=5745.60";

            //Response.Redirect(resultURL);

            //业务参数赋值；
            string out_trade_no = lblSOSysNo.Text.Trim();
            string subject = lblSOID.Text;	//subject		商品名称
            string body = lblSOID.Text; //Request.QueryString["miaos"];T_body.Text;		//body			商品描述    
            string payment_type = "1";                  //支付类型	
            string total_fee = lblSOAmt.Text.Trim(); // Request.QueryString["prices"]; T_total_fee.Text;                      //总金额					0.01～50000.00
            string show_url = YoeJoyConfig.SiteBaseURL;
            string notify_url = String.Concat(YoeJoyConfig.SiteBaseURL, YoeJoyConfig.AlipayNotifyURL);
            string return_url = String.Concat(YoeJoyConfig.SiteBaseURL, YoeJoyConfig.AlipayReturnURL);
            string seller_email = YoeJoyConfig.AlipaySellerEmailAddress;
            //string anti_phishing_key = Submit.Query_timestamp();
            string anti_phishing_key = String.Empty;
            string exter_invoke_ip = String.Empty;

            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("seller_email", seller_email);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("anti_phishing_key", anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
        }

        public static string GetMD5(string s)
        {   /// <summary>
            /// 与ASP兼容的MD5加密算法
            /// </summary>

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        public static string[] BubbleSort(string[] r)
        {
            /// <summary>
            /// 冒泡排序法
            /// </summary>

            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Length - 2; j >= i; j--)
                {
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)　//交换条件
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }

            }
            return r;
        }

        public string CreatUrl(
            string gateway,
            string service,
            string partner,
            string sign_type,
            string out_trade_no,
            string subject,
            string body,
            string payment_type,
            string total_fee,
            string show_url,
            string seller_email,
            string key,
            string return_url,
            string notify_url
            )
        {
            /// <summary>
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
            /// </summary>
            int i;

            //构造数组；
            string[] Oristr ={ 
                "service="+service, 
                "partner=" + partner, 
                "subject=" + subject, 
                "body=" + body, 
                "out_trade_no=" + out_trade_no, 
                "total_fee=" + total_fee, 
                "show_url=" + show_url,  
                "payment_type=" + payment_type, 
                "seller_email=" + seller_email, 
                "notify_url=" + notify_url,
                "return_url=" + return_url 
            };

            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);


            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);

                }
                else
                {

                    prestr.Append(Sortedstr[i] + "&");
                }

            }

            prestr.Append(key);

            //生成Md5摘要；
            string sign = GetMD5(prestr.ToString());

            //构造支付Url；
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {
                parameter.Append(Sortedstr[i] + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);

            //返回支付Url；
            return parameter.ToString();
        }

        protected string GetPayType()
        {
            PayTypeInfo payType = (PayTypeInfo)ASPManager.GetInstance().GetPayTypeHash()[soInfo.PayTypeSysNo];
            return payType.PayTypeName;
        }

        protected string GetShipType()
        {
            ShipTypeInfo shipType = (ShipTypeInfo)ASPManager.GetInstance().GetShipTypeHash()[soInfo.ShipTypeSysNo];
            return shipType.ShipTypeName;
        }

    }
}