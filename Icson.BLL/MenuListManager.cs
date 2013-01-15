using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using Icson.Utils;

using Icson.Objects;
using Icson.Objects.Basic;

using Icson.DBAccess;
using Icson.DBAccess.Online;

using Icson.BLL.Basic;

namespace Icson.BLL
{
    public class MenuListManager
    {
        public MenuListManager()
        {
        }

        private static MenuListManager _instance;
        public static MenuListManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MenuListManager();
            }
            return _instance;
        }


        public string GetNenuString(DataSet ds)
        {

            StringBuilder sb = new StringBuilder(2000);
            sb.Append("<div id=\"list3\" >");

            #region foreachMenuID

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["ParentID"]) == 0)
                {
                    sb.Append("<div>");
                    sb.Append("<div class=\"title\">" + dr["Name"].ToString() + "</div>");
                    sb = GetParentMenuString(ds, sb, Util.TrimIntNull(dr["MenuID"]));
                    sb.Append("</div>");
                }

            }
            #endregion

            sb.Append("</div>");

            return sb.ToString();
        }

        private StringBuilder GetParentMenuString(DataSet ds, StringBuilder sb, int menuid)
        {
            sb.Append("<div>");
            sb.Append("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["ParentID"]) == menuid)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append("<a class=\"menuparent\" href=\"javascript:setTopFrameUrl('" + dr["href"].ToString() + "','SiteMapPath.aspx?url=" + dr["Name"].ToString() + "')\">" + dr["Name"].ToString() + "</a>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
            }
            sb.Append("</table>");
            sb.Append("</div>");

            return sb;
        }

        public string GetNenuNavigationString(DataSet ds, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder(2000);
            sb.Append("<ul id=\"navigation\" class=\"navstyle\">");
            StringBuilder _sb = new StringBuilder();

            #region foreachMenuID

            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "ParentID=0";

            foreach (DataRow dr in dv.Table.Rows)
            {
                if (Util.TrimIntNull(dr["ParentID"]) == 0)
                {
                    _sb = new StringBuilder();
                    StringBuilder _subsb = new StringBuilder();
                    _sb.Append("<li>");
                    //sb.Append("<a class=\"head\" onclick=\"javascript:helpClose();\">" + "<img src=\"../Images/jquery/" + dr["Icon"].ToString() + "\" border=\"0\"/>&nbsp;&nbsp;" + dr["Name"].ToString() + "</a>");
                    //_sb.Append("<a herf=\"?n=" + dr["href"].ToString() + "\" class=\"head\" onclick=\"javascript:helpClose();\">" + "<img src=\"../Images/jquery/" + dr["Icon"].ToString() + "\" border=\"0\"/>&nbsp;&nbsp;" + dr["Name"].ToString() + "</a>");
                    _sb.Append("<a id=\"" + dr["href"].ToString() + "\" class=\"head\" onclick=\"javascript:helpClose();\">" + "<img src=\"../Images/jquery/" + dr["Icon"].ToString() + "\" border=\"0\"/>&nbsp;&nbsp;" + dr["Name"].ToString() + "</a>");

                    _subsb = GetParentMenuNavigationString(ds, ht, sb, Util.TrimIntNull(dr["MenuID"]));
                    _sb.Append(_subsb.ToString());
                    _sb.Append("</li>");
                    if (_subsb.ToString() == "")
                    {
                        _sb = new StringBuilder();
                    }
                    else
                    {
                        //_sb.Append(_subsb.ToString());
                    }
                    sb.Append(_sb.ToString());
                }
            }
            #endregion
            sb.Append("</ul>");

            return sb.ToString();
        }

        private StringBuilder GetParentMenuNavigationString(DataSet ds, Hashtable ht, StringBuilder sb, int menuid)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append("<ul>");

            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "ParentID=" + menuid.ToString();
            //dv.RowFilter = "ParentID <> 0";

            foreach (DataRow dr in dv.Table.Rows)
            {
                if (Util.TrimIntNull(dr["ParentID"]) == menuid)
                {
                    string[] keys = dr["Privilege"].ToString().Split(',');
                    bool isPrivilege = false;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (ht.ContainsKey(int.Parse(keys[i])) || int.Parse(keys[i]) == 0)//未设置权限的默认0
                        //if (ht.ContainsKey(int.Parse(keys[i])))
                        {
                            isPrivilege = true;
                            i = keys.Length;
                        }
                    }
                    if (isPrivilege || ht.ContainsKey((int)AppEnum.Privilege.Administrator))
                    {
                        _sb.Append("<li>");
                        //sb.Append("<a href=\"?n=" + dr["href"].ToString() + "\" onclick=\"javascript:setTopFrameUrl('" + dr["href"].ToString() + "?url=" + dr["Name"] + "','SiteMapPath.aspx?url=" + dr["Name"].ToString() + "','" + dr["help"].ToString() + "')\">" + "<img id=\"arrow\" src=\"../Images/jquery/menu_arrow.gif\" border=\"0\"/>" + dr["Name"].ToString() + "</a>");
                        _sb.Append("<a href=\"javascript:void('/" + dr["help"].ToString() + "/');\" class=\"" + dr["help"].ToString() + "\" id=\"" + dr["help"].ToString() + "\" onclick=\"javascript:setTopFrameUrl('" + dr["href"].ToString() + "?url=" + dr["Name"] + "','" + dr["Name"].ToString() + "','" + dr["help"].ToString() + "');this.blur();\">" + "<img id=\"arrow\" src=\"../Images/jquery/menu_arrow.gif\" border=\"0\"/>" + dr["Name"].ToString() + "</a>");
                        _sb.Append("</li>");
                    }
                    else
                    {
                        //sb.Append("<li>");
                        //sb.Append("<a href=\"javascript:void(0);\">无权限</a>");
                        //sb.Append("</li>");
                    }
                }
            }
            _sb.Append("</ul>");
            if (_sb.ToString() != "<ul></ul>")
            {
                return _sb;
            }
            else
            {
                return new StringBuilder();
            }

        }

    }
}
