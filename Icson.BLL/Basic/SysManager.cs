using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

using Icson.Objects.ImportData;
using Icson.DBAccess.ImportData;

namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for SysManager.
	/// </summary>
	public class SysManager
	{
		private SysManager()
		{
		}
		private static SysManager _instance;
		public static SysManager GetInstance()
		{
			if( _instance == null )
				_instance = new SysManager();
			return _instance;
		}
		public SortedList GetPMList()
		{
            //string pmStr = AppConfig.PMCollection; // 0035:0061
            string pmStr = "";

            Hashtable ht = new Hashtable(5);
            ht.Add("Flag", "1,2,3");
            DataSet ds = SysManager.GetInstance().GetUserDs(ht);
            if (!Util.HasMoreRow(ds))
                pmStr = "";
            else
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    pmStr = pmStr + dr["UserID"].ToString() + ":";
                }
                pmStr = pmStr.Substring(0, pmStr.Length - 1);
            }

			return this.GetUserList(pmStr);
		}

        public DataSet GetPMGroupDs()//09-8-22
        {
            string sql = "select * from PMGroup order by OrderNum";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetPMGroupName(int PMGroupSysNo)
        {
            
            string sql = "select top 1 GroupName from PMGroup where GroupSysNo=" + PMGroupSysNo.ToString();
            DataSet ds=SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";
            else
                return ds.Tables[0].Rows[0]["GroupName"].ToString().Trim();
        }


        public SortedList GetAPMList()
        {
            string pmStr = AppConfig.APMCollection; 
            return this.GetUserList(pmStr);
        }
		public SortedList GetFreightMenList()
		{
			string fmStr = AppConfig.FMCollection;
			return this.GetUserList(fmStr);
		}
		private SortedList GetUserList(string userStr)
		{
			string[] users = userStr.Split(':');

			SortedList sl = new SortedList(users.Length);

			for(int i=0; i<users.Length; i++)
			{
				string userID = users[i];
				UserInfo user = SysManager.GetInstance().LoadUser(userID);
				if ( user == null )
					continue;
				sl.Add(user,null);
			}
			return sl;
		}
		public void ImportPrivilege()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			string sql = " select top 1 * from Sys_Privilege ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table Sys_Privilege is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				SortedList sl = AppEnum.GetPrivilege();
				foreach(int privilegeSysNo in sl.Keys)
				{
					PrivilegeInfo item = new PrivilegeInfo();
					item.SysNo = privilegeSysNo;
					item.PrivilegeID = privilegeSysNo.ToString();
					item.PrivilegeName = AppEnum.GetPrivilege(privilegeSysNo);
					item.Status = (int)AppEnum.BiStatus.Valid;
					new PrivilegeDac().Insert(item);
				}

				scope.Complete();
            }


		}
		public void ImportUser()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from Sys_User ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table User is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = @"select u.sysno, u.usercode, u.userpassword, u.status, e.employeename, e.email, e.memo from ipp2003..employee e, ipp2003..internal_user u
								where e.sysno = u.sysno
								and u.usercode = e.employeecode";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows )
				{
					UserInfo oUser = new UserInfo();

					oUser.SysNo = Util.TrimIntNull(dr1["SysNo"]);
					oUser.UserID = Util.TrimNull(dr1["UserCode"]);
					oUser.UserName = Util.TrimNull(dr1["EmployeeName"]);
					oUser.Pwd = Util.TrimNull(dr1["UserPassword"]);
					oUser.Email = Util.TrimNull(dr1["Email"]);
					oUser.Phone = "";
					oUser.Note = Util.TrimNull(dr1["Memo"]);
					oUser.Status = Util.TrimIntNull(dr1["Status"]);

					this.InsertUser(oUser);

					//insert into convert table
					ImportInfo oUserConvert = new ImportInfo();
					oUserConvert.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
					oUserConvert.NewSysNo = oUser.SysNo;
					new ImportDac().Insert(oUserConvert, "Sys_User");
					
				}
				scope.Complete();
            }
		}

        /// <summary>
        /// 判断用户账号是否重复
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool UserIsExist(string UserID)
        {
            string sql = "select top 1 sysno from sys_user where userid = " + Util.ToSqlString(UserID);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 增加申请新账户的信息
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int InsertRequestAccount(RequestAccountInfo oParam)
        {

            return new UserDac().InsertRequestAccount(oParam);
        }

        //删除用户账号申请
        public int DeleteRequestAccount(int requestSysNo)
        {
            return new UserDac().DeleteRequestAccount(requestSysNo);
        }

        /// <summary>
        /// 标记已处理
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public int MarkRequestAccount(int SysNo)
        {
            return new UserDac().MarkDispose(SysNo);
        }


        public int DeletePMDFlag()
        {
            string sql = "update sys_user set Flag=" + ((int)AppEnum.UserFlag.PM).ToString() + " where Flag=" + ((int)AppEnum.UserFlag.PMD).ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }

		/// <summary>
		/// 增加用户，用户ID不能重复
		/// </summary>
		/// <param name="oParam"></param>
		/// <returns></returns>
		public int InsertUser(UserInfo oParam)
		{
			string sql = "select top 1 sysno from sys_user where userid = " + Util.ToSqlString(oParam.UserID);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);

			if ( Util.HasMoreRow(ds))
				throw new BizException("the same User ID exists");

			oParam.SysNo = SequenceDac.GetInstance().Create("Sys_Sequence");
            if (oParam.Flag == (int)AppEnum.UserFlag.PMD)
                DeletePMDFlag();
			return new UserDac().Insert(oParam);
		}
		/// <summary>
		/// 更新用户，用户ID不能修改
		/// </summary>
		/// <param name="oParam"></param>
		/// <returns></returns>
		public int UpdateUser(UserInfo oParam)
		{
			string sql = "select top 1 sysno from sys_user where sysno = " + oParam.SysNo + " and userid <> " + Util.ToSqlString(oParam.UserID);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow((ds)))
				throw new BizException("can't change User ID");
            if (oParam.Flag == (int)AppEnum.UserFlag.PMD)
                DeletePMDFlag();
			return new UserDac().Update(oParam);
		}

        //删除用户
        public int DeleteUser(int userSysNo)
        {
            int _return = 0;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                _return = new UserDac().DeleteUser(userSysNo);
                scope.Complete();
            }
            return _return;
        }

        /// <summary>
        /// 更新用户，用户ID可以修改
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int UpdateUser_DBC(UserInfo oParam)
        {
            if (oParam.Flag == (int)AppEnum.UserFlag.PMD)
                DeletePMDFlag();
            return new UserDac().Update(oParam);
        }

		/// <summary>
		/// 根据sysno获取用户
		/// </summary>
		/// <param name="paramUserSysNo"></param>
		/// <returns></returns>
		public UserInfo LoadUser(int paramUserSysNo)
		{
			string sql = "select * from sys_user where sysno = " + paramUserSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds) )
				return null;
			UserInfo oUserInfo = new UserInfo();
			map(oUserInfo, ds.Tables[0].Rows[0]);
			return oUserInfo;
		}
		public UserInfo LoadUser(string paramUserID)
		{
			string sql = "select * from sys_user where userid=" + Util.ToSqlString(paramUserID);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			UserInfo oUserInfo = new UserInfo();
			map(oUserInfo, ds.Tables[0].Rows[0]);
			return oUserInfo;
		}
		/// <summary>
		/// 根据hasttable传入的条件，获取user dataset
		/// </summary>
		/// <param name="paramHash"></param>
		/// <returns></returns>
		public DataSet GetUserDs(Hashtable paramHash)
		{
			//select * from product where productid = '26-101-123' and sysno <>234
            string _strTmp = "";
            string sql = " select a.SysNo,a.UserID,a.UserName,a.Pwd,a.Email,a.Phone,a.Note,a.Status,a.MobilePhone,a.DepartmentSysNo,a.Flag,a.PMGroupSysNo,a.StationSysNo,b.TypeName from sys_user as a,sys_OperationType as b where a.DepartMentSysNo=b.SysNo @query1 union select SysNo,UserID,UserName,Pwd,Email,Phone,Note,Status,MobilePhone,DepartmentSysNo,Flag,PMGroupSysNo,StationSysNo,'无所属部门' as TypeName from sys_user where DepartMentSysNo not in(select sysno from sys_operationtype) @query2";
			//判断是否有查询条件传入
			//如果有就append where
			//先把需要特殊处理的过滤掉
			//然后统一处理普通意义的，就是等于的情况
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
                StringBuilder sb1 = new StringBuilder(100);
                //sb.Append(" where 1=1 " );


				foreach(string key in paramHash.Keys)
				{

					object item = paramHash[key];
					if ( key == "NoDept")
					{
						//special deal
                        sql = " select a.SysNo,a.UserID,a.UserName,a.Pwd,a.Email,a.Phone,a.Note,a.Status,a.MobilePhone,'无所属部门' as TypeName from sys_user as a where a.DepartMentSysNo not in(select sysno from sys_operationtype) @query1";
					}
					else if ( item is int)
					{
                        sb.Append(" and ");
                        sb1.Append(" and ");

                        sb.Append("a."+key).Append("=").Append(item.ToString());
						sb1.Append(key).Append("=" ).Append(item.ToString());
					}
                    else if (key == "Flag")
                    {
                        sb.Append(" and ");
                        sb1.Append(" and ");



                        switch (item.ToString())
                        {
                            case "normal":
                                sb.Append("a." + key).Append(" is null or a." + key + "<0");
                                sb1.Append(key).Append(" is null or " + key + "<0");
                                break;
                            default:
                                sb.Append("a." + key).Append(" in (" + item.ToString() + ")");
                                sb1.Append(key).Append(" in (" + item.ToString() + ")");
                                break;
                        }
                    }
					else if ( item is string)
					{
                        sb.Append(" and ");
                        sb1.Append(" and ");
                        sb.Append("a."+key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
						sb1.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
					}
				}
                //sql += sb.ToString();
                _strTmp = sql.Replace("@query1",sb.ToString()).Replace("@query2",sb1.ToString());
                sql = _strTmp;
                
			}
			else
			{
                //sql = sql.Replace("select", "select top 50");
                sql = _strTmp = sql.Replace("@query1","").Replace("@query2", "");
			}
			sql += " order by a.sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

        /// <summary>
        /// 根据hasttable传入的条件，获取申请账号 dataset
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetRequestAccountDs(Hashtable paramHash)
        {
            string sql = " select * from sys_RequestAccount ";

            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                sb.Append(" where 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "some key special")
                    {
                        //special deal
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
                sql += sb.ToString()+" order by RequestTime desc";
            }
            return SqlHelper.ExecuteDataSet(sql);
        }

		/// <summary>
		/// 增加角色，角色ID不能重复
		/// </summary>
		/// <param name="oParam"></param>
		/// <returns></returns>
		public int InsertRole(RoleInfo oParam)
		{
			string sql = "select top 1 sysno from sys_role where roleid = " + Util.ToSqlString((oParam.RoleID));
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow((ds)))
				throw new BizException("the same Role ID exists");

			oParam.SysNo = SequenceDac.GetInstance().Create("Sys_Sequence");

			return new RoleDac().Insert(oParam);
		}

        /// <summary>
        /// 增加角色，角色ID不能重复,增加了部门
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int InsertRole_DBC(RoleInfo oParam,string paramDelSql)
        {
            //string sql = "select top 1 sysno from sys_role where roleid = " + Util.ToSqlString((oParam.RoleID));
            //DataSet ds = SqlHelper.ExecuteDataSet(sql);
            //if (Util.HasMoreRow((ds)))
            //    throw new BizException("the same Role ID exists");

            //oParam.SysNo = SequenceDac.GetInstance().Create("Sys_Sequence");

            //return new RoleDac().Insert(oParam);

            int _return = 0;

            string[] strPrivilege = paramDelSql.Split(',');
            oParam.SysNo = SequenceDac.GetInstance().Create("Sys_Sequence");
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                _return = new RoleDac().Insert_DBC(oParam);
                _return = new RoleDac().DelPrivilegeRole(paramDelSql, oParam.SysNo);
                for (int i = 0; i < strPrivilege.Length; i++)
                {
                    if (Util.IsNumber(strPrivilege[i]))
                    _return = new RoleDac().InsertPrivilegeRole(oParam.SysNo, Int16.Parse(strPrivilege[i]));
                }
                scope.Complete();
            }
            return _return;

        }

		/// <summary>
		/// 更新角色
		/// </summary>
		/// <param name="oParam"></param>
		/// <returns></returns>
		public int UpdateRole(RoleInfo oParam)
		{
			string sql = "select * from sys_role where sysno <> " + oParam.SysNo + " and roleid = " + Util.ToSqlString(oParam.RoleID);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				throw new BizException("the same Role ID exists");
			return new RoleDac().Update(oParam);
		}

        /// <summary>
        /// 更新角色,添加的部门
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int UpdateRole_DBC(RoleInfo oParam, string paramDelSql)
        {
            int _return = 0;

            string[] strPrivilege = paramDelSql.Split(',');
            //oParam.SysNo = SequenceDac.GetInstance().Create("Sys_Sequence");
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                _return = new RoleDac().Update_DBC(oParam);
                _return = new RoleDac().DelPrivilegeRole(paramDelSql, oParam.SysNo);
                for (int i = 0; i < strPrivilege.Length; i++)
                {
                    if (Util.IsNumber(strPrivilege[i]))
                        _return = new RoleDac().InsertPrivilegeRole(oParam.SysNo, Int16.Parse(strPrivilege[i]));
                }
                scope.Complete();
            }
            return _return;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int DeleteRole(int SysNo)
        {
            int _return = 0;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                _return = new RoleDac().DeleteRole(SysNo);
                scope.Complete();
            }
            return _return;
        }
        
		/// <summary>
		/// 根据SysNo获取角色
		/// </summary>
		/// <param name="paramRoleSysNo"></param>
		/// <returns></returns>
		public RoleInfo LoadRole(int paramRoleSysNo)
		{
			string sql = "select * from sys_role where sysno = " + paramRoleSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds) )
				return null;
			RoleInfo oRoleInfo = new RoleInfo();
			map(oRoleInfo, ds.Tables[0].Rows[0]);
			return oRoleInfo;
		}
		/// <summary>
		/// 根据传入的Hashtable，获取权限的Hashtable
		/// </summary>
		/// <param name="paramHash"></param>
		/// <returns></returns>
		public Hashtable GetPrivilegeHt(Hashtable paramHash)
		{
			DataSet ds = GetPrivilegeDs(paramHash);
			if ( !Util.HasMoreRow(ds))
				return null;
			Hashtable ht = new Hashtable(20);
			foreach(DataRow dr in ds.Tables[0].Rows )
			{
				PrivilegeInfo item = new PrivilegeInfo();
				map(item, dr);
				ht.Add(item, null);
			}
			return ht;
		}
		public Hashtable GetRoleHt(Hashtable paramHash)
		{
			DataSet ds = GetRoleDs(paramHash);
			if ( !Util.HasMoreRow(ds))
				return null;
			Hashtable ht = new Hashtable(20);
			foreach( DataRow dr in ds.Tables[0].Rows )
			{
				RoleInfo item = new RoleInfo();
				map(item, dr);
				ht.Add(item, null);
			}
			return ht;
		}


        public Hashtable GetRoleHtByDept(Hashtable paramHash)
        {
            DataSet ds = GetRoleDsByDept(paramHash);
            if (!Util.HasMoreRow(ds))
                return null;
            Hashtable ht = new Hashtable(20);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RoleInfo item = new RoleInfo();
                map(item, dr);
                ht.Add(item, null);
            }
            return ht;
        }

		public DataSet GetPrivilegeDsByUser(int paramUserSysNo)
		{

			string sql = @"select distinct c.privilegeid, c.privilegename from
							sys_user as a, sys_role as b, sys_privilege as c,
							sys_user_role as ab, sys_role_privilege as bc
							where 
							a.sysno = ab.usersysno and b.sysno = ab.rolesysno and
							b.sysno = bc.rolesysno and c.sysno = bc.privilegesysno ";
			sql = sql + " and a.sysno=" + paramUserSysNo;
			return SqlHelper.ExecuteDataSet(sql);
		}
		public Hashtable GetPrivilegeHtByUser(int paramUserSysNo)
		{
			string sql = @"select distinct c.* from
							sys_user as a, sys_role as b, sys_privilege as c,
							sys_user_role as ab, sys_role_privilege as bc
							where 
							a.sysno = ab.usersysno and b.sysno = ab.rolesysno and
							b.sysno = bc.rolesysno and c.sysno = bc.privilegesysno ";
			sql = sql + " and a.sysno=" + paramUserSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql); 
			if ( !Util.HasMoreRow(ds))
				return null;
			Hashtable ht = new Hashtable(10);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				PrivilegeInfo item = new PrivilegeInfo();
				map(item, dr);
				ht.Add(item.SysNo, item);
			}
			return ht;

		}
		/// <summary>
		/// 根据角色SysNo，获取本角色对应的Hasttable权限
		/// </summary>
		/// <param name="paramRoleSysNo"></param>
		/// <returns></returns>
		public Hashtable GetPrivilegeHtByRole(int paramRoleSysNo)
		{
			DataSet ds = GetPrivilegeDsByRole(paramRoleSysNo);
			if ( !Util.HasMoreRow(ds))
				return null;
			Hashtable ht = new Hashtable(20);
			foreach(DataRow dr in ds.Tables[0].Rows )
			{
				RolePrivilegeInfo item = new RolePrivilegeInfo();
				map(item, dr);
				ht.Add(item, null);
			}
			return ht;
		}

        public DataSet GetPrivilegeDsByRole(int paramRoleSysNo)
        {
            string sql = "select b.sysno, b.rolesysno, b.privilegesysno, a.privilegeid, a.privilegename from sys_privilege as a, sys_role_privilege as b where a.sysno = b.PrivilegeSysNo and b.RoleSysNo = " + paramRoleSysNo;
            return SqlHelper.ExecuteDataSet(sql);
        }


        public DataSet GetPrivilegeDsByPrivilegeID(int PrivilegeID)
        {
            string sql = "select * from  Sys_Privilege where PrivilegeID = " + PrivilegeID;
            return SqlHelper.ExecuteDataSet(sql);
        }


		public Hashtable GetRoleHtByUser(int paramUserSysNo)
		{
			string sql = " select b.sysno, b.usersysno, b.rolesysno, a.roleid, a.rolename from sys_role as a, sys_user_role as b where a.sysno = b.rolesysno and b.usersysno =" + paramUserSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			Hashtable ht = new Hashtable(20);
			foreach(DataRow dr in ds.Tables[0].Rows )
			{
				UserRoleInfo item = new UserRoleInfo();
				map(item, dr);
				ht.Add(item, null);
			}
			return ht;
		}
		/// <summary>
		/// 根据传入的Hashtable， 获取权限的DataSet
		/// </summary>
		/// <param name="paramHash"></param>
		/// <returns></returns>
		public DataSet GetPrivilegeDs(Hashtable paramHash)
		{
			string sql = " select * from sys_privilege ";
			//判断是否有查询条件传入
			//如果有就append where
			//先把需要特殊处理的过滤掉
			//然后统一处理普通意义的，就是等于的情况
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
			return SqlHelper.ExecuteDataSet(sql);
		}

        /// <summary>
        /// 根据部门， 获取权限的HashTable
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public Hashtable GetPrivilegeHtbyDept(Hashtable paramHash)
        {
            DataSet ds = GetPrivilegeDsbyDept(paramHash);
            if (!Util.HasMoreRow(ds)) 
                return null;
            Hashtable ht=new Hashtable(20);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PrivilegeInfo item = new PrivilegeInfo();
                map(item, dr);
                ht.Add(item, null);
            }
            return ht;
        }

        /// <summary>
        /// 获取指定部门已拥有的权限
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public Hashtable GetPrivilegeHtbyDept(int paramSysNo)
        {
            DataSet ds = GetPrivilegeDsbyDept(paramSysNo);
            if (!Util.HasMoreRow(ds))
                return null;
            Hashtable ht = new Hashtable(20);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PrivilegeInfo item = new PrivilegeInfo();
                map(item, dr);
                ht.Add(item, null);
            }
            return ht;
        }


        /// <summary>
        /// 根据部门， 获取权限的DataSet
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetPrivilegeDsbyDept(Hashtable paramHash)
        {
 
            //========================================
            string sql = "";
            //是否不显示前部的部门名称
            //bool deldept = false;

            //if (paramHash["deldept"] != null)
            //{
            //    if (paramHash["deldept"].ToString().Trim() == "1")
            //        deldept = true;
            //}


            //if (deldept)
            //    sql = " select sysno,privilegeid,privilegename,status from sys_privilege where sysno not in(select privilegeid from sys_operationtype_privilege) union all select a.sysno as sysno,a.privilegeid as privilegeid,'【'+c.typename+'】 '+a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=c.sysno order by sysno";
            //else
            //    sql=" select sysno,privilegeid,'【无所属部门】 '+privilegename as privilegename,status from sys_privilege where sysno not in(select privilegeid from sys_operationtype_privilege) union all select a.sysno as sysno,a.privilegeid as privilegeid,'【'+c.typename+'】 '+a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=c.sysno order by sysno";

            
            //if (paramHash != null && paramHash.Count != 0)
            //{

            //    if (paramHash["havesel"].ToString().Length != 0)
            //        if(deldept)
            //            sql = " select sysno,privilegeid,privilegename,status from sys_privilege where sysno not in(select privilegeid from sys_operationtype_privilege) and (sysno not in(" + paramHash["havesel"].ToString() + ")) union all select a.sysno as sysno,a.privilegeid as privilegeid,'【'+c.typename+'】 '+a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=c.sysno and a.sysno not in (" + paramHash["havesel"].ToString() + ") order by sysno";
            //        else
            //            sql = " select sysno,privilegeid,'【无所属部门】 '+privilegename as privilegename,status from sys_privilege where sysno not in(select privilegeid from sys_operationtype_privilege) and (sysno not in(" + paramHash["havesel"].ToString() + ")) union all select a.sysno as sysno,a.privilegeid as privilegeid,'【'+c.typename+'】 '+a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=c.sysno and a.sysno not in (" + paramHash["havesel"].ToString() + ") order by sysno";

            // if (paramHash["opt"].ToString() == "all")
            //    {
            //        if (paramHash["dept"].ToString() != "all" && Util.IsNumber(paramHash["dept"].ToString()))
            //        {
            //            if(paramHash["havesel"].ToString().Length!=0)
            //                if(deldept)
            //                    sql = "select a.sysno as sysno,a.privilegeid as privilegeid,a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=" + paramHash["dept"].ToString() + " and b.operationtypeid=c.sysno and (a.sysno not in(" + paramHash["havesel"].ToString() + ")) order by a.sysno";
            //                else
            //                    sql = "select a.sysno as sysno,a.privilegeid as privilegeid, '【'+c.typename+'】 '+a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=" + paramHash["dept"].ToString() + " and b.operationtypeid=c.sysno and (a.sysno not in(" + paramHash["havesel"].ToString() + ")) order by a.sysno";
            //            else
            //                if(deldept)
            //                    sql = "select a.sysno as sysno,a.privilegeid as privilegeid, a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=" + paramHash["dept"].ToString() + " and b.operationtypeid=c.sysno order by a.sysno";
            //                else
            //                    sql = "select a.sysno as sysno,a.privilegeid as privilegeid, '【'+c.typename+'】 '+a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=" + paramHash["dept"].ToString() + " and b.operationtypeid=c.sysno order by a.sysno";
                        
            //        }
            //    }
            //    else
            //    {
            //        if(paramHash["havesel"].ToString().Length!=0)
            //            if(deldept)
            //                sql = "select sysno,privilegeid,privilegename,status from sys_privilege where sysno not in(select privilegeid from sys_operationtype_privilege) and sysno not in ("+paramHash["havesel"].ToString()+") order by sysno";
            //            else
            //                sql = "select sysno,privilegeid,'【无所属部门】  '+privilegename as privilegename,status from sys_privilege where sysno not in(select privilegeid from sys_operationtype_privilege) and sysno not in ("+paramHash["havesel"].ToString()+") order by sysno";
            //        else
            //            if(deldept)
            //                sql = "select sysno,privilegeid,privilegename,status from sys_privilege where sysno not in(select privilegeid from sys_operationtype_privilege) order by sysno";
            //            else
            //                sql = "select sysno,privilegeid,'【无所属部门】  '+privilegename as privilegename,status from sys_privilege where sysno not in(select privilegeid from sys_operationtype_privilege) order by sysno";
            //    }
 
            //}

            if (paramHash["havesel"].ToString().Length != 0)
                if (paramHash["dept"].ToString() != "all" && Util.IsNumber(paramHash["dept"].ToString()))
                    sql = "select a.sysno as sysno,a.privilegeid as privilegeid,a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=" + paramHash["dept"].ToString() + " and b.operationtypeid=c.sysno and (a.sysno not in(" + paramHash["havesel"].ToString() + ")) order by a.sysno";
                else
                    sql = "select sysno,privilegeid,privilegename,status from sys_privilege where sysno not in (" + paramHash["havesel"].ToString() + ") order by sysno";
            else
                if (paramHash["dept"].ToString() != "all" && Util.IsNumber(paramHash["dept"].ToString()))
                    sql = "select a.sysno as sysno,a.privilegeid as privilegeid, a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=" + paramHash["dept"].ToString() + " and b.operationtypeid=c.sysno order by a.sysno";
                else
                    sql = "select sysno,privilegeid,privilegename,status from sys_privilege order by sysno";

            return SqlHelper.ExecuteDataSet(sql);

        }

        /// <summary>
        /// 获取指定部门拥有的权限
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetPrivilegeDsbyDept(int paramSysNo)
        {

            //========================================
            string sql = "";




            sql = sql = "select a.sysno as sysno,a.privilegeid as privilegeid, '【'+c.typename+'】 '+a.privilegename as privilegename,a.status as status from sys_privilege as a,sys_Operationtype_privilege as b,sys_operationtype as c where a.sysno=b.privilegeid and b.operationtypeid=" + paramSysNo + " and b.operationtypeid=c.sysno order by a.sysno";


            return SqlHelper.ExecuteDataSet(sql);

        }


		/// <summary>
		/// 根据传入的Hashtable，获取角色的DataSet
		/// </summary>
		/// <param name="paramHash"></param>
		/// <returns></returns>
		public DataSet GetRoleDs(Hashtable paramHash)
		{
			string sql = " select * from sys_role ";

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
			return SqlHelper.ExecuteDataSet(sql);
		}
		/// <summary>
		/// Map UserInfo
		/// </summary>
		/// <param name="oParam"></param>
		/// <param name="tempdr"></param>
		private void map(UserInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.UserID = Util.TrimNull(tempdr["UserID"]);
			oParam.UserName = Util.TrimNull(tempdr["UserName"]);
			oParam.Pwd = Util.TrimNull(tempdr["Pwd"]);
			oParam.Email = Util.TrimNull(tempdr["Email"]);
			oParam.Phone = Util.TrimNull(tempdr["Phone"]);
			oParam.Note = Util.TrimNull(tempdr["Note"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.DepartmentSysNo = Util.TrimIntNull(tempdr["DepartmentSysNo"]);
            oParam.BranchSysNo = Util.TrimIntNull(tempdr["BranchSysNo"]);
            oParam.StationSysNo = Util.TrimIntNull(tempdr["StationSysNo"]);
		}
		/// <summary>
		/// Map RoleInfo
		/// </summary>
		/// <param name="oParam"></param>
		/// <param name="tempdr"></param>
		private void map(RoleInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.RoleID = Util.TrimNull(tempdr["RoleID"]);
			oParam.RoleName = Util.TrimNull(tempdr["RoleName"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
		/// <summary>
		/// Map PrivilegeInfo
		/// </summary>
		/// <param name="oParam"></param>
		/// <param name="tempdr"></param>
		private void map(PrivilegeInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.PrivilegeID = Util.TrimNull(tempdr["PrivilegeID"]);
			oParam.PrivilegeName = Util.TrimNull(tempdr["PrivilegeName"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
		/// <summary>
		/// Map Role&Privilege relation info
		/// </summary>
		/// <param name="oParam"></param>
		/// <param name="tempdr"></param>
		private void map(RolePrivilegeInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.RoleSysNo = Util.TrimIntNull(tempdr["RoleSysNo"]);
			oParam.PrivilegeSysNo = Util.TrimIntNull(tempdr["PrivilegeSysNo"]);
			oParam.PrivilegeID = Util.TrimNull(tempdr["PrivilegeID"]);
			oParam.PrivilegeName = Util.TrimNull(tempdr["PrivilegeName"]);
		}
		private void map(UserRoleInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.UserSysNo = Util.TrimIntNull(tempdr["UserSysNo"]);
			oParam.RoleSysNo = Util.TrimIntNull(tempdr["RoleSysNo"]);
			oParam.RoleID = Util.TrimNull(tempdr["RoleID"]);
			oParam.RoleName = Util.TrimNull(tempdr["RoleName"]);

		}

        /// <summary>
        /// Map OperationInfo
        /// </summary>
        /// <param name="oParam"></param>
        /// <param name="tempdr"></param>
        private void map(OperationTypeInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.TypeName = Util.TrimNull(tempdr["TypeName"]);
            oParam.TypeDescription = Util.TrimNull(tempdr["TypeDescription"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

		/// <summary>
		/// 加入Role Privilege关系
		/// </summary>
		/// <param name="paramRoleSysNo"></param>
		/// <param name="paramPrivilegeSysNo"></param>
		/// <returns></returns>
		public int InsertRolePrivilege(int paramRoleSysNo, int paramPrivilegeSysNo)
		{
			string sql = "select top 1 sysno from sys_role_privilege where rolesysno = " + paramRoleSysNo + " and privilegesysno = " + paramPrivilegeSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				throw new BizException("this role owns the same privilege");
			return new UserRolePrivilegeDac().RolePrivilegeInsert(paramRoleSysNo, paramPrivilegeSysNo);
		}

        /// <summary>
        /// 添加部门。
        /// </summary>
        /// <param name="paramRolePrivilegeSysNo"></param>
        /// <returns></returns>
        public int InsertDeptInfo(OperationTypeInfo oParam,string paramDelSql)
        {
            bool isAdd = false;
            int _return = 0;

            if (oParam.SysNo == -1) isAdd = true;

            string[] strPrivilege = paramDelSql.Split(',');
            if(isAdd) oParam.SysNo = SequenceDac.GetInstance().Create("Sys_Sequence");
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                _return = new DepartMentDC().InsertOrUpdateDepart(oParam,isAdd);
                _return = new DepartMentDC().DelPrivilegeDept(paramDelSql,oParam.SysNo);
                for (int i = 0; i < strPrivilege.Length; i++)
                {
                    if (Util.IsNumber(strPrivilege[i]))
                        _return = new DepartMentDC().InsertPrivilegeDept(oParam.SysNo, Int16.Parse(strPrivilege[i]));
                }
                scope.Complete();
            }
            return _return;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int DeleteDept(int SysNo)
        {
            int _return = 0;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                _return = new DepartMentDC().DelDept(SysNo);
                scope.Complete();
            }
            return _return;
        }


		/// <summary>
		/// 删除Role privilege关系。
		/// </summary>
		/// <param name="paramRolePrivilegeSysNo"></param>
		/// <returns></returns>
		public int DeleteRolePrivilege(int paramRolePrivilegeSysNo)
		{
			return new UserRolePrivilegeDac().RolePrivilegeDelete(paramRolePrivilegeSysNo);
		}
		public int InsertUserRole(int paramUserSysNo, int paramRoleSysNo)
		{
			string sql = "select top 1 sysno from sys_user_role where rolesysno =" + paramRoleSysNo + " and usersysno = " + paramUserSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				throw new BizException("this user owns the same role");
			return new UserRolePrivilegeDac().UserRoleInsert(paramUserSysNo, paramRoleSysNo);
		}
		public int DeleteUserRole(int paramUserRoleSysNo)
		{
			return new UserRolePrivilegeDac().UserRoleDelete(paramUserRoleSysNo);
		}

        /// <summary>
        /// 批量更新用户所拥有角色
        /// </summary>
        /// <returns></returns>
        public int UpdateUserRole(int paramUserSysNo,string paramRoles)
        {
            int _return = 0;

            string[] strRoles = paramRoles.Split(',');

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                _return = new UserRolePrivilegeDac().TheUserRoleDelete(paramUserSysNo);
                for (int i = 0; i < strRoles.Length; i++)
                {
                    if (Util.IsNumber(strRoles[i]))
                        _return = new UserRolePrivilegeDac().UserRoleInsert(paramUserSysNo,Convert.ToInt32(strRoles[i]));
                }
                scope.Complete();
            }
            return _return;
        }

		public SortedList GetFavoriteLinkList(int userSysNo)
		{
			string sql = "select top 10 * from Sys_User_FavoriteLink where userSysNo = " + userSysNo;
			
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			SortedList sl = new SortedList(10);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				UserFavoriteLinkInfo oItem = new UserFavoriteLinkInfo();
				map(oItem, dr);
				sl.Add(oItem,null);
			}
			return sl;

		}
		private void map(UserFavoriteLinkInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.UserSysNo = Util.TrimIntNull(tempdr["UserSysNo"]);
			oParam.LinkName = Util.TrimNull(tempdr["LinkName"]);
			oParam.LinkAhref = Util.TrimNull(tempdr["LinkAhref"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
		}
		public int InsertUserFavoriteLink(UserFavoriteLinkInfo oParam)
		{
			return new UserFavoriteLinkDac().Insert(oParam);
		}
		public int DeleteUserFavoriteLink(int sysno)
		{
			return new UserFavoriteLinkDac().Delete(sysno);
		}

        public DataSet GetLowerRankManByBossSysNo(int BossSysNo)
        {
            string sql = "select * from sys_user where bosssysno=" + BossSysNo + " ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public DataSet GetUserDepartmentList(int departmentsysno)
        {
            //string sql = "select * from sys_user where departmentsysno=" + departmentsysno +"and status=0";
            string sql = "select * from sys_user where departmentsysno=" + departmentsysno + "and status=0";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public string GetUserName(int UserSysNo)
        {
            string sql = "select * from sys_user where sysno=" + UserSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return Util.TrimNull(ds.Tables[0].Rows[0]["UserName"]);
            else
                return "";
        }


        public Hashtable GetUserHt(Hashtable paramHash)
        {
            DataSet ds = GetUserDs(paramHash);
            if (!Util.HasMoreRow(ds))
                return null;
            Hashtable ht = new Hashtable(20);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PrivilegeInfo item = new PrivilegeInfo();
                map(item, dr);
                ht.Add(item, null);
            }
            return ht;
        }

        public Hashtable GetDepartmentDs()
        {
            string sql = " select distinct DepartmentSysNo from Sys_User where 1=1 and DepartmentSysNo is not null  and Status=" + (int)AppEnum.BiStatus.Valid;
            //判断是否有查询条件传入
            //如果有就append where
            //先把需要特殊处理的过滤掉
            //然后统一处理普通意义的，就是等于的情况
  
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            Hashtable ht = new Hashtable(10);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["DepartmentSysNo"]), AppEnum.GetDepartmentID(Util.TrimIntNull(dr["DepartmentSysNo"])));
            }
            return ht;
        }
        public Hashtable GetDepartmentValidUserDs(int DepartmentSysNo)
        {
            string sql = " select * from Sys_User where 1=1 and Status="+(int)AppEnum.BiStatus.Valid +"and DepartmentSysNo="+DepartmentSysNo;
            sql += "order by username";

           DataSet ds=SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            Hashtable ht = new Hashtable(10);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                UserInfo item = new UserInfo();
                map(item, dr);
                ht.Add(item.SysNo, item);
            }
            return ht;
        }

        /// <summary>
        /// 获取操作类型HTML格式
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public string GetOperationTypeStr()
        {

            Hashtable ht = new Hashtable(5);
            DataSet ds = GetOperationTypeDs(ht);
            StringBuilder sb = new StringBuilder();
            int int_index=3;
            string strContent = "";
            string strTips = "";
            //加入固定的“全部”单元行
            sb.Append("<tr id='id1' tag='all' onmouseover='TR_over(this)' onmouseout='TR_out(this)' onclick='TR_sel(this)'>\n");
            sb.Append("<td align='left' dypop='<span color=#151515>显示所有部门角色</span>'>【全部】</td>\n");
            sb.Append("</tr>\n");
            sb.Append("<tr id='id2' tag='none' class='alt' onmouseover='TR_over(this)' onmouseout='TR_out(this)' onclick='TR_sel(this)'>\n");
            sb.Append("<td align='left' dypop='<span color=#151515>显示无所属部门角色</span>'>【无所属部门】</td>\n");
            sb.Append("</tr>\n");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["Status"])!=(int)AppEnum.BiStatus.Valid) continue;
                strContent = "";
                if (int_index % 2 == 0)
                    strContent = " class='alt' ";

                //如果为偶数行，则加入样式
                sb.Append("<tr id='id" + int_index.ToString() + "'" + strContent + " tag='" + Util.TrimIntNull(dr["SysNo"]).ToString() + "' onmouseover='TR_over(this)' onmouseout='TR_out(this)' onclick='TR_sel(this)'>\n");
                strContent = "";

                strContent = Util.TrimNull(dr["TypeName"]);

                strTips = Util.TrimNull(dr["TypeDescription"]);
                if (strTips != "")
                    sb.Append("<td align='left' dypop='<span color=#151515>" + Util.TrimNull(dr["TypeDescription"]) + "</span>'>" + strContent + "</td>\n");
                else
                    sb.Append("<td align='left'>" + strContent + "</td>\n");
                strTips = "";
                sb.Append("</tr>\n");
                int_index++;
            }


            return sb.ToString();
        }

        /// <summary>
        /// 获取操作类型Dateset
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public Hashtable GetOperationTypeHt(Hashtable paramHash)
        {
            DataSet ds = GetOperationTypeDs(paramHash);
            if(!Util.HasMoreRow(ds))
                return null;
            Hashtable ht=new Hashtable();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                OperationTypeInfo item = new OperationTypeInfo();
                map(item, dr);
                ht.Add(item, null);
            }
            return ht;

        }

        /// <summary>
        /// 获取操作类型Dateset
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetOperationTypeDs(Hashtable paramHash)
        {
            string sql = " select SysNo,TypeName,TypeDescription,Status from sys_operationtype";

            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                sb.Append(" where 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "some key special")
                    {
                        //special deal
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

             sql += " order by SysNo";
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取配送员分站点
        /// </summary>
        /// <returns></returns>
        public DataSet GetStation()
        {
            string sql = "select * from sys_station order by ordernum";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetStationName(int StationSysNo)
        {
            string sql = "select stationname from sys_station where stationsysno=" + StationSysNo.ToString();
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";
            else
                return ds.Tables[0].Rows[0]["stationname"].ToString().Trim();
        }

        /// <summary>
        /// 获取指定部门的角色,无其他条件
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetTheRoleDs(string OptSysNo)
        {

            if (!Util.IsNumber(OptSysNo)&&OptSysNo!="none")
            {
                OptSysNo = "all";
            }

            string sql = "select a.SysNo,a.RoleID,a.RoleName,a.Status,b.TypeName,isnull(a.OperationTypeID,'') as OperationTypeID from sys_role as a,sys_OperationType as b where a.OperationTypeID=b.SysNo union select SysNo,RoleID,RoleName,Status,'无所属部门' as TypeName,OperationTypeID from sys_Role where isnull(OperationTypeID,-10) not in (select SysNo from sys_OperationType) order by a.sysno";
            switch (OptSysNo)
            {
                case "all":
                    break;
                case "none":
                    sql = "select SysNo,RoleID,RoleName,OperationTypeID,Status,'无所属部门' as TypeName from sys_Role where isnull(OperationTypeID,-10) not in (select SysNo from sys_OperationType) order by sysno";
                    
                    break;
                default:
                    sql = "select a.SysNo,a.RoleID,a.RoleName,a.Status,b.TypeName,isnull(a.OperationTypeID,'') as OperationTypeID from sys_role as a,sys_OperationType as b where a.OperationTypeID=b.SysNo and a.OperationTypeID=" + OptSysNo + " order by a.sysno";
                    
                    break;
            }
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 根据部门获取角色
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetRoleDsByDept(Hashtable paramHash)
        {
            string _havesel="";
            string _dept="-1";
            string sql = "select a.SysNo,a.RoleID,'【'+b.TypeName+'】'+a.RoleName as RoleName,a.Status,b.TypeName,isnull(a.OperationTypeID,'') as OperationTypeID from sys_role as a,sys_OperationType as b where a.OperationTypeID=b.SysNo union select SysNo,RoleID,'【无所属部门】'+RoleName as RoleName,Status,'无所属部门' as TypeName,OperationTypeID from sys_Role where isnull(OperationTypeID,-10) not in (select SysNo from sys_OperationType) order by a.sysno";
            if (paramHash != null && paramHash.Count != 0)
            {
                if (paramHash["havesel"] != null)
                {
                    _havesel = paramHash["havesel"].ToString().Trim();
                }
                
                if(paramHash["dept"]!=null)
                {
                    if (Util.IsNumber(paramHash["dept"].ToString().Trim()) && paramHash["dept"].ToString().Trim()!="-10")
                    {
                        _dept=paramHash["dept"].ToString().Trim();
                    }
                }

                if (_havesel == "")
                {
                    if(_dept!="-1")
                        sql = "select a.SysNo,a.RoleID,'【'+b.TypeName+'】'+a.RoleName as RoleName,a.Status,b.TypeName,isnull(a.OperationTypeID,'') as OperationTypeID from sys_role as a,sys_OperationType as b where a.OperationTypeID=b.SysNo and a.OperationTypeID=" + _dept + " order by a.sysno";
                }
                else
                {
                    if(_dept=="-1")
                        sql = "select a.SysNo,a.RoleID,'【'+b.TypeName+'】'+a.RoleName as RoleName,a.Status,b.TypeName,isnull(a.OperationTypeID,'') as OperationTypeID from sys_role as a,sys_OperationType as b where a.OperationTypeID=b.SysNo and a.SysNo not in(" + paramHash["havesel"].ToString().Trim() + ") union select SysNo,RoleID,'【无所属部门】'+RoleName as RoleName,Status,'无所属部门' as TypeName,OperationTypeID from sys_Role where SysNo not in(" + paramHash["havesel"].ToString().Trim() + ") and isnull(OperationTypeID,-10) not in (select SysNo from sys_OperationType) order by a.sysno";
                    else
                        sql = "select a.SysNo,a.RoleID,'【'+b.TypeName+'】'+a.RoleName as RoleName,a.Status,b.TypeName,isnull(a.OperationTypeID,'') as OperationTypeID from sys_role as a,sys_OperationType as b where a.SysNo not in(" + paramHash["havesel"].ToString().Trim() + ") and a.OperationTypeID=b.SysNo and a.OperationTypeID=" + _dept + " order by a.sysno";
                }
            }
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetMenuDs()
        {
            string sql = " select * from sys_menu ";
            sql += " where Status <> -1 ";
            sql += " order by OrderNum,SubOrder ";
            return SqlHelper.ExecuteDataSet(sql);
        }


	}
}
