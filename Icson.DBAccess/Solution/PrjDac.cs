using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Objects.Solution;
using Icson.Utils;

namespace Icson.DBAccess.Solution
{
    public class PrjDac
    {
        public PrjDac()
        {}

        #region prj_type
        public int Insert(PrjTypeInfo oParam)
        {
            string sql = @"INSERT INTO prj_type
                            (
                            SlnSysNo, ID, Name, Title, 
                            Description, OrderNum, Status
                            )
                            VALUES (
                            @SlnSysNo, @ID, @Name, @Title, 
                            @Description, @OrderNum, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnSysNo = new SqlParameter("@SlnSysNo", SqlDbType.Int, 4);
            SqlParameter paramID = new SqlParameter("@ID", SqlDbType.NVarChar, 50);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 500);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 500);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.Text, 0);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SlnSysNo != AppConst.IntNull)
                paramSlnSysNo.Value = oParam.SlnSysNo;
            else
                paramSlnSysNo.Value = System.DBNull.Value;
            if (oParam.ID != AppConst.StringNull)
                paramID.Value = oParam.ID;
            else
                paramID.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Title != AppConst.StringNull)
                paramTitle.Value = oParam.Title;
            else
                paramTitle.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSlnSysNo);
            cmd.Parameters.Add(paramID);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramTitle);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(PrjTypeInfo oParam)
        {
            string sql = @"UPDATE prj_type SET 
                            SlnSysNo=@SlnSysNo, ID=@ID, 
                            Name=@Name, Title=@Title, 
                            Description=@Description, OrderNum=@OrderNum, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnSysNo = new SqlParameter("@SlnSysNo", SqlDbType.Int, 4);
            SqlParameter paramID = new SqlParameter("@ID", SqlDbType.NVarChar, 50);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 500);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 500);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.Text, 0);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;

            if (oParam.SlnSysNo != AppConst.IntNull)
                paramSlnSysNo.Value = oParam.SlnSysNo;
            else
                paramSlnSysNo.Value = System.DBNull.Value;
            if (oParam.ID != AppConst.StringNull)
                paramID.Value = oParam.ID;
            else
                paramID.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Title != AppConst.StringNull)
                paramTitle.Value = oParam.Title;
            else
                paramTitle.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSlnSysNo);
            cmd.Parameters.Add(paramID);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramTitle);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int SetOrderNum(PrjTypeInfo oParam)
        {
            string sql = "update prj_type set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int GetPrjTypeNewOrderNum(PrjTypeInfo oParam)
        {
            string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from prj_type where slnsysno=" + oParam.SlnSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region prj_master
        public int Insert(PrjMasterInfo oParam)
        {
            string sql = @"INSERT INTO Prj_Master
                            (
                            SlnSysNo, PrjTypeSysNo, ID, Name, 
                            Title, Description, SysUserSysNo, DateStamp, 
                            OrderNum, Status
                            )
                            VALUES (
                            @SlnSysNo, @PrjTypeSysNo, @ID, @Name, 
                            @Title, @Description, @SysUserSysNo, @DateStamp, 
                            @OrderNum, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnSysNo = new SqlParameter("@SlnSysNo", SqlDbType.Int, 4);
            SqlParameter paramPrjTypeSysNo = new SqlParameter("@PrjTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramID = new SqlParameter("@ID", SqlDbType.NVarChar, 50);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 500);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 500);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.Text, 0);
            SqlParameter paramSysUserSysNo = new SqlParameter("@SysUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDateStamp = new SqlParameter("@DateStamp", SqlDbType.DateTime);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SlnSysNo != AppConst.IntNull)
                paramSlnSysNo.Value = oParam.SlnSysNo;
            else
                paramSlnSysNo.Value = System.DBNull.Value;
            if (oParam.PrjTypeSysNo != AppConst.IntNull)
                paramPrjTypeSysNo.Value = oParam.PrjTypeSysNo;
            else
                paramPrjTypeSysNo.Value = System.DBNull.Value;
            if (oParam.ID != AppConst.StringNull)
                paramID.Value = oParam.ID;
            else
                paramID.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Title != AppConst.StringNull)
                paramTitle.Value = oParam.Title;
            else
                paramTitle.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.SysUserSysNo != AppConst.IntNull)
                paramSysUserSysNo.Value = oParam.SysUserSysNo;
            else
                paramSysUserSysNo.Value = System.DBNull.Value;
            if (oParam.DateStamp != AppConst.DateTimeNull)
                paramDateStamp.Value = oParam.DateStamp;
            else
                paramDateStamp.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSlnSysNo);
            cmd.Parameters.Add(paramPrjTypeSysNo);
            cmd.Parameters.Add(paramID);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramTitle);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramSysUserSysNo);
            cmd.Parameters.Add(paramDateStamp);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(PrjMasterInfo oParam)
        {
            string sql = @"UPDATE Prj_Master SET 
                            SlnSysNo=@SlnSysNo, PrjTypeSysNo=@PrjTypeSysNo, 
                            ID=@ID, Name=@Name, 
                            Title=@Title, Description=@Description, 
                            SysUserSysNo=@SysUserSysNo, DateStamp=@DateStamp, 
                            OrderNum=@OrderNum, Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnSysNo = new SqlParameter("@SlnSysNo", SqlDbType.Int, 4);
            SqlParameter paramPrjTypeSysNo = new SqlParameter("@PrjTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramID = new SqlParameter("@ID", SqlDbType.NVarChar, 50);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 500);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 500);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.Text, 0);
            SqlParameter paramSysUserSysNo = new SqlParameter("@SysUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDateStamp = new SqlParameter("@DateStamp", SqlDbType.DateTime);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = AppConst.IntNull;
            if (oParam.SlnSysNo != AppConst.IntNull)
                paramSlnSysNo.Value = oParam.SlnSysNo;
            else
                paramSlnSysNo.Value = System.DBNull.Value;
            if (oParam.PrjTypeSysNo != AppConst.IntNull)
                paramPrjTypeSysNo.Value = oParam.PrjTypeSysNo;
            else
                paramPrjTypeSysNo.Value = System.DBNull.Value;
            if (oParam.ID != AppConst.StringNull)
                paramID.Value = oParam.ID;
            else
                paramID.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Title != AppConst.StringNull)
                paramTitle.Value = oParam.Title;
            else
                paramTitle.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.SysUserSysNo != AppConst.IntNull)
                paramSysUserSysNo.Value = oParam.SysUserSysNo;
            else
                paramSysUserSysNo.Value = System.DBNull.Value;
            if (oParam.DateStamp != AppConst.DateTimeNull)
                paramDateStamp.Value = oParam.DateStamp;
            else
                paramDateStamp.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSlnSysNo);
            cmd.Parameters.Add(paramPrjTypeSysNo);
            cmd.Parameters.Add(paramID);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramTitle);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramSysUserSysNo);
            cmd.Parameters.Add(paramDateStamp);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int SetOrderNum(PrjMasterInfo oParam)
        {
            string sql = "update prj_master set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int GetPrjMasterNewOrderNum(PrjMasterInfo oParam)
        {
            string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from prj_master where slnsysno=" + oParam.SlnSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region prj_item
        public int Insert(PrjItemInfo oParam)
        {
            string sql = @"INSERT INTO Prj_Item
                            (
                            PrjSysNo, SlnItemSysNo, DefaultProductSysNo, DefaultQty, IsShowPic, Status
                            )
                            VALUES (
                            @PrjSysNo, @SlnItemSysNo, @DefaultProductSysNo, @DefaultQty, @IsShowPic, @Status
                            );set @SysNo = SCOPE_IDENTITY();";            
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPrjSysNo = new SqlParameter("@PrjSysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnItemSysNo = new SqlParameter("@SlnItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramDefaultProductSysNo = new SqlParameter("@DefaultProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramDefaultQty = new SqlParameter("@DefaultQty", SqlDbType.Int, 4);
            SqlParameter paramIsShowPic = new SqlParameter("@IsShowPic",SqlDbType.Int,4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.PrjSysNo != AppConst.IntNull)
                paramPrjSysNo.Value = oParam.PrjSysNo;
            else
                paramPrjSysNo.Value = System.DBNull.Value;
            if (oParam.SlnItemSysNo != AppConst.IntNull)
                paramSlnItemSysNo.Value = oParam.SlnItemSysNo;
            else
                paramSlnItemSysNo.Value = System.DBNull.Value;
            if (oParam.DefaultProductSysNo != AppConst.IntNull)
                paramDefaultProductSysNo.Value = oParam.DefaultProductSysNo;
            else
                paramDefaultProductSysNo.Value = System.DBNull.Value;
            if (oParam.DefaultQty != AppConst.IntNull)
                paramDefaultQty.Value = oParam.DefaultQty;
            else
                paramDefaultQty.Value = System.DBNull.Value;
            if (oParam.IsShowPic != AppConst.IntNull)
                paramIsShowPic.Value = oParam.IsShowPic;
            else
                paramIsShowPic.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPrjSysNo);
            cmd.Parameters.Add(paramSlnItemSysNo);
            cmd.Parameters.Add(paramDefaultProductSysNo);
            cmd.Parameters.Add(paramDefaultQty);
            cmd.Parameters.Add(paramIsShowPic);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(PrjItemInfo oParam)
        {           
            string sql = @"UPDATE Prj_Item SET 
                            PrjSysNo=@PrjSysNo, SlnItemSysNo=@SlnItemSysNo, DefaultProductSysNo=@DefaultProductSysNo, DefaultQty=@DefaultQty, 
                            IsShowPic=@IsShowPic, Status=@Status 
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPrjSysNo = new SqlParameter("@PrjSysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnItemSysNo = new SqlParameter("@SlnItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramDefaultProductSysNo = new SqlParameter("@DefaultProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramDefaultQty = new SqlParameter("@DefaultQty",SqlDbType.Int,4);
            SqlParameter paramIsShowPic = new SqlParameter("@IsShowPic",SqlDbType.Int,4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;

            if (oParam.PrjSysNo != AppConst.IntNull)
                paramPrjSysNo.Value = oParam.PrjSysNo;
            else
                paramPrjSysNo.Value = System.DBNull.Value;
            if (oParam.SlnItemSysNo != AppConst.IntNull)
                paramSlnItemSysNo.Value = oParam.SlnItemSysNo;
            else
                paramSlnItemSysNo.Value = System.DBNull.Value;
            if (oParam.DefaultProductSysNo != AppConst.IntNull)
                paramDefaultProductSysNo.Value = oParam.DefaultProductSysNo;
            else
                paramDefaultProductSysNo.Value = System.DBNull.Value;
            if (oParam.DefaultQty != AppConst.IntNull)
                paramDefaultQty.Value = oParam.DefaultQty;
            else
                paramDefaultQty.Value = System.DBNull.Value;
            if (oParam.IsShowPic != AppConst.IntNull)
                paramIsShowPic.Value = oParam.IsShowPic;
            else
                paramIsShowPic.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPrjSysNo);
            cmd.Parameters.Add(paramSlnItemSysNo);
            cmd.Parameters.Add(paramDefaultProductSysNo);
            cmd.Parameters.Add(paramDefaultQty);
            cmd.Parameters.Add(paramIsShowPic);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        #endregion

        #region prj_item_filter
        public int Insert(PrjItemFilterInfo oParam)
        {
            string sql = @"INSERT INTO prj_item_filter
                            (
                            PrjItemSysNo, Filter,PriceFrom,PriceTo
                            )
                            VALUES (
                            @PrjItemSysNo, @Filter,@PriceFrom,@PriceTo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPrjItemSysNo = new SqlParameter("@PrjItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramFilter = new SqlParameter("@Filter", SqlDbType.NVarChar, 2000);
            SqlParameter paramPriceFrom = new SqlParameter("@PriceFrom", SqlDbType.Decimal, 9);
            SqlParameter paramPriceTo = new SqlParameter("@PriceTo", SqlDbType.Decimal, 9);
            
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.PrjItemSysNo != AppConst.IntNull)
                paramPrjItemSysNo.Value = oParam.PrjItemSysNo;
            else
                paramPrjItemSysNo.Value = System.DBNull.Value;
            if (oParam.Filter != AppConst.StringNull)
                paramFilter.Value = oParam.Filter;
            else
                paramFilter.Value = System.DBNull.Value;
            if (oParam.PriceFrom != AppConst.DecimalNull)
                paramPriceFrom.Value = oParam.PriceFrom;
            else
                paramPriceFrom.Value = System.DBNull.Value;
            if (oParam.PriceTo != AppConst.DecimalNull)
                paramPriceTo.Value = oParam.PriceTo;
            else
                paramPriceTo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPrjItemSysNo);
            cmd.Parameters.Add(paramFilter);
            cmd.Parameters.Add(paramPriceFrom);
            cmd.Parameters.Add(paramPriceTo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(PrjItemFilterInfo oParam)
        {
            string sql = @"UPDATE prj_item_filter SET 
                            PrjItemSysNo=@PrjItemSysNo, Filter=@Filter,PriceFrom=@PriceFrom,PriceTo=@PriceTo
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPrjItemSysNo = new SqlParameter("@PrjItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramFilter = new SqlParameter("@Filter", SqlDbType.NVarChar, 2000);
            SqlParameter paramPriceFrom = new SqlParameter("@PriceFrom", SqlDbType.Decimal, 9);
            SqlParameter paramPriceTo = new SqlParameter("@PriceTo", SqlDbType.Decimal, 9);

            if(oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;                
            if (oParam.PrjItemSysNo != AppConst.IntNull)
                paramPrjItemSysNo.Value = oParam.PrjItemSysNo;
            else
                paramPrjItemSysNo.Value = System.DBNull.Value;
            if (oParam.Filter != AppConst.StringNull)
                paramFilter.Value = oParam.Filter;
            else
                paramFilter.Value = System.DBNull.Value;
            if (oParam.PriceFrom != AppConst.DecimalNull)
                paramPriceFrom.Value = oParam.PriceFrom;
            else
                paramPriceFrom.Value = System.DBNull.Value;
            if (oParam.PriceTo != AppConst.DecimalNull)
                paramPriceTo.Value = oParam.PriceTo;
            else
                paramPriceTo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPrjItemSysNo);
            cmd.Parameters.Add(paramFilter);
            cmd.Parameters.Add(paramPriceFrom);
            cmd.Parameters.Add(paramPriceTo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        #endregion
    }
}
