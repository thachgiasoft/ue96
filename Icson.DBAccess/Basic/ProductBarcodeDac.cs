using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// ProductBarcodeDac 的摘要说明。
	/// </summary>
	public class ProductBarcodeDac
	{
		
		public ProductBarcodeDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public int Insert(ProductBarcodeInfo oParam)
		{
			string sql = @"INSERT INTO product_barcode(ProductSysNo,Barcode) VALUES(@ProductSysNo, @Barcode)";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramBarcode = new SqlParameter("@Barcode",SqlDbType.NVarChar,50);

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;

			if( oParam.Barcode != AppConst.StringNull)
				paramBarcode.Value = oParam.Barcode;
			else
				paramBarcode.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramBarcode);
			
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(ProductBarcodeInfo oParam)
		{
			string sql = @"UPDATE product_barcode SET 
                            ProductSysNo=@ProductSysNo, Barcode=@Barcode, DateStamp=@DateStamp 
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo",SqlDbType.Int,4);
			SqlParameter paramBarcode = new SqlParameter("@Barcode",SqlDbType.NVarChar,50);
			SqlParameter paramDateStamp = new SqlParameter("@DateStamp",SqlDbType.DateTime,8);

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;

			if( oParam.Barcode != AppConst.StringNull)
				paramBarcode.Value = oParam.Barcode;
			else
				paramBarcode.Value = System.DBNull.Value;

			if( oParam.DateStamp != AppConst.DateTimeNull)
				paramDateStamp.Value = oParam.DateStamp;
			else
				paramBarcode.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramBarcode);
			cmd.Parameters.Add(paramDateStamp);

			return SqlHelper.ExecuteNonQuery(cmd);
		}		
	}
}