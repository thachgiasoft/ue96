using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.ImportData;
using Icson.DBAccess;
using Icson.DBAccess.ImportData;
using Icson.DBAccess.Basic;


namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for ManufacturerManager.
	/// </summary>
	public class ManufacturerManager
	{
		private ManufacturerManager()
		{
		}
		private static ManufacturerManager _instance;
		public static ManufacturerManager GetInstance()
		{
			if ( _instance == null )
			{
				_instance = new ManufacturerManager();
			}
			return _instance;
		}
		public void ImportManu()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from Manufacturer ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table Manufacturer is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = "select * from ipp2003..producer";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows )
				{
					ManufacturerInfo oManu = new ManufacturerInfo();

					oManu.ManufacturerID = Util.TrimNull(dr1["ProducerID"]);
					oManu.ManufacturerName = Util.TrimNull(dr1["ProducerName"]);
					oManu.BriefName = Util.TrimNull(dr1["ProducerBriefName"]);

					string orgDesc = "";
					if ( Util.TrimNull(dr1["ProducerDescription"])!=AppConst.StringNull)
						orgDesc += Util.TrimNull(dr1["ProducerDescription"]); 
					
					string address = "";
					if ( Util.TrimNull(dr1["Country"])!=AppConst.StringNull)
						address += Util.TrimNull(dr1["Country"]);
					if ( Util.TrimNull(dr1["City"])!=AppConst.StringNull)
						address += "," + Util.TrimNull(dr1["City"]);
					if ( Util.TrimNull(dr1["Address"])!=AppConst.StringNull)
						address += "," + Util.TrimNull(dr1["Address"]);

					string website = "";
					if ( Util.TrimNull(dr1["Web"])!=AppConst.StringNull)
						website += Util.TrimNull(dr1["Web"]);

					string note = "";
					if ( Util.TrimNull(dr1["Note"])!=AppConst.StringNull)
						note += Util.TrimNull(dr1["Note"]);

					if ( orgDesc != "" )
						note += "; Desc: " + orgDesc;
					if ( address != "" )
						note +="; Address: " + address;
					if ( website != "" )
						note +="; site: " + website;


					oManu.Note = note;
					oManu.Status = Util.TrimIntNull(dr1["Status"]);

					this.Insert(oManu);

					//insert into convert table
					ImportInfo oManuConvert = new ImportInfo();
					oManuConvert.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
					oManuConvert.NewSysNo = oManu.SysNo;
					new ImportDac().Insert(oManuConvert, "Manufacturer");
					
				}
				scope.Complete();
            }
		}
		public int Insert(ManufacturerInfo oParam)
		{
            //string sql = " select top 1 sysno from Manufacturer where ManufacturerID = " + Util.ToSqlString(oParam.ManufacturerID);
            //DataSet ds = SqlHelper.ExecuteDataSet(sql);
            //if ( Util.HasMoreRow(ds) )
            //    throw new BizException("the same Manufacurer ID exists already");

			oParam.SysNo = SequenceDac.GetInstance().Create("Manufacturer_Sequence");
            oParam.ManufacturerID = oParam.SysNo.ToString();
			return new ManufacturerDac().Insert(oParam);
		}
		public int Update(ManufacturerInfo oParam)
		{
			string sql = " select top 1 sysno from Manufacturer where ManufacturerID = " + Util.ToSqlString(oParam.ManufacturerID) + " and sysno <> " + oParam.SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the same Manufacurer ID exists already");

			return new ManufacturerDac().Update(oParam);
		}

        //批量删除生产商
        public int Deletes(string SysNos)
        {
            return new ManufacturerDac().Deletes(SysNos);
        }

		private void map(ManufacturerInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.ManufacturerID = Util.TrimNull(tempdr["ManufacturerID"]);
			oParam.ManufacturerName = Util.TrimNull(tempdr["ManufacturerName"]);
			oParam.BriefName = Util.TrimNull(tempdr["BriefName"]);
			oParam.Note = Util.TrimNull(tempdr["Note"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}

		public ManufacturerInfo Load(string paramID)
		{
			string sql = "select * from Manufacturer where ManufacturerID =" + Util.ToSqlString(paramID);
			return LoadBySql(sql);
		}

		public ManufacturerInfo Load(int paramSysno)
		{
			string sql = "select * from Manufacturer where SysNo = " + paramSysno;
			return LoadBySql(sql);
		}

		private ManufacturerInfo LoadBySql(string sql)
		{
			DataSet ds = SqlHelper.ExecuteDataSet(sql);

			if ( !Util.HasMoreRow(ds))
				return null;

			ManufacturerInfo oManuInfo = new ManufacturerInfo();
			map(oManuInfo, ds.Tables[0].Rows[0]);
			return oManuInfo;

		}
		public DataSet GetManuDs(Hashtable paramHash)
		{
			string sql = " select * from Manufacturer ";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				sb.Append(" where 1=1 " );
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "some key special")
					{
						//special deal
					}
					else if ( item is int)
					{
						sb.Append(key).Append("=" ).Append(item.ToString());
					}
					else if ( item is string)
					{
						sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
					}
				}
				sql += sb.ToString();
			}
			else
			{
				sql = sql.Replace("select", "select top 50");
			}

			sql += " order by sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		/// <summary>
		/// 填充online的CTFilter.ascx
		/// </summary>
		/// <param name="c3SysNo"></param>
		/// <returns></returns>
		public  SortedList GetManuListByC3(int c3SysNo)
		{
			string sql = @"select 
								distinct manufacturer.* 
							from 
								manufacturer, product
							where 
								product.manufacturersysno = manufacturer.sysno
							and product.status = @show
							and manufacturer.status = @valid
							and product.c3sysno = @c3sysno";
			sql = sql.Replace("@show", ((int)AppEnum.ProductStatus.Show).ToString());
			sql = sql.Replace("@valid", ((int)AppEnum.BiStatus.Valid).ToString());
			sql = sql.Replace("@c3sysno", c3SysNo.ToString());

			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;

			SortedList sl = new SortedList(10);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				ManufacturerInfo item = new ManufacturerInfo();
				map(item, dr);
				sl.Add(item, null);
			}
			return sl;
		}
	}
}
