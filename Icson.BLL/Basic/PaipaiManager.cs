using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

namespace Icson.BLL.Basic
{
  public  class PaipaiManager
    {
      private static PaipaiManager _instance;
      public static PaipaiManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PaipaiManager();
            }
            return _instance;
        }

      private void map(PaipaiResponseInfo oParam, DataRow tempdr)
      {
          oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
          oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
          oParam.PaipaiItemID = Util.TrimNull(tempdr["PaipaiItemID"]);
          oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
      }
      public void Insert(PaipaiResponseInfo oPaipaiResponseInfo )
      {
          new PaipaiResposeDac().Insert(oPaipaiResponseInfo);
      }

      public string CreateXml(Request oPaipai)
      {
          //XmlSerializer se = new XmlSerializer(typeof(Request));
          //Stream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
          //se.Serialize(stream, paipai);
          //stream.Close();

          string strReqXML = "<?xml version=\"1.0\" encoding=\"GBK\"?><request>";
          strReqXML += "<head>"
                                  + "<cmdid>" + oPaipai.head.cmdid + "</cmdid>"
                                  + "<sversion>" + oPaipai.head.sversion + "</sversion>"
                                  + "<spid>" + oPaipai.head.spid + "</spid>"
                                  + "<time>" + oPaipai.head.time + "</time>"
                                  + "<userip>" + oPaipai.head.userip + "</userip>"
                                  + "<uin>" + oPaipai.head.uin + "</uin>"
                                  + "<token>" + oPaipai.head.token + "</token>"
                                  + "<seqno>" + oPaipai.head.seqno + "</seqno>"
                                  + "</head>";

          strReqXML += "<body>"
                          + "<productid>" + oPaipai.body.productid + "</productid>"
                          + "<itemname>" + oPaipai.body.itemname + "</itemname>"
                          + "<itemprice>" + oPaipai.body.itemprice + "</itemprice>"
                          + "<itemstate>" + oPaipai.body.itemstate + "</itemstate>"
                          + "<itemstockcount>" + oPaipai.body.itemstockcount + "</itemstockcount>"
                          + "<itemweight>" + oPaipai.body.itemweight + "</itemweight>"
                          + "<itemdetaildescfile>" + oPaipai.body.itemdetaildescfile + "</itemdetaildescfile>"
                          + "</body>";
          strReqXML += "<sign>" + oPaipai.sign + "</sign>";
          strReqXML += "</request>";
          string postData = "call=" + strReqXML;
          return postData;
      }
      public string PostXmlToUrl(string url, string data,int productsysno)
      {
          HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
          hwr.Method = "POST";
          Stream stream = hwr.GetRequestStream();

          StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.GetEncoding("GBK"));
          sw.Write(data);
          sw.Close();

          stream = hwr.GetResponse().GetResponseStream();

          StreamReader sr = new StreamReader(stream, System.Text.Encoding.GetEncoding("GBK"));
          string ret = sr.ReadToEnd();
          sr.Close();
          return ResponseFromPaiPai(ret, productsysno);
      }
     
      public string ResponseFromPaiPai(string response,int productsysno)
      {
          XmlDocument xml = new XmlDocument();
          xml.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("GB2312").GetBytes(response)));
          XmlNodeList xmlnode = xml.SelectSingleNode("response").ChildNodes;
          string result="";
          string itemid="";
          int retcode = -1;
          string errinfo="";
          foreach (XmlNode xNo in xmlnode)
          {
              if (xNo.Name=="body"||xNo.Name=="head")
              {
                  foreach (XmlNode xNoChild in xNo.ChildNodes)
                  {
                      XmlElement xe = (XmlElement)xNoChild;
                      {
                          if (xe.Name == "itemid")
                          {
                              itemid = xe.InnerText;
                              PaipaiResponseInfo oPaipaiResponse = new PaipaiResponseInfo();
                              oPaipaiResponse.ProductSysNo = productsysno;
                              oPaipaiResponse.PaipaiItemID = itemid;
                              oPaipaiResponse.CreateTime = DateTime.Now;
                              PaipaiManager.GetInstance().Insert(oPaipaiResponse);
                          }
                          else if (xe.Name == "retcode")
                          {
                              retcode = Int32.Parse(xe.InnerText);
                          }

                          else if (xe.Name == "errinfo")
                          {
                              errinfo = xe.InnerText;
                          }
                      }
                  }
              }
          }
          if (retcode == 0)
              result = "拍拍网上架/更新成功！";
          else
              result = "拍拍网更新出错" + errinfo;
          return result;
      }

      public string GetPaipaiItemID(int ProductSysNo)
      {
          string sql = @"select PaipaiItemID from Paipai_Response where ProductSysNo=" + ProductSysNo;
          DataSet ds = SqlHelper.ExecuteDataSet(sql);
          if (!Util.HasMoreRow(ds))
              return AppConst.StringNull;
          else
              return  ds.Tables[0].Rows[0][0].ToString();
      }
    }
}
