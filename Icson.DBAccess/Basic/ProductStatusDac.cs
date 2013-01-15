using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Objects.Basic;
using Icson.Utils;


namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for ProductStatusDac.
	/// </summary>
	public class ProductStatusDac
	{
		
		public ProductStatusDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public int Insert(ProductStatusInfo oParam)
		{
			string sql = @"INSERT INTO Product_Status
                            (
                            ProductSysNo, InfoStatus, InfoUserSysNo, 
                            InfoTime, PicStatus, PicUserSysNo, PicTime, 
                            WarrantyStatus, WarrantyUserSysNo, WarrantyTime, PriceStatus, 
                            PriceUserSysNo, PriceTime, WeightStatus, WeightUserSysNo, 
                            WeightTime, AllowStatus, AllowUserSysNo, AllowTime,
                            PreviewStatus,PreviewUserSysNo,PreviewTime
                            )
                            VALUES (
                            @ProductSysNo, @InfoStatus, @InfoUserSysNo, 
                            @InfoTime, @PicStatus, @PicUserSysNo, @PicTime, 
                            @WarrantyStatus, @WarrantyUserSysNo, @WarrantyTime, @PriceStatus, 
                            @PriceUserSysNo, @PriceTime, @WeightStatus, @WeightUserSysNo, 
                            @WeightTime, @AllowStatus, @AllowUserSysNo, @AllowTime,
                            @PreviewStatus,@PreviewUserSysNo,@PreviewTime
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramInfoStatus = new SqlParameter("@InfoStatus", SqlDbType.Int,4);
			SqlParameter paramInfoUserSysNo = new SqlParameter("@InfoUserSysNo", SqlDbType.Int,4);
			SqlParameter paramInfoTime = new SqlParameter("@InfoTime", SqlDbType.DateTime);
			SqlParameter paramPicStatus = new SqlParameter("@PicStatus", SqlDbType.Int,4);
			SqlParameter paramPicUserSysNo = new SqlParameter("@PicUserSysNo", SqlDbType.Int,4);
			SqlParameter paramPicTime = new SqlParameter("@PicTime", SqlDbType.DateTime);
			SqlParameter paramWarrantyStatus = new SqlParameter("@WarrantyStatus", SqlDbType.Int,4);
			SqlParameter paramWarrantyUserSysNo = new SqlParameter("@WarrantyUserSysNo", SqlDbType.Int,4);
			SqlParameter paramWarrantyTime = new SqlParameter("@WarrantyTime", SqlDbType.DateTime);
			SqlParameter paramPriceStatus = new SqlParameter("@PriceStatus", SqlDbType.Int,4);
			SqlParameter paramPriceUserSysNo = new SqlParameter("@PriceUserSysNo", SqlDbType.Int,4);
			SqlParameter paramPriceTime = new SqlParameter("@PriceTime", SqlDbType.DateTime);
			SqlParameter paramWeightStatus = new SqlParameter("@WeightStatus", SqlDbType.Int,4);
			SqlParameter paramWeightUserSysNo = new SqlParameter("@WeightUserSysNo", SqlDbType.Int,4);
			SqlParameter paramWeightTime = new SqlParameter("@WeightTime", SqlDbType.DateTime);
			SqlParameter paramAllowStatus = new SqlParameter("@AllowStatus", SqlDbType.Int,4);
			SqlParameter paramAllowUserSysNo = new SqlParameter("@AllowUserSysNo", SqlDbType.Int,4);
			SqlParameter paramAllowTime = new SqlParameter("@AllowTime", SqlDbType.DateTime);
            SqlParameter paramPreviewStatus = new SqlParameter("@PreviewStatus", SqlDbType.Int, 4);
            SqlParameter paramPreviewUserSysNo = new SqlParameter("@PreviewUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramPreviewTime = new SqlParameter("@PreviewTime", SqlDbType.DateTime);

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.InfoStatus != AppConst.IntNull)
				paramInfoStatus.Value = oParam.InfoStatus;
			else
				paramInfoStatus.Value = System.DBNull.Value;
			if ( oParam.InfoUserSysNo != AppConst.IntNull)
				paramInfoUserSysNo.Value = oParam.InfoUserSysNo;
			else
				paramInfoUserSysNo.Value = System.DBNull.Value;
			if ( oParam.InfoTime != AppConst.DateTimeNull)
				paramInfoTime.Value = oParam.InfoTime;
			else
				paramInfoTime.Value = System.DBNull.Value;
			if ( oParam.PicStatus != AppConst.IntNull)
				paramPicStatus.Value = oParam.PicStatus;
			else
				paramPicStatus.Value = System.DBNull.Value;
			if ( oParam.PicUserSysNo != AppConst.IntNull)
				paramPicUserSysNo.Value = oParam.PicUserSysNo;
			else
				paramPicUserSysNo.Value = System.DBNull.Value;
			if ( oParam.PicTime != AppConst.DateTimeNull)
				paramPicTime.Value = oParam.PicTime;
			else
				paramPicTime.Value = System.DBNull.Value;
			if ( oParam.WarrantyStatus != AppConst.IntNull)
				paramWarrantyStatus.Value = oParam.WarrantyStatus;
			else
				paramWarrantyStatus.Value = System.DBNull.Value;
			if ( oParam.WarrantyUserSysNo != AppConst.IntNull)
				paramWarrantyUserSysNo.Value = oParam.WarrantyUserSysNo;
			else
				paramWarrantyUserSysNo.Value = System.DBNull.Value;
			if ( oParam.WarrantyTime != AppConst.DateTimeNull)
				paramWarrantyTime.Value = oParam.WarrantyTime;
			else
				paramWarrantyTime.Value = System.DBNull.Value;
			if ( oParam.PriceStatus != AppConst.IntNull)
				paramPriceStatus.Value = oParam.PriceStatus;
			else
				paramPriceStatus.Value = System.DBNull.Value;
			if ( oParam.PriceUserSysNo != AppConst.IntNull)
				paramPriceUserSysNo.Value = oParam.PriceUserSysNo;
			else
				paramPriceUserSysNo.Value = System.DBNull.Value;
			if ( oParam.PriceTime != AppConst.DateTimeNull)
				paramPriceTime.Value = oParam.PriceTime;
			else
				paramPriceTime.Value = System.DBNull.Value;
			if ( oParam.WeightStatus != AppConst.IntNull)
				paramWeightStatus.Value = oParam.WeightStatus;
			else
				paramWeightStatus.Value = System.DBNull.Value;
			if ( oParam.WeightUserSysNo != AppConst.IntNull)
				paramWeightUserSysNo.Value = oParam.WeightUserSysNo;
			else
				paramWeightUserSysNo.Value = System.DBNull.Value;
			if ( oParam.WeightTime != AppConst.DateTimeNull)
				paramWeightTime.Value = oParam.WeightTime;
			else
				paramWeightTime.Value = System.DBNull.Value;
			if ( oParam.AllowStatus != AppConst.IntNull)
				paramAllowStatus.Value = oParam.AllowStatus;
			else
				paramAllowStatus.Value = System.DBNull.Value;
			if ( oParam.AllowUserSysNo != AppConst.IntNull)
				paramAllowUserSysNo.Value = oParam.AllowUserSysNo;
			else
				paramAllowUserSysNo.Value = System.DBNull.Value;
			if ( oParam.AllowTime != AppConst.DateTimeNull)
				paramAllowTime.Value = oParam.AllowTime;
			else
				paramAllowTime.Value = System.DBNull.Value;
            if (oParam.PreviewStatus != AppConst.IntNull)
                paramPreviewStatus.Value = oParam.PreviewStatus;
            else
                paramPreviewStatus.Value = System.DBNull.Value;
            if (oParam.PreviewUserSysNo != AppConst.IntNull)
                paramPreviewUserSysNo.Value = oParam.PreviewUserSysNo;
            else
                paramPreviewUserSysNo.Value = System.DBNull.Value;
            if (oParam.PreviewTime != AppConst.DateTimeNull)
                paramPreviewTime.Value = oParam.PreviewTime;
            else
                paramPreviewTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramInfoStatus);
			cmd.Parameters.Add(paramInfoUserSysNo);
			cmd.Parameters.Add(paramInfoTime);
			cmd.Parameters.Add(paramPicStatus);
			cmd.Parameters.Add(paramPicUserSysNo);
			cmd.Parameters.Add(paramPicTime);
			cmd.Parameters.Add(paramWarrantyStatus);
			cmd.Parameters.Add(paramWarrantyUserSysNo);
			cmd.Parameters.Add(paramWarrantyTime);
			cmd.Parameters.Add(paramPriceStatus);
			cmd.Parameters.Add(paramPriceUserSysNo);
			cmd.Parameters.Add(paramPriceTime);
			cmd.Parameters.Add(paramWeightStatus);
			cmd.Parameters.Add(paramWeightUserSysNo);
			cmd.Parameters.Add(paramWeightTime);
			cmd.Parameters.Add(paramAllowStatus);
			cmd.Parameters.Add(paramAllowUserSysNo);
			cmd.Parameters.Add(paramAllowTime);
            cmd.Parameters.Add(paramPreviewStatus);
            cmd.Parameters.Add(paramPreviewUserSysNo);
            cmd.Parameters.Add(paramPreviewTime);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(ProductStatusInfo oParam)
		{
            string sql = @"UPDATE Product_Status SET 
                            ProductSysNo=@ProductSysNo, InfoStatus=@InfoStatus, 
                            InfoUserSysNo=@InfoUserSysNo, InfoTime=@InfoTime, 
                            PicStatus=@PicStatus, PicUserSysNo=@PicUserSysNo, 
                            PicTime=@PicTime, WarrantyStatus=@WarrantyStatus, 
                            WarrantyUserSysNo=@WarrantyUserSysNo, WarrantyTime=@WarrantyTime, 
                            PriceStatus=@PriceStatus, PriceUserSysNo=@PriceUserSysNo, 
                            PriceTime=@PriceTime, WeightStatus=@WeightStatus, 
                            WeightUserSysNo=@WeightUserSysNo, WeightTime=@WeightTime, 
                            AllowStatus=@AllowStatus, AllowUserSysNo=@AllowUserSysNo, 
                            AllowTime=@AllowTime, PreviewStatus=@PreviewStatus, 
                            PreviewUserSysNo=@PreviewUserSysNo, PreviewTime=@PreviewTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramInfoStatus = new SqlParameter("@InfoStatus", SqlDbType.Int, 4);
            SqlParameter paramInfoUserSysNo = new SqlParameter("@InfoUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramInfoTime = new SqlParameter("@InfoTime", SqlDbType.DateTime);
            SqlParameter paramPicStatus = new SqlParameter("@PicStatus", SqlDbType.Int, 4);
            SqlParameter paramPicUserSysNo = new SqlParameter("@PicUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramPicTime = new SqlParameter("@PicTime", SqlDbType.DateTime);
            SqlParameter paramWarrantyStatus = new SqlParameter("@WarrantyStatus", SqlDbType.Int, 4);
            SqlParameter paramWarrantyUserSysNo = new SqlParameter("@WarrantyUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramWarrantyTime = new SqlParameter("@WarrantyTime", SqlDbType.DateTime);
            SqlParameter paramPriceStatus = new SqlParameter("@PriceStatus", SqlDbType.Int, 4);
            SqlParameter paramPriceUserSysNo = new SqlParameter("@PriceUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramPriceTime = new SqlParameter("@PriceTime", SqlDbType.DateTime);
            SqlParameter paramWeightStatus = new SqlParameter("@WeightStatus", SqlDbType.Int, 4);
            SqlParameter paramWeightUserSysNo = new SqlParameter("@WeightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramWeightTime = new SqlParameter("@WeightTime", SqlDbType.DateTime);
            SqlParameter paramAllowStatus = new SqlParameter("@AllowStatus", SqlDbType.Int, 4);
            SqlParameter paramAllowUserSysNo = new SqlParameter("@AllowUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAllowTime = new SqlParameter("@AllowTime", SqlDbType.DateTime);
            SqlParameter paramPreviewStatus = new SqlParameter("@PreviewStatus", SqlDbType.Int, 4);
            SqlParameter paramPreviewUserSysNo = new SqlParameter("@PreviewUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramPreviewTime = new SqlParameter("@PreviewTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.InfoStatus != AppConst.IntNull)
                paramInfoStatus.Value = oParam.InfoStatus;
            else
                paramInfoStatus.Value = System.DBNull.Value;
            if (oParam.InfoUserSysNo != AppConst.IntNull)
                paramInfoUserSysNo.Value = oParam.InfoUserSysNo;
            else
                paramInfoUserSysNo.Value = System.DBNull.Value;
            if (oParam.InfoTime != AppConst.DateTimeNull)
                paramInfoTime.Value = oParam.InfoTime;
            else
                paramInfoTime.Value = System.DBNull.Value;
            if (oParam.PicStatus != AppConst.IntNull)
                paramPicStatus.Value = oParam.PicStatus;
            else
                paramPicStatus.Value = System.DBNull.Value;
            if (oParam.PicUserSysNo != AppConst.IntNull)
                paramPicUserSysNo.Value = oParam.PicUserSysNo;
            else
                paramPicUserSysNo.Value = System.DBNull.Value;
            if (oParam.PicTime != AppConst.DateTimeNull)
                paramPicTime.Value = oParam.PicTime;
            else
                paramPicTime.Value = System.DBNull.Value;
            if (oParam.WarrantyStatus != AppConst.IntNull)
                paramWarrantyStatus.Value = oParam.WarrantyStatus;
            else
                paramWarrantyStatus.Value = System.DBNull.Value;
            if (oParam.WarrantyUserSysNo != AppConst.IntNull)
                paramWarrantyUserSysNo.Value = oParam.WarrantyUserSysNo;
            else
                paramWarrantyUserSysNo.Value = System.DBNull.Value;
            if (oParam.WarrantyTime != AppConst.DateTimeNull)
                paramWarrantyTime.Value = oParam.WarrantyTime;
            else
                paramWarrantyTime.Value = System.DBNull.Value;
            if (oParam.PriceStatus != AppConst.IntNull)
                paramPriceStatus.Value = oParam.PriceStatus;
            else
                paramPriceStatus.Value = System.DBNull.Value;
            if (oParam.PriceUserSysNo != AppConst.IntNull)
                paramPriceUserSysNo.Value = oParam.PriceUserSysNo;
            else
                paramPriceUserSysNo.Value = System.DBNull.Value;
            if (oParam.PriceTime != AppConst.DateTimeNull)
                paramPriceTime.Value = oParam.PriceTime;
            else
                paramPriceTime.Value = System.DBNull.Value;
            if (oParam.WeightStatus != AppConst.IntNull)
                paramWeightStatus.Value = oParam.WeightStatus;
            else
                paramWeightStatus.Value = System.DBNull.Value;
            if (oParam.WeightUserSysNo != AppConst.IntNull)
                paramWeightUserSysNo.Value = oParam.WeightUserSysNo;
            else
                paramWeightUserSysNo.Value = System.DBNull.Value;
            if (oParam.WeightTime != AppConst.DateTimeNull)
                paramWeightTime.Value = oParam.WeightTime;
            else
                paramWeightTime.Value = System.DBNull.Value;
            if (oParam.AllowStatus != AppConst.IntNull)
                paramAllowStatus.Value = oParam.AllowStatus;
            else
                paramAllowStatus.Value = System.DBNull.Value;
            if (oParam.AllowUserSysNo != AppConst.IntNull)
                paramAllowUserSysNo.Value = oParam.AllowUserSysNo;
            else
                paramAllowUserSysNo.Value = System.DBNull.Value;
            if (oParam.AllowTime != AppConst.DateTimeNull)
                paramAllowTime.Value = oParam.AllowTime;
            else
                paramAllowTime.Value = System.DBNull.Value;
            if (oParam.PreviewStatus != AppConst.IntNull)
                paramPreviewStatus.Value = oParam.PreviewStatus;
            else
                paramPreviewStatus.Value = System.DBNull.Value;
            if (oParam.PreviewUserSysNo != AppConst.IntNull)
                paramPreviewUserSysNo.Value = oParam.PreviewUserSysNo;
            else
                paramPreviewUserSysNo.Value = System.DBNull.Value;
            if (oParam.PreviewTime != AppConst.DateTimeNull)
                paramPreviewTime.Value = oParam.PreviewTime;
            else
                paramPreviewTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramInfoStatus);
            cmd.Parameters.Add(paramInfoUserSysNo);
            cmd.Parameters.Add(paramInfoTime);
            cmd.Parameters.Add(paramPicStatus);
            cmd.Parameters.Add(paramPicUserSysNo);
            cmd.Parameters.Add(paramPicTime);
            cmd.Parameters.Add(paramWarrantyStatus);
            cmd.Parameters.Add(paramWarrantyUserSysNo);
            cmd.Parameters.Add(paramWarrantyTime);
            cmd.Parameters.Add(paramPriceStatus);
            cmd.Parameters.Add(paramPriceUserSysNo);
            cmd.Parameters.Add(paramPriceTime);
            cmd.Parameters.Add(paramWeightStatus);
            cmd.Parameters.Add(paramWeightUserSysNo);
            cmd.Parameters.Add(paramWeightTime);
            cmd.Parameters.Add(paramAllowStatus);
            cmd.Parameters.Add(paramAllowUserSysNo);
            cmd.Parameters.Add(paramAllowTime);
            cmd.Parameters.Add(paramPreviewStatus);
            cmd.Parameters.Add(paramPreviewUserSysNo);
            cmd.Parameters.Add(paramPreviewTime);

            return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
