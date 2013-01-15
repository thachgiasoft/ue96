using System;

using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Icson.Objects
{
    /// <summary>
    /// Summary description for AppEnum.
    /// </summary>
    public class AppEnum
    {
        public AppEnum()
        {
        }

        #region 工具函数

        private static SortedList GetStatus(System.Type t)
        {
            SortedList list = new SortedList();

            Array a = Enum.GetValues(t);
            for (int i = 0; i < a.Length; i++)
            {
                string enumName = a.GetValue(i).ToString();
                int enumKey = (int)System.Enum.Parse(t, enumName);
                string enumDescription = GetDescription(t, enumKey);
                list.Add(enumKey, enumDescription);
            }
            return list;
        }

        private static string GetName(System.Type t, object v)
        {
            try
            {
                return Enum.GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }


        /// <summary>
        /// 返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="t">枚举类型</param>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        private static string GetDescription(System.Type t, object v)
        {
            try
            {
                FieldInfo fi = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }
        #endregion

        //二值状态
        //请登记使用者 user, role, category1/2/3, categoryattribute, manufacturer，
        #region BiStatus 0, -1
        //===========================================
        public enum BiStatus : int
        {
            [Description("Valid")]
            Valid = 0,
            [Description("InValid")]
            InValid = -1

        }


        public static SortedList GetBiStatus()
        {
            return GetStatus(typeof(BiStatus));
        }
        public static string GetBiStatus(object v)
        {
            return GetDescription(typeof(BiStatus), v);
        }

        //===========================================
        //DBC修改成中文下拉框
        public enum BiStatus_DBC : int
        {
            [Description("有效")]
            Valid = 0,
            [Description("无效")]
            InValid = -1
        }

        public static SortedList GetBiStatus_DBC()
        {
            return GetStatus(typeof(BiStatus_DBC));
        }
        public static string GetBiStatus_DBC(object v)
        {
            return GetDescription(typeof(BiStatus_DBC), v);
        }
        //===========================

        //--------------------------------------------
        #endregion

        #region Yes/No 0, 1
        //===========================================
        public enum YNStatus : int
        {
            [Description("YES")]
            Yes = 1,
            [Description("NO")]
            No = 0

        }
        public static SortedList GetYNStatus()
        {
            return GetStatus(typeof(YNStatus));
        }
        public static string GetYNStatus(object v)
        {
            return GetDescription(typeof(YNStatus), v);
        }
        //--------------------------------------------
        #endregion

        #region Customer Gender 0, 1
        //===========================================
        public enum Gender : int
        {
            [Description("男")]
            Male = 1,
            [Description("女")]
            Female = 0
        }
        public static SortedList GetGender()
        {
            return GetStatus(typeof(Gender));
        }
        public static string GetGender(object v)
        {
            return GetDescription(typeof(Gender), v);
        }
        //--------------------------------------------
        #endregion

        #region Customer EmailStatus 0, 1
        //===========================================
        public enum EmailStatus : int
        {
            [Description("Origin")]
            Origin = 0,
            [Description("Confirmed")]
            Confirmed = 1

        }
        public static SortedList GetEmailStatus()
        {
            return GetStatus(typeof(EmailStatus));
        }
        public static string GetEmailStatus(object v)
        {
            return GetDescription(typeof(EmailStatus), v);
        }
        //--------------------------------------------
        #endregion

        //积分
        #region 积分原因种类
        //===========================================
        public enum PointLogType : int
        {
            [Description("Email确认")]
            Recruit = 1,	//新客户激活+
            [Description("老客户回馈")]
            Veteran = 2,	//老客户购物历史回复
            [Description("生成订单")]
            CreateOrder = 3,	//下订单-
            [Description("作废订单")]
            AbandonSO = 5,	//作废订单
            [Description("作废订单取消")]
            CancelAbandonSO = 6,	//审核作废取消-
            [Description("退货")]
            ReturnProduct = 7,	//退货-
            [Description("取消退货")]
            CancelReturn = 8,
            [Description("取消出库")]
            CancelOutstock = 9,		//取消出货
            [Description("积分转移")]
            TransferPoint = 10,   //积分转移
            [Description("购物得分")]
            AddPointLater = 11,		//滞后加分
            [Description("订单修改")]
            UpdateSO = 12,		//修改SaleOrder
            [Description("批发扣除")]
            WholeSale = 13,		//批发减分-
            [Description("买卡")]
            InfoProduct = 14,		//买卡减分-
            [Description("其他")]
            BizRequest = 15,		//Request
            [Description("低价举报奖励")]
            award = 16,      //低价举报奖励
            [Description("浩号送积分")]
            forHF = 17,       //浩号送积分 
            [Description("PCHome送积分")]
            forPCHome = 18,       //PCHome送积分 
            [Description("99bill送积分")]
            for99bill = 19,       //99bill送积分 
            [Description("5460送积分")]
            for5460 = 20,       //5460送积分 
            [Description("参加客服问卷调查")]
            CSSurvey = 21,
            [Description("邮资补偿")]
            PostageMakeup = 22,
            [Description("降价补偿")]
            PriceMakeup = 23,
            [Description("物流延期补偿")]
            DeliveryDelayMakeup = 24,
            [Description("调货延期补偿")]
            PurchaseDelayMakeup = 25,
            [Description("顾客投诉补偿")]
            CustomerComplainMakeup = 26,
            [Description("资料错误补偿")]
            ProductInfoErrorMakeup = 27,
            [Description("产品问题补偿")]
            ProductDefectMakeup = 28,
            [Description("顾客购买商品多付款")]
            CustomerPayMore = 29
        }
        public static SortedList GePointLogType()
        {
            return GetStatus(typeof(PointLogType));
        }
        public static string GetPointLogType(object v)
        {
            return GetDescription(typeof(PointLogType), v);
        }
        //--------------------------------------------
        #endregion

        //积分单据来源类型
        #region 积分单据来源类型
        //===========================================
        public enum PointSourceType : int
        {
            [Description("售后")]  //存SOSysNo
            RMA = 1,
            [Description("价格举报")]
            PriceReport = 2,
            [Description("客户反馈")]
            Feedback = 3
        }
        public static SortedList GetPointSourceType()
        {
            return GetStatus(typeof(PointSourceType));
        }
        public static string GetPointSourceType(object v)
        {
            return GetDescription(typeof(PointSourceType), v);
        }
        //--------------------------------------------
        #endregion

        //三值状态
        //请登记使用者
        #region TriStatus point, email, sms
        //===========================================
        public enum TriStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 0,
            [Description("Handled")]
            Handled = 1

        }
        public static SortedList GetTriStatus()
        {
            return GetStatus(typeof(TriStatus));
        }
        public static string GetTriStatus(object v)
        {
            return GetDescription(typeof(TriStatus), v);
        }
        //--------------------------------------------
        #endregion

        #region 地区类型
        public enum AreaType : int
        {
            [Description("Province")]
            Province = 0,
            [Description("City")]
            City = 1,
            [Description("District")]
            District = 2
        }
        public static SortedList GetAreaType()
        {
            return GetStatus(typeof(AreaType));
        }
        public static string GetAreaType(object v)
        {
            return GetDescription(typeof(AreaType), v);
        }
        #endregion

        //操作日志名称
        #region Log Type
        //===========================================
        public enum LogType : int
        {
            //xx_xx_xx
            //模块_子模块_操作
            //------------------------------------------------------------------------------------sys 10
            //		user 10, add 10, update 11.
            [Description("增加用户")]
            Sys_User_Add = 101010,
            [Description("更新用户")]
            Sys_User_Update = 101011,
            [Description("用户登录")]
            Sys_User_Login = 101012,
            //		role 11, add 10, update 11
            [Description("增加角色")]
            Sys_Role_Add = 101110,
            [Description("更新角色")]
            Sys_Role_Update = 101111,
            //		user&role 12, add 10, delete 12
            [Description("增加角色")]
            Sys_UserRole_Add = 101210,
            [Description("更新角色")]
            Sys_UserRole_Delete = 101212,
            //		role&privilege 13 add 10, delete 12
            [Description("增加角色")]
            Sys_RolePrivilege_Add = 101310,
            [Description("更新角色")]
            Sys_RolePrivilege_Delete = 101312,


            //------------------------------------------------------------------------------------basic 11
            //		category1 13, 
            //						add 10, update 11.
            [Description("增加大类")]
            Basic_Category1_Add = 111310,
            [Description("更新大类")]
            Basic_Category1_Update = 111311,
            //		category2 14, 
            //						add 10, update 11.
            [Description("增加中类")]
            Basic_Category2_Add = 111410,
            [Description("更新中类")]
            Basic_Category2_Update = 111411,
            //		category3 15, 
            //						add 10, update 11.
            [Description("增加小类")]
            Basic_Category3_Add = 111510,
            [Description("更新小类")]
            Basic_Category3_Update = 111511,
            [Description("增加小类评论项")]
            Basic_Category3ReviewItem_Add = 111512,
            [Description("更新小类评论项")]
            Basic_Category3ReviewItem_Update = 111513,

            //		categoryAttribute 16, 
            //						init 10, update 11, top 12, up 13, down 14, bottom 15, insert 16
            [Description("初始化小类属性")]
            Basic_CategoryAttribute_Init = 111610,
            [Description("修改小类属性")]
            Basic_CategoryAttribute_Update = 111611,
            [Description("移动到顶端")]
            Basic_CategoryAttribute_Top = 111612,
            [Description("移动到上一个")]
            Basic_CategoryAttribute_Up = 111613,
            [Description("移动到下一个")]
            Basic_CategoryAttribute_Down = 111614,
            [Description("移动到低端")]
            Basic_CategoryAttribute_Bottom = 111615,
            [Description("移动到低端")]
            Basic_CategoryAttribute_Insert = 111616,


            //		categoryAttributeOption 21, 
            //						init 21,insert 22 update 23, top 24, up 25, down 26, bottom 27
            [Description("初始化小类属性选项")]
            Basic_CategoryAttributeOption_Init = 111621,
            [Description("新增小类属性选项")]
            Basic_CategoryAttributeOption_Insert = 111622,
            [Description("修改小类属性选项")]
            Basic_CategoryAttributeOption_Update = 111623,
            [Description("属性选项移动到顶端")]
            Basic_CategoryAttributeOption_Top = 111624,
            [Description("属性选项移动到上一个")]
            Basic_CategoryAttributeOption_Up = 111625,
            [Description("属性选项移动到下一个")]
            Basic_CategoryAttributeOption_Down = 111626,
            [Description("属性选项移动到低端")]
            Basic_CategoryAttributeOption_Bottom = 111627,

            //		manufacturer  17, 
            //						add 10, update 11
            [Description("增加生产商")]
            Basic_Manufacturer_Add = 111710,
            [Description("更新生产商")]
            Basic_Manufacturer_Update = 111711,
            //		manufacturer  18, 
            //						add 10, update 11
            [Description("增加供应商")]
            Basic_Vendor_Add = 111810,
            [Description("更新供应商")]
            Basic_Vendor_Update = 111811,

            //		stock  19, add 10, update 11
            [Description("增加仓库")]
            Basic_Stock_Add = 111910,
            [Description("更新仓库")]
            Basic_Stock_Update = 111911,

            //		currency  20, add 10, update 11
            [Description("增加货币")]
            Basic_Currency_Add = 112010,
            [Description("更新货币")]
            Basic_Currency_Update = 112011,

            //		customer 30, add 10, update 11, invalid 12
            [Description("增加客户")]
            Basic_Customer_Add = 113001,
            [Description("修改客户")]
            Basic_Customer_Update = 113011,
            [Description("作废客户")]
            Basic_Customer_Invalid = 113012,



            //		ASP  60, 
            //			insert area 10, update area 11
            //			insert pay type 12, update pay type 13
            //			insert ship type 14, update ship type 15
            //			insert ship ! area 16, delete 17
            //			insert ship ! pay 18, delete 19
            //			insert ship area price 20, delete 21
            [Description("增加地区")]
            Basic_Area_Add = 116010,
            [Description("更新地区")]
            Basic_Area_Update = 116011,
            [Description("增加支付方式")]
            Basic_PayType_Add = 116012,
            [Description("更新支付方式")]
            Basic_PayType_Update = 116013,
            [Description("增加送货方式")]
            Basic_ShipType_Add = 116014,
            [Description("更新送货方式")]
            Basic_ShipType_Update = 116015,
            [Description("添加送货方式和地区的否关系")]
            Basic_ShipAreaUn_Add = 116016,
            [Description("删除送货方式和地区的否关系")]
            Basic_ShipAreaUn_Delete = 116017,
            [Description("添加送货方式和支付方式的否关系")]
            Basic_ShipPayUn_Add = 116018,
            [Description("删除送货方式和支付方式的否关系")]
            Basic_ShipPayUn_Delete = 116019,
            [Description("添加送货方式和地区的价格")]
            Basic_ShipAreaPrice_Add = 116020,
            [Description("删除送货方式和地区的价格")]
            Basic_ShipAreaPrice_Delete = 116021,

            //		Product 30
            [Description("新增商品")]
            Basic_Product_Add = 113001,
            [Description("更新商品基本信息")]
            Basic_Product_Basic_Update = 113002,
            [Description("更新商品价格")]
            Basic_Product_Price_Update = 113003,
            [Description("更新商品PM")]
            Basic_Product_PM_Update = 113004,
            [Description("更新商品属性")]
            Basic_Product_Attribute_Update = 113005,
            [Description("更新商品重量")]
            Basic_Product_Weight_Update = 113006,
            [Description("更新商品图片")]
            Basic_Product_Pic_Update = 113007,
            [Description("添加商品市场低价信息")]
            Basic_Product_PriceMarket_Add = 113008,
            [Description("更新商品预览")]
            Basic_Product_Preview_Update = 113009,

            //------------------------------------------------------------------------------------stock sheet 50
            //		lend 11, 
            //						
            [Description("借货单主项增加")]
            St_Lend_Master_Insert = 501110,
            [Description("借货单主项修改")]
            St_Lend_Master_Update = 501111,
            [Description("借货单明细插入")]
            St_Lend_Item_Insert = 501112,
            [Description("借货单明细删除")]
            St_Lend_Item_Delete = 501113,
            [Description("借货单明细修改")]
            St_Lend_Item_Update = 501114,

            [Description("借货单废弃")]
            St_Lend_Abandon = 501115,
            [Description("借货单取消废弃")]
            St_Lend_CancelAbandon = 501116,

            [Description("借货单审核")]
            St_Lend_Verify = 501117,
            [Description("借货单取消审核")]
            St_Lend_CancelVerify = 501118,

            [Description("借货单出库")]
            St_Lend_OutStock = 501119,
            [Description("借货单取消出库")]
            St_Lend_CancelOutStock = 501120,

            [Description("借货单还货插入")]
            St_Lend_Return_Insert = 501121,
            [Description("借货单还货删除")]
            St_Lend_Return_Delete = 501122,

            //		adjust 12, 
            //						
            [Description("损益单主项增加")]
            St_Adjust_Master_Insert = 501210,
            [Description("损益单主项修改")]
            St_Adjust_Master_Update = 501211,
            [Description("损益单明细插入")]
            St_Adjust_Item_Insert = 501212,
            [Description("损益单明细删除")]
            St_Adjust_Item_Delete = 501213,
            [Description("损益单明细修改")]
            St_Adjust_Item_Update = 501214,

            [Description("损益单废弃")]
            St_Adjust_Abandon = 501215,
            [Description("损益单取消废弃")]
            St_Adjust_CancelAbandon = 501216,

            [Description("损益单审核")]
            St_Adjust_Verify = 501217,
            [Description("损益单取消审核")]
            St_Adjust_CancelVerify = 501218,

            [Description("损益单出库")]
            St_Adjust_OutStock = 501219,
            [Description("损益单取消出库")]
            St_Adjust_CancelOutStock = 501220,

            //		shift 13, 
            //						
            [Description("移库单主项增加")]
            St_Shift_Master_Insert = 501310,
            [Description("移库单主项修改")]
            St_Shift_Master_Update = 501311,
            [Description("移库单明细插入")]
            St_Shift_Item_Insert = 501312,
            [Description("移库单明细删除")]
            St_Shift_Item_Delete = 501313,
            [Description("移库单明细修改")]
            St_Shift_Item_Update = 501314,

            [Description("移库单废弃")]
            St_Shift_Abandon = 501315,
            [Description("移库单取消废弃")]
            St_Shift_CancelAbandon = 501316,

            [Description("移库单审核")]
            St_Shift_Verify = 501317,
            [Description("移库单取消审核")]
            St_Shift_CancelVerify = 501318,

            [Description("移库单出库")]
            St_Shift_OutStock = 501319,
            [Description("移库单取消出库")]
            St_Shift_CancelOutStock = 501320,

            [Description("移库单入库")]
            St_Shift_InStock = 501321,
            [Description("移库单取消入库")]
            St_Shift_CancelInStock = 501322,

            //		transfer 14, 
            //						
            [Description("转换单主项增加")]
            St_Transfer_Master_Insert = 501410,
            [Description("转换单主项修改")]
            St_Transfer_Master_Update = 501411,
            [Description("转换单明细插入")]
            St_Transfer_Item_Insert = 501412,
            [Description("转换单明细删除")]
            St_Transfer_Item_Delete = 50143,
            [Description("转换单明细修改")]
            St_Transfer_Item_Update = 501414,

            [Description("转换单废弃")]
            St_Transfer_Abandon = 501415,
            [Description("转换单取消废弃")]
            St_Transfer_CancelAbandon = 501416,

            [Description("转换单审核")]
            St_Transfer_Verify = 501417,
            [Description("转换单取消审核")]
            St_Transfer_CancelVerify = 501418,

            [Description("转换单出库")]
            St_Transfer_OutStock = 501419,
            [Description("转换单取消出库")]
            St_Transfer_CancelOutStock = 501420,

            //		virtual 15, 
            //						
            [Description("虚库操作")]
            St_Virtual_Insert = 501510,

            //		Inventory position
            //
            [Description("设置库位")]
            St_Inventory_SetPos = 501610,

            //Purchase 20-----------------------------------------------------------
            //	Basket  10

            [Description("采购篮插入")]
            Purchase_Basket_Insert = 201010,
            [Description("采购篮更新")]
            Purchase_Basket_Update = 201011,
            [Description("采购篮删除")]
            Purchase_Basket_Delete = 201012,

            //     PO	11
            [Description("生成采购单")]
            Purchase_Create = 201110,
            [Description("采购单主项修改")]
            Purchase_Master_Update = 201111,
            [Description("采购单明细添加")]
            Purchase_Item_Insert = 201112,
            [Description("采购单明细修改")]
            Purchase_Item_Update = 201113,
            [Description("采购单明细删除")]
            Purchase_Item_Delete = 201114,
            [Description("采购单审核到摊销")]
            Purchase_Verify_Apportion = 201114,
            [Description("采购单审核到入库")]
            Purchase_Verify_InStock = 201116,
            [Description("采购单取消审核")]
            Purchase_CancelVerify = 201117,
            [Description("采购单入库")]
            Purchase_InStock = 201118,
            [Description("采购单取消入库")]
            Purchase_CancelInStock = 201119,
            [Description("采购单作废")]
            Purchase_Abandon = 201120,
            [Description("采购单取消作废")]
            Purchase_CancelAbandon = 201121,

            [Description("采购单摊销主项添加")]
            Purchase_ApportionMaster_Add = 201122,
            [Description("采购单摊销主项删除")]
            Purchase_ApportionMaster_Delete = 201123,
            [Description("采购单摊销明细添加")]
            Purchase_ApportionItem_Add = 201124,
            [Description("采购单摊销明细删除")]
            Purchase_ApportionItem_Delete = 201125,
            [Description("采购单摊销导出")]
            Purchase_Apportion_Export = 201126,
            [Description("PM采购金额限制添加")]
            Purchase_PMPOAmtRestrict_Add = 201127,
            [Description("PM采购金额限制修改")]
            Purchase_PMPOAmtRestrict_Update = 201128,


            //Finance 30
            //				po 11
            [Description("财务采购付款单添加")]
            Finance_POPay_Item_Add = 301110,
            [Description("财务采购付款单修改")]
            Finance_POPay_Item_Update = 301111,
            [Description("财务采购付款单作废")]
            Finance_POPay_Item_Abandon = 301112,
            [Description("财务采购付款单取消作废")]
            Finance_POPay_Item_CancelAbandon = 301113,
            [Description("财务采购付款单支付")]
            Finance_POPay_Item_Pay = 301114,
            [Description("财务采购付款单取消支付")]
            Finance_POPay_Item_CancelPay = 301115,

            [Description("财务采购应收更新")]
            Finance_POPay_Update = 301116,

            [Description("财务采购付款单审核")]
            Finance_POPay_Item_Audit = 301117,
            [Description("财务采购付款单取消审核")]
            Finance_POPay_Item_CancelAudit = 301118,
            [Description("财务采购付款单申请")]
            Finance_POPay_Item_Request = 301119,


            //				so income 12
            [Description("财务销售收款单添加")]
            Finance_SOIncome_Add = 301201,
            [Description("财务销售收款单作废")]
            Finance_SOIncome_Abandon = 301202,
            [Description("财务销售收款单确认")]
            Finance_SOIncome_Confirm = 301203,
            [Description("财务销售收款单取消确认")]
            Finance_SOIncome_UnConfirm = 301204,
            [Description("财务凭证录入")]
            Finance_SOIncome_Voucher_Add = 301205,

            //				netpay 13
            [Description("财务NetPay Add&Verify")]
            Finance_NetPay_AddVerified = 301310,
            [Description("财务NetPay Verify")]
            Finance_NetPay_Verify = 301311,
            [Description("财务NetPay Abandon")]
            Finance_NetPay_Abandon = 301312,


            //Sale 60
            //              so 06
            [Description("销售单生成")]
            Sale_SO_Create = 600601,
            [Description("销售单审核")]
            Sale_SO_Audit = 600602,
            [Description("销售单取消审核")]
            Sale_SO_CancelAudit = 600603,
            [Description("销售单经理审核")]
            Sale_SO_ManagerAudit = 600604,
            [Description("销售单客户作废")]
            Sale_SO_CustomerAbandon = 600605,
            [Description("销售单员工作废")]
            Sale_SO_EmployeeAbandon = 600606,
            [Description("销售单经理作废")]
            Sale_SO_ManagerAbandon = 600607,
            [Description("销售单取消作废")]
            Sale_SO_CancelAbandon = 600608,
            [Description("销售单出库")]
            Sale_SO_OutStock = 600609,
            [Description("销售单取消出库")]
            Sale_SO_CancelOutStock = 600610,
            [Description("销售单发票打印")]
            Sale_SO_PrintInvoice = 600611,
            [Description("销售单修改")]
            Sale_SO_Update = 600612,
            [Description("销售单开具发票")]
            Sale_SO_Invoice = 600613,
            [Description("销售单作废发票")]
            Sale_SO_AbandonInvoice = 600614,
            [Description("销售单更改发票类型")]
            Sale_SO_UpdateInvoiceType = 600615,

            //				rma 08
            [Description("RMA单生成")]
            Sale_RMA_Create = 600801,
            [Description("RMA单作废")]
            Sale_RMA_Abandon = 600802,
            [Description("RMA单审核")]
            Sale_RMA_Audit = 600803,
            [Description("RMA单取消审核")]
            Sale_RMA_CancelAudit = 600804,
            [Description("RMA单接收商品")]
            Sale_RMA_Receive = 600805,
            [Description("RMA单取消接收")]
            Sale_RMA_CancelReceive = 600806,
            [Description("RMA单处理")]
            Sale_RMA_Handle = 600807,
            [Description("RMA单取消处理")]
            Sale_RMA_CancelHandle = 600808,
            [Description("RMA单结案")]
            Sale_RMA_Close = 600809,
            [Description("RMA单重开")]
            Sale_RMA_Reopen = 600810,

            //				ro 09
            [Description("退货单生成")]
            Sale_RO_Create = 600901,
            [Description("退货单作废")]
            Sale_RO_Abandon = 600902,
            [Description("退货单审核")]
            Sale_RO_Audit = 600903,
            [Description("退货单取消审核")]
            Sale_RO_CancelAudit = 600904,
            [Description("退货单退货")]
            Sale_RO_Return = 600905,
            [Description("退货单取消退货")]
            Sale_RO_CancelReturn = 600906,
            [Description("退货单发票打印")]
            Sale_RO_PrintInvoice = 600907,

            //RMA new Version
            //70
            //rma requeset 10
            [Description("RMA单生成")]
            Sale_RMA_Create2 = 700801,
            [Description("RMA单作废")]
            Sale_RMA_Abandon2 = 700802,
            [Description("RMA单审核")]
            Sale_RMA_Audit2 = 700803,

            //				rma outbound 20
            [Description("RMA-送修-生成")]
            RMA_OutBound_Create = 702001,
            [Description("RMA-送修-修改")]
            RMA_OutBound_Update = 702002,
            [Description("RMA-送修-出库")]
            RMA_OutBound_OutStock = 702003,
            [Description("RMA-送修-取消出库")]
            RMA_OutBound_CancelOutStock = 702004,
            [Description("RMA-送修-删除明细")]
            RMA_OutBound_DeleteItem = 702005,
            [Description("RMA-送修-删除明细")]
            RMA_OutBound_InsertItem = 702006,
            [Description("RMA-送修-删除作废")]
            RMA_OutBound_Abandon = 702007,
            [Description("RMA-送修-删除作废")]
            RMA_OutBound_UpdateDunDesc = 702008,

            //				rma register 30
            [Description("RMA-登记-更新Check")]
            RMA_Register_Check = 703001,
            [Description("RMA-登记-更新Memo")]
            RMA_Register_Memo = 703002,
            [Description("RMA-登记-更新Outbound")]
            RMA_Register_Outbound = 703003,
            [Description("RMA-登记-更新Revert")]
            RMA_Register_Revert = 703004,
            [Description("RMA-登记-更新Revert")]
            RMA_Register_Refund = 703005,
            [Description("RMA-登记-更新Return")]
            RMA_Register_Return = 703006,
            [Description("RMA-登记-更新Close")]
            RMA_Register_Close = 703007,
            [Description("RMA-登记-SetToCC")]
            RMA_Register_ToCC = 703008,
            [Description("RMA-登记-SetToRMA")]
            RMA_Register_ToRMA = 703009,
            [Description("RMA_登记_审核Revert")]
            RMA_Register_RevertAudit = 703010,

            //           rma revert 40
            [Description("RMA-送货-生成")]
            RMA_Revert_Create = 704001,
            [Description("RMA-送货-修改")]
            RMA_Revert_Update = 704002,
            [Description("RMA-送货-作废")]
            RMA_Revert_Abandon = 704003,
            [Description("RMA-送货-取消作废")]
            RMA_Revert_CancelAbandon = 704006,
            [Description("RMA-送货-出库")]
            RMA_Revert_Out = 704004,
            [Description("RMA-送货-取消出库")]
            RMA_Revert_CancelOut = 704005,


            //           rma refund 50 
            [Description("RMA-退货-生成")]
            RMA_Refund_Create = 705001,
            [Description("RMA-退货-修改")]
            RMA_Refund_Upate = 705002,
            [Description("RMA-退货-作废")]
            RMA_Refund_Abandon = 705003,
            [Description("RMA-退货-审核")]
            RMA_Refund_Audit = 705004,
            [Description("RMA-退货-取消审核")]
            RMA_Refund_CancelAudit = 70505,
            [Description("RMA-退货-退款")]
            RMA_Refund_Refund = 705006,
            [Description("RMA-退货-取消退款")]
            RMA_Refund_CancelRefund = 705007,

            //			rma return 60
            [Description("RMA-退货入库货-生成")]
            RMA_Return_Create = 706001,
            [Description("RMA-退货入库-修改")]
            RMA_Return_Update = 706002,
            [Description("RMA-退货入库-作废")]
            RMA_Return_Abandon = 706003,
            [Description("RMA-退货入库-入库")]
            RMA_Return_Return = 706004,
            [Description("RMA-退货入库-取消入库")]
            RMA_Return_CancelReturn = 706005,
            [Description("RMA-退货入库-审核")]
            RMA_Return_Audit = 706006,

            //			rma_request 70
            [Description("RMA-申请单-生成")]
            RMA_Request_Create = 707001,
            [Description("RMA-申请单-修改")]
            RMA_Request_Update = 707002,
            [Description("RMA-申请单-收货")]
            RMA_Request_Receive = 707003,
            [Description("RMA-申请单-取消收货")]
            RMA_Request_CancelReceive = 707004,
            [Description("RMA-申请单-作废")]
            RMA_Request_Abandon = 707005,
            [Description("RMA-申请单-关闭")]
            RMA_Request_Close = 707006,
            [Description("RMA-申请单-重复生成")]
            RMA_Request_ReCreate = 707007,

            //			rma_sendAccessory 80
            [Description("RMA-补发附件单-生成")]
            RMA_SendAccessory_Create = 708001,
            [Description("RMA-补发附件单-修改主信息")]
            RMA_SendAccessory_UpdateMaster = 708002,
            [Description("RMA-补发附件单-修改商品信息")]
            RMA_SendAccessory_UpdateItem = 708003,
            [Description("RMA-补发附件单-作废")]
            RMA_SendAccessory_Abandon = 708004,
            [Description("RMA-补发附件单-取消作废")]
            RMA_SendAccessory_CancelAbandon = 708005,
            [Description("RMA-补发附件单-审核")]
            RMA_SendAccessoryt_Audit = 708006,
            [Description("RMA-补发附件单-取消审核")]
            RMA_SendAccessoryt_CancelAudit = 708007,
            [Description("RMA-补发附件单-发货")]
            RMA_SendAccessoryt_Send = 708008,
            [Description("RMA-补发附件单-取消发货")]
            RMA_SendAccessoryt_CancelSend = 708009,


            // 30
            [Description("RMA_登记_退款信息")]
            RMA_Register_IsRecommendRefund = 703011,

            //				rma handover 100
            [Description("RMA-交接-生成")]
            RMA_Handover_Create = 710001,
            [Description("RMA-交接-修改")]
            RMA_Handover_Update = 710002,
            [Description("RMA-交接-出库")]
            RMA_Handover_OutStock = 710003,
            [Description("RMA-交接-取消出库")]
            RMA_Handover_CancelOutStock = 710004,
            [Description("RMA-交接-删除明细")]
            RMA_Handover_DeleteItem = 710005,
            [Description("RMA-交接-增加明细")]
            RMA_Handover_InsertItem = 710006,
            [Description("RMA-交接-作废")]
            RMA_Handover_Abandon = 710007,
            [Description("RMA-交接-取消作废")]
            RMA_Handover_CancelAbandon = 710008,
            [Description("RMA-交接-接收")]
            RMA_Handover_Receive = 710009,
            [Description("RMA-交接-取消接收")]
            RMA_Handover_CancelReceive = 710010,

            //			sale_Return 90
            [Description("销售-退货单-生成")]
            sale_Return_Create = 709001,
            [Description("销售-退货单-审核")]
            sale_Return_Audit = 709002,
            [Description("销售-退货单-取消审核")]
            sale_Return_CancelAudit = 709003,
            [Description("销售-退货单-收货")]
            sale_Return_Receive = 709004,
            [Description("销售-退货单-取消收货")]
            sale_Return_CancelReceive = 709005,
            [Description("销售-退货单-上架")]
            sale_Return_Shelve = 709006,
            [Description("销售-退货单-取消上架")]
            sale_Return_CancelShelve = 709007,
            [Description("销售-退货单-入库")]
            sale_Return_Instock = 709008,
            [Description("销售-退货单-取消入库")]
            sale_Return_CancelInstock = 709009,
            [Description("销售-退货单-作废")]
            sale_Return_Abandon = 709010,
            [Description("销售-退货单-取消作废")]
            sale_Return_CancelAbandon = 709011,

            //	Delivery 80:dl,ds
            [Description("配送-配送单-信用额度放行")]
            delivery_dl_CreditAllow = 801001,
            [Description("配送-配送单-作废")]
            delivery_dl_Abandon = 801002,

            //Online 40
            //
            [Description("更新公告")]
            Online_Bulletin_Update = 401001,
            [Description("插入产品列表")]
            Online_List_Insert = 401002,
            [Description("删除产品列表")]
            Online_List_Delete = 401003,

            [Description("插入投票主项")]
            Online_Poll_Insert = 401004,
            [Description("更新投票主项")]
            Online_Poll_Update = 401005,
            [Description("插入投票明细")]
            Online_Poll_InsertItem = 401006,
            [Description("更新投票明细")]
            Online_Poll_UpdateItem = 401007,
            [Description("删除投票明细")]
            Online_Poll_DeleteItem = 401008,
            [Description("投票设定显示")]
            Online_Poll_Show = 401009,
            [Description("投票设定不显示")]
            Online_Poll_NotShow = 401010,

            [Description("客户反馈更新")]
            Online_FeedBack_Update = 401011,

            //Complain
            [Description("更新联系人信息")]
            Complain_Contact_Update = 101401,
            [Description("作废事故")]
            Complain_Abandon = 101402,
            [Description("取消作废事故")]
            Complain_CancelAbandon = 101403,
            [Description("设置事故下一步处理人员")]
            Complain_SetNextHandleUser = 101404,


            [Description("End no user")]
            ZZZZZ = 999999
        }
        public static SortedList GetLogType()
        {
            return GetStatus(typeof(LogType));
        }
        public static string GetLogType(object v)
        {
            return GetDescription(typeof(LogType), v);
        }
        //--------------------------------------------
        #endregion

        //供应商类型
        #region Vendor Type
        //===============================================
        public enum VendorType : int
        {

            [Description("Manufacturer")]
            Manufacturer = 0,
            [Description("Distributor")]
            Distributor = 1,
            [Description("Agent")]
            Agent = 2,
            [Description("Retailer")]
            Retailer = 3,
            [Description("Other")]
            Other = 4

        }
        public static SortedList GetVendorType()
        {
            return GetStatus(typeof(VendorType));
        }

        public static string GetVendorType(object v)
        {
            return GetDescription(typeof(VendorType), v);
        }
        //------------------------------------------------------
        #endregion

        //供应商合作类型
        #region Cooperate Type
        //===============================================
        public enum CooperateType : int
        {
            [Description("购销")]
            PurchaseSale = 0,
            [Description("联营")]
            CooperateSale = 1
        }
        public static SortedList GetCooperateType()
        {
            return GetStatus(typeof(CooperateType));
        }

        public static string GetCooperateType(object v)
        {
            return GetDescription(typeof(CooperateType), v);
        }
        //------------------------------------------------------
        #endregion

        //配置区域
        #region OnlineListArea
        //===============================================
        public enum OnlineListArea : int
        {
            //数字有间隔，为了以后扩展, 已有数字不能更改
            //Modified by Kyle for Icson -- Start
            [Description("首    页.特价Top2(*)")]
            DefaultTop2 = 10,
            [Description("首    页.上(*)")]
            DefaultUp = 20,
            [Description("首    页.下(*)")]
            DefaultDown = 30,
            [Description("电脑配件.展示(*)")]
            Hardware = 40,
            [Description("电脑配件.Top10")]
            HardwareTopSale = 50,
            [Description("数码产品.展示(*)")]
            Digital = 60,
            [Description("数码产品.Top10")]
            DigitalTopSale = 70,
            [Description("附件耗材.展示(*)")]
            Accessory = 80,
            [Description("附件耗材.Top10")]
            AccessoryTopSale = 90,
            [Description("待定")]
            Audio = 100,
            [Description("待定")]
            Newcome = 120,
            [Description("商品明细.特色商品")]
            FeturedProduct = 130,
            [Description("待定")]
            AOpenTop1 = 140,
            [Description("待定")]
            AOpenNew = 150,
            [Description("游戏外设.展示(*)")]
            GameAcc = 160,
            [Description("游戏外设.Top10(*)")]
            GameAccTopSale = 170,
            [Description("移动存储.展示(*)")]
            MovSto = 180,
            [Description("移动存储.Top10(*)")]
            MovStoTopSale = 190,
            [Description("发烧视听.展示(*)")]
            AudioFans = 200,
            [Description("发烧视听.Top10(*)")]
            AudioFansTopSale = 210,
            [Description("网络通讯.展示(*)")]
            NetCom = 220,
            [Description("网络通讯.Top10(*)")]
            NetComTopSale = 230,
            [Description("办公用品.展示(*)")]
            Office = 240,
            [Description("办公用品.Top10(*)")]
            OfficeTopSale = 250
            //Modified by Kyle for Icson -- End
        }
        public static SortedList GetOnlineListArea()
        {
            return GetStatus(typeof(OnlineListArea));
        }

        public static string GetOnlineListArea(object v)
        {
            return GetDescription(typeof(OnlineListArea), v);
        }
        //------------------------------------------------------
        #endregion

        //商品类型
        #region Product Type
        //===============================================
        public enum ProductType : int
        {
            [Description("Normal")]
            Normal = 0,
            [Description("SecondHand")]
            SecondHand = 1,
            [Description("Bad")]
            Bad = 2,
            [Description("Service")]
            Service = 3,
            [Description("OtherProduct")]
            OtherProduct = 4
        }
        public static SortedList GetProductType()
        {
            return GetStatus(typeof(ProductType));
        }

        public static string GetProductType(object v)
        {
            return GetDescription(typeof(ProductType), v);
        }
        //------------------------------------------------------
        #endregion

        //主子商品类型
        #region Product2ndType
        //===============================================
        public enum Product2ndType : int
        {
            [Description("普通商品")]
            Normal = 0,
            [Description("主商品")]
            Master = 1,
            [Description("子商品")]
            Child = 2
        }
        public static SortedList GetProduct2ndType()
        {
            return GetStatus(typeof(Product2ndType));
        }

        public static string GetProduct2ndType(object v)
        {
            return GetDescription(typeof(Product2ndType), v);
        }
        //------------------------------------------------------
        #endregion

        //商品积分兑换类型
        #region Product Pay Type
        public enum ProductPayType : int
        {
            [Description("MoneyPayOnly")]
            MoneyPayOnly = 0,
            [Description("PointPayOnly")]
            PointPayOnly = 2,
            [Description("BothSupported")]
            BothSupported = 1
        }
        public static SortedList GetProductPayType()
        {
            return GetStatus(typeof(ProductPayType));
        }
        public static string GetProductPayType(object v)
        {
            return GetDescription(typeof(ProductType), v);
        }
        #endregion

        //借货单状态
        #region Lend Status
        //===============================================
        public enum LendStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 1,
            [Description("Verified")]
            Verified = 2,
            [Description("OutStock")]
            OutStock = 3,
            [Description("ReturnPartly")]
            ReturnPartly = 4,
            [Description("ReturnAll")]
            ReturnAll = 5

        }
        public static SortedList GetLendStatus()
        {
            return GetStatus(typeof(LendStatus));
        }

        public static string GetLendStatus(object v)
        {
            return GetDescription(typeof(LendStatus), v);
        }
        //------------------------------------------------------
        #endregion

        //损益单状态
        #region Adjust Status
        //===============================================
        public enum AdjustStatus : int
        {

            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 1,
            [Description("Verified")]
            Verified = 2,
            [Description("OutStock")]
            OutStock = 3
        }
        public static SortedList GetAdjustStatus()
        {
            return GetStatus(typeof(AdjustStatus));
        }

        public static string GetAdjustStatus(object v)
        {
            return GetDescription(typeof(AdjustStatus), v);
        }
        //------------------------------------------------------
        #endregion

        //移库单状态
        #region Shift Status
        //===============================================
        public enum ShiftStatus : int
        {

            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 1,
            [Description("Verified")]
            Verified = 2,
            [Description("OutStock")]
            OutStock = 3,
            [Description("InStock")]
            InStock = 4,
            [Description("RAM待移库")]
            RMAWaitingShift = 5
        }
        public static SortedList GetShiftStatus()
        {
            return GetStatus(typeof(ShiftStatus));
        }

        public static string GetShiftStatus(object v)
        {
            return GetDescription(typeof(ShiftStatus), v);
        }
        //------------------------------------------------------
        #endregion

        //转换单状态
        #region Transfer Status
        //===============================================
        public enum TransferStatus : int
        {

            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 1,
            [Description("Verified")]
            Verified = 2,
            [Description("OutStock")]
            OutStock = 3
        }
        public static SortedList GetTransferStatus()
        {
            return GetStatus(typeof(TransferStatus));
        }

        public static string GetTransferStatus(object v)
        {
            return GetDescription(typeof(TransferStatus), v);
        }
        //------------------------------------------------------
        #endregion

        //转换单明细类型状态
        #region TransferItemType Status
        //===============================================
        public enum TransferItemType : int
        {

            [Description("Source")]
            Source = 1,
            [Description("Target")]
            Target = 2
        }
        public static SortedList GetTransferItemType()
        {
            return GetStatus(typeof(TransferItemType));
        }

        public static string GetTransferItemType(object v)
        {
            return GetDescription(typeof(TransferItemType), v);
        }
        //------------------------------------------------------
        #endregion

        //商品基本状态
        #region Product Status
        public enum ProductStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("Valid")]
            Valid = 0,
            [Description("Show")]
            Show = 1
        }
        public static SortedList GetProductStatus()
        {
            return GetStatus(typeof(ProductStatus));
        }
        public static string GetProductStatus(object v)
        {
            return GetDescription(typeof(ProductStatus), v);
        }
        #endregion

        //商品StatusInfo
        #region Product Status Info
        public enum ProductStatusInfo : int
        {
            [Description("Valid")]
            Valid = 0,
            [Description("Show")]
            Show = 1
        }
        public static SortedList GetProductStatusInfo()
        {
            return GetStatus(typeof(ProductStatusInfo));
        }
        public static string GetProductStatusInfo(object v)
        {
            return GetDescription(typeof(ProductStatusInfo), v);
        }
        #endregion

        #region SOItem Type
        public enum SOItemType : int
        {
            [Description("For Sale")]
            ForSale = 0,
            [Description("Gift")]
            Gift = 1,
            [Description("Promotion")]
            Promotion = 2
        }
        public static SortedList GetSOItemType()
        {
            return GetStatus(typeof(SOItemType));
        }
        public static string GetSOItemType(object v)
        {
            return GetDescription(typeof(SOItemType), v);
        }
        #endregion

        #region 采购单状态
        public enum POStatus : int
        {
            [Description("作废")]
            Abandon = 0,
            [Description("初始")]
            Origin = 1,
            [Description("待摊销")]
            WaitingApportion = 2,
            [Description("待入库")]
            WaitingInStock = 3,
            [Description("已入库")]
            InStock = 4,
            [Description("待收货")]
            WaitingReceive = 5,
            [Description("待上架")]
            WaitingShelve = 6
        }
        public static SortedList GetPOStatus()
        {
            return GetStatus(typeof(POStatus));
        }
        public static string GetPOStatus(object v)
        {
            return GetDescription(typeof(POStatus), v);
        }
        #endregion

        #region 采购单类型
        public enum POType : int
        {
            [Description("普通")]
            Normal = 0,
            [Description("紧急")]
            Urgent = 1
        }
        public static SortedList GetPOType()
        {
            return GetStatus(typeof(POType));
        }
        public static string GetPOType(object v)
        {
            return GetDescription(typeof(POType), v);
        }

        #endregion

        #region 采购单配送类型
        public enum POShipType : int
        {
            [Description("送仓库")]
            Stock = 0,
            [Description("送门店")]
            Store = 1
        }
        public static SortedList GetPOShipType()
        {
            return GetStatus(typeof(POShipType));
        }
        public static string GetPOShipType(object v)
        {
            return GetDescription(typeof(POShipType), v);
        }
        #endregion

        #region 采购单带票类型
        public enum POInvoiceType : int
        {
            [Description("======")]
            NoInvoice = 1,
            [Description("带增票")]
            ValueAddedInvoice = 2
        }
        public static SortedList GetPOInvoiceType()
        {
            return GetStatus(typeof(POInvoiceType));
        }
        public static string GetPOInvoiceType(object v)
        {
            return GetDescription(typeof(POInvoiceType), v);
        }

        #endregion

        #region 采购单经理审核状态
        public enum POManagerAuditStatus : int
        {
            [Description("初始")]
            Origin = 0,
            [Description("审核0分")]
            ZeroPoint = -1,
            [Description("审核1分")]
            OnePoint = 1,
            [Description("审核2分")]
            TwoPoint = 2,
            [Description("审核3分")]
            ThreePoint = 3,
            [Description("审核4分")]
            FourPoint = 4,
            [Description("审核5分")]
            FivePoint = 5
        }
        public static SortedList GetPOManagerAuditStatus()
        {
            return GetStatus(typeof(POManagerAuditStatus));
        }
        public static string GetPOManagerAuditStatus(object v)
        {
            return GetDescription(typeof(POManagerAuditStatus), v);
        }
        #endregion

        #region 采购摊销方式类型
        public enum POApportionType : int
        {
            [Description("ByMoney")]
            ByMoney = 0,			//按金额
            [Description("ByQuantity")]
            ByQuantity = 1,		//按数量
            [Description("ByWeight")]
            ByWeight = 2			//按重量
        }
        public static SortedList GetPOApportionType()
        {
            return GetStatus(typeof(POApportionType));
        }
        public static string GetPOApportionType(object v)
        {
            return GetDescription(typeof(POApportionType), v);
        }
        #endregion

        #region 财务付款单 POPayItemStyle
        public enum POPayItemStyle : int
        {
            [Description("Normal")]
            Normal = 0,
            [Description("Advanced")]
            Advanced = 1,
        }
        public static SortedList GetPOPayItemStyle()
        {
            return GetStatus(typeof(POPayItemStyle));
        }
        public static string GetPOPayItemStyle(object v)
        {
            return GetDescription(typeof(POPayItemStyle), v);
        }
        #endregion

        #region 财务付款单 POPayItemStatus
        public enum POPayItemStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 0,
            [Description("Paid")]
            Paid = 1,
            [Description("WaitingAudit")]
            WaitingAudit = 2,
            [Description("WaitingPay")]
            WaitingPay = 3
        }
        public static SortedList GetPOPayItemStatus()
        {
            return GetStatus(typeof(POPayItemStatus));
        }
        public static string GetPOPayItemStatus(object v)
        {
            return GetDescription(typeof(POPayItemStatus), v);
        }
        #endregion

        #region 财务采购应付  POPayStatus
        public enum POPayStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("UnPay")]
            UnPay = 0,
            [Description("PartlyPay")]
            PartlyPay = 1,
            [Description("FullPay")]
            FullPay = 2
        }
        public static SortedList GetPOPayStatus()
        {
            return GetStatus(typeof(POPayStatus));
        }
        public static string GetPOPayStatus(object v)
        {
            return GetDescription(typeof(POPayStatus), v);
        }
        #endregion

        #region 财务采购应付 POPayInvoiceStatus
        public enum POPayInvoiceStatus : int
        {
            [Description("Absent")]
            Absent = 0,
            [Description("Incomplete")]
            Incomplete = 1,
            [Description("Complete")]
            Complete = 2
        }
        public static SortedList GetPOPayInvoiceStatus()
        {
            return GetStatus(typeof(POPayInvoiceStatus));
        }
        public static string GetPOPayInvoiceStatus(object v)
        {
            return GetDescription(typeof(POPayInvoiceStatus), v);
        }
        #endregion

        #region 财务销售收款单 收款类型
        public enum SOIncomeStyle : int
        {
            [Description("Normal")]
            Normal = 0,
            [Description("Advanced")]
            Advanced = 1,
            [Description("RO")]
            RO = 2
        }
        public static SortedList GetSOIncomeStyle()
        {
            return GetStatus(typeof(SOIncomeStyle));
        }
        public static string GetSOIncomeStyle(object v)
        {
            return GetDescription(typeof(SOIncomeStyle), v);
        }
        #endregion

        #region 财务销售收款单 状态
        public enum SOIncomeStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 0,
            [Description("Confirmed")]
            Confirmed = 1
        }
        public static SortedList GetSOIncomeStatus()
        {
            return GetStatus(typeof(SOIncomeStatus));
        }
        public static string GetSOIncomeStatus(object v)
        {
            return GetDescription(typeof(SOIncomeStatus), v);
        }
        #endregion

        #region 财务销售收款单 单据类型
        public enum SOIncomeOrderType : int
        {
            [Description("SO")]
            SO = 1,
            [Description("RO")]
            RO = 2
        }
        public static SortedList GetSOIncomeOrderType()
        {
            return GetStatus(typeof(SOIncomeOrderType));
        }
        public static string GetSOIncomeOrderType(object v)
        {
            return GetDescription(typeof(SOIncomeOrderType), v);
        }
        #endregion

        #region 财务销售 网上支付状态
        public enum NetPayStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 0,
            [Description("Verified")]
            Verified = 1
        }
        public static SortedList GetNetPayStatus()
        {
            return GetStatus(typeof(NetPayStatus));
        }
        public static string GetNetPayStatus(object v)
        {
            return GetDescription(typeof(NetPayStatus), v);
        }
        #endregion

        #region 财务销售 网上支付来源
        public enum NetPaySource : int
        {
            [Description("Bank")]
            Bank = 0,
            [Description("Employee")]
            Employee = 1
        }
        public static SortedList GetNetPaySource()
        {
            return GetStatus(typeof(NetPaySource));
        }
        public static string GetNetPaySource(object v)
        {
            return GetDescription(typeof(NetPaySource), v);
        }
        #endregion

        #region RMA类型 RMAType
        public enum RMAType : int
        {
            [Description("不确定")]
            Unsure = 0,
            [Description("申请返修")]
            Maintain = 1,
            [Description("申请退货")]
            Return = 2,
            [Description("拒绝申请")]
            Overrule = 3
        }
        public static SortedList GetRMAType()
        {
            return GetStatus(typeof(RMAType));
        }
        public static string GetRMAType(object v)
        {
            return GetDescription(typeof(RMAType), v);
        }
        #endregion

        #region 销售单状态
        public enum SOStatus : int
        {
            [Description("部分退货")]
            PartlyReturn = -5,
            [Description("全部退货")]
            Return = -4,
            [Description("主管作废")]
            ManagerCancel = -3,
            [Description("客户作废")]
            CustomerCancel = -2,
            [Description("员工作废")]
            EmployeeCancel = -1,
            [Description("待审核")]
            Origin = 0,
            [Description("待出库")]
            WaitingOutStock = 1,
            [Description("待支付")]
            WaitingPay = 2,
            [Description("待主管审")]
            WaitingManagerAudit = 3,
            [Description("已出库")]
            OutStock = 4
        }
        public static SortedList GetSOStatus()
        {
            return GetStatus(typeof(SOStatus));
        }
        public static string GetSOStatus(object v)
        {
            return GetDescription(typeof(SOStatus), v);
        }
        #endregion

        #region 销售单Email类型
        public enum SOEmailType : int
        {
            [Description("销售单作废")]
            AbandonSO = -1,
            [Description("销售单生成")]
            CreateSO = 0,
            [Description("销售单审核")]
            AuditSO = 1,
            [Description("销售单出库")]
            OutStock = 2,
            [Description("销售单加分")]
            AddDelayPoint = 3
        }
        public static SortedList GetSOEmailType()
        {
            return GetStatus(typeof(SOEmailType));
        }
        public static string GetSOEmailType(object v)
        {
            return GetDescription(typeof(SOEmailType), v);
        }
        #endregion

        #region 退货单状态
        public enum ROStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 0,
            [Description("Audited")]
            Audited = 1,
            [Description("Returned")]
            Returned = 2
        }
        public static SortedList GetROStatus()
        {
            return GetStatus(typeof(ROStatus));
        }
        public static string GetROStatus(object v)
        {
            return GetDescription(typeof(ROStatus), v);
        }
        #endregion

        //权限表. 
        #region Privilege
        //===============================================
        public enum Privilege : int
        {
            [Description("权限维护")]
            PrivilegeOpt = 101,
            [Description("类别维护")]
            CategoryOpt = 102,
            [Description("生产商维护")]
            ManufacturerOpt = 103,
            [Description("供应商维护")]
            VendorOpt = 104,
            [Description("仓库维护")]
            StockOpt = 105,
            [Description("货币维护")]
            CurrencyOpt = 106,
            [Description("地区配送支付")]
            AreaShipPay = 107,
            [Description("商品基本信息")]
            ProductBasic = 108,
            [Description("商品价格信息")]
            ProductPrice = 109,
            [Description("商品PM信息")]
            ProductPM = 110,  //保修上架等
            [Description("商品重量信息")]
            ProductWeight = 111,
            [Description("商品图片信息")]
            ProductPic = 112,
            [Description("商品市场低价信息")]
            ProductPriceMarket = 113,
            [Description("商品预览信息")]
            ProductPreview = 114,
            [Description("供应商主管维护")]
            VendorManagerOpt = 115,
            [Description("竞争对手价格导出")]
            CompetitorPriceExport = 116,
            [Description("竞争对手价格导入系统")]
            CompetitorPriceImport = 117,
            [Description("尺码维护")]
            SizeUpdate = 129,
            [Description("PM采购金额限制维护")]
            PMPOAmtRestrictOpt = 125,

            [Description("优惠券创建")]
            CouponCreate = 135,
            [Description("优惠券审核")]
            CouponAudit = 136,
            [Description("优惠券申请批量添加")]
            CouponRequestBatch = 137,
            [Description("优惠券申请添加")]
            CouponRequest = 138,
            [Description("优惠券申请批量审核")]
            CouponRequestAudit = 139,
            [Description("优惠券申请批量取消审核")]
            CouponRequestCancelAudit = 140,
            [Description("优惠券申请批量作废")]
            CouponRequestAbandon = 141,


            [Description("借货单制单")]
            StLendFillIn = 501,
            [Description("借货单作废")]
            StLendAbandon = 502,
            [Description("借货单取消作废")]
            StLendCancelAbandon = 503,
            [Description("借货单审核")]
            StLendAudit = 504,
            [Description("借货单取消审核")]
            StLendCancelAudit = 505,
            [Description("借货单出库")]
            StLendOutStock = 506,
            [Description("借货单取消出库")]
            StLendCancelOutStock = 507,
            [Description("借货单还货操作")]
            StLendReturn = 508,

            [Description("损益单制单")]
            StAdjustFillIn = 509,
            [Description("损益单作废")]
            StAdjustAbandon = 510,
            [Description("损益单取消作废")]
            StAdjustCancelAbandon = 512,
            [Description("损益单审核")]
            StAdjustAudit = 513,
            [Description("损益单取消审核")]
            StAdjustCancelAudit = 514,
            [Description("损益单出库")]
            StAdjustOutStock = 515,
            [Description("损益单取消出库")]
            StAdjustCancelOutStock = 516,

            [Description("移库单制单")]
            StShiftFillIn = 517,
            [Description("移库单作废")]
            StShiftAbandon = 518,
            [Description("移库单取消作废")]
            StShiftCancelAbandon = 519,
            [Description("移库单审核")]
            StShiftAudit = 520,
            [Description("移库单取消审核")]
            StShiftCancelAudit = 521,
            [Description("移库单出库")]
            StShiftOutStock = 522,
            [Description("移库单取消出库")]
            StShiftCancelOutStock = 523,
            [Description("移库单入库")]
            StShiftInStock = 524,
            [Description("移库单取消入库")]
            StTranferCancelInStock = 525,

            [Description("转换单制单")]
            StTransferFillIn = 526,
            [Description("转换单作废")]
            StTransferAbandon = 527,
            [Description("转换单取消作废")]
            StTransferCancelAbandon = 528,
            [Description("转换单审核")]
            StTransferAudit = 529,
            [Description("转换单取消审核")]
            StTransferCancelAudit = 530,
            [Description("转换单出库")]
            StTransferOutStock = 531,
            [Description("转换单取消出库")]
            StTransferCancelOutStock = 532,

            [Description("虚库操作")]
            StVirtualOpt = 533,
            [Description("虚库Manager操作")]
            StVirtualManagerOpt = 534,
            [Description("采购单强制审核")]
            POForceAudit = 200,
            [Description("采购单制单")]
            POFillIn = 201, //包括采购篮/摊销/采购单作废和取消作废
            [Description("采购单审核")]
            POAudit = 202,	//包括取消审核
            [Description("采购单入库")]
            POInStock = 203,
            [Description("采购单取消入库")]
            POCancelInStock = 204,
            [Description("采购单收货")]
            POReceive = 205,
            [Description("采购单取消收货")]
            POCancelReceive = 206,
            [Description("采购单经理审核")]
            POManagerAudit = 207,
            [Description("采购单上架")]
            POShelving = 208,
            [Description("采购单取消上架")]
            POCancelShelving = 209,

            [Description("财务采购付款单制单")]
            POPayFillIn = 301,
            [Description("财务采购付款单作废")]
            POPayAbandon = 302,
            [Description("财务采购付款单取消作废")]
            POPayCancelAbandon = 303,
            [Description("财务采购付款单支付")]
            POPayPay = 304,
            [Description("财务采购付款单取消支付")]
            POPayCancelPay = 305,
            [Description("客户积分管理")]
            AddPoint = 306,
            [Description("积分申请")]
            PointRequest = 307,
            [Description("积分审核")]
            PointAudit = 308,
            [Description("积分添加")]
            PointAdd = 309,
            [Description("财务采购付款单PM申请")]
            POPayRequestByPM = 310,
            [Description("财务采购付款单主管申请")]
            POPayRequestByDirector = 311,
            [Description("财务采购付款单审核")]
            POPayAudit = 312,
            [Description("财务采购付款单取消审核")]
            POPayCancelAudit = 313,
            [Description("财务采购付款单重复申请")]
            POPayRequestMoreTime = 314,
            [Description("财务采购付款单TL提交到PMD申请")]
            POPayRequestToPMD = 315,
            [Description("结算单财务确认")]
            DSAccConfirm = 316,
            [Description("结算单财务取消确认")]
            DSAccCancelConfirm = 317,
            [Description("更新纸凭证")]
            DSUpdatePaperVoucher = 318,

            [Description("前台公告维护")]
            OnlineBulletin = 400,
            [Description("前台商品展示维护")]
            OnlineList = 401,
            [Description("前台投票维护")]
            OnlinePoll = 402,
            [Description("客户反馈修改")]
            FeedBack = 403,

            //评论权限
            [Description("评论管理")]
            ReviewManage = 405,
            TopicManage = 406,
            [Description("评论回复审核")]
            ReviewReplyAudit = 407,

            //价格举报
            [Description("价格举报审核")]
            PriceReportAudit = 408,

            //SO 
            [Description("销售单制单")]
            SOCreate = 150,
            [Description("销售单员工作废")]
            SOEmployeeAbandon = 151,
            [Description("销售单审核")]
            SOAudit = 152,
            [Description("销售单取消审核")]
            SOCancelAudit = 153,
            [Description("销售单经理审核")]
            SOManagerAudit = 154,
            [Description("销售单经理取消审核")]
            SOManagerCancelAudit = 155,
            [Description("销售单经理作废")]
            SOManagerAbandon = 156,
            [Description("销售单出库")]
            SOOutStock = 157,
            [Description("销售单取消出库")]
            SOCancelOutStock = 158,
            [Description("销售单修改")]
            SOUpdate = 159,
            [Description("销售单取消作废")]
            SOCancelAbandon = 160,
            [Description("销售单发票打印")]
            SOPrintInvoice = 176,
            [Description("销售单取消出库校验")]
            SOCancelOutStockCheck = 177,
            [Description("销售单称重数据上传")]
            SOWeightImportData = 178,
            [Description("销售单出库校验")]
            SOOutStockCheck = 179,
            [Description("商品SN导入")]
            ProductSNImport = 180,
            [Description("订单审核比例维护")]
            SOUserAuditRatioUpdate = 181,
            [Description("配送延缓新增")]
            DeliveryDelayAdd = 182,
            [Description("配送延缓删除")]
            DeliveryDelayDelete = 183,
            [Description("配送延缓更新")]
            DeliveryDelayUpdate = 184,
            [Description("仓库工作比例维护")]
            WHUserWorkRatioUpdate = 185,
            [Description("快递师傅信用维护")]
            DeliveryManDepositOpt = 191,
            [Description("配送单信用额度放行")]
            DLAllow = 193,
            [Description("更新订单发票")]
            SOUpdateInvoiceStatus = 186,
            [Description("作废订单发票")]
            SOAbandonInvoice = 187,
            [Description("申请发票")]
            SORequestInvoice = 188,
            [Description("配送单发货确认")]
            DLOutStockConfirm = 189,
            [Description("配送单生成结算单")]
            DSCreate = 190,
            [Description("结算单审核")]
            DSAudit = 194,
            [Description("结算单作废")]
            DSAbandon = 195,
            [Description("设置订单大件")]
            SOSetSizeType = 196,
            [Description("维护订单刷卡记录")]
            DSPosUpdate = 197,

            //SR退货单
            [Description("退货单新建")]
            SRNew = 210,
            [Description("退货单审核")]
            SRAudit = 211,
            [Description("退货单取消审核")]
            SRCancelAudit = 212,
            [Description("退货单收货")]
            SRReceive = 213,
            [Description("退货单取消收货")]
            SRCancelReceive = 214,
            [Description("退货单上架")]
            SRShelve = 215,
            [Description("退货单取消上架")]
            SRCancelShelve = 216,
            [Description("退货单入库")]
            SRInStock = 217,
            [Description("退货单取消入库")]
            SRCancelInStock = 218,
            [Description("退货单作废")]
            SRAbandon = 219,
            [Description("退货单取消作废")]
            SRCancelAbandon = 220,
            [Description("退货单更新")]
            SRUpdate = 221,

            //RMA
            [Description("RMA单制单")]
            RMACreate = 161,
            [Description("RMA单作废")]
            RMAAbandon = 162,
            [Description("RMA单审核")]
            RMAAudit = 163,
            [Description("RMA单取消审核")]
            RMACancelAudit = 164,
            [Description("RMA单商品确认")]
            RMAReceive = 165,
            [Description("RMA单取消商品确认")]
            RMACancelReceive = 166,
            [Description("RMA单处理")]
            RMAHandle = 167,
            [Description("RMA单取消处理")]
            RMACancelHandle = 168,
            [Description("RMA单结案")]
            RMAClose = 169,
            [Description("RMA单重开")]
            RMAReopen = 170,
            [Description("RMA单修改")]
            RMAUpdate = 171,
            //RO
            [Description("退货单审核")]
            ROAudit = 172,
            [Description("退货单取消审核")]
            ROCancelAudit = 173,
            [Description("退货单退货")]
            ROReturn = 174,
            [Description("退货单取消退货")]
            ROCancelReturn = 175,

            //RMA New Version权限
            //Register
            [Description("更新机身编号")]
            ProductNoUpdate = 601,
            [Description("更新产品检修信息")]
            RMACheckUpdate = 602,
            [Description("确认送修返还")]
            RMAResponse = 603,
            [Description("更新RMA备注信息")]
            RMAMemoUpdate = 604,
            [Description("设置待送修")]
            WaitingOutBound = 605,
            [Description("设置待发货")]
            WaitingRevert = 606,    //这里补充了对于发非当前RMA Case待发货的审核权限，编号636
            [Description("设置待退款")]
            WaitingRefund = 607,
            [Description("设置待退入库")]
            WaitingReturn = 608,
            [Description("结束RMA单")]
            RMARegisterClose = 609,
            [Description("ReOpenRMA单")]
            RMARegisterReOpen = 610,
            [Description("将RMA单提交CC处理")]
            RMASetToCC = 611,
            [Description("将RMA单提交RMA部门处理")]
            RMASetToRMA = 612,
            [Description("设置是否7天内保修")]
            RMASet7Days = 635,
            [Description("PM更新催讨信息")]
            PMUpdateDunDesc = 649,

            //修改Vendor售后信息
            [Description("RMA修改Vendor售后信息")]
            RMAUpdateAfterSale = 639,
            //Request
            [Description("增加申请单")]
            RequestAdd = 613,                   //这里补充了一个增加重复申请单，编号638
            [Description("修改申请单")]
            RequestUpdate = 614,
            [Description("申请单收货确认")]
            RequestReceive = 615,
            [Description("作废申请单")]
            RequestAbandon = 616,

            //OutBound
            [Description("生成送修单")]
            OutBoundCreate = 617,
            [Description("修改送修单")]
            OutBoundUpdate = 618,
            [Description("送修出库")]
            OutBoundOutStore = 619,
            [Description("作废送修单")]
            OutBoundAbandon = 620,
            [Description("送修单出库校验")]
            OutBoundOutStockCheck = 650,

            //Revert
            [Description("生成发货单")]
            RevertAdd = 621,
            [Description("修改发货单")]
            RevertUpdate = 622,
            [Description("发货出库")]
            RevertOutStore = 623,
            [Description("发货出库-调新品")]
            RevertOutStore_New = 624,
            [Description("作废发货单")]
            RevertAbandon = 625,
            [Description("发货单出库校验")]
            RevertOutStockCheck = 642,

            //Return
            [Description("生成退货入库单")]
            ReturnAdd = 626,
            [Description("修改退货入库单")]
            ReturnUpdate = 627,
            [Description("退货入库")]
            ReturnInStore = 628,
            [Description("作废退货入库单")]
            ReturnAbandon = 629,    //这里补充了退货入库审核的权限，在637。
            [Description("取消退货入库")]
            ReturnCancelInStore = 640,

            //Refund
            [Description("生成退款单")]
            RefundAdd = 630,
            [Description("修改退款单")]
            RefundUpdate = 631,
            [Description("审批退款单")]
            RefundAudit = 632,
            [Description("退款")]
            RefundRefund = 633,
            [Description("作废退款单")]
            RefundAbandon = 634,
            [Description("主管审批退款")]
            RefundDirectorAudit = 650,

            [Description("RMA发货审核")]
            RMARevertAudit = 636,     //RMA单件处理中心页面上的审核。

            [Description("退货入库审核")]
            RMAReturnAudit = 637,     //RMA入库的SelectTarget页面上的审核。

            [Description("重复增加申请单")]
            RequestAddRepeate = 638,         //它对应日志类型是 RMA_Request_ReCreate

            [Description("修改送修单中预约送修日期/预约返还日期")]
            RMAOutBoundDate = 641,

            //SendAccessory
            [Description("新增补发附件单")]
            SendAccessoryAdd = 643,
            [Description("修改补发附件单")]
            SendAccessoryUpdate = 644,
            [Description("补发附件审核")]
            SendAccessoryAudit = 645,
            [Description("补发附件发货")]
            SendAccessorySend = 646,
            [Description("补发附件取消发货")]
            SendAccessoryCancelSend = 648,
            [Description("作废补发附件单")]
            SendAccessoryAbandon = 647,

            //Solution
            //[Description("Sln维护")]
            //SlnOpt = 601,
            //[Description("SlnItem维护")]
            //SlnItemOpt = 602,
            //[Description("Prj维护")]
            //PrjOpt = 611,
            //[Description("PrjItem维护")]
            //PrjItemOpt = 612,

            [Description("补发附件单出库校验")]
            SendAccessoryOutStockCheck = 651,
            [Description("设置待移库")]
            SetWaitingShift = 652,
            [Description("取消待移库")]
            CancelWaitingShift = 653,
            [Description("设置待交接")]
            SetWaitingHandover = 665,
            [Description("取消待交接")]
            CancelWaitingHandover = 667,

            [Description("更新返修检测信息")]
            CheckRepairUpdate = 662,
            [Description("设置超7天待退款")]
            BeyondWaitingRefund = 664,


            [Description("退货单设置待移库")]
            ReturnSetWaitingShift = 680,
            [Description("退货单取消待移库")]
            ReturnCancelWaitingShift = 681,

            //RMA New Version权限
            //Register
            [Description("更新退款信息及建议")]
            RMAIsRecommendRefundUpdate = 665,

            //Handover
            [Description("交接单生成")]
            HandoverCreate = 667,
            [Description("交接单修改")]
            HandoverUpdate = 668,
            [Description("交接单出库")]
            HandoverOutStock = 669,
            [Description("交接单取消出库")]
            HandoverCancelOutStock = 670,
            [Description("交接单作废")]
            HandoverAbandon = 671,
            [Description("交接单接收")]
            HandoverReceive = 672,
            [Description("交接单取消接收")]
            HandoverCancelReceive = 673,

            //收款单
            [Description("销售收款单确认")]
            SOIncomeConfirm = 701,
            [Description("销售收款单作废")]
            SOIncomeAbandon = 702,
            [Description("销售收款单取消确认")]
            SOIncomeUnConfirm = 703,
            [Description("销售收款单隔天取消确认")]
            SOIncomeExpireUnConfirm = 704,
            [Description("销售收款单凭证输入")]
            SOIncomeVoucher = 705,
            [Description("销售收款单取消确认别人")]
            SOIncomeUnComfirmOthers = 706,
            [Description("网上支付新增审核")]
            SONetPayAddAudit = 707,

            [Description("查看客户密码")]
            Customer_WatchPwd = 801,
            [Description("会员设置")]
            Customer_Set = 802,
            [Description("处理自己的事故")]
            ComplainOwn = 803,
            [Description("处理全部事故")]
            ComplainAll = 804,
            [Description("更新事故回访结果")]
            UpdateReviewBack = 805,
            [Description("作废事故")]
            ComplainAbandon = 806,
            [Description("取消作废事故")]
            ComplainCancelAbandon = 807,
            [Description("事故结案")]
            ComplainClose = 808,
            [Description("事故审核")]
            ComplainAudit = 809,
            [Description("事故－员工建议")]
            ComplainEmployeeSuggest = 810,

            [Description("发送手机短信")]
            SendSMS = 850,

            //报表查看
            [Description("销售报表")]
            SaleReport = 901,
            [Description("商品销售分析全部")]
            SaleAnalysisAll = 902,
            [Description("商品销售分析自己")]
            SaleAnalysisOwn = 903,
            [Description("库存报表全部")]
            InventoryReportAll = 904,
            [Description("库存报表自己")]
            InventoryReportOwn = 905,
            [Description("PM销售报表全部")]
            PMSaleReportAll = 906,
            [Description("PM销售报表自己")]
            PMSaleReportOwn = 907,
            [Description("PM占用资金全部")]
            PMFundsReportAll = 908,
            [Description("PM占用资金自己")]
            PMFundsReportOwn = 909,
            [Description("PM帐期全部")]
            PMPayDateReportAll = 910,
            [Description("PM帐期自己")]
            PMPayDateReportOwn = 911,
            [Description("出入库单量")]
            WarehouseWorkload = 912,
            [Description("商品价格调整报表全部")]
            PriceChangeReportAll = 913,
            [Description("商品价格调整报表自己")]
            PriceChangeReportOwn = 914,
            [Description("出库销售单统计")]
            SOOutStockSearchSummary = 915,
            [Description("商品采购成本报表全部")]
            PurchaseCostReportAll = 916,
            [Description("商品采购成本报表自己")]
            PurchaseCostReportOwn = 917,
            [Description("商品点击率报表全部")]
            ProductDailyClickAll = 918,
            [Description("商品点击率报表自己")]
            ProductDailyClickOwn = 919,
            [Description("积分报表")]
            PointReport = 920,
            [Description("RMA报表")]
            RMAReportSearch = 921,
            [Description("运费报表")]
            ShipPriceSearch = 922,
            [Description("发票报表自己")]
            InvoiceReportOwn = 923,
            [Description("发票报表全部")]
            InvoiceReportAll = 924,
            [Description("数据初始化")]
            SystemDataInit = 900,
            [Description("全权")]
            Administrator = 999,
        }
        public static SortedList GetPrivilege()
        {
            return GetStatus(typeof(Privilege));
        }

        public static string GetPrivilege(object v)
        {
            return GetDescription(typeof(Privilege), v);
        }
        //------------------------------------------------------
        #endregion

        //需要同步的数据. 
        #region Sync
        //===============================================
        public enum Sync : int
        {
            [Description("Icson.BLL.Basic.ASPManager")]
            ASP = 1,
            [Description("Icson.BLL.Basic.CurrencyManager")]
            Currency = 2,
            [Description("Icson.BLL.Basic.CategoryManager")]
            Category = 3,
            [Description("Icson.BLL.Basic.StockManager")]
            Stock = 4,
            [Description("Icson.BLL.Basic.UserRatioManager")]
            UserRatio = 5,
        }
        public static SortedList GetSync()
        {
            return GetStatus(typeof(Sync));
        }
        public static string GetSync(object v)
        {
            return GetDescription(typeof(Sync), v);
        }
        //------------------------------------------------------
        #endregion

        #region 短信优先级
        public enum SMSPriority : int
        {
            [Description("Low")]
            Low = 0,
            [Description("Normal")]
            Normal = 5,
            [Description("High")]
            High = 10
        }
        public static SortedList GetSMSPriority()
        {
            return GetStatus(typeof(SMSPriority));
        }
        public static string GetSMSPriority(object v)
        {
            return GetDescription(typeof(SMSPriority), v);
        }
        #endregion

        #region FeedBackStatus
        public enum FeedBackStatus : int
        {
            [Description("Orgin")]
            Orgin = 0,
            [Description("Handled")]
            Handled = 1,
            [Description("Show")]
            Show = 2,
            [Description("Abandon")]
            Abandon = 3
        }
        public static SortedList GetFeedBackStatus()
        {
            return GetStatus(typeof(FeedBackStatus));
        }
        public static string GetFeedBackStatus(object v)
        {
            return GetDescription(typeof(FeedBackStatus), v);
        }
        #endregion

        #region ProductNotifyStatus
        public enum ProductNotifyStatus : int
        {
            [Description("作废")]
            Abandon = -1,
            [Description("未通知")]
            UnNotify = 0,
            [Description("已通知")]
            Notified = 1
        }
        public static SortedList GetProductNotifyStatus()
        {
            return GetStatus(typeof(ProductNotifyStatus));
        }
        public static string GetProductNotifyStatus(object v)
        {
            return GetDescription(typeof(ProductNotifyStatus), v);
        }
        #endregion

        //RMA
        #region RMAStatus
        public enum RMAStatus
        {
            [Description("已作废")]
            Abandon = -1,
            [Description("待审核")]
            Origin = 0,
            [Description("已审核")]
            Audited = 1,
            [Description("待处理")]
            Received = 2,
            [Description("处理中")]
            Handled = 3,
            [Description("已处理")]
            Closed = 4,
            [Description("待发还")]
            WaitingRevert = 5
        }
        public static SortedList GetRMAStatus()
        {
            return GetStatus(typeof(RMAStatus));
        }
        public static string GetRMAStatus(object v)
        {
            return GetDescription(typeof(RMAStatus), v);
        }
        #endregion

        //RMA Version 2
        #region RMA Version 2
        //rma2 master status
        public enum RMAOutBoundStatus
        {
            [Description("已作废")]
            Abandon = -1,
            [Description("待送修")]
            Origin = 0,
            [Description("已送修")]
            SendAlready = 1,
            [Description("部分返还")]
            PartlyResponsed = 2,
            [Description("全部返还")]
            Responsed = 3,
        }
        public static SortedList GetRMAOutBoundStatus()
        {
            return GetStatus(typeof(RMAOutBoundStatus));
        }
        public static string GetRMAOutBoundStatus(object v)
        {
            return GetDescription(typeof(RMAOutBoundStatus), v);
        }

        //RMAOutBoundWithInvoice
        public enum OutBoundWithInvoice
        {
            [Description("送/无")]
            SendNoInvoice = 0,
            [Description("送/有")]
            SendWithInvoice = 1,
            [Description("送/返")]
            BackInvoice = 2,
            [Description("送/未返")]
            BackNoInvoice = 3,
        }
        public static SortedList GetOutBoundWithInvoice()
        {
            return GetStatus(typeof(OutBoundWithInvoice));
        }
        public static string GetOutBoundWithInvoice(object v)
        {
            return GetDescription(typeof(OutBoundWithInvoice), v);
        }

        // 投诉系统
        // 投诉状态
        public enum ComplainStatus
        {
            [Description("已作废")]
            Abandoned = -1,
            [Description("待处理")]
            Orgin = 0,
            [Description("处理中")]
            Replied = 1,
            [Description("处理完毕")]
            Handled = 2,
        }
        public static SortedList GetComplainStatus()
        {
            return GetStatus(typeof(ComplainStatus));
        }
        public static string GetComplainStatus(object v)
        {
            return GetDescription(typeof(ComplainStatus), v);
        }
        //投诉系统 


        //RMA_Request
        public enum RMARequestStatus
        {
            [Description("已作废")]
            Abandon = -1,
            [Description("待处理")]
            Orgin = 0,
            [Description("处理中")]
            Handling = 1,
            [Description("处理完毕")]
            Closed = 2,
        }
        public static SortedList GetRMARequestStatus()
        {
            return GetStatus(typeof(RMARequestStatus));
        }
        public static string GetRMARequestStatus(object v)
        {
            return GetDescription(typeof(RMARequestStatus), v);
        }

        //RMARequest 类型
        public enum RMARequestType
        {
            [Description("不确定")]
            Unsure = 0,
            [Description("申请返修")]
            Maintain = 1,
            [Description("申请退货")]
            Return = 2,
            [Description("拒绝申请")]
            Overrule = 3
        }
        public static SortedList GetRMARequestType()
        {
            return GetStatus(typeof(RMARequestType));
        }
        public static string GetRMARequestType(object v)
        {
            return GetDescription(typeof(RMARequestType), v);
        }

        //RMARevertStatus 类型
        public enum RMARevertStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("待发还")]
            WaitingRevert = 0,
            [Description("已发还")]
            Reverted = 1,
            [Description("待审核")]
            WaitingAudit = 2
        }
        public static SortedList GetRMARevertStatus()
        {
            return GetStatus(typeof(RMARevertStatus));
        }
        public static string GetRMARevertStatus(object v)
        {
            return GetDescription(typeof(RMARevertStatus), v);
        }

        //RMARefundStatus 类型
        public enum RMARefundStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("待退款")]
            WaitingRefund = 0,
            [Description("已审核")]
            Audited = 1,
            [Description("已退款")]
            Refunded = 2
        }
        public static SortedList GetRMARefundStatus()
        {
            return GetStatus(typeof(RMARefundStatus));
        }
        public static string GetRMARefundStatus(object v)
        {
            return GetDescription(typeof(RMARefundStatus), v);
        }
        //RMASendAccessoryStatus 类型
        public enum RMASendAccessoryStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("待审核")]
            WaitingAudit = 0,
            [Description("待发货")]
            WaitingSend = 1,
            [Description("已发货")]
            Sent = 2
        }

        public static SortedList GetRMASendAccessoryStatus()
        {
            return GetStatus(typeof(RMASendAccessoryStatus));
        }
        public static string GetRMASendAccessoryStatus(object v)
        {
            return GetDescription(typeof(RMASendAccessoryStatus), v);
        }
        //RMAAccessoryType 类型
        public enum RMAAccessoryType
        {
            [Description("附件")]
            Accessory = 0,
            [Description("赠品")]
            Gift = 1,
            [Description("赠品+附件")]
            GiftandAccessory = 2,
        }
        public static SortedList GetRMAAccessoryType()
        {
            return GetStatus(typeof(RMAAccessoryType));
        }
        public static string GetRMAAccessoryType(object v)
        {
            return GetDescription(typeof(RMAAccessoryType), v);
        }

        //RMARequestReason 类型
        public enum RMARequestReasonType
        {
            [Description("质量问题")]
            QualityReason = 0,
            [Description("兼容性")]
            Conmpatibility = 1,
            [Description("不满意")]
            Discontented = 2,
            [Description("运输中损坏")]
            AttaintInTransport = 3
        }
        public static SortedList GetRMARequestReasonType()
        {
            return GetStatus(typeof(RMARequestReasonType));
        }
        public static string GetRMARequestReasonType(object v)
        {
            return GetDescription(typeof(RMARequestReasonType), v);
        }

        //RMARefundPayType 类型

        public enum RMARefundPayType : int
        {
            [Description("转积分退款")]
            TransferPointRefund = 1,
            [Description("现金退款")]
            CashRefund = 0,
            //[Description("银行退款")]
            //BankRefund = 2
            [Description("支付宝退款")]
            AlipayRefund = 2,
            [Description("工行退款")]
            ICBCRefund = 3,
            [Description("建行退款")]
            CBCRefund = 4,
            [Description("招行退款")]
            CMBRefund = 5,
            [Description("浦发退款")]
            SPDBRefund = 6,
            [Description("农行退款")]
            ABCRefund = 7,
            [Description("交行退款")]
            COMMRefund = 8,
            [Description("邮局退款")]
            PostRefund = 9,
            [Description("其他退款")]
            OtherRefund = 10
            //工行、建行、招行、浦发、农行、交行
        }
        public static SortedList GetRMARefundPayType()
        {
            return GetStatus(typeof(RMARefundPayType));
        }
        public static string GetRMARefundPayType(object v)
        {
            return GetDescription(typeof(RMARefundPayType), v);
        }


        //RMAReturnStatus 类型
        public enum RMAReturnStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("待退入库")]
            WaitingReturn = 0,
            [Description("已退入库")]
            Returned = 1
        }
        public static SortedList GetRMAReturnStatus()
        {
            return GetStatus(typeof(RMAReturnStatus));
        }
        public static string GetRMAReturnStatus(object v)
        {
            return GetDescription(typeof(RMAReturnStatus), v);
        }

        //RMANextHandler 类型
        public enum RMANextHandler
        {
            [Description("CC")]
            CC = 0,
            [Description("RMA")]
            RMA = 1
        }
        public static SortedList GetRMANextHandler()
        {
            return GetStatus(typeof(RMANextHandler));
        }
        public static string GetRMANextHandler(object v)
        {
            return GetDescription(typeof(RMANextHandler), v);
        }

        //RMA Revert Location Status
        public enum RevertLocationStatus
        {
            [Description("未指定")]
            unknown = 0,
            [Description("本地")]
            Local = 1,
            [Description("外地")]
            UnLocal = 2
        }
        public static SortedList GetRevertLocationStatus()
        {
            return GetStatus(typeof(RevertLocationStatus));
        }
        public static string GetRevertLocationStatus(object v)
        {
            return GetDescription(typeof(RevertLocationStatus), v);
        }

        //RMA Product OwnBy
        public enum RMAOwnBy
        {
            [Description("非处理状态")]
            Origin = 0,
            [Description("客户")]
            Customer = 1,
            [Description("ORS商城")]
            Icson = 2
        }
        public static SortedList GetRMAOwnBy()
        {
            return GetStatus(typeof(RMAOwnBy));
        }
        public static string GetRMAOwnBy(object v)
        {
            return GetDescription(typeof(RMAOwnBy), v);
        }

        //RMA Product Location
        public enum RMALocation
        {
            [Description("非处理状态")]
            Origin = 0,
            [Description("ORS商城")]
            Icson = 1,
            [Description("供应商")]
            Vendor = 2
        }
        public static SortedList GetRMALocation()
        {
            return GetStatus(typeof(RMALocation));
        }
        public static string GetRMALocation(object v)
        {
            return GetDescription(typeof(RMALocation), v);
        }

        #endregion

        #region RMA_Register表中新品标志的枚举对象

        public enum NewProductStatus
        {
            [Description("非换货")]
            Origin = 0,
            [Description("调新品")]
            NewProduct = 1,
            [Description("调二手")]
            SecondHand = 2,
            [Description("非当前Case产品")]
            OtherProduct = 3
        }

        /// <summary>
        /// 获取新品枚举对象的值
        /// </summary>
        /// <returns></returns>
        public static SortedList GetNewProductStatus()
        {
            return GetStatus(typeof(NewProductStatus));
        }
        public static string GetNewProductStatus(object v)
        {
            return GetDescription(typeof(NewProductStatus), v);
        }
        #endregion


        #region RMA处理人
        public enum RMAHandler : int
        {
            [Description("CallCenter")]
            CallCenter = 0,
            [Description("RMA")]
            RMA = 1
        }
        public static SortedList GetRMAHandler()
        {
            return GetStatus(typeof(RMAHandler));
        }
        public static string GetRMAHandler(object v)
        {
            return GetDescription(typeof(RMAHandler), v);
        }
        #endregion

        #region ReturnType
        public enum ReturnType : int
        {
            [Description("SecondHand")]
            SecondHand = 0,
            [Description("Bad")]
            Bad = 1,
            [Description("New")]
            New = 2
        }
        public static SortedList GetReturnType()
        {
            return GetStatus(typeof(ReturnType));
        }
        public static string GetReturnType(object v)
        {
            return GetDescription(typeof(ReturnType), v);
        }
        #endregion

        #region ReturnPriceType
        public enum ReturnPriceType : int
        {
            [Description("TenPercentsOff")]
            TenPercentsOff = 0,
            [Description("OriginPrice")]
            OriginPrice = 1,
            [Description("InputPrice")]
            InputPrice = 2
        }
        public static SortedList GetReturnPriceType()
        {
            return GetStatus(typeof(ReturnPriceType));
        }
        public static string GetReturnPriceType(object v)
        {
            return GetDescription(typeof(ReturnPriceType), v);
        }
        #endregion

        //Online Show
        #region PageSize
        public const int PageSize = 10;
        public const int PageStart = 1;
        #endregion

        //商品属性类型
        #region AttributeType 0,1,2,3
        //===========================================
        public enum AttributeType : int
        {
            [Description("普通文本")]
            Text = 0,
            [Description("选项【索引】")]
            OptionFirst = 1,
            [Description("选项")]
            OptionSecond = 2,
            [Description("选项")]
            OptionThird = 3
        }
        public static SortedList GetAttributeType()
        {
            return GetStatus(typeof(AttributeType));
        }
        public static string GetAttributeType(object v)
        {
            return GetDescription(typeof(AttributeType), v);
        }
        //--------------------------------------------
        #endregion

        //物流状态查询类型
        #region ShipStatusQueryType 1,2,3,4
        //===========================================
        public enum ShipStatusQueryType : int
        {
            [Description("DeliveryManLink")]
            DeliveryManLink = 1,  //显示快递师傅联系方式
            [Description("ExpressLink")]
            ExpressLink = 2,          //快递公司连接
            [Description("QueryUrl")]
            QueryUrl = 3,                 //快递公司查询url
            [Description("NoneQuery")]
            NoneQuery = 4                //无法查询
        }
        public static SortedList GetShipStatusQueryType()
        {
            return GetStatus(typeof(ShipStatusQueryType));
        }
        public static string GetShipStatusQueryType(object v)
        {
            return GetDescription(typeof(ShipStatusQueryType), v);
        }
        //--------------------------------------------
        #endregion

        #region 限时销售状态
        public enum CountdownStatus : int
        {
            [Description("作废")]
            Abandon = -1,
            [Description("就绪")]
            Ready = 0,
            [Description("运行")]
            Running = 1,
            [Description("完成")]
            Finish = 2,
            [Description("中止")]
            Interupt = -2
        }
        public static SortedList GetCountdownStatus()
        {
            return GetStatus(typeof(CountdownStatus));
        }
        public static string GetCountdownStatus(object v)
        {
            return GetDescription(typeof(CountdownStatus), v);
        }
        #endregion

        #region 限时销售类型
        public enum CountdownType : int
        {
            [Description("一次性")]
            OneTime = 1,
            [Description("每天")]
            EveryDay = 2
        }
        public static SortedList GetCountdownType()
        {
            return GetStatus(typeof(CountdownType));
        }
        public static string GetCountdownType(object v)
        {
            return GetDescription(typeof(CountdownType), v);
        }
        #endregion

        //会员等级
        #region CustomerRank
        public enum CustomerRank : int
        {
            [Description("普通会员")]
            Ordinary = 0,
            [Description("一星会员")]
            OneStar = 1,
            [Description("二星会员")]
            TwoStar = 2,
            [Description("三星会员")]
            ThreeStar = 3,
            [Description("四星会员")]
            FourStar = 4,
            [Description("五星会员")]
            FiveStar = 5,
            [Description("黄金会员")]
            Golden = 6,
            [Description("钻石会员")]
            Diamond = 7,
            [Description("至尊VIP会员")]
            VIP = 8
        }
        public static SortedList GetCustomerRank()
        {
            return GetStatus(typeof(CustomerRank));
        }
        public static string GetCustomerRank(object v)
        {
            return GetDescription(typeof(CustomerRank), v);
        }
        #endregion

        #region Customer Rank
        public enum CustomerVIPRank : int
        {
            [Description("普通(自动)")]
            AutoNonVIP = 1,
            [Description("VIP(自动)")]
            AutoVIP = 2,
            [Description("普通(手动)")]
            ManualNonVIP = 3,
            [Description("VIP(手动)")]
            ManualVIP = 4
        }
        public static SortedList GetCustomerVIPRank()
        {
            return GetStatus(typeof(CustomerVIPRank));
        }
        public static string GetCustomerVIPRank(object v)
        {
            return GetDescription(typeof(CustomerVIPRank), v);
        }
        #endregion

        #region AuctionRank
        public enum AuctionRank : int
        {
            [Description("铁")]
            A = 0,
            [Description("铜")]
            B = 1,
            [Description("银")]
            C = 2,
            [Description("金")]
            D = 3,
            [Description("臭")]
            E = 4,
        }
        public static SortedList GetAuctionRank()
        {
            return GetStatus(typeof(AuctionRank));
        }
        public static string GetAuctionRank(object v)
        {
            return GetDescription(typeof(AuctionRank), v);
        }
        #endregion

        //会员类型
        #region CustomerType
        public enum CustomerType : int
        {
            [Description("个人用户")]
            Personal = 0,
            [Description("公司员工")]
            Employee = 1,
            [Description("淘宝代理")]
            Taobao = 2,
            [Description("易趣代理")]
            Ebay = 3,
            [Description("拍拍代理")]
            Paipai = 4,
            [Description("企业大客户")]
            Enterprice = 5,
            [Description("企业VIP")]
            VIP = 6
        }
        public static SortedList GetCustomerType()
        {
            return GetStatus(typeof(CustomerType));
        }
        public static string GetCustomerType(object v)
        {
            return GetDescription(typeof(CustomerType), v);
        }
        #endregion

        //盘点类型
        #region InventoryReportType
        public enum InventoryReportType : int
        {
            [Description("每天盘点")]
            EveryDay = 1,
            [Description("每周盘点")]
            EveryWeek = 2,
            [Description("每月盘点")]
            EveryMonth = 3
        }
        public static SortedList GetInventoryReportType()
        {
            return GetStatus(typeof(InventoryReportType));
        }

        public static string GetInventoryReportType(object v)
        {
            return GetDescription(typeof(InventoryReportType), v);
        }
        #endregion

        //客户推荐状态
        #region CommendStatus
        public enum CommendStatus : int
        {
            [Description("原始状态")]
            Primitive = -1, //原始状态为把Email加入推荐列表
            [Description("初始状态")]
            Origin = 0,     //初始状态为已推荐
            [Description("推荐客户已注册")]
            Registered = 1,
            [Description("推荐客户已购物")]
            Bought = 2
        }
        public static SortedList GetCommendStatus()
        {
            return GetStatus(typeof(CommendStatus));
        }

        public static string GetCommendStatus(object v)
        {
            return GetDescription(typeof(CommendStatus), v);
        }
        #endregion

        //免运费赠送原因种类
        #region FreeShipFeeLogType
        //===========================================
        public enum FreeShipFeeLogType : int
        {
            [Description("推荐客户Email")]
            CommendCustomer = 1,
            [Description("推荐的客户注册")]
            CustomerRegister = 2,
            [Description("推荐的客户首次购物")]
            CustomerBuyFirst = 3,
            [Description("订单支付运费")]
            CreateOrder = 4,
            [Description("作废订单")]
            AbandonSO = 5,
            [Description("取消作废订单")]
            CancelAbandonSO = 6,
            [Description("更新订单")]
            UpdateSO = 7
        }
        public static SortedList GetFreeShipFeeLogType()
        {
            return GetStatus(typeof(FreeShipFeeLogType));
        }
        public static string GetFreeShipFeeLogType(object v)
        {
            return GetDescription(typeof(FreeShipFeeLogType), v);
        }
        //--------------------------------------------
        #endregion

        //在线推荐种类:特价,新品,精选,热销,促销
        //YoeJoy项目只用到1~4
        #region OnlineRecommendType
        public enum OnlineRecommendType : int
        {
            [Description("促销商品")]
            Discount = 1,
            [Description("新品上市")]
            NewArrive = 2,
            [Description("限时抢购")]
            Featured = 3,
            [Description("团购")]
            HotSale = 4,//热销
            [Description("清库产品")]
            Promotion = 5,
            [Description("本周销量排行")]
            PromotionTopic = 6,
            [Description("最新降价")]
            PopularProduct = 7,
            [Description("产品热评")]
            PowerfulSale = 8,
            [Description("热卖推荐")]
            ExcellentRecommend = 9,
            [Description("品牌推荐")]
            PromotionBrand = 10,
        }
        public static SortedList GetOnlineRecommendType()
        {
            return GetStatus(typeof(OnlineRecommendType));
        }
        public static string GetOnlineRecommendType(object v)
        {
            return GetDescription(typeof(OnlineRecommendType), v);
        }
        #endregion

        //在线显示区域种类
        #region OnlineAreaType
        public enum OnlineAreaType : int
        {
            [Description("首页")]
            HomePage = 1,
            [Description("一级分类")]
            FirstCategory = 2,
            [Description("二级分类")]
            SecondCategory = 3,
            [Description("三级分类")]
            ThirdCategory = 4,
            [Description("商品页面")]
            ItemDetail = 5,
            [Description("在线帮助")]
            ServiceFaq = 6,
            [Description("帐户中心")]
            AccountCenter = 7,
            [Description("搜索页")]
            Search = 8,
            [Description("购物车")]
            ShoppingCart = 9
        }
        public static SortedList GetOnlineAreaType()
        {
            return GetStatus(typeof(OnlineAreaType));
        }
        public static string GetOnlineAreaType(object v)
        {
            return GetDescription(typeof(OnlineAreaType), v);
        }
        #endregion

        //QA类型
        #region QAType
        public enum QAType : int
        {
            //[Description("网站介绍")] Introduce = 1,
            //[Description("购物流程")] ShoppingFlow = 2,
            //[Description("售后服务")] Service = 3,
            //[Description("知识性")] Knowledge = 4,
            //[Description("技术性")] Technology = 5
            //专题列表，行情速递，ORS商城学堂，常见问题，选购指南，使用技巧，入门知识
            [Description("专题列表")]
            ThematicList = 1,
            [Description("行情速递")]
            MarketExpress = 2,
            [Description("ORS商城学堂")]
            IcsonStudy = 3,
            [Description("常见问题")]
            FAQ = 4,
            [Description("选购指南")]
            BuyingGuide = 5,
            [Description("使用技巧")]
            UsingSkill = 6,
            [Description("入门知识")]
            EntryKnowledge = 7
        }
        public static SortedList GetQAType()
        {
            return GetStatus(typeof(QAType));
        }
        public static string GetQAType(object v)
        {
            return GetDescription(typeof(QAType), v);
        }
        #endregion

        //评论系统枚举库
        #region  评论系统
        /// <summary>
        /// Topic 类型 ,Experience(体验)=1,Discuss(参考)=2
        /// </summary>
        public enum TopicType : int
        {
            [Description("体验")]
            Experience = 1,
            [Description("参考")]
            Discuss = 2,
        }
        public static SortedList GetTopicType()
        {
            return GetStatus(typeof(TopicType));
        }

        public static string GetTopicType(object v)
        {
            return GetDescription(typeof(TopicType), v);
        }
        /// <summary>
        /// 评论系统中客户权限等级
        /// </summary>
        public enum TopicCustomerRight
        {
            [Description("A1")]
            A1 = 1,
            [Description("A2")]
            B2 = 2,
            [Description("A3")]
            C3 = 3,
        }
        public static SortedList GetTopicCustomerRight()
        {
            return GetStatus(typeof(TopicCustomerRight));
        }

        public static string GetTopicCustomerRight(object v)
        {
            return GetDescription(typeof(TopicCustomerRight), v);
        }

        /// <summary>
        /// Topic 的状态
        /// </summary>
        public enum TopicStatus
        {
            [Description("已作废")]
            Abandon = -2,
            [Description("未审核")]
            unconfirmed = -1,
            [Description("已审核")]
            confirmed = 1,
            [Description("已回复")]
            Replyed = 2,
        }

        public static SortedList GetTopicStatus()
        {
            return GetStatus(typeof(TopicStatus));
        }

        public static string GetTopicStatus(object v)
        {
            return GetDescription(typeof(TopicStatus), v);
        }

        /// <summary>
        /// Topic Reply 的状态
        /// </summary>
        public enum TopicReplyStatus
        {
            [Description("普通")]
            Normal = 0,
            [Description("已作废")]
            Abandon = 1,
        }
        public static SortedList GetTopicReplyStatus()
        {
            return GetStatus(typeof(TopicReplyStatus));
        }

        public static string GetTopicReplyStatus(object v)
        {
            return GetDescription(typeof(TopicReplyStatus), v);
        }

        /// <summary>
        /// Normal = 0,Abandon = 1
        /// </summary>
        public enum TopicImageStatus
        {
            [Description("普通")]
            Normal = 0,
            [Description("已作废")]
            Abandon = 1,
        }
        public static SortedList GetTopicImageStatus()
        {
            return GetStatus(typeof(TopicImageStatus));
        }

        public static string GetTopicImageStatus(object v)
        {
            return GetDescription(typeof(TopicImageStatus), v);
        }

        public enum TopicUpdateType
        {
            [Description("屏蔽评论")]
            AbandonTopic,
            [Description("撤销屏蔽评论")]
            CancelAbandonTopic,
            [Description("置顶评论")]
            TopicSetTop,
            [Description("设置精华")]
            TopicSetDigset,
            [Description("取消置顶")]
            TopicCancelTop,
            [Description("取消精华")]
            TopicCancelDigset,
            [Description("审核评论")]
            ConfirmTopic,
            [Description("取消审核评论")]
            UnConfirmTopic,

        }
        public static SortedList GetTopicUpdateType()
        {
            return GetStatus(typeof(TopicUpdateType));
        }

        public static string GetTopicUpdateType(object v)
        {
            return GetDescription(typeof(TopicUpdateType), v);
        }
        /// <summary>
        /// 显示随机精华 Topic 的页面类型
        /// </summary>
        public enum RandomDigestTopicPage
        {
            Homepage,
            ProductDetail,
        }

        /// <summary>
        /// Topic 列表的排序规则
        /// </summary>
        public enum TopicListSortBy
        {
            [Description("建立时间排序")]
            CreateDate,
            [Description("评分排序")]
            Score,
            [Description("评论总数排序")]
            TopicCount,
        }
        public static SortedList GetTopicListSortBy()
        {
            return GetStatus(typeof(TopicListSortBy));
        }

        public static string GetTopicListSortBy(object v)
        {
            return GetDescription(typeof(TopicListSortBy), v);
        }
        public enum TopicReferenceType
        {
            [Description("商品")]
            Product = 0,
            [Description("类别")]
            Category = 1,
        }
        public static SortedList GetTopicReferenceType()
        {
            return GetStatus(typeof(TopicReferenceType));
        }

        public static string GetTopicReferenceType(object v)
        {
            return GetDescription(typeof(TopicReferenceType), v);
        }
        #endregion

        //评论系统枚举库
        #region  评论系统

        /// <summary>
        /// 评分
        /// </summary>
        public enum ReviewScore : int
        {
            [Description("5分 很好")]
            Excellent = 5,
            [Description("4分 好")]
            Good = 4,
            [Description("3分 一般")]
            Average = 3,
            [Description("2分 差")]
            Poor = 2,
            [Description("1分 很差")]
            VeryPoor = 1,
        }
        public static SortedList GetReviewScore()
        {
            return GetStatus(typeof(ReviewScore));
        }

        public static string GetReviewScore(object v)
        {
            return GetDescription(typeof(ReviewScore), v);
        }

        /// <summary>
        /// 拥有时间
        /// </summary>
        public enum OwnedType : int
        {
            [Description("0-1天")]
            OneDay = 1,
            [Description("1天-1周")]
            OneWeek = 2,
            [Description("1周-1月")]
            OneMonth = 3,
            [Description("1月-1年")]
            OneYear = 4,
            [Description("1年以上")]
            MoreYear = 5,
        }

        public static SortedList GetOwnedType()
        {
            return GetStatus(typeof(OwnedType));
        }

        public static string GetOwnedType(object v)
        {
            return GetDescription(typeof(OwnedType), v);
        }

        public enum UnderstandType : int
        {
            [Description("1 - 不熟悉")]
            LowUnderstand = 1,
            [Description("2 - 熟悉一点点")]
            SomewhatUnderstand = 2,
            [Description("3 - 基本熟悉")]
            AverageUnderstand = 3,
            [Description("4 - 很熟悉")]
            HighUnderstand = 4,
            [Description("5 - 精通")]
            MasterUnderstand = 5,
        }

        public static SortedList GetUnderstandType()
        {
            return GetStatus(typeof(UnderstandType));
        }

        public static string GetUnderstandType(object v)
        {
            return GetDescription(typeof(UnderstandType), v);
        }

        /// <summary>
        /// Review 类型 ,Experience(体验)=1,Discuss(参考)=2
        /// </summary>
        public enum ReviewType : int
        {
            [Description("体验")]
            Experience = 1,
            [Description("讨论")]
            Discuss = 2,
            [Description("推荐")]
            Recommend = 3,
            [Description("询问")]
            Inquiry = 4,
        }
        public static SortedList GetReviewType()
        {
            return GetStatus(typeof(ReviewType));
        }

        public static string GetReviewType(object v)
        {
            return GetDescription(typeof(ReviewType), v);
        }
        /// <summary>
        /// 评论系统中客户权限等级
        /// </summary>
        public enum ReviewCustomerRight
        {
            [Description("A1")]
            A1 = 1,
            [Description("A2")]
            B2 = 2,
            [Description("A3")]
            C3 = 3,
        }
        public static SortedList GetReviewCustomerRight()
        {
            return GetStatus(typeof(ReviewCustomerRight));
        }

        public static string GetReviewCustomerRight(object v)
        {
            return GetDescription(typeof(ReviewCustomerRight), v);
        }

        /// <summary>
        /// Review 的状态
        /// </summary>
        public enum ReviewStatus
        {
            [Description("已作废")]
            Abandon = -2,
            [Description("未审核")]
            UnConfirmed = -1,
            [Description("已审核")]
            Confirmed = 1,
            [Description("已回复")]
            Replyed = 2,
        }

        public static SortedList GetReviewStatus()
        {
            return GetStatus(typeof(ReviewStatus));
        }

        public static string GetReviewStatus(object v)
        {
            return GetDescription(typeof(ReviewStatus), v);
        }

        /// <summary>
        /// Review Reply 的状态
        /// </summary>
        public enum ReviewReplyStatus
        {
            [Description("正常显示")]
            Normal = 0,
            [Description("已作废")]
            Abandon = 1,
            [Description("待审核")]
            WaitingAudit = 2,
        }
        public static SortedList GetReviewReplyStatus()
        {
            return GetStatus(typeof(ReviewReplyStatus));
        }

        public static string GetReviewReplyStatus(object v)
        {
            return GetDescription(typeof(ReviewReplyStatus), v);
        }

        /// <summary>
        /// Normal = 0,Abandon = 1
        /// </summary>
        public enum ReviewImageStatus
        {
            [Description("普通")]
            Normal = 0,
            [Description("已作废")]
            Abandon = 1,
        }
        public static SortedList GetReviewImageStatus()
        {
            return GetStatus(typeof(ReviewImageStatus));
        }

        public static string GetReviewImageStatus(object v)
        {
            return GetDescription(typeof(ReviewImageStatus), v);
        }

        /// <summary>
        /// BBS Record CreateUserType (Customer / Employe)
        /// </summary>
        public enum CreateUserType
        {
            [Description("顾客")]
            Customer = 0,
            [Description("CC/PM/APM")]
            Employee = 1,
        }
        public static SortedList GetCreateUserType()
        {
            return GetStatus(typeof(CreateUserType));
        }

        public static string GetCreateUserType(object v)
        {
            return GetDescription(typeof(CreateUserType), v);
        }

        public enum ReviewUpdateType
        {
            [Description("屏蔽评论")]
            AbandonReview,
            [Description("撤销屏蔽评论")]
            CancelAbandonReview,
            [Description("置顶评论")]
            ReviewSetTop,
            [Description("设置精华")]
            ReviewSetGood,
            [Description("取消置顶")]
            ReviewCancelTop,
            [Description("取消精华")]
            ReviewCancelGood,
            [Description("审核评论")]
            ConfirmReview,
            [Description("取消审核评论")]
            UnConfirmReview,

        }
        public static SortedList GetReviewUpdateType()
        {
            return GetStatus(typeof(ReviewUpdateType));
        }

        public static string GetReviewUpdateType(object v)
        {
            return GetDescription(typeof(ReviewUpdateType), v);
        }
        /// <summary>
        /// 显示随机精华 Review 的页面类型
        /// </summary>
        public enum RandomDigestReviewPage
        {
            Homepage,
            ProductDetail,
        }

        /// <summary>
        /// Review 列表的排序规则
        /// </summary>
        public enum ReviewListSortBy
        {
            [Description("建立时间排序")]
            CreateDate,
            [Description("评分排序")]
            Score,
            [Description("评论总数排序")]
            ReviewCount,
        }
        public static SortedList GetReviewListSortBy()
        {
            return GetStatus(typeof(ReviewListSortBy));
        }

        public static string GetReviewListSortBy(object v)
        {
            return GetDescription(typeof(ReviewListSortBy), v);
        }
        public enum ReviewReferenceType
        {
            [Description("商品")]
            Product = 0,
            [Description("类别")]
            Category = 1,
            [Description("促销")]
            Promotion = 2,
            [Description("装机宝")]
            DIY = 3,
        }
        public static SortedList GetReviewReferenceType()
        {
            return GetStatus(typeof(ReviewReferenceType));
        }

        public static string GetReviewReferenceType(object v)
        {
            return GetDescription(typeof(ReviewReferenceType), v);
        }
        #endregion

        #region 代销库存转财务库存的类型：销售、手动结算
        public enum ConsignToAccType : int
        {
            [Description("SO")]
            SO = 0,
            [Description("Manual")]
            Manual = 1
        }
        public static SortedList GetConsignToAccType()
        {
            return GetStatus(typeof(ConsignToAccType));
        }
        public static string GetConsignToAccType(object v)
        {
            return GetDescription(typeof(ConsignToAccType), v);
        }
        #endregion

        #region 代销转财务单状态
        public enum ConsignToAccStatus : int
        {
            [Description("Origin")]
            Origin = 0,
            [Description("FinanceSettled")]
            FinanceSettled = 1,
            [Description("VendorSettled")]
            VendorSettled = 2
        }
        public static SortedList GetConsignToAccStatus()
        {
            return GetStatus(typeof(ConsignToAccStatus));
        }
        public static string GetConsignToAccStatus(object v)
        {
            return GetDescription(typeof(ConsignToAccStatus), v);
        }
        #endregion

        // 建议退款的标志
        public enum RecommendRefund : int
        {
            [Description("YES")]
            Yes = 1,
            [Description("NO")]
            No = 0
        }

        //仓库类型
        #region StockType
        public enum StockType : int
        {
            [Description("正常仓库")]
            Normal = 1,
            [Description("RMA仓库")]
            RMA = 2
        }
        public static SortedList GetStockType()
        {
            return GetStatus(typeof(StockType));
        }

        public static string GetStockType(object v)
        {
            return GetDescription(typeof(StockType), v);
        }
        #endregion

        //添加积分状态
        #region PointRequestStatus
        public enum PointRequestStatus : int
        {
            [Description("Abandon")]
            Abandon = -1,
            [Description("Origin")]
            Origin = 1,
            [Description("Audited")]
            Audited = 2,
            [Description("Added")]
            Added = 3
        }
        public static SortedList GetPointRequestStatus()
        {
            return GetStatus(typeof(PointRequestStatus));
        }

        public static string GetPointRequestStatus(object v)
        {
            return GetDescription(typeof(PointRequestStatus), v);
        }
        #endregion

        //竞争对手
        #region Competitor
        //===========================================
        public enum Competitor : int
        {
            [Description("其他")]
            Other = 0,
            [Description("京东")]
            JD = 1,
            [Description("新蛋")]
            NewEgg = 2,
            [Description("小熊")]
            Bear = 3
        }
        public static SortedList GetCompetitor()
        {
            return GetStatus(typeof(Competitor));
        }
        public static string GetCompetitor(object v)
        {
            return GetDescription(typeof(Competitor), v);
        }
        //--------------------------------------------
        #endregion

        //价格举报
        #region PriceReportStatus
        //===========================================
        public enum PriceReportStatus : int
        {
            [Description("作废")]
            Abandon = -1,
            [Description("初始")]
            Origin = 0,
            [Description("审核未调整价格")]
            AuditedNotChangePrice = 1,
            [Description("审核已调整价格")]
            AuditChangePrice = 2,
            [Description("审核待调整价格")]
            AuditWaitingChangePrice = 3
        }
        public static SortedList GetPriceReportStatus()
        {
            return GetStatus(typeof(PriceReportStatus));
        }
        public static string GetPriceReportStatus(object v)
        {
            return GetDescription(typeof(PriceReportStatus), v);
        }
        //--------------------------------------------
        #endregion

        //价格举报原因分析
        #region PriceReportReason
        //===========================================
        public enum PriceReportReason : int
        {
            [Description("其他")]
            Other = 0,
            [Description("无法查证")]
            NotAvailable = 1,
            [Description("不是同一商品")]
            NotSameProduct = 2,
            [Description("对方无货")]
            ProductNoStock = 3,
            [Description("价格相差不大")]
            PriceDiffLittle = 4,
            [Description("价格未跟踪")]
            PriceNotTrack = 5,
            [Description("价格悬殊,需核查成本")]
            PriceDiffMore = 6
        }
        public static SortedList GetPriceReportReason()
        {
            return GetStatus(typeof(PriceReportReason));
        }
        public static string GetPriceReportReason(object v)
        {
            return GetDescription(typeof(PriceReportReason), v);
        }
        //--------------------------------------------
        #endregion

        //价格举报处理类型
        #region PriceReportHandleType
        //===========================================
        public enum PriceReportHandleType : int
        {
            [Description("其他")]
            Other = 0,
            [Description("第一个举报***")]
            FirstReport = 1,
            [Description("第二个举报***")]
            SecondReport = 2,
            [Description("第三个举报***")]
            ThirdReport = 3
        }
        public static SortedList GetPriceReportHandleType()
        {
            return GetStatus(typeof(PriceReportHandleType));
        }
        public static string GetPriceReportHandleType(object v)
        {
            return GetDescription(typeof(PriceReportHandleType), v);
        }
        //--------------------------------------------
        #endregion

        //市场最低价状态
        #region MarketLowestPriceStatus
        //===========================================
        public enum MarketLowestPriceStatus : int
        {
            [Description("作废")]
            Abandon = -1,
            [Description("初始")]
            Origin = 0,
            [Description("审核")]
            Audit = 1
        }
        public static SortedList GetMarketLowestPriceStatus()
        {
            return GetStatus(typeof(MarketLowestPriceStatus));
        }
        public static string GetMarketLowestPriceStatus(object v)
        {
            return GetDescription(typeof(MarketLowestPriceStatus), v);
        }
        //--------------------------------------------
        #endregion


        //部门列表
        #region DepartmentID
        //===========================================
        public enum DepartmentID : int
        {
            [Description("经理总监")]
            Manager = 10,
            [Description("客服")]
            CS = 11,
            [Description("PM")]
            PM = 12,
            [Description("财务")]
            Finance = 13,
            [Description("仓库")]
            WH = 14,
            [Description("MIS")]
            MIS = 15,
            [Description("RMA")]
            RMA = 16,
            [Description("杭州分站")]
            HZ = 17,
            [Description("南京分站")]
            NJ = 18,
            [Description("苏州分站")]
            SZ = 19,
            [Description("扬州分站")]
            YZ = 20,
            [Description("上海快递师傅")]
            ExpressMan = 21,
            [Description("自提点")]
            Branch = 22,
            [Description("分站快递师傅")]
            BranchMan = 23,
            [Description("人事")]
            HR = 24,
        }
        public static SortedList GetDepartmentID()
        {
            return GetStatus(typeof(DepartmentID));
        }
        public static string GetDepartmentID(object v)
        {
            return GetDescription(typeof(DepartmentID), v);
        }
        //--------------------------------------------
        #endregion

        //事故类型
        public enum AbnormalType
        {
            [Description("其他")]
            Other = 0,
            [Description("投诉")]
            Complain = 1,
            [Description("客户建议")]
            CustomerSuggest = 2,
            [Description("内部建议")]
            EmployeeSuggest = 3
        }
        public static SortedList GetAbnormalType()
        {
            return GetStatus(typeof(AbnormalType));
        }
        public static string GetAbnormalType(object v)
        {
            return GetDescription(typeof(AbnormalType), v);
        }

        //事故原因
        public enum AbnormalCauseType
        {
            [Description("员工工作失职")]
            EmployeeNeglect = 1,
            [Description("快递公司失职")]
            ExpressNeglect = 2,
            [Description("工作人员品行问题")]
            EmployeeMoralFault = 3,
            [Description("员工建议")]
            EmployeeSuggest = 4,
            [Description("快递延缓配送")]
            ExpressDelay = 5,
            [Description("PM到货延迟,延缓正常配送")]
            PMDelay = 6,
            [Description("PM资料做错误导顾客")]
            ProductInfoError = 7,
            [Description("对待客户态度问题")]
            MisbehaviorToCustomer = 8,
            [Description("其他")]
            Other = 99
        }
        public static SortedList GetAbnormalCauseType()
        {
            return GetStatus(typeof(AbnormalCauseType));
        }
        public static string GetAbnormalCauseType(object v)
        {
            return GetDescription(typeof(AbnormalCauseType), v);
        }

        #region
        //是否为发货地址状态
        public enum IsRevertAddressStatus : int
        {
            [Description("Unsure")]
            Unsure = 2,
            [Description("YES")]
            Yes = 1,
            [Description("NO")]
            No = 0
        }
        public static SortedList GetIsRevertAddressStatus()
        {
            return GetStatus(typeof(IsRevertAddressStatus));
        }
        public static string GetIsRevertAddressStatus(object v)
        {
            return GetDescription(typeof(IsRevertAddressStatus), v);
        }
        //--------------------------------------------
        #endregion

        //RMAReceiveType 申请单收货类型
        public enum RMARecieveType
        {
            [Description("单件")]
            Single = 0,
            [Description("整机")]
            Host = 1
        }
        public static SortedList GetRMARecieveType()
        {
            return GetStatus(typeof(RMARecieveType));
        }
        public static string GetRMARecieveType(object v)
        {
            return GetDescription(typeof(RMARecieveType), v);
        }

        //RMARegisterReceiveType 单件收货类型
        public enum RMARegisterRecieveType
        {
            [Description("单件坏品")]
            SingleBad = 0,
            [Description("整机良品")]
            HostGood = 1,
            [Description("整机坏品")]
            HostBad = 2
        }
        public static SortedList GetRMARegisterRecieveType()
        {
            return GetStatus(typeof(RMARegisterRecieveType));
        }
        public static string GetRMARegisterRecieveType(object v)
        {
            return GetDescription(typeof(RMARegisterRecieveType), v);
        }

        //VirtualArriveTime 虚库到货时间
        public enum VirtualArriveTime
        {
            [Description("1-3日")]
            OneToThree = 1,
            [Description("2-7日")]
            TwoToSeven = 2
        }
        public static SortedList GetVirtualArriveTime()
        {
            return GetStatus(typeof(VirtualArriveTime));
        }
        public static string GetVirtualArriveTime(object v)
        {
            return GetDescription(typeof(VirtualArriveTime), v);
        }

        //供应商账期类型
        #region APType 1,2,3,4,5,6,7,8,9
        //===========================================
        public enum APType : int
        {
            [Description("预付款")]
            PayInAdvance = 1,
            [Description("货到且票到")]
            PayWhenArrive0 = 2,
            [Description("货到且票到1周")]
            PayWhenArrive7 = 3,
            [Description("货到且票到10天")]
            PayWhenArrive10 = 4,
            [Description("货到且票到2周")]
            PayWhenArrive14 = 5,
            [Description("货到且票到20天")]
            PayWhenArrive20 = 6,
            [Description("货到且票到1个月")]
            PayWhenArrive30 = 7,
            [Description("货到且票到45天")]
            PayWhenArrive45 = 8,
            [Description("每月25日且票到")]
            PayMonth25 = 9,
            [Description("每月10,25日且票到")]
            PayMonth10and25 = 10
        }
        public static SortedList GetAPType()
        {
            return GetStatus(typeof(APType));
        }
        public static string GetAPType(object v)
        {
            return GetDescription(typeof(APType), v);
        }
        //--------------------------------------------
        #endregion

        //WhWorkType 仓库工作类型
        public enum WhWorkType
        {
            [Description("检货")]
            ProductInspection = 1,
            [Description("上架")]
            ProductShelving = 2
        }
        public static SortedList GetWhWorkType()
        {
            return GetStatus(typeof(WhWorkType));
        }
        public static string GetWhWorkType(object v)
        {
            return GetDescription(typeof(WhWorkType), v);
        }

        //WhWorkBillType 仓库工作单据类型
        public enum WhWorkBillType
        {
            [Description("订单")]
            SO = 1,
            [Description("移库单")]
            Shift = 2,
            [Description("转换单")]
            Transfer = 3
        }
        public static SortedList GetWhWorkBillType()
        {
            return GetStatus(typeof(WhWorkBillType));
        }
        public static string GetWhWorkBillType(object v)
        {
            return GetDescription(typeof(WhWorkBillType), v);
        }

        //DeliveryDelayCause 配送延迟原因
        public enum DeliveryDelayCause
        {
            [Description("客户要求改日送")]
            CustomerChangeTime = 1,
            [Description("客户改地址重新配送")]
            CustomerChangeAddress = 2,
            [Description("客户拒收，订单取消")]
            CustomerRefuse = 3,
            [Description("师傅延缓配送改日送")]
            FreightManDelay = 4,
            [Description("师傅延缓配送，订单取消")]
            FreightManDelayCancelSO = 5,
            [Description("师傅服务态度问题，订单取消")]
            FreightManBadServiceAttitude = 6,
            [Description("客服订单地址审核错误，改日送")]
            CSAuditError = 7,
            [Description("客服未确认发票问题，改日送")]
            CSCannotRequestVat = 8,
            [Description("产品质量问题，改日送")]
            ProductQuality = 9,
            [Description("发票问题改日送")]
            VatProblem = 10,
            [Description("师傅联系不上客户，改日送")]
            CustomerCannotCantact = 11,
            [Description("其它")]
            Other = 99,
        }
        public static SortedList GetDeliveryDelayCause()
        {
            return GetStatus(typeof(DeliveryDelayCause));
        }
        public static string GetDeliveryDelayCause(object v)
        {
            return GetDescription(typeof(DeliveryDelayCause), v);
        }

        /// <summary>
        /// PM付款单异常申请状态
        /// </summary>
        public enum POPayItemErrRequestStatus
        {
            [Description("PMD已驳回")]
            PMDReturn = -3,
            [Description("TL已驳回")]
            TLReturn = -2,
            [Description("PM取消申请")]
            PMCancelRequest = -1,
            [Description("PM异常申请")]
            PMRequest = 0,
            [Description("TL申请审核")]
            TLRequst = 1,
            [Description("TL已审核")]
            TLAudited = 2,
            [Description("PMD已审核")]
            PMDAudited = 3,
        }
        public static SortedList GetPOPayItemErrRequestStatus()
        {
            return GetStatus(typeof(POPayItemErrRequestStatus));
        }
        public static string GetPOPayItemErrRequestStatus(object v)
        {
            return GetDescription(typeof(POPayItemErrRequestStatus), v);
        }

        /// <summary>
        /// 负采购单类型(退货、调价)
        /// </summary>
        public enum MinusPOType
        {
            [Description("退货")]
            Return = 0,
            [Description("调价")]
            Rectify = 1,
        }
        public static SortedList GetMinusPOType()
        {
            return GetStatus(typeof(MinusPOType));
        }
        public static string GetMinusPOType(object v)
        {
            return GetDescription(typeof(MinusPOType), v);
        }


        //分站列表
        #region BranchID
        //===========================================
        public enum BranchID : int
        {
            [Description("总仓")]
            WH = 1,
            [Description("徐汇分站")]
            XH = 2,
            [Description("杨浦分站")]
            YP = 3,
            [Description("普陀分站")]
            PT = 4,
            [Description("浦东分站")]
            PD = 5,
            [Description("闵行分站")]
            MH = 6,
            [Description("苏州分站")]
            SZ = 7,
            [Description("扬州分站")]
            YZ = 8,
            [Description("杭州分站")]
            HZ = 9

        }
        public static SortedList GetBranchID()
        {
            return GetStatus(typeof(BranchID));
        }
        public static string GetBranchID(object v)
        {
            return GetDescription(typeof(BranchID), v);
        }
        //--------------------------------------------
        #endregion

        //配送列表状态 DLStatus
        #region DLStatus
        public enum DLStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("初始")]
            Origin = 0,
            [Description("发货确认")]
            StockConfirmed = 1,
            [Description("财务确认")]
            AccountConfirmed = 2,
            [Description("结算确认")]
            DSConfirmed = 3
        }
        public static SortedList GetDLStatus()
        {
            return GetStatus(typeof(DLStatus));
        }
        public static string GetDLStatus(object v)
        {
            return GetDescription(typeof(DLStatus), v);
        }
        #endregion


        //配送列表单据类型
        #region DLItemType
        public enum DLItemType
        {
            [Description("销售单")]
            SaleOrder = 1,
            [Description("RMA取件单")]
            RMARequest = 2,
            [Description("RMA发货单")]
            RMARevert = 3,
            [Description("RMA送修单")]
            RMAOutbound = 4,
            [Description("RMA补发附件单")]
            RMASendAccessory = 5,
            [Description("移库单")]
            StShift = 6,
        }
        public static SortedList GetDLItemType()
        {
            return GetStatus(typeof(DLItemType));
        }
        public static string GetDLItemType(object v)
        {
            return GetDescription(typeof(DLItemType), v);
        }
        #endregion

        //配送结算单状态 DSStatus

        #region DSStatus
        public enum DSStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("初始")]
            Origin = 0,
            [Description("财务确认")]
            AccountConfirmed = 1,
            [Description("分站审核")]
            Audited = 2,
            [Description("财务审核")]
            AccountAudited = 3,
        }
        public static SortedList GetDSStatus()
        {
            return GetStatus(typeof(DSStatus));
        }
        public static string GetDSStatus(object v)
        {
            return GetDescription(typeof(DSStatus), v);
        }
        #endregion

        #region DeliveryManArrearageLogType
        public enum DeliveryManArrearageLogType
        {
            [Description("货款多交")]
            PayMentofGoodsExcessive = 1,
            [Description("货款少交")]
            PayMentOfGoodsLack = 2,
        }
        public static SortedList GetDeliveryManArrearageLogType()
        {
            return GetStatus(typeof(DeliveryManArrearageLogType));
        }
        public static string GetDeliveryManArrearageLogType(object v)
        {
            return GetDescription(typeof(DeliveryManArrearageLogType), v);
        }
        #endregion

        //订单异常处理状态
        #region AbnormalSOStatus
        public enum AbnormalSOStatus : int
        {
            [Description("处理中")]
            Handling = 1,
            [Description("完毕")]
            Closed = 2

        }
        public static SortedList GetAbnormalSOStatus()
        {
            return GetStatus(typeof(AbnormalSOStatus));
        }
        public static string GetAbnormalSOStatus(object v)
        {
            return GetDescription(typeof(AbnormalSOStatus), v);
        }
        #endregion

        /// <summary>
        /// 订单尺寸类型
        /// </summary>
        #region SizeType
        public enum SizeType
        {
            [Description("小件")]
            Small = 1,
            [Description("中件")]
            Middle = 2,
            [Description("大件")]
            Large = 3,
        }
        public static SortedList GetSizeType()
        {
            return GetStatus(typeof(SizeType));
        }
        public static string GetSizeType(object v)
        {
            return GetDescription(typeof(SizeType), v);
        }
        #endregion


        /// <summary>
        /// 划账类型
        /// </summary>
        #region RemittanceType
        public enum RemittanceType
        {
            [Description("金卡")]
            GoldCard = 0,
            [Description("淘宝")]
            TaoBao = 1,
            [Description("其它")]
            Other = 9,
        }
        public static SortedList GetRemittanceType()
        {
            return GetStatus(typeof(RemittanceType));
        }
        public static string GetRemittanceType(object v)
        {
            return GetDescription(typeof(RemittanceType), v);
        }
        #endregion

        //InvoiceType订单开票类型 
        #region InvoiceType
        public enum InvoiceType
        {
            [Description("增值税专用发票")]
            SpecialVATInvoice = 1,
            [Description("增值税普通发票")]
            GeneralVATInvoice = 2,
            [Description("商业零售发票")]
            RetailCommercialInvoice = 3

        }
        public static SortedList GetInvoiceType()
        {
            return GetStatus(typeof(InvoiceType));
        }
        public static string GetInvoiceType(object v)
        {
            return GetDescription(typeof(InvoiceType), v);
        }
        #endregion

        //SOInvoiceStatus 订单发票状态
        #region SOInvoiceStatus
        public enum SOInvoiceStatus
        {
            [Description("发票已开")]
            InvoiceComplete = 1,
            [Description("发票未开")]
            InvoiceAbsent = 2,
            [Description("发票作废")]
            InvoiceAbandon = 3
        }
        public static SortedList GetSOInvoiceStatus()
        {
            return GetStatus(typeof(SOInvoiceStatus));
        }
        public static string GetSOInvoiceStatus(object v)
        {
            return GetDescription(typeof(SOInvoiceStatus), v);
        }
        #endregion

        //SOAuditTimeSpan审单员工作时段
        #region SOAuditTimeSpan
        public enum SOAuditTimeSpan
        {
            [Description("白班")]
            Day = 1,
            [Description("晚班")]
            Night = 2

        }
        public static SortedList GetSOAuditTimeSpan()
        {
            return GetStatus(typeof(SOAuditTimeSpan));
        }
        public static string GetSOAuditTimeSpan(object v)
        {
            return GetDescription(typeof(SOAuditTimeSpan), v);
        }
        #endregion

        //WorkTimeSpan工作时段 
        #region WorkTimeSpan
        public enum WorkTimeSpan
        {
            [Description("白班")]
            Day = 1,
            [Description("晚班")]
            Night = 2

        }
        public static SortedList GetWorkTimeSpan()
        {
            return GetStatus(typeof(WorkTimeSpan));
        }
        public static string GetWorkTimeSpan(object v)
        {
            return GetDescription(typeof(WorkTimeSpan), v);
        }
        #endregion

        //退货单状态
        #region SRStatus
        public enum SRStatus : int
        {
            [Description("作废")]
            Abandon = 0,
            [Description("初始")]
            Origin = 1,
            [Description("待入库")]
            WaitingInStock = 2,
            [Description("待上架")]
            WaitingShelve = 3,
            [Description("已上架")]
            Shelved = 4
        }
        public static SortedList GetSRStatus()
        {
            return GetStatus(typeof(SRStatus));
        }
        public static string GetSRStatus(object v)
        {
            return GetDescription(typeof(SRStatus), v);
        }
        #endregion

        //退货类型
        #region SRReturnType
        public enum SRReturnType : int
        {
            [Description("部分退货")]
            PartlyReturn = 0,
            [Description("全部退货")]
            Return = 1

        }
        public static SortedList GetSRReturnType()
        {
            return GetStatus(typeof(SRReturnType));
        }
        public static string GetSRReturnType(object v)
        {
            return GetDescription(typeof(SRReturnType), v);
        }
        #endregion

        //二值状态，图片显示
        //请登记使用者 user, role, category1/2/3, categoryattribute, manufacturer，
        #region BiStatusImage 0, -1
        //===========================================
        public enum BiStatusImage : int
        {
            [Description("<img src='../images/opt/valid.gif' border='0' alt='有效' />")]
            Valid = 0,
            [Description("<img src='../images/opt/invalid.gif' border='0' alt='无效'/>")]
            InValid = -1

        }
        public static SortedList GetBiStatusImage()
        {
            return GetStatus(typeof(BiStatusImage));
        }
        public static string GetBiStatusImage(object v)
        {
            return GetDescription(typeof(BiStatusImage), v);
        }
        //--------------------------------------------
        #endregion

        /// <summary>
        /// 客户增票信息状态
        /// </summary>
        #region CustomerVATInfoStatus
        public enum CustomerVATInfoStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("初始")]
            Origin = 0,
            [Description("已审核")]
            Audited = 1,
        }
        public static SortedList GetCustomerVATInfoStatus()
        {
            return GetStatus(typeof(CustomerVATInfoStatus));
        }
        public static string GetCustomerVATInfoStatus(object v)
        {
            return GetDescription(typeof(CustomerVATInfoStatus), v);
        }
        #endregion

        //RMAHandoverStatus 类型
        public enum RMAHandoverStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("待交接")]
            WaitingHandover = 0,
            [Description("已出库")]
            OutStock = 1,
            [Description("部分接收")]
            PartlyReceived = 2,
            [Description("全部接收")]
            FullReceived = 3,
        }

        public static SortedList GetRMAHandoverStatus()
        {
            return GetStatus(typeof(RMAHandoverStatus));
        }

        public static string GetRMAHandoverStatus(object v)
        {
            return GetDescription(typeof(RMAHandoverStatus), v);
        }

        /// <summary>
        /// PM所属分组（暂且设置）
        /// </summary>
        public enum PMGroup
        {
            [Description("暂未设置")]
            None = 0,
            [Description("核心配件")]
            CoreFittings = 1,
            [Description("外设")]
            OuterFittings = 2,
            [Description("消费电子")]
            ConsumeElectron = 3,
            [Description("笔记本")]
            NoteBook = 4,
            [Description("鞋类")]
            Shoes = 5,

        }
        public static SortedList GetPMGroup()
        {
            return GetStatus(typeof(PMGroup));
        }
        public static string GetPMGroup(object v)
        {
            return GetDescription(typeof(PMGroup), v);
        }

        /// <summary>
        /// 岗位标识
        /// </summary>
        public enum UserFlag
        {
            [Description("普通")]
            Normal = 0,
            [Description("采购员")]
            PM = 1,
            [Description("采购组长")]
            TL = 2,
            [Description("采购总监")]
            PMD = 3,
            [Description("配送员")]
            Sender = 4,

        }
        public static SortedList GetUserFlag()
        {
            return GetStatus(typeof(UserFlag));
        }
        public static string GetUserFlag(object v)
        {
            return GetDescription(typeof(UserFlag), v);
        }

        /// <summary>
        /// 优惠劵类型
        /// </summary>
        public enum CouponType
        {
            [Description("整网抵扣劵")]
            ALL = 1,
            [Description("类别抵扣劵")]
            Category = 2,
            [Description("商品抵扣劵")]
            Product = 3,
            [Description("品牌抵扣劵")]
            Manufactory = 4,
        }
        public static SortedList GetCouponType()
        {
            return GetStatus(typeof(CouponType));
        }
        public static string GetCouponType(object v)
        {
            return GetDescription(typeof(CouponType), v);
        }

        /// <summary>
        /// 优惠劵状态
        /// </summary>
        public enum CouponStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("初始")]
            Origin = 0,
            [Description("已激活")]
            Activation = 1,
            [Description("已部分使用")]
            PartlyUsed = 2,
            [Description("已全部使用")]
            FullUsed = 3,
        }
        public static SortedList GetCouponStatus()
        {
            return GetStatus(typeof(CouponStatus));
        }
        public static string GetCouponStatus(object v)
        {
            return GetDescription(typeof(CouponStatus), v);
        }

        /// <summary>
        /// 优惠劵申请记录状态
        /// </summary>
        public enum CouponRequestStatus
        {
            [Description("作废")]
            Abandon = -1,
            [Description("初始")]
            Origin = 0,
            [Description("审核")]
            Audited = 1,
        }
        public static SortedList GetCouponRequestStatus()
        {
            return GetStatus(typeof(CouponRequestStatus));
        }
        public static string GetCouponRequestStatus(object v)
        {
            return GetDescription(typeof(CouponRequestStatus), v);
        }

    }
}