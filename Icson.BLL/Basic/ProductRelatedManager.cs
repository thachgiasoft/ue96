using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using System.Transactions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.DBAccess.Basic;
using Icson.DBAccess;
using Icson.Utils;

namespace Icson.BLL.Basic
{
  public  class ProductRelatedManager
    {
      private ProductRelatedManager()
      {
      }

      private static ProductRelatedManager _instance;
      public static ProductRelatedManager GetInstance()
      {
          if (_instance == null)
          {
              _instance = new ProductRelatedManager();
          }
          return _instance;
      }

      private void map(ProductRelatedInfo oParam, DataRow tempdr)
      {
          oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
          oParam.MasterProductSysNo = Util.TrimIntNull(tempdr["MasterProductSysNo"]);
          oParam.RelatedProductSysNo = Util.TrimIntNull(tempdr["RelatedProductSysNo"]);
          oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
          oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
          oParam.Status = Util.TrimIntNull(tempdr["Status"]);
      }

      private void Insert(ProductRelatedInfo oParam)
      {
           new ProductRelatedDac().Insert(oParam);
      }

      public ProductRelatedInfo Load(int SysNo)
      {
          string sql = "select * from product_Related where sysno=" + SysNo;
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          if (Util.HasMoreRow(ds))
          {
              ProductRelatedInfo oInfo = new ProductRelatedInfo();
              map(oInfo, ds.Tables[0].Rows[0]);
              return oInfo;
          }
          else
              return null;
      }

      public void SetProductRelated(Hashtable paramHash,int CreateUserSysNo)
      {
          TransactionOptions options = new TransactionOptions();
          options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
          options.Timeout = TransactionManager.DefaultTimeout;

          using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
          {
              DataSet ds = ProductRelatedQuery(paramHash);
              if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count > 0)
              {
                  foreach (DataRow dr in ds.Tables[0].Rows)
                  {
                      int invalidstatus = (int)AppEnum.BiStatus.InValid;
                      string Updatesql = "update Product_Related set status=" + invalidstatus + " where MasterProductSysNo =" + dr["MasterProductSysNo"] + "and RelatedProductSysNo=" + dr["RelatedProductSysNo"];
                      SqlHelper.ExecuteDataSet(Updatesql);
                  }
              }
              string sql = @"select Product.sysno from Product 
                        inner join category3 on category3.sysno=Product.c3sysno
                        inner join category2 on category3.c2sysno = category2.sysno 
						inner join category1 on category2.c1sysno = category1.sysno
                        where 1=1 @MasterProductSysNo @Category @C3 @KeyWords @MasterProductList ";

              if (paramHash.Contains("MasterProductSysNo"))
              {
                  sql = sql.Replace("@MasterProductSysNo", " and Product.sysno=" + paramHash["MasterProductSysNo"].ToString());
              }
              else
              {
                  sql = sql.Replace("@MasterProductSysNo", " ");
              }
              if (paramHash.ContainsKey("Category"))
              {
                  sql = sql.Replace("@Category", " and " + (string)paramHash["Category"]);
              }
              else
              {
                  sql = sql.Replace("@Category", "");
              }
              if (paramHash.ContainsKey("C3"))
              {
                  sql = sql.Replace("@C3", " and Product." + (string)paramHash["C3"]);
              }
              else
              {
                  sql = sql.Replace("@C3", "");
              }

              if (paramHash.ContainsKey("KeyWords"))
              {
                  string[] Keys = (Util.TrimNull(paramHash["KeyWords"].ToString())).Split(' ');
                  if (Keys.Length == 1)
                  {
                      sql = sql.Replace("@KeyWords", "and (Product.productid like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + " or Product.productname like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + ")");
                  }
                  else
                  {
                      string t = "";
                      t += " and (Product.productid like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + " or ( 1=1 ";
                      for (int i = 0; i < Keys.Length; i++)
                      {
                          t += " and Product.productname like " + Util.ToSqlLikeString(Keys[i]);
                      }
                      t += "))";
                      sql = sql.Replace("@KeyWords", t);
                  }
              }
              else
              {
                  sql = sql.Replace("@KeyWords", "");
              }
              if (paramHash.ContainsKey("MasterProductList"))
              {
                  sql = sql.Replace("@MasterProductList", "and product.sysno in (" + paramHash["MasterProductList"].ToString() + ")");
              }
              else
              {
                  sql = sql.Replace("@MasterProductList", "");
              }
              sql += "order by createtime desc";
              DataSet MasterSysNods = SqlHelper.ExecuteDataSet(sql);
              if (!Util.HasMoreRow(MasterSysNods))
              {
                  throw new BizException("没有此条件的主商品记录");
              }
              foreach (DataRow dr in MasterSysNods.Tables[0].Rows)
              {
                  ProductRelatedInfo oInfo=new ProductRelatedInfo();
                  oInfo.MasterProductSysNo = Util.TrimIntNull(dr["sysno"]);
                  oInfo.RelatedProductSysNo = Util.TrimIntNull(paramHash["RelatedProductSysNo"].ToString());
                  oInfo.CreateTime = DateTime.Now;
                  oInfo.CreateUserSysNo = CreateUserSysNo;
                  oInfo.Status = (int)AppEnum.BiStatus.Valid;
                  Insert(oInfo);
              }
              scope.Complete();
          }
      }

