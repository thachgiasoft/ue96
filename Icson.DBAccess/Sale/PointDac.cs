using System;
using System.Data;
using System.Data.SqlClient;

using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
	/// <summary>
	/// 处理CustomerPointLogInfo, SalePointDelayInfo
	/// 还有customer中的totalscore 和 validscore
	/// </summary>
	public class PointDac
	{
		
		public PointDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public int InsertLog(CustomerPointLogInfo oParam)
		{
			string sql = @"INSERT INTO Customer_PointLog
                            (
                            CustomerSysNo, PointLogType, PointAmount, 
                            CreateTime, Memo, LogCheck
                            )
                            VALUES (
                            @CustomerSysNo, @PointLogType, @PointAmount, 
                            @CreateTime, @Memo, @LogCheck
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramPointLogType = new SqlParameter("@PointLogType", SqlDbType.Int,4);
			SqlParameter paramPointAmount = new SqlParameter("@PointAmount", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar,200);
			SqlParameter paramLogCheck = new SqlParameter("@LogCheck", SqlDbType.NVarChar,200);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.CustomerSysNo != AppConst.IntNull)
				paramCustomerSysNo.Value = oParam.CustomerSysNo;
			else
				paramCustomerSysNo.Value = System.DBNull.Value;
			if ( oParam.PointLogType != AppConst.IntNull)
				paramPointLogType.Value = oParam.PointLogType;
			else
				paramPointLogType.Value = System.DBNull.Value;
			if ( oParam.PointAmount != AppConst.IntNull)
				paramPointAmount.Value = oParam.PointAmount;
			else
				paramPointAmount.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.Memo != AppConst.StringNull)
				paramMemo.Value = oParam.Memo;
			else
				paramMemo.Value = System.DBNull.Value;
			if ( oParam.LogCheck != AppConst.StringNull)
				paramLogCheck.Value = oParam.LogCheck;
			else
				paramLogCheck.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramPointLogType);
			cmd.Parameters.Add(paramPointAmount);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramMemo);
			cmd.Parameters.Add(paramLogCheck);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}
		//只有在导入数据的时候才用
		public int DeleteLog()
		{
			if ( !AppConfig.IsImportable)
				return -100;
			string sql = "delete from ipp2003..pointlog where amount = 0";
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int SetScore(int customerSysNo, int increment)
		{
			string sql = @"update customer set 
						   validScore = validScore + @increment";
			if(increment>0)
				sql += " ,totalScore =  totalScore + @increment";
			sql += " where sysno=@SysNo and validScore + @increment>=0";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo",SqlDbType.Int,4);
			SqlParameter paramIncrement = new SqlParameter("@Increment",SqlDbType.Int,4);

			paramSysNo.Value = customerSysNo;
			paramIncrement.Value = increment;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramIncrement);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int InsertPointDelay(SalePointDelayInfo oParam)
		{
			string sql = @"INSERT INTO Sale_PointDelay
                            (
                            SOSysNo, CreateTime, Status
                            )
                            VALUES (
                            @SOSysNo, @CreateTime, @Status
                            )";
			
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdatePointDelay(SalePointDelayInfo oParam)
		{
			string sql = @"UPDATE Sale_PointDelay SET 
                           Status=@Status
                           WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		//---------2006-07-27-------------
//		public int AddScore(int CustomerSysNo,int AddTotalScore,int AddValidScore)
//		{
//			string sql = @"UPDATE Customer SET TotalScore = TotalScore+@AddTotalScore,ValidScore=ValidScore+@AddValidScore WHERE SysNo=@CustomerSysNo";
//
//			SqlCommand cmd = new SqlCommand(sql);
//
//			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
//			SqlParameter paramAddTotalScore = new SqlParameter("@AddTotalScore", SqlDbType.Int,4);
//			SqlParameter paramAddValidScore = new SqlParameter("@AddValidScore", SqlDbType.Int,4);
//
//			if ( CustomerSysNo != AppConst.IntNull)
//				paramCustomerSysNo.Value = CustomerSysNo;
//			else
//				paramCustomerSysNo.Value = System.DBNull.Value;
//
//			if ( AddTotalScore != AppConst.IntNull)
//				paramAddTotalScore.Value = AddTotalScore;
//			else
//				paramAddTotalScore.Value = System.DBNull.Value;
//
//			if ( AddTotalScore != AppConst.IntNull)
//				paramAddValidScore.Value = AddValidScore;
//			else
//				paramAddValidScore.Value = System.DBNull.Value;
//
//			cmd.Parameters.Add(paramCustomerSysNo);
//			cmd.Parameters.Add(paramAddTotalScore);
//			cmd.Parameters.Add(paramAddValidScore);
//
//			return SqlHelper.ExecuteNonQuery(cmd);
//		}

        public int InsertCustomerPointRequest(CustomerPointRequestInfo oParam)
        {
            string sql = @"INSERT INTO Customer_PointRequest
                            (
                            CustomerSysNo, PointSourceType, PointSourceSysNo, PointLogType, 
                            PointAmount, RequestUserType, RequestUserSysNo, RequestTime, 
                            AuditUserSysNo, AuditTime, AddUserSysNo, AddTime, 
                            AbandonUserSysNo, AbandonTime, Memo, Status,PMUserSysNo
                            )
                            VALUES (
                            @CustomerSysNo, @PointSourceType, @PointSourceSysNo, @PointLogType, 
                            @PointAmount, @RequestUserType, @RequestUserSysNo, @RequestTime, 
                            @AuditUserSysNo, @AuditTime, @AddUserSysNo, @AddTime, 
                            @AbandonUserSysNo, @AbandonTime, @Memo, @Status,@PMUserSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramPointSourceType = new SqlParameter("@PointSourceType", SqlDbType.Int, 4);
            SqlParameter paramPointSourceSysNo = new SqlParameter("@PointSourceSysNo", SqlDbType.Int, 4);
            SqlParameter paramPointLogType = new SqlParameter("@PointLogType", SqlDbType.Int, 4);
            SqlParameter paramPointAmount = new SqlParameter("@PointAmount", SqlDbType.Int, 4);
            SqlParameter paramRequestUserType = new SqlParameter("@RequestUserType", SqlDbType.Int, 4);
            SqlParameter paramRequestUserSysNo = new SqlParameter("@RequestUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestTime = new SqlParameter("@RequestTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramAddUserSysNo = new SqlParameter("@AddUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAddTime = new SqlParameter("@AddTime", SqlDbType.DateTime);
            SqlParameter paramAbandonUserSysNo = new SqlParameter("@AbandonUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAbandonTime = new SqlParameter("@AbandonTime", SqlDbType.DateTime);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramPMUserSysNo = new SqlParameter("@PMUserSysNo", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.PointSourceType != AppConst.IntNull)
                paramPointSourceType.Value = oParam.PointSourceType;
            else
                paramPointSourceType.Value = System.DBNull.Value;
            if (oParam.PointSourceSysNo != AppConst.IntNull)
                paramPointSourceSysNo.Value = oParam.PointSourceSysNo;
            else
                paramPointSourceSysNo.Value = System.DBNull.Value;
            if (oParam.PointLogType != AppConst.IntNull)
                paramPointLogType.Value = oParam.PointLogType;
            else
                paramPointLogType.Value = System.DBNull.Value;
            if (oParam.PointAmount != AppConst.IntNull)
                paramPointAmount.Value = oParam.PointAmount;
            else
                paramPointAmount.Value = System.DBNull.Value;
            if (oParam.RequestUserType != AppConst.IntNull)
                paramRequestUserType.Value = oParam.RequestUserType;
            else
                paramRequestUserType.Value = System.DBNull.Value;
            if (oParam.RequestUserSysNo != AppConst.IntNull)
                paramRequestUserSysNo.Value = oParam.RequestUserSysNo;
            else
                paramRequestUserSysNo.Value = System.DBNull.Value;
            if (oParam.RequestTime != AppConst.DateTimeNull)
                paramRequestTime.Value = oParam.RequestTime;
            else
                paramRequestTime.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.AddUserSysNo != AppConst.IntNull)
                paramAddUserSysNo.Value = oParam.AddUserSysNo;
            else
                paramAddUserSysNo.Value = System.DBNull.Value;
            if (oParam.AddTime != AppConst.DateTimeNull)
                paramAddTime.Value = oParam.AddTime;
            else
                paramAddTime.Value = System.DBNull.Value;
            if (oParam.AbandonUserSysNo != AppConst.IntNull)
                paramAbandonUserSysNo.Value = oParam.AbandonUserSysNo;
            else
                paramAbandonUserSysNo.Value = System.DBNull.Value;
            if (oParam.AbandonTime != AppConst.DateTimeNull)
                paramAbandonTime.Value = oParam.AbandonTime;
            else
                paramAbandonTime.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.PMUserSysNo != AppConst.IntNull)
                paramPMUserSysNo.Value = oParam.PMUserSysNo;
            else
                paramPMUserSysNo.Value = System.DBNull.Value;
            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramPointSourceType);
            cmd.Parameters.Add(paramPointSourceSysNo);
            cmd.Parameters.Add(paramPointLogType);
            cmd.Parameters.Add(paramPointAmount);
            cmd.Parameters.Add(paramRequestUserType);
            cmd.Parameters.Add(paramRequestUserSysNo);
            cmd.Parameters.Add(paramRequestTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramAddUserSysNo);
            cmd.Parameters.Add(paramAddTime);
            cmd.Parameters.Add(paramAbandonUserSysNo);
            cmd.Parameters.Add(paramAbandonTime);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramPMUserSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int UpdateCustomerPointRequest(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE Customer_PointRequest SET ");

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
	}
}