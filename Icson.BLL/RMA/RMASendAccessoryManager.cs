using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.RMA;
using Icson.Objects.Sale;
using Icson.Objects.Basic;

using Icson.DBAccess;
using Icson.DBAccess.RMA;

using Icson.BLL.Basic;
using Icson.BLL.Sale;
namespace Icson.BLL.RMA
{
   public class RMASendAccessoryManager
    {
        private RMASendAccessoryManager()
        { 
        }
        private static RMASendAccessoryManager _instance;
        public static RMASendAccessoryManager GetInstance()
        {
            if (_instance == null)
            {

                _instance = new RMASendAccessoryManager();
            }
            return _instance;
        }

       public RMASendAccessoryInfo Load(int sysno)
       {
           try
           {
               RMASendAccessoryInfo oInfo = new RMASendAccessoryInfo();
               string sql = "select * from RMA_SendAccessory (NOLOCK) where sysno=" + sysno;
               DataSet ds = SqlHelper.ExecuteDataSet(sql);
               if (Util.HasMoreRow(ds))
               {
                   DataRow dr = ds.Tables[0].Rows[0];
                    map(oInfo,dr);
                   string itemSql = @"select RMA_SendAccessory_Item.* 
                                       from   RMA_SendAccessory_Item (NOLOCK)
                                       inner join  RMA_SendAccessory (NOLOCK) on RMA_SendAccessory_Item.SendAccessorySysNo = RMA_SendAccessory.SysNo 
                                       where RMA_SendAccessory.sysno=" + sysno;
                   DataSet itemds = SqlHelper.ExecuteDataSet(itemSql);
                   if (Util.HasMoreRow(itemds))
                   {
                       foreach (DataRow itemdr in itemds.Tables[0].Rows)
                       {
                           RMASendAccessoryItemInfo oSAItem = new RMASendAccessoryItemInfo();
                           mapItem(oSAItem, itemdr);
                           oInfo.ItemHash.Add(Util.TrimIntNull(itemdr["SysNo"]), oSAItem);
                       }
                   }
               }
               return oInfo;
           }
           catch
           {
               throw new BizException("Load RMASendAccessoryInfo Error!");
           }

       }

       public RMASendAccessoryItemInfo LoadItem(int sysno)
       {
           try
           {
               RMASendAccessoryItemInfo oInfo = new RMASendAccessoryItemInfo();
               string sql = "select * from RMA_SendAccessory_Item (NOLOCK) where sysno=" + sysno;
               DataSet ds = SqlHelper.ExecuteDataSet(sql);
               if (Util.HasMoreRow(ds))
               {
                   DataRow dr = ds.Tables[0].Rows[0];
                   mapItem(oInfo, dr);
               }
               return oInfo;
           }
           catch
           {
               throw new BizException("Load RMASendAccessoryItemInfo Error!");
           }
       }

       public RMASendAccessoryItemInfo LoadItem(int productSysNo, int saSysNo)
       {
           try
           {
               RMASendAccessoryItemInfo oInfo = new RMASendAccessoryItemInfo();
               string sql = "select * from RMA_SendAccessory_Item (NOLOCK) where productSysNo=" + productSysNo + "and SendAccessorySysNo=" + saSysNo;
               DataSet ds = SqlHelper.ExecuteDataSet(sql);
               if (Util.HasMoreRow(ds))
               {
                   DataRow dr = ds.Tables[0].Rows[0];
                   mapItem(oInfo, dr);
               }
               return oInfo;
           }
           catch
           {
               throw new BizException("Load RMASendAccessoryItemInfo Error!");
           }
       }

       public void Update(RMASendAccessoryInfo oParam)
       {
           new RMASendAccessoryDac().Update(oParam);
       }
       public void UpdateItem(RMASendAccessoryItemInfo oParam)
       {
           new RMASendAccessoryDac().UpdateItem(oParam);
       }

       public void Update(Hashtable paramHash)
       {
           new RMASendAccessoryDac().Update(paramHash);
       }

