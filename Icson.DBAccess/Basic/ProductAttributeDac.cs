using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for ProductAttributeDac.
	/// </summary>
	public class ProductAttributeDac
	{
		
		public ProductAttributeDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int Insert(ProductAttribute2Info oParam)
		{
			string sql = @"INSERT INTO product_attribute2(ProductSysNo,Attribute2SysNo,Attribute2OptionSysNo,Attribute2Value) 
						  VALUES(@ProductSysNo,@Attribute2SysNo,@Attribute2OptionSysNo,@Attribute2Value)";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2SysNo = new SqlParameter("@Attribute2SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2OptionSysNo = new SqlParameter("@Attribute2OptionSysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2Value = new SqlParameter("@Attribute2Value", SqlDbType.NVarChar,2000);

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;

			if( oParam.Attribute2SysNo != AppConst.IntNull)
				paramAttribute2SysNo.Value = oParam.Attribute2SysNo;
			else
				paramAttribute2SysNo.Value = System.DBNull.Value;
			
			if( oParam.Attribute2OptionSysNo != AppConst.IntNull)
				paramAttribute2OptionSysNo.Value = oParam.Attribute2OptionSysNo;
			else
				paramAttribute2OptionSysNo.Value = System.DBNull.Value;
			
			if( oParam.Attribute2Value !=  AppConst.StringNull)
				paramAttribute2Value.Value = oParam.Attribute2Value;
			else
				paramAttribute2Value.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramAttribute2SysNo);
			cmd.Parameters.Add(paramAttribute2OptionSysNo);
			cmd.Parameters.Add(paramAttribute2Value);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(ProductAttribute2Info oParam)
		{
			string sql = @"Update product_attribute2 set Attribute2OptionSysNo=@Attribute2OptionSysNo,Attribute2Value=@Attribute2Value 
						   Where ProductSysNo=@ProductSysNo and Attribute2SysNo=@Attribute2SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2SysNo = new SqlParameter("@Attribute2SysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2OptionSysNo = new SqlParameter("@Attribute2OptionSysNo", SqlDbType.Int,4);
			SqlParameter paramAttribute2Value = new SqlParameter("@Attribute2Value", SqlDbType.NVarChar,2000);

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;

			if( oParam.Attribute2SysNo != AppConst.IntNull)
				paramAttribute2SysNo.Value = oParam.Attribute2SysNo;
			else
				paramAttribute2SysNo.Value = System.DBNull.Value;
			
			if( oParam.Attribute2OptionSysNo != AppConst.IntNull)
				paramAttribute2OptionSysNo.Value = oParam.Attribute2OptionSysNo;
			else
				paramAttribute2OptionSysNo.Value = System.DBNull.Value;
			
			if( oParam.Attribute2Value !=  AppConst.StringNull)
				paramAttribute2Value.Value = oParam.Attribute2Value;
			else
				paramAttribute2Value.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramAttribute2SysNo);
			cmd.Parameters.Add(paramAttribute2OptionSysNo);
			cmd.Parameters.Add(paramAttribute2Value);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int DeleteProductAttribute2(int ProductSysNo,int Attribute2SysNo)
        {
            string sql = "delete product_attribute2 where productsysno=" + ProductSysNo + " and attribute2sysno=" + Attribute2SysNo;
            return SqlHelper.ExecuteNonQuery(sql);
        }

	    public int Insert(ProductAttribute2Summary oParam)
		{
			string sql = "INSERT INTO product_attribute2_summary(productsysno,summary,summarymain) values(@ProductSysNo,@Summary,@SummaryMain)";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo",SqlDbType.Int,4);
			SqlParameter paramSummary = new SqlParameter("@Summary",SqlDbType.NText);
            SqlParameter paramSummaryMain = new SqlParameter("@SummaryMain", SqlDbType.NText);

			if(oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;

			if(oParam.Summary != AppConst.StringNull)
				paramSummary.Value = oParam.Summary;
			else
				paramSummary.Value = System.DBNull.Value;

            if (oParam.SummaryMain != AppConst.StringNull)
                paramSummaryMain.Value = oParam.SummaryMain;
            else
                paramSummaryMain.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramSummary);
	        cmd.Parameters.Add(paramSummaryMain);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(ProductAttribute2Summary oParam)
		{
			string sql = "UPDATE product_attribute2_summary set summary=@Summary,summarymain=@SummaryMain where productsysno=@ProductSysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo",SqlDbType.Int,4);
			SqlParameter paramSummary = new SqlParameter("@Summary",SqlDbType.NText);
            SqlParameter paramSummaryMain = new SqlParameter("@SummaryMain", SqlDbType.NText);

			if(oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;

			if(oParam.Summary != AppConst.StringNull)
				paramSummary.Value = oParam.Summary;
			else
				paramSummary.Value = System.DBNull.Value;

            if (oParam.SummaryMain != AppConst.StringNull)
                paramSummaryMain.Value = oParam.SummaryMain;
            else
                paramSummaryMain.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramSummary);
            cmd.Parameters.Add(paramSummaryMain);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Insert(ProductAttributeInfo oParam)
		{
			string sql = @"INSERT INTO product_attribute
                            (
                            ProductSysNo, A1, A2, 
                            A3, A4, A5, A6, 
                            A7, A8, A9, A10, 
                            A11, A12, A13, A14, 
                            A15, A16, A17, A18, 
                            A19, A20, A21, A22, 
                            A23, A24, A25, A26, 
                            A27, A28, A29, A30
                            )
                            VALUES (
                            @ProductSysNo, @A1, @A2, 
                            @A3, @A4, @A5, @A6, 
                            @A7, @A8, @A9, @A10, 
                            @A11, @A12, @A13, @A14, 
                            @A15, @A16, @A17, @A18, 
                            @A19, @A20, @A21, @A22, 
                            @A23, @A24, @A25, @A26, 
                            @A27, @A28, @A29, @A30
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramA1 = new SqlParameter("@A1", SqlDbType.NVarChar,200);
			SqlParameter paramA2 = new SqlParameter("@A2", SqlDbType.NVarChar,200);
			SqlParameter paramA3 = new SqlParameter("@A3", SqlDbType.NVarChar,200);
			SqlParameter paramA4 = new SqlParameter("@A4", SqlDbType.NVarChar,200);
			SqlParameter paramA5 = new SqlParameter("@A5", SqlDbType.NVarChar,200);
			SqlParameter paramA6 = new SqlParameter("@A6", SqlDbType.NVarChar,200);
			SqlParameter paramA7 = new SqlParameter("@A7", SqlDbType.NVarChar,200);
			SqlParameter paramA8 = new SqlParameter("@A8", SqlDbType.NVarChar,200);
			SqlParameter paramA9 = new SqlParameter("@A9", SqlDbType.NVarChar,200);
			SqlParameter paramA10 = new SqlParameter("@A10", SqlDbType.NVarChar,200);
			SqlParameter paramA11 = new SqlParameter("@A11", SqlDbType.NVarChar,200);
			SqlParameter paramA12 = new SqlParameter("@A12", SqlDbType.NVarChar,200);
			SqlParameter paramA13 = new SqlParameter("@A13", SqlDbType.NVarChar,200);
			SqlParameter paramA14 = new SqlParameter("@A14", SqlDbType.NVarChar,200);
			SqlParameter paramA15 = new SqlParameter("@A15", SqlDbType.NVarChar,200);
			SqlParameter paramA16 = new SqlParameter("@A16", SqlDbType.NVarChar,200);
			SqlParameter paramA17 = new SqlParameter("@A17", SqlDbType.NVarChar,200);
			SqlParameter paramA18 = new SqlParameter("@A18", SqlDbType.NVarChar,200);
			SqlParameter paramA19 = new SqlParameter("@A19", SqlDbType.NVarChar,200);
			SqlParameter paramA20 = new SqlParameter("@A20", SqlDbType.NVarChar,200);
			SqlParameter paramA21 = new SqlParameter("@A21", SqlDbType.NVarChar,200);
			SqlParameter paramA22 = new SqlParameter("@A22", SqlDbType.NVarChar,200);
			SqlParameter paramA23 = new SqlParameter("@A23", SqlDbType.NVarChar,200);
			SqlParameter paramA24 = new SqlParameter("@A24", SqlDbType.NVarChar,200);
			SqlParameter paramA25 = new SqlParameter("@A25", SqlDbType.NVarChar,200);
			SqlParameter paramA26 = new SqlParameter("@A26", SqlDbType.NVarChar,200);
			SqlParameter paramA27 = new SqlParameter("@A27", SqlDbType.NVarChar,200);
			SqlParameter paramA28 = new SqlParameter("@A28", SqlDbType.NVarChar,200);
			SqlParameter paramA29 = new SqlParameter("@A29", SqlDbType.NVarChar,200);
			SqlParameter paramA30 = new SqlParameter("@A30", SqlDbType.NVarChar,200);

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.A1 != AppConst.StringNull)
				paramA1.Value = oParam.A1;
			else
				paramA1.Value = System.DBNull.Value;
			if ( oParam.A2 != AppConst.StringNull)
				paramA2.Value = oParam.A2;
			else
				paramA2.Value = System.DBNull.Value;
			if ( oParam.A3 != AppConst.StringNull)
				paramA3.Value = oParam.A3;
			else
				paramA3.Value = System.DBNull.Value;
			if ( oParam.A4 != AppConst.StringNull)
				paramA4.Value = oParam.A4;
			else
				paramA4.Value = System.DBNull.Value;
			if ( oParam.A5 != AppConst.StringNull)
				paramA5.Value = oParam.A5;
			else
				paramA5.Value = System.DBNull.Value;
			if ( oParam.A6 != AppConst.StringNull)
				paramA6.Value = oParam.A6;
			else
				paramA6.Value = System.DBNull.Value;
			if ( oParam.A7 != AppConst.StringNull)
				paramA7.Value = oParam.A7;
			else
				paramA7.Value = System.DBNull.Value;
			if ( oParam.A8 != AppConst.StringNull)
				paramA8.Value = oParam.A8;
			else
				paramA8.Value = System.DBNull.Value;
			if ( oParam.A9 != AppConst.StringNull)
				paramA9.Value = oParam.A9;
			else
				paramA9.Value = System.DBNull.Value;
			if ( oParam.A10 != AppConst.StringNull)
				paramA10.Value = oParam.A10;
			else
				paramA10.Value = System.DBNull.Value;
			if ( oParam.A11 != AppConst.StringNull)
				paramA11.Value = oParam.A11;
			else
				paramA11.Value = System.DBNull.Value;
			if ( oParam.A12 != AppConst.StringNull)
				paramA12.Value = oParam.A12;
			else
				paramA12.Value = System.DBNull.Value;
			if ( oParam.A13 != AppConst.StringNull)
				paramA13.Value = oParam.A13;
			else
				paramA13.Value = System.DBNull.Value;
			if ( oParam.A14 != AppConst.StringNull)
				paramA14.Value = oParam.A14;
			else
				paramA14.Value = System.DBNull.Value;
			if ( oParam.A15 != AppConst.StringNull)
				paramA15.Value = oParam.A15;
			else
				paramA15.Value = System.DBNull.Value;
			if ( oParam.A16 != AppConst.StringNull)
				paramA16.Value = oParam.A16;
			else
				paramA16.Value = System.DBNull.Value;
			if ( oParam.A17 != AppConst.StringNull)
				paramA17.Value = oParam.A17;
			else
				paramA17.Value = System.DBNull.Value;
			if ( oParam.A18 != AppConst.StringNull)
				paramA18.Value = oParam.A18;
			else
				paramA18.Value = System.DBNull.Value;
			if ( oParam.A19 != AppConst.StringNull)
				paramA19.Value = oParam.A19;
			else
				paramA19.Value = System.DBNull.Value;
			if ( oParam.A20 != AppConst.StringNull)
				paramA20.Value = oParam.A20;
			else
				paramA20.Value = System.DBNull.Value;
			if ( oParam.A21 != AppConst.StringNull)
				paramA21.Value = oParam.A21;
			else
				paramA21.Value = System.DBNull.Value;
			if ( oParam.A22 != AppConst.StringNull)
				paramA22.Value = oParam.A22;
			else
				paramA22.Value = System.DBNull.Value;
			if ( oParam.A23 != AppConst.StringNull)
				paramA23.Value = oParam.A23;
			else
				paramA23.Value = System.DBNull.Value;
			if ( oParam.A24 != AppConst.StringNull)
				paramA24.Value = oParam.A24;
			else
				paramA24.Value = System.DBNull.Value;
			if ( oParam.A25 != AppConst.StringNull)
				paramA25.Value = oParam.A25;
			else
				paramA25.Value = System.DBNull.Value;
			if ( oParam.A26 != AppConst.StringNull)
				paramA26.Value = oParam.A26;
			else
				paramA26.Value = System.DBNull.Value;
			if ( oParam.A27 != AppConst.StringNull)
				paramA27.Value = oParam.A27;
			else
				paramA27.Value = System.DBNull.Value;
			if ( oParam.A28 != AppConst.StringNull)
				paramA28.Value = oParam.A28;
			else
				paramA28.Value = System.DBNull.Value;
			if ( oParam.A29 != AppConst.StringNull)
				paramA29.Value = oParam.A29;
			else
				paramA29.Value = System.DBNull.Value;
			if ( oParam.A30 != AppConst.StringNull)
				paramA30.Value = oParam.A30;
			else
				paramA30.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramA1);
			cmd.Parameters.Add(paramA2);
			cmd.Parameters.Add(paramA3);
			cmd.Parameters.Add(paramA4);
			cmd.Parameters.Add(paramA5);
			cmd.Parameters.Add(paramA6);
			cmd.Parameters.Add(paramA7);
			cmd.Parameters.Add(paramA8);
			cmd.Parameters.Add(paramA9);
			cmd.Parameters.Add(paramA10);
			cmd.Parameters.Add(paramA11);
			cmd.Parameters.Add(paramA12);
			cmd.Parameters.Add(paramA13);
			cmd.Parameters.Add(paramA14);
			cmd.Parameters.Add(paramA15);
			cmd.Parameters.Add(paramA16);
			cmd.Parameters.Add(paramA17);
			cmd.Parameters.Add(paramA18);
			cmd.Parameters.Add(paramA19);
			cmd.Parameters.Add(paramA20);
			cmd.Parameters.Add(paramA21);
			cmd.Parameters.Add(paramA22);
			cmd.Parameters.Add(paramA23);
			cmd.Parameters.Add(paramA24);
			cmd.Parameters.Add(paramA25);
			cmd.Parameters.Add(paramA26);
			cmd.Parameters.Add(paramA27);
			cmd.Parameters.Add(paramA28);
			cmd.Parameters.Add(paramA29);
			cmd.Parameters.Add(paramA30);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int Update(ProductAttributeInfo oParam)
		{
			string sql = @"UPDATE product_attribute SET 
                            A1=@A1, A2=@A2, 
                            A3=@A3, A4=@A4, 
                            A5=@A5, A6=@A6, 
                            A7=@A7, A8=@A8, 
                            A9=@A9, A10=@A10, 
                            A11=@A11, A12=@A12, 
                            A13=@A13, A14=@A14, 
                            A15=@A15, A16=@A16, 
                            A17=@A17, A18=@A18, 
                            A19=@A19, A20=@A20, 
                            A21=@A21, A22=@A22, 
                            A23=@A23, A24=@A24, 
                            A25=@A25, A26=@A26, 
                            A27=@A27, A28=@A28, 
                            A29=@A29, A30=@A30
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramA1 = new SqlParameter("@A1", SqlDbType.NVarChar,200);
			SqlParameter paramA2 = new SqlParameter("@A2", SqlDbType.NVarChar,200);
			SqlParameter paramA3 = new SqlParameter("@A3", SqlDbType.NVarChar,200);
			SqlParameter paramA4 = new SqlParameter("@A4", SqlDbType.NVarChar,200);
			SqlParameter paramA5 = new SqlParameter("@A5", SqlDbType.NVarChar,200);
			SqlParameter paramA6 = new SqlParameter("@A6", SqlDbType.NVarChar,200);
			SqlParameter paramA7 = new SqlParameter("@A7", SqlDbType.NVarChar,200);
			SqlParameter paramA8 = new SqlParameter("@A8", SqlDbType.NVarChar,200);
			SqlParameter paramA9 = new SqlParameter("@A9", SqlDbType.NVarChar,200);
			SqlParameter paramA10 = new SqlParameter("@A10", SqlDbType.NVarChar,200);
			SqlParameter paramA11 = new SqlParameter("@A11", SqlDbType.NVarChar,200);
			SqlParameter paramA12 = new SqlParameter("@A12", SqlDbType.NVarChar,200);
			SqlParameter paramA13 = new SqlParameter("@A13", SqlDbType.NVarChar,200);
			SqlParameter paramA14 = new SqlParameter("@A14", SqlDbType.NVarChar,200);
			SqlParameter paramA15 = new SqlParameter("@A15", SqlDbType.NVarChar,200);
			SqlParameter paramA16 = new SqlParameter("@A16", SqlDbType.NVarChar,200);
			SqlParameter paramA17 = new SqlParameter("@A17", SqlDbType.NVarChar,200);
			SqlParameter paramA18 = new SqlParameter("@A18", SqlDbType.NVarChar,200);
			SqlParameter paramA19 = new SqlParameter("@A19", SqlDbType.NVarChar,200);
			SqlParameter paramA20 = new SqlParameter("@A20", SqlDbType.NVarChar,200);
			SqlParameter paramA21 = new SqlParameter("@A21", SqlDbType.NVarChar,200);
			SqlParameter paramA22 = new SqlParameter("@A22", SqlDbType.NVarChar,200);
			SqlParameter paramA23 = new SqlParameter("@A23", SqlDbType.NVarChar,200);
			SqlParameter paramA24 = new SqlParameter("@A24", SqlDbType.NVarChar,200);
			SqlParameter paramA25 = new SqlParameter("@A25", SqlDbType.NVarChar,200);
			SqlParameter paramA26 = new SqlParameter("@A26", SqlDbType.NVarChar,200);
			SqlParameter paramA27 = new SqlParameter("@A27", SqlDbType.NVarChar,200);
			SqlParameter paramA28 = new SqlParameter("@A28", SqlDbType.NVarChar,200);
			SqlParameter paramA29 = new SqlParameter("@A29", SqlDbType.NVarChar,200);
			SqlParameter paramA30 = new SqlParameter("@A30", SqlDbType.NVarChar,200);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.A1 != AppConst.StringNull)
				paramA1.Value = oParam.A1;
			else
				paramA1.Value = System.DBNull.Value;
			if ( oParam.A2 != AppConst.StringNull)
				paramA2.Value = oParam.A2;
			else
				paramA2.Value = System.DBNull.Value;
			if ( oParam.A3 != AppConst.StringNull)
				paramA3.Value = oParam.A3;
			else
				paramA3.Value = System.DBNull.Value;
			if ( oParam.A4 != AppConst.StringNull)
				paramA4.Value = oParam.A4;
			else
				paramA4.Value = System.DBNull.Value;
			if ( oParam.A5 != AppConst.StringNull)
				paramA5.Value = oParam.A5;
			else
				paramA5.Value = System.DBNull.Value;
			if ( oParam.A6 != AppConst.StringNull)
				paramA6.Value = oParam.A6;
			else
				paramA6.Value = System.DBNull.Value;
			if ( oParam.A7 != AppConst.StringNull)
				paramA7.Value = oParam.A7;
			else
				paramA7.Value = System.DBNull.Value;
			if ( oParam.A8 != AppConst.StringNull)
				paramA8.Value = oParam.A8;
			else
				paramA8.Value = System.DBNull.Value;
			if ( oParam.A9 != AppConst.StringNull)
				paramA9.Value = oParam.A9;
			else
				paramA9.Value = System.DBNull.Value;
			if ( oParam.A10 != AppConst.StringNull)
				paramA10.Value = oParam.A10;
			else
				paramA10.Value = System.DBNull.Value;
			if ( oParam.A11 != AppConst.StringNull)
				paramA11.Value = oParam.A11;
			else
				paramA11.Value = System.DBNull.Value;
			if ( oParam.A12 != AppConst.StringNull)
				paramA12.Value = oParam.A12;
			else
				paramA12.Value = System.DBNull.Value;
			if ( oParam.A13 != AppConst.StringNull)
				paramA13.Value = oParam.A13;
			else
				paramA13.Value = System.DBNull.Value;
			if ( oParam.A14 != AppConst.StringNull)
				paramA14.Value = oParam.A14;
			else
				paramA14.Value = System.DBNull.Value;
			if ( oParam.A15 != AppConst.StringNull)
				paramA15.Value = oParam.A15;
			else
				paramA15.Value = System.DBNull.Value;
			if ( oParam.A16 != AppConst.StringNull)
				paramA16.Value = oParam.A16;
			else
				paramA16.Value = System.DBNull.Value;
			if ( oParam.A17 != AppConst.StringNull)
				paramA17.Value = oParam.A17;
			else
				paramA17.Value = System.DBNull.Value;
			if ( oParam.A18 != AppConst.StringNull)
				paramA18.Value = oParam.A18;
			else
				paramA18.Value = System.DBNull.Value;
			if ( oParam.A19 != AppConst.StringNull)
				paramA19.Value = oParam.A19;
			else
				paramA19.Value = System.DBNull.Value;
			if ( oParam.A20 != AppConst.StringNull)
				paramA20.Value = oParam.A20;
			else
				paramA20.Value = System.DBNull.Value;
			if ( oParam.A21 != AppConst.StringNull)
				paramA21.Value = oParam.A21;
			else
				paramA21.Value = System.DBNull.Value;
			if ( oParam.A22 != AppConst.StringNull)
				paramA22.Value = oParam.A22;
			else
				paramA22.Value = System.DBNull.Value;
			if ( oParam.A23 != AppConst.StringNull)
				paramA23.Value = oParam.A23;
			else
				paramA23.Value = System.DBNull.Value;
			if ( oParam.A24 != AppConst.StringNull)
				paramA24.Value = oParam.A24;
			else
				paramA24.Value = System.DBNull.Value;
			if ( oParam.A25 != AppConst.StringNull)
				paramA25.Value = oParam.A25;
			else
				paramA25.Value = System.DBNull.Value;
			if ( oParam.A26 != AppConst.StringNull)
				paramA26.Value = oParam.A26;
			else
				paramA26.Value = System.DBNull.Value;
			if ( oParam.A27 != AppConst.StringNull)
				paramA27.Value = oParam.A27;
			else
				paramA27.Value = System.DBNull.Value;
			if ( oParam.A28 != AppConst.StringNull)
				paramA28.Value = oParam.A28;
			else
				paramA28.Value = System.DBNull.Value;
			if ( oParam.A29 != AppConst.StringNull)
				paramA29.Value = oParam.A29;
			else
				paramA29.Value = System.DBNull.Value;
			if ( oParam.A30 != AppConst.StringNull)
				paramA30.Value = oParam.A30;
			else
				paramA30.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramA1);
			cmd.Parameters.Add(paramA2);
			cmd.Parameters.Add(paramA3);
			cmd.Parameters.Add(paramA4);
			cmd.Parameters.Add(paramA5);
			cmd.Parameters.Add(paramA6);
			cmd.Parameters.Add(paramA7);
			cmd.Parameters.Add(paramA8);
			cmd.Parameters.Add(paramA9);
			cmd.Parameters.Add(paramA10);
			cmd.Parameters.Add(paramA11);
			cmd.Parameters.Add(paramA12);
			cmd.Parameters.Add(paramA13);
			cmd.Parameters.Add(paramA14);
			cmd.Parameters.Add(paramA15);
			cmd.Parameters.Add(paramA16);
			cmd.Parameters.Add(paramA17);
			cmd.Parameters.Add(paramA18);
			cmd.Parameters.Add(paramA19);
			cmd.Parameters.Add(paramA20);
			cmd.Parameters.Add(paramA21);
			cmd.Parameters.Add(paramA22);
			cmd.Parameters.Add(paramA23);
			cmd.Parameters.Add(paramA24);
			cmd.Parameters.Add(paramA25);
			cmd.Parameters.Add(paramA26);
			cmd.Parameters.Add(paramA27);
			cmd.Parameters.Add(paramA28);
			cmd.Parameters.Add(paramA29);
			cmd.Parameters.Add(paramA30);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update2(ProductAttributeInfo oParam)
		{
			string sql = @"UPDATE product_attribute SET 
                            A1_AttributeOptionSysNo=@A1, A2_AttributeOptionSysNo=@A2, 
                            A3_AttributeOptionSysNo=@A3, A4_AttributeOptionSysNo=@A4, 
                            A5_AttributeOptionSysNo=@A5, A6_AttributeOptionSysNo=@A6, 
                            A7_AttributeOptionSysNo=@A7, A8_AttributeOptionSysNo=@A8, 
                            A9_AttributeOptionSysNo=@A9, A10_AttributeOptionSysNo=@A10, 
                            A11_AttributeOptionSysNo=@A11, A12_AttributeOptionSysNo=@A12, 
                            A13_AttributeOptionSysNo=@A13, A14_AttributeOptionSysNo=@A14, 
                            A15_AttributeOptionSysNo=@A15, A16_AttributeOptionSysNo=@A16, 
                            A17_AttributeOptionSysNo=@A17, A18_AttributeOptionSysNo=@A18, 
                            A19_AttributeOptionSysNo=@A19, A20_AttributeOptionSysNo=@A20, 
                            A21_AttributeOptionSysNo=@A21, A22_AttributeOptionSysNo=@A22, 
                            A23_AttributeOptionSysNo=@A23, A24_AttributeOptionSysNo=@A24, 
                            A25_AttributeOptionSysNo=@A25, A26_AttributeOptionSysNo=@A26, 
                            A27_AttributeOptionSysNo=@A27, A28_AttributeOptionSysNo=@A28, 
                            A29_AttributeOptionSysNo=@A29, A30_AttributeOptionSysNo=@A30
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramA1 = new SqlParameter("@A1", SqlDbType.NVarChar,200);
			SqlParameter paramA2 = new SqlParameter("@A2", SqlDbType.NVarChar,200);
			SqlParameter paramA3 = new SqlParameter("@A3", SqlDbType.NVarChar,200);
			SqlParameter paramA4 = new SqlParameter("@A4", SqlDbType.NVarChar,200);
			SqlParameter paramA5 = new SqlParameter("@A5", SqlDbType.NVarChar,200);
			SqlParameter paramA6 = new SqlParameter("@A6", SqlDbType.NVarChar,200);
			SqlParameter paramA7 = new SqlParameter("@A7", SqlDbType.NVarChar,200);
			SqlParameter paramA8 = new SqlParameter("@A8", SqlDbType.NVarChar,200);
			SqlParameter paramA9 = new SqlParameter("@A9", SqlDbType.NVarChar,200);
			SqlParameter paramA10 = new SqlParameter("@A10", SqlDbType.NVarChar,200);
			SqlParameter paramA11 = new SqlParameter("@A11", SqlDbType.NVarChar,200);
			SqlParameter paramA12 = new SqlParameter("@A12", SqlDbType.NVarChar,200);
			SqlParameter paramA13 = new SqlParameter("@A13", SqlDbType.NVarChar,200);
			SqlParameter paramA14 = new SqlParameter("@A14", SqlDbType.NVarChar,200);
			SqlParameter paramA15 = new SqlParameter("@A15", SqlDbType.NVarChar,200);
			SqlParameter paramA16 = new SqlParameter("@A16", SqlDbType.NVarChar,200);
			SqlParameter paramA17 = new SqlParameter("@A17", SqlDbType.NVarChar,200);
			SqlParameter paramA18 = new SqlParameter("@A18", SqlDbType.NVarChar,200);
			SqlParameter paramA19 = new SqlParameter("@A19", SqlDbType.NVarChar,200);
			SqlParameter paramA20 = new SqlParameter("@A20", SqlDbType.NVarChar,200);
			SqlParameter paramA21 = new SqlParameter("@A21", SqlDbType.NVarChar,200);
			SqlParameter paramA22 = new SqlParameter("@A22", SqlDbType.NVarChar,200);
			SqlParameter paramA23 = new SqlParameter("@A23", SqlDbType.NVarChar,200);
			SqlParameter paramA24 = new SqlParameter("@A24", SqlDbType.NVarChar,200);
			SqlParameter paramA25 = new SqlParameter("@A25", SqlDbType.NVarChar,200);
			SqlParameter paramA26 = new SqlParameter("@A26", SqlDbType.NVarChar,200);
			SqlParameter paramA27 = new SqlParameter("@A27", SqlDbType.NVarChar,200);
			SqlParameter paramA28 = new SqlParameter("@A28", SqlDbType.NVarChar,200);
			SqlParameter paramA29 = new SqlParameter("@A29", SqlDbType.NVarChar,200);
			SqlParameter paramA30 = new SqlParameter("@A30", SqlDbType.NVarChar,200);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.A1 != AppConst.StringNull)
				paramA1.Value = oParam.A1;
			else
				paramA1.Value = System.DBNull.Value;
			if ( oParam.A2 != AppConst.StringNull)
				paramA2.Value = oParam.A2;
			else
				paramA2.Value = System.DBNull.Value;
			if ( oParam.A3 != AppConst.StringNull)
				paramA3.Value = oParam.A3;
			else
				paramA3.Value = System.DBNull.Value;
			if ( oParam.A4 != AppConst.StringNull)
				paramA4.Value = oParam.A4;
			else
				paramA4.Value = System.DBNull.Value;
			if ( oParam.A5 != AppConst.StringNull)
				paramA5.Value = oParam.A5;
			else
				paramA5.Value = System.DBNull.Value;
			if ( oParam.A6 != AppConst.StringNull)
				paramA6.Value = oParam.A6;
			else
				paramA6.Value = System.DBNull.Value;
			if ( oParam.A7 != AppConst.StringNull)
				paramA7.Value = oParam.A7;
			else
				paramA7.Value = System.DBNull.Value;
			if ( oParam.A8 != AppConst.StringNull)
				paramA8.Value = oParam.A8;
			else
				paramA8.Value = System.DBNull.Value;
			if ( oParam.A9 != AppConst.StringNull)
				paramA9.Value = oParam.A9;
			else
				paramA9.Value = System.DBNull.Value;
			if ( oParam.A10 != AppConst.StringNull)
				paramA10.Value = oParam.A10;
			else
				paramA10.Value = System.DBNull.Value;
			if ( oParam.A11 != AppConst.StringNull)
				paramA11.Value = oParam.A11;
			else
				paramA11.Value = System.DBNull.Value;
			if ( oParam.A12 != AppConst.StringNull)
				paramA12.Value = oParam.A12;
			else
				paramA12.Value = System.DBNull.Value;
			if ( oParam.A13 != AppConst.StringNull)
				paramA13.Value = oParam.A13;
			else
				paramA13.Value = System.DBNull.Value;
			if ( oParam.A14 != AppConst.StringNull)
				paramA14.Value = oParam.A14;
			else
				paramA14.Value = System.DBNull.Value;
			if ( oParam.A15 != AppConst.StringNull)
				paramA15.Value = oParam.A15;
			else
				paramA15.Value = System.DBNull.Value;
			if ( oParam.A16 != AppConst.StringNull)
				paramA16.Value = oParam.A16;
			else
				paramA16.Value = System.DBNull.Value;
			if ( oParam.A17 != AppConst.StringNull)
				paramA17.Value = oParam.A17;
			else
				paramA17.Value = System.DBNull.Value;
			if ( oParam.A18 != AppConst.StringNull)
				paramA18.Value = oParam.A18;
			else
				paramA18.Value = System.DBNull.Value;
			if ( oParam.A19 != AppConst.StringNull)
				paramA19.Value = oParam.A19;
			else
				paramA19.Value = System.DBNull.Value;
			if ( oParam.A20 != AppConst.StringNull)
				paramA20.Value = oParam.A20;
			else
				paramA20.Value = System.DBNull.Value;
			if ( oParam.A21 != AppConst.StringNull)
				paramA21.Value = oParam.A21;
			else
				paramA21.Value = System.DBNull.Value;
			if ( oParam.A22 != AppConst.StringNull)
				paramA22.Value = oParam.A22;
			else
				paramA22.Value = System.DBNull.Value;
			if ( oParam.A23 != AppConst.StringNull)
				paramA23.Value = oParam.A23;
			else
				paramA23.Value = System.DBNull.Value;
			if ( oParam.A24 != AppConst.StringNull)
				paramA24.Value = oParam.A24;
			else
				paramA24.Value = System.DBNull.Value;
			if ( oParam.A25 != AppConst.StringNull)
				paramA25.Value = oParam.A25;
			else
				paramA25.Value = System.DBNull.Value;
			if ( oParam.A26 != AppConst.StringNull)
				paramA26.Value = oParam.A26;
			else
				paramA26.Value = System.DBNull.Value;
			if ( oParam.A27 != AppConst.StringNull)
				paramA27.Value = oParam.A27;
			else
				paramA27.Value = System.DBNull.Value;
			if ( oParam.A28 != AppConst.StringNull)
				paramA28.Value = oParam.A28;
			else
				paramA28.Value = System.DBNull.Value;
			if ( oParam.A29 != AppConst.StringNull)
				paramA29.Value = oParam.A29;
			else
				paramA29.Value = System.DBNull.Value;
			if ( oParam.A30 != AppConst.StringNull)
				paramA30.Value = oParam.A30;
			else
				paramA30.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramA1);
			cmd.Parameters.Add(paramA2);
			cmd.Parameters.Add(paramA3);
			cmd.Parameters.Add(paramA4);
			cmd.Parameters.Add(paramA5);
			cmd.Parameters.Add(paramA6);
			cmd.Parameters.Add(paramA7);
			cmd.Parameters.Add(paramA8);
			cmd.Parameters.Add(paramA9);
			cmd.Parameters.Add(paramA10);
			cmd.Parameters.Add(paramA11);
			cmd.Parameters.Add(paramA12);
			cmd.Parameters.Add(paramA13);
			cmd.Parameters.Add(paramA14);
			cmd.Parameters.Add(paramA15);
			cmd.Parameters.Add(paramA16);
			cmd.Parameters.Add(paramA17);
			cmd.Parameters.Add(paramA18);
			cmd.Parameters.Add(paramA19);
			cmd.Parameters.Add(paramA20);
			cmd.Parameters.Add(paramA21);
			cmd.Parameters.Add(paramA22);
			cmd.Parameters.Add(paramA23);
			cmd.Parameters.Add(paramA24);
			cmd.Parameters.Add(paramA25);
			cmd.Parameters.Add(paramA26);
			cmd.Parameters.Add(paramA27);
			cmd.Parameters.Add(paramA28);
			cmd.Parameters.Add(paramA29);
			cmd.Parameters.Add(paramA30);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
