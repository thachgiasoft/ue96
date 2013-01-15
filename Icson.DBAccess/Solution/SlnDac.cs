using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Objects.Solution;
using Icson.Utils;

namespace Icson.DBAccess.Solution
{
    public class SlnDac
    {
        public SlnDac()
        {
        }

        #region sln_master
        public int Insert(SlnMasterInfo oParam)
        {
            string sql = @"INSERT INTO sln_master
                            (
                            ID, Name, Title, Description, 
                            SysUserSysNo, DateStamp, OrderNum, Status
                            )
                            VALUES (
                            @ID, @Name, @Title, @Description, 
                            @SysUserSysNo, @DateStamp, @OrderNum, @Status
                            );set @SysNo = SCOPE_IDENTITY();";            
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramID = new SqlParameter("@ID", SqlDbType.NVarChar, 50);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 500);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 500);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.Text, 0);
            SqlParameter paramSysUserSysNo = new SqlParameter("@SysUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDateStamp = new SqlParameter("@DateStamp", SqlDbType.DateTime);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
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

        public int Update(SlnMasterInfo oParam)
        {           
            string sql = @"UPDATE sln_master SET 
                            ID=@ID, Name=@Name, 
                            Title=@Title, Description=@Description, 
                            SysUserSysNo=@SysUserSysNo, DateStamp=@DateStamp, 
                            OrderNum=@OrderNum, Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
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
                paramSysNo.Value = System.DBNull.Value;

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

        public int GetSlnMasterNewOrderNum()
        {
            string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from sln_master";
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

        #region sln_item
        public int Insert(SlnItemInfo oParam)
        {
            string sql = @"INSERT INTO sln_item
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

        public int Update(SlnItemInfo oParam)
        {            
            string sql = @"UPDATE sln_item SET 
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

        public int SetOrderNum(SlnItemInfo oParam)
        {
            string sql = "update sln_item set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int GetSlnItemNewOrderNum(SlnItemInfo oParam)
        {
            string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from sln_item where slnsysno=" + oParam.SlnSysNo;
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

        #region sln_item_c3
        public int Insert(SlnItemC3Info oParam)
        {
            string sql = @"INSERT INTO sln_item_c3
                            (
                            SlnItemSysNo, C3SysNo, Status
                            )
                            VALUES (
                            @SlnItemSysNo, @C3SysNo, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnItemSysNo = new SqlParameter("@SlnItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SlnItemSysNo != AppConst.IntNull)
                paramSlnItemSysNo.Value = oParam.SlnItemSysNo;
            else
                paramSlnItemSysNo.Value = System.DBNull.Value;
            if (oParam.C3SysNo != AppConst.IntNull)
                paramC3SysNo.Value = oParam.C3SysNo;
            else
                paramC3SysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSlnItemSysNo);
            cmd.Parameters.Add(paramC3SysNo);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(SlnItemC3Info oParam)
        {
            string sql = @"UPDATE sln_item_c3 SET 
                            SlnItemSysNo=@SlnItemSysNo, C3SysNo=@C3SysNo, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnItemSysNo = new SqlParameter("@SlnItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SlnItemSysNo != AppConst.IntNull)
                paramSlnItemSysNo.Value = oParam.SlnItemSysNo;
            else
                paramSlnItemSysNo.Value = System.DBNull.Value;
            if (oParam.C3SysNo != AppConst.IntNull)
                paramC3SysNo.Value = oParam.C3SysNo;
            else
                paramC3SysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSlnItemSysNo);
            cmd.Parameters.Add(paramC3SysNo);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        #endregion

        #region sln_item_c3_attr2
        public int Insert(SlnItemC3Attr2Info oParam)
        {
            string sql = @"INSERT INTO sln_item_c3_attr2
                            (
                            SlnItemC3SysNo, C3Attr2SysNo, Status
                            )
                            VALUES (
                            @SlnItemC3SysNo, @C3Attr2SysNo, @Status
                            );set @SysNo = SCOPE_IDENTITY();";            
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnItemC3SysNo = new SqlParameter("@SlnItemC3SysNo", SqlDbType.Int, 4);
            SqlParameter paramC3Attr2SysNo = new SqlParameter("@C3Attr2SysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SlnItemC3SysNo != AppConst.IntNull)
                paramSlnItemC3SysNo.Value = oParam.SlnItemC3SysNo;
            else
                paramSlnItemC3SysNo.Value = System.DBNull.Value;
            if (oParam.C3Attr2SysNo != AppConst.IntNull)
                paramC3Attr2SysNo.Value = oParam.C3Attr2SysNo;
            else
                paramC3Attr2SysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSlnItemC3SysNo);
            cmd.Parameters.Add(paramC3Attr2SysNo);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int Update(SlnItemC3Attr2Info oParam)
        {
            string sql = @"UPDATE sln_item_c3_attr2 SET 
                            SlnItemC3SysNo=@SlnItemC3SysNo, C3Attr2SysNo=@C3Attr2SysNo, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSlnItemC3SysNo = new SqlParameter("@SlnItemC3SysNo", SqlDbType.Int, 4);
            SqlParameter paramC3Attr2SysNo = new SqlParameter("@C3Attr2SysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SlnItemC3SysNo != AppConst.IntNull)
                paramSlnItemC3SysNo.Value = oParam.SlnItemC3SysNo;
            else
                paramSlnItemC3SysNo.Value = System.DBNull.Value;
            if (oParam.C3Attr2SysNo != AppConst.IntNull)
                paramC3Attr2SysNo.Value = oParam.C3Attr2SysNo;
            else
                paramC3Attr2SysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSlnItemC3SysNo);
            cmd.Parameters.Add(paramC3Attr2SysNo);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        #endregion
    }
}
