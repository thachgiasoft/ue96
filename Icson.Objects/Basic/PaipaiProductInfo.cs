using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    public  class Request
    {
        public Request()
        {
            Init();
        }

        public string sign;
        public string itemdetaildesc;
        public PaiPaiProductHeadInfo head;
        public PaiPaiProductBodyInfo body;
        public void Init()
        {
            head = new PaiPaiProductHeadInfo();
            body = new PaiPaiProductBodyInfo();
            sign = AppConst.StringNull;
        }
    }

    public class PaiPaiProductHeadInfo
    {
        public PaiPaiProductHeadInfo()
        {
            Init();
        }
        public string cmdid;
        public string sversion;
        public string spid;
        public string time;
        public string userip;
        public string uin;
        public string token;
        public string seqno;

        public void Init()
        {
            cmdid = AppConst.StringNull;
            sversion = AppConst.StringNull;
            spid = AppConst.StringNull;
            uin = AppConst.StringNull;
            token = AppConst.StringNull;
            seqno = AppConst.StringNull;
        }
    } 

    public class PaiPaiProductBodyInfo
    {
         public string productid;             //是否必需：是
         public string itemname;            //是
         public string itemprice;             //是,商品的市场价格，以分为单位
         public string itemscoreid;            //否 积分 
         public string itemstockcount;       //是
         public string itembuylimitamount;    //否,商品的购买限制数量
         public string itemweight;             //是,商品的重量,以克为单位,不足以1克算

         public string itempromotionsdesc;   //否,商品的促销说明
         public string itemstandardconfigdesc; //否 商品标配说明，字符串，0-400个字节 
         public string itemproperty1;          //是 0位表示是否有无发票,1位表示订单是否要求买单提供信息,2位表示订单是要求买家提供的信息买家是否必要要求回答,3位表示商品是否有配件 
         public string itemproperty;          //是 0位，表示是否是推荐商品 
        
         //public string itemschedulelist;     //否, 商品的清单 
         public string itemshopactivityclassflag;   //否, 商品所属店铺的自定义活动分类，默认为未分类 
         public string itemaccessorydesc;            //否 商品配件说明 
         public string itemreleaseuin;            //是 商品发布者商家编号 
         public string itemstate;               //是 商品状态，0x00--待审核状态，0x01--审核通过，0x02--boss审核未通过下架，0x03--boss审核未通过删除，0x04--商家自己下架，0x05-- 商家自己删除 

         public string shopcustomclassid;       //是 店铺自定义分类编号 
         public string itemdealaskinfo;          //是 商品下单要求买家提供的信息，字符串，1-200个字节 
         public string itempromotionstype;        //是 促销类型:1打折,2送礼,3免邮费  

         //public string itemsellactivityclassflag;   //是 商品所属店铺的自定义活动分类 
         //public string itempaytype;           //否         //public string itempaymoneytype;      //否         //public string itemshiptype;          //否
         public string shippingid;            //是, 商品的运费模版ID 
         //public StocksList stockslist;       //否


         public string itemdetaildescfile; //是 商品说明文件，base64位编码描述，1-50000个字节 
         public RegionInfo regioninfo;      //是,地区信息
         public AccessoryInfo accessorylist;    //附件信息
         public DeliveryPlacesInfo deliveryplaces;      //发货地区信息，可选
         public ItemSaleAttrListInfo itemsaleattrlist;  //商品属性信息;

         public string itemid;   //是 商品ID，32字节的长整型的字符串格式 
    }
    public class RegionInfo
    {
        public RegionInfo()
        {
            Init();
        }
        public uint regionid;   //是 地区信息全ID 
        public uint countryid;  //是 国家Id 
        public string country;  //是 国家 
        public uint provinceid; //条件可选 省Id，当包含有省信息时会展示 
        public string province; //条件可选 省，当包含有省信息时会展示 
        public uint cityid;     //条件可选 市Id，当包含有市信息时会展示 
        public string city;     //条件可选 市，当包含有市信息时会展示 
        public uint sectionid;  //条件可选 区Id，当包含有区信息时会展示 
        public string section;  //条件可选 区，当包含有区信息时会展示 

        public void Init()
        {
            country = AppConst.StringNull;
            province = AppConst.StringNull;
            city = AppConst.StringNull;
            section = AppConst.StringNull;
        }
    }

    public class AccessoryInfo
    {
        public AccessoryInfo()
        {
            Init();
        }
        public Accessory accessory;

        public void Init()
        {
            accessory = new Accessory();
        }
    }
    public class Accessory
    {
        public string accessoryname;     //是 配件名称,字符串,1-20字节 
        public string accessorydetaildesc;     //否 商品配件详细描述,base64位编码描述，1-255个字节 
        public uint accessoryprice;      //是 商品配件价格，大于0.01小于1000000的数字，以分为单位 
        public uint accessorysinglebuytotal;   //是 商品单个用户购买限制  
        public uint accessoryamount;     //是 配件的数量
    }

    public class DeliveryPlacesInfo
    {
        public DeliveryPlacesInfo()
        {
            Init();
        }
        public CityList citylist;
        public void Init()
        {
            citylist = new CityList();
        }
    }

    public class CityList
    {
        public CityList()
        {
            Init();
        }
        public City city;
        public void Init()
        {
            city = new City();
        }
    }

    public class City
    {
        public uint cityid;     //是 城市ID 
        public string cityname; //条件可选 城市名 
    }

    public class ItemSaleAttrListInfo
    {
        public Attribute attrlist;
    }
    public class AttrList
    {
        public Attribute attribute;
    }
    public class Attribute
    {
        public uint attrclassid;    //是 属性ID 
        public string attrclassname; //条件可选 属性ID对应的名字 
        public uint attroptiontype; //条件可选 属性选项值类型,增加/修改时该字段被忽略 
        public uint attroptionid;   //是 属性值id 
        public string attroptionvalue;  //条件可选 属性值，字符串，1-1024字节 
    }
}