       public int GetCurrentStatus(int sysno)
       {
           string sql = "select status from RMA_SendAccessory (NOLOCK) where sysno = " + sysno;
           DataSet ds = SqlHelper.ExecuteDataSet(sql);
           if (!Util.HasMoreRow(ds))
           {
               throw new BizException("Get Current Status Error!");
           }
           DataRow dr = ds.Tables[0].Rows[0];
           return Util.TrimIntNull(dr["status"]);
       }

       public void AddAccessory(RMASendAccessoryInfo rmaInfo)
       {
           TransactionOptions options = new TransactionOptions();
           options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
           options.Timeout = TransactionManager.DefaultTimeout;

           using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
           {
               rmaInfo.SysNo = SequenceDac.GetInstance().Create("RMA_SendAccessory_Sequence");
               rmaInfo.SendAccessoryID = this.BuildSendAccessoryID(rmaInfo.SysNo);
               rmaInfo.Status = (int)AppEnum.RMASendAccessoryStatus.WaitingAudit;
               this.InsertSentAccessory(rmaInfo);
               foreach (RMASendAccessoryItemInfo rmaItem in rmaInfo.ItemHash.Values)
               {
                   rmaItem.SendAccessorySysNo = rmaInfo.SysNo;
                   RMASendAccessoryManager.GetInstance().InsertSentAccessoryItem(rmaItem);
               }
               scope.Complete();
           }
       } 

