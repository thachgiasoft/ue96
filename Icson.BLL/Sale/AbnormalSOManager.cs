using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Sale;
using Icson.DBAccess.Sale;
using Icson.DBAccess;

namespace Icson.BLL.Sale
{
    public class AbnormalSOManager
    {
        private static AbnormalSOManager _instance;
        public static AbnormalSOManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AbnormalSOManager();
            }
            return _instance;
        }


        private void map(AbnormalSOInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.Description = Util.TrimNull(tempdr["Description"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.Type = Util.TrimIntNull(tempdr["Type"]);
            oParam.Createtime = Util.TrimDateNull(tempdr["Createtime"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
            oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
            oParam.CloseTime = Util.TrimDateNull(tempdr["CloseTime"]);
            oParam.CloseUserSysNo = Util.TrimIntNull(tempdr["CloseUserSysNo"]);
        }

        public void Insert(AbnormalSOInfo oInfo)
        {
            new AbnormalSODac().Insert(oInfo);
        }

        public void Update(AbnormalSOInfo oInfo)
        {
            new AbnormalSODac().Update(oInfo);
        }

        public AbnormalSOInfo Load(int sysno)
        {
            string sql = "select * from AbnormalSO where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            AbnormalSOInfo oInfo = new AbnormalSOInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }



        public bool CheckExist(int sosysno)
        {
            string sql = "select * from AbnormalSO where sosysno =" + sosysno + " and Status =" + (int)AppEnum.AbnormalSOStatus.Handling;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count > 0)
                return true;

            else
                return false;
        }


        public DataSet GetAbnormalSODs(Hashtable paramHash)
        {
            string sql = @"select distinct AbnormalSO.*,so_master.soid,su1.username as createusername,su2.username as closeusername from AbnormalSO 
                         left join so_master on so_master.sysno=AbnormalSO.sosysno 
                         left join sys_user su1 on AbnormalSO.CreateUserSysNo=su1.sysno
                         left join sys_user su2 on AbnormalSO.CloseUserSysNo=su2.sysno
                        where 1=1 ";

            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "CreateTimeFrom")
                    {
                        sb.Append("AbnormalSO.CreateTime").Append(" >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "CreateTimeTo")
                    {
                        sb.Append("AbnormalSO.CreateTime").Append(" <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "SOID")
                    {
                        sb.Append("SO_Master.soid").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }

                    else if (key == "Status")
                    {
                        sb.Append("AbnormalSO.Status").Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }

                }
                sql += sb.ToString();
            }
            else
            {
                sql = sql.Replace("select distinct", "select distinct top 50 ");
            }

            sql += "order by AbnormalSO.sysno ";

            return SqlHelper.ExecuteDataSet(sql);

        }


        public DataSet GetAbnormalSODs(int sosysno)
        {
            string sql = "select * from AbnormalSO where sosysno =" + sosysno + " and Status =" + (int)AppEnum.AbnormalSOStatus.Handling;
            return SqlHelper.ExecuteDataSet(sql);
        }
    }
}
