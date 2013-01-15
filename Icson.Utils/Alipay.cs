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
            /// ��ASP���ݵ�MD5�����㷨
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
            /// ð������
            /// </summary>

            int i, j; //������־ 
            string temp;

            bool exchange;

            for (i = 0; i < r.Length; i++) //�����R.Length-1������ 
            {
                exchange = false; //��������ʼǰ��������־ӦΪ��

                for (j = r.Length - 2; j >= i; j--)
                {
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)//��������
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //�����˽������ʽ�������־��Ϊ�� 
                    }
                }

                if (!exchange) //��������δ������������ǰ��ֹ�㷨 
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
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com��
            /// </summary>

            //------------------------------------------֧�����½ӿ�-------------------------------------------
            //string out_trade_no = sosysno.ToString();
            //ҵ�������ֵ��
            string gateway = "https://www.alipay.com/cooperate/gateway.do?";	//'֧���ӿ�
            //string service = "trade_create_by_buyer";
            string partner = "2088002012634570";		//partner		�������ID			�����ֶ�
            string sign_type = "MD5";
            //string subject = soInfo.SOID;	//subject		��Ʒ����
            //string body ="";		//body			��Ʒ����    
            //string payment_type = "1";                  //֧������	
            //string price = Convert.ToDecimal(soInfo.GetTotalAmt() - soInfo.ShipPrice).ToString("f2");
            //string price = Convert.ToDecimal(soInfo.GetTotalAmt()).ToString("f2");
            //string quantity = "1";
            //string show_url = "www.baby1one.com.cn";
            string seller_email = "stonebu@yahoo.com.cn";             //�����˺�
            string key = "";              //partner�˻���֧������ȫУ����
            //string return_url = ""; //������֪ͨ���ؽӿ�
            //string notify_url = "http://www.baby1one.com.cn/shopping/Alipay_Notify.aspx"; //������֪ͨ���ؽӿ�
            string _input_charset = "utf-8";
            //string logistics_type = "EXPRESS";//"POST";
            //string logistics_fee = soInfo.ShipPrice.ToString("f2");
            //string logistics_payment = "BUYER_PAY";
            
            int i;

            //�������飻
            string[] Oristr ={ 
                "service="+service, 
                "partner=" + partner, 
                "out_trade_no=" + out_trade_no, 
                "seller_email=" + seller_email, 
                };

            //��������
            string[] Sortedstr = BubbleSort(Oristr);


            //�����md5ժҪ�ַ��� ��

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

            //����Md5ժҪ��
            string sign = GetMD5(prestr.ToString(), _input_charset);

            //����֧��Url��
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {
                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);


            //����֧��Url��
            return parameter.ToString();
        }
    }
}
