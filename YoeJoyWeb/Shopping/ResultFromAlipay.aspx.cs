using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using Com.Alipay;

using Icson.Utils;
using Icson.BLL;
using Icson.Objects.Sale;
using Icson.BLL.Sale;
using Icson.Objects;
using Icson.Objects.Finance;
using Icson.BLL.Finance;
using YoeJoyHelper;

namespace YoeJoyWeb.Shopping
{
    public partial class ResultFromAlipay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                //TODO:
                //上线需要verifyResult==true
                if (!verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号

                    string out_trade_no = Request.Form["out_trade_no"];

                    //支付宝交易号

                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    string trade_status = Request.Form["trade_status"];


                    if (Request.Form["trade_status"] == "TRADE_FINISHED")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //该种交易状态只在两种情况下出现
                        //1、开通了普通即时到账，买家付款成功后。
                        //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
                    }
                    else if (Request.Form["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
                    }
                    else
                    {
                    }

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                    try
                    {
                        //更新自己数据库的订单语句，请自己填写一下
                        string SOsysno = Request.Form["out_trade_no"];
                        int sosysno = Int32.Parse(SOsysno);

                        if (NetPayManager.GetInstance().IsPayedForAlipay(sosysno))
                        {
                            Response.Write("fail");
                            return;
                        }

                        SOInfo order = SaleManager.GetInstance().LoadSOMaster(sosysno);

                        if (order != null)
                        {
                            NetPayInfo netpay = new NetPayInfo();

                            netpay.SOSysNo = Int32.Parse(SOsysno);
                            netpay.PayTypeSysNo = order.PayTypeSysNo;
                            netpay.InputTime = DateTime.Now;
                            netpay.Source = (int)AppEnum.NetPaySource.Bank;
                            netpay.PayAmount = decimal.Parse(Request.Form["total_fee"]);
                            netpay.Status = (int)AppEnum.NetPayStatus.Origin;
                            //netpay.Note = Request.QueryString["msg"];
                            NetPayManager.GetInstance().Insert(netpay);

                            if (netpay.PayAmount >= order.GetTotalAmt())
                            {
                                //客人通过网上支付后,自动修改订单状态为已付款,默认为ias审核,userSysNo=33
                                NetPayManager.GetInstance().Verify(netpay.SysNo, 33);
                                Response.Write("success");
                            }
                            else
                            {
                                Response.Write("success");
                            }
                        }
                        else
                        {
                            Response.Write("fail");
                        }
                    }
                    catch
                    {
                        Response.Write("fail");
                    }
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }

        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

    }
}