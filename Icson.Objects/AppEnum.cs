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

        #region ���ߺ���

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
        /// ����ָ��ö�����͵�ָ��ֵ������
        /// </summary>
        /// <param name="t">ö������</param>
        /// <param name="v">ö��ֵ</param>
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

        //��ֵ״̬
        //��Ǽ�ʹ���� user, role, category1/2/3, categoryattribute, manufacturer��
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
        //DBC�޸ĳ�����������
        public enum BiStatus_DBC : int
        {
            [Description("��Ч")]
            Valid = 0,
            [Description("��Ч")]
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
            [Description("��")]
            Male = 1,
            [Description("Ů")]
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

        //����
        #region ����ԭ������
        //===========================================
        public enum PointLogType : int
        {
            [Description("Emailȷ��")]
            Recruit = 1,	//�¿ͻ�����+
            [Description("�Ͽͻ�����")]
            Veteran = 2,	//�Ͽͻ�������ʷ�ظ�
            [Description("���ɶ���")]
            CreateOrder = 3,	//�¶���-
            [Description("���϶���")]
            AbandonSO = 5,	//���϶���
            [Description("���϶���ȡ��")]
            CancelAbandonSO = 6,	//�������ȡ��-
            [Description("�˻�")]
            ReturnProduct = 7,	//�˻�-
            [Description("ȡ���˻�")]
            CancelReturn = 8,
            [Description("ȡ������")]
            CancelOutstock = 9,		//ȡ������
            [Description("����ת��")]
            TransferPoint = 10,   //����ת��
            [Description("����÷�")]
            AddPointLater = 11,		//�ͺ�ӷ�
            [Description("�����޸�")]
            UpdateSO = 12,		//�޸�SaleOrder
            [Description("�����۳�")]
            WholeSale = 13,		//��������-
            [Description("��")]
            InfoProduct = 14,		//�򿨼���-
            [Description("����")]
            BizRequest = 15,		//Request
            [Description("�ͼ۾ٱ�����")]
            award = 16,      //�ͼ۾ٱ�����
            [Description("�ƺ��ͻ���")]
            forHF = 17,       //�ƺ��ͻ��� 
            [Description("PCHome�ͻ���")]
            forPCHome = 18,       //PCHome�ͻ��� 
            [Description("99bill�ͻ���")]
            for99bill = 19,       //99bill�ͻ��� 
            [Description("5460�ͻ���")]
            for5460 = 20,       //5460�ͻ��� 
            [Description("�μӿͷ��ʾ����")]
            CSSurvey = 21,
            [Description("���ʲ���")]
            PostageMakeup = 22,
            [Description("���۲���")]
            PriceMakeup = 23,
            [Description("�������ڲ���")]
            DeliveryDelayMakeup = 24,
            [Description("�������ڲ���")]
            PurchaseDelayMakeup = 25,
            [Description("�˿�Ͷ�߲���")]
            CustomerComplainMakeup = 26,
            [Description("���ϴ��󲹳�")]
            ProductInfoErrorMakeup = 27,
            [Description("��Ʒ���ⲹ��")]
            ProductDefectMakeup = 28,
            [Description("�˿͹�����Ʒ�ึ��")]
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

        //���ֵ�����Դ����
        #region ���ֵ�����Դ����
        //===========================================
        public enum PointSourceType : int
        {
            [Description("�ۺ�")]  //��SOSysNo
            RMA = 1,
            [Description("�۸�ٱ�")]
            PriceReport = 2,
            [Description("�ͻ�����")]
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

        //��ֵ״̬
        //��Ǽ�ʹ����
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

        #region ��������
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

        //������־����
        #region Log Type
        //===========================================
        public enum LogType : int
        {
            //xx_xx_xx
            //ģ��_��ģ��_����
            //------------------------------------------------------------------------------------sys 10
            //		user 10, add 10, update 11.
            [Description("�����û�")]
            Sys_User_Add = 101010,
            [Description("�����û�")]
            Sys_User_Update = 101011,
            [Description("�û���¼")]
            Sys_User_Login = 101012,
            //		role 11, add 10, update 11
            [Description("���ӽ�ɫ")]
            Sys_Role_Add = 101110,
            [Description("���½�ɫ")]
            Sys_Role_Update = 101111,
            //		user&role 12, add 10, delete 12
            [Description("���ӽ�ɫ")]
            Sys_UserRole_Add = 101210,
            [Description("���½�ɫ")]
            Sys_UserRole_Delete = 101212,
            //		role&privilege 13 add 10, delete 12
            [Description("���ӽ�ɫ")]
            Sys_RolePrivilege_Add = 101310,
            [Description("���½�ɫ")]
            Sys_RolePrivilege_Delete = 101312,


            //------------------------------------------------------------------------------------basic 11
            //		category1 13, 
            //						add 10, update 11.
            [Description("���Ӵ���")]
            Basic_Category1_Add = 111310,
            [Description("���´���")]
            Basic_Category1_Update = 111311,
            //		category2 14, 
            //						add 10, update 11.
            [Description("��������")]
            Basic_Category2_Add = 111410,
            [Description("��������")]
            Basic_Category2_Update = 111411,
            //		category3 15, 
            //						add 10, update 11.
            [Description("����С��")]
            Basic_Category3_Add = 111510,
            [Description("����С��")]
            Basic_Category3_Update = 111511,
            [Description("����С��������")]
            Basic_Category3ReviewItem_Add = 111512,
            [Description("����С��������")]
            Basic_Category3ReviewItem_Update = 111513,

            //		categoryAttribute 16, 
            //						init 10, update 11, top 12, up 13, down 14, bottom 15, insert 16
            [Description("��ʼ��С������")]
            Basic_CategoryAttribute_Init = 111610,
            [Description("�޸�С������")]
            Basic_CategoryAttribute_Update = 111611,
            [Description("�ƶ�������")]
            Basic_CategoryAttribute_Top = 111612,
            [Description("�ƶ�����һ��")]
            Basic_CategoryAttribute_Up = 111613,
            [Description("�ƶ�����һ��")]
            Basic_CategoryAttribute_Down = 111614,
            [Description("�ƶ����Ͷ�")]
            Basic_CategoryAttribute_Bottom = 111615,
            [Description("�ƶ����Ͷ�")]
            Basic_CategoryAttribute_Insert = 111616,


            //		categoryAttributeOption 21, 
            //						init 21,insert 22 update 23, top 24, up 25, down 26, bottom 27
            [Description("��ʼ��С������ѡ��")]
            Basic_CategoryAttributeOption_Init = 111621,
            [Description("����С������ѡ��")]
            Basic_CategoryAttributeOption_Insert = 111622,
            [Description("�޸�С������ѡ��")]
            Basic_CategoryAttributeOption_Update = 111623,
            [Description("����ѡ���ƶ�������")]
            Basic_CategoryAttributeOption_Top = 111624,
            [Description("����ѡ���ƶ�����һ��")]
            Basic_CategoryAttributeOption_Up = 111625,
            [Description("����ѡ���ƶ�����һ��")]
            Basic_CategoryAttributeOption_Down = 111626,
            [Description("����ѡ���ƶ����Ͷ�")]
            Basic_CategoryAttributeOption_Bottom = 111627,

            //		manufacturer  17, 
            //						add 10, update 11
            [Description("����������")]
            Basic_Manufacturer_Add = 111710,
            [Description("����������")]
            Basic_Manufacturer_Update = 111711,
            //		manufacturer  18, 
            //						add 10, update 11
            [Description("���ӹ�Ӧ��")]
            Basic_Vendor_Add = 111810,
            [Description("���¹�Ӧ��")]
            Basic_Vendor_Update = 111811,

            //		stock  19, add 10, update 11
            [Description("���Ӳֿ�")]
            Basic_Stock_Add = 111910,
            [Description("���²ֿ�")]
            Basic_Stock_Update = 111911,

            //		currency  20, add 10, update 11
            [Description("���ӻ���")]
            Basic_Currency_Add = 112010,
            [Description("���»���")]
            Basic_Currency_Update = 112011,

            //		customer 30, add 10, update 11, invalid 12
            [Description("���ӿͻ�")]
            Basic_Customer_Add = 113001,
            [Description("�޸Ŀͻ�")]
            Basic_Customer_Update = 113011,
            [Description("���Ͽͻ�")]
            Basic_Customer_Invalid = 113012,



            //		ASP  60, 
            //			insert area 10, update area 11
            //			insert pay type 12, update pay type 13
            //			insert ship type 14, update ship type 15
            //			insert ship ! area 16, delete 17
            //			insert ship ! pay 18, delete 19
            //			insert ship area price 20, delete 21
            [Description("���ӵ���")]
            Basic_Area_Add = 116010,
            [Description("���µ���")]
            Basic_Area_Update = 116011,
            [Description("����֧����ʽ")]
            Basic_PayType_Add = 116012,
            [Description("����֧����ʽ")]
            Basic_PayType_Update = 116013,
            [Description("�����ͻ���ʽ")]
            Basic_ShipType_Add = 116014,
            [Description("�����ͻ���ʽ")]
            Basic_ShipType_Update = 116015,
            [Description("����ͻ���ʽ�͵����ķ��ϵ")]
            Basic_ShipAreaUn_Add = 116016,
            [Description("ɾ���ͻ���ʽ�͵����ķ��ϵ")]
            Basic_ShipAreaUn_Delete = 116017,
            [Description("����ͻ���ʽ��֧����ʽ�ķ��ϵ")]
            Basic_ShipPayUn_Add = 116018,
            [Description("ɾ���ͻ���ʽ��֧����ʽ�ķ��ϵ")]
            Basic_ShipPayUn_Delete = 116019,
            [Description("����ͻ���ʽ�͵����ļ۸�")]
            Basic_ShipAreaPrice_Add = 116020,
            [Description("ɾ���ͻ���ʽ�͵����ļ۸�")]
            Basic_ShipAreaPrice_Delete = 116021,

            //		Product 30
            [Description("������Ʒ")]
            Basic_Product_Add = 113001,
            [Description("������Ʒ������Ϣ")]
            Basic_Product_Basic_Update = 113002,
            [Description("������Ʒ�۸�")]
            Basic_Product_Price_Update = 113003,
            [Description("������ƷPM")]
            Basic_Product_PM_Update = 113004,
            [Description("������Ʒ����")]
            Basic_Product_Attribute_Update = 113005,
            [Description("������Ʒ����")]
            Basic_Product_Weight_Update = 113006,
            [Description("������ƷͼƬ")]
            Basic_Product_Pic_Update = 113007,
            [Description("�����Ʒ�г��ͼ���Ϣ")]
            Basic_Product_PriceMarket_Add = 113008,
            [Description("������ƷԤ��")]
            Basic_Product_Preview_Update = 113009,

            //------------------------------------------------------------------------------------stock sheet 50
            //		lend 11, 
            //						
            [Description("�������������")]
            St_Lend_Master_Insert = 501110,
            [Description("����������޸�")]
            St_Lend_Master_Update = 501111,
            [Description("�������ϸ����")]
            St_Lend_Item_Insert = 501112,
            [Description("�������ϸɾ��")]
            St_Lend_Item_Delete = 501113,
            [Description("�������ϸ�޸�")]
            St_Lend_Item_Update = 501114,

            [Description("���������")]
            St_Lend_Abandon = 501115,
            [Description("�����ȡ������")]
            St_Lend_CancelAbandon = 501116,

            [Description("��������")]
            St_Lend_Verify = 501117,
            [Description("�����ȡ�����")]
            St_Lend_CancelVerify = 501118,

            [Description("���������")]
            St_Lend_OutStock = 501119,
            [Description("�����ȡ������")]
            St_Lend_CancelOutStock = 501120,

            [Description("�������������")]
            St_Lend_Return_Insert = 501121,
            [Description("���������ɾ��")]
            St_Lend_Return_Delete = 501122,

            //		adjust 12, 
            //						
            [Description("���浥��������")]
            St_Adjust_Master_Insert = 501210,
            [Description("���浥�����޸�")]
            St_Adjust_Master_Update = 501211,
            [Description("���浥��ϸ����")]
            St_Adjust_Item_Insert = 501212,
            [Description("���浥��ϸɾ��")]
            St_Adjust_Item_Delete = 501213,
            [Description("���浥��ϸ�޸�")]
            St_Adjust_Item_Update = 501214,

            [Description("���浥����")]
            St_Adjust_Abandon = 501215,
            [Description("���浥ȡ������")]
            St_Adjust_CancelAbandon = 501216,

            [Description("���浥���")]
            St_Adjust_Verify = 501217,
            [Description("���浥ȡ�����")]
            St_Adjust_CancelVerify = 501218,

            [Description("���浥����")]
            St_Adjust_OutStock = 501219,
            [Description("���浥ȡ������")]
            St_Adjust_CancelOutStock = 501220,

            //		shift 13, 
            //						
            [Description("�ƿⵥ��������")]
            St_Shift_Master_Insert = 501310,
            [Description("�ƿⵥ�����޸�")]
            St_Shift_Master_Update = 501311,
            [Description("�ƿⵥ��ϸ����")]
            St_Shift_Item_Insert = 501312,
            [Description("�ƿⵥ��ϸɾ��")]
            St_Shift_Item_Delete = 501313,
            [Description("�ƿⵥ��ϸ�޸�")]
            St_Shift_Item_Update = 501314,

            [Description("�ƿⵥ����")]
            St_Shift_Abandon = 501315,
            [Description("�ƿⵥȡ������")]
            St_Shift_CancelAbandon = 501316,

            [Description("�ƿⵥ���")]
            St_Shift_Verify = 501317,
            [Description("�ƿⵥȡ�����")]
            St_Shift_CancelVerify = 501318,

            [Description("�ƿⵥ����")]
            St_Shift_OutStock = 501319,
            [Description("�ƿⵥȡ������")]
            St_Shift_CancelOutStock = 501320,

            [Description("�ƿⵥ���")]
            St_Shift_InStock = 501321,
            [Description("�ƿⵥȡ�����")]
            St_Shift_CancelInStock = 501322,

            //		transfer 14, 
            //						
            [Description("ת������������")]
            St_Transfer_Master_Insert = 501410,
            [Description("ת���������޸�")]
            St_Transfer_Master_Update = 501411,
            [Description("ת������ϸ����")]
            St_Transfer_Item_Insert = 501412,
            [Description("ת������ϸɾ��")]
            St_Transfer_Item_Delete = 50143,
            [Description("ת������ϸ�޸�")]
            St_Transfer_Item_Update = 501414,

            [Description("ת��������")]
            St_Transfer_Abandon = 501415,
            [Description("ת����ȡ������")]
            St_Transfer_CancelAbandon = 501416,

            [Description("ת�������")]
            St_Transfer_Verify = 501417,
            [Description("ת����ȡ�����")]
            St_Transfer_CancelVerify = 501418,

            [Description("ת��������")]
            St_Transfer_OutStock = 501419,
            [Description("ת����ȡ������")]
            St_Transfer_CancelOutStock = 501420,

            //		virtual 15, 
            //						
            [Description("������")]
            St_Virtual_Insert = 501510,

            //		Inventory position
            //
            [Description("���ÿ�λ")]
            St_Inventory_SetPos = 501610,

            //Purchase 20-----------------------------------------------------------
            //	Basket  10

            [Description("�ɹ�������")]
            Purchase_Basket_Insert = 201010,
            [Description("�ɹ�������")]
            Purchase_Basket_Update = 201011,
            [Description("�ɹ���ɾ��")]
            Purchase_Basket_Delete = 201012,

            //     PO	11
            [Description("���ɲɹ���")]
            Purchase_Create = 201110,
            [Description("�ɹ��������޸�")]
            Purchase_Master_Update = 201111,
            [Description("�ɹ�����ϸ���")]
            Purchase_Item_Insert = 201112,
            [Description("�ɹ�����ϸ�޸�")]
            Purchase_Item_Update = 201113,
            [Description("�ɹ�����ϸɾ��")]
            Purchase_Item_Delete = 201114,
            [Description("�ɹ�����˵�̯��")]
            Purchase_Verify_Apportion = 201114,
            [Description("�ɹ�����˵����")]
            Purchase_Verify_InStock = 201116,
            [Description("�ɹ���ȡ�����")]
            Purchase_CancelVerify = 201117,
            [Description("�ɹ������")]
            Purchase_InStock = 201118,
            [Description("�ɹ���ȡ�����")]
            Purchase_CancelInStock = 201119,
            [Description("�ɹ�������")]
            Purchase_Abandon = 201120,
            [Description("�ɹ���ȡ������")]
            Purchase_CancelAbandon = 201121,

            [Description("�ɹ���̯���������")]
            Purchase_ApportionMaster_Add = 201122,
            [Description("�ɹ���̯������ɾ��")]
            Purchase_ApportionMaster_Delete = 201123,
            [Description("�ɹ���̯����ϸ���")]
            Purchase_ApportionItem_Add = 201124,
            [Description("�ɹ���̯����ϸɾ��")]
            Purchase_ApportionItem_Delete = 201125,
            [Description("�ɹ���̯������")]
            Purchase_Apportion_Export = 201126,
            [Description("PM�ɹ�����������")]
            Purchase_PMPOAmtRestrict_Add = 201127,
            [Description("PM�ɹ���������޸�")]
            Purchase_PMPOAmtRestrict_Update = 201128,


            //Finance 30
            //				po 11
            [Description("����ɹ�������")]
            Finance_POPay_Item_Add = 301110,
            [Description("����ɹ�����޸�")]
            Finance_POPay_Item_Update = 301111,
            [Description("����ɹ��������")]
            Finance_POPay_Item_Abandon = 301112,
            [Description("����ɹ����ȡ������")]
            Finance_POPay_Item_CancelAbandon = 301113,
            [Description("����ɹ����֧��")]
            Finance_POPay_Item_Pay = 301114,
            [Description("����ɹ����ȡ��֧��")]
            Finance_POPay_Item_CancelPay = 301115,

            [Description("����ɹ�Ӧ�ո���")]
            Finance_POPay_Update = 301116,

            [Description("����ɹ�������")]
            Finance_POPay_Item_Audit = 301117,
            [Description("����ɹ����ȡ�����")]
            Finance_POPay_Item_CancelAudit = 301118,
            [Description("����ɹ��������")]
            Finance_POPay_Item_Request = 301119,


            //				so income 12
            [Description("���������տ���")]
            Finance_SOIncome_Add = 301201,
            [Description("���������տ����")]
            Finance_SOIncome_Abandon = 301202,
            [Description("���������տȷ��")]
            Finance_SOIncome_Confirm = 301203,
            [Description("���������տȡ��ȷ��")]
            Finance_SOIncome_UnConfirm = 301204,
            [Description("����ƾ֤¼��")]
            Finance_SOIncome_Voucher_Add = 301205,

            //				netpay 13
            [Description("����NetPay Add&Verify")]
            Finance_NetPay_AddVerified = 301310,
            [Description("����NetPay Verify")]
            Finance_NetPay_Verify = 301311,
            [Description("����NetPay Abandon")]
            Finance_NetPay_Abandon = 301312,


            //Sale 60
            //              so 06
            [Description("���۵�����")]
            Sale_SO_Create = 600601,
            [Description("���۵����")]
            Sale_SO_Audit = 600602,
            [Description("���۵�ȡ�����")]
            Sale_SO_CancelAudit = 600603,
            [Description("���۵��������")]
            Sale_SO_ManagerAudit = 600604,
            [Description("���۵��ͻ�����")]
            Sale_SO_CustomerAbandon = 600605,
            [Description("���۵�Ա������")]
            Sale_SO_EmployeeAbandon = 600606,
            [Description("���۵���������")]
            Sale_SO_ManagerAbandon = 600607,
            [Description("���۵�ȡ������")]
            Sale_SO_CancelAbandon = 600608,
            [Description("���۵�����")]
            Sale_SO_OutStock = 600609,
            [Description("���۵�ȡ������")]
            Sale_SO_CancelOutStock = 600610,
            [Description("���۵���Ʊ��ӡ")]
            Sale_SO_PrintInvoice = 600611,
            [Description("���۵��޸�")]
            Sale_SO_Update = 600612,
            [Description("���۵����߷�Ʊ")]
            Sale_SO_Invoice = 600613,
            [Description("���۵����Ϸ�Ʊ")]
            Sale_SO_AbandonInvoice = 600614,
            [Description("���۵����ķ�Ʊ����")]
            Sale_SO_UpdateInvoiceType = 600615,

            //				rma 08
            [Description("RMA������")]
            Sale_RMA_Create = 600801,
            [Description("RMA������")]
            Sale_RMA_Abandon = 600802,
            [Description("RMA�����")]
            Sale_RMA_Audit = 600803,
            [Description("RMA��ȡ�����")]
            Sale_RMA_CancelAudit = 600804,
            [Description("RMA��������Ʒ")]
            Sale_RMA_Receive = 600805,
            [Description("RMA��ȡ������")]
            Sale_RMA_CancelReceive = 600806,
            [Description("RMA������")]
            Sale_RMA_Handle = 600807,
            [Description("RMA��ȡ������")]
            Sale_RMA_CancelHandle = 600808,
            [Description("RMA���᰸")]
            Sale_RMA_Close = 600809,
            [Description("RMA���ؿ�")]
            Sale_RMA_Reopen = 600810,

            //				ro 09
            [Description("�˻�������")]
            Sale_RO_Create = 600901,
            [Description("�˻�������")]
            Sale_RO_Abandon = 600902,
            [Description("�˻������")]
            Sale_RO_Audit = 600903,
            [Description("�˻���ȡ�����")]
            Sale_RO_CancelAudit = 600904,
            [Description("�˻����˻�")]
            Sale_RO_Return = 600905,
            [Description("�˻���ȡ���˻�")]
            Sale_RO_CancelReturn = 600906,
            [Description("�˻�����Ʊ��ӡ")]
            Sale_RO_PrintInvoice = 600907,

            //RMA new Version
            //70
            //rma requeset 10
            [Description("RMA������")]
            Sale_RMA_Create2 = 700801,
            [Description("RMA������")]
            Sale_RMA_Abandon2 = 700802,
            [Description("RMA�����")]
            Sale_RMA_Audit2 = 700803,

            //				rma outbound 20
            [Description("RMA-����-����")]
            RMA_OutBound_Create = 702001,
            [Description("RMA-����-�޸�")]
            RMA_OutBound_Update = 702002,
            [Description("RMA-����-����")]
            RMA_OutBound_OutStock = 702003,
            [Description("RMA-����-ȡ������")]
            RMA_OutBound_CancelOutStock = 702004,
            [Description("RMA-����-ɾ����ϸ")]
            RMA_OutBound_DeleteItem = 702005,
            [Description("RMA-����-ɾ����ϸ")]
            RMA_OutBound_InsertItem = 702006,
            [Description("RMA-����-ɾ������")]
            RMA_OutBound_Abandon = 702007,
            [Description("RMA-����-ɾ������")]
            RMA_OutBound_UpdateDunDesc = 702008,

            //				rma register 30
            [Description("RMA-�Ǽ�-����Check")]
            RMA_Register_Check = 703001,
            [Description("RMA-�Ǽ�-����Memo")]
            RMA_Register_Memo = 703002,
            [Description("RMA-�Ǽ�-����Outbound")]
            RMA_Register_Outbound = 703003,
            [Description("RMA-�Ǽ�-����Revert")]
            RMA_Register_Revert = 703004,
            [Description("RMA-�Ǽ�-����Revert")]
            RMA_Register_Refund = 703005,
            [Description("RMA-�Ǽ�-����Return")]
            RMA_Register_Return = 703006,
            [Description("RMA-�Ǽ�-����Close")]
            RMA_Register_Close = 703007,
            [Description("RMA-�Ǽ�-SetToCC")]
            RMA_Register_ToCC = 703008,
            [Description("RMA-�Ǽ�-SetToRMA")]
            RMA_Register_ToRMA = 703009,
            [Description("RMA_�Ǽ�_���Revert")]
            RMA_Register_RevertAudit = 703010,

            //           rma revert 40
            [Description("RMA-�ͻ�-����")]
            RMA_Revert_Create = 704001,
            [Description("RMA-�ͻ�-�޸�")]
            RMA_Revert_Update = 704002,
            [Description("RMA-�ͻ�-����")]
            RMA_Revert_Abandon = 704003,
            [Description("RMA-�ͻ�-ȡ������")]
            RMA_Revert_CancelAbandon = 704006,
            [Description("RMA-�ͻ�-����")]
            RMA_Revert_Out = 704004,
            [Description("RMA-�ͻ�-ȡ������")]
            RMA_Revert_CancelOut = 704005,


            //           rma refund 50 
            [Description("RMA-�˻�-����")]
            RMA_Refund_Create = 705001,
            [Description("RMA-�˻�-�޸�")]
            RMA_Refund_Upate = 705002,
            [Description("RMA-�˻�-����")]
            RMA_Refund_Abandon = 705003,
            [Description("RMA-�˻�-���")]
            RMA_Refund_Audit = 705004,
            [Description("RMA-�˻�-ȡ�����")]
            RMA_Refund_CancelAudit = 70505,
            [Description("RMA-�˻�-�˿�")]
            RMA_Refund_Refund = 705006,
            [Description("RMA-�˻�-ȡ���˿�")]
            RMA_Refund_CancelRefund = 705007,

            //			rma return 60
            [Description("RMA-�˻�����-����")]
            RMA_Return_Create = 706001,
            [Description("RMA-�˻����-�޸�")]
            RMA_Return_Update = 706002,
            [Description("RMA-�˻����-����")]
            RMA_Return_Abandon = 706003,
            [Description("RMA-�˻����-���")]
            RMA_Return_Return = 706004,
            [Description("RMA-�˻����-ȡ�����")]
            RMA_Return_CancelReturn = 706005,
            [Description("RMA-�˻����-���")]
            RMA_Return_Audit = 706006,

            //			rma_request 70
            [Description("RMA-���뵥-����")]
            RMA_Request_Create = 707001,
            [Description("RMA-���뵥-�޸�")]
            RMA_Request_Update = 707002,
            [Description("RMA-���뵥-�ջ�")]
            RMA_Request_Receive = 707003,
            [Description("RMA-���뵥-ȡ���ջ�")]
            RMA_Request_CancelReceive = 707004,
            [Description("RMA-���뵥-����")]
            RMA_Request_Abandon = 707005,
            [Description("RMA-���뵥-�ر�")]
            RMA_Request_Close = 707006,
            [Description("RMA-���뵥-�ظ�����")]
            RMA_Request_ReCreate = 707007,

            //			rma_sendAccessory 80
            [Description("RMA-����������-����")]
            RMA_SendAccessory_Create = 708001,
            [Description("RMA-����������-�޸�����Ϣ")]
            RMA_SendAccessory_UpdateMaster = 708002,
            [Description("RMA-����������-�޸���Ʒ��Ϣ")]
            RMA_SendAccessory_UpdateItem = 708003,
            [Description("RMA-����������-����")]
            RMA_SendAccessory_Abandon = 708004,
            [Description("RMA-����������-ȡ������")]
            RMA_SendAccessory_CancelAbandon = 708005,
            [Description("RMA-����������-���")]
            RMA_SendAccessoryt_Audit = 708006,
            [Description("RMA-����������-ȡ�����")]
            RMA_SendAccessoryt_CancelAudit = 708007,
            [Description("RMA-����������-����")]
            RMA_SendAccessoryt_Send = 708008,
            [Description("RMA-����������-ȡ������")]
            RMA_SendAccessoryt_CancelSend = 708009,


            // 30
            [Description("RMA_�Ǽ�_�˿���Ϣ")]
            RMA_Register_IsRecommendRefund = 703011,

            //				rma handover 100
            [Description("RMA-����-����")]
            RMA_Handover_Create = 710001,
            [Description("RMA-����-�޸�")]
            RMA_Handover_Update = 710002,
            [Description("RMA-����-����")]
            RMA_Handover_OutStock = 710003,
            [Description("RMA-����-ȡ������")]
            RMA_Handover_CancelOutStock = 710004,
            [Description("RMA-����-ɾ����ϸ")]
            RMA_Handover_DeleteItem = 710005,
            [Description("RMA-����-������ϸ")]
            RMA_Handover_InsertItem = 710006,
            [Description("RMA-����-����")]
            RMA_Handover_Abandon = 710007,
            [Description("RMA-����-ȡ������")]
            RMA_Handover_CancelAbandon = 710008,
            [Description("RMA-����-����")]
            RMA_Handover_Receive = 710009,
            [Description("RMA-����-ȡ������")]
            RMA_Handover_CancelReceive = 710010,

            //			sale_Return 90
            [Description("����-�˻���-����")]
            sale_Return_Create = 709001,
            [Description("����-�˻���-���")]
            sale_Return_Audit = 709002,
            [Description("����-�˻���-ȡ�����")]
            sale_Return_CancelAudit = 709003,
            [Description("����-�˻���-�ջ�")]
            sale_Return_Receive = 709004,
            [Description("����-�˻���-ȡ���ջ�")]
            sale_Return_CancelReceive = 709005,
            [Description("����-�˻���-�ϼ�")]
            sale_Return_Shelve = 709006,
            [Description("����-�˻���-ȡ���ϼ�")]
            sale_Return_CancelShelve = 709007,
            [Description("����-�˻���-���")]
            sale_Return_Instock = 709008,
            [Description("����-�˻���-ȡ�����")]
            sale_Return_CancelInstock = 709009,
            [Description("����-�˻���-����")]
            sale_Return_Abandon = 709010,
            [Description("����-�˻���-ȡ������")]
            sale_Return_CancelAbandon = 709011,

            //	Delivery 80:dl,ds
            [Description("����-���͵�-���ö�ȷ���")]
            delivery_dl_CreditAllow = 801001,
            [Description("����-���͵�-����")]
            delivery_dl_Abandon = 801002,

            //Online 40
            //
            [Description("���¹���")]
            Online_Bulletin_Update = 401001,
            [Description("�����Ʒ�б�")]
            Online_List_Insert = 401002,
            [Description("ɾ����Ʒ�б�")]
            Online_List_Delete = 401003,

            [Description("����ͶƱ����")]
            Online_Poll_Insert = 401004,
            [Description("����ͶƱ����")]
            Online_Poll_Update = 401005,
            [Description("����ͶƱ��ϸ")]
            Online_Poll_InsertItem = 401006,
            [Description("����ͶƱ��ϸ")]
            Online_Poll_UpdateItem = 401007,
            [Description("ɾ��ͶƱ��ϸ")]
            Online_Poll_DeleteItem = 401008,
            [Description("ͶƱ�趨��ʾ")]
            Online_Poll_Show = 401009,
            [Description("ͶƱ�趨����ʾ")]
            Online_Poll_NotShow = 401010,

            [Description("�ͻ���������")]
            Online_FeedBack_Update = 401011,

            //Complain
            [Description("������ϵ����Ϣ")]
            Complain_Contact_Update = 101401,
            [Description("�����¹�")]
            Complain_Abandon = 101402,
            [Description("ȡ�������¹�")]
            Complain_CancelAbandon = 101403,
            [Description("�����¹���һ��������Ա")]
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

        //��Ӧ������
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

        //��Ӧ�̺�������
        #region Cooperate Type
        //===============================================
        public enum CooperateType : int
        {
            [Description("����")]
            PurchaseSale = 0,
            [Description("��Ӫ")]
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

        //��������
        #region OnlineListArea
        //===============================================
        public enum OnlineListArea : int
        {
            //�����м����Ϊ���Ժ���չ, �������ֲ��ܸ���
            //Modified by Kyle for Icson -- Start
            [Description("��    ҳ.�ؼ�Top2(*)")]
            DefaultTop2 = 10,
            [Description("��    ҳ.��(*)")]
            DefaultUp = 20,
            [Description("��    ҳ.��(*)")]
            DefaultDown = 30,
            [Description("�������.չʾ(*)")]
            Hardware = 40,
            [Description("�������.Top10")]
            HardwareTopSale = 50,
            [Description("�����Ʒ.չʾ(*)")]
            Digital = 60,
            [Description("�����Ʒ.Top10")]
            DigitalTopSale = 70,
            [Description("�����Ĳ�.չʾ(*)")]
            Accessory = 80,
            [Description("�����Ĳ�.Top10")]
            AccessoryTopSale = 90,
            [Description("����")]
            Audio = 100,
            [Description("����")]
            Newcome = 120,
            [Description("��Ʒ��ϸ.��ɫ��Ʒ")]
            FeturedProduct = 130,
            [Description("����")]
            AOpenTop1 = 140,
            [Description("����")]
            AOpenNew = 150,
            [Description("��Ϸ����.չʾ(*)")]
            GameAcc = 160,
            [Description("��Ϸ����.Top10(*)")]
            GameAccTopSale = 170,
            [Description("�ƶ��洢.չʾ(*)")]
            MovSto = 180,
            [Description("�ƶ��洢.Top10(*)")]
            MovStoTopSale = 190,
            [Description("��������.չʾ(*)")]
            AudioFans = 200,
            [Description("��������.Top10(*)")]
            AudioFansTopSale = 210,
            [Description("����ͨѶ.չʾ(*)")]
            NetCom = 220,
            [Description("����ͨѶ.Top10(*)")]
            NetComTopSale = 230,
            [Description("�칫��Ʒ.չʾ(*)")]
            Office = 240,
            [Description("�칫��Ʒ.Top10(*)")]
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

        //��Ʒ����
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

        //������Ʒ����
        #region Product2ndType
        //===============================================
        public enum Product2ndType : int
        {
            [Description("��ͨ��Ʒ")]
            Normal = 0,
            [Description("����Ʒ")]
            Master = 1,
            [Description("����Ʒ")]
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

        //��Ʒ���ֶһ�����
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

        //�����״̬
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

        //���浥״̬
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

        //�ƿⵥ״̬
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
            [Description("RAM���ƿ�")]
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

        //ת����״̬
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

        //ת������ϸ����״̬
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

        //��Ʒ����״̬
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

        //��ƷStatusInfo
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

        #region �ɹ���״̬
        public enum POStatus : int
        {
            [Description("����")]
            Abandon = 0,
            [Description("��ʼ")]
            Origin = 1,
            [Description("��̯��")]
            WaitingApportion = 2,
            [Description("�����")]
            WaitingInStock = 3,
            [Description("�����")]
            InStock = 4,
            [Description("���ջ�")]
            WaitingReceive = 5,
            [Description("���ϼ�")]
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

        #region �ɹ�������
        public enum POType : int
        {
            [Description("��ͨ")]
            Normal = 0,
            [Description("����")]
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

        #region �ɹ�����������
        public enum POShipType : int
        {
            [Description("�Ͳֿ�")]
            Stock = 0,
            [Description("���ŵ�")]
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

        #region �ɹ�����Ʊ����
        public enum POInvoiceType : int
        {
            [Description("======")]
            NoInvoice = 1,
            [Description("����Ʊ")]
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

        #region �ɹ����������״̬
        public enum POManagerAuditStatus : int
        {
            [Description("��ʼ")]
            Origin = 0,
            [Description("���0��")]
            ZeroPoint = -1,
            [Description("���1��")]
            OnePoint = 1,
            [Description("���2��")]
            TwoPoint = 2,
            [Description("���3��")]
            ThreePoint = 3,
            [Description("���4��")]
            FourPoint = 4,
            [Description("���5��")]
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

        #region �ɹ�̯����ʽ����
        public enum POApportionType : int
        {
            [Description("ByMoney")]
            ByMoney = 0,			//�����
            [Description("ByQuantity")]
            ByQuantity = 1,		//������
            [Description("ByWeight")]
            ByWeight = 2			//������
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

        #region ���񸶿 POPayItemStyle
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

        #region ���񸶿 POPayItemStatus
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

        #region ����ɹ�Ӧ��  POPayStatus
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

        #region ����ɹ�Ӧ�� POPayInvoiceStatus
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

        #region ���������տ �տ�����
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

        #region ���������տ ״̬
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

        #region ���������տ ��������
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

        #region �������� ����֧��״̬
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

        #region �������� ����֧����Դ
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

        #region RMA���� RMAType
        public enum RMAType : int
        {
            [Description("��ȷ��")]
            Unsure = 0,
            [Description("���뷵��")]
            Maintain = 1,
            [Description("�����˻�")]
            Return = 2,
            [Description("�ܾ�����")]
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

        #region ���۵�״̬
        public enum SOStatus : int
        {
            [Description("�����˻�")]
            PartlyReturn = -5,
            [Description("ȫ���˻�")]
            Return = -4,
            [Description("��������")]
            ManagerCancel = -3,
            [Description("�ͻ�����")]
            CustomerCancel = -2,
            [Description("Ա������")]
            EmployeeCancel = -1,
            [Description("�����")]
            Origin = 0,
            [Description("������")]
            WaitingOutStock = 1,
            [Description("��֧��")]
            WaitingPay = 2,
            [Description("��������")]
            WaitingManagerAudit = 3,
            [Description("�ѳ���")]
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

        #region ���۵�Email����
        public enum SOEmailType : int
        {
            [Description("���۵�����")]
            AbandonSO = -1,
            [Description("���۵�����")]
            CreateSO = 0,
            [Description("���۵����")]
            AuditSO = 1,
            [Description("���۵�����")]
            OutStock = 2,
            [Description("���۵��ӷ�")]
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

        #region �˻���״̬
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

        //Ȩ�ޱ�. 
        #region Privilege
        //===============================================
        public enum Privilege : int
        {
            [Description("Ȩ��ά��")]
            PrivilegeOpt = 101,
            [Description("���ά��")]
            CategoryOpt = 102,
            [Description("������ά��")]
            ManufacturerOpt = 103,
            [Description("��Ӧ��ά��")]
            VendorOpt = 104,
            [Description("�ֿ�ά��")]
            StockOpt = 105,
            [Description("����ά��")]
            CurrencyOpt = 106,
            [Description("��������֧��")]
            AreaShipPay = 107,
            [Description("��Ʒ������Ϣ")]
            ProductBasic = 108,
            [Description("��Ʒ�۸���Ϣ")]
            ProductPrice = 109,
            [Description("��ƷPM��Ϣ")]
            ProductPM = 110,  //�����ϼܵ�
            [Description("��Ʒ������Ϣ")]
            ProductWeight = 111,
            [Description("��ƷͼƬ��Ϣ")]
            ProductPic = 112,
            [Description("��Ʒ�г��ͼ���Ϣ")]
            ProductPriceMarket = 113,
            [Description("��ƷԤ����Ϣ")]
            ProductPreview = 114,
            [Description("��Ӧ������ά��")]
            VendorManagerOpt = 115,
            [Description("�������ּ۸񵼳�")]
            CompetitorPriceExport = 116,
            [Description("�������ּ۸���ϵͳ")]
            CompetitorPriceImport = 117,
            [Description("����ά��")]
            SizeUpdate = 129,
            [Description("PM�ɹ��������ά��")]
            PMPOAmtRestrictOpt = 125,

            [Description("�Ż�ȯ����")]
            CouponCreate = 135,
            [Description("�Ż�ȯ���")]
            CouponAudit = 136,
            [Description("�Ż�ȯ�����������")]
            CouponRequestBatch = 137,
            [Description("�Ż�ȯ�������")]
            CouponRequest = 138,
            [Description("�Ż�ȯ�����������")]
            CouponRequestAudit = 139,
            [Description("�Ż�ȯ��������ȡ�����")]
            CouponRequestCancelAudit = 140,
            [Description("�Ż�ȯ������������")]
            CouponRequestAbandon = 141,


            [Description("������Ƶ�")]
            StLendFillIn = 501,
            [Description("���������")]
            StLendAbandon = 502,
            [Description("�����ȡ������")]
            StLendCancelAbandon = 503,
            [Description("��������")]
            StLendAudit = 504,
            [Description("�����ȡ�����")]
            StLendCancelAudit = 505,
            [Description("���������")]
            StLendOutStock = 506,
            [Description("�����ȡ������")]
            StLendCancelOutStock = 507,
            [Description("�������������")]
            StLendReturn = 508,

            [Description("���浥�Ƶ�")]
            StAdjustFillIn = 509,
            [Description("���浥����")]
            StAdjustAbandon = 510,
            [Description("���浥ȡ������")]
            StAdjustCancelAbandon = 512,
            [Description("���浥���")]
            StAdjustAudit = 513,
            [Description("���浥ȡ�����")]
            StAdjustCancelAudit = 514,
            [Description("���浥����")]
            StAdjustOutStock = 515,
            [Description("���浥ȡ������")]
            StAdjustCancelOutStock = 516,

            [Description("�ƿⵥ�Ƶ�")]
            StShiftFillIn = 517,
            [Description("�ƿⵥ����")]
            StShiftAbandon = 518,
            [Description("�ƿⵥȡ������")]
            StShiftCancelAbandon = 519,
            [Description("�ƿⵥ���")]
            StShiftAudit = 520,
            [Description("�ƿⵥȡ�����")]
            StShiftCancelAudit = 521,
            [Description("�ƿⵥ����")]
            StShiftOutStock = 522,
            [Description("�ƿⵥȡ������")]
            StShiftCancelOutStock = 523,
            [Description("�ƿⵥ���")]
            StShiftInStock = 524,
            [Description("�ƿⵥȡ�����")]
            StTranferCancelInStock = 525,

            [Description("ת�����Ƶ�")]
            StTransferFillIn = 526,
            [Description("ת��������")]
            StTransferAbandon = 527,
            [Description("ת����ȡ������")]
            StTransferCancelAbandon = 528,
            [Description("ת�������")]
            StTransferAudit = 529,
            [Description("ת����ȡ�����")]
            StTransferCancelAudit = 530,
            [Description("ת��������")]
            StTransferOutStock = 531,
            [Description("ת����ȡ������")]
            StTransferCancelOutStock = 532,

            [Description("������")]
            StVirtualOpt = 533,
            [Description("���Manager����")]
            StVirtualManagerOpt = 534,
            [Description("�ɹ���ǿ�����")]
            POForceAudit = 200,
            [Description("�ɹ����Ƶ�")]
            POFillIn = 201, //�����ɹ���/̯��/�ɹ������Ϻ�ȡ������
            [Description("�ɹ������")]
            POAudit = 202,	//����ȡ�����
            [Description("�ɹ������")]
            POInStock = 203,
            [Description("�ɹ���ȡ�����")]
            POCancelInStock = 204,
            [Description("�ɹ����ջ�")]
            POReceive = 205,
            [Description("�ɹ���ȡ���ջ�")]
            POCancelReceive = 206,
            [Description("�ɹ����������")]
            POManagerAudit = 207,
            [Description("�ɹ����ϼ�")]
            POShelving = 208,
            [Description("�ɹ���ȡ���ϼ�")]
            POCancelShelving = 209,

            [Description("����ɹ�����Ƶ�")]
            POPayFillIn = 301,
            [Description("����ɹ��������")]
            POPayAbandon = 302,
            [Description("����ɹ����ȡ������")]
            POPayCancelAbandon = 303,
            [Description("����ɹ����֧��")]
            POPayPay = 304,
            [Description("����ɹ����ȡ��֧��")]
            POPayCancelPay = 305,
            [Description("�ͻ����ֹ���")]
            AddPoint = 306,
            [Description("��������")]
            PointRequest = 307,
            [Description("�������")]
            PointAudit = 308,
            [Description("�������")]
            PointAdd = 309,
            [Description("����ɹ����PM����")]
            POPayRequestByPM = 310,
            [Description("����ɹ������������")]
            POPayRequestByDirector = 311,
            [Description("����ɹ�������")]
            POPayAudit = 312,
            [Description("����ɹ����ȡ�����")]
            POPayCancelAudit = 313,
            [Description("����ɹ�����ظ�����")]
            POPayRequestMoreTime = 314,
            [Description("����ɹ����TL�ύ��PMD����")]
            POPayRequestToPMD = 315,
            [Description("���㵥����ȷ��")]
            DSAccConfirm = 316,
            [Description("���㵥����ȡ��ȷ��")]
            DSAccCancelConfirm = 317,
            [Description("����ֽƾ֤")]
            DSUpdatePaperVoucher = 318,

            [Description("ǰ̨����ά��")]
            OnlineBulletin = 400,
            [Description("ǰ̨��Ʒչʾά��")]
            OnlineList = 401,
            [Description("ǰ̨ͶƱά��")]
            OnlinePoll = 402,
            [Description("�ͻ������޸�")]
            FeedBack = 403,

            //����Ȩ��
            [Description("���۹���")]
            ReviewManage = 405,
            TopicManage = 406,
            [Description("���ۻظ����")]
            ReviewReplyAudit = 407,

            //�۸�ٱ�
            [Description("�۸�ٱ����")]
            PriceReportAudit = 408,

            //SO 
            [Description("���۵��Ƶ�")]
            SOCreate = 150,
            [Description("���۵�Ա������")]
            SOEmployeeAbandon = 151,
            [Description("���۵����")]
            SOAudit = 152,
            [Description("���۵�ȡ�����")]
            SOCancelAudit = 153,
            [Description("���۵��������")]
            SOManagerAudit = 154,
            [Description("���۵�����ȡ�����")]
            SOManagerCancelAudit = 155,
            [Description("���۵���������")]
            SOManagerAbandon = 156,
            [Description("���۵�����")]
            SOOutStock = 157,
            [Description("���۵�ȡ������")]
            SOCancelOutStock = 158,
            [Description("���۵��޸�")]
            SOUpdate = 159,
            [Description("���۵�ȡ������")]
            SOCancelAbandon = 160,
            [Description("���۵���Ʊ��ӡ")]
            SOPrintInvoice = 176,
            [Description("���۵�ȡ������У��")]
            SOCancelOutStockCheck = 177,
            [Description("���۵����������ϴ�")]
            SOWeightImportData = 178,
            [Description("���۵�����У��")]
            SOOutStockCheck = 179,
            [Description("��ƷSN����")]
            ProductSNImport = 180,
            [Description("������˱���ά��")]
            SOUserAuditRatioUpdate = 181,
            [Description("�����ӻ�����")]
            DeliveryDelayAdd = 182,
            [Description("�����ӻ�ɾ��")]
            DeliveryDelayDelete = 183,
            [Description("�����ӻ�����")]
            DeliveryDelayUpdate = 184,
            [Description("�ֿ⹤������ά��")]
            WHUserWorkRatioUpdate = 185,
            [Description("���ʦ������ά��")]
            DeliveryManDepositOpt = 191,
            [Description("���͵����ö�ȷ���")]
            DLAllow = 193,
            [Description("���¶�����Ʊ")]
            SOUpdateInvoiceStatus = 186,
            [Description("���϶�����Ʊ")]
            SOAbandonInvoice = 187,
            [Description("���뷢Ʊ")]
            SORequestInvoice = 188,
            [Description("���͵�����ȷ��")]
            DLOutStockConfirm = 189,
            [Description("���͵����ɽ��㵥")]
            DSCreate = 190,
            [Description("���㵥���")]
            DSAudit = 194,
            [Description("���㵥����")]
            DSAbandon = 195,
            [Description("���ö������")]
            SOSetSizeType = 196,
            [Description("ά������ˢ����¼")]
            DSPosUpdate = 197,

            //SR�˻���
            [Description("�˻����½�")]
            SRNew = 210,
            [Description("�˻������")]
            SRAudit = 211,
            [Description("�˻���ȡ�����")]
            SRCancelAudit = 212,
            [Description("�˻����ջ�")]
            SRReceive = 213,
            [Description("�˻���ȡ���ջ�")]
            SRCancelReceive = 214,
            [Description("�˻����ϼ�")]
            SRShelve = 215,
            [Description("�˻���ȡ���ϼ�")]
            SRCancelShelve = 216,
            [Description("�˻������")]
            SRInStock = 217,
            [Description("�˻���ȡ�����")]
            SRCancelInStock = 218,
            [Description("�˻�������")]
            SRAbandon = 219,
            [Description("�˻���ȡ������")]
            SRCancelAbandon = 220,
            [Description("�˻�������")]
            SRUpdate = 221,

            //RMA
            [Description("RMA���Ƶ�")]
            RMACreate = 161,
            [Description("RMA������")]
            RMAAbandon = 162,
            [Description("RMA�����")]
            RMAAudit = 163,
            [Description("RMA��ȡ�����")]
            RMACancelAudit = 164,
            [Description("RMA����Ʒȷ��")]
            RMAReceive = 165,
            [Description("RMA��ȡ����Ʒȷ��")]
            RMACancelReceive = 166,
            [Description("RMA������")]
            RMAHandle = 167,
            [Description("RMA��ȡ������")]
            RMACancelHandle = 168,
            [Description("RMA���᰸")]
            RMAClose = 169,
            [Description("RMA���ؿ�")]
            RMAReopen = 170,
            [Description("RMA���޸�")]
            RMAUpdate = 171,
            //RO
            [Description("�˻������")]
            ROAudit = 172,
            [Description("�˻���ȡ�����")]
            ROCancelAudit = 173,
            [Description("�˻����˻�")]
            ROReturn = 174,
            [Description("�˻���ȡ���˻�")]
            ROCancelReturn = 175,

            //RMA New VersionȨ��
            //Register
            [Description("���»�����")]
            ProductNoUpdate = 601,
            [Description("���²�Ʒ������Ϣ")]
            RMACheckUpdate = 602,
            [Description("ȷ�����޷���")]
            RMAResponse = 603,
            [Description("����RMA��ע��Ϣ")]
            RMAMemoUpdate = 604,
            [Description("���ô�����")]
            WaitingOutBound = 605,
            [Description("���ô�����")]
            WaitingRevert = 606,    //���ﲹ���˶��ڷ��ǵ�ǰRMA Case�����������Ȩ�ޣ����636
            [Description("���ô��˿�")]
            WaitingRefund = 607,
            [Description("���ô������")]
            WaitingReturn = 608,
            [Description("����RMA��")]
            RMARegisterClose = 609,
            [Description("ReOpenRMA��")]
            RMARegisterReOpen = 610,
            [Description("��RMA���ύCC����")]
            RMASetToCC = 611,
            [Description("��RMA���ύRMA���Ŵ���")]
            RMASetToRMA = 612,
            [Description("�����Ƿ�7���ڱ���")]
            RMASet7Days = 635,
            [Description("PM���´�����Ϣ")]
            PMUpdateDunDesc = 649,

            //�޸�Vendor�ۺ���Ϣ
            [Description("RMA�޸�Vendor�ۺ���Ϣ")]
            RMAUpdateAfterSale = 639,
            //Request
            [Description("�������뵥")]
            RequestAdd = 613,                   //���ﲹ����һ�������ظ����뵥�����638
            [Description("�޸����뵥")]
            RequestUpdate = 614,
            [Description("���뵥�ջ�ȷ��")]
            RequestReceive = 615,
            [Description("�������뵥")]
            RequestAbandon = 616,

            //OutBound
            [Description("�������޵�")]
            OutBoundCreate = 617,
            [Description("�޸����޵�")]
            OutBoundUpdate = 618,
            [Description("���޳���")]
            OutBoundOutStore = 619,
            [Description("�������޵�")]
            OutBoundAbandon = 620,
            [Description("���޵�����У��")]
            OutBoundOutStockCheck = 650,

            //Revert
            [Description("���ɷ�����")]
            RevertAdd = 621,
            [Description("�޸ķ�����")]
            RevertUpdate = 622,
            [Description("��������")]
            RevertOutStore = 623,
            [Description("��������-����Ʒ")]
            RevertOutStore_New = 624,
            [Description("���Ϸ�����")]
            RevertAbandon = 625,
            [Description("����������У��")]
            RevertOutStockCheck = 642,

            //Return
            [Description("�����˻���ⵥ")]
            ReturnAdd = 626,
            [Description("�޸��˻���ⵥ")]
            ReturnUpdate = 627,
            [Description("�˻����")]
            ReturnInStore = 628,
            [Description("�����˻���ⵥ")]
            ReturnAbandon = 629,    //���ﲹ�����˻������˵�Ȩ�ޣ���637��
            [Description("ȡ���˻����")]
            ReturnCancelInStore = 640,

            //Refund
            [Description("�����˿")]
            RefundAdd = 630,
            [Description("�޸��˿")]
            RefundUpdate = 631,
            [Description("�����˿")]
            RefundAudit = 632,
            [Description("�˿�")]
            RefundRefund = 633,
            [Description("�����˿")]
            RefundAbandon = 634,
            [Description("���������˿�")]
            RefundDirectorAudit = 650,

            [Description("RMA�������")]
            RMARevertAudit = 636,     //RMA������������ҳ���ϵ���ˡ�

            [Description("�˻�������")]
            RMAReturnAudit = 637,     //RMA����SelectTargetҳ���ϵ���ˡ�

            [Description("�ظ��������뵥")]
            RequestAddRepeate = 638,         //����Ӧ��־������ RMA_Request_ReCreate

            [Description("�޸����޵���ԤԼ��������/ԤԼ��������")]
            RMAOutBoundDate = 641,

            //SendAccessory
            [Description("��������������")]
            SendAccessoryAdd = 643,
            [Description("�޸Ĳ���������")]
            SendAccessoryUpdate = 644,
            [Description("�����������")]
            SendAccessoryAudit = 645,
            [Description("������������")]
            SendAccessorySend = 646,
            [Description("��������ȡ������")]
            SendAccessoryCancelSend = 648,
            [Description("���ϲ���������")]
            SendAccessoryAbandon = 647,

            //Solution
            //[Description("Slnά��")]
            //SlnOpt = 601,
            //[Description("SlnItemά��")]
            //SlnItemOpt = 602,
            //[Description("Prjά��")]
            //PrjOpt = 611,
            //[Description("PrjItemά��")]
            //PrjItemOpt = 612,

            [Description("��������������У��")]
            SendAccessoryOutStockCheck = 651,
            [Description("���ô��ƿ�")]
            SetWaitingShift = 652,
            [Description("ȡ�����ƿ�")]
            CancelWaitingShift = 653,
            [Description("���ô�����")]
            SetWaitingHandover = 665,
            [Description("ȡ��������")]
            CancelWaitingHandover = 667,

            [Description("���·��޼����Ϣ")]
            CheckRepairUpdate = 662,
            [Description("���ó�7����˿�")]
            BeyondWaitingRefund = 664,


            [Description("�˻������ô��ƿ�")]
            ReturnSetWaitingShift = 680,
            [Description("�˻���ȡ�����ƿ�")]
            ReturnCancelWaitingShift = 681,

            //RMA New VersionȨ��
            //Register
            [Description("�����˿���Ϣ������")]
            RMAIsRecommendRefundUpdate = 665,

            //Handover
            [Description("���ӵ�����")]
            HandoverCreate = 667,
            [Description("���ӵ��޸�")]
            HandoverUpdate = 668,
            [Description("���ӵ�����")]
            HandoverOutStock = 669,
            [Description("���ӵ�ȡ������")]
            HandoverCancelOutStock = 670,
            [Description("���ӵ�����")]
            HandoverAbandon = 671,
            [Description("���ӵ�����")]
            HandoverReceive = 672,
            [Description("���ӵ�ȡ������")]
            HandoverCancelReceive = 673,

            //�տ
            [Description("�����տȷ��")]
            SOIncomeConfirm = 701,
            [Description("�����տ����")]
            SOIncomeAbandon = 702,
            [Description("�����տȡ��ȷ��")]
            SOIncomeUnConfirm = 703,
            [Description("�����տ����ȡ��ȷ��")]
            SOIncomeExpireUnConfirm = 704,
            [Description("�����տƾ֤����")]
            SOIncomeVoucher = 705,
            [Description("�����տȡ��ȷ�ϱ���")]
            SOIncomeUnComfirmOthers = 706,
            [Description("����֧���������")]
            SONetPayAddAudit = 707,

            [Description("�鿴�ͻ�����")]
            Customer_WatchPwd = 801,
            [Description("��Ա����")]
            Customer_Set = 802,
            [Description("�����Լ����¹�")]
            ComplainOwn = 803,
            [Description("����ȫ���¹�")]
            ComplainAll = 804,
            [Description("�����¹ʻطý��")]
            UpdateReviewBack = 805,
            [Description("�����¹�")]
            ComplainAbandon = 806,
            [Description("ȡ�������¹�")]
            ComplainCancelAbandon = 807,
            [Description("�¹ʽ᰸")]
            ComplainClose = 808,
            [Description("�¹����")]
            ComplainAudit = 809,
            [Description("�¹ʣ�Ա������")]
            ComplainEmployeeSuggest = 810,

            [Description("�����ֻ�����")]
            SendSMS = 850,

            //����鿴
            [Description("���۱���")]
            SaleReport = 901,
            [Description("��Ʒ���۷���ȫ��")]
            SaleAnalysisAll = 902,
            [Description("��Ʒ���۷����Լ�")]
            SaleAnalysisOwn = 903,
            [Description("��汨��ȫ��")]
            InventoryReportAll = 904,
            [Description("��汨���Լ�")]
            InventoryReportOwn = 905,
            [Description("PM���۱���ȫ��")]
            PMSaleReportAll = 906,
            [Description("PM���۱����Լ�")]
            PMSaleReportOwn = 907,
            [Description("PMռ���ʽ�ȫ��")]
            PMFundsReportAll = 908,
            [Description("PMռ���ʽ��Լ�")]
            PMFundsReportOwn = 909,
            [Description("PM����ȫ��")]
            PMPayDateReportAll = 910,
            [Description("PM�����Լ�")]
            PMPayDateReportOwn = 911,
            [Description("����ⵥ��")]
            WarehouseWorkload = 912,
            [Description("��Ʒ�۸��������ȫ��")]
            PriceChangeReportAll = 913,
            [Description("��Ʒ�۸���������Լ�")]
            PriceChangeReportOwn = 914,
            [Description("�������۵�ͳ��")]
            SOOutStockSearchSummary = 915,
            [Description("��Ʒ�ɹ��ɱ�����ȫ��")]
            PurchaseCostReportAll = 916,
            [Description("��Ʒ�ɹ��ɱ������Լ�")]
            PurchaseCostReportOwn = 917,
            [Description("��Ʒ����ʱ���ȫ��")]
            ProductDailyClickAll = 918,
            [Description("��Ʒ����ʱ����Լ�")]
            ProductDailyClickOwn = 919,
            [Description("���ֱ���")]
            PointReport = 920,
            [Description("RMA����")]
            RMAReportSearch = 921,
            [Description("�˷ѱ���")]
            ShipPriceSearch = 922,
            [Description("��Ʊ�����Լ�")]
            InvoiceReportOwn = 923,
            [Description("��Ʊ����ȫ��")]
            InvoiceReportAll = 924,
            [Description("���ݳ�ʼ��")]
            SystemDataInit = 900,
            [Description("ȫȨ")]
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

        //��Ҫͬ��������. 
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

        #region �������ȼ�
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
            [Description("����")]
            Abandon = -1,
            [Description("δ֪ͨ")]
            UnNotify = 0,
            [Description("��֪ͨ")]
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
            [Description("������")]
            Abandon = -1,
            [Description("�����")]
            Origin = 0,
            [Description("�����")]
            Audited = 1,
            [Description("������")]
            Received = 2,
            [Description("������")]
            Handled = 3,
            [Description("�Ѵ���")]
            Closed = 4,
            [Description("������")]
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
            [Description("������")]
            Abandon = -1,
            [Description("������")]
            Origin = 0,
            [Description("������")]
            SendAlready = 1,
            [Description("���ַ���")]
            PartlyResponsed = 2,
            [Description("ȫ������")]
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
            [Description("��/��")]
            SendNoInvoice = 0,
            [Description("��/��")]
            SendWithInvoice = 1,
            [Description("��/��")]
            BackInvoice = 2,
            [Description("��/δ��")]
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

        // Ͷ��ϵͳ
        // Ͷ��״̬
        public enum ComplainStatus
        {
            [Description("������")]
            Abandoned = -1,
            [Description("������")]
            Orgin = 0,
            [Description("������")]
            Replied = 1,
            [Description("�������")]
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
        //Ͷ��ϵͳ 


        //RMA_Request
        public enum RMARequestStatus
        {
            [Description("������")]
            Abandon = -1,
            [Description("������")]
            Orgin = 0,
            [Description("������")]
            Handling = 1,
            [Description("�������")]
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

        //RMARequest ����
        public enum RMARequestType
        {
            [Description("��ȷ��")]
            Unsure = 0,
            [Description("���뷵��")]
            Maintain = 1,
            [Description("�����˻�")]
            Return = 2,
            [Description("�ܾ�����")]
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

        //RMARevertStatus ����
        public enum RMARevertStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("������")]
            WaitingRevert = 0,
            [Description("�ѷ���")]
            Reverted = 1,
            [Description("�����")]
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

        //RMARefundStatus ����
        public enum RMARefundStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("���˿�")]
            WaitingRefund = 0,
            [Description("�����")]
            Audited = 1,
            [Description("���˿�")]
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
        //RMASendAccessoryStatus ����
        public enum RMASendAccessoryStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("�����")]
            WaitingAudit = 0,
            [Description("������")]
            WaitingSend = 1,
            [Description("�ѷ���")]
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
        //RMAAccessoryType ����
        public enum RMAAccessoryType
        {
            [Description("����")]
            Accessory = 0,
            [Description("��Ʒ")]
            Gift = 1,
            [Description("��Ʒ+����")]
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

        //RMARequestReason ����
        public enum RMARequestReasonType
        {
            [Description("��������")]
            QualityReason = 0,
            [Description("������")]
            Conmpatibility = 1,
            [Description("������")]
            Discontented = 2,
            [Description("��������")]
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

        //RMARefundPayType ����

        public enum RMARefundPayType : int
        {
            [Description("ת�����˿�")]
            TransferPointRefund = 1,
            [Description("�ֽ��˿�")]
            CashRefund = 0,
            //[Description("�����˿�")]
            //BankRefund = 2
            [Description("֧�����˿�")]
            AlipayRefund = 2,
            [Description("�����˿�")]
            ICBCRefund = 3,
            [Description("�����˿�")]
            CBCRefund = 4,
            [Description("�����˿�")]
            CMBRefund = 5,
            [Description("�ַ��˿�")]
            SPDBRefund = 6,
            [Description("ũ���˿�")]
            ABCRefund = 7,
            [Description("�����˿�")]
            COMMRefund = 8,
            [Description("�ʾ��˿�")]
            PostRefund = 9,
            [Description("�����˿�")]
            OtherRefund = 10
            //���С����С����С��ַ���ũ�С�����
        }
        public static SortedList GetRMARefundPayType()
        {
            return GetStatus(typeof(RMARefundPayType));
        }
        public static string GetRMARefundPayType(object v)
        {
            return GetDescription(typeof(RMARefundPayType), v);
        }


        //RMAReturnStatus ����
        public enum RMAReturnStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("�������")]
            WaitingReturn = 0,
            [Description("�������")]
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

        //RMANextHandler ����
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
            [Description("δָ��")]
            unknown = 0,
            [Description("����")]
            Local = 1,
            [Description("���")]
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
            [Description("�Ǵ���״̬")]
            Origin = 0,
            [Description("�ͻ�")]
            Customer = 1,
            [Description("ORS�̳�")]
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
            [Description("�Ǵ���״̬")]
            Origin = 0,
            [Description("ORS�̳�")]
            Icson = 1,
            [Description("��Ӧ��")]
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

        #region RMA_Register������Ʒ��־��ö�ٶ���

        public enum NewProductStatus
        {
            [Description("�ǻ���")]
            Origin = 0,
            [Description("����Ʒ")]
            NewProduct = 1,
            [Description("������")]
            SecondHand = 2,
            [Description("�ǵ�ǰCase��Ʒ")]
            OtherProduct = 3
        }

        /// <summary>
        /// ��ȡ��Ʒö�ٶ����ֵ
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


        #region RMA������
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

        //��Ʒ��������
        #region AttributeType 0,1,2,3
        //===========================================
        public enum AttributeType : int
        {
            [Description("��ͨ�ı�")]
            Text = 0,
            [Description("ѡ�������")]
            OptionFirst = 1,
            [Description("ѡ��")]
            OptionSecond = 2,
            [Description("ѡ��")]
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

        //����״̬��ѯ����
        #region ShipStatusQueryType 1,2,3,4
        //===========================================
        public enum ShipStatusQueryType : int
        {
            [Description("DeliveryManLink")]
            DeliveryManLink = 1,  //��ʾ���ʦ����ϵ��ʽ
            [Description("ExpressLink")]
            ExpressLink = 2,          //��ݹ�˾����
            [Description("QueryUrl")]
            QueryUrl = 3,                 //��ݹ�˾��ѯurl
            [Description("NoneQuery")]
            NoneQuery = 4                //�޷���ѯ
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

        #region ��ʱ����״̬
        public enum CountdownStatus : int
        {
            [Description("����")]
            Abandon = -1,
            [Description("����")]
            Ready = 0,
            [Description("����")]
            Running = 1,
            [Description("���")]
            Finish = 2,
            [Description("��ֹ")]
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

        #region ��ʱ��������
        public enum CountdownType : int
        {
            [Description("һ����")]
            OneTime = 1,
            [Description("ÿ��")]
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

        //��Ա�ȼ�
        #region CustomerRank
        public enum CustomerRank : int
        {
            [Description("��ͨ��Ա")]
            Ordinary = 0,
            [Description("һ�ǻ�Ա")]
            OneStar = 1,
            [Description("���ǻ�Ա")]
            TwoStar = 2,
            [Description("���ǻ�Ա")]
            ThreeStar = 3,
            [Description("���ǻ�Ա")]
            FourStar = 4,
            [Description("���ǻ�Ա")]
            FiveStar = 5,
            [Description("�ƽ��Ա")]
            Golden = 6,
            [Description("��ʯ��Ա")]
            Diamond = 7,
            [Description("����VIP��Ա")]
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
            [Description("��ͨ(�Զ�)")]
            AutoNonVIP = 1,
            [Description("VIP(�Զ�)")]
            AutoVIP = 2,
            [Description("��ͨ(�ֶ�)")]
            ManualNonVIP = 3,
            [Description("VIP(�ֶ�)")]
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
            [Description("��")]
            A = 0,
            [Description("ͭ")]
            B = 1,
            [Description("��")]
            C = 2,
            [Description("��")]
            D = 3,
            [Description("��")]
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

        //��Ա����
        #region CustomerType
        public enum CustomerType : int
        {
            [Description("�����û�")]
            Personal = 0,
            [Description("��˾Ա��")]
            Employee = 1,
            [Description("�Ա�����")]
            Taobao = 2,
            [Description("��Ȥ����")]
            Ebay = 3,
            [Description("���Ĵ���")]
            Paipai = 4,
            [Description("��ҵ��ͻ�")]
            Enterprice = 5,
            [Description("��ҵVIP")]
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

        //�̵�����
        #region InventoryReportType
        public enum InventoryReportType : int
        {
            [Description("ÿ���̵�")]
            EveryDay = 1,
            [Description("ÿ���̵�")]
            EveryWeek = 2,
            [Description("ÿ���̵�")]
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

        //�ͻ��Ƽ�״̬
        #region CommendStatus
        public enum CommendStatus : int
        {
            [Description("ԭʼ״̬")]
            Primitive = -1, //ԭʼ״̬Ϊ��Email�����Ƽ��б�
            [Description("��ʼ״̬")]
            Origin = 0,     //��ʼ״̬Ϊ���Ƽ�
            [Description("�Ƽ��ͻ���ע��")]
            Registered = 1,
            [Description("�Ƽ��ͻ��ѹ���")]
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

        //���˷�����ԭ������
        #region FreeShipFeeLogType
        //===========================================
        public enum FreeShipFeeLogType : int
        {
            [Description("�Ƽ��ͻ�Email")]
            CommendCustomer = 1,
            [Description("�Ƽ��Ŀͻ�ע��")]
            CustomerRegister = 2,
            [Description("�Ƽ��Ŀͻ��״ι���")]
            CustomerBuyFirst = 3,
            [Description("����֧���˷�")]
            CreateOrder = 4,
            [Description("���϶���")]
            AbandonSO = 5,
            [Description("ȡ�����϶���")]
            CancelAbandonSO = 6,
            [Description("���¶���")]
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

        //�����Ƽ�����:�ؼ�,��Ʒ,��ѡ,����,����
        //YoeJoy��Ŀֻ�õ�1~4
        #region OnlineRecommendType
        public enum OnlineRecommendType : int
        {
            [Description("������Ʒ")]
            Discount = 1,
            [Description("��Ʒ����")]
            NewArrive = 2,
            [Description("��ʱ����")]
            Featured = 3,
            [Description("�Ź�")]
            HotSale = 4,//����
            [Description("����Ʒ")]
            Promotion = 5,
            [Description("������������")]
            PromotionTopic = 6,
            [Description("���½���")]
            PopularProduct = 7,
            [Description("��Ʒ����")]
            PowerfulSale = 8,
            [Description("�����Ƽ�")]
            ExcellentRecommend = 9,
            [Description("Ʒ���Ƽ�")]
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

        //������ʾ��������
        #region OnlineAreaType
        public enum OnlineAreaType : int
        {
            [Description("��ҳ")]
            HomePage = 1,
            [Description("һ������")]
            FirstCategory = 2,
            [Description("��������")]
            SecondCategory = 3,
            [Description("��������")]
            ThirdCategory = 4,
            [Description("��Ʒҳ��")]
            ItemDetail = 5,
            [Description("���߰���")]
            ServiceFaq = 6,
            [Description("�ʻ�����")]
            AccountCenter = 7,
            [Description("����ҳ")]
            Search = 8,
            [Description("���ﳵ")]
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

        //QA����
        #region QAType
        public enum QAType : int
        {
            //[Description("��վ����")] Introduce = 1,
            //[Description("��������")] ShoppingFlow = 2,
            //[Description("�ۺ����")] Service = 3,
            //[Description("֪ʶ��")] Knowledge = 4,
            //[Description("������")] Technology = 5
            //ר���б������ٵݣ�ORS�̳�ѧ�ã��������⣬ѡ��ָ�ϣ�ʹ�ü��ɣ�����֪ʶ
            [Description("ר���б�")]
            ThematicList = 1,
            [Description("�����ٵ�")]
            MarketExpress = 2,
            [Description("ORS�̳�ѧ��")]
            IcsonStudy = 3,
            [Description("��������")]
            FAQ = 4,
            [Description("ѡ��ָ��")]
            BuyingGuide = 5,
            [Description("ʹ�ü���")]
            UsingSkill = 6,
            [Description("����֪ʶ")]
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

        //����ϵͳö�ٿ�
        #region  ����ϵͳ
        /// <summary>
        /// Topic ���� ,Experience(����)=1,Discuss(�ο�)=2
        /// </summary>
        public enum TopicType : int
        {
            [Description("����")]
            Experience = 1,
            [Description("�ο�")]
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
        /// ����ϵͳ�пͻ�Ȩ�޵ȼ�
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
        /// Topic ��״̬
        /// </summary>
        public enum TopicStatus
        {
            [Description("������")]
            Abandon = -2,
            [Description("δ���")]
            unconfirmed = -1,
            [Description("�����")]
            confirmed = 1,
            [Description("�ѻظ�")]
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
        /// Topic Reply ��״̬
        /// </summary>
        public enum TopicReplyStatus
        {
            [Description("��ͨ")]
            Normal = 0,
            [Description("������")]
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
            [Description("��ͨ")]
            Normal = 0,
            [Description("������")]
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
            [Description("��������")]
            AbandonTopic,
            [Description("������������")]
            CancelAbandonTopic,
            [Description("�ö�����")]
            TopicSetTop,
            [Description("���þ���")]
            TopicSetDigset,
            [Description("ȡ���ö�")]
            TopicCancelTop,
            [Description("ȡ������")]
            TopicCancelDigset,
            [Description("�������")]
            ConfirmTopic,
            [Description("ȡ���������")]
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
        /// ��ʾ������� Topic ��ҳ������
        /// </summary>
        public enum RandomDigestTopicPage
        {
            Homepage,
            ProductDetail,
        }

        /// <summary>
        /// Topic �б���������
        /// </summary>
        public enum TopicListSortBy
        {
            [Description("����ʱ������")]
            CreateDate,
            [Description("��������")]
            Score,
            [Description("������������")]
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
            [Description("��Ʒ")]
            Product = 0,
            [Description("���")]
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

        //����ϵͳö�ٿ�
        #region  ����ϵͳ

        /// <summary>
        /// ����
        /// </summary>
        public enum ReviewScore : int
        {
            [Description("5�� �ܺ�")]
            Excellent = 5,
            [Description("4�� ��")]
            Good = 4,
            [Description("3�� һ��")]
            Average = 3,
            [Description("2�� ��")]
            Poor = 2,
            [Description("1�� �ܲ�")]
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
        /// ӵ��ʱ��
        /// </summary>
        public enum OwnedType : int
        {
            [Description("0-1��")]
            OneDay = 1,
            [Description("1��-1��")]
            OneWeek = 2,
            [Description("1��-1��")]
            OneMonth = 3,
            [Description("1��-1��")]
            OneYear = 4,
            [Description("1������")]
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
            [Description("1 - ����Ϥ")]
            LowUnderstand = 1,
            [Description("2 - ��Ϥһ���")]
            SomewhatUnderstand = 2,
            [Description("3 - ������Ϥ")]
            AverageUnderstand = 3,
            [Description("4 - ����Ϥ")]
            HighUnderstand = 4,
            [Description("5 - ��ͨ")]
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
        /// Review ���� ,Experience(����)=1,Discuss(�ο�)=2
        /// </summary>
        public enum ReviewType : int
        {
            [Description("����")]
            Experience = 1,
            [Description("����")]
            Discuss = 2,
            [Description("�Ƽ�")]
            Recommend = 3,
            [Description("ѯ��")]
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
        /// ����ϵͳ�пͻ�Ȩ�޵ȼ�
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
        /// Review ��״̬
        /// </summary>
        public enum ReviewStatus
        {
            [Description("������")]
            Abandon = -2,
            [Description("δ���")]
            UnConfirmed = -1,
            [Description("�����")]
            Confirmed = 1,
            [Description("�ѻظ�")]
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
        /// Review Reply ��״̬
        /// </summary>
        public enum ReviewReplyStatus
        {
            [Description("������ʾ")]
            Normal = 0,
            [Description("������")]
            Abandon = 1,
            [Description("�����")]
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
            [Description("��ͨ")]
            Normal = 0,
            [Description("������")]
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
            [Description("�˿�")]
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
            [Description("��������")]
            AbandonReview,
            [Description("������������")]
            CancelAbandonReview,
            [Description("�ö�����")]
            ReviewSetTop,
            [Description("���þ���")]
            ReviewSetGood,
            [Description("ȡ���ö�")]
            ReviewCancelTop,
            [Description("ȡ������")]
            ReviewCancelGood,
            [Description("�������")]
            ConfirmReview,
            [Description("ȡ���������")]
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
        /// ��ʾ������� Review ��ҳ������
        /// </summary>
        public enum RandomDigestReviewPage
        {
            Homepage,
            ProductDetail,
        }

        /// <summary>
        /// Review �б���������
        /// </summary>
        public enum ReviewListSortBy
        {
            [Description("����ʱ������")]
            CreateDate,
            [Description("��������")]
            Score,
            [Description("������������")]
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
            [Description("��Ʒ")]
            Product = 0,
            [Description("���")]
            Category = 1,
            [Description("����")]
            Promotion = 2,
            [Description("װ����")]
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

        #region �������ת����������ͣ����ۡ��ֶ�����
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

        #region ����ת����״̬
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

        // �����˿�ı�־
        public enum RecommendRefund : int
        {
            [Description("YES")]
            Yes = 1,
            [Description("NO")]
            No = 0
        }

        //�ֿ�����
        #region StockType
        public enum StockType : int
        {
            [Description("�����ֿ�")]
            Normal = 1,
            [Description("RMA�ֿ�")]
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

        //��ӻ���״̬
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

        //��������
        #region Competitor
        //===========================================
        public enum Competitor : int
        {
            [Description("����")]
            Other = 0,
            [Description("����")]
            JD = 1,
            [Description("�µ�")]
            NewEgg = 2,
            [Description("С��")]
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

        //�۸�ٱ�
        #region PriceReportStatus
        //===========================================
        public enum PriceReportStatus : int
        {
            [Description("����")]
            Abandon = -1,
            [Description("��ʼ")]
            Origin = 0,
            [Description("���δ�����۸�")]
            AuditedNotChangePrice = 1,
            [Description("����ѵ����۸�")]
            AuditChangePrice = 2,
            [Description("��˴������۸�")]
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

        //�۸�ٱ�ԭ�����
        #region PriceReportReason
        //===========================================
        public enum PriceReportReason : int
        {
            [Description("����")]
            Other = 0,
            [Description("�޷���֤")]
            NotAvailable = 1,
            [Description("����ͬһ��Ʒ")]
            NotSameProduct = 2,
            [Description("�Է��޻�")]
            ProductNoStock = 3,
            [Description("�۸�����")]
            PriceDiffLittle = 4,
            [Description("�۸�δ����")]
            PriceNotTrack = 5,
            [Description("�۸�����,��˲�ɱ�")]
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

        //�۸�ٱ���������
        #region PriceReportHandleType
        //===========================================
        public enum PriceReportHandleType : int
        {
            [Description("����")]
            Other = 0,
            [Description("��һ���ٱ�***")]
            FirstReport = 1,
            [Description("�ڶ����ٱ�***")]
            SecondReport = 2,
            [Description("�������ٱ�***")]
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

        //�г���ͼ�״̬
        #region MarketLowestPriceStatus
        //===========================================
        public enum MarketLowestPriceStatus : int
        {
            [Description("����")]
            Abandon = -1,
            [Description("��ʼ")]
            Origin = 0,
            [Description("���")]
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


        //�����б�
        #region DepartmentID
        //===========================================
        public enum DepartmentID : int
        {
            [Description("�����ܼ�")]
            Manager = 10,
            [Description("�ͷ�")]
            CS = 11,
            [Description("PM")]
            PM = 12,
            [Description("����")]
            Finance = 13,
            [Description("�ֿ�")]
            WH = 14,
            [Description("MIS")]
            MIS = 15,
            [Description("RMA")]
            RMA = 16,
            [Description("���ݷ�վ")]
            HZ = 17,
            [Description("�Ͼ���վ")]
            NJ = 18,
            [Description("���ݷ�վ")]
            SZ = 19,
            [Description("���ݷ�վ")]
            YZ = 20,
            [Description("�Ϻ����ʦ��")]
            ExpressMan = 21,
            [Description("�����")]
            Branch = 22,
            [Description("��վ���ʦ��")]
            BranchMan = 23,
            [Description("����")]
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

        //�¹�����
        public enum AbnormalType
        {
            [Description("����")]
            Other = 0,
            [Description("Ͷ��")]
            Complain = 1,
            [Description("�ͻ�����")]
            CustomerSuggest = 2,
            [Description("�ڲ�����")]
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

        //�¹�ԭ��
        public enum AbnormalCauseType
        {
            [Description("Ա������ʧְ")]
            EmployeeNeglect = 1,
            [Description("��ݹ�˾ʧְ")]
            ExpressNeglect = 2,
            [Description("������ԱƷ������")]
            EmployeeMoralFault = 3,
            [Description("Ա������")]
            EmployeeSuggest = 4,
            [Description("����ӻ�����")]
            ExpressDelay = 5,
            [Description("PM�����ӳ�,�ӻ���������")]
            PMDelay = 6,
            [Description("PM���������󵼹˿�")]
            ProductInfoError = 7,
            [Description("�Դ��ͻ�̬������")]
            MisbehaviorToCustomer = 8,
            [Description("����")]
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
        //�Ƿ�Ϊ������ַ״̬
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

        //RMAReceiveType ���뵥�ջ�����
        public enum RMARecieveType
        {
            [Description("����")]
            Single = 0,
            [Description("����")]
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

        //RMARegisterReceiveType �����ջ�����
        public enum RMARegisterRecieveType
        {
            [Description("������Ʒ")]
            SingleBad = 0,
            [Description("������Ʒ")]
            HostGood = 1,
            [Description("������Ʒ")]
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

        //VirtualArriveTime ��⵽��ʱ��
        public enum VirtualArriveTime
        {
            [Description("1-3��")]
            OneToThree = 1,
            [Description("2-7��")]
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

        //��Ӧ����������
        #region APType 1,2,3,4,5,6,7,8,9
        //===========================================
        public enum APType : int
        {
            [Description("Ԥ����")]
            PayInAdvance = 1,
            [Description("������Ʊ��")]
            PayWhenArrive0 = 2,
            [Description("������Ʊ��1��")]
            PayWhenArrive7 = 3,
            [Description("������Ʊ��10��")]
            PayWhenArrive10 = 4,
            [Description("������Ʊ��2��")]
            PayWhenArrive14 = 5,
            [Description("������Ʊ��20��")]
            PayWhenArrive20 = 6,
            [Description("������Ʊ��1����")]
            PayWhenArrive30 = 7,
            [Description("������Ʊ��45��")]
            PayWhenArrive45 = 8,
            [Description("ÿ��25����Ʊ��")]
            PayMonth25 = 9,
            [Description("ÿ��10,25����Ʊ��")]
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

        //WhWorkType �ֿ⹤������
        public enum WhWorkType
        {
            [Description("���")]
            ProductInspection = 1,
            [Description("�ϼ�")]
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

        //WhWorkBillType �ֿ⹤����������
        public enum WhWorkBillType
        {
            [Description("����")]
            SO = 1,
            [Description("�ƿⵥ")]
            Shift = 2,
            [Description("ת����")]
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

        //DeliveryDelayCause �����ӳ�ԭ��
        public enum DeliveryDelayCause
        {
            [Description("�ͻ�Ҫ�������")]
            CustomerChangeTime = 1,
            [Description("�ͻ��ĵ�ַ��������")]
            CustomerChangeAddress = 2,
            [Description("�ͻ����գ�����ȡ��")]
            CustomerRefuse = 3,
            [Description("ʦ���ӻ����͸�����")]
            FreightManDelay = 4,
            [Description("ʦ���ӻ����ͣ�����ȡ��")]
            FreightManDelayCancelSO = 5,
            [Description("ʦ������̬�����⣬����ȡ��")]
            FreightManBadServiceAttitude = 6,
            [Description("�ͷ�������ַ��˴��󣬸�����")]
            CSAuditError = 7,
            [Description("�ͷ�δȷ�Ϸ�Ʊ���⣬������")]
            CSCannotRequestVat = 8,
            [Description("��Ʒ�������⣬������")]
            ProductQuality = 9,
            [Description("��Ʊ���������")]
            VatProblem = 10,
            [Description("ʦ����ϵ���Ͽͻ���������")]
            CustomerCannotCantact = 11,
            [Description("����")]
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
        /// PM����쳣����״̬
        /// </summary>
        public enum POPayItemErrRequestStatus
        {
            [Description("PMD�Ѳ���")]
            PMDReturn = -3,
            [Description("TL�Ѳ���")]
            TLReturn = -2,
            [Description("PMȡ������")]
            PMCancelRequest = -1,
            [Description("PM�쳣����")]
            PMRequest = 0,
            [Description("TL�������")]
            TLRequst = 1,
            [Description("TL�����")]
            TLAudited = 2,
            [Description("PMD�����")]
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
        /// ���ɹ�������(�˻�������)
        /// </summary>
        public enum MinusPOType
        {
            [Description("�˻�")]
            Return = 0,
            [Description("����")]
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


        //��վ�б�
        #region BranchID
        //===========================================
        public enum BranchID : int
        {
            [Description("�ܲ�")]
            WH = 1,
            [Description("����վ")]
            XH = 2,
            [Description("���ַ�վ")]
            YP = 3,
            [Description("���ӷ�վ")]
            PT = 4,
            [Description("�ֶ���վ")]
            PD = 5,
            [Description("���з�վ")]
            MH = 6,
            [Description("���ݷ�վ")]
            SZ = 7,
            [Description("���ݷ�վ")]
            YZ = 8,
            [Description("���ݷ�վ")]
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

        //�����б�״̬ DLStatus
        #region DLStatus
        public enum DLStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("��ʼ")]
            Origin = 0,
            [Description("����ȷ��")]
            StockConfirmed = 1,
            [Description("����ȷ��")]
            AccountConfirmed = 2,
            [Description("����ȷ��")]
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


        //�����б�������
        #region DLItemType
        public enum DLItemType
        {
            [Description("���۵�")]
            SaleOrder = 1,
            [Description("RMAȡ����")]
            RMARequest = 2,
            [Description("RMA������")]
            RMARevert = 3,
            [Description("RMA���޵�")]
            RMAOutbound = 4,
            [Description("RMA����������")]
            RMASendAccessory = 5,
            [Description("�ƿⵥ")]
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

        //���ͽ��㵥״̬ DSStatus

        #region DSStatus
        public enum DSStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("��ʼ")]
            Origin = 0,
            [Description("����ȷ��")]
            AccountConfirmed = 1,
            [Description("��վ���")]
            Audited = 2,
            [Description("�������")]
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
            [Description("����ཻ")]
            PayMentofGoodsExcessive = 1,
            [Description("�����ٽ�")]
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

        //�����쳣����״̬
        #region AbnormalSOStatus
        public enum AbnormalSOStatus : int
        {
            [Description("������")]
            Handling = 1,
            [Description("���")]
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
        /// �����ߴ�����
        /// </summary>
        #region SizeType
        public enum SizeType
        {
            [Description("С��")]
            Small = 1,
            [Description("�м�")]
            Middle = 2,
            [Description("���")]
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
        /// ��������
        /// </summary>
        #region RemittanceType
        public enum RemittanceType
        {
            [Description("��")]
            GoldCard = 0,
            [Description("�Ա�")]
            TaoBao = 1,
            [Description("����")]
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

        //InvoiceType������Ʊ���� 
        #region InvoiceType
        public enum InvoiceType
        {
            [Description("��ֵ˰ר�÷�Ʊ")]
            SpecialVATInvoice = 1,
            [Description("��ֵ˰��ͨ��Ʊ")]
            GeneralVATInvoice = 2,
            [Description("��ҵ���۷�Ʊ")]
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

        //SOInvoiceStatus ������Ʊ״̬
        #region SOInvoiceStatus
        public enum SOInvoiceStatus
        {
            [Description("��Ʊ�ѿ�")]
            InvoiceComplete = 1,
            [Description("��Ʊδ��")]
            InvoiceAbsent = 2,
            [Description("��Ʊ����")]
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

        //SOAuditTimeSpan��Ա����ʱ��
        #region SOAuditTimeSpan
        public enum SOAuditTimeSpan
        {
            [Description("�װ�")]
            Day = 1,
            [Description("���")]
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

        //WorkTimeSpan����ʱ�� 
        #region WorkTimeSpan
        public enum WorkTimeSpan
        {
            [Description("�װ�")]
            Day = 1,
            [Description("���")]
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

        //�˻���״̬
        #region SRStatus
        public enum SRStatus : int
        {
            [Description("����")]
            Abandon = 0,
            [Description("��ʼ")]
            Origin = 1,
            [Description("�����")]
            WaitingInStock = 2,
            [Description("���ϼ�")]
            WaitingShelve = 3,
            [Description("���ϼ�")]
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

        //�˻�����
        #region SRReturnType
        public enum SRReturnType : int
        {
            [Description("�����˻�")]
            PartlyReturn = 0,
            [Description("ȫ���˻�")]
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

        //��ֵ״̬��ͼƬ��ʾ
        //��Ǽ�ʹ���� user, role, category1/2/3, categoryattribute, manufacturer��
        #region BiStatusImage 0, -1
        //===========================================
        public enum BiStatusImage : int
        {
            [Description("<img src='../images/opt/valid.gif' border='0' alt='��Ч' />")]
            Valid = 0,
            [Description("<img src='../images/opt/invalid.gif' border='0' alt='��Ч'/>")]
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
        /// �ͻ���Ʊ��Ϣ״̬
        /// </summary>
        #region CustomerVATInfoStatus
        public enum CustomerVATInfoStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("��ʼ")]
            Origin = 0,
            [Description("�����")]
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

        //RMAHandoverStatus ����
        public enum RMAHandoverStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("������")]
            WaitingHandover = 0,
            [Description("�ѳ���")]
            OutStock = 1,
            [Description("���ֽ���")]
            PartlyReceived = 2,
            [Description("ȫ������")]
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
        /// PM�������飨�������ã�
        /// </summary>
        public enum PMGroup
        {
            [Description("��δ����")]
            None = 0,
            [Description("�������")]
            CoreFittings = 1,
            [Description("����")]
            OuterFittings = 2,
            [Description("���ѵ���")]
            ConsumeElectron = 3,
            [Description("�ʼǱ�")]
            NoteBook = 4,
            [Description("Ь��")]
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
        /// ��λ��ʶ
        /// </summary>
        public enum UserFlag
        {
            [Description("��ͨ")]
            Normal = 0,
            [Description("�ɹ�Ա")]
            PM = 1,
            [Description("�ɹ��鳤")]
            TL = 2,
            [Description("�ɹ��ܼ�")]
            PMD = 3,
            [Description("����Ա")]
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
        /// �Ż݄�����
        /// </summary>
        public enum CouponType
        {
            [Description("�����ֿۄ�")]
            ALL = 1,
            [Description("���ֿۄ�")]
            Category = 2,
            [Description("��Ʒ�ֿۄ�")]
            Product = 3,
            [Description("Ʒ�Ƶֿۄ�")]
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
        /// �Ż݄�״̬
        /// </summary>
        public enum CouponStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("��ʼ")]
            Origin = 0,
            [Description("�Ѽ���")]
            Activation = 1,
            [Description("�Ѳ���ʹ��")]
            PartlyUsed = 2,
            [Description("��ȫ��ʹ��")]
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
        /// �Ż݄������¼״̬
        /// </summary>
        public enum CouponRequestStatus
        {
            [Description("����")]
            Abandon = -1,
            [Description("��ʼ")]
            Origin = 0,
            [Description("���")]
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