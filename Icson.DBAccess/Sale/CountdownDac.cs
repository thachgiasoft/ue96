using System;
using System.Data;
using System.Data.SqlClient;

using System.Text;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
    /// <summary>
    /// Summary description for CountdownDac.
    /// </summary>
    public class CountdownDac
    {

        public CountdownDac()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int Insert(CountdownInfo oParam)
        {
            string sql = @"INSERT INTO Sale_Countdown
                            (
                            CreateUserSysNo, CreateTime, ProductSysNo, 
                            StartTime, EndTime, CountDownCurrentPrice, CountDownCashRebate, 
                            CountDownPoint, CountDownQty, SnapShotCurrentPrice, SnapShotCashRebate, 
                            SnapShotPoint, AffectedVirtualQty, Status, Type
                            )
                            VALUES (
                            @CreateUserSysNo, @CreateTime, @ProductSysNo, 
                            @StartTime, @EndTime, @CountDownCurrentPrice, @CountDownCashRebate, 
                            @CountDownPoint, @CountDownQty, @SnapShotCurrentPrice, @SnapShotCashRebate, 
                            @SnapShotPoint, @AffectedVirtualQty, @Status, @Type
                            );set @SysNo = SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramStartTime = new SqlParameter("@StartTime", SqlDbType.DateTime);
            SqlParameter paramEndTime = new SqlParameter("@EndTime", SqlDbType.DateTime);
            SqlParameter paramCountDownCurrentPrice = new SqlParameter("@CountDownCurrentPrice", SqlDbType.Decimal, 9);
            SqlParameter paramCountDownCashRebate = new SqlParameter("@CountDownCashRebate", SqlDbType.Decimal, 9);
            SqlParameter paramCountDownPoint = new SqlParameter("@CountDownPoint", SqlDbType.Int, 4);
            SqlParameter paramCountDownQty = new SqlParameter("@CountDownQty", SqlDbType.Int, 4);
            SqlParameter paramSnapShotCurrentPrice = new SqlParameter("@SnapShotCurrentPrice", SqlDbType.Decimal, 9);
            SqlParameter paramSnapShotCashRebate = new SqlParameter("@SnapShotCashRebate", SqlDbType.Decimal, 9);
            SqlParameter paramSnapShotPoint = new SqlParameter("@SnapShotPoint", SqlDbType.Int, 4);
            SqlParameter paramAffectedVirtualQty = new SqlParameter("@AffectedVirtualQty", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);

            paramSysNo.Direction = ParameterDirection.Output;

            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.StartTime != AppConst.DateTimeNull)
                paramStartTime.Value = oParam.StartTime;
            else
                paramStartTime.Value = System.DBNull.Value;
            if (oParam.EndTime != AppConst.DateTimeNull)
                paramEndTime.Value = oParam.EndTime;
            else
                paramEndTime.Value = System.DBNull.Value;
            if (oParam.CountDownCurrentPrice != AppConst.DecimalNull)
                paramCountDownCurrentPrice.Value = oParam.CountDownCurrentPrice;
            else
                paramCountDownCurrentPrice.Value = System.DBNull.Value;
            if (oParam.CountDownCashRebate != AppConst.DecimalNull)
                paramCountDownCashRebate.Value = oParam.CountDownCashRebate;
            else
                paramCountDownCashRebate.Value = System.DBNull.Value;
            if (oParam.CountDownPoint != AppConst.IntNull)
                paramCountDownPoint.Value = oParam.CountDownPoint;
            else
                paramCountDownPoint.Value = System.DBNull.Value;
            if (oParam.CountDownQty != AppConst.IntNull)
                paramCountDownQty.Value = oParam.CountDownQty;
            else
                paramCountDownQty.Value = System.DBNull.Value;
            if (oParam.SnapShotCurrentPrice != AppConst.DecimalNull)
                paramSnapShotCurrentPrice.Value = oParam.SnapShotCurrentPrice;
            else
                paramSnapShotCurrentPrice.Value = System.DBNull.Value;
            if (oParam.SnapShotCashRebate != AppConst.DecimalNull)
                paramSnapShotCashRebate.Value = oParam.SnapShotCashRebate;
            else
                paramSnapShotCashRebate.Value = System.DBNull.Value;
            if (oParam.SnapShotPoint != AppConst.IntNull)
                paramSnapShotPoint.Value = oParam.SnapShotPoint;
            else
                paramSnapShotPoint.Value = System.DBNull.Value;
            if (oParam.AffectedVirtualQty != AppConst.IntNull)
                paramAffectedVirtualQty.Value = oParam.AffectedVirtualQty;
            else
                paramAffectedVirtualQty.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.Type != AppConst.IntNull)
                paramType.Value = oParam.Type;
            else
                paramType.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramStartTime);
            cmd.Parameters.Add(paramEndTime);
            cmd.Parameters.Add(paramCountDownCurrentPrice);
            cmd.Parameters.Add(paramCountDownCashRebate);
            cmd.Parameters.Add(paramCountDownPoint);
            cmd.Parameters.Add(paramCountDownQty);
            cmd.Parameters.Add(paramSnapShotCurrentPrice);
            cmd.Parameters.Add(paramSnapShotCashRebate);
            cmd.Parameters.Add(paramSnapShotPoint);
            cmd.Parameters.Add(paramAffectedVirtualQty);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramType);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(CountdownInfo oParam)
        {
            string sql = @"UPDATE Sale_CountDown SET 
                            CreateUserSysNo=@CreateUserSysNo, 
                            CreateTime=@CreateTime, ProductSysNo=@ProductSysNo, 
                            StartTime=@StartTime, EndTime=@EndTime, 
                            CountDownCurrentPrice=@CountDownCurrentPrice, CountDownCashRebate=@CountDownCashRebate, 
                            CountDownPoint=@CountDownPoint, CountDownQty=@CountDownQty, 
                            SnapShotCurrentPrice=@SnapShotCurrentPrice, SnapShotCashRebate=@SnapShotCashRebate, 
                            SnapShotPoint=@SnapShotPoint, AffectedVirtualQty=@AffectedVirtualQty, 
                            Status=@Status, Type=@Type
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramStartTime = new SqlParameter("@StartTime", SqlDbType.DateTime);
            SqlParameter paramEndTime = new SqlParameter("@EndTime", SqlDbType.DateTime);
            SqlParameter paramCountDownCurrentPrice = new SqlParameter("@CountDownCurrentPrice", SqlDbType.Decimal, 9);
            SqlParameter paramCountDownCashRebate = new SqlParameter("@CountDownCashRebate", SqlDbType.Decimal, 9);
            SqlParameter paramCountDownPoint = new SqlParameter("@CountDownPoint", SqlDbType.Int, 4);
            SqlParameter paramCountDownQty = new SqlParameter("@CountDownQty", SqlDbType.Int, 4);
            SqlParameter paramSnapShotCurrentPrice = new SqlParameter("@SnapShotCurrentPrice", SqlDbType.Decimal, 9);
            SqlParameter paramSnapShotCashRebate = new SqlParameter("@SnapShotCashRebate", SqlDbType.Decimal, 9);
            SqlParameter paramSnapShotPoint = new SqlParameter("@SnapShotPoint", SqlDbType.Int, 4);
            SqlParameter paramAffectedVirtualQty = new SqlParameter("@AffectedVirtualQty", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.StartTime != AppConst.DateTimeNull)
                paramStartTime.Value = oParam.StartTime;
            else
                paramStartTime.Value = System.DBNull.Value;
            if (oParam.EndTime != AppConst.DateTimeNull)
                paramEndTime.Value = oParam.EndTime;
            else
                paramEndTime.Value = System.DBNull.Value;
            if (oParam.CountDownCurrentPrice != AppConst.DecimalNull)
                paramCountDownCurrentPrice.Value = oParam.CountDownCurrentPrice;
            else
                paramCountDownCurrentPrice.Value = System.DBNull.Value;
            if (oParam.CountDownCashRebate != AppConst.DecimalNull)
                paramCountDownCashRebate.Value = oParam.CountDownCashRebate;
            else
                paramCountDownCashRebate.Value = System.DBNull.Value;
            if (oParam.CountDownPoint != AppConst.IntNull)
                paramCountDownPoint.Value = oParam.CountDownPoint;
            else
                paramCountDownPoint.Value = System.DBNull.Value;
            if (oParam.CountDownQty != AppConst.IntNull)
                paramCountDownQty.Value = oParam.CountDownQty;
            else
                paramCountDownQty.Value = System.DBNull.Value;
            if (oParam.SnapShotCurrentPrice != AppConst.DecimalNull)
                paramSnapShotCurrentPrice.Value = oParam.SnapShotCurrentPrice;
            else
                paramSnapShotCurrentPrice.Value = System.DBNull.Value;
            if (oParam.SnapShotCashRebate != AppConst.DecimalNull)
                paramSnapShotCashRebate.Value = oParam.SnapShotCashRebate;
            else
                paramSnapShotCashRebate.Value = System.DBNull.Value;
            if (oParam.SnapShotPoint != AppConst.IntNull)
                paramSnapShotPoint.Value = oParam.SnapShotPoint;
            else
                paramSnapShotPoint.Value = System.DBNull.Value;
            if (oParam.AffectedVirtualQty != AppConst.IntNull)
                paramAffectedVirtualQty.Value = oParam.AffectedVirtualQty;
            else
                paramAffectedVirtualQty.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.Type != AppConst.IntNull)
                paramType.Value = oParam.Type;
            else
                paramType.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramStartTime);
            cmd.Parameters.Add(paramEndTime);
            cmd.Parameters.Add(paramCountDownCurrentPrice);
            cmd.Parameters.Add(paramCountDownCashRebate);
            cmd.Parameters.Add(paramCountDownPoint);
            cmd.Parameters.Add(paramCountDownQty);
            cmd.Parameters.Add(paramSnapShotCurrentPrice);
            cmd.Parameters.Add(paramSnapShotCashRebate);
            cmd.Parameters.Add(paramSnapShotPoint);
            cmd.Parameters.Add(paramAffectedVirtualQty);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramType);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE Sale_Countdown SET ");

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
                        if ((int)item == AppConst.IntNull)
                            sb.Append(key).Append("= null");
                        else
                            sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is decimal)
                    {
                        if ((decimal)item == AppConst.DecimalNull)
                            sb.Append(key).Append("= null");
                        else
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

        public int Delete(int SysNo)
        {
            string sql = "delete Sale_CountDown where sysno=" + SysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }
    }
}
