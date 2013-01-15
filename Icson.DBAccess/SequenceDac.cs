using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;

namespace Icson.DBAccess
{
	/// <summary>
	/// Summary description for Sequence.
	/// </summary>
	public class SequenceDac
	{
		

		private SequenceDac()
		{
		}

		private static SequenceDac _instance;

		public static SequenceDac GetInstance()
		{
			if ( _instance == null )
			{
				_instance = new SequenceDac();
			}
			return _instance;
		}

		public int Create(string paramTable)
		{
            string sql = " insert into " + paramTable + "(createtime) values(getdate());";
            sql += " SELECT SCOPE_IDENTITY()";

            return Convert.ToInt32(SqlHelper.ExecuteScalar(sql));

		}


	}
}