       public DataSet GetSentAccessorytList(Hashtable paramHash)
       {
           string sql = @"select RMA_SendAccessory.* , SO_Master.SOID , Customer.CustomerID ,Customer.CustomerName,Product.ProductID,Product.ProductName,
                           from   RMA_SendAccessory (NOLOCK)
                                  left join V_SO_Master SO_master (NOLOCK) on SO_Master.sysno = RMA_SendAccessory.SOSysNo
                                  left join Customer (NOLOCK) on RMA_SendAccessory.CustomerSysNo = Customer.SysNo                                  
                                  left join Sys_User as su2 (NOLOCK) on RMA_SendAccessory.CreateUserSysNo = su2.SysNo
                           where  1=1  @DateTo  @DateFrom  @SOID @SendAccessoryID @SysNo @CustomerSysNo @CustomerID @CreateUserSysNo order by @GroupBy desc";

           if (paramHash.ContainsKey("DateTo"))
               sql = sql.Replace("@DateTo", " and RMA_SendAccessory.CreateTime <=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
           else
               sql = sql.Replace("@DateTo", "");

           if (paramHash.ContainsKey("DateFrom"))
               sql = sql.Replace("@DateFrom", " and RMA_SendAccessory.CreateTime >=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
           else
               sql = sql.Replace("@DateFrom", "");

           if (paramHash.ContainsKey("SOID"))
               sql = sql.Replace("@SOID", " and SO_Master.SOID like " + Util.ToSqlLikeString(paramHash["SOID"].ToString()));
           else
               sql = sql.Replace("@SOID", "");

           if (paramHash.ContainsKey("SendAccessoryID"))
               sql = sql.Replace("@SendAccessoryID", " and RMA_SendAccessory.SendAccessoryID like " + Util.ToSqlLikeString(paramHash["SendAccessoryID"].ToString()));
           else
               sql = sql.Replace("@SendAccessoryID", "");

           if (paramHash.ContainsKey("SysNo"))
               sql = sql.Replace("@SysNo", " and RMA_Request.SysNo = " + Util.ToSqlString(paramHash["SysNo"].ToString()));
           else
               sql = sql.Replace("@SysNo", "");

           if (paramHash.ContainsKey("CustomerSysNo"))
           {
               sql = sql.Replace("@CustomerSysNo", "  and customer.SysNo =" + Util.ToSqlString(paramHash["CustomerSysNo"].ToString()));
           }
           else
           {
               sql = sql.Replace("@CustomerSysNo", "");
           }

           if (paramHash.ContainsKey("CustomerID"))
           {
               sql = sql.Replace("@CustomerID", "  and customer.CustomerID =" + Util.ToSqlString(paramHash["CustomerID"].ToString()));
           }
           else
           {
               sql = sql.Replace("@CustomerID", "");
           }

          if (paramHash.ContainsKey("CreateUserSysNo"))
           {
               sql = sql.Replace("@CreateUserSysNo", " and RMA_SendAccessory.CreateUserSysNo=" + Util.ToSqlString(paramHash["CreateUserSysNo"].ToString()));
           }
           else
           {
               sql = sql.Replace("@CreateUserSysNo", "");
           }

           if (paramHash.ContainsKey("GroupBy"))
           {
                   sql = sql.Replace("@GroupBy", "RMA_SendAccessory.CreateTime");
           }
           else
           {
               sql = sql.Replace("@GroupBy", "RMA_SendAccessory.CreateTime");
           }

           if (paramHash == null || paramHash.Count == 1)
               sql = sql.Replace("select", "select top 50 ");

           return SqlHelper.ExecuteDataSet(sql);
       }
   
       public DataSet GetSentAccessorytItemList(int sysno)
       {
           string sql = @"select RMA_SendAccessory_Item.* ,Product.ProductName ,Product.ProductID,Product.SysNo as ProductSysNo
                            from RMA_SendAccessory_Item (NOLOCK) 
							inner join Product (NOLOCK) on Product.sysno = RMA_SendAccessory_Item.ProductSysNo 
							inner join RMA_SendAccessory (NOLOCK) on RMA_SendAccessory.SysNo = RMA_SendAccessory_Item.SendAccessorySysNo
							where RMA_SendAccessory.SysNo = " + sysno;
           return SqlHelper.ExecuteDataSet(sql);
       }

       public DataSet CheckSO(int SOsysno)
       {
           string sql = @"select RMA_SendAccessory.*
                        from RMA_SendAccessory
                        where RMA_SendAccessory.sosysno=" + SOsysno+"order by CreateTime Desc";
           return SqlHelper.ExecuteDataSet(sql);
       }

       public string CheckSOItem(RMASendAccessoryInfo oInfo)
       {
           //是否已存在该SO中该商品的RMA单

           string result = AppConst.StringNull;
           foreach (RMASendAccessoryItemInfo oSAItemInfo in oInfo.ItemHash.Values)
           {
               string sql = @"select RMA_SendAccessory_Item.*,RMA_SendAccessory.SendAccessoryID
                               from  RMA_SendAccessory_Item (NOLOCK) 
                               inner join RMA_SendAccessory(NOLOCK) on RMA_SendAccessory.sysno = RMA_SendAccessory_Item.SendAccessorySysNo
                               where RMA_SendAccessory_Item.sosysno = " + oInfo.SOSysNo + " and RMA_SendAccessory_Item.productsysno=" + oSAItemInfo.ProductSysNo
                   + " and (RMA_SendAccessory.status = " + (int)AppEnum.RMASendAccessoryStatus.WaitingAudit + " or RMA_SendAccessory.status = " + (int)AppEnum.RMASendAccessoryStatus.WaitingSend + ")";

               DataSet ds = SqlHelper.ExecuteDataSet(sql);
               foreach(DataRow dr in ds.Tables[0].Rows)
               {
                   ProductBasicInfo oProduct = ProductManager.GetInstance().LoadBasic(oSAItemInfo.ProductSysNo);
                   if (result != AppConst.StringNull)
                       result += "<br>";
                   result += oProduct.ProductName + "(" + oProduct.ProductID + ")" + "&nbsp;&nbsp;<a href='../RMA/SendAccessoryOpt.aspx?sysno=" + dr["sendAccessorysysno"].ToString() + "&opt=view'>" + dr["SendAccessoryID"].ToString() + "</a>";
               }
              
           }
           return result;
       }

       public void InsertSentAccessory(RMASendAccessoryInfo oInfo)
       {
           new RMASendAccessoryDac().Insert(oInfo);
       }
       public void InsertSentAccessoryItem(RMASendAccessoryItemInfo oInfo)
       {
           new RMASendAccessoryDac().InsertItem(oInfo);
       }

       public int DeleteItem( int productSysNo,int saSysNo)
       {
           int result;
           RMASendAccessoryItemInfo saItemInfo = RMASendAccessoryManager.GetInstance().LoadItem(productSysNo, saSysNo);
           RMASendAccessoryInfo saInfo = RMASendAccessoryManager.GetInstance().Load(saSysNo);
           if (saInfo.Status != (int)AppEnum.RMASendAccessoryStatus.WaitingAudit)
               throw new BizException("补发附件单不是待审核状态，不能删除相关Item!");
           else
           {
               result = new RMASendAccessoryDac().DeleteItem(saItemInfo.SysNo);
           }
           return result;
       }

       public string BuildSendAccessoryID(int rmaSendAccessorySysNo)
       {
           return "R5" + rmaSendAccessorySysNo.ToString().PadLeft(8, '0');
       }

       private void map(RMASendAccessoryInfo oParam, DataRow tempdr)
       {
           oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
           oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
           oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
           oParam.SendAccessoryID = Util.TrimNull(tempdr["SendAccessoryID"]);
           oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
           oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
           oParam.AreaSysNo = Util.TrimIntNull(tempdr["AreaSysNo"]);
           oParam.Address = Util.TrimNull(tempdr["Address"]);
           oParam.Contact = Util.TrimNull(tempdr["Contact"]);
           oParam.Phone = Util.TrimNull(tempdr["Phone"]);
           oParam.Status = Util.TrimIntNull(tempdr["Status"]);
           oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
           oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
           oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
           oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
           oParam.SendTime = Util.TrimDateNull(tempdr["SendTime"]);
           oParam.SendStockSynNo = Util.TrimIntNull(tempdr["SendStockSynNo"]);
           oParam.ShipTypeSysNo = Util.TrimIntNull(tempdr["ShipTypeSysNo"]);
           oParam.SendUserSysNo = Util.TrimIntNull(tempdr["SendUserSysNo"]);
           oParam.Memo = Util.TrimNull(tempdr["Memo"]);
           oParam.IsPrintPackage = Util.TrimIntNull(tempdr["IsPrintPackage"]);
           oParam.PackageID = Util.TrimNull(tempdr["PackageID"]);
           oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
           oParam.SetDeliveryManTime = Util.TrimDateNull(tempdr["SetDeliveryManTime"]);
       }
       private void mapItem(RMASendAccessoryItemInfo oParam, DataRow tempdr)
       {
           oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
           oParam.SendAccessorySysNo = Util.TrimIntNull(tempdr["SendAccessorySysNo"]);
           oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
           oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
           oParam.ProductIDSysNo = Util.TrimIntNull(tempdr["ProductIDSysNo"]);
           oParam.NewProductSysNo = Util.TrimIntNull(tempdr["NewProductSysNo"]);
           oParam.NewProductQty = Util.TrimIntNull(tempdr["NewProductQty"]);
           oParam.NewProductIDSysNo = Util.TrimIntNull(tempdr["NewProductIDSysNo"]);
           oParam.AccessoryType = Util.TrimIntNull(tempdr["AccessoryType"]);
           oParam.AccessoryName = Util.TrimNull(tempdr["AccessoryName"]);
           oParam.SOItemPODesc = Util.TrimNull(tempdr["SOItemPODesc"]);
           oParam.Note = Util.TrimNull(tempdr["Note"]);
       }
       public RMASendAccessoryInfo BuildfromSO(int soSysNo)
        {
            RMASendAccessoryInfo rmaInfo = new RMASendAccessoryInfo();
            SOInfo soInfo = SaleManager.GetInstance().LoadSO(soSysNo);
            if (soInfo != null && soInfo.Status == (int)AppEnum.SOStatus.OutStock)
            {
                rmaInfo.SOSysNo = soInfo.SysNo;
                rmaInfo.CustomerSysNo = soInfo.CustomerSysNo;
                CustomerInfo oCustomer = CustomerManager.GetInstance().Load(soInfo.CustomerSysNo);
                rmaInfo.Address = soInfo.ReceiveAddress != AppConst.StringNull ? soInfo.ReceiveAddress : (oCustomer.ReceiveAddress != AppConst.StringNull ? oCustomer.ReceiveAddress : (oCustomer.DwellAddress != AppConst.StringNull ? oCustomer.DwellAddress : "无"));
                rmaInfo.Contact = soInfo.ReceiveContact != AppConst.StringNull ? soInfo.ReceiveContact : (oCustomer.ReceiveContact != AppConst.StringNull ? oCustomer.ReceiveContact : (oCustomer.CustomerName != AppConst.StringNull ? oCustomer.CustomerName : "无"));
                rmaInfo.CreateTime = DateTime.Now;
                if (soInfo.ReceivePhone != AppConst.StringNull)
                    rmaInfo.Phone = soInfo.ReceivePhone;
                if (soInfo.ReceiveCellPhone != AppConst.StringNull)
                    rmaInfo.Phone += "  " + soInfo.ReceiveCellPhone;
                rmaInfo.AreaSysNo = soInfo.ReceiveAreaSysNo;
                if (soInfo.ItemHash.Count > 0)
                {
                    int j = 0;
                    foreach (SOItemInfo soItem in soInfo.ItemHash.Values)
                    {
                        if (soItem.ProductType == (int)AppEnum.SOItemType.Promotion)
                            continue;  //这里排除优惠券。


                        if (soItem.Quantity > 0)
                        {
                            Hashtable htTemp = new Hashtable(1);
                            htTemp.Add("SOItemSysNo", soItem.SysNo.ToString());
                            DataSet dsTemp = SaleManager.GetInstance().GetSOProductIDSysNoList(htTemp);

                            int rowCount = 0;
                            if (Util.HasMoreRow(dsTemp))
                                rowCount = dsTemp.Tables[0].Rows.Count;

                            for (int i = 1; i <= soItem.Quantity; i++)
                            {
                                RMASendAccessoryItemInfo rmaItem = new RMASendAccessoryItemInfo();
                                rmaItem.ProductSysNo = soItem.ProductSysNo;
                                rmaItem.AccessoryType=
                                rmaItem.AccessoryType = (int)AppEnum.RMAAccessoryType.Accessory;

                                if (i <= rowCount)
                                {
                                    int POSysNo = Util.TrimIntNull(dsTemp.Tables[0].Rows[i - 1]["posysno"].ToString());
                                    int ProductIDSysNo = Util.TrimIntNull(dsTemp.Tables[0].Rows[i - 1]["ProductIDSysNo"]);
                                    if (POSysNo > 0)
                                    {
                                        rmaItem.SOItemPODesc = "采购单号:<a href=\"javascript:openWindowS2('../Purchase/POSheet.aspx?sysno=" + POSysNo + "&opt=view')" + "\" >" + POSysNo + "</a><br>";
                                    }
                                    else if (ProductIDSysNo > 0)
                                    {
                                        rmaItem.SOItemPODesc = "商品序列号:<a href=\"javascript:openWindowS2('../Basic/ProductID.aspx?sysno=" + ProductIDSysNo + "')" + "\" >" + ProductIDSysNo + "</a><br>";
                                    }
                                    rmaItem.ProductIDSysNo = ProductIDSysNo;
                                    rmaItem.AccessoryType =(int) AppEnum.RMAAccessoryType.Accessory;
                                }
                                rmaInfo.ItemHash.Add(j, rmaItem);
                                j = j + 1;
                            }
                        }
                    }
                }
            }
            else
                rmaInfo = null;
            return rmaInfo;
        }
       public RMASendAccessoryInfo BuildfromSO(string soID)
        {
            int sysno = SaleManager.GetInstance().GetSOSysNofromID(soID.ToString());
            if (sysno != AppConst.IntNull)
                return this.BuildfromSO(sysno);
            else
            {
                try
                {
                    return this.BuildfromSO(Util.TrimIntNull(soID));
                }
                catch
                {
                    return null;
                }
            }
        }

       public DataSet GetSendAccessoryList(Hashtable paramHash)
       {
           string sql = @"select RMA_SendAccessory.* , SO_Master.SOID , Customer.CustomerID ,Customer.CustomerName ,su.UserName as CreateUser,su2.UserName as SendUser,ShipType.shiptypename
                           from   RMA_SendAccessory (NOLOCK)
                                  left join SO_Master  (NOLOCK) on SO_Master.sysno = RMA_SendAccessory.SOSysNo
                                  left join Customer (NOLOCK) on RMA_SendAccessory.CustomerSysNo = Customer.SysNo                                  
                                  left join Sys_User as su  on RMA_SendAccessory.CreateUserSysNo = su.SysNo
                                  left join Sys_User as su2 on RMA_SendAccessory.SendUserSysNo = su2.SysNo
                                  left join ShipType on RMA_SendAccessory.shiptypesysno=ShipType.sysno
                           where  1=1  @DateTo  @DateFrom  @SysNo  @SOID @SendAccessoryID @HandleTime @CustomerSysNo @CustomerID  @ShipTypeSysNo @Status  order by @GroupBy desc";

           if (paramHash.ContainsKey("DateTo"))
               sql = sql.Replace("@DateTo", " and RMA_SendAccessory.CreateTime <=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
           else
               sql = sql.Replace("@DateTo", "");

           if (paramHash.ContainsKey("DateFrom"))
               sql = sql.Replace("@DateFrom", " and RMA_SendAccessory.CreateTime >=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
           else
               sql = sql.Replace("@DateFrom", "");


           if (paramHash.ContainsKey("SOID"))
               sql = sql.Replace("@SOID", " and SO_Master.SOID like " + Util.ToSqlLikeString(paramHash["SOID"].ToString()));
           else
               sql = sql.Replace("@SOID", "");

           if (paramHash.ContainsKey("SendAccessoryID"))
               sql = sql.Replace("@SendAccessoryID", " and RMA_SendAccessory.SendAccessoryID like " + Util.ToSqlLikeString(paramHash["SendAccessoryID"].ToString()));
           else
               sql = sql.Replace("@SendAccessoryID", "");

           if (paramHash.ContainsKey("SysNo"))
               sql = sql.Replace("@SysNo", " and RMA_SendAccessory.SysNo = " + Util.ToSqlString(paramHash["SysNo"].ToString()));
           else
               sql = sql.Replace("@SysNo", "");

           if (paramHash.ContainsKey("CustomerSysNo"))
           {
               sql = sql.Replace("@CustomerSysNo", "  and customer.SysNo =" + Util.ToSqlString(paramHash["CustomerSysNo"].ToString()));
           }
           else
           {
               sql = sql.Replace("@CustomerSysNo", "");
           }

           if (paramHash.ContainsKey("CustomerID"))
           {
               sql = sql.Replace("@CustomerID", "  and customer.CustomerID =" + Util.ToSqlString(paramHash["CustomerID"].ToString()));
           }
           else
           {
               sql = sql.Replace("@CustomerID", "");
           }
           if (paramHash.ContainsKey("ShipTypeSysNo"))
           {
               sql=sql.Replace("@ShipTypeSysNo","and RMA_SendAccessory.ShipTypeSysNo="+Util.ToSqlString(paramHash["ShipTypeSysNo"].ToString()));
           
           }
           else
           {
               sql = sql.Replace("@ShipTypeSysNo", "");
           }

           if (paramHash.ContainsKey("Status"))
           {
               sql = sql.Replace("@Status", "and RMA_SendAccessory.Status=" + Util.ToSqlString(paramHash["Status"].ToString()));

           }
           else
           {
               sql = sql.Replace("@Status", "");
           }

           if (paramHash.ContainsKey("HandleTime"))
           {
               sql = sql.Replace("@HandleTime", " and DateDiff(Day,RMA_SendAccessory.CreateTime,isnull(RMA_SendAccessory.SendTime,getdate()))" + (paramHash["HandleTime"].ToString()));
           }
           else
               sql = sql.Replace("@HandleTime", "");

           if (paramHash.ContainsKey("GroupBy"))
           {
               if (paramHash["GroupBy"].ToString() == "0")
                   sql = sql.Replace("@GroupBy", "RMA_SendAccessory.CreateTime");
               else
                   sql = sql.Replace("@GroupBy", "RMA_SendAccessory.RecvTime");
           }
           else
           {
               sql = sql.Replace("@GroupBy", "RMA_SendAccessory.CreateTime");
           }

           if (paramHash == null || paramHash.Count == 0)
               sql = sql.Replace("select", "select top 50 ");

           return SqlHelper.ExecuteDataSet(sql);
       }

       public int GetSendAccessorySysNoByID(string SendAccessoryID)
       {
           string sql = @"select SysNo
                           from  RMA_SendAccessory (nolock)
                           where RMA_SendAccessory.SendAccessoryID='" + SendAccessoryID + "'";

           int SendAccessorySysNo = AppConst.IntNull;

           DataSet ds = SqlHelper.ExecuteDataSet(sql);
           if (Util.HasMoreRow(ds))
           {
               DataRow dr = ds.Tables[0].Rows[0];
               SendAccessorySysNo = Util.TrimIntNull(dr["SysNo"]);
           }

           return SendAccessorySysNo;
       }

       public int GetFreightUserSysNoByID(string SendAccessoryID)
       {
           string sql = @"select FreightUserSysNo
                           from  RMA_SendAccessory (nolock)
                           where RMA_SendAccessory.SendAccessoryID='" + SendAccessoryID + "'";

           DataSet ds = SqlHelper.ExecuteDataSet(sql);
           if (Util.HasMoreRow(ds))
               return (int)ds.Tables[0].Rows[0][0];
           else
               return AppConst.IntNull;
       }
       public int GetDLSysNoByID(string SendAccessoryID)
       {
           string sql = @"select dlsysno
                           from  RMA_SendAccessory (nolock)
                           where RMA_SendAccessory.SendAccessoryID='" + SendAccessoryID + "'";

           DataSet ds = SqlHelper.ExecuteDataSet(sql);
           if (Util.HasMoreRow(ds))
               return (int)ds.Tables[0].Rows[0][0];
           else
               return AppConst.IntNull;
       }

       public DataSet GetFreightMenDs(Hashtable paramHash)
       {
           string sql = @"select RMA_SendAccessory.*,area.DistrictName,area.localcode,sys_user.username as freightusername
                          from RMA_SendAccessory,area ,sys_user
                          where RMA_SendAccessory.status="+(int)AppEnum.RMASendAccessoryStatus.Sent+" and AreaSysNo=area.sysno and sys_user.sysno=FreightUserSysNo";


           string sql1 = "";

           if (paramHash != null && paramHash.Count != 0)
           {
               StringBuilder sb = new StringBuilder(100);
               foreach (string key in paramHash.Keys)
               {
                   object item = paramHash[key];
                   if (key == "DateFrom")
                   {
                       sb.Append(" and");
                       sb.Append(" SetDeliveryManTime >=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (key == "DateTo")
                   {
                       sb.Append(" and");
                       sb.Append(" SetDeliveryManTime <=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (item is int)
                   {
                       sb.Append(" and ");
                       sb.Append(key).Append("=").Append(item.ToString());
                   }
                   else if (key == "orderby")
                   {
                       sql1 = " order by SetDeliveryManTime desc ";
                   }
                   sql += sb.ToString();
               }
           }
           sql += sql1;
           return SqlHelper.ExecuteDataSet(sql);
       }

       public void SetDeliveryMen(int sysno, int freightManSysNo, int dlsysno)
       {
           TransactionOptions options = new TransactionOptions();
           options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
           options.Timeout = TransactionManager.DefaultTimeout;

           using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
           {
               //RMASendAccessoryInfo oInfo = RMASendAccessoryManager.GetInstance().Load(sysno);
               //oInfo.FreightUserSysNo = freightManSysNo;
               //oInfo.SetDeliveryManTime = DateTime.Now;
               //RMASendAccessoryManager.GetInstance().Update(oInfo);

               Hashtable updateHash = new Hashtable();
               updateHash.Add("SysNo", sysno);
               updateHash.Add("freightusersysno", freightManSysNo);
               updateHash.Add("SetDeliveryManTime", System.DateTime.Now);
               updateHash.Add("DLSysNo", dlsysno);

               new RMASendAccessoryDac().Update(updateHash);
               scope.Complete();
           }
       }
       public void SetDeliveryMen(Hashtable ht)
       {
           TransactionOptions options = new TransactionOptions();
           options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
           options.Timeout = TransactionManager.DefaultTimeout;

           using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
           {
               new RMASendAccessoryDac().Update(ht);
               scope.Complete();
           }
       }
    }
}
