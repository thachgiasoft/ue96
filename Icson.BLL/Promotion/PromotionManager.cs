using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Icson.Objects;
using Icson.Objects.Promotion;
using Icson.DBAccess.Promotion;
using Icson.DBAccess;
using Icson.Utils;

namespace Icson.BLL.Promotion
{
  public  class PromotionManager
    {
      private PromotionManager()
      { 
      }

      private static PromotionManager _instance;
      public static PromotionManager GetInstance()
      {
          if (_instance == null)
          {
              _instance = new PromotionManager();
          }
          return _instance;
      }

      public DataSet GetPromotionDs(Hashtable paramHash)
      {
          string sql = @"select pm.SysNo,pm.PromotionName,pm.CreateTime,pm.status, su.username as username
                        from Promotion_Master pm
                        left join sys_user su on pm.CreateUserSysNo=su.sysno
                        where 1=1 @DateFrom @DateTo @status @KeyWords ";
          if (paramHash.ContainsKey("DateFrom"))
          {
              sql = sql.Replace("@DateFrom", "and pm.CreateTime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
          }
          else
          {
              sql = sql.Replace("@DateFrom", "");
          }
          if (paramHash.ContainsKey("DateTo"))
          {
              sql = sql.Replace("@DateTo", "and pm.CreateTime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
          }
          else
          {
              sql = sql.Replace("@DateTo", "");
          }
          if (paramHash.ContainsKey("Status"))
          {
              sql = sql.Replace("@status", "and pm.status=" + Util.ToSqlString(paramHash["Status"].ToString()));
          }
          else
          {
              sql = sql.Replace("@status", "");
          }
          if (paramHash.ContainsKey("KeyWords"))
          {
              string[] keys = (Util.TrimNull(paramHash["KeyWords"].ToString())).Split(' ');
              if (keys.Length == 1)
              {
                  sql = sql.Replace("@KeyWords", "and pm.PromotionName like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()));
              }
              else
              {
                  string t = "";
                  for (int i = 0; i < keys.Length; i++)
                  {
                      t += "and pm.PromotionName like" + Util.ToSqlLikeString(keys[i]);
                     
                  }
                  sql = sql.Replace("@KeyWords", t);

              }
          }
          else
          {
              sql = sql.Replace("@KeyWords", "");
          }

          if (paramHash == null || paramHash.Count == 0)
              sql = sql.Replace("select", "select top 50 ");
          sql = sql + "order by pm.sysno desc";

          return SqlHelper.ExecuteDataSet(sql);

         
      }


      private void map(PromotionMasterInfo oParam, DataRow tempdr)
      {
          oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
          oParam.PromotionName = Util.TrimNull(tempdr["PromotionName"]);
          oParam.PromotionNote = Util.TrimNull(tempdr["PromotionNote"]);
          oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
          oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
          oParam.Status = Util.TrimIntNull(tempdr["Status"]);
      }

      private void map(PromotionItemGroupInfo oParam, DataRow tempdr)
      {
          oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
          oParam.PromotionSysNo = Util.TrimIntNull(tempdr["PromotionSysNo"]);
          oParam.GroupName = Util.TrimNull(tempdr["GroupName"]);
          oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
          oParam.Status = Util.TrimIntNull(tempdr["Status"]);
      }

      private void map(PromotionItemProductInfo oParam, DataRow tempdr)
      {
          oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
          oParam.PromotionItemGroupSysNo = Util.TrimIntNull(tempdr["PromotionItemGroupSysNo"]);
          oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
          oParam.PromotionDiscount = Util.TrimDecimalNull(tempdr["PromotionDiscount"]);
          oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
      }
      public PromotionMasterInfo Load(int sysNo)
      {
          string sql = "select * from Promotion_Master where sysno =" + sysNo;
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          if (!Util.HasMoreRow(ds))
              return null;
          PromotionMasterInfo oMasterInfo = new PromotionMasterInfo();
          map(oMasterInfo, ds.Tables[0].Rows[0]);
          return oMasterInfo;
      }
      public PromotionItemGroupInfo LoadGroup(int sysNo)
      {
          string sql = "select * from Promotion_Item_Group where sysno =" + sysNo;
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          if (!Util.HasMoreRow(ds))
              return null;
          PromotionItemGroupInfo oGroutInfo = new PromotionItemGroupInfo();
          map(oGroutInfo, ds.Tables[0].Rows[0]);
          return oGroutInfo;
      }
      public PromotionItemProductInfo LoadItemProduct(int sysNo)
      {
          string sql = "select * from Promotion_Item_Product where sysno =" + sysNo;
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          if (!Util.HasMoreRow(ds))
              return null;
          PromotionItemProductInfo oItemProductInfo = new PromotionItemProductInfo();
          map(oItemProductInfo, ds.Tables[0].Rows[0]);
          return oItemProductInfo;
      }

      public void InsertMaster (PromotionMasterInfo oParam)
      {
          new PromotionMasterDac().Insert(oParam);
      }
      public void UpdateMaster(PromotionMasterInfo oParam)
      {
          new PromotionMasterDac().Update(oParam);
      }

      public void InsertItemProduct(PromotionItemProductInfo oParam)
      {
          new PromotionItemProductDac().Insert(oParam);
      }
      public void UpdateItemProduct(PromotionItemProductInfo oParm)
      {
          new PromotionItemProductDac().Update(oParm);
      }

      public void InsertItemGroup(PromotionItemGroupInfo oParam)
      {
          new PromotionItemGroupDac().Insert(oParam);
      }

      public void UpdateItemGroup(PromotionItemGroupInfo oParam)
      {
          new PromotionItemGroupDac().Update(oParam);
      }

      public Hashtable GetPromotionProductSysNoHash(int promotionSysNo)
      {
          string sql = @"select distinct pp.productsysno,isnull(pp.PromotionDiscount,0) as PromotionDiscount
                        from Promotion_Item_Product pp 
                        left join Promotion_Item_Group pg on pp.PromotionItemGroupSysNo=pg.sysno 
                        left join promotion_master pm on pg.promotionsysno=pm.sysno 
                        where pm.status=0 and pg.promotionsysno=" + promotionSysNo;
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          if (Util.HasMoreRow(ds))
          {
              Hashtable ht = new Hashtable(50);
              foreach (DataRow dr in ds.Tables[0].Rows)
              {
                  ht.Add(Util.TrimIntNull(dr["productsysno"]), Util.TrimDecimalNull(dr["PromotionDiscount"]));
              }
              return ht;
          }
          else
              return null;
      }

      public DataSet GetPromotionProductDs(Hashtable paramHash)
      {
          string sql = @"select pp.sysno,product.ProductName,pg.GroupName,pp.ordernum,pp.productsysno,pp.promotiondiscount
                        from Promotion_Item_Product pp 
                        left join product on pp.productsysno =product.sysno
                        left join Promotion_Item_Group pg on pp.PromotionItemGroupSysNo=pg.sysno
                        where 1=1 @PromotionMasterSysNo @productsysno order by pg.ordernum,pp.ordernum";
          if (paramHash.ContainsKey("PromotionMasterSysNo"))
          {
              sql=sql.Replace("@PromotionMasterSysNo","and pg.promotionsysno="+Util.ToSqlString( paramHash["PromotionMasterSysNo"].ToString()));
          }
          else
          {
              sql=sql.Replace("@PromotionMasterSysNo","");
          }
          if (paramHash.ContainsKey("productsysno"))
          {
              sql = sql.Replace("@productsysno", "and pp.productsysno=" + Util.ToSqlString(paramHash["productsysno"].ToString()));
          }
          else
          {
              sql=sql.Replace("@productsysno","");
          }
         return SqlHelper.ExecuteDataSet(sql);
      }

      public DataSet GetPromotionGroupDs(Hashtable paramHash)
      {
          string sql = @"select Promotion_Item_Group.GroupName,Promotion_Item_Group.sysno,Promotion_Item_Group.ordernum,Promotion_Item_Group.status
                        from Promotion_Item_Group 
                        left join Promotion_Master on Promotion_Item_Group.PromotionSysNo=Promotion_Master.sysno
                        where 1=1 @PromotionMasterSysNo @status @MasterStatus order by ordernum";
          if (paramHash.ContainsKey("PromotionMasterSysNo"))
          {
              sql = sql.Replace("@PromotionMasterSysNo", "and PromotionSysNo=" + Util.ToSqlString(paramHash["PromotionMasterSysNo"].ToString()));
          }
          else
          {
              sql = sql.Replace("@PromotionMasterSysNo", "");
          }
          if (paramHash.ContainsKey("status"))
          {
              sql = sql.Replace("@status", "and Promotion_Item_Group.status=" + Util.ToSqlString(paramHash["status"].ToString()));
          }
          else
          {
              sql = sql.Replace("@status", "");
          }
          if (paramHash.ContainsKey("MasterStatus"))
          {
              sql = sql.Replace("@MasterStatus", "and Promotion_Master.status=" + Util.ToSqlString(paramHash["status"].ToString()));
          }
          else
          {
              sql = sql.Replace("@MasterStatus", "");
          }

          return SqlHelper.ExecuteDataSet(sql);
      }

      public void DeletePromotionItemProduct(int ItemProductSysNo)
      {
          string sql = @"delete from Promotion_Item_Product where sysno=" + ItemProductSysNo;
          SqlHelper.ExecuteNonQuery(sql);
      }
    }
}
