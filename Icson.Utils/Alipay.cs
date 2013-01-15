using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Icson.Utils
{
    public class Alipay
    {
        public static string GetMD5(string s, string _input_charset)
        {
            /// <summary>
            /// 与ASP兼容的MD5加密算法
            /// </summary>
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
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
            /// 
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
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)//交换条件
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

        public static string CreatUrl(
            string service,
            string out_trade_no
            )
        {
            /// <summary>
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
            /// </summary>

            //------------------------------------------支付宝新接口-------------------------------------------
            //string out_trade_no = sosysno.ToString();
            //业务参数赋值；
            string gateway = "https://www.alipay.com/cooperate/gateway.do?";	//'支付接口
            //string service = "trade_create_by_buyer";
            string partner = "2088002012634570";		//partner		合作伙伴ID			保留字段
            string sign_type = "MD5";
            //string subject = soInfo.SOID;	//subject		商品名称
            //string body ="";		//body			商品描述    
            //string payment_type = "1";                  //支付类型	
            //string price = Convert.ToDecimal(soInfo.GetTotalAmt() - soInfo.ShipPrice).ToString("f2");
            //string price = Convert.ToDecimal(soInfo.GetTotalAmt()).ToString("f2");
            //string quantity = "1";
            //string show_url = "www.baby1one.com.cn";
            string seller_email = "stonebu@yahoo.com.cn";             //卖家账号
            string key = "";              //partner账户的支付宝安全校验码
            //string return_url = ""; //服务器通知返回接口
            //string notify_url = "http://www.baby1one.com.cn/shopping/Alipay_Notify.aspx"; //服务器通知返回接口
            string _input_charset = "utf-8";
            //string logistics_type = "EXPRESS";//"POST";
            //string logistics_fee = soInfo.ShipPrice.ToString("f2");
            //string logistics_payment = "BUYER_PAY";
            
            int i;

            //构造数组；
            string[] Oristr ={ 
                "service="+service, 
                "partner=" + partner, 
                "out_trade_no=" + out_trade_no, 
                "seller_email=" + seller_email, 
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
            string sign = GetMD5(prestr.ToString(), _input_charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {
                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);


            //返回支付Url；
            return parameter.ToString();
        }
    }
}
