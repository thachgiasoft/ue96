using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Icson.Utils;
using System.Transactions;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

using Icson.Objects;

namespace Icson.BLL.Basic
{
   public class SizeManager
    {
       private SizeManager()
		{
		}
       private static SizeManager _instance;
       public static SizeManager GetInstance()
		{
			if ( _instance == null )
			{
                _instance = new SizeManager();
			}
			return _instance;
		}

       private void map(Size1Info oParam, DataRow tempdr)
       {
           oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
           oParam.Size1ID = Util.TrimNull(tempdr["Size1ID"]);
           oParam.Size1Name = Util.TrimNull(tempdr["Size1Name"]);
           oParam.Status = Util.TrimIntNull(tempdr["Status"]);
           oParam.Size1Type = Util.TrimNull(tempdr["Size1Type"]);
       }

       private void map(Size2Info oParam, DataRow tempdr)
       {
           oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
           oParam.Size1SysNo = Util.TrimIntNull(tempdr["Size1SysNo"]);
           oParam.Size2ID = Util.TrimNull(tempdr["Size2ID"]);
           oParam.Size2Name = Util.TrimNull(tempdr["Size2Name"]);
           oParam.Status = Util.TrimIntNull(tempdr["Status"]);
           oParam.Size2Img = Util.TrimNull(tempdr["Size2Img"]);
       }
       public Size1Info LoadSize1(int SysNo)
       {
           string sql = "select * from size1 where sysno=" + SysNo;
           DataSet ds = SqlHelper.ExecuteDataSet(sql);

           if (!Util.HasMoreRow(ds))
               return null;
           Size1Info oInfo=new Size1Info();
           map(oInfo, ds.Tables[0].Rows[0]);
           return oInfo;
           
       }
       public Size2Info LoadSize2(int SysNo)
       {
           string sql = "select * from size2 where sysno=" + SysNo;
           DataSet ds = SqlHelper.ExecuteDataSet(sql);

           if (!Util.HasMoreRow(ds))
               return null;
           Size2Info oInfo = new Size2Info();
           map(oInfo, ds.Tables[0].Rows[0]);
           return oInfo;

       }

       public void InsertSize1(Size1Info oParam)
       {
           new SizeDac().Insert(oParam);
       }
       public void InsertSize2(Size2Info oParam)
       {
           new SizeDac().Insert(oParam);
       }
       public void UpdateSize1(Size1Info oParam)
       {
          string sql="select * from size1 where Size1ID="+oParam.Size1ID+" and SysNo<>"+oParam.SysNo;
           DataSet ds=SqlHelper.ExecuteDataSet(sql);
           if(Util.HasMoreRow(ds))
               throw new BizException("相同ID编号的尺码已存在");
           new SizeDac().Update(oParam);
       }
       public void UpdateSize2(Size2Info oParam)
       {
           string sql = "select * from size2 where Size2ID=" + oParam.Size2ID + " and SysNo<>" + oParam.SysNo;
           DataSet ds = SqlHelper.ExecuteDataSet(sql);
           if (Util.HasMoreRow(ds))
               throw new BizException("相同ID编号的尺码已存在");
           new SizeDac().Update(oParam);
       }

       public DataSet GetSize1Ds(Hashtable paramHash)
       {
           string sql = "select * from Size1 ";

           if (paramHash != null && paramHash.Count != 0)
           {
               StringBuilder sb = new StringBuilder(100);
               sb.Append(" where 1=1 ");
               foreach (string key in paramHash.Keys)
               {
                   sb.Append(" and ");
                   object item = paramHash[key];
                   if (key == "Size1ID")
                   {
                       sb.Append(key).Append("=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (key == "Size1Name")
                   {
                       sb.Append(key).Append("=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (key == "Status")
                   {
                       sb.Append(key).Append("=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (item is int)
                   {
                       sb.Append(key).Append("=").Append(item.ToString());
                   }
                   else if (item is string)
                   {
                       sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                   }
               }
               sql += sb.ToString();
           }

           sql += " order by size1id";
           return SqlHelper.ExecuteDataSet(sql);
 
       }
       public DataSet GetSize2Ds(Hashtable paramHash)
       {
           string sql = "select Size2.*,size1.Size1Name from Size2 ,Size1 where size1.sysno=size2.size1sysno";

           if (paramHash != null && paramHash.Count != 0)
           {
               StringBuilder sb = new StringBuilder(100);
              
               foreach (string key in paramHash.Keys)
               {
                   sb.Append(" and ");
                   object item = paramHash[key];
                   if (key == "Size2ID")
                   {
                       sb.Append(key).Append("=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (key == "Size2Name")
                   {
                       sb.Append(key).Append("=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (key == "Status")
                   {
                       sb.Append(key).Append("=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (key == "Size1SysNo")
                   {
                       sb.Append(key).Append("=").Append(Util.ToSqlString(item.ToString()));
                   }
                   else if (item is int)
                   {
                       sb.Append(key).Append("=").Append(item.ToString());
                   }
                   else if (item is string)
                   {
                       sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                   }
               }
               sql += sb.ToString();
           }

           sql += " order by size1sysno,Size2ID";
           return SqlHelper.ExecuteDataSet(sql);

       }
    }
}
