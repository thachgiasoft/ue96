using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Objects.Promotion;
using Icson.Utils;

namespace Icson.DBAccess.Promotion
{
   public class PromotionItemProductDac
    {
       public int Insert(PromotionItemProductInfo oParam)
       {
           string sql = @"INSERT INTO Promotion_Item_Product
                            (
                            PromotionItemGroupSysNo, ProductSysNo, PromotionDiscount, OrderNum
                            )
                            VALUES (
                            @PromotionItemGroupSysNo, @ProductSysNo, @PromotionDiscount, @OrderNum
                            );set @SysNo = SCOPE_IDENTITY();";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramPromotionItemGroupSysNo = new SqlParameter("@PromotionItemGroupSysNo", SqlDbType.Int, 4);
           SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
           SqlParameter paramPromotionDiscount = new SqlParameter("@PromotionDiscount", SqlDbType.Decimal, 9);
           SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
           paramSysNo.Direction = ParameterDirection.Output;
           if (oParam.PromotionItemGroupSysNo != AppConst.IntNull)
               paramPromotionItemGroupSysNo.Value = oParam.PromotionItemGroupSysNo;
           else
               paramPromotionItemGroupSysNo.Value = System.DBNull.Value;
           if (oParam.ProductSysNo != AppConst.IntNull)
               paramProductSysNo.Value = oParam.ProductSysNo;
           else
               paramProductSysNo.Value = System.DBNull.Value;
           if (oParam.PromotionDiscount != AppConst.DecimalNull)
               paramPromotionDiscount.Value = oParam.PromotionDiscount;
           else
               paramPromotionDiscount.Value = System.DBNull.Value;
           if (oParam.OrderNum != AppConst.IntNull)
               paramOrderNum.Value = oParam.OrderNum;
           else
               paramOrderNum.Value = System.DBNull.Value;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramPromotionItemGroupSysNo);
           cmd.Parameters.Add(paramProductSysNo);
           cmd.Parameters.Add(paramPromotionDiscount);
           cmd.Parameters.Add(paramOrderNum);

           return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
       }
       public int Update(PromotionItemProductInfo oParam)
       {
           string sql = @"UPDATE Promotion_Item_Product SET 
                            PromotionItemGroupSysNo=@PromotionItemGroupSysNo, ProductSysNo=@ProductSysNo, 
                            PromotionDiscount=@PromotionDiscount, OrderNum=@OrderNum
                            WHERE SysNo=@SysNo";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramPromotionItemGroupSysNo = new SqlParameter("@PromotionItemGroupSysNo", SqlDbType.Int, 4);
           SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
           SqlParameter paramPromotionDiscount = new SqlParameter("@PromotionDiscount", SqlDbType.Decimal, 9);
           SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);

           if (oParam.SysNo != AppConst.IntNull)
               paramSysNo.Value = oParam.SysNo;
           else
               paramSysNo.Value = System.DBNull.Value;
           if (oParam.PromotionItemGroupSysNo != AppConst.IntNull)
               paramPromotionItemGroupSysNo.Value = oParam.PromotionItemGroupSysNo;
           else
               paramPromotionItemGroupSysNo.Value = System.DBNull.Value;
           if (oParam.ProductSysNo != AppConst.IntNull)
               paramProductSysNo.Value = oParam.ProductSysNo;
           else
               paramProductSysNo.Value = System.DBNull.Value;
           if (oParam.PromotionDiscount != AppConst.DecimalNull)
               paramPromotionDiscount.Value = oParam.PromotionDiscount;
           else
               paramPromotionDiscount.Value = System.DBNull.Value;
           if (oParam.OrderNum != AppConst.IntNull)
               paramOrderNum.Value = oParam.OrderNum;
           else
               paramOrderNum.Value = System.DBNull.Value;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramPromotionItemGroupSysNo);
           cmd.Parameters.Add(paramProductSysNo);
           cmd.Parameters.Add(paramPromotionDiscount);
           cmd.Parameters.Add(paramOrderNum);

           return SqlHelper.ExecuteNonQuery(cmd);
       }
    }
}
