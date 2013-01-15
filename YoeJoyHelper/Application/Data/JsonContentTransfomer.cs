using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;

namespace YoeJoyHelper
{
    /// <summary>
    /// Json的序列化
    /// 需要将Object序列化成Json对象的类
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class JsonContentTransfomer<R>
    {
        /// <summary>
        /// 获取Json格式化的数据
        /// </summary>
        /// <param name="rawObj"></param>
        /// <returns></returns>
        public static string GetJsonContent(R rawObj)
        {
            if (rawObj == null)
            {
                return String.Empty;
            }
            else
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                StringBuilder output = new StringBuilder();
                try
                {
                    jsonSerializer.Serialize(rawObj, output);
                    return output.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}
