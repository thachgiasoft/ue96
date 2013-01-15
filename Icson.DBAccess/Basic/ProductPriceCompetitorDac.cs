using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
   public class ProductPriceCompetitorDac
    {
       public int Insert(ProductPriceCompetitorInfo oParam)
       {
           string sql = @"INSERT INTO Product_Price_Competitor
                            (
                            ProductSysNo, CompetitorPrice1, ImportTime1, CompetitorPrice2, 
                            ImportTime2, CompetitorPrice3, ImportTime3
                            )
                            VALUES (
                            @ProductSysNo, @CompetitorPrice1, @ImportTime1, @CompetitorPrice2, 
                            @ImportTime2, @CompetitorPrice3, @ImportTime3
                            );set @SysNo = SCOPE_IDENTITY();";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
           SqlParameter paramCompetitorPrice1 = new SqlParameter("@CompetitorPrice1", SqlDbType.Decimal, 9);
           SqlParameter paramImportTime1 = new SqlParameter("@ImportTime1", SqlDbType.DateTime);
           SqlParameter paramCompetitorPrice2 = new SqlParameter("@CompetitorPrice2", SqlDbType.Decimal, 9);
           SqlParameter paramImportTime2 = new SqlParameter("@ImportTime2", SqlDbType.DateTime);
           SqlParameter paramCompetitorPrice3 = new SqlParameter("@CompetitorPrice3", SqlDbType.Decimal, 9);
           SqlParameter paramImportTime3 = new SqlParameter("@ImportTime3", SqlDbType.DateTime);
           paramSysNo.Direction = ParameterDirection.Output;
           if (oParam.ProductSysNo != AppConst.IntNull)
               paramProductSysNo.Value = oParam.ProductSysNo;
           else
               paramProductSysNo.Value = System.DBNull.Value;
           if (oParam.CompetitorPrice1 != AppConst.DecimalNull)
               paramCompetitorPrice1.Value = oParam.CompetitorPrice1;
           else
               paramCompetitorPrice1.Value = System.DBNull.Value;
           if (oParam.ImportTime1 != AppConst.DateTimeNull)
               paramImportTime1.Value = oParam.ImportTime1;
           else
               paramImportTime1.Value = System.DBNull.Value;
           if (oParam.CompetitorPrice2 != AppConst.DecimalNull)
               paramCompetitorPrice2.Value = oParam.CompetitorPrice2;
           else
               paramCompetitorPrice2.Value = System.DBNull.Value;
           if (oParam.ImportTime2 != AppConst.DateTimeNull)
               paramImportTime2.Value = oParam.ImportTime2;
           else
               paramImportTime2.Value = System.DBNull.Value;
           if (oParam.CompetitorPrice3 != AppConst.DecimalNull)
               paramCompetitorPrice3.Value = oParam.CompetitorPrice3;
           else
               paramCompetitorPrice3.Value = System.DBNull.Value;
           if (oParam.ImportTime3 != AppConst.DateTimeNull)
               paramImportTime3.Value = oParam.ImportTime3;
           else
               paramImportTime3.Value = System.DBNull.Value;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramProductSysNo);
           cmd.Parameters.Add(paramCompetitorPrice1);
           cmd.Parameters.Add(paramImportTime1);
           cmd.Parameters.Add(paramCompetitorPrice2);
           cmd.Parameters.Add(paramImportTime2);
           cmd.Parameters.Add(paramCompetitorPrice3);
           cmd.Parameters.Add(paramImportTime3);

           return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
       }
    }
}
