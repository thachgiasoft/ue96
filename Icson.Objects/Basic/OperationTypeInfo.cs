using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class OperationTypeInfo
    {

		public OperationTypeInfo()
		{
			Init();
		}
		private int _sysno;
		private string _typename;
        private string _typedescription;
		private int _status;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_typename = AppConst.StringNull;
			_typedescription = AppConst.StringNull;
			_status = AppConst.IntNull;
		}

		public int SysNo
		{
			get
			{
				return _sysno;
			}
			set
			{
				_sysno = value;
			}
		}
		public string TypeName
		{
			get
			{
				return _typename;
			}
			set
			{
				_typename = value;
			}
		}
		public string TypeDescription
		{
			get
			{
				return _typedescription;
			}
			set
			{
				_typedescription = value;
			}
		}
		public int Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}

	}

}
