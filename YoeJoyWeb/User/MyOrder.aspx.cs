using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Icson.Objects.Basic;
using Icson.Objects.Online;
using Icson.Objects;
using Icson.Objects.Finance;
using Icson.Utils;
using Icson.BLL;
using Icson.BLL.Online;
using Icson.BLL.Sale;
using Icson.BLL.Finance;
using Icson.BLL.RMA;
using System.Text;
using YoeJoyHelper;
using System.Data;
using YoeJoyHelper.Security;

namespace YoeJoyWeb.User
{
    public partial class MyOrder : SecurityPageBase
    {
        private PagedDataSource pds = new PagedDataSource();
        IcsonSessionInfo oS;

        protected void Page_Load(object sender, EventArgs e)
        {

            base.CheckProfile(Context);

            pds.PageSize = 8;
            pds.AllowPaging = true;
            oS = CommonUtility.GetUserSession(Context);

            if (!IsPostBack)
            {
                DataList();
            }

        }

        private void DataList()
        {
            DataSet ds = new DataSet();

            ds = ConvertDs(SaleManager.GetInstance().GetSOOnlineDs(oS.sCustomer.SysNo));

            pds.DataSource = ds.Tables[0].DefaultView;

            DataList1.DataSource = pds;
            DataList1.DataBind();

            DBCSoft.Component.NavigatorBarUtil.GetInstance().setNavigatorBar(NavigatorBar1, pds, ds.Tables[0].Rows.Count);
        }

        private DataSet ConvertDs(DataSet ds)
        {
            ds.Tables[0].Columns.Add("opt");
            ds.Tables[0].Columns.Add("statusName");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int soSysNo = Util.TrimIntNull(dr["sysno"]);
                string soID = Util.TrimNull(dr["soid"]);
                dr["soid"] = Util.TrimNull(dr["soid"]);
                dr["OrderDate"] = Util.TrimDateNull(dr["OrderDate"]).ToString();

                decimal totalCash = Util.TrimDecimalNull(dr["totalCash"]);
                if (Util.TrimIntNull(dr["IsPayWhenRecv"]) == (int)AppEnum.YNStatus.Yes)
                    totalCash = Util.TruncMoney(totalCash);
                dr["totalCash"] = totalCash.ToString("#####0.00");


                dr["PointPay"] = Util.TrimIntNull(dr["PointPay"]);
                dr["PointAmt"] = Util.TrimIntNull(dr["PointAmt"]);
                dr["doNo"] = Util.TrimNull(dr["doNo"]);
                dr["statusName"] = AppEnum.GetSOStatus(Util.TrimIntNull(dr["status"]));

                int soStatus = Util.TrimIntNull(dr["Status"]);
                bool isPay = NetPayManager.GetInstance().IsPayed(soSysNo);

                if (soStatus == (int)AppEnum.SOStatus.Origin && !isPay)
                    dr["opt"] = "<a href='MyOrderDetail.aspx?action=cancel&ID=" + soSysNo.ToString() + "'>作废订单</a>&nbsp;&nbsp;";
                dr["opt"] += "<a href='MyOrderDetail.aspx?action=update&ID=" + soSysNo.ToString() + "'>修改订单</a>&nbsp;&nbsp;";
                if (soStatus == (int)AppEnum.SOStatus.Origin || soStatus == (int)AppEnum.SOStatus.WaitingPay || soStatus == (int)AppEnum.SOStatus.WaitingManagerAudit)
                {
                    if (Util.TrimNull(dr["paymentpage"]) != AppConst.StringNull && !isPay && Util.TrimIntNull(dr["IsNet"]) == (int)AppEnum.YNStatus.Yes)
                        dr["opt"] += "<a href='../Shopping/" + Util.TrimNull(dr["paymentpage"]) + "?id=" + soSysNo.ToString() + "&sono=" + soID + "&soamt=" + totalCash.ToString("#####0.00") + "'>支付货款</a>&nbsp;&nbsp;";
                }

                DataTable dt = RMARequestManager.GetInstance().GetRMABySO(soSysNo);
                if (dt != null)
                {
                    dr["opt"] += "<a href='../Account/RMAQuery.aspx?Type=single&ID=" + soSysNo.ToString() + "'>查看返修信息</a>&nbsp;&nbsp;";

                }
                dr["opt"] += "<a href='MyOrderDetail.aspx?ID=" + soSysNo.ToString()+"'>查看订单明细</a>";
                if (Util.TrimNull(dr["Memo"]) == "")
                {
                    dr["Memo"] = Util.TrimNull(dr["Memo"]);
                }
                else
                {
                    dr["Memo"] = "<tr><td height=25px align=right bgcolor=#E7F9F9>备注信息：</td><td colspan=5 bgcolor=#ffffff>" + Util.TrimNull(dr["Memo"]) + "</td></tr>";
                }
            }

            return ds;
        }

        protected void NavigatorBar1_PageIndexChanged(object sender, DBCSoft.Component.NavigatorBar.PageChangedEventArgs e)
        {
            pds.CurrentPageIndex = e.NewPageIndex;
            DataList();
        }
    }
}