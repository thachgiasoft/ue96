using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Icson.Utils;
using Icson.Objects.RMA;

namespace Icson.DBAccess.RMA
{
    /// <summary>
    /// Summary description for RMAReturnDac.
    /// </summary>
    public class RMAReturnDac
    {

        public RMAReturnDac()
        {

        }
        public int Insert(RMAReturnInfo oParam)
        {
            string sql = @"INSERT INTO RMA_Return
                            (
                            SysNo, ReturnID, StockSysNo, CreateTime, 
                            CreateUserSysNo, ReturnTime, ReturnUserSysNo, Note, 
                            Status
                            )
                            VALUES (
                            @SysNo, @ReturnID, @StockSysNo, @CreateTime, 
                            @CreateUserSysNo, @ReturnTime, @ReturnUserSysNo, @Note, 
                            @Status
                            )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReturnID = new SqlParameter("@ReturnID", SqlDbType.NVarChar, 10);
            SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramReturnTime = new SqlParameter("@ReturnTime", SqlDbType.DateTime);
            SqlParameter paramReturnUserSysNo = new SqlParameter("@ReturnUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ReturnID != AppConst.StringNull)
                paramReturnID.Value = oParam.ReturnID;
            else
                paramReturnID.Value = System.DBNull.Value;
            if (oParam.StockSysNo != AppConst.IntNull)
                paramStockSysNo.Value = oParam.StockSysNo;
            else
                paramStockSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.ReturnTime != AppConst.DateTimeNull)
                paramReturnTime.Value = oParam.ReturnTime;
            else
                paramReturnTime.Value = System.DBNull.Value;
            if (oParam.ReturnUserSysNo != AppConst.IntNull)
                paramReturnUserSysNo.Value = oParam.ReturnUserSysNo;
            else
                paramReturnUserSysNo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReturnID);
            cmd.Parameters.Add(paramStockSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramReturnTime);
            cmd.Parameters.Add(paramReturnUserSysNo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdateReturn(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE RMA_Return SET ");

            if (paramHash != null && paramHash.Count != 0)
            {
                int index = 0;
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key.ToLower() == "sysno")
                        continue;

                    if (index != 0)
                        sb.Append(",");
                    index++;


                    if (item is int || item is decimal)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (item is DateTime)
                    {
                        sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                    }
                }
            }

            sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }

        public int InsertReturnItem(RMAReturnItemInfo oParam)
        {
            string sql = @"INSERT INTO RMA_Return_Item
                            (
                            ReturnSysNo, RegisterSysNo, TargetProductSysNo , Cost
                            ,ReturnProductType,AuditStatus,AuditUserSysNo,AuditTime,AuditMemo
                            )
                            VALUES (
                            @ReturnSysNo, @RegisterSysNo, @TargetProductSysNo , @Cost
                            ,@ReturnProductType,@AuditStatus,@AuditUserSysNo,@AuditTime,@AuditMemo
                            );set @SysNo = SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReturnSysNo = new SqlParameter("@ReturnSysNo", SqlDbType.Int, 4);
            SqlParameter paramRegisterSysNo = new SqlParameter("@RegisterSysNo", SqlDbType.Int, 4);
            SqlParameter paramTargetProductSysNo = new SqlParameter("@TargetProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramCost = new SqlParameter("@Cost", SqlDbType.Decimal, 9);
            SqlParameter paramReturnProductType = new SqlParameter("@ReturnProductType", SqlDbType.Int, 4);
            SqlParameter paramAuditStatus = new SqlParameter("@AuditStatus", SqlDbType.Int, 4);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramAuditMemo = new SqlParameter("@AuditMemo", SqlDbType.NVarChar, 200);

            paramSysNo.Direction = ParameterDirection.Output;

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ReturnSysNo != AppConst.IntNull)
                paramReturnSysNo.Value = oParam.ReturnSysNo;
            else
                paramReturnSysNo.Value = System.DBNull.Value;
            if (oParam.RegisterSysNo != AppConst.IntNull)
                paramRegisterSysNo.Value = oParam.RegisterSysNo;
            else
                paramRegisterSysNo.Value = System.DBNull.Value;
            if (oParam.TargetProductSysNo != AppConst.IntNull)
                paramTargetProductSysNo.Value = oParam.TargetProductSysNo;
            else
                paramTargetProductSysNo.Value = System.DBNull.Value;

            if (oParam.Cost != AppConst.DecimalNull)
                paramCost.Value = oParam.Cost;
            else
                paramCost.Value = System.DBNull.Value;

            if (oParam.ReturnProductType != AppConst.IntNull) paramReturnProductType.Value = oParam.ReturnProductType;
            else paramReturnProductType.Value = System.DBNull.Value;

            if (oParam.AuditStatus != AppConst.IntNull) paramAuditStatus.Value = oParam.AuditStatus;
            else paramAuditStatus.Value = System.DBNull.Value;

            if (oParam.AuditUserSysNo != AppConst.IntNull) paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else paramAuditUserSysNo.Value = System.DBNull.Value;

            if (oParam.AuditTime != AppConst.DateTimeNull) paramAuditTime.Value = oParam.AuditTime;
            else paramAuditTime.Value = System.DBNull.Value;

            if (oParam.AuditMemo != AppConst.StringNull) paramAuditMemo.Value = oParam.AuditMemo;
            else paramAuditMemo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReturnSysNo);
            cmd.Parameters.Add(paramRegisterSysNo);
            cmd.Parameters.Add(paramTargetProductSysNo);
            cmd.Parameters.Add(paramCost);

