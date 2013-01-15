using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using Icson.Objects;
using Icson.Objects.Sale;
using Icson.DBAccess.Sale;
using Icson.Utils;
using Icson.DBAccess;

namespace Icson.BLL.Sale
{
  public class WhProductShelvingInspectionManager
    {
      private WhProductShelvingInspectionManager()
      {
      }
      private static WhProductShelvingInspectionManager _instance;
      public static WhProductShelvingInspectionManager GetInstance()
      {
          if (_instance == null)
          {
              _instance = new WhProductShelvingInspectionManager();
          }
          return _instance;
      }
     
      private void map(WhProductShelvingInspectionInfo oParam, DataRow tempdr)
      {
          oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
          oParam.WorkType = Util.TrimIntNull(tempdr["WorkType"]);
          oParam.BillType = Util.TrimIntNull(tempdr["BillType"]);
          oParam.BillSysNo = Util.TrimIntNull(tempdr["BillSysNo"]);
          oParam.AllocatedUserSysNo = Util.TrimIntNull(tempdr["AllocatedUserSysNo"]);
          oParam.RealUserSysNo = Util.TrimIntNull(tempdr["RealUserSysNo"]);
          oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
          oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
      }
      public WhProductShelvingInspectionInfo Load(int SysNo)
      {
          string sql = "select * from Wh_ProductShelvingInspection where sysno =" + SysNo;
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          if (!Util.HasMoreRow(ds))
              return null;
          WhProductShelvingInspectionInfo oInfo = new WhProductShelvingInspectionInfo();
          this.map(oInfo, ds.Tables[0].Rows[0]);
          return oInfo;
      }
      public void Insert(WhProductShelvingInspectionInfo oParam)
      {
          new WhProductShelvingInspectionDac().Insert(oParam);
      }
      public void Update(WhProductShelvingInspectionInfo oParam)
      {
          new WhProductShelvingInspectionDac().Update(oParam);
      }
      public DataSet GetUserBillDs(Hashtable paramHash)
      {
          string sql = @"select psi.*,su1.username as Realusername,su2.username as allocateUserName
                          from Wh_ProductShelvingInspection as psi
                          left join sys_user su1 on su1.sysno=psi.RealUserSysNo
                          left join sys_user su2 on su2.sysno=AllocatedUserSysNo
                          where 1=1";


          string sql1 = "";

          if (paramHash != null && paramHash.Count != 0)
          {
              StringBuilder sb = new StringBuilder(100);
              foreach (string key in paramHash.Keys)
              {
                  object item = paramHash[key];
                  if (key == "WorkType")
                  {
                      sb.Append(" and");
                      sb.Append(" psi.WorkType =").Append(Util.ToSqlString(item.ToString()));
                  }
                  else if (key == "BillType")
                  {
                      sb.Append(" and");
                      sb.Append(" psi.BillType =").Append(Util.ToSqlString(item.ToString()));
                  }
                  else if (key == "UpdateTimeFrom")
                  {
                      sb.Append(" and ");
                      sb.Append(" psi.UpdateTime >=").Append(Util.ToSqlString(item.ToString()));
                  }
                  else if (key == "UpdateTimeTo")
                  {
                      sb.Append(" and ");
                      sb.Append(" psi.UpdateTime <=").Append(Util.ToSqlEndDate(item.ToString()));
                  }
                  else if (key == "top")
                  {
                      sql = sql.Replace("select ", item.ToString());
                  }
                  else if (item is int)
                  {
                      sb.Append(" and ");
                      sb.Append(key).Append("=").Append(item.ToString());
                  }
                  else if (key == "orderby")
                  {
                      sql1 = " order by UpdateTime desc ";
                  }
              }
              sql += sb.ToString();
          }
          sql += sql1;
          return SqlHelper.ExecuteDataSet(sql);
      }

      public int GetSysNo(Hashtable paramHash)
      {
          string sql = @"select sysno  from Wh_ProductShelvingInspection as psi where 1=1";
          string sql1 = "";

          if (paramHash != null && paramHash.Count != 0)
          {
              StringBuilder sb = new StringBuilder(100);
              foreach (string key in paramHash.Keys)
              {
                  object item = paramHash[key];
                  if (key == "WorkType")
                  {
                      sb.Append(" and");
                      sb.Append(" psi.WorkType =").Append(Util.ToSqlString(item.ToString()));
                  }
                  else if (key == "BillType")
                  {
                      sb.Append(" and");
                      sb.Append(" psi.BillType =").Append(Util.ToSqlString(item.ToString()));
                  }
                  else if (key == "BillSysNo")
                  {
                      sb.Append(" and");
                      sb.Append(" psi.BillSysNo =").Append(Util.ToSqlString(item.ToString()));
                  }
                  else if (item is int)
                  {
                      sb.Append(" and ");
                      sb.Append(key).Append("=").Append(item.ToString());
                  }
                  else if (key == "orderby")
                  {
                      sql1 = " order by UpdateTime desc ";
                  }
              }
              sql += sb.ToString();
          }
          sql += sql1;

          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          int sysno = 0;
          if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count > 0)
          {
              sysno = Util.TrimIntNull(ds.Tables[0].Rows[0][0].ToString());
          }
          return sysno;

      }
    
    
    }
}
