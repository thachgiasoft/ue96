using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;


namespace Icson.Utils
{
    public class DataGridUtil : System.Web.UI.Page
    {
        public DataGridUtil()
        {

        }

        private static DataGridUtil _instance;
        public static DataGridUtil GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DataGridUtil();
            }
            return _instance;
        }

        public void setgridstyle(DataGrid dg, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            dg.CssClass = "GridViewStyle";
            dg.FooterStyle.CssClass = "GridViewFooterStyle";
            dg.ItemStyle.CssClass = "GridViewRowStyle";
            dg.SelectedItemStyle.CssClass = "GridViewSelectedRowStyle";
            dg.PagerStyle.CssClass = "GridViewPagerStyle";
            dg.AlternatingItemStyle.CssClass = "GridViewAlternatingRowStyle";
            dg.HeaderStyle.CssClass = "GridViewHeaderStyle";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight=\"\";");
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor=\"#d8e3e7\",this.style.fontWeight=\"\";");
                //e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor=\"#d8e3e7\",this.style.fontWeight=\"\";jquery_Tools_showthis(this);");
                //e.Item.Attributes.Add("onclick", "javascript:jquery_Tools_showthis(this);");
            }
        }

        /// <summary>
        /// 设置排序图片
        /// </summary>
        /// <param name="dg">DataGrid</param>
        /// <param name="sortexpression">排序字段</param>
        /// <param name="sortmode">排序方式</param>
        /// <param name="applicationpath">应用程序目录</param>
        public void setSortImage(DataGrid dg, string sortexpression, string sortmode, string applicationpath)
        {
            string ImgDown = "<img border=\"0\" src=" + applicationpath + "/images/site/sortarrow_down.gif>";
            string ImgUp = "<img border=\"0\" src=" + applicationpath + "/images/site/sortarrow_up.gif>";

            int colindex = -1;
            //清空之前的图标
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                dg.Columns[i].HeaderText = (dg.Columns[i].HeaderText).ToString().Replace(ImgDown, "");
                dg.Columns[i].HeaderText = (dg.Columns[i].HeaderText).ToString().Replace(ImgUp, "");
            }

            //找到所点击的HeaderText的索引号
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                if (dg.Columns[i].SortExpression == sortexpression)
                {
                    colindex = i;
                    break;
                }
            }

            //设置图标
            if (sortmode == " DESC")
            {
                dg.Columns[colindex].HeaderText = dg.Columns[colindex].HeaderText + ImgDown;
            }
            else
            {
                dg.Columns[colindex].HeaderText = dg.Columns[colindex].HeaderText + ImgUp;
            }


        }


        public void setSortImage(DataGrid dg, string lbID, string sortexpression, string sortmode, string applicationpath)
        {
            string ImgDown = "<img border=\"0\" src=" + applicationpath + "/images/site/sortarrow_down.gif>";
            string ImgUp = "<img border=\"0\" src=" + applicationpath + "/images/site/sortarrow_up.gif>";

            //int colindex = -1;
            ////清空之前的图标
            //for (int i = 0; i < dg.Columns.Count; i++)
            //{
            //    lb.Text = (dg.Columns[i].HeaderText).ToString().Replace(ImgDown, "");
            //    lb.Text = (dg.Columns[i].HeaderText).ToString().Replace(ImgUp, "");
            //}

            ////找到所点击的HeaderText的索引号
            //for (int i = 0; i < dg.Columns.Count; i++)
            //{
            //    if (dg.Columns[i].SortExpression == sortexpression)
            //    {
            //        colindex = i;
            //        break;
            //    }
            //}

            //设置图标

            Table tl = (Table)dg.Controls[0];
            TableRow tr = (TableRow)tl.Rows[0];

            LinkButton lb = (LinkButton)tr.FindControl(lbID);

            if (sortmode == " DESC")
            {
                lb.Text = lb.Text + ImgDown;
            }
            else
            {
                lb.Text = lb.Text + ImgUp;
            }


        }


        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="SB"></param>
        /// <param name="e"></param>
        public void SetSort(System.Web.UI.StateBag SB, DataGridSortCommandEventArgs e)
        {
            if (SB["sort"] == null)
            {
                SB["sort"] = e.SortExpression;
                SB["desc"] = "";
            }
            else if ((SB["sort"].ToString() + SB["desc"].ToString()).ToString() == e.SortExpression)
            {
                SB["sort"] = e.SortExpression;
                SB["desc"] = " DESC";
            }
            else
            {
                SB["sort"] = e.SortExpression;
                SB["desc"] = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SB"></param>
        /// <param name="sortExpression"></param>
        public void SetSort(System.Web.UI.StateBag SB, string sortExpression)
        {
            if (SB["sort"] == null)
            {
                SB["sort"] = sortExpression;
                SB["desc"] = "";
            }
            else if ((SB["sort"].ToString() + SB["desc"].ToString()).ToString() == sortExpression)
            {
                SB["sort"] = sortExpression;
                SB["desc"] = " DESC";
            }
            else
            {
                SB["sort"] = sortExpression;
                SB["desc"] = "";
            }
        }



        public void setGridViewJs(GridView gv, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight=\"\";");
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor=\"#d8e3e7\",this.style.fontWeight=\"\";");
                //e.Row.Attributes.Add("ondblclick", "window.open(\"tProduct_Edit.aspx?ids=" + _info.tProduct_ID.ToString() + "&page=" + GridView1.PageIndex.ToString() + "\",\"_self\");");
            }

        }

        public void setGridViewCss(GridView gv, EventArgs e)
        {
            gv.CssClass = "GridViewStyle";
            gv.FooterStyle.CssClass = "GridViewFooterStyle";
            gv.RowStyle.CssClass = "GridViewRowStyle";
            gv.SelectedRowStyle.CssClass = "GridViewSelectedRowStyle";
            gv.PagerStyle.CssClass = "GridViewPagerStyle";
            gv.AlternatingRowStyle.CssClass = "GridViewAlternatingRowStyle";
            gv.HeaderStyle.CssClass = "GridViewHeaderStyle";
        }

    }


}
