using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.Sql;
using YoeJoyHelper;
using YoeJoyHelper.Security;

using Icson.Utils;
using Icson.Objects.Online;
using Icson.BLL;
using Icson.BLL.Basic;
using Icson.BLL.Online;
using Icson.Objects;
using Icson.Objects.Sale;
using Icson.Objects.Basic;
using Icson.BLL.Sale;

namespace YoeJoyWeb.Shopping
{
    public partial class ShoppingCart : SecurityPageBase
    {
        protected IcsonSessionInfo oSession;
        protected SOInfo soInfo = new SOInfo();

        protected string ShoppingCartHTML { get; set; }

        protected Hashtable newHt = new Hashtable();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            base.CheckProfile(Context);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            oSession = CommonUtility.GetUserSession(Context);
            Hashtable ht = CartManager.GetInstance().GetCartHash();
            ShoppingCartHTML = CustomerHelper.GetCustomerShoppingCart(ht);

            //private void GoShopping()
            //{
            //    DataSet ds = new DataSet();
            //    SOInfo oldSoInfo = null;
            //    try
            //    {
            //        ds = SaleManager.GetInstance().GetSOOnlineDs(oSession.sCustomer.SysNo, 1);
            //    }
            //    catch
            //    {
            //        checkGo = true;
            //        Response.Redirect("../User/login.aspx?url=../Shopping/ShoppingCart.aspx");
            //    }
            //    //-------------------------库存判断--------------------------//

            //    //newHt = CartManager.GetInstance().GetCartHash();
            //    //DataSet ds2 = OnlineListManager.GetInstance().GetCartDs(newHt);
            //    //foreach (DataRow dr in ds2.Tables[0].Rows)
            //    //{
            //    //    int que = Int32.Parse(dr["OnlineQty"].ToString());

            //    //    if (que <= 0)
            //    //    {
            //    //        ShowMessage("您要购买的" + dr["productName"] + "已无库存！");
            //    //        return;
            //    //    }


            //    //}
            //    //------------------------------------------------------------//


            //    if (Util.HasMoreRow(ds))
            //    {
            //        if (ds.Tables[0].Rows.Count == 1)
            //        {
            //            DataRow dr = ds.Tables[0].Rows[0];

            //            int soSysNo = int.Parse(dr["sysno"].ToString());

            //            oldSoInfo = SaleManager.GetInstance().LoadSOMaster(soSysNo);
            //        }
            //    }


            //    if (oldSoInfo != null)
            //    {


            //        oSession.sSO = new SOInfo();
            //        oSession.sSO.CustomerSysNo = oSession.sCustomer.SysNo;
            //        oSession.sSO.ReceiveAddress = oldSoInfo.ReceiveAddress;
            //        oSession.sSO.ReceiveAreaSysNo = oldSoInfo.ReceiveAreaSysNo;
            //        oSession.sSO.ReceiveCellPhone = oldSoInfo.ReceiveCellPhone;
            //        oSession.sSO.ReceiveContact = oldSoInfo.ReceiveContact;
            //        oSession.sSO.ReceiveName = oldSoInfo.ReceiveName;
            //        oSession.sSO.ReceivePhone = oldSoInfo.ReceivePhone;
            //        oSession.sSO.ReceiveZip = oldSoInfo.ReceiveZip;

            //        oSession.sSO.IsPremium = (int)AppEnum.YNStatus.Yes;
            //        oSession.sSO.IsVAT = (int)AppEnum.YNStatus.No;
            //        oSession.sSO.IsWholeSale = (int)AppEnum.YNStatus.No;
            //        oSession.sSO.VATEMSFee = 0;
            //        oSession.sSO.VatInfo.VATEMSFee = 0;
            //        oSession.sSO.FreeShipFeePay = 0;
            //        oSession.sSO.PayTypeSysNo = oldSoInfo.PayTypeSysNo;
            //        oSession.sSO.ShipTypeSysNo = oldSoInfo.ShipTypeSysNo;
            //        Response.Redirect("CheckOut.aspx");
            //    }

            //    else
            //        Response.Redirect("precheckout.aspx");
            //}
        }
    }
}