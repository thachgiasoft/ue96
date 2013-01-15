using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace Icson.Utils
{
    /// <summary>
    /// Summary description for AppConfig.
    /// </summary>
    public class AppConfig
    {

        private AppConfig()
        {
        }

        public static string RequestAccountPwd
        {
            get
            {
                return ConfigurationManager.AppSettings["RequestAccountPwd"];
            }
        }

        public static string IPPVersion
        {
            get
            {
                return ConfigurationManager.AppSettings["IPPVersion"];
            }
        }

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString"];
            }
        }
        public static string ConnectionString2
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString2"];
            }
        }
        public static string ErrorLogFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["ErrorLogFolder"];
            }
        }


        public static string SOAllocatedMap
        {
            get
            {
                return ConfigurationManager.AppSettings["SOAllocatedMap"];
            }
        }

        public static string PMCollection
        {
            get
            {
                return ConfigurationManager.AppSettings["PMCollection"];
            }
        }
        public static string APMCollection
        {
            get
            {
                return ConfigurationManager.AppSettings["APMCollection"];
            }
        }
        public static string FMCollection
        {
            get
            {
                return ConfigurationManager.AppSettings["FMCollection"];
            }
        }
        public static Decimal FMMonthCredit
        {
            get
            {
                return Decimal.Parse(ConfigurationManager.AppSettings["FMMonthCredit"]);
            }
        }
        public static string MailCharset
        {
            get
            {
                return ConfigurationManager.AppSettings["MailCharset"];
            }
        }

        public static string MailFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["MailFrom"];
            }
        }

        public static string MailFromName
        {
            get
            {
                return ConfigurationManager.AppSettings["MailFromName"];
            }
        }

        public static string MailServer
        {
            get
            {
                return ConfigurationManager.AppSettings["MailServer"];
            }
        }

        public static string MailUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["MailUserName"];
            }
        }

        public static string MailUserPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["MailUserPassword"];
            }
        }
        public static string MailLord
        {
            get
            {
                return ConfigurationManager.AppSettings["MailLord"];
            }
        }

        public static bool IsDaemon
        {
            get
            {
                string dd = ConfigurationManager.AppSettings["IsDaemon"];
                if (dd == null || dd.ToLower() != "true")
                    return false;
                else
                    return true;
            }
        }

        public static bool IsIAS
        {
            get
            {
                string dd = ConfigurationManager.AppSettings["IsIAS"];
                if (dd == null || dd.ToLower() != "true")
                    return false;
                else
                    return true;
            }
        }

        public static bool IsAutoUpdateSecondHandProductPrice
        {
            get
            {
                string dd = ConfigurationManager.AppSettings["IsAutoUpdateSecondHandProductPrice"];
                if (dd == null || dd.ToLower() != "true")
                    return false;
                else
                    return true;
            }
        }

        public static string AdminEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminEmail"];
            }
        }

        public static bool IsSendEMail
        {
            get
            {
                string dd = ConfigurationManager.AppSettings["IsSendEMail"];
                if (dd == null || dd.ToLower() != "true")
                    return false;
                else
                    return true;
            }
        }

        public static bool IsSendEmailToPPM
        {
            get
            {
                string dd = ConfigurationManager.AppSettings["IsSendEmailToPPM"];
                if (dd == null || dd.ToLower() != "true")
                    return false;
                else
                    return true;
            }
        }

        public static string SOMailTemplet
        {
            get
            {
                return ConfigurationManager.AppSettings["OrderMailTemplet"];
            }
        }

        public static string PicturePath
        {
            get
            {
                return ConfigurationManager.AppSettings["PicturePath"];
            }
        }
        public static string OnlineRootPath
        {
            get
            {
                return ConfigurationManager.AppSettings["OnlineRootPath"];
            }
        }
        public static bool IsImportable
        {
            get
            {
                string dd = ConfigurationManager.AppSettings["IsImportable"];
                if (dd == null || dd.ToLower() != "true")
                    return false;
                else
                    return true;
            }
        }
        public static string ReviewBox
        {
            get
            {
                return ConfigurationManager.AppSettings["ReviewBox"];
            }
        }
        public static string PartnerSysNo
        {
            get
            {
                return ConfigurationManager.AppSettings["PartnerSysNo"];
            }
        }

        public static string PartnerLinkSource
        {
            get
            {
                return ConfigurationManager.AppSettings["PartnerLinkSource"];
            }
        }

        public static decimal VATEMSFee
        {
            get
            {
                return Convert.ToDecimal(ConfigurationManager.AppSettings["VATEMSFee"]);
            }
        }

        public static string ErrorBarcode
        {
            get
            {
                return ConfigurationManager.AppSettings["ErrorBarcode"];
            }
        }

        public static string PrintBarcodeFile
        {
            get
            {
                return ConfigurationManager.AppSettings["PrintBarcodeFile"];
            }
        }

        public static int AutoCreateSOTimer
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["AutoCreateSOTimer"]);
            }
        }
        public static int AutoCreateSOCount
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["AutoCreateSOCount"]);
            }
        }

        public static string RMAandCCMail
        {
            get
            {
                return "";
            }
        }

        public static string DayReportMail
        {
            get
            {
                return "";
            }
        }

        public static bool IsShowYagoleCategory
        {
            get
            {
                string dd = ConfigurationManager.AppSettings["IsShowYagoleCategory"];
                if (dd == null || dd.ToLower() != "true")
                    return false;
                else
                    return true;
            }
        }

        public static bool IsSendSMS
        {
            get
            {
                string dd = ConfigurationManager.AppSettings["IsSendSMS"];
                if (dd == null || dd.ToLower() != "true")
                    return false;
                else
                    return true;
            }
        }

        public static int DefaultStockSysNo
        {
            get
            {
                return Util.TrimIntNull(ConfigurationManager.AppSettings["DefaultStockSysNo"]);
            }
        }

        public static string CategoryNameOfUnsalableTwoWeek
        {
            get
            {
                return ConfigurationManager.AppSettings["CategoryNameOfUnsalableTwoWeek"];
            }
        }

        public static TopicSection Topic
        {
            get
            {
                return (TopicSection)GetSectionByName("TopicSection");
            }
        }

        public static object GetSectionByName(string sectionName)
        {
            CacheManager configCache = CacheFactory.GetCacheManager();
            ConfigurationSection site = configCache.GetData(sectionName) as ConfigurationSection;
            if (site == null)
            {
                ConfigurationFileMap fileMap = new ConfigurationFileMap();
                string configFileName = Util.GetAbsoluteFilePath(ConfigurationManager.AppSettings["MyConfigFile"]);
                fileMap.MachineConfigFilename = configFileName;
                System.Configuration.Configuration config = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
                site = config.GetSection(sectionName) as ConfigurationSection;

                configCache.Add(sectionName, site, CacheItemPriority.Normal, null, new FileDependency(configFileName));
            }
            return site;

        }

        public static string DomainName
        {
            get
            {
                return ConfigurationManager.AppSettings["DomainName"];
            }
        }

        public class TopicSection : ConfigurationSection
        {
            [ConfigurationProperty("UpImagePath")]
            public string UpImagePath
            {
                set
                {
                    this["UpImagePath"] = value;
                }
                get
                {
                    return (string)this["UpImagePath"];
                }
            }

            [ConfigurationProperty("ThumbnailPath")]
            public string ThumbnailPath
            {
                set
                {
                    this["ThumbnailPath"] = value;
                }
                get
                {
                    return (string)this["ThumbnailPath"];
                }
            }

            [ConfigurationProperty("UpImageRoot")]
            public string UpImageRoot
            {
                set
                {
                    this["UpImageRoot"] = value;
                }
                get
                {
                    return (string)this["UpImageRoot"];
                }
            }
            [ConfigurationProperty("ThumbnailRoot")]
            public string ThumbnailRoot
            {
                set
                {
                    this["ThumbnailRoot"] = value;
                }
                get
                {
                    return (string)this["ThumbnailRoot"];
                }
            }
            [ConfigurationProperty("ThumbnailLimitWidth")]
            public int ThumbnailLimitWidth
            {
                set
                {
                    this["ThumbnailLimitWidth"] = value;
                }
                get
                {
                    return (int)this["ThumbnailLimitWidth"];
                }
            }
            [ConfigurationProperty("ThumbnailLimitHeight")]
            public int ThumbnailLimitHeight
            {
                set
                {
                    this["ThumbnailLimitHeight"] = value;
                }
                get
                {
                    return (int)this["ThumbnailLimitHeight"];
                }
            }
            [ConfigurationProperty("PostReply_Everyday_CountLimit")]
            public int PostReply_Everyday_CountLimit
            {
                set
                {
                    this["PostReply_Everyday_CountLimit"] = value;
                }
                get
                {
                    return (int)this["PostReply_Everyday_CountLimit"];
                }
            }
            [ConfigurationProperty("PostTopic_Everyday_CountLimit")]
            public int PostTopic_Everyday_CountLimit
            {
                set
                {
                    this["PostTopic_Everyday_CountLimit"] = value;
                }
                get
                {
                    return (int)this["PostTopic_Everyday_CountLimit"];
                }
            }



        }
    }
}