            cmd.Parameters.Add(paramReturnProductType);
            cmd.Parameters.Add(paramAuditStatus);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramAuditMemo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }


        public int UpdateReturnItem(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE RMA_Return_Item SET ");

            if (paramHash != null && paramHash.Count != 0)
            {
                int index = 0;
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key.ToLower() == "sysno")
                        continue;

                    if (index != 0)
                        sb.Append(",");
                    index++;

                    if (item is int || item is decimal)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (item is DateTime)
                    {
                        sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                    }
                }
            }

            sb.Append(" WHERE RegisterSysNo=").Append(paramHash["SysNo"]);

            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }

        public int DeleteItem(int sysno)
        {
            string sql = "delete from RMA_Return_Item where sysno = " + sysno;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int CancelReturn(int sysno)
        {
            string sql = "update RMA_Return set ReturnTime = null , ReturnUserSysNo = null where sysno = " + sysno;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        //		public int CancelTarget(int sysno)
        //		{
        //		    string sql = "update RMA_Return_Item set TargetProductSysNo = null where sysno =" + sysno ;
        //			SqlCommand cmd = new SqlCommand(sql);
        //			return SqlHelper.ExecuteNonQuery(cmd);
        //		}

        public int UpdateReturnTarget(Hashtable paramHash)
        {
            //string sql = "update RMA_Return_Item set TargetProductSysNo = @ProductSysNo, Cost=@Cost  where sysno = @SysNo";
            //if(ht.ContainsKey("ProductSysNo"))
            //    sql = sql.Replace("@ProductSysNo" , ht["ProductSysNo"].ToString());
            //else
            //    sql = sql.Replace("@ProductSysNo" , "null");

            //if(ht.ContainsKey("SysNo"))
            //    sql = sql.Replace("@SysNo" , ht["SysNo"].ToString());
            //else
            //    sql = sql.Replace("@SysNo" , "0");

            //if(ht.ContainsKey("Cost"))
            //    sql = sql.Replace("@Cost" , ht["Cost"].ToString());
            //else
            //    sql = sql.Replace("@Cost" , "null");

            //SqlCommand cmd = new SqlCommand(sql);
            //return SqlHelper.ExecuteNonQuery(cmd);

            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE RMA_Return_Item SET ");

            if (paramHash != null && paramHash.Count != 0)
            {
                int index = 0;
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key.ToLower() == "sysno")
                        continue;

                    if (index != 0)
                        sb.Append(",");
                    index++;


                    if (item is int)
                    {
                        if (Util.TrimIntNull(item) == AppConst.IntNull) sb.Append(key).Append("=null");
                        else   sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is decimal)
                    {
                        if (Util.TrimDecimalNull(item) == AppConst.DecimalNull) sb.Append(key).Append("=null");
                        else sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (item is DateTime)
                    {
                        if (Util.TrimDateNull(item) == AppConst.DateTimeNull) sb.Append(key).Append("=null");
                        else  sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                    }
                }
            }

            sb.Append(" WHERE sysNo=").Append(paramHash["SysNo"]);

            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }

    }
}
