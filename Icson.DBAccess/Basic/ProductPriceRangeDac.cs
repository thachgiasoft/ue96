using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for ProductBasicDac.
	/// </summary>
	public class ProductPriceRangeDac
	{
		
		public ProductPriceRangeDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public int Insert(ProductPriceRangeInfo oParam)
		{
			string sql = @"INSERT INTO product_price_range
                            (
                            RangeID,RangeName,RangeFrom,RangeTo
                            )
                            VALUES (
                            @RangeID,@RangeName,@RangeFrom,@RangeTo
                            )";
		
			SqlCommand cmd = new SqlCommand(sql);
			
			SqlParameter paramRangeID = new SqlParameter("@RangeID", SqlDbType.Int,4);
			SqlParameter paramRangeName = new SqlParameter("@RangeName", SqlDbType.NVarChar,100);
			SqlParameter paramRangeFrom = new SqlParameter("@RangeFrom", SqlDbType.Int,4);
			SqlParameter paramRangeTo = new SqlParameter("@RangeTo", SqlDbType.Int,4);
			
			if(oParam.RangeID != AppConst.IntNull)
				paramRangeID.Value = oParam.RangeID;
			else
				paramRangeID.Value = System.DBNull.Value;

			if(oParam.RangeName != AppConst.StringNull)
				paramRangeName.Value = oParam.RangeName;
			else
				paramRangeName.Value = System.DBNull.Value;

			if(oParam.RangeFrom != AppConst.IntNull)
				paramRangeFrom.Value = oParam.RangeFrom;
			else
				paramRangeFrom.Value = System.DBNull.Value;

			if(oParam.RangeTo != AppConst.IntNull)
				paramRangeTo.Value = oParam.RangeTo;
			else
				paramRangeTo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramRangeID);
			cmd.Parameters.Add(paramRangeName);
			cmd.Parameters.Add(paramRangeFrom);
			cmd.Parameters.Add(paramRangeTo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		
		public int Update(ProductPriceRangeInfo oParam)
		{
			string sql = @"Update product_price_range 
							set RangeID=@RangeID,RangeName=@RangeName,RangeFrom=@RangeFrom,RangeTo=@RangeTo 
							where SysNo=@SysNo";
		
			SqlCommand cmd = new SqlCommand(sql);
			
			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRangeID = new SqlParameter("@RangeID", SqlDbType.Int,4);
			SqlParameter paramRangeName = new SqlParameter("@RangeName", SqlDbType.NVarChar,100);
			SqlParameter paramRangeFrom = new SqlParameter("@RangeFrom", SqlDbType.Int,4);
			SqlParameter paramRangeTo = new SqlParameter("@RangeTo", SqlDbType.Int,4);
			
			if(oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;

			if(oParam.RangeID != AppConst.IntNull)
				paramRangeID.Value = oParam.RangeID;
			else
				paramRangeID.Value = System.DBNull.Value;

			if(oParam.RangeName != AppConst.StringNull)
				paramRangeName.Value = oParam.RangeName;
			else
				paramRangeName.Value = System.DBNull.Value;

			if(oParam.RangeFrom != AppConst.IntNull)
				paramRangeFrom.Value = oParam.RangeFrom;
			else
				paramRangeFrom.Value = System.DBNull.Value;

			if(oParam.RangeTo != AppConst.IntNull)
				paramRangeTo.Value = oParam.RangeTo;
			else
				paramRangeTo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRangeID);
			cmd.Parameters.Add(paramRangeName);
			cmd.Parameters.Add(paramRangeFrom);
			cmd.Parameters.Add(paramRangeTo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
