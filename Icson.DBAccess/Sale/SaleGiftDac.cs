using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
	/// <summary>
	/// Summary description for SaleGiftDac.
	/// </summary>
	public class SaleGiftDac
	{
		public SaleGiftDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		
		
		public int Insert(SaleGiftInfo oParam)
		{
			string sql = @"INSERT INTO Sale_Gift
                           (
                           ProductSysNo, GiftSysNo, ListOrder, 
                           CreateUserSysNo)
                           VALUES (
                           @ProductSysNo, @GiftSysNo, @ListOrder, 
                           @CreateUserSysNo)";
			
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramGiftSysNo = new SqlParameter("@GiftSysNo", SqlDbType.Int,4);
			SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.NVarChar,10);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.GiftSysNo != AppConst.IntNull)
				paramGiftSysNo.Value = oParam.GiftSysNo;
			else
				paramGiftSysNo.Value = System.DBNull.Value;
			if ( oParam.ListOrder != AppConst.StringNull)
				paramListOrder.Value = oParam.ListOrder;
			else
				paramListOrder.Value = System.DBNull.Value;
			if ( oParam.CreateUserSysNo != AppConst.IntNull)
				paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			else
				paramCreateUserSysNo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramGiftSysNo);
			cmd.Parameters.Add(paramListOrder);
			cmd.Parameters.Add(paramCreateUserSysNo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        //更新赠品信息，不仅仅是作废
        public int UpdateGift(SaleGiftInfo oParam)
        {
            string sql = @"UPDATE Sale_Gift SET 
                           ProductSysNo=@ProductSysNo,GiftSysNo=@GiftSysNo, 
                           ListOrder=@ListOrder,CreateUserSysNo=@CreateUserSysNo
                           WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramGiftSysNo = new SqlParameter("@GiftSysNo", SqlDbType.Int, 4);
            SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.NVarChar, 10);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.GiftSysNo != AppConst.IntNull)
                paramGiftSysNo.Value = oParam.GiftSysNo;
            else
                paramGiftSysNo.Value = System.DBNull.Value;
            if (oParam.ListOrder != AppConst.StringNull)
                paramListOrder.Value = oParam.ListOrder;
            else
                paramListOrder.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramGiftSysNo);
            cmd.Parameters.Add(paramListOrder);
            cmd.Parameters.Add(paramCreateUserSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

		public int Update(SaleGiftInfo oParam)
		{
			string sql = @"UPDATE Sale_Gift SET 
                           Status=@Status,AbandonUserSysNo=@AbandonUserSysNo, 
                           AbandonTime=@AbandonTime
                           WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAbandonUserSysNo = new SqlParameter("@AbandonUserSysNo", SqlDbType.Int,4);
			SqlParameter paramAbandonTime = new SqlParameter("@AbandonTime", SqlDbType.DateTime);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;
			if ( oParam.AbandonUserSysNo != AppConst.IntNull)
				paramAbandonUserSysNo.Value = oParam.AbandonUserSysNo;
			else
				paramAbandonUserSysNo.Value = System.DBNull.Value;
			if ( oParam.AbandonTime != AppConst.DateTimeNull)
				paramAbandonTime.Value = oParam.AbandonTime;
			else
				paramAbandonTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAbandonUserSysNo);
			cmd.Parameters.Add(paramAbandonTime);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int SetInValid(string sysNos, int userSysNo, int status)
        {
            string sql = "update Sale_Gift set Status=" + status.ToString() + ",AbandonUserSysNo=" + userSysNo.ToString() + ",AbandonTime=getdate() where SysNo in(" + sysNos + ")";
            return SqlHelper.ExecuteNonQuery(sql);
        }

	}
}
