using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;

using Icson.Objects.Complain;
using Icson.DBAccess.Complain;
using Icson.Utils;
using Icson.Objects;
using Icson.DBAccess;

namespace Icson.BLL.Complain
{
  public class ComplainManager
    {
        private ComplainManager()
        {
        }

        private static ComplainManager _instance;
        public static ComplainManager GetInstance()
        {
            if(_instance==null)
            {
                _instance = new ComplainManager();
            }
            return _instance;
        }




      private void map(ComplainInfo oParam, DataRow tempdr)
      {
          oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
          oParam.SoSysNo = Util.TrimIntNull(tempdr["SoSysNo"]);
          oParam.CustomerSysno = Util.TrimIntNull(tempdr["CustomerSysno"]);
          oParam.Contact = Util.TrimNull(tempdr["Contact"]);
          oParam.ContactPhone = Util.TrimNull(tempdr["ContactPhone"]);
          oParam.AreaSysNo = Util.TrimIntNull(tempdr["AreaSysNo"]);
          oParam.Address = Util.TrimNull(tempdr["Address"]);
          oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
          oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
          oParam.Status = Util.TrimIntNull(tempdr["Status"]);
          oParam.LastUpdateUserSysNo = Util.TrimIntNull(tempdr["LastUpdateUserSysNo"]);
          oParam.LastUpdateTime = Util.TrimDateNull(tempdr["LastUpdateTime"]);
          oParam.CurrentHandleUserSysNo = Util.TrimIntNull(tempdr["CurrentHandleUserSysNo"]);
          oParam.AbnormalType = Util.TrimIntNull(tempdr["AbnormalType"]);
          oParam.AbnormalCauseType = Util.TrimIntNull(tempdr["AbnormalCauseType"]);
          oParam.CustomerNote = Util.TrimNull(tempdr["CustomerNote"]);
          oParam.EmployeeNote = Util.TrimNull(tempdr["EmployeeNote"]);
          oParam.AuditNote = Util.TrimNull(tempdr["AuditNote"]);
          oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
          oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
          oParam.Score = Util.TrimIntNull(tempdr["Score"]);
          oParam.ReviewBackNote = Util.TrimNull(tempdr["ReviewBackNote"]);
          oParam.LastReviewBackUserSysNo = Util.TrimIntNull(tempdr["LastReviewBackUserSysNo"]);
          oParam.LastReviewTime = Util.TrimDateNull(tempdr["LastReviewTime"]);
          oParam.CloseUserSysNo = Util.TrimIntNull(tempdr["CloseUserSysNo"]);
          oParam.CloseTime = Util.TrimDateNull(tempdr["CloseTime"]);
      }
        public  DataSet GetComplainDs(Hashtable paramHash )
        {
            string sql = @" select Complain.*, DateDiff(day,Complain.Createtime,isnull(Complain.CloseTime,getdate()))+1 as handleday,customer.customerID as customerID,customer.customername as customername,sys_user.username as CurrentHandleUserName,
                                 sy.username as createusername,sy1.username as closeusername,sy2.username as auditusername,sys_user.DepartmentSysno
                           from Complain 
                          left join so_master sm on complain.sosysno=sm.sysno
                          left join sys_user on complain.CurrentHandleUserSysNo=sys_user.sysno  
                          left join sys_user sy on complain.createusersysno=sy.sysno
                          left join sys_user sy1 on complain.closeusersysno=sy1.sysno
                          left join sys_user sy2 on complain.AuditUserSysNo=sy2.sysno
                          left join customer on complain.customersysno= customer.sysno   
                          where 1=1 @CreateFrom @CreateTo @SOID  @Status @Type  @CustomerID  
                            @HandleDay @CurrentHandleUserSysno @IsReviewBack @Department @ComplainSysNo @IsAudit @AbnormalCauseType";
            if (paramHash.ContainsKey("CreateFrom"))
                sql = sql.Replace("@CreateFrom", "and Complain.Createtime >=" + Util.ToSqlString(paramHash["CreateFrom"].ToString()));
            else
                sql = sql.Replace("@CreateFrom", "");
            if (paramHash.ContainsKey("CreateTo"))
                sql = sql.Replace("@CreateTo", " and Complain.Createtime <=" + Util.ToSqlEndDate(paramHash["CreateTo"].ToString()));
            else
                sql = sql.Replace("@CreateTo", "");
            if (paramHash.ContainsKey("SOID"))
                sql = sql.Replace("@SOID", "and sm.soid like" + Util.ToSqlLikeString(paramHash["SOID"].ToString()));
            else
                sql = sql.Replace("@SOID", "");
           
            if (paramHash.ContainsKey("Status"))
                sql = sql.Replace("@Status", "and Complain.status =" + Util.ToSqlString(paramHash["Status"].ToString()));
            else
                sql = sql.Replace("@Status", "");
            if (paramHash.ContainsKey("CustomerID"))
                sql = sql.Replace("@CustomerID", "and customer.CustomerID =" + Util.ToSqlString(paramHash["CustomerID"].ToString()));
            else
                sql = sql.Replace("@CustomerID", "");
            if (paramHash.ContainsKey("Type"))
                sql = sql.Replace("@Type", " and Complain.AbnormalType =" + Util.ToSqlString(paramHash["Type"].ToString()));
            else
                sql = sql.Replace("@Type", "");
            if (paramHash.ContainsKey("HandleDay"))
                sql = sql.Replace("@HandleDay", "and DateDiff(day,Complain.Createtime,isnull(Complain.CloseTime,getdate()))+1") + paramHash["HandleDay"].ToString();
            else
                sql = sql.Replace("@HandleDay", "");
            if (paramHash.ContainsKey("CurrentHandleUserSysno"))
                sql = sql.Replace("@CurrentHandleUserSysno", "and complain.CurrentHandleUserSysno= " + Util.ToSqlString(paramHash["CurrentHandleUserSysno"].ToString()));
            else
                sql = sql.Replace("@CurrentHandleUserSysno", "");
            if (paramHash.ContainsKey("IsReviewBack"))
            {
                if (Util.TrimIntNull(paramHash["IsReviewBack"]) == (int)AppEnum.YNStatus.Yes)
                {
                    sql = sql.Replace("@IsReviewBack", "and complain.LastReviewTime is not null");
                }
                else if (Util.TrimIntNull(paramHash["IsReviewBack"]) == (int)AppEnum.YNStatus.No)
                {
                    sql = sql.Replace("@IsReviewBack", "and complain.LastReviewTime is null");
                }
            }
            else
                sql = sql.Replace("@IsReviewBack", "");
            if (paramHash.ContainsKey("IsAudit"))
            {
                if (Util.TrimIntNull(paramHash["IsAudit"]) == (int)AppEnum.YNStatus.Yes)
                {
                    sql = sql.Replace("@IsAudit", "and complain.AuditTime is not null");
                }
                else if (Util.TrimIntNull(paramHash["IsAudit"]) == (int)AppEnum.YNStatus.No)
                {
                    sql = sql.Replace("@IsAudit", "and complain.AuditTime is null");
                }
            }
            else
                sql = sql.Replace("@IsAudit", "");
            if (paramHash.ContainsKey("Department"))
                sql = sql.Replace("@Department", " and sys_user.DepartmentSysno =" + Util.ToSqlString(paramHash["Department"].ToString()));
            else
                sql = sql.Replace("@Department", "");
            if (paramHash.ContainsKey("ComplainSysNo"))
                sql = sql.Replace("@ComplainSysNo", " and Complain.SysNo =" + Util.ToSqlString(paramHash["ComplainSysNo"].ToString()));
            else
                sql = sql.Replace("@ComplainSysNo", "");
            if (paramHash.ContainsKey("AbnormalCauseType"))
                sql = sql.Replace("@AbnormalCauseType", " and Complain.AbnormalCauseType =" + Util.ToSqlString(paramHash["AbnormalCauseType"].ToString()));
            else
                sql = sql.Replace("@AbnormalCauseType", "");

            
            if (paramHash == null || paramHash.Count == 0)
                sql = sql.Replace("select", "select top 50");
            sql += " order by createtime desc";

            return SqlHelper.ExecuteDataSet(sql);
        }

      public DataSet GetComplainDs(int sosysno,int ComplainSysNo)
      {
          string sql = "select complain.* from complain where sosysno=" + sosysno +" and sysno<>"+ComplainSysNo+" order by createtime desc";
          return SqlHelper.ExecuteDataSet(sql);
      }

      public DataSet GetComplainDs(int sosysno)
      {
          string sql = "select complain.* from complain where sosysno=" + sosysno + " order by createtime desc";
          return SqlHelper.ExecuteDataSet(sql);
      }
      public void Insert(ComplainInfo oParam)
      {
          new ComplainDac().Insert(oParam);
      }

      public void Update(ComplainInfo oParam)
      {
          new ComplainDac().Update(oParam);
      }
      public ComplainInfo Load(int sysNo)
      {
          string sql = "select * from Complain where sysno =" + sysNo;
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          if (!Util.HasMoreRow(ds))
              return null;

          ComplainInfo oComplain = new ComplainInfo();
          map(oComplain, ds.Tables[0].Rows[0]);
          return oComplain;
      }
    }
}
