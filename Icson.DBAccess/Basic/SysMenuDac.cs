using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    public class SysMenuDac
    {
        public int Insert(SysMenuInfo oParam)
        {
            string sql = @"INSERT INTO Sys_Menu
      (MenuID, ParentID, OrderNum, SubOrder, Name, Description, href, help, Icon, 
      Privilege)
VALUES (@MenuID, @ParentID, @OrderNum, @SubOrder, @Name, @Description, @href, @help, @Icon, 
      @Privilege)";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramMenuID = new SqlParameter("@MenuID", SqlDbType.Int, 4);
            SqlParameter paramParentID = new SqlParameter("@ParentID", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramSubOrder = new SqlParameter("@SubOrder", SqlDbType.Int, 4);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.NVarChar, 255);
            SqlParameter paramhref = new SqlParameter("@href", SqlDbType.NVarChar, 255);
            SqlParameter paramhelp = new SqlParameter("@help", SqlDbType.NVarChar, 255);
            SqlParameter paramIcon = new SqlParameter("@Icon", SqlDbType.NVarChar, 50);
            SqlParameter paramPrivilege = new SqlParameter("@Privilege", SqlDbType.NVarChar, 500);

            if (oParam.MenuID != AppConst.IntNull)
                paramMenuID.Value = oParam.MenuID;
            else
                paramMenuID.Value = System.DBNull.Value;
            if (oParam.ParentID != AppConst.IntNull)
                paramParentID.Value = oParam.ParentID;
            else
                paramParentID.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.SubOrder != AppConst.IntNull)
                paramSubOrder.Value = oParam.SubOrder;
            else
                paramSubOrder.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.href != AppConst.StringNull)
                paramhref.Value = oParam.href;
            else
                paramhref.Value = System.DBNull.Value;
            if (oParam.help != AppConst.StringNull)
                paramhelp.Value = oParam.help;
            else
                paramhelp.Value = System.DBNull.Value;
            if (oParam.Icon != AppConst.StringNull)
                paramIcon.Value = oParam.Icon;
            else
                paramIcon.Value = System.DBNull.Value;
            if (oParam.Privilege != AppConst.StringNull)
                paramPrivilege.Value = oParam.Privilege;
            else
                paramPrivilege.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramMenuID);
            cmd.Parameters.Add(paramParentID);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramSubOrder);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramhref);
            cmd.Parameters.Add(paramhelp);
            cmd.Parameters.Add(paramIcon);
            cmd.Parameters.Add(paramPrivilege);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(SysMenuInfo oParam)
        {
            string sql = @"UPDATE Sys_Menu
SET MenuID = @MenuID, ParentID = @ParentID, OrderNum = @OrderNum, SubOrder = @SubOrder, Name = @Name, 
      Description = @Description, href = @href, help = @help, Icon = @Icon, Privilege = @Privilege
                           WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramMenuID = new SqlParameter("@MenuID", SqlDbType.Int, 4);
            SqlParameter paramParentID = new SqlParameter("@ParentID", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramSubOrder = new SqlParameter("@SubOrder", SqlDbType.Int, 4);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.NVarChar, 255);
            SqlParameter paramhref = new SqlParameter("@href", SqlDbType.NVarChar, 255);
            SqlParameter paramhelp = new SqlParameter("@help", SqlDbType.NVarChar, 255);
            SqlParameter paramIcon = new SqlParameter("@Icon", SqlDbType.NVarChar, 50);
            SqlParameter paramPrivilege = new SqlParameter("@Privilege", SqlDbType.NVarChar, 500);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.MenuID != AppConst.IntNull)
                paramMenuID.Value = oParam.MenuID;
            else
                paramMenuID.Value = System.DBNull.Value;
            if (oParam.ParentID != AppConst.IntNull)
                paramParentID.Value = oParam.ParentID;
            else
                paramParentID.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.SubOrder != AppConst.IntNull)
                paramSubOrder.Value = oParam.SubOrder;
            else
                paramSubOrder.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.href != AppConst.StringNull)
                paramhref.Value = oParam.href;
            else
                paramhref.Value = System.DBNull.Value;
            if (oParam.help != AppConst.StringNull)
                paramhelp.Value = oParam.help;
            else
                paramhelp.Value = System.DBNull.Value;
            if (oParam.Icon != AppConst.StringNull)
                paramIcon.Value = oParam.Icon;
            else
                paramIcon.Value = System.DBNull.Value;
            if (oParam.Privilege != AppConst.StringNull)
                paramPrivilege.Value = oParam.Privilege;
            else
                paramPrivilege.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramMenuID);
            cmd.Parameters.Add(paramParentID);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramSubOrder);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramhref);
            cmd.Parameters.Add(paramhelp);
            cmd.Parameters.Add(paramIcon);
            cmd.Parameters.Add(paramPrivilege);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

    }
}
