using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Online;
using Icson.Utils;

namespace Icson.DBAccess.Online
{
    public class WebBulletinDac
    {
        public int Insert(WebBulletinInfo oParam)
        {
            string sql = @"INSERT INTO WebBulletin
                            (
                            Title, Content, CreateDate, OrderNum, 
                            Status
                            )
                            VALUES (
                            @Title, @Content, @CreateDate, @OrderNum, 
                            @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 500);
            SqlParameter paramContent = new SqlParameter("@Content", SqlDbType.Text, 0);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.Title != AppConst.StringNull)
                paramTitle.Value = oParam.Title;
            else
                paramTitle.Value = System.DBNull.Value;
            if (oParam.Content != AppConst.StringNull)
                paramContent.Value = oParam.Content;
            else
                paramContent.Value = System.DBNull.Value;
            if (oParam.CreateDate != AppConst.DateTimeNull)
                paramCreateDate.Value = oParam.CreateDate;
            else
                paramCreateDate.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramTitle);
            cmd.Parameters.Add(paramContent);
            cmd.Parameters.Add(paramCreateDate);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(WebBulletinInfo oParam)
        {
            string sql = @"UPDATE WebBulletin SET 
                            Title=@Title, Content=@Content, 
                            CreateDate=@CreateDate, OrderNum=@OrderNum, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 500);
            SqlParameter paramContent = new SqlParameter("@Content", SqlDbType.Text, 0);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.Title != AppConst.StringNull)
                paramTitle.Value = oParam.Title;
            else
                paramTitle.Value = System.DBNull.Value;
            if (oParam.Content != AppConst.StringNull)
                paramContent.Value = oParam.Content;
            else
                paramContent.Value = System.DBNull.Value;
            if (oParam.CreateDate != AppConst.DateTimeNull)
                paramCreateDate.Value = oParam.CreateDate;
            else
                paramCreateDate.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramTitle);
            cmd.Parameters.Add(paramContent);
            cmd.Parameters.Add(paramCreateDate);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int SetOrderNum(WebBulletinInfo oParam)
        {
            string sql = "update WebBulletin set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
