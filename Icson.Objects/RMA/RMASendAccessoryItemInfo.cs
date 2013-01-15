using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.RMA
{
   public class RMASendAccessoryItemInfo
    {
       public RMASendAccessoryItemInfo()
       {
           Init();
       }
       public int SysNo;
       public int SendAccessorySysNo;
       public int SOSysNo;
       public int ProductSysNo;
       public int ProductIDSysNo;
       public int NewProductSysNo;
       public int NewProductQty;
       public int NewProductIDSysNo;
       public int AccessoryType;
       public string AccessoryName;
       public string SOItemPODesc;
       public string Note;

       public void Init()
       {
           SysNo = AppConst.IntNull;
           SendAccessorySysNo = AppConst.IntNull;
           SOSysNo = AppConst.IntNull;
           ProductSysNo = AppConst.IntNull;
           ProductIDSysNo = AppConst.IntNull;
           NewProductSysNo = AppConst.IntNull;
           NewProductQty = AppConst.IntNull;
           NewProductIDSysNo = AppConst.IntNull;
           AccessoryType = AppConst.IntNull;
           AccessoryName = AppConst.StringNull;
           SOItemPODesc = AppConst.StringNull;
           Note = AppConst.StringNull;
       }
    }
}
