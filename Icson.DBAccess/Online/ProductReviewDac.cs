using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.DBAccess.Online
{
    public class ProductReviewDac
    {
        public int Insert(Category3ReviewItemInfo oParam)
        {
            string sql = @"INSERT INTO Category3_ReviewItem
                            (
                            C3SysNo, ID, Name, Description, 
                            Weight, OrderNum, Status
                            )
                            VALUES (
                            @C3SysNo, @ID, @Name, @Description, 
                            @Weight, @OrderNum, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int, 4);
            SqlParameter paramID = new SqlParameter("@ID", SqlDbType.NVarChar, 10);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 100);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.Text, 0);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.C3SysNo != AppConst.IntNull)
                paramC3SysNo.Value = oParam.C3SysNo;
            else
                paramC3SysNo.Value = System.DBNull.Value;
            if (oParam.ID != AppConst.StringNull)
                paramID.Value = oParam.ID;
            else
                paramID.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramC3SysNo);
            cmd.Parameters.Add(paramID);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(Category3ReviewItemInfo oParam)
        {
            string sql = @"UPDATE Category3_ReviewItem SET 
                            C3SysNo=@C3SysNo, ID=@ID, 
                            Name=@Name, Description=@Description, 
                            Weight=@Weight, OrderNum=@OrderNum, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int, 4);
            SqlParameter paramID = new SqlParameter("@ID", SqlDbType.NVarChar, 10);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 100);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.Text, 0);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.C3SysNo != AppConst.IntNull)
                paramC3SysNo.Value = oParam.C3SysNo;
            else
                paramC3SysNo.Value = System.DBNull.Value;
            if (oParam.ID != AppConst.StringNull)
                paramID.Value = oParam.ID;
            else
                paramID.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramC3SysNo);
            cmd.Parameters.Add(paramID);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        //É¾³ýÆÀÂÛÏî
        public int Delete(int reviewSysNo)
        {
            string sql = "delete from Category3_ReviewItem where SysNo=" + reviewSysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int SetOrderNum(Category3ReviewItemInfo oParam)
        {
            string sql = "update category3_reviewitem set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int GetCatetory3ReviewItemNewOrderNum(Category3ReviewItemInfo oParam)
        {
            string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from category3_reviewitem where c3sysno=" + oParam.C3SysNo;
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
    }
}