      public DataSet ProductRelatedQuery(Hashtable paramHash)
      {
          string sql = @"select Product_Related.* ,p1.productid as MasterProductID,p1.productname as MasterProductName,p2.productid as RelatedProductID,p2.productname as RelatedProductName
                        from Product_Related
                        left join product p1 on Product_Related.MasterProductSysNo =p1.sysno   
                        left join product p2 on Product_Related.RelatedProductSysNo =p2.sysno
                        inner join category3 on category3.sysno=p1.c3sysno
                        inner join category2 on category3.c2sysno = category2.sysno 
						inner join category1 on category2.c1sysno = category1.sysno
                        where 1=1 @RelatedStatus @MasterProductSysNo @RelatedProductSysNo @Category @C3 @KeyWords
                      ";
          sql = sql.Replace("@RelatedStatus", " and Product_Related.Status=" + ((int)AppEnum.BiStatus.Valid).ToString());
         if (paramHash.Contains("MasterProductSysNo"))
         {
            sql = sql.Replace("@MasterProductSysNo", " and Product_Related.MasterProductSysNo=" + paramHash["MasterProductSysNo"].ToString());
         }
         else
         {
             sql = sql.Replace("@MasterProductSysNo", " ");
         }
         if (paramHash.Contains("RelatedProductSysNo"))
         {
             sql = sql.Replace("@RelatedProductSysNo", " and Product_Related.RelatedProductSysNo=" + paramHash["RelatedProductSysNo"].ToString());
         }
         else
         {
             sql = sql.Replace("@RelatedProductSysNo", " ");
         }
        
         if (paramHash.ContainsKey("Category"))
         {
             sql = sql.Replace("@Category", " and " + (string)paramHash["Category"]);
         }
         else
         {
             sql = sql.Replace("@Category", "");
         }

         if (paramHash.ContainsKey("C3"))
         {
             sql = sql.Replace("@C3", " and p1." + (string)paramHash["C3"]);
         }
         else
         {
             sql = sql.Replace("@C3", "");
         }
         if (paramHash.ContainsKey("KeyWords"))
         {
             string[] Keys = (Util.TrimNull(paramHash["KeyWords"].ToString())).Split(' ');
             if (Keys.Length == 1)
             {
                 sql = sql.Replace("@KeyWords", "and (p1.productid like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + " or p1.productname like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + ")");
             }
             else
             {
                 string t = "";
                 t += " and (p1.productid like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + " or ( 1=1 ";
                 for (int i = 0; i < Keys.Length; i++)
                 {
                     t += " and p1.productname like " + Util.ToSqlLikeString(Keys[i]);
                 }
                 t += "))";
                 sql = sql.Replace("@KeyWords", t);
             }

         }
         else
         {
             sql = sql.Replace("@KeyWords", "");
         }
         sql += "order by Product_Related.createtime desc";
         if (paramHash.Count <= 0)
         {
             sql = sql.Replace("select", "select top 50");
         }
         DataSet ds=SqlHelper.ExecuteDataSet(sql);
         return ds;
     }

      public DataSet AddProductRelatedSearch(Hashtable paramHash)
      {
          string sql = @"select Product.sysno,productid as MasterProductID,productname as MasterProductName
                        from Product
                        inner join category3 on category3.sysno=Product.c3sysno
                        inner join category2 on category3.c2sysno = category2.sysno 
						inner join category1 on category2.c1sysno = category1.sysno
                        where 1=1 @Status @MasterProductSysNo @Category @C3 @KeyWords
                      ";
          sql = sql.Replace("@Status", " and product.status=" + ((int)AppEnum.ProductStatus.Show).ToString());
          if (paramHash.Contains("MasterProductSysNo"))
          {
              sql = sql.Replace("@MasterProductSysNo", " and Product.SysNo=" + paramHash["MasterProductSysNo"].ToString());
          }
          else
          {
              sql = sql.Replace("@MasterProductSysNo", " ");
          }
          if (paramHash.ContainsKey("Category"))
          {
              sql = sql.Replace("@Category", " and " + (string)paramHash["Category"]);
          }
          else
          {
              sql = sql.Replace("@Category", "");
          }

          if (paramHash.ContainsKey("C3"))
          {
              sql = sql.Replace("@C3", " and " + (string)paramHash["C3"]);
          }
          else
          {
              sql = sql.Replace("@C3", "");
          }
          if (paramHash.ContainsKey("KeyWords"))
          {
              string[] Keys = (Util.TrimNull(paramHash["KeyWords"].ToString())).Split(' ');
              if (Keys.Length == 1)
              {
                  sql = sql.Replace("@KeyWords", "and (productid like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + " or productname like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + ")");
              }
              else
              {
                  string t = "";
                  t += " and (productid like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + " or ( 1=1 ";
                  for (int i = 0; i < Keys.Length; i++)
                  {
                      t += " and productname like " + Util.ToSqlLikeString(Keys[i]);
                  }
                  t += "))";
                  sql = sql.Replace("@KeyWords", t);
              }

          }
          else
          {
              sql = sql.Replace("@KeyWords", "");
          }
          sql += "order by Product.createtime desc";
        
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          return ds;
 
      }

      public void DeleteProductRelated(int sysno)
      {
          string sql = @"delete product_related where sysno=" + sysno;
          SqlHelper.ExecuteDataSet(sql);
      }
      public void DeleteProductRelatedBuffer(string sysnoList)
      {
          string sql = @"delete product_related where sysno in (" + sysnoList+")";
          SqlHelper.ExecuteDataSet(sql);
      }
    }
}
