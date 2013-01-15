using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    class DepartmentInfo
    {
        public class Department
        {
            public Department()
            {
                Init();

            }
            public int SysNo;
            public int DepartmentId;
            public string DepartmentName;
            public int Status;

            public void Init()
            {
                SysNo = AppConst.IntNull;
                DepartmentId = AppConst.IntNull;
                DepartmentName = AppConst.StringNull;
                Status = AppConst.IntNull;
            }
        }
    }
}
