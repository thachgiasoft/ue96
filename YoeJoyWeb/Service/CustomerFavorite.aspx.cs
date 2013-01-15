using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Icson.Utils;
using Icson.Objects.Online;
using Icson.Objects.Basic;
using Icson.BLL.Online;
using YoeJoyHelper;

namespace YoeJoyWeb
{
    /// <summary>
    /// 用户收藏
    /// </summary>
    public partial class CustomerFavorite : System.Web.UI.Page
    {
        /// <summary>
        /// 收藏的服务指令
        /// </summary>
        protected string Cmd
        {
            get
            {
                if (Request.QueryString["cmd"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    string command = Request.QueryString["cmd"].ToString().Trim().ToLower(CultureInfo.InvariantCulture);
                    return command;
                }
            }
        }

        protected int productSysNo
        {
            get
            {
                if (Request.QueryString["pid"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["pid"].ToString().Trim());
                }
            }
        }

        protected string[] productSysNos
        {
            get
            {
                if (Request.QueryString["pids"] == null)
                {
                    return null;
                }
                else
                {
                    return Request.QueryString["pids"].ToString().Split(',');
                }
            }
        }

        protected int StartIndex
        {
            get
            {
                if (Request.QueryString["startIndex"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["startIndex"].ToString().Trim());
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            IcsonSessionInfo oSessionInfo = CommonUtility.GetUserSession(Context);

            CustomerInfo cInfo = oSessionInfo.sCustomer;
            int cSysNo = cInfo.SysNo;

            bool result = false;
            string msg = String.Empty;

            try
            {
                switch (Cmd)
                {
                    //添加商品到收藏
                    case "add":
                        {
                            WishListInfo wInfo = new WishListInfo();
                            wInfo.CustomerSysNo = cSysNo;
                            wInfo.ProductSysNo = productSysNo;
                            WishListManager.GetInstance().Insert(wInfo);
                            result = true;
                            msg = "成功添加";
                            Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
                            break;
                        }
                    //删除单个收藏商品
                    case "delete":
                        {
                            foreach (string pid in productSysNos)
                            {
                                WishListManager.GetInstance().Delete(cSysNo, int.Parse(pid));
                            }
                            msg = "删除成功";
                            Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
                            break;
                        }
                    //清空收藏商品
                    case "empty":
                        {
                            WishListManager.GetInstance().Clear(cSysNo);
                            msg = "清空收藏夹成功";
                            Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
                            break;
                        }
                    //获得分页的收藏列表
                    default:
                        {
                            Response.Write(CustomerHelper.GetCustomerWishListProducts(cSysNo, StartIndex));
                            break;
                        }
                }
            }
            catch
            {
                msg = "用户请求的操作失败";
                Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
            }
        }
    }
}